﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Mod_Tool.Models
{
    public class WhitelistUser
    {
        public int Id { get; set; }
        public string TwitchId { get; set; }
        public string Name { get; set; }
        public string Channel { get; set; }

    }
}