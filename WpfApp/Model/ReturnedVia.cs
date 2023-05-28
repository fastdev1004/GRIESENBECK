using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReturnedVia:ViewModelBase
    {
        private int _returnedViaID;
        private string _returnedViaName;

        public int ID
        {
            get => _returnedViaID;
            set
            {
                if (value == _returnedViaID) return;
                _returnedViaID = value;
                OnPropertyChanged();
            }
        }

        public string ReturnedViaName
        {
            get => _returnedViaName;
            set
            {
                if (value == _returnedViaName) return;
                _returnedViaName = value;
                OnPropertyChanged();
            }
        }
    }
}
