using Singer.Business.Api.SongsManagement;
using Singer.Business.SongsManagement;
using System.Collections.Generic;
using System.Linq;

namespace Singer.Business.ApiServices
{
    public class SongLibrariesProvider : ISongLibrariesProvider
    {
        private readonly SongLibrariesRepository? _songsRepository;

        public SongLibrariesProvider()
        {
            _songsRepository = SongLibrariesRepository.Instance;
        }

        public SongLibraryDto? GetRootLibrary()
        {
            var rootMediaLibrary = _songsRepository?.Libraries;
            if(rootMediaLibrary == null) return null;

            var rootLibraryDto = new SongLibraryDto()
            {
                Id = rootMediaLibrary.Id,
                Name = rootMediaLibrary.Name
            };

            FillWithChildLibraries(rootMediaLibrary, rootLibraryDto);

            return rootLibraryDto;
        }

        private static void FillWithChildLibraries(MediaLibrary mediaLibrary, SongLibraryDto rootLibraryDto)
        {
            foreach (var library in mediaLibrary.ChildsLibraries)
            {
                var childSongLibraryDto = new SongLibraryDto()
                {
                    Id = library.Id,
                    Name = library.Name
                };
                rootLibraryDto.AddChildLibrary(childSongLibraryDto);
                FillWithChildLibraries(library, childSongLibraryDto);
            }
        }

        public void Use(IEnumerable<string> searchDirectories)
        {
            if (searchDirectories == null || searchDirectories?.Count() == 0) return;

            foreach (var directory in searchDirectories)
            {
                _songsRepository?.LoadDirectory(directory);
            }
        }
    }
}
