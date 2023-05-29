using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class Labor:ViewModelBase
    {
        private int _laborID;
        private string _laborDesc;
        private double _laborUnitPrice;
        private bool _active;

        public int ID
        {
            get => _laborID;
            set
            {
                if (value == _laborID) return;
                _laborID = value;
                OnPropertyChanged();
            }
        }

        public string LaborDesc
        {
            get => _laborDesc;
            set
            {
                if (value == _laborDesc) return;
                _laborDesc = value;
                OnPropertyChanged();
            }
        }

        public double UnitPrice
        {
            get => _laborUnitPrice;
            set
            {
                if (value == _laborUnitPrice) return;
                _laborUnitPrice = value;
                OnPropertyChanged();
            }
        }

        public bool Active
        {
            get => _active;
            set
            {
                if (value == _active) return;
                _active = value;
                OnPropertyChanged();
            }
        }
    }
}