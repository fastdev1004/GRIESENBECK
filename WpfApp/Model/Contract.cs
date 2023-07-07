using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class Contract : ViewModelBase
    {
        private int _fetchID;
        private int _scID;
        private string _jobNo;
        private string _contractNumber;
        private bool _changeOrder;
        private string _changeOrderNo;
        private DateTime _dateRecd;
        private DateTime _dateProcessed;
        private int _amtOfContract;
        private DateTime _signedoffbySales;
        private DateTime _signedoffbyoperations;
        private DateTime _givenAcctingforreview;
        private DateTime _givenforfinalsignature;
        private DateTime _dateReturnedback;
        private DateTime _returnedtoDawn;
        private string _scope;
        private string _returnedVia;
        private string _comment;
        private int _projectID;
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

        public int ScID
        {
            get => _scID;
            set
            {
                if (value == _scID) return;
                _scID = value;
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

        public string ContractNumber
        {
            get => _contractNumber;
            set
            {
                if (value == _contractNumber) return;
                _contractNumber = value;
                OnPropertyChanged();
            }
        }

        public bool ChangeOrder
        {
            get => _changeOrder;
            set
            {
                if (value == _changeOrder) return;
                _changeOrder = value;
                OnPropertyChanged();
            }
        }

        public string ChangeOrderNo
        {
            get => _changeOrderNo;
            set
            {
                if (value == _changeOrderNo) return;
                _changeOrderNo = value;
                OnPropertyChanged();
            }
        }
        public DateTime DateRecd
        {
            get => _dateRecd;
            set
            {
                if (value == _dateRecd) return;
                _dateRecd = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateProcessed
        {
            get => _dateProcessed;
            set
            {
                if (value == _dateProcessed) return;
                _dateProcessed = value;
                OnPropertyChanged();
            }
        }

        public int AmtOfContract
        {
            get => _amtOfContract;
            set
            {
                if (value == _amtOfContract) return;
                _amtOfContract = value;
                OnPropertyChanged();
            }
        }

        public DateTime SignedoffbySales
        {
            get => _signedoffbySales;
            set
            {

                if (value == _signedoffbySales) return;
                _signedoffbySales = value;
                OnPropertyChanged();
            }
        }

        public DateTime Signedoffbyoperations
        {
            get => _signedoffbyoperations;
            set
            {

                if (value == _signedoffbyoperations) return;
                _signedoffbyoperations = value;
                OnPropertyChanged();
            }
        }

        public DateTime GivenAcctingforreview
        {
            get => _givenAcctingforreview;
            set
            {

                if (value == _givenAcctingforreview) return;
                _givenAcctingforreview = value;
                OnPropertyChanged();
            }
        }

        public DateTime Givenforfinalsignature
        {
            get => _givenforfinalsignature;
            set
            {

                if (value == _givenforfinalsignature) return;
                _givenforfinalsignature = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateReturnedback
        {
            get => _dateReturnedback;
            set
            {

                if (value == _dateReturnedback) return;
                _dateReturnedback = value;
                OnPropertyChanged();
            }
        }

        public DateTime ReturnedtoDawn
        {
            get => _returnedtoDawn;
            set
            {

                if (value == _returnedtoDawn) return;
                _returnedtoDawn = value;
                OnPropertyChanged();
            }
        }

        public string Scope
        {
            get => _scope;
            set
            {
                if (value == _scope) return;
                _scope = value;
                OnPropertyChanged();
            }
        }

        public string ReturnedVia
        {
            get => _returnedVia;
            set
            {
                if (value == _returnedVia) return;
                _returnedVia = value;
                OnPropertyChanged();
            }
        }

        public string Comment
        {
            get => _comment;
            set
            {
                if (value == _comment) return;
                _comment = value;
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
