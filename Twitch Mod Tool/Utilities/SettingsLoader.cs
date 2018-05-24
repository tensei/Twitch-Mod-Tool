using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Twitch_Mod_Tool.Models;

namespace Twitch_Mod_Tool.Utilities
{
    public class SettingsLoader
    {
        private TwitchSettings _twitchSettings;
        private readonly string _settingsFile = "settings.json";

        public SettingsLoader(TwitchSettings twitchSettings)
        {
            _twitchSettings = twitchSettings;
        }

        private void _twitchSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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

            _twitchSettings.PropertyChanged += _twitchSettings_PropertyChanged;
        }
    }
}
