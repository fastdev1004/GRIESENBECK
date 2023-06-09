using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportShipNotRecv : ViewModelBase
    {
        private int _projectID;
        private DateTime _matlReqd;
        private string _jobNo;
        private string _projectName;
        private string _salesmanName;
        private string _matType;
        private string _phase;
        private string _manufName;
        private string _poNumber;
        private DateTime _schedShip;
        private DateTime _estDeliv;

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

        public string MatType
        {
            get => _matType;
            set
            {
                if (value == _matType) return;
                _matType = value;
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

        public DateTime SchedShip
        {
            get => _schedShip;
            set
            {
                if (value == _schedShip) return;
                _schedShip = value;
                OnPropertyChanged();
            }
        }

        public DateTime EstDeliv
        {
            get => _estDeliv;
            set
            {
                if (value == _estDeliv) return;
                _estDeliv = value;
                OnPropertyChanged();
            }
        }
    }
}