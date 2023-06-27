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
    class Estimator:ViewModelBase
    {
        private int _estimatorID;
        private string _initial;
        private string _estimatorName;
        private string _cell;
        private string _email;
        private bool _active;

        public int ID
        {
            get => _estimatorID;
            set
            {
                if (value == _estimatorID) return;
                _estimatorID = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _estimatorName;
            set
            {
                if (value == _estimatorName) return;
                _estimatorName = value;
                OnPropertyChanged();
            }
        }

        public string Initial
        {
            get => _initial;
            set
            {
                if (value == _initial) return;
                _initial = value;
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
    