using MvvmDialogs;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using Singer.Business.Api.BroadcastingManagement;
using Singer.Business.Api.PlaylistManagement;
using Singer.Business.Api.SongsManagement;
using Singer.Business.ApiServices;
using Singer.LibrariesManagement.Module;
using Singer.MediaAndLyrics.Module;
using Singer.MediaAndLyrics.Module.ViewModels;
using Singer.MediaAndLyrics.Module.Views;
using Singer.PlayerControlBar.Module;
using Singer.PlaylistManagement.Module;
using Singer.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;

namespace Singer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var rootFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            var rootFolderKey = config.AppSettings.Settings.AllKeys.FirstOrDefault(s => s.Equals("RootFolder"));
                       
            if (rootFolderKey != null)
                rootFolderPath = config.AppSettings.Settings["RootFolder"].Value;
            
            Container.Resolve<ISongLibrariesProvider>().Use(new List<string>() { rootFolderPath });
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ISongLibrariesProvider, SongLibrariesProvider>();
            containerRegistry.Register<ISongsProvider, SongsProvider>();
            containerRegistry.RegisterSingleton<IPlaylistService, PlaylistService>();
            containerRegistry.RegisterSingleton<IPlayer, BroadcastingService>();
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<LibrariesManagementModule>();
            moduleCatalog.AddModule<PlaylistManagementModule>();
            moduleCatalog.AddModule<MediaAndLyricsModule>();
            moduleCatalog.AddModule<PlayerControlBarModule>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.Register<LyricsWindow, MediaAndLyricsViewerViewModel>();
        }
    }
}
