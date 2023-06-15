using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp.Model;
using WpfApp.Utils;
using WpfApp.ViewModel;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for AdministrationView.xaml
    /// </summary>
    public partial class AdminView : Page
    {
        private AdminViewModel AdminVM;
        private NoteHelper noteHelper;
        public AdminView()
        {
            InitializeComponent();
            AdminVM = new AdminViewModel();
            noteHelper = new NoteHelper();
            this.DataContext = AdminVM;
        }



        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }

        private void CustSupt_PreviewKeyUp(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                int selectedRowIndex = AdminVM.SelectedSuptRowIndex;

                Grid parentGrid = textBox.Parent as Grid;
                TextBlock supID = parentGrid.Children[1] as TextBlock;
                AdminVM.SelectedCustSupID = int.Parse(supID.Text);

                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        AdminVM.ActionSupState = "CreateSup";
                        ObservableCollection<Superintendent> custSupts = AdminVM.CustSupts;
                        Superintendent item = new Superintendent();
                        custSupts.Add(item);
                        e.Handled = true;
                    }
                    else
                    {
                        AdminVM.ActionSupState = "UpdateSup";
                    }

                    string itemName = textBox.Tag as string;
                    AdminVM.CurrentSupName = "";
                    AdminVM.CurrentSupPhone = "";
                    AdminVM.CurrentSupCell = "";
                    AdminVM.CurrentSupEmail = "";
                    switch (itemName)
                    {
                        case "SupName":
                            AdminVM.CurrentSupName = textBox.Text;
                            break;
                        case "SupPhone":
                            AdminVM.CurrentSupPhone = textBox.Text;
                            break;
                        case "SupCell":
                            AdminVM.CurrentSupCell = textBox.Text;
                            break;
                        case "SupEmail":
                            AdminVM.CurrentSupEmail = textBox.Text;
                            break;
                    }
                }
            }
        }

        private void CustSupt_ChkPreviewKeyUp(object sender, RoutedEventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            DataGrid dataGrid = noteHelper.FindDataGrid(activeCheckBox);
            int selectedRowIndex = AdminVM.SelectedSuptRowIndex;

            Grid parentGrid = activeCheckBox.Parent as Grid;
            TextBlock supID = parentGrid.Children[1] as TextBlock;
            if (dataGrid != null)
            {
                if (!string.IsNullOrEmpty(supID.Text))
                    AdminVM.SelectedCustSupID = int.Parse(supID.Text);
                if (selectedRowIndex == dataGrid.Items.Count - 1)
                {
                    AdminVM.ActionSupState = "CreateSup";
                    if (activeCheckBox.IsChecked == true)
                        AdminVM.CurrentSupActive = true;
                    else AdminVM.CurrentSupActive = false;

                    ObservableCollection<Superintendent> custSupts = AdminVM.CustSupts;
                    Superintendent item = new Superintendent();
                    custSupts.Add(item);
                    e.Handled = true;
                }
                else
                {
                    AdminVM.ActionSupState = "UpdateSup";
                    if (activeCheckBox.IsChecked == true)
                        AdminVM.CurrentSupActive = true;
                    else AdminVM.CurrentSupActive = false;
                }
            }
        }

        private void CustContact_PreviewKeyUp(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                int selectedRowIndex = AdminVM.SelectedSubmRowIndex;

                Grid parentGrid = textBox.Parent as Grid;
                TextBlock submID = parentGrid.Children[1] as TextBlock;
                AdminVM.SelectedCustSubmID = int.Parse(submID.Text);

                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        AdminVM.ActionSubmState = "CreateSubm";
                        // add new row
                        ObservableCollection<CustomerContact> custPMs = AdminVM.CustContacts;
                        CustomerContact item = new CustomerContact();
                        custPMs.Add(item);
                        e.Handled = true;
                    }
                    else
                    {
                        AdminVM.ActionSubmState = "UpdateSubm";
                    }

                    string itemName = textBox.Tag as string;

                    switch (itemName)
                    {
                        case "SubmName":
                            AdminVM.CurrentSubmName = textBox.Text;
                            break;
                        case "SubmPhone":
                            AdminVM.CurrentSubmPhone = textBox.Text;
                            break;
                        case "SubmCell":
                            AdminVM.CurrentSubmCell = textBox.Text;
                            break;
                        case "SubmEmail":
                            AdminVM.CurrentSubmEmail = textBox.Text;
                            break;
                    }
                }
            }
        }

        private void CustContact_ChkPreviewKeyUp(object sender, RoutedEventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            DataGrid dataGrid = noteHelper.FindDataGrid(activeCheckBox);
            int selectedRowIndex = AdminVM.SelectedSubmRowIndex;

            Grid parentGrid = activeCheckBox.Parent as Grid;
            TextBlock ccID = parentGrid.Children[1] as TextBlock;

            if (dataGrid != null)
            {
                if (!string.IsNullOrEmpty(ccID.Text))
                    AdminVM.SelectedCustSubmID = int.Parse(ccID.Text);
                if (selectedRowIndex == dataGrid.Items.Count - 1)
                {
                    AdminVM.ActionSubmState = "CreateSubm";

                    if (activeCheckBox.IsChecked == true)
                        AdminVM.CurrentSubmActive = true;
                    else AdminVM.CurrentSubmActive = false;

                    ObservableCollection<CustomerContact> custPMs = AdminVM.CustContacts;
                    CustomerContact item = new CustomerContact();
                    custPMs.Add(item);
                    e.Handled = true;
                }
                else
                {
                    AdminVM.ActionSubmState = "UpdateSubm";
                    if (activeCheckBox.IsChecked == true)
                        AdminVM.CurrentSubmActive = true;
                    else AdminVM.CurrentSubmActive = false;
                }
            }
        }

        private void CustPM_PreviewKeyUp(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                int selectedRowIndex = AdminVM.SelectedPMRowIndex;
                Grid parentGrid = textBox.Parent as Grid;
                TextBlock pmID = parentGrid.Children[1] as TextBlock;
               
                AdminVM.SelectedCustPmID = int.Parse(pmID.Text);
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        AdminVM.ActionPmState = "CreatePM";
                        // add new row
                        ObservableCollection<ProjectManager> custPMs = AdminVM.CustPMs;
                        ProjectManager item = new ProjectManager();
                        custPMs.Add(item);
                        e.Handled = true;
                    }
                    else
                    {
                        AdminVM.ActionPmState = "UpdatePM";
                    }
                    string itemName = textBox.Tag as string;

                    switch (itemName)
                    {
                        case "PmName":
                            AdminVM.CurrentPmName = textBox.Text;
                            break;
                        case "PmPhone":
                            AdminVM.CurrentPmPhone = textBox.Text;
                            break;
                        case "PmCell":
                            AdminVM.CurrentPmCell = textBox.Text;
                            break;
                        case "PmEmail":
                            AdminVM.CurrentPmEmail = textBox.Text;
                            break;
                    }
                }
            }
        }

        private void CustPM_ChkPreviewKeyUp(object sender, RoutedEventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            DataGrid dataGrid = noteHelper.FindDataGrid(activeCheckBox);
            int selectedRowIndex = AdminVM.SelectedPMRowIndex;

            Grid parentGrid = activeCheckBox.Parent as Grid;
            TextBlock pmID = parentGrid.Children[1] as TextBlock;

            if (dataGrid != null)
            {
                if(!string.IsNullOrEmpty(pmID.Text))
                    AdminVM.SelectedCustPmID = int.Parse(pmID.Text);
                if (selectedRowIndex == dataGrid.Items.Count - 1)
                {
                    AdminVM.ActionPmState = "CreatePM";
                    if (activeCheckBox.IsChecked == true)
                        AdminVM.CurrentPmActive = true;
                    else AdminVM.CurrentPmActive = false;

                    ObservableCollection<ProjectManager> custPMs = AdminVM.CustPMs;
                    ProjectManager item = new ProjectManager();
                    custPMs.Add(item);
                    e.Handled = true;
                }
                else
                {
                    AdminVM.ActionPmState = "UpdatePM";
                    if (activeCheckBox.IsChecked == true)
                        AdminVM.CurrentPmActive = true;
                    else AdminVM.CurrentPmActive = false;
                }
            }
        }

        private void ManufNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (AdminVM.SelectedManufID != 0)
            {
                TextBox NotesNote = sender as TextBox;
                if (NotesNote != null)
                {
                    DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);
                    int selectedRowIndex = AdminVM.SelectedManufNoteRowIndex;

                    if (dataGrid != null)
                    {
                        Grid parentGrid = NotesNote.Parent as Grid;
                        Grid grandParentGrid = parentGrid.Parent as Grid;
                        Grid firstChildGrid = grandParentGrid.Children[0] as Grid;
                        TextBlock NotesDateAdded = firstChildGrid.Children[0] as TextBlock;
                        TextBlock NoteUserName = firstChildGrid.Children[1] as TextBlock;
                        TextBlock NoteID = firstChildGrid.Children[2] as TextBlock;

                        if (selectedRowIndex == dataGrid.Items.Count - 1)
                        {
                            ObservableCollection<Note> notes = AdminVM.ManufNotes;
                            NotesDateAdded.Text = DateTime.Now.ToString();
                            NoteUserName.Text = "smile";

                            // Create Note
                            AdminVM.ActionNoteState = "CreateNote";
                            AdminVM.CurrentNotesDateAdded = DateTime.Now;
                            AdminVM.CurrnetNoteUser = "smile";
                            AdminVM.CurrentNotesDesc = "Manuf";
                            AdminVM.CurrentNotesNote = NotesNote.Text;
                            NoteID.Text = AdminVM.CurrentNoteID.ToString();
                            Note item = new Note();
                            notes.Add(item);
                        }
                        else
                        {
                            // Update Note
                            AdminVM.ActionNoteState = "UpdateNote";
                            AdminVM.CurrentNoteID = int.Parse(NoteID.Text);
                            AdminVM.CurrentNotesDateAdded = DateTime.Now;
                            AdminVM.CurrnetNoteUser = "smile";
                            AdminVM.CurrentNotesDesc = "Manuf";
                            AdminVM.CurrentNotesNote = NotesNote.Text;
                        }

                        e.Handled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please make your selection first");
            }
        }
        
        private void CustomerNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (AdminVM.SelectedCustomerID != 0)
            {
                TextBox NotesNote = sender as TextBox;
                if (NotesNote != null)
                {
                    DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);
                    int selectedRowIndex = AdminVM.SelectedCustNoteRowIndex;
                    if (dataGrid != null)
                    {
                        Grid parentGrid = NotesNote.Parent as Grid;
                        Grid grandParentGrid = parentGrid.Parent as Grid;
                        Grid firstChildGrid = grandParentGrid.Children[0] as Grid;
                        TextBlock NotesDateAdded = firstChildGrid.Children[0] as TextBlock;
                        TextBlock NoteUserName = firstChildGrid.Children[1] as TextBlock;
                        TextBlock NoteID = firstChildGrid.Children[2] as TextBlock;
                        
                        if (selectedRowIndex == dataGrid.Items.Count - 1)
                        {
                            ObservableCollection<Note> notes = AdminVM.CustomerNotes;
                            NotesDateAdded.Text = DateTime.Now.ToString();
                            NoteUserName.Text = "smile";

                            // Create Note
                            AdminVM.ActionNoteState = "CreateNote";
                            AdminVM.CurrentNotesDateAdded = DateTime.Now;
                            AdminVM.CurrnetNoteUser = "smile";
                            AdminVM.CurrentNotesDesc = "Customer";
                            AdminVM.CurrentNotesNote = NotesNote.Text;
                            NoteID.Text = AdminVM.CurrentNoteID.ToString();
                            Note item = new Note();
                            notes.Add(item);
                        }
                        else
                        {
                            // Update Note
                            AdminVM.ActionNoteState = "UpdateNote";
                            AdminVM.CurrentNoteID = int.Parse(NoteID.Text);
                            AdminVM.CurrentNotesDateAdded = DateTime.Now;
                            AdminVM.CurrnetNoteUser = "smile";
                            AdminVM.CurrentNotesDesc = "Customer";
                            AdminVM.CurrentNotesNote = NotesNote.Text;
                        }

                        e.Handled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please make your selection first");
            }
        }

        private void PMNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (AdminVM.SelectedCustPmID != 0)
            {
                TextBox NotesNote = sender as TextBox;
                if (NotesNote != null)
                {
                    DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);
                    int selectedRowIndex = AdminVM.SelectedPMNoteRowIndex;

                    if (dataGrid != null)
                    {
                        Grid parentGrid = NotesNote.Parent as Grid;
                        Grid grandParentGrid = parentGrid.Parent as Grid;
                        Grid firstChildGrid = grandParentGrid.Children[0] as Grid;
                        TextBlock NotesDateAdded = firstChildGrid.Children[0] as TextBlock;
                        TextBlock NoteUserName = firstChildGrid.Children[1] as TextBlock;
                        TextBlock NoteID = firstChildGrid.Children[2] as TextBlock;

                        if (selectedRowIndex == dataGrid.Items.Count - 1)
                        {
                            ObservableCollection<Note> notes = AdminVM.CustPMNotes;
                            NotesDateAdded.Text = DateTime.Now.ToString();
                            NoteUserName.Text = "smile";

                            // Create Note
                            AdminVM.ActionNoteState = "CreateNote";
                            AdminVM.CurrentNotesDateAdded = DateTime.Now;
                            AdminVM.CurrnetNoteUser = "smile";
                            AdminVM.CurrentNotesDesc = "ProjectManager";
                            AdminVM.CurrentNotesNote = NotesNote.Text;
                            NoteID.Text = AdminVM.CurrentNoteID.ToString();
                            Note item = new Note();
                            notes.Add(item);
                        }
                        else
                        {
                            // Update Note
                            AdminVM.ActionNoteState = "UpdateNote";
                            AdminVM.CurrentNoteID = int.Parse(NoteID.Text);
                            AdminVM.CurrentNotesDateAdded = DateTime.Now;
                            AdminVM.CurrnetNoteUser = "smile";
                            AdminVM.CurrentNotesDesc = "ProjectManager";
                            AdminVM.CurrentNotesNote = NotesNote.Text;
                        }

                        e.Handled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please make your selection first");
            }
        }

        private void SuptNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (AdminVM.SelectedCustSupID != 0)
            {
                TextBox NotesNote = sender as TextBox;
                if (NotesNote != null)
                {
                    DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);
                    int selectedRowIndex = AdminVM.SelectedSuptNoteRowIndex;

                    if (dataGrid != null)
                    {
                        Grid parentGrid = NotesNote.Parent as Grid;
                        Grid grandParentGrid = parentGrid.Parent as Grid;
                        Grid firstChildGrid = grandParentGrid.Children[0] as Grid;
                        TextBlock NotesDateAdded = firstChildGrid.Children[0] as TextBlock;
                        TextBlock NoteUserName = firstChildGrid.Children[1] as TextBlock;
                        TextBlock NoteID = firstChildGrid.Children[2] as TextBlock;

                        if (selectedRowIndex == dataGrid.Items.Count - 1)
                        {
                            ObservableCollection<Note> notes = AdminVM.CustSuptNotes;
                            NotesDateAdded.Text = DateTime.Now.ToString();
                            NoteUserName.Text = "smile";

                            // Create Note
                            AdminVM.ActionNoteState = "CreateNote";
                            AdminVM.CurrentNotesDateAdded = DateTime.Now;
                            AdminVM.CurrnetNoteUser = "smile";
                            AdminVM.CurrentNotesDesc = "Superintendent";
                            AdminVM.CurrentNotesNote = NotesNote.Text;
                            NoteID.Text = AdminVM.CurrentNoteID.ToString();
                            Note item = new Note();
                            notes.Add(item);
                        }
                        else
                        {
                            // Update Note
                            AdminVM.ActionNoteState = "UpdateNote";
                            AdminVM.CurrentNoteID = int.Parse(NoteID.Text);
                            AdminVM.CurrentNotesDateAdded = DateTime.Now;
                            AdminVM.CurrnetNoteUser = "smile";
                            AdminVM.CurrentNotesDesc = "Superintendent";
                            AdminVM.CurrentNotesNote = NotesNote.Text;
                        }

                        e.Handled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please make your selection first");
            }
        }

        private void SubmNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (AdminVM.SelectedCustSubmID != 0)
            {
                TextBox NotesNote = sender as TextBox;
                if (NotesNote != null)
                {
                    DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);
                    int selectedRowIndex = AdminVM.SelectedSubmNoteRowIndex;

                    if (dataGrid != null)
                    {
                        Grid parentGrid = NotesNote.Parent as Grid;
                        Grid grandParentGrid = parentGrid.Parent as Grid;
                        Grid firstChildGrid = grandParentGrid.Children[0] as Grid;
                        TextBlock NotesDateAdded = firstChildGrid.Children[0] as TextBlock;
                        TextBlock NoteUserName = firstChildGrid.Children[1] as TextBlock;
                        TextBlock NoteID = firstChildGrid.Children[2] as TextBlock;

                        if (selectedRowIndex == dataGrid.Items.Count - 1)
                        {
                            ObservableCollection<Note> notes = AdminVM.CustSubmNotes;
                            NotesDateAdded.Text = DateTime.Now.ToString();
                            NoteUserName.Text = "smile";

                            // Create Note
                            AdminVM.ActionNoteState = "CreateNote";
                            AdminVM.CurrentNotesDateAdded = DateTime.Now;
                            AdminVM.CurrnetNoteUser = "smile";
                            AdminVM.CurrentNotesDesc = "CustomerContact";
                            AdminVM.CurrentNotesNote = NotesNote.Text;
                            NoteID.Text = AdminVM.CurrentNoteID.ToString();
                            Note item = new Note();
                            notes.Add(item);
                        }
                        else
                        {
                            // Update Note
                            AdminVM.ActionNoteState = "UpdateNote";
                            AdminVM.CurrentNoteID = int.Parse(NoteID.Text);
                            AdminVM.CurrentNotesDateAdded = DateTime.Now;
                            AdminVM.CurrnetNoteUser = "smile";
                            AdminVM.CurrentNotesDesc = "CustomerContact";
                            AdminVM.CurrentNotesNote = NotesNote.Text;
                        }

                        e.Handled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please make your selection first");
            }
        }
    }
}
