using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ChangeOrder:ViewModelBase
    {
        private int _projectID;
        private int _coID;
        private int _coItemNo;
        private DateTime _coDate;
        private string _coAppDen;
        private DateTime _coDateAppDen;
        private string _coComment;


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

        public DateTime CoDate
        {
            get => _coDate;
            set
            {
                if (value == _coDate) return;
                _coDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime CoDateAppDen
        {
            get => _coDateAppDen;
            set
            {
                if (value == _coDateAppDen) return;
                _coDateAppDen = value;
                OnPropertyChanged();
            }
        }

        public string CoAppDen
        {
            get => _coAppDen;
            set
            {
                if (value == _coAppDen) return;
                _coAppDen = value;
                OnPropertyChanged();
            }
        }

        public string CoComment
        {
            get => _coComment;
            set
            {
                if (value == _coComment) return;
                _coComment = value;
                OnPropertyChanged();
            }
        }
    }
}