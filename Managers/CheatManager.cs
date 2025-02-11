using GTAQuickCheater.Entities;
using GTAQuickCheater.OSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GTAQuickCheater.Managers
{
    internal class CheatManager
    {
        private KeyboardListener keyboardListener;
        private KeyboardSender keyboardSender;

        private Dictionary<char, string> tabCheatCodeTable;

        private bool isTabPressed = false;

        public CheatManager()
        {
            keyboardListener = new KeyboardListener();
            keyboardSender = new KeyboardSender();
            tabCheatCodeTable = new Dictionary<char, string>();

            keyboardListener.OnKeyDown += OnKeyDown;
            keyboardListener.OnKeyUp += OnKeyUp;

            keyboardListener.HookKeyboard();
        }

        ~CheatManager()
        {
            keyboardListener.UnHookKeyboard();
        }

        public void SetCheatItems(List<CheatItem> cheatItems)
        {
            tabCheatCodeTable.Clear();
            foreach(var cheatItem in cheatItems)
            {
                var cheatKey = cheatItem.keys.Replace(" ", string.Empty).ToUpper();
                if (cheatKey.Length != 5 || !cheatKey.StartsWith("TAB+"))
                {
                    continue;
                }

                tabCheatCodeTable.Add(char.ToUpper(cheatItem.keys[4]), cheatItem.code);
            }
        }

        private void OnKeyDown(object? sender, KeyPressedArgs args)
        {
            if (args.KeyPressed == Key.Tab)
            {
                isTabPressed = true;
                return;
            }
            
            if (isTabPressed && tabCheatCodeTable.ContainsKey((char)KeyInterop.VirtualKeyFromKey(args.KeyPressed)))
            {
                var cheatCode = tabCheatCodeTable[(char)KeyInterop.VirtualKeyFromKey(args.KeyPressed)];
                foreach (var key in cheatCode)
                {
                    keyboardSender.SendKey(key);
                }
            }
        }

        private void OnKeyUp(object? sender, KeyPressedArgs args)
        {
            if (args.KeyPressed == Key.Tab)
            {
                isTabPressed = false;
                return;
            }
        }
    }
}
