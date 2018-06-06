using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Mod_Tool.Models
{
    public class CustomCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Channel { get; set; }
        public string Message { get; set; }
    }
}
