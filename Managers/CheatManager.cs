using GTAQuickCheater.Entities;
using GTAQuickCheater.OSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GTAQuickCheater.Managers
{
    internal class CheatManager
    {
        private KeyboardListener keyboardListener;
        private KeyboardSender keyboardSender;
        private List<CheatItem> cheatItems;

        public CheatManager()
        {
            keyboardListener = new KeyboardListener();
            keyboardSender = new KeyboardSender();
            cheatItems = new List<CheatItem>();

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
            this.cheatItems = cheatItems;
        }

        private void OnKeyDown(object? sender, KeyPressedArgs args)
        {
            MessageBox.Show(args.KeyPressed.ToString());
        }

        private void OnKeyUp(object? sender, KeyPressedArgs args)
        {

        }
    }
}
