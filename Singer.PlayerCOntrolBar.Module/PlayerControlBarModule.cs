using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Singer.PlayerControlBar.Module.Views;
using Singer.PlaylistManagement.Module.Views;
using Singer.Presentation.Wpf;
using System;

namespace Singer.PlayerControlBar.Module
{
    public class PlayerControlBarModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion(RegionNames.PlayerControlContent, typeof(PlayerControlBarView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
