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
        private string _ccName;
        private string _ccPhone;
        private string _ccCell;
        private string _ccEmail;
        private bool _ccActive;

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
            get => _ccName;
            set
            {
                if (value == _ccName) return;
                _ccName = value;
                OnPropertyChanged();
            }
        }

        public string CCPhone
        {
            get => _ccPhone;
            set
            {
                if (value == _ccPhone) return;
                _ccPhone = value;
                OnPropertyChanged();
            }
        }

        public string CCCell
        {
            get => _ccCell;
            set
            {
                if (value == _ccCell) return;
                _ccCell = value;
                OnPropertyChanged();
            }
        }

        public string CCEmail
        {
            get => _ccEmail;
            set
            {
                if (value == _ccEmail) return;
                _ccEmail = value;
                OnPropertyChanged();
            }
        }

        public bool CCActive
        {
            get => _ccActive;
            set
            {
                if (value == _ccActive) return;
                _ccActive = value;
                OnPropertyChanged();
            }
        }
    }
}