using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Singer.PlaylistManagement.Module.Views;
using Singer.Presentation.Wpf;

namespace Singer.PlaylistManagement.Module
{
    public class PlaylistManagementModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion(RegionNames.PlaylistContent, typeof(PlaylistViewer));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
