using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class PathDescription : ViewModelBase
    {
        private string _pathDesc;

        public string PathDesc
        {
            get => _pathDesc;
            set
            {
                if (value == _pathDesc) return;
                _pathDesc = value;
                OnPropertyChanged();
            }
        }
    }
}
