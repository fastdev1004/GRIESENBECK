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
        private int _coItemNo;
        private string _sovDesc;
        private bool _matOnly;

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

        public string SovDesc
        {
            get => _sovDesc;
            set
            {
                if (value == _sovDesc) return;
                _sovDesc = value;
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

        public string SovAcronymCoItem
        {
            get
            {
                return _sovAcronym + " | " + _sovDesc;
            }
        }
    }
}
