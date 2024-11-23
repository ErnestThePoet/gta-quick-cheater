using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GTAQuickCheater.Managers
{
    internal class AppManager
    {
        public static void ErrorExit(string message)
        {
            MessageBox.Show(message,
                    ResManager.FindString("ErrorTitle"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            Application.Current.Shutdown();
        }
    }
}
