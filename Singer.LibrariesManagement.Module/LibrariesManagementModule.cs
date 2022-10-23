using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Singer.LibrariesManagement.Module.Views;
using Singer.Presentation.Wpf;

namespace Singer.LibrariesManagement.Module
{
    public class LibrariesManagementModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.Resolve<IRegionManager>().RegisterViewWithRegion(RegionNames.ManagementContent, typeof(LibrariesViewer));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
