using System;
using System.Collections.Generic;

namespace Singer.Business.Api.SongsManagement
{
    public class SongLibraryDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        private List<SongLibraryDto> _childsLibraries = new List<SongLibraryDto>();

        public IEnumerable<SongLibraryDto> ChildsLibraries
        {
            get
            {
                foreach (var library in _childsLibraries) yield return library;
            }
        }

        public void AddChildLibrary(SongLibraryDto childLibrary)
        {
            _childsLibraries.Add(childLibrary);
        }
    }
}
