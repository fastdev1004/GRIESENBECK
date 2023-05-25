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
        private int _projMtID;
        private DateTime _schedShipDate;
        private DateTime _estDelivDate;
        private DateTime _estInstallDate;
        private string _estWeight;
        private int _estPallet;
        private int _freightCoID;
        private string _trackingNo;
        private DateTime _dateShipped;
        private int _qtyShipped;
        private int _noOfPallet;
        private DateTime _dateRecvd;
        private int _qtyRecvd;
        private bool _freightDamage;
        private DateTime _deliverToJob;
        private string _siteStroage;
        private string _stroageDetail;

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

        public int QtyShipped
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

        public int QtyRecvd
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

        public string SiteStroage
        {
            get => _siteStroage;
            set
            {
                if (value == _siteStroage) return;
                _siteStroage = value;
                OnPropertyChanged();
            }
        }

        public string StroageDetail
        {
            get => _stroageDetail;
            set
            {
                if (value == _stroageDetail) return;
                _stroageDetail = value;
                OnPropertyChanged();
            }
        }
    }
}