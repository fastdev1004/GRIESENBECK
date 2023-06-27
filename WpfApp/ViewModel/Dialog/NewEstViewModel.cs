using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.Command;
using WpfApp.Model;
using WpfApp.Utils;

namespace WpfApp.ViewModel.Dialog
{
    class NewEstViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        private string sqlquery = "";

        public NewEstViewModel()
        {
            TempEst = new Estimator();
            dbConnection = new DatabaseConnection();
            this.SaveCommand = new RelayCommand((e) => this.SaveEstimator());
        }

        // Binding Variable
        private Estimator _tempEst;

        public Estimator TempEst
        {
            get => _tempEst;
            set
            {
                if (value == _tempEst) return;
                _tempEst = value;
                OnPropertyChanged();
            }
        }

        // Command
        public RelayCommand SaveCommand { get; set; }

        private void SaveEstimator()
        {

            sqlquery = "INSERT INTO tblEstimators(Estimator_Initials, Estimator_Name, Cell, Estimator_Email, Active) OUTPUT INSERTED.Estimator_ID VALUES (@Initial, @Name, @Cell, @Email, @Active)";

            string initial = TempEst.Initial;
            string name = TempEst.Name;
            string cell = TempEst.Cell;
            string email = TempEst.Email;
            bool active = TempEst.Active;

            int insertedPMId = dbConnection.RunQueryToCreateEstimator(sqlquery, initial, name, cell, email, active);

            MessageBox.Show("New Estimator is added successfully", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
