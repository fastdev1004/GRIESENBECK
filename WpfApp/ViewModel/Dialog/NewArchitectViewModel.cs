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
    class NewArchitectViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        private string sqlquery = "";

        public NewArchitectViewModel()
        {
            TempArch = new Architect();
            dbConnection = new DatabaseConnection();
            this.SaveCommand = new RelayCommand((e) => this.SaveArchitect());
        }

        // Binding Variable
        private Architect _tempArch;

        public Architect TempArch
        {
            get => _tempArch;
            set
            {
                if (value == _tempArch) return;
                _tempArch = value;
                OnPropertyChanged();
            }
        }

        // Command
        public RelayCommand SaveCommand { get; set; }

        public RelayCommand AddNewNoteCommand { get; set; }

        private void SaveArchitect()
        {
            sqlquery = "INSERT INTO tblArchitects(Arch_Company, Arch_Contact, Arch_Address, Arch_City, Arch_State, Arch_ZIP, Arch_Phone, Arch_FAX, Arch_Cell, Arch_Email, Active) OUTPUT INSERTED.Architect_ID VALUES (@Company, @Contact, @Address, @City, @State, @Zip, @Phone, @Fax, @Cell, @Email, @Active)";

            string archCompany = TempArch.ArchCompany;
            string archContact = TempArch.Contact;
            string archAddress = TempArch.Address;
            string archCity = TempArch.City;
            string archState = TempArch.State;
            string archZip = TempArch.Zip;
            string archPhone = TempArch.Phone;
            string archFax = TempArch.Fax;
            string archCell = TempArch.Cell;
            string archEmail = TempArch.Email;
            bool active = TempArch.Active;

            int insertedArchID = dbConnection.RunQueryToCreateArch(sqlquery, archCompany, archContact, archAddress, archCity, archState, archZip, archPhone, archFax, archCell, archEmail, active);

            MessageBox.Show("New Architect is added successfully", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
