using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Twitch_Mod_Tool.Models;

namespace Twitch_Mod_Tool
{
    public class ToolContext : DbContext
    {
        public DbSet<BannedUser> BannedUsers { get; set; }
        public DbSet<WhitelistWord> WhitelistWords { get; set; }
        public DbSet<WhitelistUser> WhitelistUsers { get; set; }
        public DbSet<CustomCommand> CustomCommands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=tool.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BannedUser>(e => {
                e.HasKey(x => x.Id);
                e.Property(x => x.TwitchId).IsRequired();
            });
            modelBuilder.Entity<WhitelistWord>(e => {
                e.HasKey(x => x.Id);
                e.Property(x => x.Word).IsRequired();
            });
            modelBuilder.Entity<WhitelistUser>(e => {
                e.HasKey(x => x.Id);
                e.Property(x => x.TwitchId).IsRequired();
            });
            modelBuilder.Entity<CustomCommand>(e => {
                e.HasKey(x => x.Id);
                e.Property(x => x.Channel).IsRequired();
                e.Property(x => x.Message).IsRequired();
                e.Property(x => x.Name).IsRequired();
            });
        }
    }
}
