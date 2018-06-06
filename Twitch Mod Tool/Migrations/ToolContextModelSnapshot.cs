﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Twitch_Mod_Tool;

namespace Twitch_Mod_Tool.Migrations
{
    [DbContext(typeof(ToolContext))]
    partial class ToolContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799");

            modelBuilder.Entity("Twitch_Mod_Tool.Models.BannedUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Channel");

                    b.Property<string>("Name");

                    b.Property<string>("Reason");

                    b.Property<DateTime>("Time");

                    b.Property<string>("TwitchId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("BannedUsers");
                });

            modelBuilder.Entity("Twitch_Mod_Tool.Models.CustomCommand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Channel")
                        .IsRequired();

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("CustomCommands");
                });

            modelBuilder.Entity("Twitch_Mod_Tool.Models.WhitelistUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Channel");

                    b.Property<string>("Name");

                    b.Property<string>("TwitchId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("WhitelistUsers");
                });

            modelBuilder.Entity("Twitch_Mod_Tool.Models.WhitelistWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Word")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("WhitelistWords");
                });
#pragma warning restore 612, 618
        }
    }
}