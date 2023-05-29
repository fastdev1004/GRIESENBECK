using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class FreightCo:ViewModelBase
    {
        private int _freightCoID;
        private string _freightName;
        private string _phone;
        private string _email;
        private string _contact;
        private string _cell;
        private bool _active;

        public int ID
        {
            get => _freightCoID;
            set
            {
                if (value == _freightCoID) return;
                _freightCoID = value;
                OnPropertyChanged();
            }
        }

        public string FreightName
        {
            get => _freightName;
            set
            {
                if (value == _freightName) return;
                _freightName = value;
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

        public string Contact
        {
            get => _contact;
            set
            {
                if (value == _contact) return;
                _contact = value;
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
