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
            TextBox input = sender as TextBox;
            if (input != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(input);
                int selectedRowIndex = AdminVM.SelectedSuptRowIndex;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<Superintendent> custSupts = AdminVM.CustSupts;
                        Superintendent item = new Superintendent();
                        custSupts.Add(item);
                        e.Handled = true;
                    }
                }
            }
        }

        private void CustSupt_ChkPreviewKeyUp(object sender, RoutedEventArgs e)
        {
            CheckBox input = sender as CheckBox;
            DataGrid dataGrid = noteHelper.FindDataGrid(input);
            int selectedRowIndex = AdminVM.SelectedSuptRowIndex;
            if (dataGrid != null)
            {
                if (selectedRowIndex == dataGrid.Items.Count - 1)
                {
                    ObservableCollection<Superintendent> custSupts = AdminVM.CustSupts;
                    Superintendent item = new Superintendent();
                    custSupts.Add(item);
                    e.Handled = true;
                }
            }
        }

        private void CustContact_PreviewKeyUp(object sender, RoutedEventArgs e)
        {
            TextBox input = sender as TextBox;
            if (input != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(input);
                int selectedRowIndex = AdminVM.SelectedSubmRowIndex;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<CustomerContact> custPMs = AdminVM.CustContacts;
                        CustomerContact item = new CustomerContact();
                        custPMs.Add(item);
                        e.Handled = true;
                    }
                }
            }
        }

        private void CustContact_ChkPreviewKeyUp(object sender, RoutedEventArgs e)
        {
            CheckBox input = sender as CheckBox;
            DataGrid dataGrid = noteHelper.FindDataGrid(input);
            int selectedRowIndex = AdminVM.SelectedSubmRowIndex;
            if (dataGrid != null)
            {
                if (selectedRowIndex == dataGrid.Items.Count - 1)
                {

                    ObservableCollection<CustomerContact> custPMs = AdminVM.CustContacts;
                    CustomerContact item = new CustomerContact();
                    custPMs.Add(item);
                    e.Handled = true;
                }
            }
        }

        private void CustPM_PreviewKeyUp(object sender, RoutedEventArgs e)
        {
            TextBox input = sender as TextBox;
            if (input != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(input);
                int selectedRowIndex = AdminVM.SelectedPMRowIndex;
                if (dataGrid != null)
                {
                    if(selectedRowIndex == dataGrid.Items.Count - 1) {
                        ObservableCollection<ProjectManager> custPMs = AdminVM.CustPMs;
                        ProjectManager item = new ProjectManager();
                        custPMs.Add(item);
                        e.Handled = true;
                    }
                }
            }
        }

        private void CustPM_ChkPreviewKeyUp(object sender, RoutedEventArgs e)
        {
            CheckBox input = sender as CheckBox;
            DataGrid dataGrid = noteHelper.FindDataGrid(input);
            int selectedRowIndex = AdminVM.SelectedPMRowIndex;
            if (dataGrid != null)
            {
                if (selectedRowIndex == dataGrid.Items.Count - 1)
                {
                    ObservableCollection<ProjectManager> custPMs = AdminVM.CustPMs;
                    ProjectManager item = new ProjectManager();
                    custPMs.Add(item);
                    e.Handled = true;
                }
            }
        }

        private void ManufNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox NotesNote = sender as TextBox;
            if (NotesNote != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);
                int selectedRowIndex = AdminVM.SelectedManufNoteRowIndex;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<Note> notes = AdminVM.ManufNotes;
                        Grid parentGrid = NotesNote.Parent as Grid;
                        Grid grandParentGrid = parentGrid.Parent as Grid;
                        Grid firstChildGrid = grandParentGrid.Children[0] as Grid;
                        TextBlock NotesDateAdded = firstChildGrid.Children[0] as TextBlock;
                        TextBlock NoteUserName = firstChildGrid.Children[1] as TextBlock;
                        NotesDateAdded.Text = DateTime.Now.ToString();
                        NoteUserName.Text = "smile";

                        Note item = new Note();
                        notes.Add(item);
                        e.Handled = true;
                    }
                }
            }
        }
        
        private void CustomerNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox NotesNote = sender as TextBox;
            if (NotesNote != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);
                int selectedRowIndex = AdminVM.SelectedCustNoteRowIndex;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<Note> notes = AdminVM.CustomerNotes;
                        Grid parentGrid = NotesNote.Parent as Grid;
                        Grid grandParentGrid = parentGrid.Parent as Grid;
                        Grid firstChildGrid = grandParentGrid.Children[0] as Grid;
                        TextBlock NotesDateAdded = firstChildGrid.Children[0] as TextBlock;
                        TextBlock NoteUserName = firstChildGrid.Children[1] as TextBlock;
                        NotesDateAdded.Text = DateTime.Now.ToString();
                        NoteUserName.Text = "smile";

                        Note item = new Note();
                        notes.Add(item);
                        e.Handled = true;
                    }
                }
            }
        }

        private void PMNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox NotesNote = sender as TextBox;
            if (NotesNote != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);
                int selectedRowIndex = AdminVM.SelectedPMNoteRowIndex;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<Note> notes = AdminVM.PMNotes;
                        Grid parentGrid = NotesNote.Parent as Grid;
                        Grid grandParentGrid = parentGrid.Parent as Grid;
                        Grid firstChildGrid = grandParentGrid.Children[0] as Grid;
                        TextBlock NotesDateAdded = firstChildGrid.Children[0] as TextBlock;
                        TextBlock NoteUserName = firstChildGrid.Children[1] as TextBlock;
                        NotesDateAdded.Text = DateTime.Now.ToString();
                        NoteUserName.Text = "smile";

                        Note item = new Note();
                        notes.Add(item);
                        e.Handled = true;
                    }
                }
            }
        }

        private void SuptNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox NotesNote = sender as TextBox;
            if (NotesNote != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);
                int selectedRowIndex = AdminVM.SelectedSuptNoteRowIndex;
                if (dataGrid != null)
                {
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<Note> notes = AdminVM.SuptNotes;
                        Grid parentGrid = NotesNote.Parent as Grid;
                        Grid grandParentGrid = parentGrid.Parent as Grid;
                        Grid firstChildGrid = grandParentGrid.Children[0] as Grid;
                        TextBlock NotesDateAdded = firstChildGrid.Children[0] as TextBlock;
                        TextBlock NoteUserName = firstChildGrid.Children[1] as TextBlock;
                        NotesDateAdded.Text = DateTime.Now.ToString();
                        NoteUserName.Text = "smile";

                        Note item = new Note();
                        notes.Add(item);
                        e.Handled = true;
                    }
                }
            }
        }

        private void SubmNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox NotesNote = sender as TextBox;
            if (NotesNote != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);
                int selectedRowIndex = AdminVM.SelectedSubmRowIndex;
                if (dataGrid != null)
                {
                    TextBox lastRowTextBox = noteHelper.GetLastRowTextBox(dataGrid);
                    if (selectedRowIndex == dataGrid.Items.Count - 1)
                    {
                        ObservableCollection<Note> notes = AdminVM.SubmNotes;
                        Grid parentGrid = NotesNote.Parent as Grid;
                        Grid grandParentGrid = parentGrid.Parent as Grid;
                        Grid firstChildGrid = grandParentGrid.Children[0] as Grid;
                        TextBlock NotesDateAdded = firstChildGrid.Children[0] as TextBlock;
                        TextBlock NoteUserName = firstChildGrid.Children[1] as TextBlock;
                        NotesDateAdded.Text = DateTime.Now.ToString();
                        NoteUserName.Text = "smile";

                        Note item = new Note();
                        notes.Add(item);
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
