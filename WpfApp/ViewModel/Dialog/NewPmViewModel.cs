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
    class NewPmViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        private string sqlquery = "";

        public NewPmViewModel()
        {
            TempPM = new ProjectManager();
            PmNotes = new ObservableCollection<Note>();
            dbConnection = new DatabaseConnection();
            this.SaveCommand = new RelayCommand((e) => this.SaveProjectManager());
            this.AddNewNoteCommand = new RelayCommand((e) => this.AddNewNote());
        }

        // Binding Variable
        private ProjectManager _tempPM;

        public ProjectManager TempPM
        {
            get => _tempPM;
            set
            {
                if (value == _tempPM) return;
                _tempPM = value;
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

        private ObservableCollection<Note> _pmNotes;

        public ObservableCollection<Note> PmNotes
        {
            get => _pmNotes;
            set
            {
                _pmNotes = value;
                OnPropertyChanged();
            }
        }

        // Command
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand AddNewNoteCommand { get; set; }

        private void SaveProjectManager()
        {
            sqlquery = "INSERT INTO tblProjectManagers(Customer_ID, PM_Name, PM_Phone, PM_CellPhone, PM_Email, Active) OUTPUT INSERTED.PM_ID VALUES (@CustomerID, @Name, @Phone, @Cell, @Email, @Active)";

            //int selectedCustomerID = SelectedCustomerID;
            int customerID = CustomerID;
            string name = TempPM.PMName;
            string phone = TempPM.PMPhone;
            string cell = TempPM.PMCellPhone;
            string email = TempPM.PMEmail;
            bool active = TempPM.Active;

            int insertedPMId = dbConnection.RunQueryToCreateCustAddInfo(sqlquery, customerID, name, phone, cell, email, active);

            foreach (Note _note in PmNotes)
            {
                sqlquery = "INSERT INTO tblNotes(Notes_PK, Notes_PK_Desc, Notes_Note, Notes_DateAdded, Notes_User) OUTPUT INSERTED.Notes_ID VALUES (@NotesPK, @NotesDesc, @NotesNote, @NotesDateAdded, @NotesUser)";

                int notesPK = insertedPMId;
                string notesPkDesc = "ProjectManager";
                string notesNote = _note.NotesNote;
                DateTime notesDateAdded = _note.NotesDateAdded;
                string user = "smile";

                int insertedNoteId = dbConnection.RunQueryToCreateNote(sqlquery, notesPK, notesPkDesc, notesNote, notesDateAdded, user);
            }
            MessageBox.Show("New Project Manager is added successfully", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
       
        }

        private void AddNewNote()
        {
            Note _note = new Note();
            _note.NotesDateAdded = DateTime.Now;
            _note.NoteUser = "smile";
            _note.NotesNote = "";
            PmNotes.Add(_note);
        }
    }
}
