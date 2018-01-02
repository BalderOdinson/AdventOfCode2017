using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EighteenthPuzzle
{
    public class Router : IConnectionProvider, IDisposable
    {
        public IPAddress IpAddress { get; }
        public MACAddress MacAddress { get; }
        private readonly ConcurrentDictionary<IPAddress, MACAddress> _routingDictionary;

        private readonly CancellationTokenSource _cancellationToken;
        private readonly Task _dispatcher;

        public IDictionary<MACAddress, IConnection> LocalNetworkDictionary => _localNetworkDictionary;
        private readonly ConcurrentDictionary<MACAddress, IConnection> _localNetworkDictionary;

        public IDictionary<IPAddress, IConnectionProvider> EthernetDictionary => _ethernetDictionary;

        private readonly ConcurrentDictionary<IPAddress, IConnectionProvider> _ethernetDictionary;

        private readonly ConcurrentQueue<IPDatagram> _messageQueue;

        public Router(int subnetRange)
        {
            var randomizer = new Random();
            var subnet = randomizer.Next(10, 200) + "." +
                         randomizer.Next(10, 200) + "." +
                         randomizer.Next(10, 200) + "." +
                         randomizer.Next(10, 200) + "/" +
                         subnetRange;
            IpAddress = new IPAddress(subnet, randomizer.Next(20, 255));
            MacAddress = new MACAddress();
            _localNetworkDictionary = new ConcurrentDictionary<MACAddress, IConnection>();
            _ethernetDictionary = new ConcurrentDictionary<IPAddress, IConnectionProvider>();
            _routingDictionary = new ConcurrentDictionary<IPAddress, MACAddress>();
            _messageQueue = new ConcurrentQueue<IPDatagram>();
            _cancellationToken = new CancellationTokenSource();
            _dispatcher = Worker(_cancellationToken.Token);
        }

        public Router()
        {
            var randomizer = new Random();
            var subnet = randomizer.Next(10, 200) + "." +
                         randomizer.Next(10, 200) + "." +
                         randomizer.Next(10, 200) + "." +
                         randomizer.Next(10, 200) + "/" +
                         24;
            IpAddress = new IPAddress(subnet, randomizer.Next(20, 255));
            MacAddress = new MACAddress();
            _localNetworkDictionary = new ConcurrentDictionary<MACAddress, IConnection>();
            _ethernetDictionary = new ConcurrentDictionary<IPAddress, IConnectionProvider>();
            _routingDictionary = new ConcurrentDictionary<IPAddress, MACAddress>();
            _messageQueue = new ConcurrentQueue<IPDatagram>();
            _cancellationToken = new CancellationTokenSource();
            _dispatcher = Worker(_cancellationToken.Token);
        }

        public async Task Send(IPDatagram data)
        {
            await Task.Run(() =>
            {
                _messageQueue.Enqueue(data);
            });
        }

        public async Task Worker(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (_messageQueue.IsEmpty)
                        continue;
                    if (!_messageQueue.TryDequeue(out var datagram))
                    {
                        continue;
                    }

                    if (_routingDictionary.TryGetValue(datagram.Reciever, out var macAddress))
                    {
                        if (_localNetworkDictionary.TryGetValue(macAddress, out var connection))
                        {
                            await connection.Send(datagram);
                        }

                        continue;
                    }

                    if (_ethernetDictionary.TryGetValue(datagram.Reciever, out var connectionProvider))
                    {
                        await connectionProvider.Send(datagram);
                    }
                }
            }, token);
        }

        public async Task ShutDown()
        {
            _cancellationToken.Cancel();
            await _dispatcher;
        }

        public async Task<IPAddress> Connect(IConnection connection, int id)
        {
            return await Task.Run(() =>
            {
                if (_localNetworkDictionary.ContainsKey(connection.MacAddress))
                    return null;
                var ipAddress = new IPAddress(IpAddress.ToString(), id);
                _localNetworkDictionary.GetOrAdd(connection.MacAddress, connection);
                _routingDictionary.GetOrAdd(ipAddress, connection.MacAddress);
                connection.IdleChangedEventHandler += ConnectionOnIdleChangedEventHandler;
                return ipAddress;
            });
        }

        public async Task<IPAddress> Connect(IConnectionProvider connection, int id)
        {
            return await Task.Run(() =>
            {
                if (!_ethernetDictionary.ContainsKey(connection.IpAddress))
                {
                    _ethernetDictionary.GetOrAdd(connection.IpAddress, connection);
                    return IpAddress;
                }

                return null;
            });
        }

        private async void ConnectionOnIdleChangedEventHandler(object sender, EventArgs eventArgs)
        {
            if (!(sender is IConnection connection)) return;
            if (connection.IsIdle && _messageQueue.IsEmpty)
            {
                await Task.Run(() =>
                {
                    var connections = _localNetworkDictionary.ToList();
                    if (!connections.TrueForAll(pair => pair.Value.IsIdle)) return;
                    Thread.Sleep(10);
                    if (!connections.TrueForAll(pair => pair.Value.IsIdle)) return;
                    foreach (var localConnection in connections)
                    {
                        localConnection.Value.RequestShutdown();
                    }
                });
            }
        }


        public async Task Disconnect(MACAddress macAddress)
        {
            await Task.Run(() =>
            {
                IConnection connection;
                while (!_localNetworkDictionary.TryRemove(macAddress, out connection))
                {

                }

                while (!_routingDictionary.TryRemove(connection.IpAddress, out macAddress))
                {

                }

            });
        }

        public async void Dispose()
        {
            await ShutDown();
        }
    }
}
