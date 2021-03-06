﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Twitch_Mod_Tool.Dialogs;
using Twitch_Mod_Tool.Models;
using Twitch_Mod_Tool.Services;

namespace Twitch_Mod_Tool.ViewModels
{
    public class CommandsViewModel : INotifyPropertyChanged
    {
        private readonly ToolContext _context;
        private readonly TwitchService _twitchService;

        public CommandsViewModel(TwitchService twitchService, ToolContext context)
        {
            _twitchService = twitchService;
            _context = context;
            CustomCommands = new ObservableCollection<CustomCommand>(_context.CustomCommands.ToList());
        }

        public ObservableCollection<CustomCommand> CustomCommands { get; }
        public string WordToWhitelist { get; set; }

        public ICommand WhitelistWordCommand =>
            new ActionCommand(async () => await WhitelistWord());

        public ICommand SendCommand =>
            new ActionCommand(cmnd => SendCustomCommand((CustomCommand) cmnd));

        public ICommand AddNewCustomCommand =>
            new ActionCommand(async () => await AddNewCommand());

        public ICommand DeleteCustomCommand =>
            new ActionCommand(async cmnd => await DeleteCommand((CustomCommand) cmnd));

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task WhitelistWord()
        {
            WordToWhitelist = WordToWhitelist.Trim();
            if (string.IsNullOrWhiteSpace(WordToWhitelist) || WordToWhitelist.Contains(" "))
            {
                return;
            }

            var wluser = new WhitelistWord
            {
                Word = WordToWhitelist
            };
            if (await _context.WhitelistWords.AnyAsync(u => u.Word.Equals(WordToWhitelist)))
            {
                return;
            }

            await _context.WhitelistWords.AddAsync(wluser);
            await _context.SaveChangesAsync();
            WordToWhitelist = string.Empty;
        }

        private void SendCustomCommand(CustomCommand customCommand)
        {
            _twitchService.Client.SendMessage(customCommand.Channel, customCommand.Message);
        }

        private async Task AddNewCommand()
        {
            var cmnd = new CustomCommand();
            var dialog = new CustomCommandAddDialog
            {
                DataContext = cmnd
            };
            var result = (bool) await DialogHost.Show(dialog);
            if (result &&
                !string.IsNullOrWhiteSpace(cmnd.Name) &&
                !string.IsNullOrWhiteSpace(cmnd.Channel) &&
                !string.IsNullOrWhiteSpace(cmnd.Message))
            {
                await _context.CustomCommands.AddAsync(cmnd);
                await _context.SaveChangesAsync();
                CustomCommands.Add(cmnd);
            }
        }

        private async Task DeleteCommand(CustomCommand customCommand)
        {
            _context.CustomCommands.Remove(customCommand);
            await _context.SaveChangesAsync();
            CustomCommands.Remove(customCommand);
        }
    }
}