using Singer.Business.Api.Common;
using System;
using System.Collections.Generic;

namespace Singer.Business.Api.SongsManagement
{
    public interface ISongsProvider
    {
        public IEnumerable<SongDto>? GetSongs(Guid libraryId);
    }
}
