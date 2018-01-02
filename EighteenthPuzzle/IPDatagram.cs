using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenthPuzzle
{
    public struct IPDatagram
    {
        public IPDatagram(IPAddress sender, IPAddress reciever, object data, Type dataType, long packetId)
        {
            Sender = sender;
            Reciever = reciever;
            Data = data;
            PacketId = packetId;
            DataType = dataType;
        }

        public IPAddress Sender { get; }
        public IPAddress Reciever { get; }
        public object Data { get; }
        public Type DataType { get; }
        public long PacketId { get; }
    }
}
