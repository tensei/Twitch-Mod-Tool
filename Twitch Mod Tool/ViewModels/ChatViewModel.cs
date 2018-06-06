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
using Microsoft.EntityFrameworkCore;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using Twitch_Mod_Tool.Models;
using Twitch_Mod_Tool.Services;
using TwitchLib.Client.Extensions;
using Twitch_Mod_Tool.Interfaces;
using Twitch_Mod_Tool.Windows;

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
        private readonly ToolContext _context;
        private readonly OverrustlelogsService _overrustlelogsService;
        private readonly int _messageLimit = 1000;
#if DEBUG
        private bool _dryRun = true;
#else
        private bool _dryRun = false;
#endif
        

        public ChatViewModel(TwitchService twitchService,
            TwitchSettings twitchSettings,
            IPhoneticMatch phonetic,
            ToolContext context,
            OverrustlelogsService overrustlelogsService)
        {
            Messages = new ObservableCollection<TwitchMessage>();
            _twitchService = twitchService;
            TwitchSettings = twitchSettings;
            _phonetic = phonetic;
            _context = context;
            _overrustlelogsService = overrustlelogsService;
            _twitchService.Client.OnMessageReceived += Client_OnMessageReceived;
        }

        private async void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            ReceivedMessages++;
            // check if user is whitelisted
            if (await _context.WhitelistUsers.AnyAsync(w => w.TwitchId.Equals(e.ChatMessage.UserId)))
            {
                return;
            }

            var whitelistedWords = await _context.WhitelistWords.ToListAsync();
            var hasBadWord = false;
            var tm = new TwitchMessage(e.ChatMessage);
            if (TwitchSettings.BadWordFilter && CheckForBadWord(tm, whitelistedWords))
            {
                tm.Filters.Add("Word");
                hasBadWord = true;
            }
            if (TwitchSettings.BadWordRegexFilter && CheckForBadWordRegex(tm, whitelistedWords))
            {
                tm.Filters.Add("Regex");
                hasBadWord = true;
            }
            if (TwitchSettings.BadWordPhoneticFilter && CheckForBadWordPhonetic(tm, whitelistedWords))
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

        private bool CheckForBadWord(TwitchMessage twitchMessage, IReadOnlyCollection<WhitelistWord> whitelistWords)
        {
            foreach (var word in TwitchSettings.BadWords)
            {
                // skip whitelisted words 
                if (whitelistWords.Any(w => w.Word.Equals(word, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }
                if (!twitchMessage.Content.Contains(word) && !twitchMessage.Author.Contains(word))
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        private bool CheckForBadWordRegex(TwitchMessage twitchMessage, IReadOnlyCollection<WhitelistWord> whitelistWords)
        {
            var words = twitchMessage.Content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                // skip whitelisted words 
                if (whitelistWords.Any(w => w.Word.Equals(word, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }
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
        private bool CheckForBadWordPhonetic(TwitchMessage twitchMessage, IReadOnlyCollection<WhitelistWord> whitelistWords)
        {
            var words = twitchMessage.Content.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                // skip whitelisted words 
                if (whitelistWords.Any(w => w.Word.Equals(word, StringComparison.OrdinalIgnoreCase)) ||
                    word.Length < 3)
                {
                    continue;
                }
                var messageWordPhonetic = _phonetic.CreateToken(word);
                foreach (var badWord in TwitchSettings.BadWords)
                {
                    var badPhonetic = _phonetic.CreateToken(badWord);
                    if (messageWordPhonetic.StartsWith(badPhonetic, StringComparison.OrdinalIgnoreCase))
                    {
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
            new ActionCommand(async tm => await LogsAsync((TwitchMessage)tm));
        public ICommand WhitelistCommand =>
            new ActionCommand(async tm => await Whitelist((TwitchMessage)tm));

        private async Task Whitelist(TwitchMessage twitchMessage)
        {
            var wluser = new WhitelistUser
            {
                TwitchId = twitchMessage.ChatMessage.UserId,
                Name = twitchMessage.Author,
                Channel = twitchMessage.Channel
            };
            if (await _context.WhitelistUsers.AnyAsync(u => u.Channel.Equals(twitchMessage.Channel)
                                                 && u.TwitchId.Equals(twitchMessage.ChatMessage.UserId)))
            {
                return;
            }

            await _context.WhitelistUsers.AddAsync(wluser);
            await _context.SaveChangesAsync();
        }

        private void Timeout(TwitchMessage twitchMessage, TimeSpan time)
        {
            _twitchService.Client.TimeoutUser(twitchMessage.Channel, twitchMessage.Author, time, dryRun: _dryRun);
        }
        private void Perma(TwitchMessage twitchMessage)
        {
            _twitchService.Client.BanUser(twitchMessage.Channel, viewer: twitchMessage.Author, dryRun: _dryRun);
        }
        private void Unban(TwitchMessage twitchMessage)
        {
            _twitchService.Client.UnbanUser(twitchMessage.Channel, twitchMessage.Author, _dryRun);
        }
        private async Task LogsAsync(TwitchMessage twitchMessage)
        {
            try
            {
                var logWindow = new LogWindow
                {
                    Log = {Text = await _overrustlelogsService.GetUserlogs(twitchMessage.Author, twitchMessage.Channel)},
                    Title = $"Logs User: {twitchMessage.Author} Channel: {twitchMessage.Channel}"
                };
                logWindow.Show();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
