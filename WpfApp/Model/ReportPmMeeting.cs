using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportPmMeeting:ViewModelBase
    {
        private int _id;
        private string _projectName;
        private DateTime _targetDate;
        private bool _complete;
        private string _architect;
        private string _address;
        private string _state;
        private string _zip;
        private string _jobNo;
        private string _pmName;
        private string _estimatorName;
        private string _customerName;
        private int _bilingDue;
        private bool _isCip;
        private bool _isC3;
        private bool _isCertPayroll;
        private bool _isContractRecvd;
        private bool _storedMat;
        private List<ProjectMatTracking> _projectMatTracking;

        public int ProjectID
        {
            get => _id;
            set
            {
                if (value == _id) return;
                _id = value;
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

        public bool Complete
        {
            get => _complete;
            set
            {
                if (value == _complete) return;
                _complete = value;
                OnPropertyChanged();
            }
        }

        public string ArchitectName
        {
            get => _architect;
            set
            {
                if (value == _architect) return;
                _architect = value;
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

        public string PmName
        {
            get => _pmName;
            set
            {
                if (value == _pmName) return;
                _pmName = value;
                OnPropertyChanged();
            }
        }

        public string EstimatorName
        {
            get => _estimatorName;
            set
            {
                if (value == _estimatorName) return;
                _estimatorName = value;
                OnPropertyChanged();
            }
        }

        public int BilingDue
        {
            get => _bilingDue;
            set
            {
                if (value == _bilingDue) return;
                _bilingDue = value;
                OnPropertyChanged();
            }
        }

        public bool IsCip
        {
            get => _isCip;
            set
            {
                if (value == _isCip) return;
                _isCip = value;
                OnPropertyChanged();
            }
        }

        public bool IsC3
        {
            get => _isC3;
            set
            {
                if (value == _isC3) return;
                _isC3 = value;
                OnPropertyChanged();
            }
        }

        public bool IsCertPayroll
        {
            get => _isCertPayroll;
            set
            {
                if (value == _isCertPayroll) return;
                _isCertPayroll = value;
                OnPropertyChanged();
            }
        }

        public bool IsContractRecvd
        {
            get => _isContractRecvd;
            set
            {
                if (value == _isContractRecvd) return;
                _isContractRecvd = value;
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

        public string Product_Customer_Name
        {
            get
            {
                return _projectName + " | " + _customerName;
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

        public string FullAddress
        {
            get
            {
                return _address + ", " + _state + ", " + _zip;
            }
        }
    }

}