using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class WorkOrderLabor : ViewModelBase
    {
        private int _fetchID;
        private int _wodID;
        private int _woID;
        private int _projLabID;
        private int _coItemNo;
        private string _sovAcronymName;
        private string _labDesc;
        private string _labPhase;
        private float _labQty;
        private float _totalQty;
        private float _unitPrice;
        private float _total;
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

        public int ProjLabID
        {
            get => _projLabID;
            set
            {
                if (value == _projLabID) return;
                _projLabID = value;
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

        public string LabDesc
        {
            get => _labDesc;
            set
            {
                if (value == _labDesc) return;
                _labDesc = value;
                OnPropertyChanged();
            }
        }

        public string LabPhase
        {
            get => _labPhase;
            set
            {
                if (value == _labPhase) return;
                _labPhase = value;
                OnPropertyChanged();
            }
        }

        public float LabQty
        {
            get => _labQty;
            set
            {
                if (value == _labQty) return;
                _labQty = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Total));
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

        public float UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (value == _unitPrice) return;
                _unitPrice = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Total));
            }
        }

        public float Total
        {
            get => _labQty * _unitPrice;
            set
            {
                if (value == _labQty * _unitPrice) return;
                _total = value;
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