using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class InHouseInstaller:ViewModelBase
    {
        private int _installerID;
        private string _installerName;
        private string _installerCell;
        private string _installerEmail;
        private string _OSHALevel;
        private int _crewID;
        private DateTime _OSHAExpireDate;
        private string _OSHACert;
        private DateTime _firstAidExpireDate;
        private string _firstAidCert;
        private bool _active;

        public int ID
        {
            get => _installerID;
            set
            {
                if (value == _installerID) return;
                _installerID = value;
                OnPropertyChanged();
            }
        }

        public string InstallerName
        {
            get => _installerName;
            set
            {
                if (value == _installerName) return;
                _installerName = value;
                OnPropertyChanged();
            }
        }

        public string InstallerCell
        {
            get => _installerCell;
            set
            {
                if (value == _installerCell) return;
                _installerCell = value;
                OnPropertyChanged();
            }
        }

        public string InstallerEmail
        {
            get => _installerEmail;
            set
            {
                if (value == _installerEmail) return;
                _installerEmail = value;
                OnPropertyChanged();
            }
        }
        public string OSHALevel
        {
            get => _OSHALevel;
            set
            {
                if (value == _OSHALevel) return;
                _OSHALevel = value;
                OnPropertyChanged();
            }
        }
        public int CrewID
        {
            get => _crewID;
            set
            {
                if (value == _crewID) return;
                _crewID = value;
                OnPropertyChanged();
            }
        }
        public DateTime OSHAExpireDate
        {
            get => _OSHAExpireDate;
            set
            {
                if (value == _OSHAExpireDate) return;
                _OSHAExpireDate = value;
                OnPropertyChanged();
            }
        }
        public string OSHACert
        {
            get => _OSHACert;
            set
            {
                if (value == _OSHACert) return;
                _OSHACert = value;
                OnPropertyChanged();
            }
        }
        public DateTime FirstAidExpireDate
        {
            get => _firstAidExpireDate;
            set
            {
                if (value == _firstAidExpireDate) return;
                _firstAidExpireDate = value;
                OnPropertyChanged();
            }
        }
        public string FirstAidCert
        {
            get => _firstAidCert;
            set
            {
                if (value == _firstAidCert) return;
                _firstAidCert = value;
                OnPropertyChanged();
            }
        }

        public bool Active
        {
            get => _active;
            set
            {
                if (value == _active) return;
                _active = value;
                OnPropertyChanged();
            }
        }
    }
}
