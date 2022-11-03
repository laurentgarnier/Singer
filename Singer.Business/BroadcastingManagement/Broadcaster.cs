using Singer.Business.PlaylistManagement;
using System;
using System.IO;
using System.Linq;
using MediaPathFile = System.String;

namespace Singer.Business.BroadcastingManagement
{
    internal class Broadcaster
    {
        private readonly PlaylistManager? _playlistManager;

        public Guid CurrrentMedia { get; internal set; }

        public event EventHandler<MediaPathFile>? PlayMediaRequested;
        public event EventHandler? StopRequested;
        public event EventHandler? PauseRequested;

        public Broadcaster()
        {
            _playlistManager = PlaylistManager.Instance;
        }

        internal string GetLyrics()
        {
            var firstSongInPlaylist = _playlistManager?.Songs.FirstOrDefault();
            if(firstSongInPlaylist == null) return String.Empty;

            var lyricsFile = Path.Combine(Path.GetDirectoryName(firstSongInPlaylist.MediaPath), $"{Path.GetFileNameWithoutExtension(firstSongInPlaylist.MediaPath)}.pdf");
            if (File.Exists(lyricsFile))
                return lyricsFile;

            return String.Empty;
        }

        internal void Pause()
        {
            PauseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void PlayMediaFile(string mediaPathFile)
        {
            PlayMediaRequested?.Invoke(this, mediaPathFile);
        }

        internal void Play()
        {
            var firstSongInPlaylist = _playlistManager?.Songs.FirstOrDefault();
            if (firstSongInPlaylist == null || firstSongInPlaylist.MediaPath == null) return;
            PlayMediaFile(firstSongInPlaylist.MediaPath);
            CurrrentMedia = firstSongInPlaylist.Id;
        }

        internal void Stop()
        {
            StopRequested?.Invoke(this, EventArgs.Empty);
        }

        internal void PlayNextMedia()
        {
            var firstSongInPlaylist = _playlistManager?.Songs.FirstOrDefault();
            
            if (firstSongInPlaylist == null || firstSongInPlaylist.MediaPath == null || _playlistManager?.Songs.Count() < 1) return;

            _playlistManager?.Remove(firstSongInPlaylist.Id);
            Play();
        }
    }
}
