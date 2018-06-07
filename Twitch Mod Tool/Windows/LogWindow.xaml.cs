using System.Windows;
using System.Windows.Controls;

namespace Twitch_Mod_Tool.Windows
{
    /// <summary>
    ///     Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow
    {
        public LogWindow()
        {
            InitializeComponent();
        }

        private void Log_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = (TextBox) sender;
            textbox.ScrollToEnd();
        }
    }
}