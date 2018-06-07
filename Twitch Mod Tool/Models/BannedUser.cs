using System;

namespace Twitch_Mod_Tool.Models
{
    public class BannedUser
    {
        public int Id { get; set; }
        public string TwitchId { get; set; }
        public string Name { get; set; }
        public string Channel { get; set; }
        public DateTime Time { get; set; }
        public string Reason { get; set; }
    }
}