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
        private int _id;
        private int _projectID;
        private string _sovAcronym;
        private int _laborID;
        private double _qtyReqd;
        private double _unitPrice;
        private double _total;
        private int _coItemNo;
        private int _coID;
        private string _phase;
        private bool _complete;
        private int _actionFlag;
        private int _projSovID;
        private int _projLabID;
        private int _labLineNo;
        private bool _matOnly;

        public int ID
        {
            get => _id;
            set
            {
                if (value == _id) return;
                _id = value;
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

        public string SovAcronymName
        {
            get => _sovAcronym;
            set
            {
                if (value == _sovAcronym) return;
                _sovAcronym = value;
                OnPropertyChanged();
            }
        }

        public int LaborID
        {
            get => _laborID;
            set
            {
                if (value == _laborID) return;
                _laborID = value;
                OnPropertyChanged();
            }
        }

        public double QtyReqd
        {
            get => _qtyReqd;
            set
            {
                if (value == _qtyReqd) return;
                _qtyReqd = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Total));
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
                OnPropertyChanged(nameof(Total));
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

        public bool MatOnly
        {
            get => _matOnly;
            set
            {
                if (value == _matOnly) return;
                _matOnly = value;
                OnPropertyChanged();
            }
        }

        public int LabLineNo
        {
            get => _labLineNo;
            set
            {
                if (value == _labLineNo) return;
                _labLineNo = value;
                OnPropertyChanged();
            }
        }

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

        public int ActionFlag
        {
            get => _actionFlag;
            set
            {
                if (value == _actionFlag) return;
                _actionFlag = value;
                OnPropertyChanged();
            }
        }
    }
}
