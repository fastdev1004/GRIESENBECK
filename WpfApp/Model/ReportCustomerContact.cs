using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportCustomerContact:ViewModelBase
    {
        private int _customerID;
        private string _fullName;
        private List<ProjectManager> _projectManagers;


        public int CustomerID
        {
            get => _customerID;
            set
            {
                if (value == _customerID) return;
                _customerID = value;
                OnPropertyChanged();
            }
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                if (value == _fullName) return;
                _fullName = value;
                OnPropertyChanged();
            }
        }

        public List<ProjectManager> ProjectManagers
        {
            get => _projectManagers;
            set
            {
                if (value == _projectManagers) return;
                _projectManagers = value;
                OnPropertyChanged();
            }
        }
    }
}
