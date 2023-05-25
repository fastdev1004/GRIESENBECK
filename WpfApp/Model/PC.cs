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
    class PC:ViewModelBase
    {
        private int _pcID;
        private string _pcName;

        public int ID
        {
            get => _pcID;
            set
            {
                if (value == _pcID) return;
                _pcID = value;
                OnPropertyChanged();
            }
        }

        public string PCName
        {
            get => _pcName;
            set
            {
                if (value == _pcName) return;
                _pcName = value;
                OnPropertyChanged();
            }
        }
    }
}