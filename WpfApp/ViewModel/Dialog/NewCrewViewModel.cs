using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.Command;
using WpfApp.Model;
using WpfApp.Utils;

namespace WpfApp.ViewModel.Dialog
{
    class NewCrewViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        private string sqlquery = "";

        public NewCrewViewModel()
        {
            TempCrew = new Crew();
            dbConnection = new DatabaseConnection();
            this.SaveCommand = new RelayCommand((e) => this.SaveCrew());
        }

        // Binding Variable
        private Crew _tempCrew;

        public Crew TempCrew
        {
            get => _tempCrew;
            set
            {
                if (value == _tempCrew) return;
                _tempCrew = value;
                OnPropertyChanged();
            }
        }

        // Command
        public RelayCommand SaveCommand { get; set; }

        private void SaveCrew()
        {

            sqlquery = "INSERT INTO tblInstallCrew(Crew_Name, Crew_Phone, Crew_Cell, Crew_Email, Active) OUTPUT INSERTED.Crew_ID VALUES (@Name, @Phone, @Cell, @Email, @Active)";

            string name = TempCrew.CrewName;
            string phone = TempCrew.Phone;
            string cell = TempCrew.Cell;
            string email = TempCrew.Email;
            bool active = TempCrew.Active;

            int insertedCrewId = dbConnection.RunQueryToCreateCrew(sqlquery, name, phone, cell, email, active);

            MessageBox.Show("New Crew is added successfully", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}