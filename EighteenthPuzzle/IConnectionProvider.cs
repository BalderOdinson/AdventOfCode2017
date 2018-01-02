using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenthPuzzle
{
    public interface IConnectionProvider
    {
        IPAddress IpAddress { get; }
        MACAddress MacAddress { get; }
        Task Send(IPDatagram data);
        Task<IPAddress> Connect(IConnection connection, int id);
        Task Disconnect(MACAddress macAddress);
        IDictionary<MACAddress, IConnection> LocalNetworkDictionary { get; }
        IDictionary<IPAddress, IConnectionProvider> EthernetDictionary { get; }
        Task ShutDown();
    }
}
