using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportShopReq : ViewModelBase
    {
        private int _projectID;
        private DateTime _matlReqd;
        private string _jobNO;
        private string _projectName;
        private string _salesmanName;
        private string _materialName;
        private string _manufName;
        private DateTime _shopRecvd;
        private DateTime _shopReq;

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

        public DateTime ShopRecvd
        {
            get => _shopRecvd;
            set
            {
                if (value == _shopRecvd) return;
                _shopRecvd = value;
                OnPropertyChanged();
            }
        }

        public DateTime ShopReq
        {
            get => _shopReq;
            set
            {
                if (value == _shopReq) return;
                _shopReq = value;
                OnPropertyChanged();
            }
        }
    }
}