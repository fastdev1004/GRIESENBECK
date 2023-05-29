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
    class Salesman:ViewModelBase
    {
        private int _salesmanID;
        private string _init;
        private string _salesmanName;
        private string _phone;
        private string _cell;
        private string _email;
        private bool _active;

        public int ID
        {
            get => _salesmanID;
            set
            {
                if (value == _salesmanID) return;
                    _salesmanID = value;
                OnPropertyChanged();
            }
        }

        public string SalesmanName
        {
            get => _salesmanName;
            set
            {
                if (value == _salesmanName) return;
                    _salesmanName = value;
                OnPropertyChanged();
            }
        }
    
        public string Init
        {
            get => _init;
            set
            {
                if (value == _init) return;
                _init = value;
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
        public string Cell
        {
            get => _cell;
            set
            {
                if (value == _cell) return;
                _cell = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                if (value == _email) return;
                _email = value;
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