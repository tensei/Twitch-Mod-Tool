using System.Windows;
using Ninject;
using Twitch_Mod_Tool.Interfaces;
using Twitch_Mod_Tool.Models;
using Twitch_Mod_Tool.Services;
using Twitch_Mod_Tool.Utilities;
using Twitch_Mod_Tool.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Twitch_Mod_Tool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            var kernel = new StandardKernel();
            kernel.Bind<ToolContext>().ToSelf().InSingletonScope();

            kernel.Bind<TwitchSettings>().ToSelf().InSingletonScope();
            kernel.Bind<SettingsLoader>().ToSelf().InSingletonScope();
            kernel.Bind<IPhoneticMatch>().To<Metaphone>();

            // load settings before starting 
            kernel.Get<SettingsLoader>().Load();

            kernel.Bind<TwitchService>().ToSelf().InSingletonScope();
            kernel.Bind<OverrustlelogsService>().ToSelf().InSingletonScope();

            kernel.Bind<MainWindowViewModel>().ToSelf().InSingletonScope();
            kernel.Bind<ChatViewModel>().ToSelf().InSingletonScope();
            kernel.Bind<CommandsViewModel>().ToSelf().InSingletonScope();

            kernel.Bind<MainWindow>().ToSelf().InSingletonScope();

            await kernel.Get<ToolContext>().Database.MigrateAsync();

            kernel.Get<MainWindow>().Show();
        }
    }
}
