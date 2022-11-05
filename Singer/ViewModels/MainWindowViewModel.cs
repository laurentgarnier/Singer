using Microsoft.Xaml.Behaviors.Core;
using Prism.Events;
using Prism.Mvvm;
using Singer.Presentation.Wpf;
using System.Configuration;
using System.Linq;
using System.Windows.Input;

namespace Singer.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        private ActionCommand? _windowClosingCommand;
        public ICommand WindowClosingCommand => _windowClosingCommand ??= new ActionCommand(() => _eventAggregator.GetEvent<ApplicationClosingEvent>().Publish());

        private readonly IEventAggregator _eventAggregator;

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<RootLibraryChangedEvent>().Subscribe(UpadteLibraryList);
        }

        private void UpadteLibraryList(string obj)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var rootFolderKey = config.AppSettings.Settings.AllKeys.FirstOrDefault(s => s.Equals("RootFolder"));
            if (rootFolderKey != null)
                config.AppSettings.Settings["RootFolder"].Value = obj;
            else
                config.AppSettings.Settings.Add("RootFolder", obj);
            config.Save(ConfigurationSaveMode.Minimal);
        }

        private ActionCommand _nextPageCommand;
        public ICommand NextPageCommand => _nextPageCommand ??= new ActionCommand(GoToNextPage);

        private void GoToNextPage()
        {
            _eventAggregator.GetEvent<NextKeyPressedEvent>().Publish();
        }

        private ActionCommand _previousPageCommand;
        public ICommand PreviousPageCommand => _previousPageCommand ??= new ActionCommand(GoToPreviousPage);

        private void GoToPreviousPage()
        {
            _eventAggregator.GetEvent<PreviousKeyPressedEvent>().Publish();
        }
    }
}
