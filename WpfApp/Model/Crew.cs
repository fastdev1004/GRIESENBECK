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
    class Crew:ViewModelBase
    {
        private int _crewID;
        private string _crewName;
        private string _phone;
        private string _cell;
        private string _email;
        private bool _active;
        public int ID
        {
            get => _crewID;
            set
            {
                if (value == _crewID) return;
                _crewID = value;
                OnPropertyChanged();
            }
        }

        public string CrewName
        {
            get => _crewName;
            set
            {
                if (value == _crewName) return;
                _crewName = value;
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