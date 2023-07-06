using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class TrackLaborReport : ViewModelBase
    {
        private int _projLabID;
        private int _projSovID;
        private int _projectID;
        private int _coID;
        private string _sovAcronym;
        private int _coItemNo;
        private string _laborDesc;
        private string _labPhase;
        private bool _complete;
        private DateTime _targetDate;

        public int ProjLabID
        {
            get => _projLabID;
            set
            {
                if (value == _projLabID) return;
                _projLabID = value;
                OnPropertyChanged();
            }
        }

        public int ProjSovID
        {
            get => _projSovID;
            set
            {
                if (value == _projSovID) return;
                _projSovID = value;
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

        public int CoID
        {
            get => _coID;
            set
            {
                if (value == _coID) return;
                _coID = value;
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

        public int CoItemNo
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
