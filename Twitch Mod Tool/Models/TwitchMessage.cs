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
        public readonly ChatMessage ChatMessage;

        public TwitchMessage(ChatMessage chatMessage)
        {
            ChatMessage = chatMessage;
            Filters = new List<string>();
            CoughtWords = new List<string>();
        }

        public string Channel => ChatMessage.Channel;
        public string Content => ChatMessage.Message;
        public string Author => ChatMessage.Username;
        public string Created => $"{DateTime.Now:g}";
        public List<string> Filters { get; }
        public string CoughtBy => Filters.Count > 0 ?  $"Filters: {string.Join(", ", Filters)}": string.Empty;
        public List<string> CoughtWords { get; }
        public string CoughtWordsText => CoughtWords.Count > 0 ? $"Words: {string.Join(", ", CoughtWords)}": string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
