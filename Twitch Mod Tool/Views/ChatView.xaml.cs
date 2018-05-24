using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Twitch_Mod_Tool.ViewModels;

namespace Twitch_Mod_Tool.Views
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl
    {
        public ChatView()
        {
            InitializeComponent();
        }
        private bool _autoScroll = true;

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            // User scroll event : set or unset auto-scroll mode
            if (e.ExtentHeightChange == 0)
            {
                // Content unchanged : user scroll event
                _autoScroll = scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight;
            }

            // Content scroll event : auto-scroll eventually
            if (_autoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and auto-scroll mode set
                // Autoscroll
                scrollViewer.ScrollToVerticalOffset(scrollViewer.ExtentHeight);
            }
        }
    }
}
