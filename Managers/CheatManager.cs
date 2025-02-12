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
        enum CheaterModifierKey
        {
            Tab = 0,
            Shift = 1,
            Ctrl = 2,
            Alt = 4
        }

        private KeyboardListener keyboardListener;
        private KeyboardSender keyboardSender;

        private Dictionary<CheaterModifierKey, Dictionary<char, string>> cheatCodeTable;

        private HashSet<CheaterModifierKey> pressedModifierKeys;

        public CheatManager()
        {
            keyboardListener = new KeyboardListener();
            keyboardSender = new KeyboardSender();
            cheatCodeTable = new Dictionary<CheaterModifierKey, Dictionary<char, string>>();
            pressedModifierKeys = new HashSet<CheaterModifierKey>();

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
            cheatCodeTable.Clear();
            foreach(var cheatItem in cheatItems)
            {
                var cheatKey = cheatItem.keys.Replace(" ", string.Empty).ToUpper();
                var splitResult = cheatKey.Split('+');

                if (splitResult.Length < 2 || string.IsNullOrWhiteSpace(splitResult[1]))
                {
                    continue;
                }

                CheaterModifierKey? modifierKey = null;

                switch (splitResult[0])
                {
                    case "TAB":
                        modifierKey = CheaterModifierKey.Tab;
                        break;
                    case "SHIFT":
                        modifierKey = CheaterModifierKey.Shift;
                        break;
                    case "CTRL":
                        modifierKey = CheaterModifierKey.Ctrl;
                        break;
                    case "ALT":
                        modifierKey = CheaterModifierKey.Alt;
                        break;
                }

                if (modifierKey == null)
                {
                    continue;
                }

                if (!cheatCodeTable.ContainsKey(modifierKey.Value))
                {
                    cheatCodeTable[modifierKey.Value] = new Dictionary<char, string>()
                    {
                        { splitResult[1][0], cheatItem.code }
                    };
                }
                else
                {
                    cheatCodeTable[modifierKey.Value][splitResult[1][0]] = cheatItem.code;
                }
            }
        }

        private CheaterModifierKey? GetCheatModifierKey(Key key)
        {
            switch (key)
            {
                case Key.Tab:
                    return CheaterModifierKey.Tab;
                case Key.LeftShift:
                case Key.RightShift:
                    return CheaterModifierKey.Shift;
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    return CheaterModifierKey.Ctrl;
                case Key.LeftAlt:
                case Key.RightAlt:
                    return CheaterModifierKey.Alt;
            }

            return null;
        }

        private void OnKeyDown(object? sender, KeyPressedArgs args)
        {
            var modifierKey = GetCheatModifierKey(args.KeyPressed);

            if (modifierKey != null)
            {
                pressedModifierKeys.Add(modifierKey.Value);
                return;
            }

            foreach(var pressedModifierKey in pressedModifierKeys)
            {
                if (cheatCodeTable.ContainsKey(pressedModifierKey) && 
                    cheatCodeTable[pressedModifierKey].ContainsKey((char)KeyInterop.VirtualKeyFromKey(args.KeyPressed)))
                {
                    var cheatCode = cheatCodeTable[pressedModifierKey][(char)KeyInterop.VirtualKeyFromKey(args.KeyPressed)];
                    foreach (var key in cheatCode)
                    {
                        keyboardSender.SendKey(key);
                    }
                }
            }
        }

        private void OnKeyUp(object? sender, KeyPressedArgs args)
        {
            var modifierKey = GetCheatModifierKey(args.KeyPressed);

            if (modifierKey != null)
            {
                pressedModifierKeys.Remove(modifierKey.Value);
                return;
            }
        }
    }
}
