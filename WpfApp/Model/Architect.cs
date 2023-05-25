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
    class Architect:ViewModelBase
    {
        private int _architectID;
        private string _archCompany;

        public int ID
        {
            get => _architectID;
            set
            {
                if (value == _architectID) return;
                _architectID = value;
                OnPropertyChanged();
            }
        }

        public string ArchCompany
        {
            get => _archCompany;
            set
            {
                if (value == _archCompany) return;
                _archCompany = value;
                OnPropertyChanged();
            }
        }

    }
}
