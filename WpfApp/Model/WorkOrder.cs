using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class WorkOrder : ViewModelBase
    {
        private int _wodID;
        private int _woID;
        private int _projectID;
        private int _woNumber;
        private int _crewID;
        private string _crewName;
        private int _suptID;
        private DateTime _dateStarted;
        private DateTime _dateCompleted;
        private DateTime _schedStartDate;
        private DateTime _schedComplDate;
        private bool _complete;

        public int WodID
        {
            get => _wodID;
            set
            {
                if (value == _wodID) return;
                _wodID = value;
                OnPropertyChanged();
            }
        }

        public int WoID
        {
            get => _woID;
            set
            {
                if (value == _woID) return;
                _woID = value;
                OnPropertyChanged();
            }
        }

        public int WoNumber
        {
            get => _woNumber;
            set
            {
                if (value == _woNumber) return;
                _woNumber = value;
                OnPropertyChanged();
            }
        }

        public int ProjectID
        {
            get => _projectID;
            set
            {
                if (value == _projectID) return;
                _projectID = value;
                OnPropertyChanged();
            }
        }

        public int CrewID
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

        public int SuptID
        {
            get => _suptID;
            set
            {
                if (value == _suptID) return;
                _suptID = value;
                OnPropertyChanged();
            }
        }

        public DateTime SchedStartDate
        {
            get => _schedStartDate;
            set
            {
                if (value == _schedStartDate) return;
                _schedStartDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime SchedComplDate
        {
            get => _schedComplDate;
            set
            {
                if (value == _schedComplDate) return;
                _schedComplDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateStarted
        {
            get => _dateStarted;
            set
            {
                if (value == _dateStarted) return;
                _dateStarted = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateCompleted
        {
            get => _dateCompleted;
            set
            {
                if (value == _dateCompleted) return;
                _dateCompleted = value;
                OnPropertyChanged();
            }
        }

        public bool Complete
        {
            get => _complete;
            set
            {
                if (value == _complete) return;
                _complete = value;
                OnPropertyChanged();
            }
        }
    }
}
