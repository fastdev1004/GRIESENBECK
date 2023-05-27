using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class TrackReport:ViewModelBase
    {
        private int _projMat;
        private DateTime _matReqdDate;
        private string _manufacturerName;
        private string _qtyOrd;
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
        private bool _laborComplete;

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

        public string QtyOrd
        {
            get => _qtyOrd;
            set
            {
                if (value == _qtyOrd) return;
                _qtyOrd = value;
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

        public string ManufLeadTime
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
    }
}