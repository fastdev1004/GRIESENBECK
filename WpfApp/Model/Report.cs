using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class Report:ViewModelBase
    {
        private int _reportID;
        private string _reportName;
        private string _reportType;
        private string _reportObjectName;
        private string _reportDateName;
        private string _reportDateOnTbl;
        private bool _active;

        public int ID
        {
            get => _reportID;
            set
            {
                if (value == _reportID) return;
                _reportID = value;
                OnPropertyChanged();
            }
        }

        public string ReportName
        {
            get => _reportName;
            set
            {
                if (value == _reportName) return;
                _reportName = value;
                OnPropertyChanged();
            }
        }

        public string ReportType
        {
            get => _reportType;
            set
            {
                if (value == _reportType) return;
                _reportType = value;
                OnPropertyChanged();
            }
        }

        public string ReportObjectName
        {
            get => _reportObjectName;
            set
            {
                if (value == _reportObjectName) return;
                _reportObjectName = value;
                OnPropertyChanged();
            }
        }

        public string ReportDateName
        {
            get => _reportDateName;
            set
            {
                if (value == _reportDateName) return;
                _reportDateName = value;
                OnPropertyChanged();
            }
        }

        public string ReportDateOnTbl
        {
            get => _reportDateOnTbl;
            set
            {
                if (value == _reportDateOnTbl) return;
                _reportDateOnTbl = value;
                OnPropertyChanged();
            }
        }

        public bool Active
        {
            get => _active;
            set
            {
                if (value == _active) return;
                _active = value;
                OnPropertyChanged();
            }
        }
    }
}
