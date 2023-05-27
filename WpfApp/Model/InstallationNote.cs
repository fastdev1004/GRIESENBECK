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
        private int _installationID;
        private string _installationNote;
        private int _projectID;
        private DateTime _installDateAdded;

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
    }
}
