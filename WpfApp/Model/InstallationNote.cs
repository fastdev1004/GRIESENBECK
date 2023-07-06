using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class InstallationNote:ViewModelBase
    {
        private int _fetchID;
        private int _installationID;
        private string _installationNote;
        private int _projectID;
        private string _installNoteUser;
        private DateTime _installDateAdded;
        private int _actionFlag;

        public int FetchID
        {
            get => _fetchID;
            set
            {
                if (value == _fetchID) return;
                _fetchID = value;
                OnPropertyChanged();
            }
        }

        public int ID
        {
            get => _installationID;
            set
            {
                if (value == _installationID) return;
                _installationID = value;
                OnPropertyChanged();
            }
        }

        public int ProjectID
        {
            get => _projectID;
            set
            {   
                if (value == _projectID) return;
                _projectID = value;
                OnPropertyChanged();
            }
        }

        public string InstallNote
        {
            get => _installationNote;
            set
            {
                if (value == _installationNote) return;
                _installationNote = value;
                OnPropertyChanged();
            }
        }

        public string InstallNoteUser
        {
            get => _installNoteUser;
            set
            {
                if (value == _installNoteUser) return;
                _installNoteUser = value;
                OnPropertyChanged();
            }
        }

        public DateTime InstallDateAdded
        {
            get => _installDateAdded;
            set
            {
                if (value == _installDateAdded) return;
                _installDateAdded = value;
                OnPropertyChanged();
            }
        }

        public int ActionFlag
        {
            get => _actionFlag;
            set
            {
                if (value == _actionFlag) return;
                _actionFlag = value;
                OnPropertyChanged();
            }
        }
    }
}
