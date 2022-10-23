using Singer.Business.SongsManagement;
using System;
using System.Collections.Generic;

namespace Singer.Business.PlaylistManagement
{
    internal class PlaylistManager
    {
        private List<MusicMedia> _songs = new List<MusicMedia>();
        private readonly SongLibrariesRepository? _songLibrariesRepository;

        public event EventHandler<MusicMedia>? SongChanged;
        public event EventHandler? SongListChanged;

        private static PlaylistManager? _instance;
        public static PlaylistManager? Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PlaylistManager();
                return _instance;
            }
        }
        private PlaylistManager()
        {
            _songLibrariesRepository = SongLibrariesRepository.Instance;
        }

        public IEnumerable<MusicMedia> Songs
        {
            get { foreach (var song in _songs) yield return song; }
        }

        public void AddSong(Guid songId)
        {
            var songToAdd = _songs.Find(s => s.Id.Equals(songId));
            if (songToAdd != null) return;

            songToAdd = _songLibrariesRepository?.GetMusic(songId);
            
            if (songToAdd == null) return;
            
            _songs.Add(songToAdd);
            SongListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void GotoNextSong()
        {
            if (_songs.Count == 0) return;

            var song = _songs[0];
            SongChanged?.Invoke(this, song);
            Remove(song.Id);
        }


        public void Remove(Guid songId)
        {
            var songToRemoved = _songs.Find(s => s.Id.Equals(songId));
            _songs.Remove(songToRemoved);
            SongListChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
