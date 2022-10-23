using Microsoft.Xaml.Behaviors.Core;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.FolderBrowser;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Singer.Business.Api.BroadcastingManagement;
using Singer.Business.Api.PlaylistManagement;
using Singer.MediaAndLyrics.Module.Views;
using Singer.Presentation.Wpf;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace Singer.PlayerControlBar.Module.ViewModels
{
    public class PlayerControlBarViewModel : BindableBase
    {
        private readonly IPlayer _player;
        private readonly IPlaylistService _playlistService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly LyricsWindow _lyricsWindow;


        public PlayerControlBarViewModel(IPlayer player, IPlaylistService playlistService, IEventAggregator eventAggregator, IDialogService dialogService, LyricsWindow lyricsWindow)
        {
            _player = player;
            _playlistService = playlistService;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _lyricsWindow = lyricsWindow;

            eventAggregator.GetEvent<EllapsedTimeEvent>().Subscribe(UpdateEllapsedTime);
            _playlistService.SongListChanged += playlistService_SongListChanged;
            DisplayedTime = "00:00";
            RemainingTime = "00:00";
        }

        private void UpdateEllapsedTime(TimeSpan obj)
        {
            DisplayedTime = obj.ToString("mm\\:ss");
            if (_playlistService.Songs.Count() > 0)
                RemainingTime = (_playlistService.Songs.FirstOrDefault().Duration - obj).ToString("mm\\:ss");
        }

        private ActionCommand? _playCommand;
        public ICommand PlayCommand => _playCommand ??= new ActionCommand(() => _player.Play());

        private ActionCommand? _stopCommand;
        public ICommand StopCommand => _stopCommand ??= new ActionCommand(() => _player.Stop());

        private ActionCommand? _pauseCommand;
        public ICommand PauseCommand => _pauseCommand ??= new ActionCommand(() => _player.Pause());

        private ActionCommand? _nextCommand;
        public ICommand NextCommand => _nextCommand ??= new ActionCommand(() => _player.PlayNext());

        private ActionCommand? _openSecondScreenCommand;
        public ICommand OpenSecondScreenCommand => _openSecondScreenCommand ??= new ActionCommand(() =>
        {
                _lyricsWindow.Show();
        });

        private DelegateCommand? _paramCommand;
        public ICommand ParamCommand => _paramCommand ??= new DelegateCommand(EnteringApplicationParameters);

        private void EnteringApplicationParameters()
        {
            var settings = new FolderBrowserDialogSettings
            {
                Description = "Sélectionner le répertoire contenant les fichiers",
                SelectedPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            };

            bool? success = _dialogService.ShowFolderBrowserDialog(this, settings);
            if (success == true)
            {
                _eventAggregator.GetEvent<RootLibraryChangedEvent>().Publish(settings.SelectedPath);
            }
        }

        private string _currentSong;

        public string CurrentSong { get => _currentSong; set => SetProperty(ref _currentSong, value); }

        private string _nextSong;

        public string NextSong { get => _nextSong; set => SetProperty(ref _nextSong, value); }

        private void playlistService_SongListChanged(object? sender, System.EventArgs e)
        {
            var currentsong = _playlistService.Songs.FirstOrDefault();
            if (currentsong != null)
                CurrentSong = currentsong.Name;
            else
                CurrentSong = String.Empty;

            if (_playlistService.Songs.Count() < 2)
            {
                NextSong = String.Empty;
                return;
            }

            var nextSong = _playlistService.Songs.ElementAt(1);
            if (nextSong != null)
                NextSong = nextSong.Name;
            else
                NextSong = String.Empty;
        }

        private string _displayedTime;
        public string DisplayedTime
        {
            get { return _displayedTime; }
            set { SetProperty(ref _displayedTime, value); }
        }

        private string _remainingTime;
        public string RemainingTime
        {
            get { return _remainingTime; }
            set { SetProperty(ref _remainingTime, value); }
        }
    }
}
