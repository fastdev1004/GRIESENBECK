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
    class Salesman:ViewModelBase
    {
        private int _salesmanID;
        private string _salesmanName;

        public int ID
        {
            get => _salesmanID;
            set
            {
                if (value == _salesmanID) return;
                    _salesmanID = value;
                OnPropertyChanged();
            }
        }

        public string SalesmanName
        {
            get => _salesmanName;
            set
            {
                if (value == _salesmanName) return;
                    _salesmanName = value;
                OnPropertyChanged();
            }
        }
    }
}