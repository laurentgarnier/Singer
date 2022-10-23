using Nelibur.ObjectMapper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Singer.Business.Api.BroadcastingManagement;
using Singer.Business.Api.Common;
using Singer.Business.Api.PlaylistManagement;
using Singer.Business.Api.SongsManagement;
using Singer.LibrariesManagement.Module.Extensions;
using Singer.LibrariesManagement.Module.Models;
using Singer.Presentation.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Singer.LibrariesManagement.Module.ViewModels
{
    public class LibrariesViewerViewModel : BindableBase
    {
        private readonly ISongsProvider _songsProvider;
        private readonly ISongLibrariesProvider _songLibrariesProvider;
        private readonly IPlaylistService _playlistService;
        private readonly IPlayer _player;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="songLibrariesProvider">Library list provider</param>
        /// <param name="songsProvider">Songs provider</param>
        public LibrariesViewerViewModel(ISongLibrariesProvider songLibrariesProvider, ISongsProvider songsProvider, IPlaylistService playlistService,
            IEventAggregator eventAggregator, IPlayer player)
        {
            _songLibrariesProvider = songLibrariesProvider;
            _songsProvider = songsProvider;
            _playlistService = playlistService;
            _player = player;

            _songs = new ObservableCollection<SongModel>();

            var rootLibraryDto = _songLibrariesProvider.GetRootLibrary();

            if (rootLibraryDto == null) return;

            RootLibrary = new ObservableCollection<SongLibraryModel>();
            RootLibrary.Add(rootLibraryDto.ConvertToModel());

            SelectedLibrary = RootLibrary[0];

            _playlistService.SongListChanged += playlistService_SongListChanged;

            eventAggregator.GetEvent<RootLibraryChangedEvent>().Subscribe(UpadteLibraryList);
        }

        private void UpadteLibraryList(string rootFolder)
        {
            _songLibrariesProvider.Use(new List<string>() { rootFolder });
            RootLibrary?.Clear();
            RootLibrary.Add(_songLibrariesProvider?.GetRootLibrary().ConvertToModel());
        }

        private void playlistService_SongListChanged(object? sender, System.EventArgs e)
        {
            ManageVisualCheckStatus(_selectedLibrary.Id);
        }

        private ObservableCollection<SongLibraryModel>? _rootLibrary;

        /// <summary>
        /// Root of library list
        /// </summary>
        public ObservableCollection<SongLibraryModel>? RootLibrary
        {
            get { return _rootLibrary; }
            set { SetProperty(ref _rootLibrary, value); }
        }

        private SongLibraryModel? _selectedLibrary;

        /// <summary>
        /// User selected library
        /// </summary>
        public SongLibraryModel? SelectedLibrary
        {
            get { return _selectedLibrary; }
            set
            {
                if (value == null) return;

                if (_selectedLibrary != null)
                    _selectedLibrary.IsSelected = false;
                value.IsSelected = true;

                SetProperty(ref _selectedLibrary, value);
                ManageVisualCheckStatus(value.Id);
            }
        }

        private void ManageVisualCheckStatus(Guid id)
        {
            Songs.Clear();
            var songsDto = _songsProvider.GetSongs(id);

            if (songsDto == null) return;

            TinyMapper.Bind<SongDto, SongModel>();

            var playlistSongs = _playlistService.Songs;
            foreach (var song in songsDto)
            {
                var songDtoInPlaylist = playlistSongs.FirstOrDefault<SongDto>(s => s.Id.Equals(song.Id));
                var songModel = TinyMapper.Map<SongModel>(song);
                songModel.IsSelected = songDtoInPlaylist != null;

                Songs.Add(songModel);
            }
        }

        private ObservableCollection<SongModel> _songs;

        /// <summary>
        /// Selected library' songs list
        /// </summary>
        public ObservableCollection<SongModel> Songs
        {
            get { return _songs; }
            set { SetProperty(ref _songs, value); }
        }

        #region Commands

        private DelegateCommand<SongLibraryModel>? _librarySelectedByUser;

        /// <summary>
        /// User interaction, he selects a library
        /// </summary>
        public ICommand LibrarySelectedByUser => _librarySelectedByUser ??= new DelegateCommand<SongLibraryModel>(ManageLibrarySelectedByUser);

        private void ManageLibrarySelectedByUser(SongLibraryModel library)
        {
            SelectedLibrary = library;
        }

        private DelegateCommand<SongModel?>? _songSelectedByUser;

        /// <summary>
        /// User interaction, he selects a song
        /// </summary>
        public ICommand SongSelectedByUser => _songSelectedByUser ??= new DelegateCommand<SongModel?>(ManageSongSelectedByUser);

        private void ManageSongSelectedByUser(SongModel? song)
        {
            if (song == null) return;
            TinyMapper.Bind<SongModel, SongDto>();
            var songDto = TinyMapper.Map<SongDto>(song);
            
            if (song.IsSelected)
            {
                _playlistService.AddSong(songDto);
            }   
            else
            {
                if (_player.CurrrentMedia.Equals(songDto.Id))
                {
                    song.IsSelected = true;
                    return; 
                }
               
                _playlistService.RemoveSong(songDto);
            }
                
        }



        #endregion Commands

       
    }
}
