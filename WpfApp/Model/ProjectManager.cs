using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Utils;
using WpfApp.ViewModel;

namespace WpfApp.Model
{
    class ProjectManager:ViewModelBase
    {
        private DatabaseConnection dbConnection;

        private int _projPmID;
        private int _id;
        private int _customerID;
        private string _name;
        private string _phone;
        private string _cellPhone;
        private string _email;
        private bool _active;

        public ProjectManager()
        {
            dbConnection = new DatabaseConnection();
        }

        public int ProjPmID
        {
            get => _projPmID;
            set
            {
                if (value == _projPmID) return;
                _projPmID = value;
                OnPropertyChanged();
            }
        }

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
        
        public int CustomerID
        {
            get => _customerID;
            set
            {
                if (value == _customerID) return;
                _customerID = value;
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

        public string PMPhone
        {
            get => _phone;
            set
            {
                if (value == _phone) return;
                _phone = value;
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

        public bool Active
        {
            get => _active;
            set
            {
                if (value == _active) return;
                _active = value;
                OnPropertyChanged();
            }
        }
    }
}