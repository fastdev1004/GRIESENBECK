using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ReportSelection:ViewModelBase
    {
        private int _reportID;
        private string _selecionName;
        private string _selectionDataType;

        public int ReportID
        {
            get => _reportID;
            set
            {
                if (value == _reportID) return;
                _reportID = value;
                OnPropertyChanged();
            }
        }

        public string SelecionName
        {
            get => _selecionName;
            set
            {
                if (value == _selecionName) return;
                _selecionName = value;
                OnPropertyChanged();
            }
        }

        public string SelectionDataType
        {
            get => _selectionDataType;
            set
            {
                if (value == _selectionDataType) return;
                _selectionDataType = value;
                OnPropertyChanged();
            }
        }
    }
}
