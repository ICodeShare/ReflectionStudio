using System.Runtime.InteropServices;

namespace ReflectionStudio.Common
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct InputKeys
    {
        public uint type;
        public uint wVk;
        public uint wScan;
        public uint dwFlags;
        public uint time;
        public uint dwExtra;
    }
}
