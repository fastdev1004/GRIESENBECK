using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class FreightCo:ViewModelBase
    {
        private int _freightCoID;
        private string _freightName;

        public int FreightCoID
        {
            get => _freightCoID;
            set
            {
                if (value == _freightCoID) return;
                _freightCoID = value;
                OnPropertyChanged();
            }
        }

        public string FreightName
        {
            get => _freightName;
            set
            {
                if (value == _freightName) return;
                _freightName = value;
                OnPropertyChanged();
            }
        }
    }
}
