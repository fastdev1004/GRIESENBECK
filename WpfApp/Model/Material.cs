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
        private string _materialDesc;

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
    }
}
