using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EighteenthPuzzle
{
    public class MACAddress
    {
        private readonly byte _firstOctet;
        private readonly byte _secondOctet;
        private readonly byte _thirdOctet;
        private readonly byte _fourthOctet;
        private readonly byte _fifthOctet;
        private readonly byte _sixthOctet;

        public MACAddress(string macaddress)
        {
            var temp = Regex.Split(macaddress, ":").Select(m => Convert.ToByte(m, 16)).ToList();
            _firstOctet = temp[0];
            _secondOctet = temp[1];
            _thirdOctet = temp[2];
            _fourthOctet = temp[3];
            _fifthOctet = temp[4];
            _sixthOctet = temp[5];
        }

        public MACAddress()
        {
            var randomizer = new Random(Guid.NewGuid().GetHashCode());
            _firstOctet = (byte)randomizer.Next(1,255);
            _secondOctet = (byte)randomizer.Next(1, 255);
            _thirdOctet = (byte)randomizer.Next(1, 255);
            _fourthOctet = (byte)randomizer.Next(1, 255);
            _fifthOctet = (byte)randomizer.Next(1, 255);
            _sixthOctet = (byte)randomizer.Next(1, 255);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is MACAddress macadr))
                return false;
            return macadr._firstOctet.Equals(_firstOctet) &&
                macadr._secondOctet.Equals(_secondOctet) &&
                macadr._thirdOctet.Equals(_thirdOctet) &&
                macadr._fourthOctet.Equals(_fourthOctet) &&
                macadr._fifthOctet.Equals(_fifthOctet) &&
                macadr._sixthOctet.Equals(_sixthOctet);
        }

        public override string ToString()
        {
            return Convert.ToString(_firstOctet, 16).PadLeft(2,'0') + ":" +
                Convert.ToString(_secondOctet, 16).PadLeft(2, '0') + ":" +
                   Convert.ToString(_thirdOctet, 16).PadLeft(2, '0') + ":" +
                   Convert.ToString(_fourthOctet, 16).PadLeft(2, '0') + ":" +
                   Convert.ToString(_fifthOctet, 16).PadLeft(2, '0') + ":" +
                   Convert.ToString(_sixthOctet, 16).PadLeft(2, '0');
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
