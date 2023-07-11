using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportContract : ViewModelBase
    {
        private int _projectID;
        private string _projectName;
        private string _jobNo;
        private string _salesmanName;
        private string _customerName;
        private List<Contract> _contract;
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

        public List<Contract> Contracts
        {
            get => _contract;
            set
            {
                if (value == _contract) return;
                _contract = value;
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
    }
}