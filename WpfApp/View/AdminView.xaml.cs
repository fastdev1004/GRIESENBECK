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
using Microsoft.Win32;

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

        private void CustSupt_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (AdminVM.SelectedCustomerID != 0)
            {
                if (textBox != null)
                    {
                        DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                        int selectedRowIndex = dataGrid.SelectedIndex;
                        int rowIndex = selectedRowIndex;

                        string itemName = textBox.Tag as string;
                        if (AdminVM.ActionSubState.Equals("AddRow") || (AdminVM.ActionSubState.Equals("UpdateRow") && selectedRowIndex == -1))
                        {
                            rowIndex = AdminVM.CurrentIndex;
                        }
                        else
                        {
                            rowIndex = selectedRowIndex;
                        }

                        Superintendent newRow = dataGrid.Items[rowIndex] as Superintendent;

                        if (dataGrid != null)
                        {
                            if (selectedRowIndex == dataGrid.Items.Count - 1)
                            {
                                ObservableCollection<Superintendent> supts = AdminVM.CustSupts;
                                Superintendent item = new Superintendent();
                                supts.Add(item);
                                AdminVM.ActionSubState = "AddRow";
                                AdminVM.TempSup = new Superintendent();
                                switch (itemName)
                                {
                                    case "SupName":
                                        AdminVM.TempSup.SupName = textBox.Text;
                                        break;
                                    case "SupPhone":
                                        AdminVM.TempSup.SupPhone = textBox.Text;
                                        break;
                                    case "SupCell":
                                        AdminVM.TempSup.SupCellPhone = textBox.Text;
                                        break;
                                    case "SupEmail":
                                        AdminVM.TempSup.SupEmail = textBox.Text;
                                        break;
                                }
                                AdminVM.CreateCustSup();
                            }
                            else
                            {
                                Superintendent _supt = dataGrid.Items[rowIndex] as Superintendent;
                                AdminVM.TempSup = _supt;
                                switch (itemName)
                                {
                                    case "SupName":
                                        AdminVM.TempSup.SupName = textBox.Text;
                                        break;
                                    case "SupPhone":
                                        AdminVM.TempSup.SupPhone = textBox.Text;
                                        break;
                                    case "SupCell":
                                        AdminVM.TempSup.SupCellPhone = textBox.Text;
                                        break;
                                    case "SupEmail":
                                        AdminVM.TempSup.SupEmail = textBox.Text;
                                        break;
                                }
                                AdminVM.ActionSubState = "UpdateRow";
                                AdminVM.UpdateCustSup();
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

        private void CustSupActive_CheckBox(object sender, RoutedEventArgs e)
        {
            if (AdminVM.SelectedCustomerID != 0)
            {
                if (AdminVM.SelectedCustSupID != 0)
                {
                    CheckBox chkBox = sender as CheckBox;

                    DataGrid dataGrid = noteHelper.FindDataGrid(chkBox);
                    int selectedRowIndex = dataGrid.SelectedIndex;
                    int rowIndex = selectedRowIndex;

                    if (AdminVM.ActionSubState.Equals("AddRow") || (AdminVM.ActionSubState.Equals("UpdateRow") && selectedRowIndex == -1))
                    {
                        rowIndex = AdminVM.CurrentIndex;
                    }
                    else
                    {
                        rowIndex = selectedRowIndex;
                    }

                    if (rowIndex >= 0)
                    {
                        Superintendent newRow = dataGrid.Items[rowIndex] as Superintendent;
                        if (dataGrid != null)
                        {
                            if (selectedRowIndex == dataGrid.Items.Count - 1)
                            {
                                ObservableCollection<Superintendent> custSups = AdminVM.CustSupts;
                                Superintendent item = new Superintendent();
                                custSups.Add(item);
                                AdminVM.ActionSubState = "AddRow";
                                AdminVM.TempSup = new Superintendent();

                                if (chkBox.IsChecked == true)
                                    AdminVM.TempSup.Active = true;
                                else AdminVM.TempSup.Active = false;

                                AdminVM.CreateCustSup();
                            }
                            else
                            {
                                Superintendent _supt = dataGrid.Items[rowIndex] as Superintendent;
                                AdminVM.TempSup = _supt;

                                if (chkBox.IsChecked == true)
                                    AdminVM.TempSup.Active = true;
                                else AdminVM.TempSup.Active = false;
                                AdminVM.ActionSubState = "UpdateRow";

                                AdminVM.UpdateCustSup();
                            }

                            e.Handled = true;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please make your Superindent selection first");
                }
            }
            else
            {
                MessageBox.Show("Please make your selection first");
            }
        }

        private void CustContact_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (AdminVM.SelectedCustomerID != 0)
            {
                if (textBox != null)
                {
                    DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                    int selectedRowIndex = dataGrid.SelectedIndex;
                    int rowIndex = selectedRowIndex;

                    string itemName = textBox.Tag as string;
                    if (AdminVM.ActionSubState.Equals("AddRow") || (AdminVM.ActionSubState.Equals("UpdateRow") && selectedRowIndex == -1))
                    {
                        rowIndex = AdminVM.CurrentIndex;
                    }
                    else
                    {
                        rowIndex = selectedRowIndex;
                    }

                    CustomerContact newRow = dataGrid.Items[rowIndex] as CustomerContact;

                    if (dataGrid != null)
                    {
                        if (selectedRowIndex == dataGrid.Items.Count - 1)
                        {
                            ObservableCollection<CustomerContact> customerContacts = AdminVM.CustContacts;
                            CustomerContact item = new CustomerContact();
                            customerContacts.Add(item);
                            AdminVM.ActionSubState = "AddRow";
                            AdminVM.TempCC = new CustomerContact();
                            switch (itemName)
                            {
                                case "SubmName":
                                    AdminVM.TempCC.CCName= textBox.Text;
                                    break;
                                case "SubmPhone":
                                    AdminVM.TempCC.CCPhone = textBox.Text;
                                    break;
                                case "SubmCell":
                                    AdminVM.TempCC.CCCell = textBox.Text;
                                    break;
                                case "SubmEmail":
                                    AdminVM.TempCC.CCEmail = textBox.Text;
                                    break;
                            }
                            AdminVM.CreateCustCC();
                        }
                        else
                        {
                            CustomerContact _customerContact = dataGrid.Items[rowIndex] as CustomerContact;
                            AdminVM.TempCC = _customerContact;
                            switch (itemName)
                            {
                                case "SubmName":
                                    AdminVM.TempCC.CCName = textBox.Text;
                                    break;
                                case "SubmPhone":
                                    AdminVM.TempCC.CCPhone = textBox.Text;
                                    break;
                                case "SubmCell":
                                    AdminVM.TempCC.CCCell = textBox.Text;
                                    break;
                                case "SubmEmail":
                                    AdminVM.TempCC.CCEmail = textBox.Text;
                                    break;
                            }
                            AdminVM.ActionSubState = "UpdateRow";
                            AdminVM.UpdateCustCC();
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

        private void CustCCActive_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            DataGrid dataGrid = noteHelper.FindDataGrid(chkBox);
            int selectedRowIndex = dataGrid.SelectedIndex;
            int rowIndex = selectedRowIndex;

            if (AdminVM.ActionSubState.Equals("AddRow") || (AdminVM.ActionSubState.Equals("UpdateRow") && selectedRowIndex == -1))
            {
                rowIndex = AdminVM.CurrentIndex;
            }
            else
            {
                rowIndex = selectedRowIndex;
            }

            if (rowIndex >= 0)
            {
                CustomerContact newRow = dataGrid.Items[rowIndex] as CustomerContact;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<CustomerContact> custCCs = AdminVM.CustContacts;
                        CustomerContact item = new CustomerContact();
                        custCCs.Add(item);
                        AdminVM.ActionSubState = "AddRow";
                        AdminVM.TempCC = new CustomerContact();

                        if (chkBox.IsChecked == true)
                            AdminVM.TempCC.CCActive = true;
                        else AdminVM.TempCC.CCActive = false;

                        AdminVM.CreateCustCC();
                    }
                    else
                    {
                        CustomerContact _customerContact = dataGrid.Items[rowIndex] as CustomerContact;
                        AdminVM.TempCC = _customerContact;

                        if (chkBox.IsChecked == true)
                            AdminVM.TempCC.CCActive = true;
                        else AdminVM.TempCC.CCActive = false;
                        AdminVM.ActionSubState = "UpdateRow";

                        AdminVM.UpdateCustCC();
                    }

                    e.Handled = true;
                }
            }
        }

        private void CustPM_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (AdminVM.SelectedCustomerID != 0)
            {
                if (textBox != null)
                    {
                        DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                        int selectedRowIndex = dataGrid.SelectedIndex;
                        int rowIndex = selectedRowIndex;

                        string itemName = textBox.Tag as string;
                        if (AdminVM.ActionSubState.Equals("AddRow") || (AdminVM.ActionSubState.Equals("UpdateRow") && selectedRowIndex == -1))
                        {
                            rowIndex = AdminVM.CurrentIndex;
                        }
                        else
                        {
                            rowIndex = selectedRowIndex;
                        }

                        ProjectManager newRow = dataGrid.Items[rowIndex] as ProjectManager;

                        if (dataGrid != null)
                        {
                            if (selectedRowIndex == dataGrid.Items.Count - 1)
                            {
                                ObservableCollection<ProjectManager> projectManagers = AdminVM.CustPMs;
                                ProjectManager item = new ProjectManager();
                                projectManagers.Add(item);
                                AdminVM.ActionSubState = "AddRow";
                                AdminVM.TempPM = new ProjectManager();
                                switch (itemName)
                                {
                                    case "PmName":
                                        AdminVM.TempPM.PMName = textBox.Text;
                                        break;
                                    case "PmPhone":
                                        AdminVM.TempPM.PMPhone = textBox.Text;
                                        break;
                                    case "PmCell":
                                        AdminVM.TempPM.PMCellPhone = textBox.Text;
                                        break;
                                    case "PmEmail":
                                        AdminVM.TempPM.PMEmail = textBox.Text;
                                        break;
                                }
                                AdminVM.CreateCustPM();
                            }
                            else
                            {
                                ProjectManager _projectManager = dataGrid.Items[rowIndex] as ProjectManager;
                                AdminVM.TempPM = _projectManager;
                                switch (itemName)
                                {
                                    case "PmName":
                                        AdminVM.TempPM.PMName = textBox.Text;
                                        break;
                                    case "PmPhone":
                                        AdminVM.TempPM.PMPhone = textBox.Text;
                                        break;
                                    case "PmCell":
                                        AdminVM.TempPM.PMCellPhone = textBox.Text;
                                        break;
                                    case "PmEmail":
                                        AdminVM.TempPM.PMEmail = textBox.Text;
                                        break;
                                }
                                AdminVM.ActionSubState = "UpdateRow";
                                AdminVM.UpdateCustPM();
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

        private void CustPMActive_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            DataGrid dataGrid = noteHelper.FindDataGrid(chkBox);
            int selectedRowIndex = dataGrid.SelectedIndex;
            int rowIndex = selectedRowIndex;

            if (AdminVM.ActionSubState.Equals("AddRow") || (AdminVM.ActionSubState.Equals("UpdateRow") && selectedRowIndex == -1))
            {
                rowIndex = AdminVM.CurrentIndex;
            }
            else
            {
                rowIndex = selectedRowIndex;
            }

            if (rowIndex >= 0)
            {
                ProjectManager newRow = dataGrid.Items[rowIndex] as ProjectManager;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<ProjectManager> custPms = AdminVM.CustPMs;
                        ProjectManager item = new ProjectManager();
                        custPms.Add(item);
                        AdminVM.ActionSubState = "AddRow";
                        AdminVM.TempPM = new ProjectManager();

                        if (chkBox.IsChecked == true)
                            AdminVM.TempPM.Active = true;
                        else AdminVM.TempPM.Active = false;

                        AdminVM.CreateCustPM();
                    }
                    else
                    {
                        ProjectManager _projectManager = dataGrid.Items[rowIndex] as ProjectManager;
                        AdminVM.TempPM = _projectManager;

                        if (chkBox.IsChecked == true)
                            AdminVM.TempPM.Active = true;
                        else AdminVM.TempPM.Active = false;
                        AdminVM.ActionSubState = "UpdateRow";

                        AdminVM.UpdateCustPM();
                    }

                    e.Handled = true;
                }
            }
        }

        private void Note_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox NotesNote = sender as TextBox;
            string tag = NotesNote.Tag as string;
            ObservableCollection<Note> notes = new ObservableCollection<Note>();
            Note note = new Note();
            string notesDesc = "";
            int selectedID = 0;
            switch (tag)
            {
                case "Customer":
                    notes = AdminVM.CustomerNotes;
                    notesDesc = "Customer";
                    selectedID = AdminVM.SelectedCustomerID;
                    break;
                case "ProjectManager":
                    notes = AdminVM.CustPMNotes;
                    notesDesc = "ProjectManager";
                    selectedID = AdminVM.SelectedCustPmID;
                    break;
                case "Superintendent":
                    notes = AdminVM.CustSuptNotes;
                    notesDesc = "Superintendent";
                    selectedID = AdminVM.SelectedCustSupID;
                    break;
                case "CustomerContact":
                    notes = AdminVM.CustSubmNotes;
                    notesDesc = "CustomerContact";
                    selectedID = AdminVM.SelectedCustSubmID;
                    break;
                case "Manuf":
                    notes = AdminVM.ManufNotes;
                    notesDesc = "Manuf";
                    selectedID = AdminVM.SelectedManufID;
                    break;
                default:
                    break;
            }
            if (selectedID != 0)
            {
                if (NotesNote != null)
                {
                    DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);
                    int selectedRowIndex = dataGrid.SelectedIndex;
                    int rowIndex = selectedRowIndex;
                    Grid parentGrid = NotesNote.Parent as Grid;
                    Grid grandParentGrid = parentGrid.Parent as Grid;
                    Grid firstChildGrid = grandParentGrid.Children[0] as Grid;
                    TextBlock NotesDateAdded = firstChildGrid.Children[0] as TextBlock;
                    TextBlock NoteUserName = firstChildGrid.Children[1] as TextBlock;

                    if (dataGrid != null)
                    {
                        if (AdminVM.ActionNoteState.Equals("AddRow") || (AdminVM.ActionNoteState.Equals("UpdateRow") && selectedRowIndex == -1))
                        {
                            rowIndex = AdminVM.CurrentIndex;
                        }
                        else
                        {
                            rowIndex = selectedRowIndex;
                        }

                        Note newRow = dataGrid.Items[rowIndex] as Note;

                        if (selectedRowIndex == dataGrid.Items.Count - 1)
                        {
                            NotesDateAdded.Text = DateTime.Now.ToString();
                            NoteUserName.Text = "smile";
                            AdminVM.TempNote = new Note();

                            AdminVM.TempNote.NoteUserName = "smile";
                            AdminVM.TempNote.NotesDateAdded = DateTime.Now;
                            AdminVM.TempNote.NotesNote = NotesNote.Text;
                            AdminVM.TempNote.NotesPKDesc = notesDesc;
                            // Create Note
                            AdminVM.ActionNoteState = "AddRow";
                            Note item = new Note();
                            notes.Add(item);

                            AdminVM.CreateNote();
                        }
                        else
                        {
                            // Update Note
                            Note _note = dataGrid.Items[rowIndex] as Note;
                            AdminVM.TempNote.NoteID = _note.NoteID;
                            AdminVM.TempNote.NotesDateAdded = DateTime.Now;
                            AdminVM.TempNote.NoteUser = "smile";
                            AdminVM.TempNote.NotePK = _note.NotePK;
                            AdminVM.TempNote.NotesPKDesc = notesDesc;
                            AdminVM.TempNote.NotesNote = NotesNote.Text;
                            AdminVM.ActionNoteState = "UpdateRow";
                            AdminVM.UpdateNote();
                       }

                        e.Handled = true;
                    }
                }
            }
            else
            {
                NotesNote.Text = "";
                MessageBox.Show("Please make your Customer selection first");
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
                    int selectedIndex = dataGrid.SelectedIndex;
                    if (AdminVM.SelectedTempCustIndex == -1)
                    {
                        AdminVM.SelectedTempCustIndex = selectedRowIndex;
                    }
                    else if (AdminVM.SelectedTempCustIndex != selectedRowIndex)
                    {
                        AdminVM.SelectedTempCustIndex = selectedRowIndex;
                    }
                    Customer item = dataGrid.Items[selectedRowIndex] as Customer;
                  
                    AdminVM.TempCustomer = item;
                    //AdminVM.Customers[selectedRowIndex] = AdminVM.TempCustomer;
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

                if (selectedRowIndex >= 0)
                {
                    Customer item = dataGrid.Items[selectedRowIndex] as Customer;

                    AdminVM.TempCustomer = item;
                    AdminVM.Customers[selectedRowIndex] = AdminVM.TempCustomer;
                    if (activeCheckBox.IsChecked == true)
                        AdminVM.TempCustomer.Active = true;
                    else AdminVM.TempCustomer.Active = false;

                    AdminVM.UpdateCustomer();
                }
            }
        }

        private void AcronymTab_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                int selectedRowIndex = dataGrid.SelectedIndex;
                int rowIndex = selectedRowIndex;

                string itemName = textBox.Tag as string;
                if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
                {
                    rowIndex = AdminVM.CurrentIndex;
                }
                else
                {
                    rowIndex = selectedRowIndex;
                }

                Acronym newRow = dataGrid.Items[rowIndex] as Acronym;

                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        if (!string.IsNullOrEmpty(textBox.Text) && itemName.Equals("AcronymName"))
                        {
                            bool isExisted = false;
                            for (int i = 0; i < dataGrid.Items.Count - 2; i++)
                            {
                                Acronym _acronym = dataGrid.Items[i] as Acronym;
                                if (_acronym.AcronymName.Equals(textBox.Text))
                                {
                                    isExisted = true;
                                }
                            }

                            if (!isExisted)
                            {
                                ObservableCollection<Acronym> acronyms = AdminVM.Acronyms;
                                Acronym item = new Acronym();
                                acronyms.Add(item);
                                AdminVM.ActionState = "AddRow";
                                AdminVM.TempAcronym = new Acronym();
                                switch (itemName)
                                {
                                    case "AcronymDesc":
                                        AdminVM.TempAcronym.AcronymDesc = textBox.Text;
                                        break;
                                    case "AcronymName":
                                        AdminVM.TempAcronym.AcronymName = textBox.Text;
                                        break;
                                }
                                AdminVM.CreateAcronym();
                            }
                            else
                            {
                                MessageBox.Show("Acronym Name is existed.");
                            }

                        }
                        else
                        {
                            newRow.AcronymDesc = "";
                            MessageBox.Show("Acronym Name is required.");
                        }
                    }
                    else
                    {
                        Acronym _acronym = dataGrid.Items[rowIndex] as Acronym;
                        AdminVM.SelectedSovName = _acronym.AcronymName;
                        AdminVM.TempAcronym = _acronym;
                        switch (itemName)
                        {
                            case "AcronymDesc":
                                AdminVM.TempAcronym.AcronymDesc = textBox.Text;
                                break;
                            case "AcronymName":
                                AdminVM.TempAcronym.AcronymName = textBox.Text;
                                break;
                        }
                        AdminVM.ActionState = "UpdateRow";
                        AdminVM.UpdateAcronym();
                    }

                    e.Handled = true;
                }
            }
        }

        private void AcronymTab_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            DataGrid dataGrid = noteHelper.FindDataGrid(chkBox);
            int selectedRowIndex = dataGrid.SelectedIndex;
            int rowIndex = selectedRowIndex;

            if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
            {
                rowIndex = AdminVM.CurrentIndex;
            }
            else
            {
                rowIndex = selectedRowIndex;
            }
         
            if (rowIndex >= 0)
            {
                Acronym newRow = dataGrid.Items[rowIndex] as Acronym;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        if (!string.IsNullOrEmpty(newRow.AcronymName))
                        {
                            ObservableCollection<Acronym> acronyms = AdminVM.Acronyms;
                            Acronym item = new Acronym();
                            acronyms.Add(item);
                            AdminVM.ActionState = "AddRow";
                            AdminVM.TempAcronym = new Acronym();

                            if (chkBox.IsChecked == true)
                                AdminVM.TempAcronym.Active = true;
                            else AdminVM.TempAcronym.Active = false;

                            AdminVM.CreateAcronym();
                        }
                        else
                        {
                            newRow.Active = false;
                            MessageBox.Show("Acronym Name is required.");
                        }
                    }
                    else
                    {
                        Acronym _acronym = dataGrid.Items[rowIndex] as Acronym;
                        AdminVM.SelectedSovName = _acronym.AcronymName;
                        AdminVM.TempAcronym = _acronym;

                        if (chkBox.IsChecked == true)
                            AdminVM.TempAcronym.Active = true;
                        else AdminVM.TempAcronym.Active = false;
                        AdminVM.ActionState = "UpdateRow";

                        AdminVM.UpdateAcronym();
                    }

                    e.Handled = true;
                }
            }
        }

        private void MaterialTab_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                int selectedRowIndex = dataGrid.SelectedIndex;
                int rowIndex = selectedRowIndex;

                string itemName = textBox.Tag as string;
                if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
                {
                    rowIndex = AdminVM.CurrentIndex;
                }
                else
                {
                    rowIndex = selectedRowIndex;
                }

                Material newRow = dataGrid.Items[rowIndex] as Material;

                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        if (!string.IsNullOrEmpty(textBox.Text) && itemName.Equals("MatDesc"))
                        {
                            bool isExisted = false;
                            for (int i = 0; i < dataGrid.Items.Count - 2; i++)
                            {
                                Material _material = dataGrid.Items[i] as Material;
                                if (_material.MatDesc.Equals(textBox.Text))
                                {
                                    isExisted = true;
                                }
                            }

                            if (!isExisted)
                            {
                                ObservableCollection<Material> materials = AdminVM.Materials;
                                Material item = new Material();
                                materials.Add(item);
                                AdminVM.ActionState = "AddRow";
                                AdminVM.TempMaterial = new Material();
                                switch (itemName)
                                {
                                    case "MatDesc":
                                        AdminVM.TempMaterial.MatDesc = textBox.Text;
                                        break;
                                }
                                AdminVM.CreateMaterial();
                            }
                            else
                            {
                                MessageBox.Show("Material Desc is existed.");
                            }

                        }
                        else
                        {
                            newRow.MatDesc = "";
                            MessageBox.Show("Material Desc is required.");
                        }
                    }
                    else
                    {
                        Material _material = dataGrid.Items[rowIndex] as Material;
                        //AdminVM.SelectedMaterialID = _material.ID;
                        AdminVM.TempMaterial = _material;
                        switch (itemName)
                        {
                            case "MatDesc":
                                AdminVM.TempMaterial.MatDesc = textBox.Text;
                                break;
                        }
                        AdminVM.TempMaterial.ID = _material.ID;
                        AdminVM.ActionState = "UpdateRow";
                        AdminVM.UpdateMaterial();
                    }

                    e.Handled = true;
                }
            }
        }

        private void MaterialTab_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            DataGrid dataGrid = noteHelper.FindDataGrid(chkBox);
            int selectedRowIndex = dataGrid.SelectedIndex;
            int rowIndex = selectedRowIndex;

            if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
            {
                rowIndex = AdminVM.CurrentIndex;
            } else
            {
                rowIndex = selectedRowIndex;
            }
            if (rowIndex >= 0)
            {
                Material newRow = dataGrid.Items[rowIndex] as Material;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        if (!string.IsNullOrEmpty(newRow.MatDesc))
                        {
                            ObservableCollection<Material> materials = AdminVM.Materials;
                            Material item = new Material();
                            materials.Add(item);
                            AdminVM.ActionState = "AddRow";
                            AdminVM.TempMaterial = new Material();

                            if (chkBox.IsChecked == true)
                                AdminVM.TempMaterial.Active = true;
                            else AdminVM.TempMaterial.Active = false;

                            AdminVM.CreateMaterial();
                        }
                        else
                        {
                            newRow.Active = false;
                            MessageBox.Show("Material Name is required.");
                        }
                    }
                    else
                    {
                        Material _material = dataGrid.Items[rowIndex] as Material;
                        AdminVM.SelectedMaterialID = _material.ID;
                        AdminVM.TempMaterial = _material;

                        if (chkBox.IsChecked == true)
                            AdminVM.TempMaterial.Active = true;
                        else AdminVM.TempMaterial.Active = false;
                        AdminVM.ActionState = "UpdateRow";

                        AdminVM.UpdateMaterial();
                    }

                    e.Handled = false;
                }
            }
        }

        private void LaborTab_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                int selectedRowIndex = dataGrid.SelectedIndex;
                int rowIndex = selectedRowIndex;

                string itemName = textBox.Tag as string;
                if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
                {
                    rowIndex = AdminVM.CurrentIndex;
                }
                else
                {
                    rowIndex = selectedRowIndex;
                }

                Labor newRow = dataGrid.Items[rowIndex] as Labor;

                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        if (!string.IsNullOrEmpty(textBox.Text) && itemName.Equals("LaborDesc"))
                        {
                            bool isExisted = false;
                            for (int i = 0; i < dataGrid.Items.Count - 2; i++)
                            {
                                Labor _labor = dataGrid.Items[i] as Labor;
                                if (_labor.LaborDesc.Equals(textBox.Text))
                                {
                                    isExisted = true;
                                }
                            }

                            if (!isExisted)
                            {
                                ObservableCollection<Labor> labors = AdminVM.Labors;
                                Labor item = new Labor();
                                labors.Add(item);
                                AdminVM.ActionState = "AddRow";
                                AdminVM.TempLabor = new Labor();
                                switch (itemName)
                                {
                                    case "LaborDesc":
                                        AdminVM.TempLabor.LaborDesc = textBox.Text;
                                        break;
                                    case "UnitPrice":
                                        AdminVM.TempLabor.UnitPrice = double.Parse(textBox.Text);
                                        break;
                                }
                                AdminVM.CreateLabor();
                            }
                            else
                            {
                                Console.WriteLine("Labor Desc is existed.");
                                MessageBox.Show("Labor Desc is existed.");
                            }

                        }
                        else
                        {
                            newRow.LaborDesc = "";
                            MessageBox.Show("Labor Desc is required.");
                        }
                    }
                    else
                    {
                        Labor _labor = dataGrid.Items[rowIndex] as Labor;
                        AdminVM.TempLabor = _labor;
                        switch (itemName)
                        {
                            case "LaborDesc":
                                AdminVM.TempLabor.LaborDesc = textBox.Text;
                                break;
                            case "UnitPrice":
                                AdminVM.TempLabor.UnitPrice = double.Parse(textBox.Text);
                                break;
                        }
                        AdminVM.TempLabor.ID = _labor.ID;
                        AdminVM.TempLabor.Active = _labor.Active;
                        AdminVM.ActionState = "UpdateRow";
                        AdminVM.UpdateLabor();
                    }

                    e.Handled = true;
                }
            }
        }

        private void LaborTab_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            DataGrid dataGrid = noteHelper.FindDataGrid(chkBox);
            int selectedRowIndex = dataGrid.SelectedIndex;
            int rowIndex = selectedRowIndex;

            if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
            {
                rowIndex = AdminVM.CurrentIndex;
            } else
            {
                rowIndex = selectedRowIndex;
            }
            if (rowIndex >= 0)
            {
                Labor newRow = dataGrid.Items[rowIndex] as Labor;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        if (!string.IsNullOrEmpty(newRow.LaborDesc))
                        {
                            ObservableCollection<Labor> labors = AdminVM.Labors;
                            Labor item = new Labor();
                            labors.Add(item);
                            AdminVM.ActionState = "AddRow";
                            AdminVM.TempLabor = new Labor();

                            if (chkBox.IsChecked == true)
                                AdminVM.TempLabor.Active = true;
                            else AdminVM.TempLabor.Active = false;

                            AdminVM.CreateLabor();
                        }
                        else
                        {
                            newRow.Active = false;
                            MessageBox.Show("Labor Name is required.");
                        }
                    }
                    else
                    {
                        Labor _labor = dataGrid.Items[rowIndex] as Labor;
                        AdminVM.SelectedLaborID = _labor.ID;
                        AdminVM.TempLabor = _labor;

                        if (chkBox.IsChecked == true)
                            AdminVM.TempLabor.Active = true;
                        else AdminVM.TempLabor.Active = false;
                        AdminVM.ActionState = "UpdateRow";

                        AdminVM.UpdateLabor();
                    }

                    e.Handled = false;
                }
            }
        }

        private void SalesTab_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                int selectedRowIndex = dataGrid.SelectedIndex;
                int rowIndex = selectedRowIndex;

                string itemName = textBox.Tag as string;
                if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
                {
                    rowIndex = AdminVM.CurrentIndex;
                }
                else
                {
                    rowIndex = selectedRowIndex;
                }

                Salesman newRow = dataGrid.Items[rowIndex] as Salesman;

                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        if (!string.IsNullOrEmpty(textBox.Text) && itemName.Equals("Name"))
                        {
                            ObservableCollection<Salesman> salesMans = AdminVM.Salesmans;
                            Salesman item = new Salesman();
                            salesMans.Add(item);
                            AdminVM.ActionState = "AddRow";
                            AdminVM.TempSalesman = new Salesman();
                            switch (itemName)
                            {
                                case "Salesman_Init":
                                    AdminVM.TempSalesman.Init = textBox.Text;
                                    break;
                                case "Name":
                                    AdminVM.TempSalesman.SalesmanName = textBox.Text;
                                    break;
                                case "Phone":
                                    AdminVM.TempSalesman.Phone= textBox.Text;
                                    break;
                                case "Cell":
                                    AdminVM.TempSalesman.Cell = textBox.Text;
                                    break;
                                case "Email":
                                    AdminVM.TempSalesman.Email = textBox.Text;
                                    break;
                            }
                            AdminVM.CreateSalesman();
                        }
                        else
                        {
                            newRow.SalesmanName = "";
                            MessageBox.Show("Salesman Name is required.");
                        }
                    }
                    else
                    {
                        Salesman _salesman = dataGrid.Items[rowIndex] as Salesman;
                        AdminVM.TempSalesman = _salesman;

                        switch (itemName)
                        {
                            case "Init":
                                AdminVM.TempSalesman.Init = textBox.Text;
                                break;
                            case "Name":
                                AdminVM.TempSalesman.SalesmanName = textBox.Text;
                                break;
                            case "Phone":
                                AdminVM.TempSalesman.Phone = textBox.Text;
                                break;
                            case "Cell":
                                AdminVM.TempSalesman.Cell = textBox.Text;
                                break;
                            case "Email":
                                AdminVM.TempSalesman.Email = textBox.Text;
                                break;
                        }
                        AdminVM.TempSalesman.ID = _salesman.ID;
                        AdminVM.TempSalesman.Active = _salesman.Active;
                        AdminVM.ActionState = "UpdateRow";
                        AdminVM.UpdateSalesman();
                    }

                    e.Handled = true;
                }
            }
        }

        private void SalesTab_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            DataGrid dataGrid = noteHelper.FindDataGrid(chkBox);
            int selectedRowIndex = dataGrid.SelectedIndex;
            int rowIndex = selectedRowIndex;

            if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
            {
                rowIndex = AdminVM.CurrentIndex;
            }
            else
            {
                rowIndex = selectedRowIndex;
            }
            if (rowIndex >= 0)
            {
                Salesman newRow = dataGrid.Items[rowIndex] as Salesman;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        if (!string.IsNullOrEmpty(newRow.SalesmanName))
                        {
                            ObservableCollection<Salesman> salesmans = AdminVM.Salesmans;
                            Salesman item = new Salesman();
                            salesmans.Add(item);
                            AdminVM.ActionState = "AddRow";
                            AdminVM.TempSalesman = new Salesman();

                            if (chkBox.IsChecked == true)
                                AdminVM.TempSalesman.Active = true;
                            else AdminVM.TempSalesman.Active = false;

                            AdminVM.CreateSalesman();
                        }
                        else
                        {
                            newRow.Active = false;
                            MessageBox.Show("Salesman name is required.");
                        }
                    }
                    else
                    {
                        Salesman _salesman = dataGrid.Items[rowIndex] as Salesman;
                        AdminVM.SelectedSalesmanID = _salesman.ID;
                        AdminVM.TempSalesman = _salesman;

                        if (chkBox.IsChecked == true)
                            AdminVM.TempSalesman.Active = true;
                        else AdminVM.TempSalesman.Active = false;
                        AdminVM.ActionState = "UpdateRow";

                        AdminVM.UpdateSalesman();
                    }

                    e.Handled = false;
                }
            }
        }

        private void CrewTab_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                int selectedRowIndex = dataGrid.SelectedIndex;
                int rowIndex = selectedRowIndex;

                string itemName = textBox.Tag as string;
                if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
                {
                    rowIndex = AdminVM.CurrentIndex;
                }
                else
                {
                    rowIndex = selectedRowIndex;
                }

                Crew newRow = dataGrid.Items[rowIndex] as Crew;

                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                            ObservableCollection<Crew> crews = AdminVM.Crews;
                            Crew item = new Crew();
                            crews.Add(item);
                            AdminVM.ActionState = "AddRow";
                            AdminVM.TempCrew = new Crew();
                            switch (itemName)
                            {
                                case "Name":
                                    AdminVM.TempCrew.CrewName = textBox.Text;
                                    break;
                                case "Phone":
                                    AdminVM.TempCrew.Phone = textBox.Text;
                                    break;
                                case "Cell":
                                    AdminVM.TempCrew.Cell = textBox.Text;
                                    break;
                                case "Email":
                                    AdminVM.TempCrew.Email = textBox.Text;
                                    break;
                            }
                            AdminVM.CreateCrew();
                    }
                    else
                    {
                        Crew _crew = dataGrid.Items[rowIndex] as Crew;
                        AdminVM.TempCrew = _crew;
                        switch (itemName)
                        {
                            case "Name":
                                AdminVM.TempCrew.CrewName = textBox.Text;
                                break;
                            case "Phone":
                                AdminVM.TempCrew.Phone = textBox.Text;
                                break;
                            case "Cell":
                                AdminVM.TempCrew.Cell = textBox.Text;
                                break;
                            case "Email":
                                AdminVM.TempCrew.Email = textBox.Text;
                                break;
                        }
                        AdminVM.TempCrew.ID = _crew.ID;
                        AdminVM.TempCrew.Active = _crew.Active;
                        AdminVM.ActionState = "UpdateRow";
                        AdminVM.UpdateCrew();
                    }

                    e.Handled = true;
                }
            }
        }

        private void CrewTab_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            DataGrid dataGrid = noteHelper.FindDataGrid(chkBox);
            int selectedRowIndex = dataGrid.SelectedIndex;
            int rowIndex = selectedRowIndex;

            if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
            {
                rowIndex = AdminVM.CurrentIndex;
            }
            else
            {
                rowIndex = selectedRowIndex;
            }
            if (rowIndex >= 0)
            {
                Crew newRow = dataGrid.Items[rowIndex] as Crew;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        //if (!string.IsNullOrEmpty(newRow.CrewName))
                        //{
                            ObservableCollection<Crew> crews = AdminVM.Crews;
                            Crew item = new Crew();
                            crews.Add(item);
                            AdminVM.ActionState = "AddRow";
                            AdminVM.TempCrew = new Crew();

                            if (chkBox.IsChecked == true)
                                AdminVM.TempCrew.Active = true;
                            else AdminVM.TempCrew.Active = false;

                            AdminVM.CreateCrew();
                        //}
                        //else
                        //{
                        //    newRow.Active = false;
                        //    MessageBox.Show("Crew name is required.");
                        //}
                    }
                    else
                    {
                        Crew _crew = dataGrid.Items[rowIndex] as Crew;
                        AdminVM.SelectedCrewID = _crew.ID;
                        AdminVM.TempCrew = _crew;

                        if (chkBox.IsChecked == true)
                            AdminVM.TempCrew.Active = true;
                        else AdminVM.TempCrew.Active = false;
                        AdminVM.ActionState = "UpdateRow";

                        AdminVM.UpdateCrew();
                    }

                    e.Handled = true;
                }
            }
        }

        private void ArchTab_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                int selectedRowIndex = dataGrid.SelectedIndex;
                int rowIndex = selectedRowIndex;

                string itemName = textBox.Tag as string;
                if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
                {
                    rowIndex = AdminVM.CurrentIndex;
                }
                else
                {
                    rowIndex = selectedRowIndex;
                }

                Architect newRow = dataGrid.Items[rowIndex] as Architect;

                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<Architect> architects = AdminVM.Architects;
                        Architect item = new Architect();
                        architects.Add(item);
                        AdminVM.ActionState = "AddRow";
                        AdminVM.TempArch = new Architect();
                        switch (itemName)
                        {
                            case "Company":
                                AdminVM.TempArch.ArchCompany = textBox.Text;
                                break;
                            case "Contact":
                                AdminVM.TempArch.Contact = textBox.Text;
                                break;
                            case "Address":
                                AdminVM.TempArch.Address = textBox.Text;
                                break;
                            case "City":
                                AdminVM.TempArch.City = textBox.Text;
                                break;
                            case "State":
                                AdminVM.TempArch.State = textBox.Text;
                                break;
                            case "Zip":
                                AdminVM.TempArch.Zip = textBox.Text;
                                break;
                            case "Phone":
                                AdminVM.TempArch.Phone = textBox.Text;
                                break;
                            case "Fax":
                                AdminVM.TempArch.Fax = textBox.Text;
                                break;
                            case "Cell":
                                AdminVM.TempArch.Cell = textBox.Text;
                                break;
                        }
                        AdminVM.CreateArch();
                    }
                    else
                    {
                        Architect _arch = dataGrid.Items[rowIndex] as Architect;
                        AdminVM.TempArch = _arch;
                        switch (itemName)
                        {
                            case "Company":
                                AdminVM.TempArch.ArchCompany = textBox.Text;
                                break;
                            case "Contact":
                                AdminVM.TempArch.Contact = textBox.Text;
                                break;
                            case "Address":
                                AdminVM.TempArch.Address = textBox.Text;
                                break;
                            case "City":
                                AdminVM.TempArch.City = textBox.Text;
                                break;
                            case "State":
                                AdminVM.TempArch.State = textBox.Text;
                                break;
                            case "Zip":
                                AdminVM.TempArch.Zip = textBox.Text;
                                break;
                            case "Phone":
                                AdminVM.TempArch.Phone = textBox.Text;
                                break;
                            case "Fax":
                                AdminVM.TempArch.Fax = textBox.Text;
                                break;
                            case "Cell":
                                AdminVM.TempArch.Cell = textBox.Text;
                                break;
                        }
                        AdminVM.TempArch.ID = _arch.ID;
                        AdminVM.TempArch.Active = _arch.Active;
                        AdminVM.ActionState = "UpdateRow";
                        AdminVM.UpdateArch();
                    }

                    e.Handled = true;
                }
            }
        }

        private void ArchTab_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            DataGrid dataGrid = noteHelper.FindDataGrid(chkBox);
            int selectedRowIndex = dataGrid.SelectedIndex;
            int rowIndex = selectedRowIndex;

            if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
            {
                rowIndex = AdminVM.CurrentIndex;
            }
            else
            {
                rowIndex = selectedRowIndex;
            }
            if (rowIndex >= 0)
            {
                Architect newRow = dataGrid.Items[rowIndex] as Architect;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<Architect> architects = AdminVM.Architects;
                        Architect item = new Architect();
                        architects.Add(item);
                        AdminVM.ActionState = "AddRow";
                        AdminVM.TempArch = new Architect();

                        if (chkBox.IsChecked == true)
                            AdminVM.TempArch.Active = true;
                        else AdminVM.TempArch.Active = false;

                        AdminVM.CreateCrew();
                    }
                    else
                    {
                        Architect _architect = dataGrid.Items[rowIndex] as Architect;
                        AdminVM.SelectedArchID = _architect.ID;
                        AdminVM.TempArch = _architect;

                        if (chkBox.IsChecked == true)
                            AdminVM.TempArch.Active = true;
                        else AdminVM.TempArch.Active = false;
                        AdminVM.ActionState = "UpdateRow";

                        AdminVM.UpdateArch();
                    }

                    e.Handled = true;
                }
            }
        }

        private void FreightTab_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                int selectedRowIndex = dataGrid.SelectedIndex;
                int rowIndex = selectedRowIndex;

                string itemName = textBox.Tag as string;
                if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
                {
                    rowIndex = AdminVM.CurrentIndex;
                }
                else
                {
                    rowIndex = selectedRowIndex;
                }

                FreightCo newRow = dataGrid.Items[rowIndex] as FreightCo;

                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<FreightCo> freightCOs = AdminVM.FreightCos;
                        FreightCo item = new FreightCo();
                        freightCOs.Add(item);
                        AdminVM.ActionState = "AddRow";
                        AdminVM.TempFreightCo = new FreightCo();
                        switch (itemName)
                        {
                            case "Name":
                                AdminVM.TempFreightCo.FreightName = textBox.Text;
                                break;
                            case "ContactName":
                                AdminVM.TempFreightCo.Contact = textBox.Text;
                                break;
                            case "Phone":
                                AdminVM.TempFreightCo.Phone = textBox.Text;
                                break;
                            case "Cell":
                                AdminVM.TempFreightCo.Cell = textBox.Text;
                                break;
                            case "Email":
                                AdminVM.TempFreightCo.Email = textBox.Text;
                                break;
                        }
                        AdminVM.CreateFreightCo();
                    }
                    else
                    {
                        FreightCo _freightCo = dataGrid.Items[rowIndex] as FreightCo;
                        AdminVM.TempFreightCo = _freightCo;
                        switch (itemName)
                        {
                            case "Name":
                                AdminVM.TempFreightCo.FreightName = textBox.Text;
                                break;
                            case "ContactName":
                                AdminVM.TempFreightCo.Contact = textBox.Text;
                                break;
                            case "Phone":
                                AdminVM.TempFreightCo.Phone = textBox.Text;
                                break;
                            case "Cell":
                                AdminVM.TempFreightCo.Cell = textBox.Text;
                                break;
                            case "Email":
                                AdminVM.TempFreightCo.Email = textBox.Text;
                                break;
                        }
                        AdminVM.TempFreightCo.ID = _freightCo.ID;
                        AdminVM.TempFreightCo.Active = _freightCo.Active;
                        AdminVM.ActionState = "UpdateRow";
                        AdminVM.UpdateFreightCo();
                    }

                    e.Handled = true;
                }
            }
        }

        private void FreightTab_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            DataGrid dataGrid = noteHelper.FindDataGrid(chkBox);
            int selectedRowIndex = dataGrid.SelectedIndex;
            int rowIndex = selectedRowIndex;

            if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
            {
                rowIndex = AdminVM.CurrentIndex;
            }
            else
            {
                rowIndex = selectedRowIndex;
            }
            if (rowIndex >= 0)
            {
                FreightCo newRow = dataGrid.Items[rowIndex] as FreightCo;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<FreightCo> freightCos = AdminVM.FreightCos;
                        FreightCo item = new FreightCo();
                        freightCos.Add(item);
                        AdminVM.ActionState = "AddRow";
                        AdminVM.TempFreightCo = new FreightCo();

                        if (chkBox.IsChecked == true)
                            AdminVM.TempFreightCo.Active = true;
                        else AdminVM.TempFreightCo.Active = false;

                        AdminVM.CreateFreightCo();
                    }
                    else
                    {
                        FreightCo _freightCo = dataGrid.Items[rowIndex] as FreightCo;
                        AdminVM.SelectedFreightCoID = _freightCo.ID;
                        AdminVM.TempFreightCo = _freightCo;

                        if (chkBox.IsChecked == true)
                            AdminVM.TempFreightCo.Active = true;
                        else AdminVM.TempFreightCo.Active = false;
                        AdminVM.ActionState = "UpdateRow";

                        AdminVM.UpdateFreightCo();
                    }
                    e.Handled = true;
                }
            }
        }

        private void UserTab_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                int selectedRowIndex = dataGrid.SelectedIndex;
                int rowIndex = selectedRowIndex;

                string itemName = textBox.Tag as string;
                if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
                {
                    rowIndex = AdminVM.CurrentIndex;
                }
                else
                {
                    rowIndex = selectedRowIndex;
                }

                User newRow = dataGrid.Items[rowIndex] as User;

                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<User> users = AdminVM.Users;
                        User item = new User();
                        users.Add(item);
                        AdminVM.ActionState = "AddRow";
                        AdminVM.TempUser = new User();
                        switch (itemName)
                        {
                            case "UserName":
                                AdminVM.TempUser.UserName = textBox.Text;
                                break;
                            case "PersonName":
                                AdminVM.TempUser.PersonName = textBox.Text;
                                break;
                            case "Level":
                                AdminVM.TempUser.Level = int.Parse(textBox.Text);
                                break;
                            case "Email":
                                AdminVM.TempUser.Email = textBox.Text;
                                break;
                        }
                        AdminVM.CreateUser();
                    }
                    else
                    {
                        User _user = dataGrid.Items[rowIndex] as User;
                        AdminVM.TempUser = _user;
                        switch (itemName)
                        {
                            case "UserName":
                                AdminVM.TempUser.UserName = textBox.Text;
                                break;
                            case "PersonName":
                                AdminVM.TempUser.PersonName = textBox.Text;
                                break;
                            case "Level":
                                AdminVM.TempUser.Level = int.Parse(textBox.Text);
                                break;
                            case "Email":
                                AdminVM.TempUser.Email = textBox.Text;
                                break;
                        }
                        AdminVM.TempUser.ID = _user.ID;
                        AdminVM.TempUser.Active = _user.Active;
                        AdminVM.ActionState = "UpdateRow";
                        AdminVM.UpdateUser();
                    }

                    e.Handled = true;
                }
            }
        }

        private void UserTab_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            DataGrid dataGrid = noteHelper.FindDataGrid(chkBox);
            int selectedRowIndex = dataGrid.SelectedIndex;
            int rowIndex = selectedRowIndex;

            if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
            {
                rowIndex = AdminVM.CurrentIndex;
            }
            else
            {
                rowIndex = selectedRowIndex;
            }
            if (rowIndex >= 0)
            {
                User newRow = dataGrid.Items[rowIndex] as User;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<User> users = AdminVM.Users;
                        User item = new User();
                        users.Add(item);
                        AdminVM.ActionState = "AddRow";
                        AdminVM.TempUser = new User();

                        if (chkBox.IsChecked == true)
                            AdminVM.TempUser.Active = true;
                        else AdminVM.TempUser.Active = false;

                        AdminVM.CreateUser();
                    }
                    else
                    {
                        User _user = dataGrid.Items[rowIndex] as User;
                        AdminVM.SelectedUserID = _user.ID;
                        AdminVM.TempUser = _user;

                        if (chkBox.IsChecked == true)
                            AdminVM.TempUser.Active = true;
                        else AdminVM.TempUser.Active = false;
                        AdminVM.ActionState = "UpdateRow";

                        AdminVM.UpdateUser();
                    }
                    e.Handled = true;
                }
            }
        }

        private void UserTab_ComboBox(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if(comboBox.SelectedValue != null)
            {
                int optID = int.Parse(comboBox.SelectedValue.ToString());
                DataGrid dataGrid = noteHelper.FindDataGrid(comboBox);
                int selectedRowIndex = dataGrid.SelectedIndex;
                int rowIndex = selectedRowIndex;

                if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
                {
                    rowIndex = AdminVM.CurrentIndex;
                }
                else
                {
                    rowIndex = selectedRowIndex;
                }
                if (rowIndex >= 0)
                {
                    User newRow = dataGrid.Items[rowIndex] as User;
                    if (dataGrid != null)
                    {
                        if (selectedRowIndex == dataGrid.Items.Count - 1)
                        {
                            ObservableCollection<User> users = AdminVM.Users;
                            User item = new User();
                            users.Add(item);
                            AdminVM.ActionState = "AddRow";
                            AdminVM.TempUser = new User();
                            AdminVM.TempUser.FormOnOpen = optID;

                            AdminVM.CreateUser();
                        }
                        else
                        {
                            User _user = dataGrid.Items[rowIndex] as User;
                            AdminVM.SelectedUserID = _user.ID;
                            AdminVM.TempUser = _user;
                            AdminVM.TempUser.FormOnOpen = optID;

                            AdminVM.ActionState = "UpdateRow";

                            AdminVM.UpdateUser();
                        }
                        e.Handled = true;
                    }
                }
            }
        }

        private void Installer_FileSelector(object sender, RoutedEventArgs e)
        {
            Button oshaCertButton = sender as Button;
            string itemName = oshaCertButton.Tag as string;

            DataGrid dataGrid = noteHelper.FindDataGrid(oshaCertButton);
            int selectedRowIndex = dataGrid.SelectedIndex;
            InHouseInstaller _installer = dataGrid.Items[selectedRowIndex] as InHouseInstaller;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                switch (itemName)
                {
                    case "OshaCert":
                        _installer.OSHACert = selectedFileName;
                        break;
                    case "FacCert":
                        _installer.FirstAidCert = selectedFileName;
                        break;
                    default:
                        break;
                }
                AdminVM.TempInstaller = _installer;
                AdminVM.ActionState = "UpdateRow";
                AdminVM.UpdateInstaller();
            }
        }

        private void Install_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(textBox);
                int selectedRowIndex = dataGrid.SelectedIndex;
                int rowIndex = selectedRowIndex;

                string itemName = textBox.Tag as string;
                if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
                {
                    rowIndex = AdminVM.CurrentIndex;
                }
                else
                {
                    rowIndex = selectedRowIndex;
                }

                User newRow = dataGrid.Items[rowIndex] as User;

                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<InHouseInstaller> installers = AdminVM.Installers;
                        InHouseInstaller item = new InHouseInstaller();
                        installers.Add(item);
                        AdminVM.ActionState = "AddRow";
                        AdminVM.TempInstaller = new InHouseInstaller();
                        switch (itemName)
                        {
                            case "Name":
                                AdminVM.TempInstaller.InstallerName= textBox.Text;
                                break;
                            case "Cell":
                                AdminVM.TempInstaller.InstallerCell = textBox.Text;
                                break;
                            case "Email":
                                AdminVM.TempInstaller.InstallerEmail = textBox.Text;
                                break;
                            case "CertificationDate":
                                AdminVM.TempInstaller.OSHAExpireDate = DateTime.Parse(textBox.Text);
                                break;
                            case "FacDate":
                                AdminVM.TempInstaller.FirstAidExpireDate = DateTime.Parse(textBox.Text);
                                break;
                        }
                        AdminVM.CreateInstaller();
                    }
                    else
                    {
                        InHouseInstaller _installer = dataGrid.Items[rowIndex] as InHouseInstaller;
                        AdminVM.TempInstaller = _installer;
                        switch (itemName)
                        {
                            case "Name":
                                AdminVM.TempInstaller.InstallerName = textBox.Text;
                                break;
                            case "Cell":
                                AdminVM.TempInstaller.InstallerCell = textBox.Text;
                                break;
                            case "Email":
                                AdminVM.TempInstaller.InstallerEmail = textBox.Text;
                                break;
                            case "CertificationDate":
                                AdminVM.TempInstaller.OSHAExpireDate = DateTime.Parse(textBox.Text);
                                break;
                            case "OshaCert":
                                AdminVM.TempInstaller.OSHACert = textBox.Text;
                                break;
                            case "FacDate":
                                AdminVM.TempInstaller.FirstAidExpireDate = DateTime.Parse(textBox.Text);
                                break;
                            case "FacCert":
                                AdminVM.TempInstaller.FirstAidCert = textBox.Text;
                                break;
                        }
                        AdminVM.TempInstaller.ID = _installer.ID;
                        AdminVM.TempInstaller.CrewID = _installer.CrewID;
                        AdminVM.TempInstaller.OSHALevel = _installer.OSHALevel;
                        AdminVM.TempInstaller.Active = _installer.Active;
                        AdminVM.ActionState = "UpdateRow";
                        AdminVM.UpdateInstaller();
                    }

                    e.Handled = true;
                }
            }
        }

        private void InstallCrew_ComboBox(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedValue != null)
            {
                int crewID = int.Parse(comboBox.SelectedValue.ToString());
                DataGrid dataGrid = noteHelper.FindDataGrid(comboBox);
                int selectedRowIndex = dataGrid.SelectedIndex;
                int rowIndex = selectedRowIndex;

                if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
                {
                    rowIndex = AdminVM.CurrentIndex;
                }
                else
                {
                    rowIndex = selectedRowIndex;
                }
                if (rowIndex >= 0)
                {
                    InHouseInstaller newRow = dataGrid.Items[rowIndex] as InHouseInstaller;
                    if (dataGrid != null)
                    {
                        if (selectedRowIndex == dataGrid.Items.Count - 1)
                        {
                            ObservableCollection<InHouseInstaller> installers = AdminVM.Installers;
                            InHouseInstaller item = new InHouseInstaller();
                            installers.Add(item);
                            AdminVM.ActionState = "AddRow";
                            AdminVM.TempInstaller = new InHouseInstaller();
                            AdminVM.TempInstaller.CrewID = crewID;

                            AdminVM.CreateInstaller();
                        }
                        else
                        {
                            InHouseInstaller _installer = dataGrid.Items[rowIndex] as InHouseInstaller;
                            AdminVM.SelectedInstallerID = _installer.ID;
                            AdminVM.TempInstaller = _installer;
                            AdminVM.TempInstaller.CrewID = crewID;

                            AdminVM.ActionState = "UpdateRow";

                            AdminVM.UpdateInstaller();
                        }
                        e.Handled = true;
                    }
                }
            }
        }

        private void InstallOsha_ComboBox(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedValue != null)
            {
                string oshaLevel = comboBox.SelectedValue.ToString();
                DataGrid dataGrid = noteHelper.FindDataGrid(comboBox);
                int selectedRowIndex = dataGrid.SelectedIndex;
                int rowIndex = selectedRowIndex;

                if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
                {
                    rowIndex = AdminVM.CurrentIndex;
                }
                else
                {
                    rowIndex = selectedRowIndex;
                }
                if (rowIndex >= 0)
                {
                    InHouseInstaller newRow = dataGrid.Items[rowIndex] as InHouseInstaller;
                    if (dataGrid != null)
                    {
                        if (selectedRowIndex == dataGrid.Items.Count - 1)
                        {
                            ObservableCollection<InHouseInstaller> installers = AdminVM.Installers;
                            InHouseInstaller item = new InHouseInstaller();
                            installers.Add(item);
                            AdminVM.ActionState = "AddRow";
                            AdminVM.TempInstaller = new InHouseInstaller();
                            AdminVM.TempInstaller.OSHALevel = oshaLevel;

                            AdminVM.CreateInstaller();
                        }
                        else
                        {
                            InHouseInstaller _installer = dataGrid.Items[rowIndex] as InHouseInstaller;
                            AdminVM.SelectedInstallerID = _installer.ID;
                            AdminVM.TempInstaller = _installer;
                            AdminVM.TempInstaller.OSHALevel = oshaLevel;

                            AdminVM.ActionState = "UpdateRow";

                            AdminVM.UpdateInstaller();
                        }
                        e.Handled = true;
                    }
                }
            }
        }

        private void Install_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            DataGrid dataGrid = noteHelper.FindDataGrid(chkBox);
            int selectedRowIndex = dataGrid.SelectedIndex;
            int rowIndex = selectedRowIndex;

            if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
            {
                rowIndex = AdminVM.CurrentIndex;
            }
            else
            {
                rowIndex = selectedRowIndex;
            }
            if (rowIndex >= 0)
            {
                InHouseInstaller newRow = dataGrid.Items[rowIndex] as InHouseInstaller;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<InHouseInstaller> installers = AdminVM.Installers;
                        InHouseInstaller item = new InHouseInstaller();
                        installers.Add(item);
                        AdminVM.ActionState = "AddRow";
                        AdminVM.TempInstaller = new InHouseInstaller();

                        if (chkBox.IsChecked == true)
                            AdminVM.TempInstaller.Active = true;
                        else AdminVM.TempInstaller.Active = false;

                        AdminVM.CreateInstaller();
                    }
                    else
                    {
                        InHouseInstaller _installer = dataGrid.Items[rowIndex] as InHouseInstaller;
                        AdminVM.SelectedInstallerID = _installer.ID;
                        AdminVM.TempInstaller = _installer;

                        if (chkBox.IsChecked == true)
                            AdminVM.TempInstaller.Active = true;
                        else AdminVM.TempInstaller.Active = false;
                        AdminVM.ActionState = "UpdateRow";

                        AdminVM.UpdateInstaller();
                    }
                    e.Handled = true;
                }
            }
        }

        private void AdminDataGrid_TabChanged(object sender, SelectionChangedEventArgs e)
        {
            //AdminVM.CurrentIndex = 0;
        }

        private void ManufTab_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(textBox);

                if (dataGrid != null)
                {
                    string itemName = textBox.Tag as string;
                    AdminVM.UpdateComponent = "Table";

                    int selectedRowIndex = dataGrid.SelectedIndex;

                    Manufacturer item = dataGrid.Items[selectedRowIndex] as Manufacturer;
                    AdminVM.TempManuf = item;

                    switch (itemName)
                    {
                        case "ManufacturerName":
                            AdminVM.TempManuf.ManufacturerName = textBox.Text;
                            break;
                        case "Address":
                            AdminVM.TempManuf.Address = textBox.Text;
                            break;
                        case "City":
                            AdminVM.TempManuf.City = textBox.Text;
                            break;
                        case "State":
                            AdminVM.TempManuf.State = textBox.Text;
                            break;
                        case "Zip":
                            AdminVM.TempManuf.Zip = textBox.Text;
                            break;
                        case "Phone":
                            AdminVM.TempManuf.Phone = textBox.Text;
                            break;
                    }

                    AdminVM.UpdateManuf();
                }
            }
            e.Handled = true;
        }

        private void ManufTab_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            DataGrid dataGrid = noteHelper.FindDataGrid(chkBox);
            int selectedRowIndex = dataGrid.SelectedIndex;
            int rowIndex = selectedRowIndex;

            if (AdminVM.ActionState.Equals("AddRow") || (AdminVM.ActionState.Equals("UpdateRow") && selectedRowIndex == -1))
            {
                rowIndex = AdminVM.CurrentIndex;
            }
            else
            {
                rowIndex = selectedRowIndex;
            }
            if (rowIndex >= 0)
            {
                Manufacturer newRow = dataGrid.Items[rowIndex] as Manufacturer;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<Manufacturer> manufs = AdminVM.Manufacturers;
                        Manufacturer item = new Manufacturer();
                        manufs.Add(item);
                        AdminVM.ActionState = "AddRow";
                        AdminVM.TempManuf = new Manufacturer();

                        if (chkBox.IsChecked == true)
                            AdminVM.TempManuf.Active = true;
                        else AdminVM.TempManuf.Active = false;

                        AdminVM.CreateManuf();
                    }
                    else
                    {
                        Manufacturer _manuf = dataGrid.Items[rowIndex] as Manufacturer;
                        AdminVM.SelectedManufID = _manuf.ID;
                        AdminVM.TempManuf = _manuf;

                        if (chkBox.IsChecked == true)
                            AdminVM.TempManuf.Active = true;
                        else AdminVM.TempManuf.Active = false;

                        AdminVM.UpdateComponent = "Table";
                        AdminVM.UpdateManuf();
                    }
                    e.Handled = true;
                }
            }
        }

        private void ManufDetail_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            string itemName = textBox.Tag as string;
            if (AdminVM.SelectedManufID == -1)
            {
                // Create Manuf
                AdminVM.TempCreateManuf = new Manufacturer();
                //Manufacturer _manuf = new Manufacturer();
                switch (itemName)
                {
                    case "Name":
                        AdminVM.TempCreateManuf.ManufacturerName = textBox.Text;
                        break;
                    case "Address":
                        AdminVM.TempCreateManuf.Address = textBox.Text;
                        break;
                    case "Address2":
                        AdminVM.TempCreateManuf.Address2 = textBox.Text;
                        break;
                    case "City":
                        AdminVM.TempCreateManuf.City = textBox.Text;
                        break;
                    case "State":
                        AdminVM.TempCreateManuf.State = textBox.Text;
                        break;
                    case "Zip":
                        AdminVM.TempCreateManuf.Zip = textBox.Text;
                        break;
                    case "Phone":
                        AdminVM.TempCreateManuf.Phone = textBox.Text;
                        break;
                    case "Fax":
                        AdminVM.TempCreateManuf.Fax = textBox.Text;
                        break;
                    case "ContactName":
                        AdminVM.TempCreateManuf.ContactName = textBox.Text;
                        break;
                    case "ContactPhone":
                        AdminVM.TempCreateManuf.ContactPhone = textBox.Text;
                        break;
                    case "ContactEmail":
                        AdminVM.TempCreateManuf.ContactEmail = textBox.Text;
                        break;
                }
                AdminVM.CreateManuf();
            }
            else
            {
                // Update Manuf
                switch (itemName)
                {
                    case "Name":
                        AdminVM.TempDetailManuf.ManufacturerName = textBox.Text;
                        break;
                    case "Address":
                        AdminVM.TempDetailManuf.Address = textBox.Text;
                        break;
                    case "Address2":
                        AdminVM.TempDetailManuf.Address2 = textBox.Text;
                        break;
                    case "City":
                        AdminVM.TempDetailManuf.City = textBox.Text;
                        break;
                    case "State":
                        AdminVM.TempDetailManuf.State = textBox.Text;
                        break;
                    case "Zip":
                        AdminVM.TempDetailManuf.Zip = textBox.Text;
                        break;
                    case "Phone":
                        AdminVM.TempDetailManuf.Phone = textBox.Text;
                        break;
                    case "Fax":
                        AdminVM.TempDetailManuf.Fax = textBox.Text;
                        break;
                    case "ContactName":
                        AdminVM.TempDetailManuf.ContactName = textBox.Text;
                        break;
                    case "ContactPhone":
                        AdminVM.TempDetailManuf.ContactPhone = textBox.Text;
                        break;
                    case "ContactEmail":
                        AdminVM.TempDetailManuf.ContactEmail = textBox.Text;
                        break;
                }
                AdminVM.UpdateManuf();
            }
        }

        private void ManufDetail_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            if (chkBox.IsChecked == true)
                AdminVM.TempDetailManuf.Active = true;
            else AdminVM.TempDetailManuf.Active = false;

            AdminVM.UpdateComponent = "Detail";
            AdminVM.UpdateManuf();
        }

        private void CustomerDetail_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            string itemName = textBox.Tag as string;
            if (AdminVM.SelectedCustomerID == -1)
            {
                AdminVM.TempCreateCustomer = new Customer();
                switch (itemName)
                {
                    case "FullName":
                        AdminVM.TempCreateCustomer.FullName = textBox.Text;
                        break;
                    case "ShortName":
                        AdminVM.TempCreateCustomer.ShortName = textBox.Text;
                        break;
                    case "PoBox":
                        AdminVM.TempCreateCustomer.PoBox = textBox.Text;
                        break;
                    case "Address":
                        AdminVM.TempCreateCustomer.Address = textBox.Text;
                        break;
                    case "City":
                        AdminVM.TempCreateCustomer.City = textBox.Text;
                        break;
                    case "State":
                        AdminVM.TempCreateCustomer.State = textBox.Text;
                        break;
                    case "Zip":
                        AdminVM.TempCreateCustomer.Zip = textBox.Text;
                        break;
                    case "Phone":
                        AdminVM.TempCreateCustomer.Phone = textBox.Text;
                        break;
                    case "Fax":
                        AdminVM.TempCreateCustomer.Fax = textBox.Text;
                        break;
                    case "Email":
                        AdminVM.TempCreateCustomer.Email = textBox.Text;
                        break;
                }
                AdminVM.CreateCustomer();
            }
            else
            {
                switch (itemName)
                {
                    case "FullName":
                        AdminVM.TempDetailCustomer.FullName = textBox.Text;
                        break;
                    case "ShortName":
                        AdminVM.TempDetailCustomer.ShortName = textBox.Text;
                        break;
                    case "PoBox":
                        AdminVM.TempDetailCustomer.PoBox = textBox.Text;
                        break;
                    case "Address":
                        AdminVM.TempDetailCustomer.Address = textBox.Text;
                        break;
                    case "City":
                        AdminVM.TempDetailCustomer.City = textBox.Text;
                        break;
                    case "State":
                        AdminVM.TempDetailCustomer.State = textBox.Text;
                        break;
                    case "Zip":
                        AdminVM.TempDetailCustomer.Zip = textBox.Text;
                        break;
                    case "Phone":
                        AdminVM.TempDetailCustomer.Phone = textBox.Text;
                        break;
                    case "Fax":
                        AdminVM.TempDetailCustomer.Fax = textBox.Text;
                        break;
                    case "Email":
                        AdminVM.TempDetailCustomer.Email = textBox.Text;
                        break;
                }
                AdminVM.UpdateCustomer();
            }
        }

        private void CustomerDetail_CheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;

            if (chkBox.IsChecked == true)
                AdminVM.TempDetailCustomer.Active = true;
            else AdminVM.TempDetailCustomer.Active = false;

            AdminVM.UpdateComponent = "Detail";
            AdminVM.UpdateCustomer();
        }
    }
}
