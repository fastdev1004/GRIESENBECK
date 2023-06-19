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
    class Acronym:ViewModelBase
    {
        private int _acronymID;
        private string _acronymName;
        private string _acronymDesc;
        private bool _active;

        public int ID
        {
            get => _acronymID;
            set
            {
                if (value == _acronymID) return;
                _acronymID = value;
                OnPropertyChanged();
            }
        }

        public string AcronymName
        {
            get => _acronymName;
            set
            {
                if (value == _acronymName) return;
                _acronymName = value;
                OnPropertyChanged();
            }
        }

        public string AcronymDesc
        {
            get => _acronymDesc;
            set
            {
                if (value == _acronymDesc) return;
                _acronymDesc = value;
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
