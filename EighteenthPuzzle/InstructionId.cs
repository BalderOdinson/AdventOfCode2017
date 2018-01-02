using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EighteenthPuzzle
{
    public enum InstructionId
    {
        [Description("halt")]
        Halt = -1,
        [Description("init")]
        Init = 0,
        [Description("snd")]
        Snd,
        [Description("set")]
        Set,
        [Description("add")]
        Add,
        [Description("mul")]
        Mul,
        [Description("mod")]
        Mod,
        [Description("rcv")]
        Rcv,
        [Description("jgz")]
        Jgz
    }
}
