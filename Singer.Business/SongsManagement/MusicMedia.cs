using Singer.Business.Helpers;
using System;
using System.IO;

namespace Singer.Business.SongsManagement
{
    internal class MusicMedia : MediaItem
    {
        public MusicMedia(string musicFilePath)
        {
            Id = Guid.NewGuid();
            MediaPath = musicFilePath;
            Duration = MediaDurationHelper.GetDuration(musicFilePath);
            Name = $"{Path.GetFileNameWithoutExtension(musicFilePath)} ({Duration.ToString("mm\\:ss")})";
        }

        public TimeSpan Duration { get; set; }
    }
}
