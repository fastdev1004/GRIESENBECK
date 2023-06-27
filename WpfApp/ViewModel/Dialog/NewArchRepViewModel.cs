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
    class NewArchRepViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        private string sqlquery = "";

        public NewArchRepViewModel()
        {
            TempArchRep = new ArchRep();
            dbConnection = new DatabaseConnection();
            this.SaveCommand = new RelayCommand((e) => this.SavePC());
        }

        // Binding Variable
        private ArchRep _tempArchRep;

        public ArchRep TempArchRep
        {
            get => _tempArchRep;
            set
            {
                if (value == _tempArchRep) return;
                _tempArchRep = value;
                OnPropertyChanged();
            }
        }

        // Command
        public RelayCommand SaveCommand { get; set; }

        private void SavePC()
        {
            sqlquery = "INSERT INTO tblArchRep(Arch_Rep_Name, Arch_Rep_Email, Active) OUTPUT INSERTED.Arch_Rep_ID VALUES (@Name, @Email, @Active)";

            string name = TempArchRep.ArchRepName;
            string email = TempArchRep.ArchRepEmail;
            bool active = TempArchRep.Active;

            int insertedPcID = dbConnection.RunQueryToCreatePC(sqlquery, name, email, active);

            MessageBox.Show("New ArchRep is added successfully", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}