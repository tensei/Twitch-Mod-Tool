using System.Collections.Generic;
using System.ComponentModel;

namespace Twitch_Mod_Tool.Models
{
    public class TwitchSettings : INotifyPropertyChanged
    {
        public string Username { get; set; } = "";
        public string Oauth { get; set; } = "";
        public List<string> Channels { get; set; } = new List<string>();
        public bool BadWordFilter { get; set; }
        public bool BadWordRegexFilter { get; set; }
        public bool BadWordPhoneticFilter { get; set; }
        public List<string> BadWords { get; set; } = new List<string>();
        public List<string> BadWordsRegex { get; set; } = new List<string>();
        public event PropertyChangedEventHandler PropertyChanged;
    }
}