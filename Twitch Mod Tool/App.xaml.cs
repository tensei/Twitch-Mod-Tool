using System.Windows;
using Ninject;
using Twitch_Mod_Tool.Models;
using Twitch_Mod_Tool.Services;
using Twitch_Mod_Tool.Utilities;
using Twitch_Mod_Tool.ViewModels;

namespace Twitch_Mod_Tool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var kernel = new StandardKernel();
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


            kernel.Get<MainWindow>().Show();
        }
    }
}
