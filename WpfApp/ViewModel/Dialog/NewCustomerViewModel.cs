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
    class NewCustomerViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        private string sqlquery = "";

        public NewCustomerViewModel()
        {
            TempCustomer = new Customer();
            CustomerNotes = new ObservableCollection<Note>();
            dbConnection = new DatabaseConnection();
            this.SaveCommand = new RelayCommand((e) => this.SaveCustomer());
            this.AddNewNoteCommand = new RelayCommand((e) => this.AddNewNote());
        }

        // Binding Variable
        private Customer _tempCustomer;

        public Customer TempCustomer
        {
            get => _tempCustomer;
            set
            {
                if (value == _tempCustomer) return;
                _tempCustomer = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Note> _customerNotes;

        public ObservableCollection<Note> CustomerNotes
        {
            get => _customerNotes;
            set
            {
                _customerNotes = value;
                OnPropertyChanged();
            }
        }

        // Command
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand AddNewNoteCommand { get; set; }

        private void SaveCustomer()
        {
            
            if (!string.IsNullOrEmpty(TempCustomer.FullName))
            {
                sqlquery = "INSERT INTO tblCustomers(Short_Name, Full_Name, PO_Box, Address, City, State, ZIP, Phone, FAX, Email, Active) OUTPUT INSERTED.Customer_ID VALUES (@ShortName, @FullName, @PoNumber, @Address, @City, @State, @Zip, @Phone, @Fax, @Email, @Active)";

                string fullName = TempCustomer.FullName;
                string shortName = TempCustomer.ShortName;
                string poNumber = TempCustomer.PoBox;
                string address = TempCustomer.Address;
                string city = TempCustomer.City;
                string state = TempCustomer.State;
                string zip = TempCustomer.Zip;
                string phone = TempCustomer.Phone;
                string fax = TempCustomer.Fax;
                string email = TempCustomer.Email;
                bool active = TempCustomer.Active;

                int insertedCustomerId = dbConnection.RunQueryToCreateCustomer(sqlquery, fullName, shortName, poNumber, address, city, state, zip, phone, fax, email, active);

                foreach (Note _note in CustomerNotes)
                {
                    sqlquery = "INSERT INTO tblNotes(Notes_PK, Notes_PK_Desc, Notes_Note, Notes_DateAdded, Notes_User) OUTPUT INSERTED.Notes_ID VALUES (@NotesPK, @NotesDesc, @NotesNote, @NotesDateAdded, @NotesUser)";

                    int notesPK = insertedCustomerId;
                    string notesPkDesc = "Customer";
                    string notesNote = _note.NotesNote;
                    DateTime notesDateAdded = _note.NotesDateAdded;
                    string user = "smile";

                    int insertedNoteId = dbConnection.RunQueryToCreateNote(sqlquery, notesPK, notesPkDesc, notesNote, notesDateAdded, user);
                }

                MessageBox.Show("New Customer is added successfully", "Save" , MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Customer Full Name is required", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddNewNote()
        {
            Note _note = new Note();
            _note.NotesDateAdded = DateTime.Now;
            _note.NoteUser = "smile";
            _note.NotesNote = "";
            CustomerNotes.Add(_note);
        }
    }
}
