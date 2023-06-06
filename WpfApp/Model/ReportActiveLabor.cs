using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportActiveLabor : ViewModelBase
    {
        private int _laborID;
        private string _projectName;
        private string _jobNo;
        private string _customerName;
        private string _salesman;
        //private IObservable<string> _labors;

        public int ID
        {
            get => _laborID;
            set
            {
                if (value == _laborID) return;
                _laborID = value;
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

        private List<TrackLaborReport> _laborReport;

        public List<TrackLaborReport> LaborReports
        {
            get => _laborReport;
            set
            {
                if (value == _laborReport) return;
                _laborReport = value;
                OnPropertyChanged();
            }
        }
    }
}
