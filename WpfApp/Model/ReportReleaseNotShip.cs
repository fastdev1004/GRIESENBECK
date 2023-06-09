using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportReleaseNotShip : ViewModelBase
    {
        private int _projectID;
        private DateTime _matlReqd;
        private string _jobNo;
        private string _projectName;
        private string _salesmanName;
        private string _materialName;
        private string _phase;
        private string _manufName;
        private string _poNumber;
        private DateTime _rFF;

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

        public string MaterialName
        {
            get => _materialName;
            set
            {
                if (value == _materialName) return;
                _materialName = value;
                OnPropertyChanged();
            }
        }

        public string Phase
        {
            get => _phase;
            set
            {
                if (value == _phase) return;
                _phase = value;
                OnPropertyChanged();
            }
        }

        public string ManufName
        {
            get => _manufName;
            set
            {
                if (value == _manufName) return;
                _manufName = value;
                OnPropertyChanged();
            }
        }

        public string PoNumber
        {
            get => _poNumber;
            set
            {
                if (value == _poNumber) return;
                _poNumber = value;
                OnPropertyChanged();
            }
        }

        public DateTime MatlReqd
        {
            get => _matlReqd;
            set
            {
                if (value == _matlReqd) return;
                _matlReqd = value;
                OnPropertyChanged();
            }
        }

        public DateTime RFF
        {
            get => _rFF;
            set
            {
                if (value == _rFF) return;
                _rFF = value;
                OnPropertyChanged();
            }
        }
    }
}