using System;
using System.Collections.Generic;
using System.IO;

namespace Singer.Business.SongsManagement
{
    internal class MediaLibrary : MediaItem, ICloneable
    {
        private readonly ICollection<MusicMedia> _musics;
        private readonly ICollection<MediaLibrary> _childsLibraries;

        public MediaLibrary(string libraryPath) : this()
        {
            Name = Path.GetFileNameWithoutExtension(libraryPath);
            Id = Guid.NewGuid();
            MediaPath = libraryPath;
        }

        private MediaLibrary()
        {
            _musics = new List<MusicMedia>();
            _childsLibraries = new List<MediaLibrary>();
        }

        public void AddMusic(MusicMedia music)
        {
            _musics.Add(music);
        }

        public void AddChildLibrary(MediaLibrary childLibrary)
        {
            _childsLibraries.Add(childLibrary);
        }

        public IEnumerable<MusicMedia> Musics
        {
            get
            {
                foreach (var music in _musics) yield return music;
            }
        }

        public IEnumerable<MediaLibrary> ChildsLibraries
        {
            get
            {
                foreach (var library in _childsLibraries) yield return library;
            }
        }
        private void FillLibrairies(List<MediaLibrary> libraries, MediaLibrary library)
        {
            libraries.Add(library);
            foreach (var lib in library.ChildsLibraries)
                FillLibrairies(libraries, lib);
        }

        public object Clone()
        {
            var clonedLibrary = new MediaLibrary()
            {
                Id = this.Id,
                MediaPath = this.MediaPath,
                Name = this.Name,
            };
            foreach (var music in this.Musics)
                clonedLibrary.AddMusic(music);

            foreach (var library in this.ChildsLibraries)
                clonedLibrary.AddChildLibrary((MediaLibrary)library.Clone());

            return clonedLibrary;
        }

        internal MediaLibrary? FindLibrary(Guid libraryId)
        {
            if (libraryId.Equals(this.Id)) return this;

            foreach (var library in _childsLibraries)
            {
                var lib = library.FindLibrary(libraryId);
                if (lib != null) return lib;
            }
            return null;
        }
    }
}
