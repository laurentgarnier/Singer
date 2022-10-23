using Microsoft.Xaml.Behaviors.Core;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Singer.Business.Api.BroadcastingManagement;
using Singer.Business.Api.Common;
using Singer.Business.Api.PlaylistManagement;
using Singer.Presentation.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Singer.PlaylistManagement.Module.ViewModels
{
    internal class PlaylistViewerViewModel : BindableBase
    {
        private readonly IPlaylistService _playlistService;
        private readonly IPlayer _player;

        public PlaylistViewerViewModel(IPlaylistService playlistService, IPlayer player, IEventAggregator eventAggregator)
        {
            _playlistService = playlistService;
            _player = player;
            eventAggregator.GetEvent<EllapsedTimeEvent>().Subscribe(UpdateEllapsedTime);

            _playlistService.SongListChanged += playlistService_SongListChanged;

            _songs = new ObservableCollection<SongDto>();
            ComputeHeaderText();
        }

        private void UpdateEllapsedTime(TimeSpan obj)
        {
            EllapsedTime = obj.ToString("mm\\:ss");
            if (_playlistService.Songs.Count() > 0)
                RemainingTime = (_playlistService.Songs.FirstOrDefault().Duration - obj).ToString("mm\\:ss");
        }

        private string _ellapsedTime;
        public string EllapsedTime
        {
            get { return _ellapsedTime; }
            set { SetProperty(ref _ellapsedTime, value); }
        }

        private string _remainingTime;
        public string RemainingTime
        {
            get { return _remainingTime; }
            set { SetProperty(ref _remainingTime, value); }
        }

        private void playlistService_SongListChanged(object? sender, EventArgs e)
        {
            _songs.Clear();
            _songs.AddRange(_playlistService.Songs);
            var currentSong = _playlistService.Songs.FirstOrDefault();
            if (currentSong != null)
                CurrentSong = currentSong.Name;
            else
                CurrentSong = String.Empty;
            ComputeHeaderText();
        }

        private ObservableCollection<SongDto> _songs;
        public ObservableCollection<SongDto> Songs
        {
            get { return _songs; }
            set { SetProperty(ref _songs, value); }
        }

        private DelegateCommand<SongDto?>? _songUnchecked;
        public ICommand SongUnchecked => _songUnchecked ??= new DelegateCommand<SongDto?>(ManageSongUnchecked);

        private void ManageSongUnchecked(SongDto? song)
        {
            if (_player.CurrrentMedia.Equals(song?.Id))
            {
                playlistService_SongListChanged(this, EventArgs.Empty);
                return;
            }
            _playlistService.RemoveSong(song);
            ComputeHeaderText();
        }

        private void ComputeHeaderText()
        {
            TimeSpan totalDuration = TimeSpan.Zero;
            foreach (var song in Songs)
                totalDuration += song.Duration;

            HeaderText = $"Playlist ({Songs.Count().ToString()}) - {totalDuration.ToString("mm\\:ss")}";
        }

        private string _currentSong;

        public string CurrentSong { get => _currentSong; set => SetProperty(ref _currentSong, value); }

        private string _headerText;

        public string HeaderText { get => _headerText; set => SetProperty(ref _headerText, value); }
    }
}
