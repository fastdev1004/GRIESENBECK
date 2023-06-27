using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class Note:ViewModelBase
    {
        private int _notesID;
        private int _notesPK;
        private string _notesPKDesc;
        private string _notesNote;
        private DateTime _notesDateAdded;
        private string _notesUser;
        private string _notesUserName;

        public int NoteID
        {
            get => _notesID;
            set
            {
                if (value == _notesID) return;
                _notesID = value;
                OnPropertyChanged();
            }
        }

        public int NotePK
        {
            get => _notesPK;
            set
            {
                if (value == _notesPK) return;
                _notesPK = value;
                OnPropertyChanged();
            }
        }

        public string NotesPKDesc
        {
            get => _notesPKDesc;
            set
            {
                if (value == _notesPKDesc) return;
                _notesPKDesc = value;
                OnPropertyChanged();
            }
        }

        public DateTime NotesDateAdded
        {
            get => _notesDateAdded;
            set
            {
                if (value == _notesDateAdded) return;
                _notesDateAdded = value;
                OnPropertyChanged();
            }
        }

        public string NoteUser
        {
            get => _notesUser;
            set
            {
                if (value == _notesUser) return;
                _notesUser = value;
                OnPropertyChanged();
            }
        }

        public string NoteUserName
        {
            get => _notesUserName;
            set
            {
                if (value == _notesUserName) return;
                _notesUserName = value;
                OnPropertyChanged();
            }
        }

        public string NotesNote
        {
            get => _notesNote;
            set
            {
                if (value == _notesNote) return;
                _notesNote = value;
                OnPropertyChanged();
            }
        }

    }
}
