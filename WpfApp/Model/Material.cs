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
    class Material:ViewModelBase
    {
        private int _materialID;
        private string _materialCode;
        private string _materialDesc;
        private bool _active;

        public int ID
        {
            get => _materialID;
            set
            {
                if (value == _materialID) return;
                _materialID = value;
                OnPropertyChanged();
            }
        }

        public string MatCode
        {
            get => _materialCode;
            set
            {
                if (value == _materialCode) return;
                _materialCode = value;
                OnPropertyChanged();
            }
        }

        public string MatDesc
        {
            get => _materialDesc;
            set
            {
                if (value == _materialDesc) return;
                _materialDesc = value;
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
