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
    class NewSovViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        private string sqlquery = "";

        public NewSovViewModel()
        {
            TempAcronym = new Acronym();
            dbConnection = new DatabaseConnection();
            this.SaveCommand = new RelayCommand((e) => this.SaveAcronym());
        }

        // Binding Variable
        private Acronym _tempAcronym;

        public Acronym TempAcronym
        {
            get => _tempAcronym;
            set
            {
                if (value == _tempAcronym) return;
                _tempAcronym = value;
                OnPropertyChanged();
            }
        }

        // Command
        public RelayCommand SaveCommand { get; set; }

        private void SaveAcronym()
        {
            sqlquery = "INSERT INTO tblScheduleOfValues(SOV_Acronym, SOV_Desc, Active) OUTPUT INSERTED.SOV_Acronym VALUES (@Name, @Desc, @Active)";

            string name = TempAcronym.AcronymName;
            string desc = TempAcronym.AcronymDesc;
            bool active = TempAcronym.Active;

            string insertedAcronymName = dbConnection.RunQueryToCreateSOV(sqlquery, name, desc, active);

            MessageBox.Show("New Schedule of Values is added successfully", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
