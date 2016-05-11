using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReflectionStudio.Common
{
    public struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public long time;
        public uint dwExtraInfo;
    }
}
