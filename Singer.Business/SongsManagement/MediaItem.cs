using System;

namespace Singer.Business.SongsManagement
{
    internal abstract class MediaItem
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? MediaPath { get; set; }
    }
}
