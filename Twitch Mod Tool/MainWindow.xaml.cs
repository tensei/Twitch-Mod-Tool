using Twitch_Mod_Tool.Services;
using Twitch_Mod_Tool.ViewModels;

namespace Twitch_Mod_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly TwitchService _twitchService;

        public MainWindow(MainWindowViewModel mainWindowViewModel,
            TwitchService twitchService)
        {
            _twitchService = twitchService;
            DataContext = mainWindowViewModel;
            Closing += MainWindow_Closing;
            InitializeComponent();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _twitchService.Stop();
        }
    }
}
