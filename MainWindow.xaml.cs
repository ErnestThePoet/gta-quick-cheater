using GTAQuickCheater.Managers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GTAQuickCheater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConfigManager configManager;

        public MainWindow()
        {
            InitializeComponent();
            ResManager.SetContext(this);
            configManager = new ConfigManager("cheater_config.json");
        }
    }
}