using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GTAQuickCheater.Managers
{
    internal class ResManager
    {
        private static FrameworkElement? context;
        public static void SetContext(FrameworkElement context)
        {
            ResManager.context = context;
        }

        public static string FindString(string key)
        {
            return context?.FindResource(key).ToString() ?? string.Empty;
        }
    }
}
