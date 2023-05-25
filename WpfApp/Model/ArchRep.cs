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
    }
}