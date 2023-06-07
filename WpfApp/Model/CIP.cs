using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class CIP:ViewModelBase
    {
        private string _jobNo;
        private int _projectID;
        private string _cipType;
        private DateTime _targetDate;
        private double _originalContractAmt;
        private double _finalContractAmt;
        private DateTime _formsRecD;
        private DateTime _formsSent;
        private DateTime _certRecD;
        private bool _exemptionApproved;
        private DateTime _exemptionAppDate;
        private string _crewEnrolled;
        private string _notes;

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

        public string CipType
        {
            get => _cipType;
            set
            {
                if (value == _cipType) return;
                _cipType = value;
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
        public double OriginalContractAmt
        {
            get => _originalContractAmt;
            set
            {
                if (value == _originalContractAmt) return;
                _originalContractAmt = value;
                OnPropertyChanged();
            }
        }

        public double FinalContractAmt
        {
            get => _finalContractAmt;
            set
            {
                if (value == _finalContractAmt) return;
                _finalContractAmt = value;
                OnPropertyChanged();
            }
        }

        public DateTime FormsRecD
        {
            get => _formsRecD;
            set
            {
                if (value == _formsRecD) return;
                _formsRecD = value;
                OnPropertyChanged();
            }
        }

        public DateTime FormsSent
        {
            get => _formsSent;
            set
            {
                if (value == _formsSent) return;
                _formsSent = value;
                OnPropertyChanged();
            }
        }

        public DateTime CertRecD
        {
            get => _certRecD;
            set
            {
                if (value == _certRecD) return;
                _certRecD = value;
                OnPropertyChanged();
            }
        }

        public bool ExemptionApproved
        {
            get => _exemptionApproved;
            set
            {
                if (value == _exemptionApproved) return;
                _exemptionApproved = value;
                OnPropertyChanged();
            }
        }

        public DateTime ExemptionAppDate
        {
            get => _exemptionAppDate;
            set
            {
                if (value == _exemptionAppDate) return;
                _exemptionAppDate = value;
                OnPropertyChanged();
            }
        }

        public string CrewEnrolled
        {
            get => _crewEnrolled;
            set
            {
                if (value == _crewEnrolled) return;
                _crewEnrolled = value;
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
    }
}
