using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportCOItem : ViewModelBase
    {
        private int _projectID;
        private string _contract;
        private string _changeOrderNo;
        private DateTime _dateOFCO;
        private string _amtOfCO;
        private DateTime _dateProcessed;
        private DateTime _signedOffBySales;
        private DateTime _finalSig;
        private DateTime _returnedBack;
        private string _returnedVia;
        private string _scope;
        private string _comment;

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

        public string Contract
        {
            get => _contract;
            set
            {
                if (value == _contract) return;
                _contract = value;
                OnPropertyChanged();
            }
        }

        public string ChangeOrderNO
        {
            get => _changeOrderNo;
            set
            {
                if (value == _changeOrderNo) return;
                _changeOrderNo = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateOFCO
        {
            get => _dateOFCO;
            set
            {
                if (value == _dateOFCO) return;
                _dateOFCO = value;
                OnPropertyChanged();
            }
        }

        public string AmtOfCO
        {
            get => _amtOfCO;
            set
            {
                if (value == _amtOfCO) return;
                _amtOfCO = value;
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

        public DateTime SalesDate
        {
            get => _signedOffBySales;
            set
            {
                if (value == _signedOffBySales) return;
                _signedOffBySales = value;
                OnPropertyChanged();
            }
        }

        public DateTime FinalSig
        {
            get => _finalSig;
            set
            {
                if (value == _finalSig) return;
                _finalSig = value;
                OnPropertyChanged();
            }
        }

        public DateTime ReturnedBack
        {
            get => _returnedBack;
            set
            {
                if (value == _returnedBack) return;
                _returnedBack = value;
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
    }
}
