using System;
using System.Runtime.InteropServices;

namespace ReflectionStudio.Common
{
    public class NativeWin32
    {
        public const uint INPUT_KEYBOARD = 1u;
        public const uint KEYEVENTF_EXTENDEDKEY = 1u;
        public const uint KEYEVENTF_KEYUP = 2u;
        public const int HWND_BROADCAST = 65535;
        public static readonly int WM_SHOWSAMBAPOS = NativeWin32.RegisterWindowMessage("WM_SHOWSAMBAPOS");

        [DllImport("user32.dll")]
        public static extern bool Keybd_Event(int dwKey, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        [DllImport("User32.DLL")]
        public static extern uint SendInput(uint nInputs, InputKeys[] inputs, int cbSize);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
    }
}
