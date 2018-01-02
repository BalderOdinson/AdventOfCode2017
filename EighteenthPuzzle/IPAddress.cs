using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EighteenthPuzzle
{
    public class IPAddress
    {
        private readonly byte _firstOctet;
        private readonly byte _secondOctet;
        private readonly byte _thirdOctet;
        private readonly byte _fourthOctet;

        public int SubnetPrefix { get; }

        public IPAddress(string subnet, int id)
        {
            var binarySubnet = string.Empty;
            var subnetParted = Regex.Split(subnet, "/|\\.");
            for (int i = 0; i < subnetParted.Length; i++)
            {
                if (i == subnetParted.Length - 1)
                    SubnetPrefix = int.Parse(subnetParted[i]);
                else
                {
                    binarySubnet += Convert.ToString(Convert.ToByte(subnetParted[i]), 2).PadLeft(8, '0');
                }
            }

            var ipaddress = binarySubnet.Substring(0, SubnetPrefix) +
                            Convert.ToString(Convert.ToByte(id), 2).PadLeft(32 - SubnetPrefix, '0');
            _firstOctet = Convert.ToByte(ipaddress.Substring(0, 8), 2);
            _secondOctet = Convert.ToByte(ipaddress.Substring(8, 8), 2);
            _thirdOctet = Convert.ToByte(ipaddress.Substring(16, 8), 2);
            _fourthOctet = Convert.ToByte(ipaddress.Substring(24, 8), 2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is IPAddress ipAddress))
                return false;
            return ipAddress._firstOctet.Equals(_firstOctet) &&
                   ipAddress._secondOctet.Equals(_secondOctet) &&
                   ipAddress._thirdOctet.Equals(_thirdOctet) &&
                   ipAddress._fourthOctet.Equals(_fourthOctet);
        }

        public override string ToString()
        {
            return Convert.ToString(_firstOctet) + "." +
                   Convert.ToString(_secondOctet) + "." +
                   Convert.ToString(_thirdOctet) + "." +
                   Convert.ToString(_fourthOctet) + "/" +
                   Convert.ToString(SubnetPrefix);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
