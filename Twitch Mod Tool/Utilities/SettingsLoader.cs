using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using Twitch_Mod_Tool.Models;

namespace Twitch_Mod_Tool.Utilities
{
    public class SettingsLoader
    {
        private readonly string _settingsFile = "settings.json";
        private readonly TwitchSettings _twitchSettings;

        public SettingsLoader(TwitchSettings twitchSettings)
        {
            _twitchSettings = twitchSettings;
        }

        private void _twitchSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var s = JsonConvert.SerializeObject(_twitchSettings, Formatting.Indented);
            File.WriteAllText(_settingsFile, s);
        }

        public void Load()
        {
            if (!File.Exists(_settingsFile))
            {
                var s = JsonConvert.SerializeObject(_twitchSettings, Formatting.Indented);
                File.WriteAllText(_settingsFile, s);
                return;
            }

            var f = File.ReadAllText(_settingsFile);
            var ts = JsonConvert.DeserializeObject<TwitchSettings>(f);
            // workaround 
            _twitchSettings.Username = ts.Username;
            _twitchSettings.Oauth = ts.Oauth;
            _twitchSettings.Channels = ts.Channels;
            _twitchSettings.BadWords = ts.BadWords;
            _twitchSettings.BadWordsRegex = ts.BadWordsRegex;
            _twitchSettings.BadWordFilter = ts.BadWordFilter;
            _twitchSettings.BadWordRegexFilter = ts.BadWordRegexFilter;
            _twitchSettings.BadWordPhoneticFilter = ts.BadWordPhoneticFilter;

            _twitchSettings.PropertyChanged += _twitchSettings_PropertyChanged;
        }
    }
}