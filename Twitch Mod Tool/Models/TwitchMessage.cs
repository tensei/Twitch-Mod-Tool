using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace Twitch_Mod_Tool.Models
{
    public class TwitchMessage
    {
        private readonly ChatMessage _chatMessage;

        public TwitchMessage(ChatMessage chatMessage)
        {
            _chatMessage = chatMessage;
        }

        public string Channel => _chatMessage.Channel;
        public string Content => _chatMessage.Message;
        public string Author => _chatMessage.Username;
        public string Created => $"{DateTime.Now:g}";
    }
}
