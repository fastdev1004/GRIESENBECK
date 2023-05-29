using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class Manufacturer : ViewModelBase
    {
        private int _manufID;
        private string _manufName;
        private string _address;
        private string _address2;
        private string _city;
        private string _state;
        private string _zip;
        private string _phone;
        private string _fax;
        private string _contactName;
        private string _contactPhone;
        private string _contactEmail;
        private bool _active;

        public int ID
        {
            get => _manufID;
            set
            {
                if (value == _manufID) return;
                _manufID = value;
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

        public string Address
        {
            get => _address;
            set
            {
                if (value == _address) return;
                _address = value;
                OnPropertyChanged();
            }
        }

        public string Address2
        {
            get => _address2;
            set
            {
                if (value == _address2) return;
                _address2 = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get => _city;
            set
            {
                if (value == _city) return;
                _city = value;
                OnPropertyChanged();
            }
        }

        public string State
        {
            get => _state;
            set
            {
                if (value == _state) return;
                _state = value;
                OnPropertyChanged();
            }
        }

        public string Zip
        {
            get => _zip;
            set
            {
                if (value == _zip) return;
                _zip = value;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (value == _phone) return;
                _phone = value;
                OnPropertyChanged();
            }
        }

        public string Fax
        {
            get => _fax;
            set
            {
                if (value == _fax) return;
                _fax = value;
                OnPropertyChanged();
            }
        }

        public string ContactName
        {
            get => _contactName;
            set
            {
                if (value == _contactName) return;
                _contactName = value;
                OnPropertyChanged();
            }
        }

        public string ContactPhone
        {
            get => _contactPhone;
            set
            {
                if (value == _contactPhone) return;
                _contactPhone = value;
                OnPropertyChanged();
            }
        }

        public string ContactEmail
        {
            get => _contactEmail;
            set
            {
                if (value == _contactEmail) return;
                _contactEmail = value;
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
