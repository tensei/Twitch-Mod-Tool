using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace Twitch_Mod_Tool.Models
{
    public class TwitchMessage: INotifyPropertyChanged
    {
        private readonly ChatMessage _chatMessage;

        public TwitchMessage(ChatMessage chatMessage)
        {
            _chatMessage = chatMessage;
            Filters = new List<string>();
            CoughtWords = new List<string>();
        }

        public string Channel => _chatMessage.Channel;
        public string Content => _chatMessage.Message;
        public string Author => _chatMessage.Username;
        public string Created => $"{DateTime.Now:g}";
        public List<string> Filters { get; private set; }
        public string CoughtBy => Filters.Count > 0 ?  $"Cought by Filters: {string.Join(", ", Filters)}": string.Empty;
        public List<string> CoughtWords { get; set; }
        public string CoughtWordsText => CoughtWords.Count > 0 ? $"Cought words: {string.Join(", ", CoughtWords)}": string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
