using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch_Mod_Tool.Services;

namespace Twitch_Mod_Tool.ViewModels
{
    public class CommandsViewModel: INotifyPropertyChanged
    {
        private readonly TwitchService _twitchService;

        public CommandsViewModel(TwitchService twitchService)
        {
            _twitchService = twitchService;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
