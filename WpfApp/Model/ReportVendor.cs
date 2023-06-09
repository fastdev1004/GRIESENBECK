using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportVendor : ViewModelBase
    {
        private int _manufID;
        private string _manufName;
        private string _contactName;
        private string _contactPhone;
        private string _contactEmail;

        public int ManufID
        {
            get => _manufID;
            set
            {
                if (value == _manufID) return;
                _manufID = value;
                OnPropertyChanged();
            }
        }

        public string ManufName
        {
            get => _manufName;
            set
            {
                if (value == _manufName) return;
                _manufName = value;
                OnPropertyChanged();
            }
        }

        public string ContactName
        {
            get => _contactName;
            set
            {
                if (value == _contactName) return;
                _contactName = value;
                OnPropertyChanged();
            }
        }

        public string ContactPhone
        {
            get => _contactPhone;
            set
            {
                if (value == _contactPhone) return;
                _contactPhone = value;
                OnPropertyChanged();
            }
        }

        public string ContactEmail
        {
            get => _contactEmail;
            set
            {
                if (value == _contactEmail) return;
                _contactEmail = value;
                OnPropertyChanged();
            }
        }
    }
}