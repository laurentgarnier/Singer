using Singer.Business.Api.Common;
using System;
using System.Collections.Generic;

namespace Singer.Business.Api.PlaylistManagement
{
    public interface IPlaylistService
    {
        public IEnumerable<SongDto> Songs { get; }

        public void AddSong(SongDto song);

        void RemoveSong(SongDto song);
        void GotoNextSong();

        public event EventHandler<SongDto> SongChanged;
        public event EventHandler SongListChanged;

        string GetLyricsFile(Guid songId);
    }
}
