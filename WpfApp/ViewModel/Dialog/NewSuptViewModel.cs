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
    class NewSuptViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        private string sqlquery = "";

        public NewSuptViewModel()
        {
            TempSup = new Superintendent();
            SuptNotes = new ObservableCollection<Note>();
            dbConnection = new DatabaseConnection();
            this.SaveCommand = new RelayCommand((e) => this.SaveSuperintendent());
            this.AddNewNoteCommand = new RelayCommand((e) => this.AddNewNote());
        }

        // Binding Variable
        private Superintendent _tempSupt;

        public Superintendent TempSup
        {
            get => _tempSupt;
            set
            {
                if (value == _tempSupt) return;
                _tempSupt = value;
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

        private ObservableCollection<Note> _suptNotes;

        public ObservableCollection<Note> SuptNotes
        {
            get => _suptNotes;
            set
            {
                _suptNotes = value;
                OnPropertyChanged();
            }
        }

        // Command
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand AddNewNoteCommand { get; set; }

        private void SaveSuperintendent()
        {
            sqlquery = "INSERT INTO tblSuperintendents(Customer_ID, Sup_Name, Sup_Phone, Sup_CellPhone, Sup_Email, Active) OUTPUT INSERTED.Sup_ID VALUES (@CustomerID, @Name, @Phone, @Cell, @Email, @Active)";

            int selectedCustomerID = CustomerID;
            string name = TempSup.SupName;
            string phone = TempSup.SupPhone;
            string cell = TempSup.SupCellPhone;
            string email = TempSup.SupEmail;
            bool active = TempSup.Active;

            int insertedSupId = dbConnection.RunQueryToCreateCustAddInfo(sqlquery, selectedCustomerID, name, phone, cell, email, active);

            foreach (Note _note in SuptNotes)
            {
                sqlquery = "INSERT INTO tblNotes(Notes_PK, Notes_PK_Desc, Notes_Note, Notes_DateAdded, Notes_User) OUTPUT INSERTED.Notes_ID VALUES (@NotesPK, @NotesDesc, @NotesNote, @NotesDateAdded, @NotesUser)";

                int notesPK = insertedSupId;
                string notesPkDesc = "Superintendent";
                string notesNote = _note.NotesNote;
                DateTime notesDateAdded = _note.NotesDateAdded;
                string user = "smile";

                int insertedNoteId = dbConnection.RunQueryToCreateNote(sqlquery, notesPK, notesPkDesc, notesNote, notesDateAdded, user);
            }
            MessageBox.Show("New Superintendent is added successfully", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddNewNote()
        {
            Note _note = new Note();
            _note.NotesDateAdded = DateTime.Now;
            _note.NoteUser = "smile";
            _note.NotesNote = "";
            SuptNotes.Add(_note);
        }
    }
}
