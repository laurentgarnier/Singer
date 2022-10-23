using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace Singer.LibrariesManagement.Module.Models
{
    public class SongLibraryModel : BindableBase
    {
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name = String.Empty;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private ObservableCollection<SongLibraryModel> _childs = new ObservableCollection<SongLibraryModel>();

        public ObservableCollection<SongLibraryModel> Childs
        {
            get { return _childs; }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

    }
}
