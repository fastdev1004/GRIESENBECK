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
    class Customer:ViewModelBase
    {
        private int _customerID;
        private string _customerAddress;
        private string _customerName;

        public int ID
        {
            get => _customerID;
            set
            {
                if (value == _customerID) return;
                _customerID = value;
                OnPropertyChanged();
            }
        }

        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (value == _customerName) return;
                _customerName = value;
                OnPropertyChanged();
            }
        }

        public string CustomerAddress
        {
            get => _customerAddress;
            set
            {
                if (value == _customerAddress) return;
                _customerAddress = value;
                OnPropertyChanged();
            }
        }

        public string Customer_Name_Address
        {
            get
            {
                return _customerName + " | " + _customerAddress;
            }
        }
    }
}
