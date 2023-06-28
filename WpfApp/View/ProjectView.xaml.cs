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
using WpfApp.View.Dialog;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for ViewProject.xaml
    /// </summary>
    public partial class ProjectView : Page
    {
        private NoteHelper noteHelper;

        private ProjectViewModel ProjectVM;
        public ProjectView()
        {
            InitializeComponent();
            noteHelper = new NoteHelper();
            ProjectVM = new ProjectViewModel();
            this.DataContext = ProjectVM;
            Loaded += LoadPage;
        }
        
        //private void ProjectNote_PreviewKeyUp(object sender, KeyEventArgs e)
        //{
        //    TextBox NotesNote = sender as TextBox;
        //    if (NotesNote != null)
        //    {
        //        DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);

        //        if (dataGrid != null)
        //        {
        //            TextBox lastRowTextBox = noteHelper.GetLastRowTextBox(dataGrid);
        //            if (lastRowTextBox != null)
        //            {
        //                string lastRowText = lastRowTextBox.Text;
        //                if (!string.IsNullOrEmpty(lastRowText))
        //                {
        //                    ObservableCollection<Note> notes = ProjectVM.ProjectNotes;
        //                    Grid parentGrid = NotesNote.Parent as Grid;
        //                    Grid grandParentGrid = parentGrid.Parent as Grid;
        //                    Grid firstChildGrid = grandParentGrid.Children[0] as Grid;
        //                    TextBlock NotesDateAdded = firstChildGrid.Children[0] as TextBlock;
        //                    TextBlock NoteUserName = firstChildGrid.Children[1] as TextBlock;
        //                    NotesDateAdded.Text = DateTime.Now.ToString();
        //                    NoteUserName.Text = "smile";

        //                    Note item = new Note();
        //                    notes.Add(item);
        //                    e.Handled = true;
        //                }
        //            }
        //        }
        //    }
        //}

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

        private void CustomerCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewCustomerDialog newCustomerDlg = new NewCustomerDialog();
                newCustomerDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                CustomerName_CB.SelectedIndex = 1;
                newCustomerDlg.ShowDialog();
            }
        }

        private void EstimatorCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewEstimatorDialog newEstimatorDlg = new NewEstimatorDialog();
                newEstimatorDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = 1;
                newEstimatorDlg.ShowDialog();
            }
        }

        private void SubContactCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewSCDialog newSCDlg = new NewSCDialog();
                newSCDlg.CustomerID = ProjectVM.CustomerID;
                newSCDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = 1;
                newSCDlg.ShowDialog();
            }
        }

        private void ArchCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewArchitectDialog newArchDlg = new NewArchitectDialog();
                newArchDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = 1;
                newArchDlg.ShowDialog();
            }
        }

        private void PmCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewPmDialog newPmDlg = new NewPmDialog();
                newPmDlg.CustomerID = ProjectVM.CustomerID;
                newPmDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = 1;
                newPmDlg.ShowDialog();
            }
        }

        private void SuptCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewSuptDialog newSuptDlg = new NewSuptDialog();
                newSuptDlg.CustomerID = ProjectVM.CustomerID;
                newSuptDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = 1;
                newSuptDlg.ShowDialog();
            }
        }

        private void ProjectCrdCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewProjectCrdDialog newProjectCrdDlg = new NewProjectCrdDialog();
                newProjectCrdDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = 1;
                newProjectCrdDlg.ShowDialog();
            }
        }

        private void ArchRepCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewArchRepDialog newArchRepDlg = new NewArchRepDialog();
                newArchRepDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = 1;
                newArchRepDlg.ShowDialog();
            }
        }

        private void SovCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewSovDialog newSovDlg = new NewSovDialog();
                newSovDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = 1;
                newSovDlg.ShowDialog();
            }
        }

        private void CustomerCombo_Loaded(object sender, RoutedEventArgs e)
        {
            CustomerName_CB.SelectionChanged += CustomerCB_Changed;
            ComboBoxItem newItem = CustomerName_CB.ItemContainerGenerator.ContainerFromIndex(0) as ComboBoxItem;
            if (newItem != null)
            {
                newItem.IsEnabled = false;
            }
            CustomerName_CB.SelectedIndex = 1;
        }

        private void EstCombo_Loaded(object sender, RoutedEventArgs e)
        {
            Estimator_CB.SelectionChanged += EstimatorCB_Changed;
            Estimator_CB.SelectedIndex = 1;
        }

        private void ScCombo_Loaded(object sender, RoutedEventArgs e)
        {
            SubContact_CB.SelectionChanged += SubContactCB_Changed;
            SubContact_CB.SelectedIndex = 1;
        }

        private void ArchCombo_Loaded(object sender, RoutedEventArgs e)
        {
            Architect_CB.SelectionChanged += ArchCB_Changed;
            Architect_CB.SelectedIndex = 1;
        }

        private void PcCombo_Loaded(object sender, RoutedEventArgs e)
        {
            ProjectCoord_CB.SelectionChanged += ProjectCrdCB_Changed;
            ProjectCoord_CB.SelectedIndex = 1;
        }

        private void ArchRepCombo_Loaded(object sender, RoutedEventArgs e)
        {
            ArchRep_CB.SelectionChanged += ArchRepCB_Changed;
            ArchRep_CB.SelectedIndex = 1;
        }

        private void MaterialCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewMaterialDialog newMatDlg = new NewMaterialDialog();
                newMatDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = 1;
                newMatDlg.ShowDialog();
            }
        }

        private void ManufCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewManufDialog newManufDlg = new NewManufDialog();
                newManufDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = 1;
                newManufDlg.ShowDialog();
            }
        }

        private void FreightCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewFreightDialog newFreightDlg = new NewFreightDialog();
                newFreightDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = 1;
                newFreightDlg.ShowDialog();
            }
        }

        private void CrewCombo_Loaded(object sender, RoutedEventArgs e)
        {
            WorkCrew_CB.SelectionChanged += CrewCB_Changed;
            WorkCrew_CB.SelectedIndex = 1;
            Crew_CB.SelectionChanged += CrewCB_Changed;
            Crew_CB.SelectedIndex = 1;
        }

        private void CrewCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewCrewDialog newCrewDlg = new NewCrewDialog();
                newCrewDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = 1;
                newCrewDlg.ShowDialog();
            }
        }

        private void SuptCombo_Loaded(object sender, RoutedEventArgs e)
        {
            WorkSupt_CB.SelectionChanged += SuptCB_Changed;
            WorkSupt_CB.SelectedIndex = 1;
        }

        private void RemoveNoteItem(object sender, RoutedEventArgs e)
        {
            Note selectedItem = ProjectNote_DataGrid.SelectedItem as Note;
            int selectedIndex = ProjectNote_DataGrid.SelectedIndex;
            ProjectVM.ProjectNotes.RemoveAt(selectedIndex);
        }
    }
}
