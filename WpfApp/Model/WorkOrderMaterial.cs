using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class WorkOrderMaterial : ViewModelBase
    {
        private int _fetchID;
        private int _wodID;
        private int _woID;
        private int _projMsID;
        private string _sovAcronymName;
        private string _matName;
        private string _manufName;
        private float _matQty;
        private float _totalQty;
        private int _coItemNo;
        private DateTime _shipDate;
        private int _actionFlag;

        public int FetchID
        {
            get => _fetchID;
            set
            {
                if (value == _fetchID) return;
                _fetchID = value;
                OnPropertyChanged();
            }
        }

        public int WodID
        {
            get => _wodID;
            set
            {
                if (value == _wodID) return;
                _wodID = value;
                OnPropertyChanged();
            }
        }

        public int WoID
        {
            get => _woID;
            set
            {
                if (value == _woID) return;
                _woID = value;
                OnPropertyChanged();
            }
        }

        public int ProjMsID
        {
            get => _projMsID;
            set
            {
                if (value == _projMsID) return;
                _projMsID = value;
                OnPropertyChanged();
            }
        }

        public string SovAcronymName
        {
            get => _sovAcronymName;
            set
            {
                if (value == _sovAcronymName) return;
                _sovAcronymName = value;
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

        public float MatQty
        {
            get => _matQty;
            set
            {
                if (value == _matQty) return;
                _matQty = value;
                OnPropertyChanged();
            }
        }

        public float TotalQty
        {
            get => _totalQty;
            set
            {
                if (value == _totalQty) return;
                _totalQty = value;
                OnPropertyChanged();
            }
        }

        public int CoItemNo
        {
            get => _coItemNo;
            set
            {
                if (value == _coItemNo) return;
                _coItemNo = value;
                OnPropertyChanged();
            }
        }

        public DateTime ShipDate
        {
            get => _shipDate;
            set
            {
                if (value == _shipDate) return;
                _shipDate = value;
                OnPropertyChanged();
            }
        }

        public int ActionFlag
        {
            get => _actionFlag;
            set
            {
                if (value == _actionFlag) return;
                _actionFlag = value;
                OnPropertyChanged();
            }
        }
    }
}