using Microsoft.Xaml.Behaviors.Core;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Singer.Business.Api.BroadcastingManagement;
using Singer.Business.Api.PlaylistManagement;
using Singer.MediaAndLyrics.Module.Models;
using Singer.Presentation.Wpf;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Singer.MediaAndLyrics.Module.ViewModels
{
    public class MediaAndLyricsViewerViewModel : BindableBase
    {
        private readonly IPlayer _player;
        private readonly MediaPlayer _mediaPlayer;
        private readonly IPlaylistService _playlistService;
        private MediaPlayerStatus _mediaPlayerStatus;
        private readonly IEventAggregator _eventAggregator;

        private DispatcherTimer _ellapsedTimeTimer = new DispatcherTimer();

        public DelegateCommand<CancelEventArgs> LyricsWindowClosingCommand { get; set; }


        public MediaAndLyricsViewerViewModel(IPlayer player, IEventAggregator eventAggregator, IPlaylistService playlistService)
        {
            _player = player;
            _eventAggregator = eventAggregator;
            _playlistService = playlistService;

            _player.PlayMediaRequested += player_PlayMediaRequested;
            _player.StopRequested += player_StopRequested;
            _player.PauseRequested += player_PauseRequested;

            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaEnded += mediaPlayer_MediaEnded;

            _mediaPlayerStatus = MediaPlayerStatus.Stopped;
            _ellapsedTimeTimer.Interval = TimeSpan.FromSeconds(1);
            _ellapsedTimeTimer.Tick += UpdateEllapsedTime;

            _playlistService.SongChanged += playlistService_SongChanged;
            _playlistService.SongListChanged += playlistService_SongListChanged;

            LyricsWindowClosingCommand = new DelegateCommand<CancelEventArgs>(LyricsWindowClosing);
            _eventAggregator.GetEvent<ApplicationClosingEvent>().Subscribe(() => Process.GetCurrentProcess().Kill());
            _eventAggregator.GetEvent<PreviousKeyPressedEvent>().Subscribe(() => GoToPreviousPage());
            _eventAggregator.GetEvent<NextKeyPressedEvent>().Subscribe(() => GoToNextPage());
            RemainingTime = "00:00";
            DisplayedTime = "00:00";

        }

        private void playlistService_SongListChanged(object? sender, EventArgs e)
        {
            ComputeRemaininSong();
        }

        private void ComputeRemaininSong()
        {
            RemainingSongs = _playlistService.Songs.Count() > 0 ? (_playlistService.Songs.Count()-1).ToString() : "0";
        }

        private void playlistService_SongChanged(object? sender, Business.Api.Common.SongDto e)
        {
            throw new NotImplementedException();
        }

        private CancelEventArgs _cancelLyricsWindowClosing;
        public CancelEventArgs CancelLyricsWindowClosing
        {
            get { return _cancelLyricsWindowClosing; }
            set { SetProperty(ref _cancelLyricsWindowClosing, value); }
        }

        private void LyricsWindowClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            LyricsWindowVisibility = System.Windows.Visibility.Hidden;
        }

        private void UpdateEllapsedTime(object? sender, EventArgs e)
        {
            TimeSpan ellapsedTime = TimeSpan.MinValue;
            try
            {
                if (_mediaPlayer.NaturalDuration.HasTimeSpan)
                {
                    ellapsedTime = _mediaPlayer.Position;
                    _eventAggregator.GetEvent<EllapsedTimeEvent>().Publish(ellapsedTime);
                    DisplayedTime = ellapsedTime.ToString("mm\\:ss");
                    if (_playlistService.Songs.Count() > 0)
                        RemainingTime = (_playlistService.Songs.FirstOrDefault().Duration - ellapsedTime).ToString("mm\\:ss");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            
        }

        private void mediaPlayer_MediaEnded(object? sender, System.EventArgs e)
        {
            if (_mediaPlayer.Source != null)
                _mediaPlayer.Close();
            ClosePdf = true;
            RemainingTime = "00:00";
            DisplayedTime = "00:00";
            _mediaPlayerStatus = MediaPlayerStatus.Stopped;
            _player.PlayNext();
        }

        private void player_PauseRequested(object? sender, System.EventArgs e)
        {
            _mediaPlayer?.Pause();
            _mediaPlayerStatus = MediaPlayerStatus.Pause;
        }

        private void player_StopRequested(object? sender, System.EventArgs e)
        {
            _mediaPlayer?.Stop();
            _ellapsedTimeTimer.Stop();
            _mediaPlayerStatus = MediaPlayerStatus.Stopped;
        }

        private void player_PlayMediaRequested(object? sender, string e)
        {
            if (_mediaPlayerStatus == MediaPlayerStatus.Pause)
            {
                _mediaPlayer.Play();
                _mediaPlayerStatus = MediaPlayerStatus.Playing;
                return;
            }

            if (_mediaPlayer.Source != null)
                _mediaPlayer.Close();

            var lyricsFile = _player.LyricsFileName;

            if (!lyricsFile.Equals(string.Empty))
            {
                PdfFile = lyricsFile;
                ClosePdf = false;
            }
            else
                ClosePdf = true;
            
            _mediaPlayer.Open(new Uri(e));
           
            _mediaPlayer.Play();
            _mediaPlayerStatus = MediaPlayerStatus.Playing;
            _ellapsedTimeTimer.Start();
        }
        
        private string? _pdfFile;

        public string? PdfFile { get => _pdfFile; set => SetProperty(ref _pdfFile, value); }

        private bool _closePdf = false;
        public bool ClosePdf
        {
            get { return _closePdf; }
            set { SetProperty(ref _closePdf, value); }
        }

        private System.Windows.Visibility _lyricsWindowVisibility;

        public System.Windows.Visibility LyricsWindowVisibility { get => _lyricsWindowVisibility; set => SetProperty(ref _lyricsWindowVisibility, value); }

        private ActionCommand loadedCommand;
        public ICommand LoadedCommand => loadedCommand ??= new ActionCommand(Loaded);

        private void Loaded()
        {
            LyricsWindowVisibility = System.Windows.Visibility.Hidden;
        }

        private void GoToNextPage()
        {
            NextPage = !NextPage;
        }

        private void GoToPreviousPage()
        {
            PreviousPage = !PreviousPage;
        }

        private string _remainingTime;

        public string RemainingTime { get => _remainingTime; set => SetProperty(ref _remainingTime, value); }

        private string _displayedTime;

        public string DisplayedTime { get => _displayedTime; set => SetProperty(ref _displayedTime, value); }

        private string _remainingSongs;

        public string RemainingSongs { get => _remainingSongs; set => SetProperty(ref _remainingSongs, value); }

        private bool _nextPage;

        public bool NextPage { get => _nextPage; set => SetProperty(ref _nextPage, value); }

        private bool _previousPage;

        public bool PreviousPage { get => _previousPage; set => SetProperty(ref _previousPage, value); }
    }
}
