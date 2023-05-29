using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class Warranty:ViewModelBase
    {
        private int _warrantyID;
        private string _docuReq;
        private DateTime _complDate;
        private DateTime _dateRecd;
        private int _numOfCopy;
        private string _contractName;
        private string _submVia;
        private DateTime _dateSent;
        private string _sentVia;
        private string _notes;


        public int ID
        {
            get => _warrantyID;
            set
            {
                if (value == _warrantyID) return;
                _warrantyID = value;
                OnPropertyChanged();
            }
        }

        public string DocuReq
        {
            get => _docuReq;
            set
            {
                if (value == _docuReq) return;
                _docuReq = value;
                OnPropertyChanged();
            }
        }

        public DateTime ComplDate
        {
            get => _complDate;
            set
            {
                if (value == _complDate) return;
                _complDate = value;
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

        public int NumOfCopy
        {
            get => _numOfCopy;
            set
            {
                if (value == _numOfCopy) return;
                _numOfCopy = value;
                OnPropertyChanged();
            }
        }

        public string ContractName
        {
            get => _contractName;
            set
            {
                if (value == _contractName) return;
                _contractName = value;
                OnPropertyChanged();
            }
        }

        public string SubmVia
        {
            get => _submVia;
            set
            {
                if (value == _submVia) return;
                _submVia = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateSent
        {
            get => _dateSent;
            set
            {
                if (value == _dateSent) return;
                _dateSent = value;
                OnPropertyChanged();
            }
        }

        public string SentVia
        {
            get => _sentVia;
            set
            {
                if (value == _sentVia) return;
                _sentVia = value;
                OnPropertyChanged();
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (value == _notes) return;
                _notes = value;
                OnPropertyChanged();
            }
        }
    }
}
