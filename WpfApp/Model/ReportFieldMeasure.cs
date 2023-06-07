using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportFieldMeasure : ViewModelBase
    {
        private int _projectID;
        private string _projectName;
        private DateTime _targetDate;
        private DateTime _dateComplete;
        private string _address;
        private string _state;
        private string _zip;
        private string _jobNo;
        private string _salesmanName;
        private string _customerName;
        private bool _storedMat;
        private string _notes;
        private int _billingDue;
        private List<ProjectMatTracking> _projectMatTrackings;

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

        public DateTime DateComplete
        {
            get => _dateComplete;
            set
            {
                if (value == _dateComplete) return;
                _dateComplete = value;
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

        public bool StoredMat
        {
            get => _storedMat;
            set
            {
                if (value == _storedMat) return;
                _storedMat = value;
                OnPropertyChanged();
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (value == _notes) return;
                _notes = value;
                OnPropertyChanged();
            }
        }

        public int BillingDue
        {
            get => _billingDue;
            set
            {
                if (value == _billingDue) return;
                _billingDue = value;
                OnPropertyChanged();
            }
        }

        public List<ProjectMatTracking> ProjectMatTrackings
        {
            get => _projectMatTrackings;
            set
            {
                if (value == _projectMatTrackings) return;
                _projectMatTrackings = value;
                OnPropertyChanged();
            }
        }


    }
}