using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class SovCO:ViewModelBase
    {
        private string _sovAcronym;
        private string _coItem;

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

        public string CoItem
        {
            get => _coItem;
            set
            {
                if (value == _coItem) return;
                _coItem = value;
                OnPropertyChanged();
            }
        }

        public string SovAcronymCoItem
        {
            get
            {
                return _sovAcronym + " | " + _coItem;
            }
        }
    }
}
