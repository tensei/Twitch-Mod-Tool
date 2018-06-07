using System.ComponentModel;
using Twitch_Mod_Tool.Services;

namespace Twitch_Mod_Tool.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly TwitchService _twitchService;

        public MainWindowViewModel(TwitchService twitchService,
            ChatViewModel chatViewModel,
            CommandsViewModel commandsViewModel)
        {
            ChatViewModel = chatViewModel;
            CommandsViewModel = commandsViewModel;
            _twitchService = twitchService;
            _twitchService.Start();
        }

        public ChatViewModel ChatViewModel { get; set; }
        public CommandsViewModel CommandsViewModel { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}