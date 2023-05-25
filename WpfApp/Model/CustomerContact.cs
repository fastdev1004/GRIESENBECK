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
    class CustomerContact:ViewModelBase
    {
        private int _ccID;
        private int _customerID;
        private string __ccName;

        public int ID
        {
            get => _ccID;
            set
            {
                if (value == _ccID) return;
                _ccID = value;
                OnPropertyChanged();
            }
        }

        public int CustomerID
        {
            get => _customerID;
            set
            {
                if (value == _customerID) return;
                _customerID = value;
                OnPropertyChanged();
            }
        }

        public string CCName
        {
            get => __ccName;
            set
            {
                if (value == __ccName) return;
                __ccName = value;
                OnPropertyChanged();
            }
        }
    }
}