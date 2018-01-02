using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace EighteenthPuzzle
{
    public class NetworkCard : IConnection
    {
        public NetworkCard(int id, IConnectionProvider connectionProvider)
        {
            _id = id;
            _connectionProvider = connectionProvider;
            _messageQueue = new ConcurrentQueue<long>();
            PackegesSent = 0;
            PackegesRecieved = 0;
            MacAddress = new MACAddress();
            _tokenSource = new CancellationTokenSource();
            _cancellationToken = _tokenSource.Token;
        }

        private readonly CancellationToken _cancellationToken;

        private readonly CancellationTokenSource _tokenSource;

        private readonly IConnectionProvider _connectionProvider;

        private readonly ConcurrentQueue<long> _messageQueue;

        private readonly int _id;

        public long LastMessage { get; private set; }

        public long PackegesSent { get; private set; }

        public long PackegesRecieved { get; private set; }

        public async Task SendTo(long msg, TargetConnectionOption option, IPAddress reciverIpAddress = null)
        {
            await Task.Run(async () =>
            {
                switch (option)
                {
                    case TargetConnectionOption.LocalSelf:
                        LastMessage = msg;
                        break;
                    case TargetConnectionOption.LocalBroadcast:
                        var list = _connectionProvider.LocalNetworkDictionary.ToList();
                        foreach (var ipAddress in list)
                        {
                            if (Equals(ipAddress.Key, MacAddress)) continue;
                            var datagram = new IPDatagram(IpAddress, ipAddress.Value.IpAddress, msg, msg.GetType(), PackegesSent++);
                            await _connectionProvider.Send(datagram);
                        }
                        break;
                    case TargetConnectionOption.Ethernet:
                        if (reciverIpAddress != null)
                        {
                            var datagram = new IPDatagram(IpAddress, reciverIpAddress, msg, msg.GetType(), PackegesSent++);
                            await _connectionProvider.Send(datagram);
                        }
                        break;
                }
            });
        }

        public async Task<long> Recieve()
        {
            return await Task.Run(() =>
            {
                if (_messageQueue.IsEmpty)
                {
                    IsIdle = true;
                    OnIdleChangedEventHandler();
                }
                long result;
                while (!_messageQueue.TryDequeue(out result) && !_cancellationToken.IsCancellationRequested)
                {

                }
                _cancellationToken.ThrowIfCancellationRequested();
                IsIdle = false;
                PackegesRecieved++;
                return result;
            }, _cancellationToken);
        }

        public IPAddress IpAddress { get; private set; }
        public MACAddress MacAddress { get; }

        public async Task Connect()
        {
            IpAddress = await _connectionProvider.Connect(this, _id);
            if (IpAddress == null)
                throw new AuthenticationException("Couldn't connect to router!");
        }

        public async Task Send(IPDatagram data)
        {
            await Task.Run(() =>
            {
                if (data.Reciever.Equals(IpAddress) && data.DataType == typeof(long))
                    _messageQueue.Enqueue((long)data.Data);
            });
        }

        public bool IsIdle { get; private set; }
        public void RequestShutdown()
        {
            _tokenSource.Cancel();
        }

        public event EventHandler IdleChangedEventHandler;

        protected virtual void OnIdleChangedEventHandler()
        {
            IdleChangedEventHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}
