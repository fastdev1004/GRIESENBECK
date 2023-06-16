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
            if (AdminVM.SelectedCustomerID != 0)
            {
                TextBox textBox = sender as TextBox;
                if (textBox != null)
                {
                    DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                    int selectedRowIndex = AdminVM.SelectedSuptRowIndex;

                    Grid parentGrid = textBox.Parent as Grid;
                    TextBlock supID = parentGrid.Children[1] as TextBlock;

                    if (dataGrid != null)
                    {
                        if (selectedRowIndex == dataGrid.Items.Count - 1)
                        {
                            AdminVM.CurrentSupName = "";
                            AdminVM.CurrentSupPhone = "";
                            AdminVM.CurrentSupCell = "";
                            AdminVM.CurrentSupEmail = "";
                            ObservableCollection<Superintendent> custSupts = AdminVM.CustSupts;
                            Superintendent item = new Superintendent();
                            custSupts.Add(item);

                            AdminVM.ActionSupName = "CreateSup";
                            AdminVM.ActionState = "AddRow";
                        }
                        else if (AdminVM.ActionState.Equals("AddRow"))
                        {
                            AdminVM.SelectedCustSupID = int.Parse(supID.Text);
                            AdminVM.ActionState = "AddRow";
                            AdminVM.ActionSupName = "UpdateSup";
                        }
                        else if (AdminVM.ActionState.Equals("UpdateRow"))
                        {
                            AdminVM.SelectedCustSupID = int.Parse(supID.Text);
                            AdminVM.ActionState = "UpdateRow";
                            AdminVM.ActionSupName = "UpdateSup";
                        }

                        string itemName = textBox.Tag as string;
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
                        e.Handled = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please make your selection first");
            }
        }

        private void CustSupt_ChkPreviewKeyUp(object sender, RoutedEventArgs e)
        {
            if (AdminVM.SelectedCustomerID != 0)
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
                        AdminVM.ActionSupName = "CreateSup";

                        AdminVM.CurrentSupName = "";
                        AdminVM.CurrentSupPhone = "";
                        AdminVM.CurrentSupCell = "";
                        AdminVM.CurrentSupEmail = "";

                        ObservableCollection<Superintendent> custSupts = AdminVM.CustSupts;
                        Superintendent item = new Superintendent();
                        custSupts.Add(item);
                        AdminVM.ActionSupName = "CreateSup";
                        AdminVM.ActionState = "AddRow";
                    }
                    else if (AdminVM.ActionState.Equals("AddRow"))
                    {
                        AdminVM.ActionState = "AddRow";
                        AdminVM.ActionSupName = "UpdateSup";
                    }
                    else if (AdminVM.ActionState.Equals("UpdateRow"))
                    {
                        AdminVM.ActionState = "UpdateRow";
                        AdminVM.ActionSupName = "UpdateSup";
                    }

                    if (activeCheckBox.IsChecked == true)
                        AdminVM.CurrentSupActive = true;
                    else AdminVM.CurrentSupActive = false;
                    e.Handled = false;
                }
            }
            else
            {
                MessageBox.Show("Please make your selection first");
            }
        }

        private void CustContact_PreviewKeyUp(object sender, RoutedEventArgs e)
        {
            if (AdminVM.SelectedCustomerID != 0)
            {
                TextBox textBox = sender as TextBox;
                if (textBox != null)
                {
                    DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                    int selectedRowIndex = AdminVM.SelectedSubmRowIndex;

                    Grid parentGrid = textBox.Parent as Grid;
                    TextBlock submID = parentGrid.Children[1] as TextBlock;

                    if (dataGrid != null)
                    {
                        if (selectedRowIndex == dataGrid.Items.Count - 1)
                        {
                            AdminVM.CurrentSubmName = "";
                            AdminVM.CurrentSubmPhone = "";
                            AdminVM.CurrentSubmCell = "";
                            AdminVM.CurrentSubmEmail = "";

                            // add new row
                            ObservableCollection<CustomerContact> custPMs = AdminVM.CustContacts;
                            CustomerContact item = new CustomerContact();
                            custPMs.Add(item);
                            AdminVM.ActionSubmName = "CreateSubm";
                            AdminVM.ActionState = "AddRow";
                        }
                        else if (AdminVM.ActionState.Equals("AddRow"))
                        {

                            AdminVM.SelectedCustSubmID = int.Parse(submID.Text);
                            AdminVM.ActionState = "AddRow";
                            AdminVM.ActionPmName = "UpdateSubm";
                        }
                        else if (AdminVM.ActionState.Equals("UpdateRow"))
                        {
                            AdminVM.SelectedCustSubmID = int.Parse(submID.Text);
                            AdminVM.ActionState = "UpdateRow";
                            AdminVM.ActionSubmName = "UpdateSubm";
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
                        e.Handled = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please make your selection first");
            }
        }

        private void CustContact_ChkPreviewKeyUp(object sender, RoutedEventArgs e)
        {
            if (AdminVM.SelectedCustomerID != 0)
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
                        AdminVM.CurrentSubmName = "";
                        AdminVM.CurrentSubmPhone = "";
                        AdminVM.CurrentSubmCell = "";
                        AdminVM.CurrentSubmEmail = "";

                        ObservableCollection<CustomerContact> custPMs = AdminVM.CustContacts;
                        CustomerContact item = new CustomerContact();
                        custPMs.Add(item);

                        AdminVM.ActionSubmName = "CreateSubm";
                        AdminVM.ActionState = "AddRow";
                        e.Handled = true;
                    }
                    else if (AdminVM.ActionState.Equals("AddRow"))
                    {
                        AdminVM.ActionState = "AddRow";
                        AdminVM.ActionPmName = "UpdateSubm";
                    }
                    else if (AdminVM.ActionState.Equals("UpdateRow"))
                    {
                        AdminVM.ActionState = "UpdateRow";
                        AdminVM.ActionSubmName = "UpdateSubm";
                    }

                    if (activeCheckBox.IsChecked == true)
                        AdminVM.CurrentSubmActive = true;
                    else AdminVM.CurrentSubmActive = false;
                }
            }
            else
            {
                MessageBox.Show("Please make your selection first");
            }
        }

        private void CustPM_PreviewKeyUp(object sender, RoutedEventArgs e)
        {
            if (AdminVM.SelectedCustomerID != 0)
            {
                TextBox textBox = sender as TextBox;
                if (textBox != null)
                {
                    DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                    int selectedRowIndex = AdminVM.SelectedPMRowIndex;
                    Grid parentGrid = textBox.Parent as Grid;
                    TextBlock pmID = parentGrid.Children[1] as TextBlock;

                    if (dataGrid != null)
                    {
                        if (selectedRowIndex == dataGrid.Items.Count - 1)
                        {
                            // add new row
                            AdminVM.CurrentPmName = "";
                            AdminVM.CurrentPmPhone = "";
                            AdminVM.CurrentPmCell = "";
                            AdminVM.CurrentPmEmail = "";
                            ObservableCollection<ProjectManager> custPMs = AdminVM.CustPMs;
                            ProjectManager item = new ProjectManager();
                            custPMs.Add(item);
                            AdminVM.ActionPmName = "CreatePM";
                            AdminVM.ActionState = "AddRow";
                        }
                        else if (AdminVM.ActionState.Equals("AddRow"))
                        {

                            AdminVM.SelectedCustPmID = int.Parse(pmID.Text);
                            AdminVM.ActionState = "AddRow";
                            AdminVM.ActionPmName = "UpdatePM";
                        }
                        else if (AdminVM.ActionState.Equals("UpdateRow"))
                        {
                            AdminVM.SelectedCustPmID = int.Parse(pmID.Text);
                            AdminVM.ActionState = "UpdateRow";
                            AdminVM.ActionPmName = "UpdatePM";
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
                        e.Handled = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please make your selection first");
            }
        }

        private void CustPM_ChkPreviewKeyUp(object sender, RoutedEventArgs e)
        {
            if (AdminVM.SelectedCustomerID != 0)
            {
                CheckBox activeCheckBox = sender as CheckBox;
                DataGrid dataGrid = noteHelper.FindDataGrid(activeCheckBox);
                int selectedRowIndex = AdminVM.SelectedPMRowIndex;

                Grid parentGrid = activeCheckBox.Parent as Grid;
                TextBlock pmID = parentGrid.Children[1] as TextBlock;

                if (dataGrid != null)
                {
                    if (!string.IsNullOrEmpty(pmID.Text))
                        AdminVM.SelectedCustPmID = int.Parse(pmID.Text);
                    if ((selectedRowIndex == dataGrid.Items.Count - 1) && AdminVM.ActionPmName.CompareTo("UpdatePM") != 0)
                    {
                        AdminVM.ActionPmName = "CreatePM";
                        if (activeCheckBox.IsChecked == true)
                            AdminVM.CurrentPmActive = true;
                        else AdminVM.CurrentPmActive = false;

                        ObservableCollection<ProjectManager> custPMs = AdminVM.CustPMs;
                        ProjectManager item = new ProjectManager();
                        custPMs.Add(item);
                    }
                    else
                    {
                        AdminVM.ActionPmName = "UpdatePM";
                        if (activeCheckBox.IsChecked == true)
                            AdminVM.CurrentPmActive = true;
                        else AdminVM.CurrentPmActive = false;
                    }
                    e.Handled = false;
                }
            }
            else
            {
                MessageBox.Show("Please make your selection first");
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


        private void CustomerTab_PreviewKeyUp(object sender, KeyEventArgs e)
        {
           
                TextBox textBox = sender as TextBox;
                if (textBox != null)
                {
                    DataGrid dataGrid = noteHelper.FindDataGrid(textBox);

                    if (dataGrid != null)
                    {
                        string itemName = textBox.Tag as string;
                        AdminVM.UpdateComponent = "Table";

                        int selectedRowIndex = AdminVM.SelectedCustRowIndex;
                        if (AdminVM.SelectedTempCustIndex == -1)
                        {
                            AdminVM.SelectedTempCustIndex = selectedRowIndex;
                        }
                        else if (AdminVM.SelectedTempCustIndex != selectedRowIndex)
                        {
                            AdminVM.TempCustomer = new Customer();
                            AdminVM.SelectedTempCustIndex = selectedRowIndex;
                        }

                        Customer item = dataGrid.Items[selectedRowIndex] as Customer;

                        if (string.IsNullOrEmpty(AdminVM.TempCustomer.FullName))
                        {
                            AdminVM.TempCustomer.ID = item.ID;
                            AdminVM.TempCustomer.FullName = item.FullName;
                            AdminVM.TempCustomer.ShortName = item.ShortName;
                            AdminVM.TempCustomer.PoBox = item.PoBox;
                            AdminVM.TempCustomer.Address = item.Address;
                            AdminVM.TempCustomer.City = item.City;
                            AdminVM.TempCustomer.State = item.State;
                            AdminVM.TempCustomer.Zip = item.Zip;
                            AdminVM.TempCustomer.Phone = item.Phone;
                            AdminVM.TempCustomer.Fax = item.Fax;
                            AdminVM.TempCustomer.Active = item.Active;
                        }

                        switch (itemName)
                        {
                            case "ShortName":
                                AdminVM.TempCustomer.ShortName = textBox.Text;
                                break;
                            case "FullName":
                                AdminVM.TempCustomer.FullName = textBox.Text;
                                break;
                            case "PoNumber":
                                AdminVM.TempCustomer.PoBox = textBox.Text;
                                break;
                            case "Address":
                                AdminVM.TempCustomer.Address = textBox.Text;
                                break;
                            case "City":
                                AdminVM.TempCustomer.City = textBox.Text;
                                break;
                            case "State":
                                AdminVM.TempCustomer.State = textBox.Text;
                                break;
                            case "Zip":
                                AdminVM.TempCustomer.Zip = textBox.Text;
                                break;
                            case "Phone":
                                AdminVM.TempCustomer.Phone = textBox.Text;
                                break;
                            case "Fax":
                                AdminVM.TempCustomer.Fax = textBox.Text;
                                break;
                        }

                        AdminVM.UpdateCustomer();
                    }
                }
                e.Handled = true;
          
        }

        private void CustomerTab_ChkPreviewKeyUp(object sender, RoutedEventArgs e)
        {
            {
                CheckBox activeCheckBox = sender as CheckBox;
                DataGrid dataGrid = noteHelper.FindDataGrid(activeCheckBox);
                if (dataGrid != null)
                {
                    AdminVM.UpdateComponent = "Table";
                    int selectedRowIndex = AdminVM.SelectedCustRowIndex;
                    if (AdminVM.SelectedTempCustIndex == -1)
                    {
                        AdminVM.SelectedTempCustIndex = selectedRowIndex;
                    }
                    else if (AdminVM.SelectedTempCustIndex != selectedRowIndex)
                    {
                        AdminVM.TempCustomer = new Customer();
                        AdminVM.SelectedTempCustIndex = selectedRowIndex;
                    }

                    Customer item = dataGrid.Items[selectedRowIndex] as Customer;

                    if (string.IsNullOrEmpty(AdminVM.TempCustomer.FullName))
                    {
                        AdminVM.TempCustomer.ID = item.ID;
                        AdminVM.TempCustomer.FullName = item.FullName;
                        AdminVM.TempCustomer.ShortName = item.ShortName;
                        AdminVM.TempCustomer.PoBox = item.PoBox;
                        AdminVM.TempCustomer.Address = item.Address;
                        AdminVM.TempCustomer.City = item.City;
                        AdminVM.TempCustomer.State = item.State;
                        AdminVM.TempCustomer.Zip = item.Zip;
                        AdminVM.TempCustomer.Phone = item.Phone;
                        AdminVM.TempCustomer.Fax = item.Fax;
                        AdminVM.TempCustomer.Active = item.Active;
                    }

                    if (activeCheckBox.IsChecked == true)
                        AdminVM.TempCustomer.Active = true;
                    else AdminVM.TempCustomer.Active = false;

                    AdminVM.UpdateCustomer();
                }
                e.Handled = true;
                }
        }
    }
}
