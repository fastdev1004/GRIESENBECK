using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ProjectWorkOrder:ViewModelBase
    {
        private int _projectID;
        private string _sovAcronym;
        private string _manufName;
        private string _matName;
        private bool _stock;
        private DateTime _matlReqd;
        private int _qtyReqd;
        private int _qtyOrd;
        private int _qtyRecvd;
        private int _matQty;

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

        public string SovAcronym
        {
            get => _sovAcronym;
            set
            {
                if (value == _sovAcronym) return;
                _sovAcronym = value;
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

        public string MatName
        {
            get => _matName;
            set
            {
                if (value == _matName) return;
                _matName = value;
                OnPropertyChanged();
            }
        }

        public bool Stock
        {
            get => _stock;
            set
            {
                if (value == _stock) return;
                _stock = value;
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

        public int QtyReqd
        {
            get => _qtyReqd;
            set
            {
                if (value == _qtyReqd) return;
                _qtyReqd = value;
                OnPropertyChanged();
            }
        }

        public int QtyOrd
        {
            get => _qtyOrd;
            set
            {
                if (value == _qtyOrd) return;
                _qtyOrd = value;
                OnPropertyChanged();
            }
        }

        public int QtyRecvd
        {
            get => _qtyRecvd;
            set
            {
                if (value == _qtyRecvd) return;
                _qtyRecvd = value;
                OnPropertyChanged();
            }
        }

        public int MatQty
        {
            get => _matQty;
            set
            {
                if (value == _matQty) return;
                _matQty = value;
                OnPropertyChanged();
            }
        }
    }
}
