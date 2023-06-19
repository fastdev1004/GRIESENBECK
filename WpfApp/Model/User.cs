using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class User:ViewModelBase
    {
        private int _id;
        private string _name;
        private string _personName;
        private int _level;
        private int _formOnOpen;
        private string _email;
        private bool _active;

        public int ID
        {
            get => _id;
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string PersonName
        {
            get => _personName;
            set
            {
                if (value == _personName) return;
                _personName = value;
                OnPropertyChanged();
            }
        }

        public int Level
        {
            get => _level;
            set
            {
                if (value == _level) return;
                _level = value;
                OnPropertyChanged();
            }
        }

        public int FormOnOpen
        {
            get => _formOnOpen;
            set
            {
                if (value == _formOnOpen) return;
                _formOnOpen = value;
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
