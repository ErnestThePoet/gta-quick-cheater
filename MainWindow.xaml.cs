using GTAQuickCheater.Managers;
using GTAQuickCheater.OSInterop;
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
        private CheatManager cheatManager;

        public MainWindow()
        {
            InitializeComponent();
            ResManager.SetContext(this);

            configManager = new ConfigManager("cheater_config.json");
            cheatManager = new CheatManager();

            CreateCheatSetsUI();
        }

        private void CreateCheatSetsUI()
        {
            cbGtaVersion.Items.Clear();
            var cheatSets = configManager.GetCheatSets();
            foreach (var cheatSet in cheatSets)
            {
                cbGtaVersion.Items.Add(cheatSet.name);
            }

            if(cheatSets.Count > 0)
            {
                cbGtaVersion.SelectedIndex = 0;
            }
        }

        private void cbGtaVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lvCheatList.Items.Clear();
            foreach(var cheat in configManager.GetCheatSets()[cbGtaVersion.SelectedIndex].cheats)
            {
                lvCheatList.Items.Add(cheat);
            }

            cheatManager.SetCheatItems(configManager.GetCheatSets()[cbGtaVersion.SelectedIndex].cheats);
        }
    }
}