using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportJobArchRep : ViewModelBase
    {
        private int _projectID;
        private int _projectMatID;
        private DateTime _targetDate;
        private string _architect;
        private string _archRep;
        private string _projectName;
        private string _customerName;
        private string _address;
        private string _state;
        private string _zip;
        private string _jobNo;
        private string _salesmanName;
        private bool _active;
        private List<ProjectMatTracking> _projectMatTracking;

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

        public int ProjectMatID
        {
            get => _projectMatID;
            set
            {
                if (value == _projectMatID) return;
                _projectMatID = value;
                OnPropertyChanged();
            }
        }

        public DateTime TargetDate
        {
            get => _targetDate;
            set
            {
                if (value == _targetDate) return;
                _targetDate = value;
                OnPropertyChanged();
            }
        }

        public string Architect
        {
            get => _architect;
            set
            {
                if (value == _architect) return;
                _architect = value;
                OnPropertyChanged();
            }
        }

        public string ArchRep
        {
            get => _archRep;
            set
            {
                if (value == _archRep) return;
                _archRep = value;
                OnPropertyChanged();
            }
        }

        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (value == _customerName) return;
                _customerName = value;
                OnPropertyChanged();
            }
        }

        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (value == _projectName) return;
                _projectName = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                if (value == _address) return;
                _address = value;
                OnPropertyChanged();
            }
        }

        public string State
        {
            get => _state;
            set
            {
                if (value == _state) return;
                _state = value;
                OnPropertyChanged();
            }
        }

        public string Zip
        {
            get => _zip;
            set
            {
                if (value == _zip) return;
                _zip = value;
                OnPropertyChanged();
            }
        }

        public string FullAddress
        {
            get { return _address + ',' + _state + ',' + _zip; }
        }

        public string JobNo
        {
            get => _jobNo;
            set
            {
                if (value == _jobNo) return;
                _jobNo = value;
                OnPropertyChanged();
            }
        }

        public string SalesmanName
        {
            get => _salesmanName;
            set
            {
                if (value == _salesmanName) return;
                _salesmanName = value;
                OnPropertyChanged();
            }
        }

        public List<ProjectMatTracking> ProjectMatTrackings
        {
            get => _projectMatTracking;
            set
            {
                if (value == _projectMatTracking) return;
                _projectMatTracking = value;
                OnPropertyChanged();
            }
        }
    }
}