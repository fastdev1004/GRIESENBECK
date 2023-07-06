using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    public class Project:ViewModelBase
    {
        private int _id;
        private string _projectName;
        private DateTime _targetDate;
        private DateTime _dateCompleted;
        private bool _complete;
        private string _architect;
        private string _pmName;
        private int _estimatorID;
        private int _customerID;
        private int _projectCoordID;
        private int _archRepID;
        private int _submittalContactID;
        private int _architectID;
        private int _billingDate;

        private bool _backgroundChk;
        private bool _cip;
        private bool _c3;
        private bool _certPayroll;
        private bool _pnpBond;
        private bool _gapBid;
        private bool _gapEst;
        private bool _lcpTracker;
        private bool _payReqd;

        private bool _contractRecvd;
        private bool _storedMat;
        private string _payReqdNote;
        private string _addtlInfo;
        private string _masterContract;
        private int _crewID;
        private string _jobNo;
        private bool _onHold;
        private string _address;
        private string _city;
        private string _state;
        private string _zip;
        private string _safetyBadging;

        private int _origContractAmt;
        private int _totalChangerOrder;
        private int _totalContractAmt;

        public int TotalContractAmt
        {
            get => _totalContractAmt;
            set
            {
                if (value == _totalContractAmt) return;
                _totalContractAmt = value;
                OnPropertyChanged();
            }
        }

        public int TotalChangerOrder
        {
            get => _totalChangerOrder;
            set
            {
                if (value == _totalChangerOrder) return;
                _totalChangerOrder = value;
                OnPropertyChanged();
            }
        }

        public int OrigContractAmt
        {
            get => _origContractAmt;
            set
            {
                if (value == _origContractAmt) return;
                _origContractAmt = value;
                OnPropertyChanged();
            }
        }

        public bool BackgroundChk
        {
            get => _backgroundChk;
            set
            {
                if (value == _backgroundChk) return;
                _backgroundChk = value;
                OnPropertyChanged();
            }
        }

        public bool Cip
        {
            get => _cip;
            set
            {
                if (value == _cip) return;
                _cip = value;
                OnPropertyChanged();
            }
        }

        public bool C3
        {
            get => _c3;
            set
            {
                if (value == _c3) return;
                _c3 = value;
                OnPropertyChanged();
            }
        }

        public bool CertPayRoll
        {
            get => _certPayroll;
            set
            {
                if (value == _certPayroll) return;
                _certPayroll = value;
                OnPropertyChanged();
            }
        }

        public bool PnpBond
        {
            get => _pnpBond;
            set
            {
                if (value == _pnpBond) return;
                _pnpBond = value;
                OnPropertyChanged();
            }
        }

        public bool GapBid
        {
            get => _gapBid;
            set
            {
                if (value == _gapBid) return;
                _gapBid = value;
                OnPropertyChanged();
            }
        }

        public bool GapEst
        {
            get => _gapEst;
            set
            {
                if (value == _gapEst) return;
                _gapEst = value;
                OnPropertyChanged();
            }
        }
        public bool LcpTracker
        {
            get => _lcpTracker;
            set
            {
                if (value == _lcpTracker) return;
                _lcpTracker = value;
                OnPropertyChanged();
            }
        }

        public bool PayReqd
        {
            get => _payReqd;
            set
            {
                if (value == _payReqd) return;
                _payReqd = value;
                OnPropertyChanged();
            }
        }



        public string SafetyBadging
        {
            get => _safetyBadging;
            set
            {
                if (value == _safetyBadging) return;
                _safetyBadging = value;
                OnPropertyChanged();
            }
        }

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

        public int ProjectCoordID
        {
            get => _projectCoordID;
            set
            {
                if (value == _projectCoordID) return;
                _projectCoordID = value;
                OnPropertyChanged();
            }
        }

        public int ArchRepID
        {
            get => _archRepID;
            set
            {
                if (value == _archRepID) return;
                _archRepID = value;
                OnPropertyChanged();
            }
        }

        public int SubmittalContactID
        {
            get => _submittalContactID;
            set
            {
                if (value == _submittalContactID) return;
                _submittalContactID = value;
                OnPropertyChanged();
            }
        }

        public int ArchitectID
        {
            get => _architectID;
            set
            {
                if (value == _architectID) return;
                _architectID = value;
                OnPropertyChanged();
            }
        }

        public int EstimatorID
        {
            get => _estimatorID;
            set
            {
                if (value == _estimatorID) return;
                _estimatorID = value;
                OnPropertyChanged();
            }
        }

        public int CrewID
        {
            get => _crewID;
            set
            {
                if (value == _crewID) return;
                _crewID = value;
                OnPropertyChanged();
            }
        }

        public string MasterContract
        {
            get => _masterContract;
            set
            {
                if (value == _masterContract) return;
                _masterContract = value;
                OnPropertyChanged();
            }
        }

        public string AddtlInfo
        {
            get => _addtlInfo;
            set
            {
                if (value == _addtlInfo) return;
                _addtlInfo = value;
                OnPropertyChanged();
            }
        }

        public bool OnHold
        {
            get => _onHold;
            set
            {
                if (value == _onHold) return;
                _onHold = value;
                OnPropertyChanged();
            }
        }

        public string PayReqdNote
        {
            get => _payReqdNote;
            set
            {
                if (value == _payReqdNote) return;
                _payReqdNote = value;
                OnPropertyChanged();
            }
        }

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

        public DateTime DateCompleted
        {
            get => _dateCompleted;
            set
            {
                if (value == _dateCompleted) return;
                _dateCompleted = value;
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

        public string Architect
        {
            get => _architect;
            set
            {
                if (value == _architect) return;
                _architect = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                if (value == _address) return;
                _address = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get => _city;
            set
            {
                if (value == _city) return;
                _city = value;
                OnPropertyChanged();
            }
        }

        public string State
        {
            get => _state;
            set
            {
                if (value == _state) return;
                _state = value;
                OnPropertyChanged();
            }
        }
        public string Zip
        {
            get => _zip;
            set
            {
                if (value == _zip) return;
                _zip = value;
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

        public string PmName
        {
            get => _pmName;
            set
            {
                if (value == _pmName) return;
                _pmName = value;
                OnPropertyChanged();
            }
        }

        public int BillingDate
        {
            get => _billingDate;
            set
            {
                if (value == _billingDate) return;
                _billingDate = value;
                OnPropertyChanged();
            }
        }

        public bool ContractRecvd
        {
            get => _contractRecvd;
            set
            {
                if (value == _contractRecvd) return;
                _contractRecvd = value;
                OnPropertyChanged();
            }
        }

        public bool StoredMat
        {
            get => _storedMat;
            set
            {
                if (value == _storedMat) return;
                _storedMat = value;
                OnPropertyChanged();
            }
        }
    }
}