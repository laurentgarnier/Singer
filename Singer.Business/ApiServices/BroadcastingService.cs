using Singer.Business.Api.BroadcastingManagement;
using Singer.Business.BroadcastingManagement;
using System;
using MediaPathFile = System.String;

namespace Singer.Business.ApiServices
{
    public class BroadcastingService : IPlayer
    {
        public event EventHandler? BroadcastingEnded;
        public event EventHandler<MediaPathFile>? PlayMediaRequested;
        public event EventHandler? StopRequested;
        public event EventHandler? PauseRequested;

        private readonly Broadcaster _broadcaster;

        public string LyricsFileName => _broadcaster.GetLyrics();

        public Guid CurrrentMedia => _broadcaster.CurrrentMedia;

        public BroadcastingService()
        {
            _broadcaster = new Broadcaster();
            _broadcaster.PlayMediaRequested += broadcaster_PlayMediaRequested;
            _broadcaster.PauseRequested += broadcaster_PauseRequested;
            _broadcaster.StopRequested += broadcaster_StopRequested;

        }

        private void broadcaster_StopRequested(object sender, EventArgs e)
        {
            StopRequested?.Invoke(this, EventArgs.Empty);
        }

        private void broadcaster_PauseRequested(object sender, EventArgs e)
        {
            PauseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void broadcaster_PlayMediaRequested(object sender, MediaPathFile e)
        {
            PlayMediaRequested?.Invoke(this, e);
        }

        public void Pause()
        {
            _broadcaster.Pause();
        }

        public void Play()
        {
            _broadcaster.Play();
        }

        public void Stop()
        {
            _broadcaster.Stop();
        }

        public void PlayNext()
        {
            _broadcaster.PlayNextMedia();
        }
    }
}
