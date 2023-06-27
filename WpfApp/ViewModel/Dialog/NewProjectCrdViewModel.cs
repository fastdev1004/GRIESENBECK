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
    class NewProjectCrdViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        private string sqlquery = "";

        public NewProjectCrdViewModel()
        {
            TempProjectCrd = new PC();
            dbConnection = new DatabaseConnection();
            this.SaveCommand = new RelayCommand((e) => this.SavePC());
        }

        // Binding Variable
        private PC _tempProjectCrd;

        public PC TempProjectCrd
        {
            get => _tempProjectCrd;
            set
            {
                if (value == _tempProjectCrd) return;
                _tempProjectCrd = value;
                OnPropertyChanged();
            }
        }

        // Command
        public RelayCommand SaveCommand { get; set; }

        private void SavePC()
        {
            sqlquery = "INSERT INTO tblPCs(PC_Name, PC_Email, PC_Active) OUTPUT INSERTED.PC_ID VALUES (@Name, @Email, @Active)";

            string name = TempProjectCrd.PCName;
            string email = TempProjectCrd.PCEmail;
            bool active = TempProjectCrd.Active;

            int insertedPcID = dbConnection.RunQueryToCreatePC(sqlquery, name, email, active);

            MessageBox.Show("New Project Coord is added successfully", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
