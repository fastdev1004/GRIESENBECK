using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class Superintendent:ViewModelBase
    {
        private int _supID;
        private string _supName;
        private string _cellPhone;
        private string _email;

        public int SupID
        {
            get => _supID;
            set
            {
                if (value == _supID) return;
                _supID = value;
                OnPropertyChanged();
            }
        }

        public string SupName
        {
            get => _supName;
            set
            {
                if (value == _supName) return;
                _supName = value;
                OnPropertyChanged();
            }
        }

        public string SupCellPhone
        {
            get => _cellPhone;
            set
            {
                if (value == _cellPhone) return;
                _cellPhone = value;
                OnPropertyChanged();
            }
        }

        public string SupEmail
        {
            get => _email;
            set
            {
                if (value == _email) return;
                _email = value;
                OnPropertyChanged();
            }
        }
    }
}
