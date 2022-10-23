using System;

namespace Singer.Business.Api.Common
{
    public class SongDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
       
        public TimeSpan Duration { get; set; }
    }
}
