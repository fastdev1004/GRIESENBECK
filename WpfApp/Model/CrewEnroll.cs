using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class CrewEnroll : ViewModelBase
    {
        private string _crewEnrollValue;

        public string CrewEnrollValue
        {
            get => _crewEnrollValue;
            set
            {
                if (value == _crewEnrollValue) return;
                _crewEnrollValue = value;
                OnPropertyChanged();
            }
        }
    }
}