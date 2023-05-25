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
    class TrackShipRecv:ViewModelBase
    {
        private int _trackID;
        private string _materialName;
        private string _sov;
        private string _changeOrder;
        private string _phase;
        private string _type;
        private string _color;
        private string _qtyReqd;
        private string _projMatID;

        public int ID
        {
            get => _trackID;
            set
            {
                if (value == _trackID) return;
                _trackID = value;
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
        public string SovName
        {
            get => _sov;
            set
            {
                if (value == _sov) return;
                _sov = value;
                OnPropertyChanged();
            }
        }
        public string ChangeOrder
        {
            get => _changeOrder;
            set
            {
                if (value == _changeOrder) return;
                _changeOrder = value;
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
        public string Type
        {
            get => _type;
            set
            {
                if (value == _type) return;
                _type = value;
                OnPropertyChanged();
            }
        }
        public string Color
        {
            get => _color;
            set
            {
                if (value == _color) return;
                _color = value;
                OnPropertyChanged();
            }
        }
        public string QtyReqd
        {
            get => _qtyReqd;
            set
            {
                if (value == _qtyReqd) return;
                _qtyReqd = value;
                OnPropertyChanged();
            }
        }

        public string ProjMatID
        {
            get => _projMatID;
            set
            {
                if (value == _projMatID) return;
                _projMatID = value;
                OnPropertyChanged();
            }
        }
    }
}
