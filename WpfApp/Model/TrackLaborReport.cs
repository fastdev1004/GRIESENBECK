using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class TrackLaborReport:ViewModelBase
    {
        private int _projectID;
        private string _sovAcronym;
        private string _laborDesc;
        private string _coItemNo;
        private string _labPhase;
        private bool _complete;
        private DateTime _targetDate;

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

        public DateTime TargetDate
        {
            get => _targetDate;
            set
            {
                if (value == _targetDate) return;
                _targetDate = value;
                OnPropertyChanged();
            }
        }

        public string SovAcronym
        {
            get => _sovAcronym;
            set
            {
                if (value == _sovAcronym) return;
                _sovAcronym = value;
                OnPropertyChanged();
            }
        }

        public string LaborDesc
        {
            get => _laborDesc;
            set
            {
                if (value == _laborDesc) return;
                _laborDesc = value;
                OnPropertyChanged();
            }
        }

        public string CoItemNo
        {
            get => _coItemNo;
            set
            {
                if (value == _coItemNo) return;
                _coItemNo = value;
                OnPropertyChanged();
            }
        }

        public string LabPhase
        {
            get => _labPhase;
            set
            {
                if (value == _labPhase) return;
                _labPhase = value;
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
