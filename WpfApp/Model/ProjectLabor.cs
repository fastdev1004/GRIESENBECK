using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ProjectLabor:ViewModelBase
    {
        private int _projectID;
        private string _sovAcronym;
        private string _labor;
        private int _qtyReqd;
        private double _unitPrice;
        private double _total;
        private int _changeOrder;
        private string _phase;

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

        public string Labor
        {
            get => _labor;
            set
            {
                if (value == _labor) return;
                _labor = value;
                OnPropertyChanged();
            }
        }

        public int QtyReqd
        {
            get => _qtyReqd;
            set
            {
                if (value == _qtyReqd) return;
                _qtyReqd = value;
                OnPropertyChanged();
            }
        }

        public double UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (value == _unitPrice) return;
                _unitPrice = value;
                OnPropertyChanged();
            }
        }

        public double Total
        {
            get => _qtyReqd * _unitPrice;
            set
            {
                if (value == _qtyReqd * _unitPrice) return;
                _total = value;
                OnPropertyChanged();
            }
        }

        public int ChangeOrder
        {
            get => _changeOrder;
            set
            {
                if (value == _changeOrder) return;
                _changeOrder = value;
                OnPropertyChanged();
            }
        }

        public string Phase
        {
            get => _phase;
            set
            {
                if (value == _phase) return;
                _phase = value;
                OnPropertyChanged();
            }
        }
    }
}
