using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class Manufacturer: ViewModelBase
    {
        private int _manufIF;
        private string _manufName;

        public int ID
        {
            get => _manufIF;
            set
            {
                if (value == _manufIF) return;
                _manufIF = value;
                OnPropertyChanged();
            }
        }

        public string ManufacturerName
        {
            get => _manufName;
            set
            {
                if (value == _manufName) return;
                _manufName = value;
                OnPropertyChanged();
            }
        }
    }
}
