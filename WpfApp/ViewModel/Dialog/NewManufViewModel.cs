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
    class NewManufViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        private string sqlquery = "";

        public NewManufViewModel()
        {
            TempManuf = new Manufacturer();
            ManufNotes = new ObservableCollection<Note>();
            dbConnection = new DatabaseConnection();
            this.SaveCommand = new RelayCommand((e) => this.SaveManuf());
            this.AddNewNoteCommand = new RelayCommand((e) => this.AddNewNote());
        }

        // Binding Variable
        private Manufacturer _tempManuf;

        public Manufacturer TempManuf
        {
            get => _tempManuf;
            set
            {
                if (value == _tempManuf) return;
                _tempManuf = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Note> _manufNotes;

        public ObservableCollection<Note> ManufNotes
        {
            get => _manufNotes;
            set
            {
                _manufNotes = value;
                OnPropertyChanged();
            }
        }

        // Command
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand AddNewNoteCommand { get; set; }

        private void SaveManuf()
        {

            if (!string.IsNullOrEmpty(TempManuf.ManufacturerName))
            {
                sqlquery = "INSERT INTO tblManufacturers(Manuf_Name, Address, Address2, City, State, ZIP, Phone, FAX, Contact_Name, Contact_Phone, Contact_Email, Active) OUTPUT INSERTED.Manuf_ID VALUES (@ManufName, @Address, @Address2, @City, @State, @Zip, @Phone, @Fax, @ContactName, @ContactPhone, @ContactEmail, @Active)";

                string manufName = TempManuf.ManufacturerName;
                string address = TempManuf.Address;
                string address2 = TempManuf.Address2;
                string city = TempManuf.City;
                string state = TempManuf.State;
                string zip = TempManuf.Zip;
                string phone = TempManuf.Phone;
                string fax = TempManuf.Fax;
                string contactName = TempManuf.ContactName;
                string contactPhone = TempManuf.ContactPhone;
                string contactEmail = TempManuf.ContactEmail;
                bool active = TempManuf.Active;

                int insertedManufId = dbConnection.RunQueryToCreateManuf(sqlquery, manufName, address, address2, city, state, zip, phone, fax, contactName, contactPhone, contactEmail, active);

                foreach (Note _note in ManufNotes)
                {
                    sqlquery = "INSERT INTO tblNotes(Notes_PK, Notes_PK_Desc, Notes_Note, Notes_DateAdded, Notes_User) OUTPUT INSERTED.Notes_ID VALUES (@NotesPK, @NotesDesc, @NotesNote, @NotesDateAdded, @NotesUser)";

                    int notesPK = insertedManufId;
                    string notesPkDesc = "Manuf";
                    string notesNote = _note.NotesNote;
                    DateTime notesDateAdded = _note.NotesDateAdded;
                    string user = "smile";

                    int insertedNoteId = dbConnection.RunQueryToCreateNote(sqlquery, notesPK, notesPkDesc, notesNote, notesDateAdded, user);
                }
                MessageBox.Show("New Manufacturer is added successfully", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddNewNote()
        {
            Note _note = new Note();
            _note.NotesDateAdded = DateTime.Now;
            _note.NoteUser = "smile";
            _note.NotesNote = "";
            ManufNotes.Add(_note);
        }
    }
}