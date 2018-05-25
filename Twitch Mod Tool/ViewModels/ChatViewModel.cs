using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        public int ReceivedMessages { get; set; }
        public bool LimitMessages { get; set; } = true;
        private readonly TwitchService _twitchService;
        public TwitchSettings TwitchSettings { get; }
        private readonly IPhoneticMatch _phonetic;
        private readonly int _messageLimit = 1000;
#if DEBUG
        private bool _dryRun = true;
#else
        private bool _dryRun = false;
#endif
        

        public ChatViewModel(TwitchService twitchService, TwitchSettings twitchSettings, IPhoneticMatch phonetic)
        {
            Messages = new ObservableCollection<TwitchMessage>();
            _twitchService = twitchService;
            TwitchSettings = twitchSettings;
            _phonetic = phonetic;
            _twitchService.Client.OnMessageReceived += Client_OnMessageReceived;
        }

        private async void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            ReceivedMessages++;
            var hasBadWord = false;
            var tm = new TwitchMessage(e.ChatMessage);
            if (TwitchSettings.BadWordFilter && CheckForBadWord(tm))
            {
                tm.Filters.Add("Word");
                hasBadWord = true;
            }
            if (TwitchSettings.BadWordRegexFilter && CheckForBadWordRegex(tm))
            {
                tm.Filters.Add("Regex");
                hasBadWord = true;
            }
            if (TwitchSettings.BadWordPhoneticFilter && CheckForBadWordPhonetic(tm))
            {
                tm.Filters.Add("Phonetic");
                hasBadWord = true;
            }

            if (!hasBadWord)
            {
                return;
            }
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
             {
                 if (Messages.Count == _messageLimit)
                 {
                     Messages.RemoveAt(0);
                 }
                 Messages.Add(tm);
             }));
        }

        private bool CheckForBadWord(TwitchMessage twitchMessage)
        {
            foreach (var word in TwitchSettings.BadWords)
            {
                if (!twitchMessage.Content.Contains(word) && !twitchMessage.Author.Contains(word))
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        private bool CheckForBadWordRegex(TwitchMessage twitchMessage)
        {
            var words = twitchMessage.Content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                foreach (var badword in TwitchSettings.BadWordsRegex)
                {
                    if (!Regex.IsMatch(word, badword, RegexOptions.IgnoreCase))
                    {
                        continue;
                    }

                    twitchMessage.CoughtWords.Add(word);
                    return true;
                }
            }
            return false;
        }
        private bool CheckForBadWordPhonetic(TwitchMessage twitchMessage)
        {
            var words = twitchMessage.Content.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in TwitchSettings.BadWords)
            {
                var badPhonetic = _phonetic.CreateToken(word);
                foreach (var s in words)
                {
                    if (s.Length < 3)
                    {
                        continue;
                    }
                    var messageWordPhonetic = _phonetic.CreateToken(s);
                    if (messageWordPhonetic == badPhonetic)
                    {
                        twitchMessage.CoughtWords.Add(s);
                        return true;
                    }
                }
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
