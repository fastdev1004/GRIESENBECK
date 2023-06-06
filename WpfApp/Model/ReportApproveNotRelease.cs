using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportApproveNotRelease : ViewModelBase
    {
        private int _projectID;
        private DateTime _matReqDate;
        private string _jobNo;
        private string _projectName;
        private string _salesmanName;
        private string _materialName;
        private string _manufName;
        private DateTime _submAppr;
        private string _leadTime;


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

        public DateTime MatReqDate
        {
            get => _matReqDate;
            set
            {
                if (value == _matReqDate) return;
                _matReqDate = value;
                OnPropertyChanged();
            }
        }

        public string JobNo
        {
            get => _jobNo;
            set
            {
                if (value == _jobNo) return;
                _jobNo = value;
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

        public string LeadTime
        {
            get => _leadTime;
            set
            {
                if (value == _leadTime) return;
                _leadTime = value;
                OnPropertyChanged();
            }
        }
    }
}