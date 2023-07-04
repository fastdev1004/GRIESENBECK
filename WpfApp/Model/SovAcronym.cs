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
    class SovAcronym:ViewModelBase
    {
        private int _id;
        private string _sovAcronym;
        private int _coItemNo;
        private string _sovDesc;
        private bool _matOnly;
        private int _projSovID;
        private int _coID;
        private int _actionFlag;

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

        public string SovDesc
        {
            get => _sovDesc;
            set
            {
                if (value == _sovDesc) return;
                _sovDesc = value;
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

        public string SovAcronymCoItem
        {
            get
            {
                return _sovAcronym + " | " + _sovDesc;
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
