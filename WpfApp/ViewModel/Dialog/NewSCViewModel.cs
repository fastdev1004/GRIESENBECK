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
    class NewSCViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        private string sqlquery = "";

        public NewSCViewModel()
        {
            TempSC = new CustomerContact();
            ScNotes = new ObservableCollection<Note>();
            dbConnection = new DatabaseConnection();
            this.SaveCommand = new RelayCommand((e) => this.SaveSC());
            this.AddNewNoteCommand = new RelayCommand((e) => this.AddNewNote());
        }

        // Binding Variable
        private CustomerContact _tempSC;

        public CustomerContact TempSC
        {
            get => _tempSC;
            set
            {
                if (value == _tempSC) return;
                _tempSC = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Note> _scNotes;

        public ObservableCollection<Note> ScNotes
        {
            get => _scNotes;
            set
            {
                _scNotes = value;
                OnPropertyChanged();
            }
        }

        private int _customerID;

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

        // Command
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand AddNewNoteCommand { get; set; }

        private void SaveSC()
        {

            sqlquery = "INSERT INTO tblCustomerContacts(Customer_ID, CC_Name, CC_Phone, CC_CellPhone, CC_Email, Active) OUTPUT INSERTED.CC_ID VALUES (@CustomerID, @Name, @Phone, @Cell, @Email, @Active)";

            int customerID = CustomerID;
            string name = TempSC.CCName;
            string phone = TempSC.CCPhone;
            string cell = TempSC.CCCell;
            string email = TempSC.CCEmail;
            bool active = TempSC.CCActive;

            int insertedSubmID = dbConnection.RunQueryToCreateCustAddInfo(sqlquery, CustomerID, name, phone, cell, email, active);

            foreach (Note _note in ScNotes)
            {
                sqlquery = "INSERT INTO tblNotes(Notes_PK, Notes_PK_Desc, Notes_Note, Notes_DateAdded, Notes_User) OUTPUT INSERTED.Notes_ID VALUES (@NotesPK, @NotesDesc, @NotesNote, @NotesDateAdded, @NotesUser)";

                int notesPK = insertedSubmID;
                string notesPkDesc = "CustomerContact";
                string notesNote = _note.NotesNote;
                DateTime notesDateAdded = _note.NotesDateAdded;
                string user = "smile";

                int insertedNoteId = dbConnection.RunQueryToCreateNote(sqlquery, notesPK, notesPkDesc, notesNote, notesDateAdded, user);
            }
            MessageBox.Show("New Subittal Contact is added successfully", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddNewNote()
        {
            Note _note = new Note();
            _note.NotesDateAdded = DateTime.Now;
            _note.NoteUser = "smile";
            _note.NotesNote = "";
            ScNotes.Add(_note);
        }
    }
}