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
    class ArchRep:ViewModelBase
    {
        private int _archRepID;
        private string _archRepName;
        private string _archRepEmail;
        private bool _active;

        public int ID
        {
            get => _archRepID;
            set
            {
                if (value == _archRepID) return;
                _archRepID = value;
                OnPropertyChanged();
            }
        }

        public string ArchRepName
        {
            get => _archRepName;
            set
            {
                if (value == _archRepName) return;
                _archRepName = value;
                OnPropertyChanged();
            }
        }

        public string ArchRepEmail
        {
            get => _archRepEmail;
            set
            {
                if (value == _archRepEmail) return;
                _archRepEmail = value;
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