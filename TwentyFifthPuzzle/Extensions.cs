using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyFifthPuzzle
{
    public static class Extensions
    {
        public static MoveDirection GetMoveDirection(this string description)
        {
            foreach (var enumValue in typeof(MoveDirection).GetEnumValues())
            {
                if (((MoveDirection)enumValue).GetDescription() == description)
                    return (MoveDirection)enumValue;
            }
            return default(MoveDirection);
        }

        public static string GetDescription(this MoveDirection moveDirection)
        {
            var fi = moveDirection.GetType().GetField(moveDirection.ToString());
            return fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Length > 0 ? attributes[0].Description : null;
        }
    }
}
