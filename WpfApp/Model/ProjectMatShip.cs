using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ProjectMatShip:ViewModelBase
    {
        private int _fetchID;
        private int _projMsID;
        private int _projMtID;
        private DateTime _schedShipDate;
        private DateTime _estDelivDate;
        private DateTime _estInstallDate;
        private string _estWeight;
        private int _estPallet;
        private int _freightCoID;
        private string _trackingNo;
        private DateTime _dateShipped;
        private double _qtyShipped;
        private int _noOfPallet;
        private DateTime _dateRecvd;
        private double _qtyRecvd;
        private bool _freightDamage;
        private int _projectID;
        private DateTime _deliverToJob;
        private string _siteStorage;
        private string _storageDetail;
        private int _actionFlag;

        public int FetchID
        {
            get => _fetchID;
            set
            {
                if (value == _fetchID) return;
                _fetchID = value;
                OnPropertyChanged();
            }
        }

        public int ProjMsID
        {
            get => _projMsID;
            set
            {
                if (value == _projMsID) return;
                _projMsID = value;
                OnPropertyChanged();
            }
        }

        public int ProjMtID
        {
            get => _projMtID;
            set
            {
                if (value == _projMtID) return;
                _projMtID = value;
                OnPropertyChanged();
            }
        }

        public DateTime SchedShipDate
        {
            get => _schedShipDate;
            set
            {
                if (value == _schedShipDate) return;
                _schedShipDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime EstDelivDate
        {
            get => _estDelivDate;
            set
            {
                if (value == _estDelivDate) return;
                _estDelivDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime EstInstallDate
        {
            get => _estInstallDate;
            set
            {
                if (value == _estInstallDate) return;
                _estInstallDate = value;
                OnPropertyChanged();
            }
        }

        public string EstWeight
        {
            get => _estWeight;
            set
            {
                if (value == _estWeight) return;
                _estWeight = value;
                OnPropertyChanged();
            }
        }

        public int EstPallet
        {
            get => _estPallet;
            set
            {
                if (value == _estPallet) return;
                _estPallet = value;
                OnPropertyChanged();
            }
        }

        public int FreightCoID
        {
            get => _freightCoID;
            set
            {
                if (value == _freightCoID) return;
                _freightCoID = value;
                OnPropertyChanged();
            }
        }

        public string TrackingNo
        {
            get => _trackingNo;
            set
            {
                if (value == _trackingNo) return;
                _trackingNo = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateShipped
        {
            get => _dateShipped;
            set
            {
                if (value == _dateShipped) return;
                _dateShipped = value;
                OnPropertyChanged();
            }
        }

        public double QtyShipped
        {
            get => _qtyShipped;
            set
            {
                if (value == _qtyShipped) return;
                _qtyShipped = value;
                OnPropertyChanged();
            }
        }

        public int NoOfPallet
        {
            get => _noOfPallet;
            set
            {
                if (value == _noOfPallet) return;
                _noOfPallet = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateRecvd
        {
            get => _dateRecvd;
            set
            {
                if (value == _dateRecvd) return;
                _dateRecvd = value;
                OnPropertyChanged();
            }
        }

        public double QtyRecvd
        {
            get => _qtyRecvd;
            set
            {
                if (value == _qtyRecvd) return;
                _qtyRecvd = value;
                OnPropertyChanged();
            }
        }

        public Boolean FreightDamage
        {
            get => _freightDamage;
            set
            {
                if (value == _freightDamage) return;
                _freightDamage = value;
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

        public DateTime DeliverToJob
        {
            get => _deliverToJob;
            set
            {
                if (value == _deliverToJob) return;
                _deliverToJob = value;
                OnPropertyChanged();
            }
        }

        public string SiteStorage
        {
            get => _siteStorage;
            set
            {
                if (value == _siteStorage) return;
                _siteStorage = value;
                OnPropertyChanged();
            }
        }

        public string StorageDetail
        {
            get => _storageDetail;
            set
            {
                if (value == _storageDetail) return;
                _storageDetail = value;
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