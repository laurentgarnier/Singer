using System.Collections.Generic;

namespace Singer.Business.Api.SongsManagement
{
    public interface ISongLibrariesProvider
    {
        public void Use(IEnumerable<string> searchDirectories);
        public SongLibraryDto? GetRootLibrary();
    }
}
