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
    class Estimator:ViewModelBase
    {
        private int _estimatorID;
        private string _estimatorName;

        public int ID
        {
            get => _estimatorID;
            set
            {
                if (value == _estimatorID) return;
                _estimatorID = value;
                OnPropertyChanged();
            }
        }

        public string EstimatorName
        {
            get => _estimatorName;
            set
            {
                if (value == _estimatorName) return;
                _estimatorName = value;
                OnPropertyChanged();
            }
        }
    }
}
    