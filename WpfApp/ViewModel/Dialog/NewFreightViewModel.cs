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
    class NewFreightViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        private string sqlquery = "";

        public NewFreightViewModel()
        {
            TempFreight = new FreightCo();
            dbConnection = new DatabaseConnection();
            this.SaveCommand = new RelayCommand((e) => this.SaveFreight());
        }

        // Binding Variable
        private FreightCo _tempFreight;

        public FreightCo TempFreight
        {
            get => _tempFreight;
            set
            {
                if (value == _tempFreight) return;
                _tempFreight = value;
                OnPropertyChanged();
            }
        }

        // Command
        public RelayCommand SaveCommand { get; set; }

        private void SaveFreight()
        {

            sqlquery = "INSERT INTO tblFreightCo(FreightCo_Name, FreightCo_Phone, FreightCo_Cell, FreightCo_Email, FreightCo_Contact, Active) OUTPUT INSERTED.FreightCo_ID VALUES (@Name, @Phone, @Cell, @Email, @Contact, @Active)";

            string name = TempFreight.FreightName;
            string phone = TempFreight.Phone;
            string cell = TempFreight.Cell;
            string email = TempFreight.Email;
            string contact = TempFreight.Contact;
            bool active = TempFreight.Active;

            int insertedPMId = dbConnection.RunQueryToCreateFreight(sqlquery, name, phone, cell, email, contact, active);

            MessageBox.Show("New Freight CO is added successfully", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
