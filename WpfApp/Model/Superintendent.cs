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
        private string _supPhone;
        private string _cellPhone;
        private string _email;
        private bool _active;

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

        public string SupPhone
        {
            get => _supPhone;
            set
            {
                if (value == _supPhone) return;
                _supPhone = value;
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
