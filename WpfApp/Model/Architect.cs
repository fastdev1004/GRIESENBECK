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
    class Architect:ViewModelBase
    {
        private int _architectID;
        private string _archCompany;
        private string _archContact;
        private string _archAddress;
        private string _archCity;
        private string _archState;
        private string _archZip;
        private string _archPhone;
        private string _archFax;
        private string _archCell;
        private string _archEmail;
        private bool _active;

        public int ID
        {
            get => _architectID;
            set
            {
                if (value == _architectID) return;
                _architectID = value;
                OnPropertyChanged();
            }
        }

        public string ArchCompany
        {
            get => _archCompany;
            set
            {
                if (value == _archCompany) return;
                _archCompany = value;
                OnPropertyChanged();
            }
        }

        public string Contact
        {
            get => _archContact;
            set
            {
                if (value == _archContact) return;
                _archContact = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _archAddress;
            set
            {
                if (value == _archAddress) return;
                _archAddress = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get => _archCity;
            set
            {
                if (value == _archCity) return;
                _archCity = value;
                OnPropertyChanged();
            }
        }

        public string State
        {
            get => _archState;
            set
            {
                if (value == _archState) return;
                _archState = value;
                OnPropertyChanged();
            }
        }
        public string Zip
        {
            get => _archZip;
            set
            {
                if (value == _archZip) return;
                _archZip = value;
                OnPropertyChanged();
            }
        }
        public string Phone
        {
            get => _archPhone;
            set
            {
                if (value == _archPhone) return;
                _archPhone = value;
                OnPropertyChanged();
            }
        }
        public string Fax
        {
            get => _archFax;
            set
            {
                if (value == _archFax) return;
                _archFax = value;
                OnPropertyChanged();
            }
        }
        public string Cell
        {
            get => _archCell;
            set
            {
                if (value == _archCell) return;
                _archCell = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get => _archEmail;
            set
            {
                if (value == _archEmail) return;
                _archEmail = value;
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
