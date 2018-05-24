using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using Twitch_Mod_Tool.Models;

namespace Twitch_Mod_Tool.Services
{
    public class TwitchService
    {
        private readonly TwitchSettings _twitchSettings;

        public TwitchService(TwitchSettings twitchSettings)
        {
            Client = new TwitchClient();
            _twitchSettings = twitchSettings;
        }

        public TwitchClient Client { get; set; }
        public void Start()
        {
            var credentials = new ConnectionCredentials(_twitchSettings.Username, _twitchSettings.Oauth);

            Client.Initialize(credentials);
            Client.OnMessageReceived += Client_OnMessageReceived;
            Client.OnJoinedChannel += Client_OnJoinedChannel;
            Client.OnConnected += Client_OnConnected;
            Client.Connect();
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Debug.WriteLine("Connected");
            _twitchSettings.Channels.ForEach(ch => Client.JoinChannel(ch));
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Debug.WriteLine($"Joined {e.Channel}");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            Debug.WriteLine($"{e.ChatMessage.Username}: {e.ChatMessage.Message}");
        }

        public void Stop()
        {
            Client.Disconnect();
        }
    }
}
