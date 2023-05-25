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
    class Crew:ViewModelBase
    {
        private int _crewID;
        private string _crewName;

        public int ID
        {
            get => _crewID;
            set
            {
                if (value == _crewID) return;
                _crewID = value;
                OnPropertyChanged();
            }
        }

        public string CrewName
        {
            get => _crewName;
            set
            {
                if (value == _crewName) return;
                _crewName = value;
                OnPropertyChanged();
            }
        }
    }
}