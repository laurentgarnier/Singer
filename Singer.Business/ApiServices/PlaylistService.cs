using Singer.Business.Api.Common;
using Singer.Business.Api.PlaylistManagement;
using Singer.Business.PlaylistManagement;
using System;
using System.Collections.Generic;

namespace Singer.Business.ApiServices
{
    public class PlaylistService : IPlaylistService
    {

        private readonly PlaylistManager? _playlistManager;

        public PlaylistService()
        {
            _playlistManager = PlaylistManager.Instance;
            if(_playlistManager == null) return;

            _playlistManager.SongChanged += playlistManager_SongChanged;
            _playlistManager.SongListChanged += playlistManager_SongListChanged;
        }

        private void playlistManager_SongListChanged(object sender, EventArgs e)
        {
            SongListChanged?.Invoke(this, EventArgs.Empty);
        }

        private void playlistManager_SongChanged(object sender, SongsManagement.MusicMedia e)
        {
            SongChanged?.Invoke(this, new SongDto() { Id = e.Id, Name = e.Name });
        }

        public IEnumerable<SongDto> Songs
        {
            get
            {
                List<SongDto> songDtos = new List<SongDto>();

                if (_playlistManager == null) return songDtos; 
                foreach (var music in _playlistManager.Songs)
                {
                    songDtos.Add(new SongDto() { Id = music.Id, Name = music.Name, Duration = music.Duration });
                }
                return songDtos;
            }
        }

        public event EventHandler<SongDto>? SongChanged;
        public event EventHandler? SongListChanged;

        public void AddSong(SongDto song)
        {
            _playlistManager?.AddSong(song.Id);
        }

        public void GotoNextSong()
        {
            _playlistManager.GotoNextSong();
        }

        public void RemoveSong(SongDto song)
        {
            _playlistManager.Remove(song.Id);
        }
    }
}
