using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EighteenthPuzzle
{
    public interface IConnection
    {
        IPAddress IpAddress { get; }
        MACAddress MacAddress { get; }
        Task Send(IPDatagram data);
        bool IsIdle { get; }
        void RequestShutdown();
        event EventHandler IdleChangedEventHandler;
    }
}
