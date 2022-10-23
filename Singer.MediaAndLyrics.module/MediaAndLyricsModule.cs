using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Singer.MediaAndLyrics.Module.ViewModels;
using Singer.MediaAndLyrics.Module.Views;
using Singer.Presentation.Wpf;
using System;

namespace Singer.MediaAndLyrics.Module
{
    public class MediaAndLyricsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion(RegionNames.MainContent, typeof(MediaAndLyricsViewer));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<MediaAndLyricsViewerViewModel, MediaAndLyricsViewerViewModel>();
        }
    }
}
