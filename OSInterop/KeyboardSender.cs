using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GTAQuickCheater.OSInterop
{
    internal class KeyboardSender
    {
        [DllImport("user32.dll")]

        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);

        private const int KEYEVENTF_KEYUP = 0x0002;

        public void SendKey(byte virtualKeyCode)
        {
            keybd_event(virtualKeyCode, 0, 0, IntPtr.Zero);
            keybd_event(virtualKeyCode, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }

        public void SendKey(char key)
        {
            if (!Char.IsAsciiLetterOrDigit(key))
            {
                return;
            }

            key = Char.ToUpper(key);

            SendKey((byte)key);
        }
    }
}
