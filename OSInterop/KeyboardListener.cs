using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GTAQuickCheater.OSInterop
{
    internal class KeyboardListener
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private delegate IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private KeyboardProc proc;

        public event EventHandler<KeyPressedArgs>? OnKeyDown;
        public event EventHandler<KeyPressedArgs>? OnKeyUp;

        private IntPtr hookID = IntPtr.Zero;

        public KeyboardListener()
        {
            proc = HookCallback;
        }

        public void HookKeyboard()
        {
            hookID = SetHook(proc);
        }

        public void UnHookKeyboard()
        {
            UnhookWindowsHookEx(hookID);
        }

        private IntPtr SetHook(KeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            int vkCode = Marshal.ReadInt32(lParam);

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                if (OnKeyDown != null)
                {
                    OnKeyDown(this, new KeyPressedArgs(KeyInterop.KeyFromVirtualKey(vkCode)));
                }
            }

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
            {
                if (OnKeyUp != null)
                {
                    OnKeyUp(this, new KeyPressedArgs(KeyInterop.KeyFromVirtualKey(vkCode)));
                }
            }

            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }
    }

    public class KeyPressedArgs : EventArgs
    {
        public Key KeyPressed { get; private set; }

        public KeyPressedArgs(Key key)
        {
            KeyPressed = key;
        }
    }
}
