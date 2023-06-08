using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportActiveProject : ViewModelBase
    {
        private int _projectID;
        private string _projectName;
        private string _jobNo;
        private string _customerName;
        private string _salesman;
        private List<ProjectMatTracking> _projectMatTracking;

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

        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (value == _customerName) return;
                _customerName = value;
                OnPropertyChanged();
            }
        }

        public string SalesmanName
        {
            get => _salesman;
            set
            {
                if (value == _salesman) return;
                _salesman = value;
                OnPropertyChanged();
            }
        }

        public List<ProjectMatTracking> ProjectMatTrackings
        {
            get => _projectMatTracking;
            set
            {
                if (value == _projectMatTracking) return;
                _projectMatTracking = value;
                OnPropertyChanged();
            }
        }
    }
}