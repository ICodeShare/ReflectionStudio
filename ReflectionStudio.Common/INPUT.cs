using System.Runtime.InteropServices;

namespace ReflectionStudio.Common
{
    [StructLayout(LayoutKind.Explicit, Size = 28)]
    public struct INPUT
    {
        [FieldOffset(0)]
        public uint type;

        [FieldOffset(4)]
        public KEYBDINPUT ki;
    }
}
