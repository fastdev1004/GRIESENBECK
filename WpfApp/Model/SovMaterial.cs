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
        private string _acronymName;
        private string _acronymDesc;
        private int _coItemNo;
        private bool _matOnly;
        private string _matPhase;
        private string _matType;
        private string _color;
        private int _qtyReqd;
        private double _totalCost;

        public string AcronymName
        {
            get => _acronymName;
            set
            {
                if (value == _acronymName) return;
                _acronymName = value;
                OnPropertyChanged();
            }
        }

        public string AcronymDesc
        {
            get => _acronymDesc;
            set
            {
                if (value == _acronymDesc) return;
                _acronymDesc = value;
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