using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenthPuzzle
{
    public static class Extensions
    {
        public static InstructionId GetInstructionId(this string description)
        {
            foreach (var enumValue in typeof(InstructionId).GetEnumValues())
            {
                if (((InstructionId)enumValue).GetDescription() == description)
                    return (InstructionId)enumValue;
            }
            return default(InstructionId);
        }

        public static string GetDescription(this InstructionId instructionId)
        {
            var fi = instructionId.GetType().GetField(instructionId.ToString());
            return fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Length > 0 ? attributes[0].Description : null;
        }
    }
}
