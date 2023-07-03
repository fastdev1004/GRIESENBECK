using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class SovMaterial : ViewModelBase
    {
        private int _id;
        private string _acronymName;
        private int _coID;
        private int _coItemNo;
        private bool _matOnly;
        private int _matID;
        private string _matPhase;
        private string _matType;
        private string _color;
        private int _qtyReqd;
        private double _totalCost;
        private int _actionFlag;
        private int _projSovID;
        private int _matLineNo;
        private bool _matLot;
        private double _matOrigRate;
        private int _projMatID;

        public int ProjMatID
        {
            get => _projMatID;
            set
            {
                if (value == _projMatID) return;
                _projMatID = value;
                OnPropertyChanged();
            }
        }

        public double MatOrigRate
        {
            get => _matOrigRate;
            set
            {
                if (value == _matOrigRate) return;
                _matOrigRate = value;
                OnPropertyChanged();
            }
        }

        public bool MatLot
        {
            get => _matLot;
            set
            {
                if (value == _matLot) return;
                _matLot = value;
                OnPropertyChanged();
            }
        }

        public int MatLineNo
        {
            get => _matLineNo;
            set
            {
                if (value == _matLineNo) return;
                _matLineNo = value;
                OnPropertyChanged();
            }
        }

        public int ProjSovID
        {
            get => _projSovID;
            set
            {
                if (value == _projSovID) return;
                _projSovID = value;
                OnPropertyChanged();
            }
        }


        public int ID
        {
            get => _id;
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public int ActionFlag
        {
            get => _actionFlag;
            set
            {
                // 0: init, 1: insert, 2: update, 3: delete
                if (value == _actionFlag) return;
                _actionFlag = value;
                OnPropertyChanged();
            }
        }

        public string SovAcronymName
        {
            get => _acronymName;
            set
            {
                if (value == _acronymName) return;
                _acronymName = value;
                OnPropertyChanged();
            }
        }

        public int CoID
        {
            get => _coID;
            set
            {
                if (value == _coID) return;
                _coID = value;
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

        public bool MatOnly
        {
            get => _matOnly;
            set
            {
                if (value == _matOnly) return;
                _matOnly = value;
                OnPropertyChanged();
            }
        }

        public int MatID
        {
            get => _matID;
            set
            {
                if (value == _matID) return;
                _matID = value;
                OnPropertyChanged();
            }
        }

        public string MatPhase
        {
            get => _matPhase;
            set
            {
                if (value == _matPhase) return;
                _matPhase = value;
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

        public double TotalCost
        {
            get => _totalCost;
            set
            {
                if (value == _totalCost) return;
                _totalCost = value;
                OnPropertyChanged();
            }
        }
    }
}