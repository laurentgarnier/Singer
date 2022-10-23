using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Singer.Business.SongsManagement
{
    internal class SongLibrariesRepository
    {
        private static SongLibrariesRepository _instance;
        private readonly List<MusicMedia> _musics;

        public event EventHandler RootLibraryChanged;

        public static SongLibrariesRepository? Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SongLibrariesRepository();
                return _instance;
            }
        }

        private SongLibrariesRepository()
        {
            _musics = new List<MusicMedia>();
        }

        public void LoadDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) return;

            var library = new MediaLibrary(directoryPath);


            LoadFiles(directoryPath, library);
            LoadSubDirectories(directoryPath, library);

           // if (_root == null)
                _root = library;
            RootLibraryChanged?.Invoke(this, EventArgs.Empty);
            //else
            //    _root.AddChildLibrary(library);
        }

        public MediaLibrary Libraries
        {
            get
            {
                return (MediaLibrary)_root.Clone();
            }
        }

        public MusicMedia? GetMusic(Guid mediaId)
        {
            if(_musics == null || _musics.Count == 0) return null;

            return _musics.Find(m => m.Id.Equals(mediaId));
        }

        private MediaLibrary _root = null;

        private void LoadSubDirectories(string directoryPath, MediaLibrary library)
        {
            var subdirectories = Directory.GetDirectories(directoryPath);
            foreach (var subdirectory in subdirectories)
            {
                var subLibrary = new MediaLibrary(subdirectory);
                LoadFiles(subdirectory, subLibrary);
                LoadSubDirectories(subdirectory, subLibrary);
                library.AddChildLibrary(subLibrary);
            }

        }

        /// <summary>
        /// Load music files with an associated lyrics file.
        /// Associated lyrics file must have exactly the same name
        /// </summary>
        /// <param name="directoryPath">Directory where the music files are searched</param>
        /// <param name="library">The root library object where the musics will be linked</param>
        private void LoadFiles(string directoryPath, MediaLibrary library)
        {
            var scanner = new DirectoryInfo(directoryPath);
            var musicFiles = scanner.GetFiles("*.mp3").ToList();
            var lyricsFiles = scanner.GetFiles("*.pdf").ToList();

            foreach (var mp3File in musicFiles)
            {
                //foreach (var pdfFile in lyricsFiles)
                //{
                //    // only music files with associated lyric file is added
                //    if (Path.GetFileNameWithoutExtension(mp3File.Name).Equals(Path.GetFileNameWithoutExtension(pdfFile.Name)))
                //    {
                        var media = new MusicMedia(mp3File.FullName);
                        library.AddMusic(media);
                        _musics.Add(media);
                //    }
                //}
            }
        }

        internal IEnumerable<MusicMedia> GetLibrarySongs(Guid libraryId)
        {
            var library = _root.FindLibrary(libraryId);
            if (library == null) return null;
            return library.Musics;
        }
    }
}
