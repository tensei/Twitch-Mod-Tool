using System.Windows.Controls;

namespace Twitch_Mod_Tool.Views
{
    /// <summary>
    ///     Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl
    {
        private bool _autoScroll = true;

        public ChatView()
        {
            InitializeComponent();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer) sender;
            // User scroll event : set or unset auto-scroll mode
            if (e.ExtentHeightChange == 0)
            {
                // Content unchanged : user scroll event
                _autoScroll = scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight;
            }

            // Content scroll event : auto-scroll eventually
            if (_autoScroll && e.ExtentHeightChange != 0)
            {
                // Content changed and auto-scroll mode set
                // Autoscroll
                scrollViewer.ScrollToVerticalOffset(scrollViewer.ExtentHeight);
            }
        }
    }
}