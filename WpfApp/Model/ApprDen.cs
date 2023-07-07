using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ApprDen : ViewModelBase
    {
        private string _apprDenValue;
 
        public string ApprDenValue
        {
            get => _apprDenValue;
            set
            {
                if (value == _apprDenValue) return;
                _apprDenValue = value;
                OnPropertyChanged();
            }
        }
    }
}