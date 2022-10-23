using Singer.Business.Api.Common;
using Singer.Business.Api.SongsManagement;
using Singer.Business.Helpers;
using Singer.Business.SongsManagement;
using System;
using System.Collections.Generic;

namespace Singer.Business.ApiServices
{
    public class SongsProvider : ISongsProvider
    {
        private readonly SongLibrariesRepository? _songsRepository;

        public SongsProvider()
        {
            _songsRepository = SongLibrariesRepository.Instance;
        }

        public IEnumerable<SongDto>? GetSongs(Guid libraryId)
        {
            var songs = _songsRepository?.GetLibrarySongs(libraryId);
            if (songs == null) return null;

            List<SongDto> songsDto = new List<SongDto>();
            foreach (var song in songs)
            {
                songsDto.Add(new SongDto() { Name = song.Name, Id = song.Id, Duration = MediaDurationHelper.GetDuration(song.MediaPath) });
            }
            return songsDto;
        }
    }
}
