using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using Twitch_Mod_Tool.Models;
using Twitch_Mod_Tool.Services;
using TwitchLib.Client.Extensions;

namespace Twitch_Mod_Tool.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TwitchMessage> Messages { get; set; }
        public bool LimitMessages { get; set; } = true;
        private readonly TwitchService _twitchService;
        private readonly TwitchSettings _twitchSettings;
        private readonly int _messageLimit = 1000;
        private bool _dryRun = true;


        public ChatViewModel(TwitchService twitchService, TwitchSettings twitchSettings)
        {
            Messages = new ObservableCollection<TwitchMessage>();
            _twitchService = twitchService;
            _twitchSettings = twitchSettings;
            _twitchService.Client.OnMessageReceived += Client_OnMessageReceived;
        }

        private async void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (!CheckForBadWord(e.ChatMessage) && !CheckForBadWordRegex(e.ChatMessage))
            {
                return;
            }
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
             {
                 if (Messages.Count == _messageLimit)
                 {
                     Messages.RemoveAt(0);
                 }
                 Messages.Add(new TwitchMessage(e.ChatMessage));
             }));
        }

        private bool CheckForBadWord(ChatMessage chatMessage)
        {
            foreach (var word in _twitchSettings.BadWords)
            {
                if (!chatMessage.Message.Contains(word) && !chatMessage.Username.Contains(word))
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        private bool CheckForBadWordRegex(ChatMessage chatMessage)
        {
            foreach (var word in _twitchSettings.BadWordsRegex)
            {
                if (!Regex.IsMatch(chatMessage.Message, word, RegexOptions.IgnoreCase))
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        public ICommand ClearCommand => new ActionCommand(Messages.Clear);

        public ICommand TenMinTimeoutCommand =>
            new ActionCommand(tm => Timeout((TwitchMessage) tm, TimeSpan.FromMinutes(10)));
        public ICommand OneHourTimeoutCommand =>
            new ActionCommand(tm => Timeout((TwitchMessage)tm, TimeSpan.FromHours(1)));
        public ICommand OneDayTimeoutCommand =>
            new ActionCommand(tm => Timeout((TwitchMessage)tm, TimeSpan.FromDays(1)));
        public ICommand PermaCommand =>
            new ActionCommand(tm => Perma((TwitchMessage)tm));
        public ICommand UnbanCommand =>
            new ActionCommand(tm => Unban((TwitchMessage)tm));
        public ICommand LogsCommand =>
            new ActionCommand(tm => Logs((TwitchMessage)tm));

        private void Timeout(TwitchMessage twitchMessage, TimeSpan time)
        {
            _twitchService.Client.TimeoutUser(twitchMessage.Author, time, dryRun: _dryRun);
        }
        private void Perma(TwitchMessage twitchMessage)
        {
            _twitchService.Client.BanUser(twitchMessage.Author, dryRun: _dryRun);
        }
        private void Unban(TwitchMessage twitchMessage)
        {
            _twitchService.Client.UnbanUser(twitchMessage.Author, _dryRun);
        }
        private void Logs(TwitchMessage twitchMessage)
        {
            // TODO
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
