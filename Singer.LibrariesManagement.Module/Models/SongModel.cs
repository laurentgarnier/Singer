using Prism.Mvvm;
using System;

namespace Singer.LibrariesManagement.Module.Models
{
    public class SongModel : BindableBase
    {
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string? _name;
        public string? Name
        {
            get { return _name; }// return $"{_name} ({_duration.ToString("mm\\:ss")})"; }
            set { SetProperty(ref _name, value); }
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set { SetProperty(ref _duration, value); }
        }

        private bool _isSeletected;
        public bool IsSelected
        {
            get { return _isSeletected; }
            set { SetProperty(ref _isSeletected, value); }
        }

    }
}
