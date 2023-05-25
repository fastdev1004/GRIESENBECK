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
    class ProjectManager:ViewModelBase
    {
        private int _id;
        private string _name;
        private string _cellPhone;
        private string _email;

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

        public string PMName
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string PMCellPhone
        {
            get => _cellPhone;
            set
            {
                if (value == _cellPhone) return;
                _cellPhone = value;
                OnPropertyChanged();
            }
        }

        public string PMEmail
        {
            get => _email;
            set
            {
                if (value == _email) return;
                _email = value;
                OnPropertyChanged();
            }
        }
    }
}