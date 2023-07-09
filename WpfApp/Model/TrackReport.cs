using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class TrackReport : ViewModelBase
    {
        private int _projMat;
        private DateTime _matReqdDate;
        private string _manufacturerName;
        private double _qtyOrd;
        private string _phase;
        private string _type;
        private string _manufLeadTime;
        private string _poNumber;
        private DateTime _shopReqDate;
        private DateTime _shopRecvdDate;
        private DateTime _submIssue;
        private DateTime _reSubmit;
        private DateTime _submAppr;
        private string _color;
        private bool _guarDim;
        private DateTime _fieldDim;
        private DateTime _rFF;
        private bool _orderComplete;
        private bool _laborComplete;
        private int _projectID;
        private int _projMtID;
        private int _manufacturer;
        private bool _takeFromStock;
        private string _orderNo;
        private DateTime _dateOrd;
        private bool _noSubm;
        private bool _shipToJob;
        private bool _needFM;
        private bool _finalsRev;

        public int ProjMat
        {
            get => _projMat;
            set
            {
                if (value == _projMat) return;
                _projMat = value;
                OnPropertyChanged();
            }
        }

        public DateTime MatReqdDate
        {
            get => _matReqdDate;
            set
            {
                if (value == _matReqdDate) return;
                _matReqdDate = value;
                OnPropertyChanged();
            }
        }

        public string ManufacturerName
        {
            get => _manufacturerName;
            set
            {
                if (value == _manufacturerName) return;
                _manufacturerName = value;
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

        public string Type
        {
            get => _type;
            set
            {
                if (value == _type) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        public DateTime ShopReqDate
        {
            get => _shopReqDate;
            set
            {
                if (value == _shopReqDate) return;
                _shopReqDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime ShopRecvdDate
        {
            get => _shopRecvdDate;
            set
            {
                if (value == _shopRecvdDate) return;
                _shopRecvdDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime SubmIssue
        {
            get => _submIssue;
            set
            {
                if (value == _submIssue) return;
                _submIssue = value;
                OnPropertyChanged();
            }
        }

        public DateTime ReSubmit
        {
            get => _reSubmit;
            set
            {
                if (value == _reSubmit) return;
                _reSubmit = value;
                OnPropertyChanged();
            }
        }

        public DateTime SubmAppr
        {
            get => _submAppr;
            set
            {
                if (value == _submAppr) return;
                _submAppr = value;
                OnPropertyChanged();
            }
        }

        public string Color
        {
            get => _color;
            set
            {
                if (value == _color) return;
                _color = value;
                OnPropertyChanged();
            }
        }

        public bool GuarDim
        {
            get => _guarDim;
            set
            {
                if (value == _guarDim) return;
                _guarDim = value;
                OnPropertyChanged();
            }
        }

        public DateTime FieldDim
        {
            get => _fieldDim;
            set
            {
                if (value == _fieldDim) return;
                _fieldDim = value;
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

        public bool OrderComplete
        {
            get => _orderComplete;
            set
            {
                if (value == _orderComplete) return;
                _orderComplete = value;
                OnPropertyChanged();
            }
        }

        public bool LaborComplete
        {
            get => _laborComplete;
            set
            {
                if (value == _laborComplete) return;
                _laborComplete = value;
                OnPropertyChanged();
            }
        }

        public int ProjMtID
        {
            get => _projMtID;
            set
            {
                if (value == _projMtID) return;
                _projMtID = value;
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

        public int ManufacturerID
        {
            get => _manufacturer;
            set
            {
                if (value == _manufacturer) return;
                _manufacturer = value;
                OnPropertyChanged();
            }
        }

        public bool TakeFromStock
        {
            get => _takeFromStock;
            set
            {
                if (value == _takeFromStock) return;
                _takeFromStock = value;
                OnPropertyChanged();
            }
        }


        public string ManuOrderNo
        {
            get => _orderNo;
            set
            {
                if (value == _orderNo) return;
                _orderNo = value;
                OnPropertyChanged();
            }
        }

        public bool NoSubm
        {
            get => _noSubm;
            set
            {
                if (value == _noSubm) return;
                _noSubm = value;
                OnPropertyChanged();
            }
        }
        public bool ShipToJob
        {
            get => _shipToJob;
            set
            {
                if (value == _shipToJob) return;
                _shipToJob = value;
                OnPropertyChanged();
            }
        }

        public bool NeedFM
        {
            get => _needFM;
            set
            {
                if (value == _needFM) return;
                _needFM = value;
                OnPropertyChanged();
            }
        }

        public bool FinalsRev
        {
            get => _finalsRev;
            set
            {
                if (value == _finalsRev) return;
                _finalsRev = value;
                OnPropertyChanged();
            }
        }

        public double QtyOrd
        {
            get => _qtyOrd;
            set
            {
                if (value == _qtyOrd) return;
                _qtyOrd = value;
                OnPropertyChanged();
            }
        }

        public string LeadTime
        {
            get => _manufLeadTime;
            set
            {
                if (value == _manufLeadTime) return;
                _manufLeadTime = value;
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
        public DateTime DateOrd
        {
            get => _dateOrd;
            set
            {
                if (value == _dateOrd) return;
                _dateOrd = value;
                OnPropertyChanged();
            }
        }
    }
}