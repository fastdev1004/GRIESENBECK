using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportSubmit : ViewModelBase
    {
        private int _projectID;
        private DateTime _matlReqd;
        private string _jobNO;
        private string _projectName;
        private string _salesmanName;
        private string _materialName;
        private string _manufName;
        private DateTime _submIssue;
        private DateTime _reSubmit;
        private DateTime _submAppr;

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

        public DateTime MatlReqd
        {
            get => _matlReqd;
            set
            {
                if (value == _matlReqd) return;
                _matlReqd = value;
                OnPropertyChanged();
            }
        }

        public string JobNo
        {
            get => _jobNO;
            set
            {
                if (value == _jobNO) return;
                _jobNO = value;
                OnPropertyChanged();
            }
        }

        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (value == _projectName) return;
                _projectName = value;
                OnPropertyChanged();
            }
        }

        public string MaterialName
        {
            get => _materialName;
            set
            {
                if (value == _materialName) return;
                _materialName = value;
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

        public string SalesmanName
        {
            get => _salesmanName;
            set
            {
                if (value == _salesmanName) return;
                _salesmanName = value;
                OnPropertyChanged();
            }
        }

        public DateTime SubmIssue
        {
            get => _submIssue;
            set
            {
                if (value == _submIssue) return;
                _submIssue = value;
                OnPropertyChanged();
            }
        }

        public DateTime ReSubmit
        {
            get => _reSubmit;
            set
            {
                if (value == _reSubmit) return;
                _reSubmit = value;
                OnPropertyChanged();
            }
        }

        public DateTime SubmAppr
        {
            get => _submAppr;
            set
            {
                if (value == _submAppr) return;
                _submAppr = value;
                OnPropertyChanged();
            }
        }
    }
}