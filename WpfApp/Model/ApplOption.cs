using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ApplOption:ViewModelBase
    {
        private int _optionID;
        private string _optCat;
        private string _optTxtVal;
        private string _optNumVal;
        private string _optDesc;
        private string _optLong;

        public int OptID
        {
            get => _optionID;
            set
            {
                if (value == _optionID) return;
                _optionID = value;
                OnPropertyChanged();
            }
        }

        public string OptCat
        {
            get => _optCat;
            set
            {
                if (value == _optCat) return;
                _optCat = value;
                OnPropertyChanged();
            }
        }

        public string OptTxtVal
        {
            get => _optTxtVal;
            set
            {
                if (value == _optTxtVal) return;
                _optTxtVal = value;
                OnPropertyChanged();
            }
        }

        public string OptNumVal
        {
            get => _optNumVal;
            set
            {
                if (value == _optNumVal) return;
                _optNumVal = value;
                OnPropertyChanged();
            }
        }

        public string OptDesc
        {
            get => _optDesc;
            set
            {
                if (value == _optDesc) return;
                _optDesc = value;
                OnPropertyChanged();
            }
        }

        public string OptLong
        {
            get => _optLong;
            set
            {
                if (value == _optLong) return;
                _optLong = value;
                OnPropertyChanged();
            }
        }
    }
}