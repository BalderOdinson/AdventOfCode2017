using System.ComponentModel;

namespace TwentyThirdPuzzle
{
    public enum InstructionId
    {
        [Description("halt")]
        Halt = -1,
        [Description("init")]
        Init = 0,
        [Description("set")]
        Set,
        [Description("sub")]
        Sub,
        [Description("mul")]
        Mul,
        [Description("jnz")]
        Jnz
    }
}
