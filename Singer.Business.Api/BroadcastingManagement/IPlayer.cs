using System;
using MediaPathFile = System.String;

namespace Singer.Business.Api.BroadcastingManagement
{
    public interface IPlayer
    {
        void Stop();

        void Pause();
        void Play();

        void PlayNext();

        public string LyricsFileName { get; }

        public Guid CurrrentMedia { get; }

        event EventHandler BroadcastingEnded;
        public event EventHandler<MediaPathFile>? PlayMediaRequested;
        public event EventHandler? StopRequested;
        public event EventHandler? PauseRequested;


    }
}
