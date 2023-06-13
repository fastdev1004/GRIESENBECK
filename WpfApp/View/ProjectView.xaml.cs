using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using WpfApp.ViewModel;
using WpfApp.Model;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Data;
using WpfApp.Controls;
using System.Drawing;
using System.Windows.Media;
using WpfApp.Utils;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for ViewProject.xaml
    /// </summary>
    public partial class ProjectView : Page
    {
        private DatabaseConnection dbConnection;
        private NoteHelper noteHelper;

        private ProjectViewModel ProjectVM;
        public ProjectView()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            noteHelper = new NoteHelper();
            ProjectVM = new ProjectViewModel();
            this.DataContext = ProjectVM;
            Loaded += LoadPage;
        }
        
        private void ProjectNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox NotesNote = sender as TextBox;
            if (NotesNote != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);

                if (dataGrid != null)
                {
                    TextBox lastRowTextBox = noteHelper.GetLastRowTextBox(dataGrid);
                    if (lastRowTextBox != null)
                    {
                        string lastRowText = lastRowTextBox.Text;
                        if (!string.IsNullOrEmpty(lastRowText))
                        {
                            ObservableCollection<Note> notes = ProjectVM.ProjectNotes;
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

        private void WorkOrderNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox NotesNote = sender as TextBox;
            if (NotesNote != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);

                if (dataGrid != null)
                {
                    TextBox lastRowTextBox = noteHelper.GetLastRowTextBox(dataGrid);
                    if (lastRowTextBox != null)
                    {
                        string lastRowText = lastRowTextBox.Text;
                        if (!string.IsNullOrEmpty(lastRowText))
                        {
                            ObservableCollection<Note> notes = ProjectVM.WorkOrderNotes;
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

        private void LoadPage(object sender, RoutedEventArgs e)
        {
            ProjectViewModel loadContext = DataContext as ProjectViewModel;

            if (loadContext != null)
            {
                ProjectVM = new ProjectViewModel();

                int projectID = loadContext.ProjectID;
                string navigationBackName = loadContext.NavigationBackName;

                ProjectVM.ProjectID = projectID;
                ProjectVM.NavigationBackName = navigationBackName;

                this.DataContext = ProjectVM;
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            //string navigationBackName = ProjectVM.NavigationBackName;
            //switch (navigationBackName)
            //{
            //    case "ReportDetailView":
            //        this.NavigationService.Navigate(new Uri("View/ReportDetailView.xaml", UriKind.Relative));
            //        break;
            //    default:
            //        this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
            //        break;
            //}
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }

        private void SelectPaymentComboBoxItem(object sender, MouseButtonEventArgs e)
        {
            ComboBoxItem item = sender as ComboBoxItem;
            Payment Payment = new Payment();
            Payment dataObject = item.DataContext as Payment;
            dataObject.IsChecked = !dataObject.IsChecked;
            
            e.Handled = true;
        }

        private void SelectPaymentCheckBox(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
