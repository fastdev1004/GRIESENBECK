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
using Microsoft.Win32;
using System.IO;
using System.Linq;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for ViewProject.xaml
    /// </summary>
    public partial class ProjectView : Page
    {
        private FindComponentHelper findComponentHelper;

        private ProjectViewModel ProjectVM;
        public ProjectView()
        {
            InitializeComponent();
            findComponentHelper = new FindComponentHelper();
            ProjectVM = new ProjectViewModel();
            this.DataContext = ProjectVM;
            Loaded += LoadPage;
        }

        private void WorkOrderNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox NotesNote = sender as TextBox;
            if (NotesNote != null)
            {
                DataGrid dataGrid = findComponentHelper.FindDataGrid(NotesNote);

                if (dataGrid != null)
                {
                    TextBox lastRowTextBox = findComponentHelper.GetLastRowTextBox(dataGrid);
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
            CheckBox checkBox = FindChild<CheckBox>(item);

            checkBox.IsChecked = !checkBox.IsChecked;
            e.Handled = true;
        }

        private static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T result)
                {
                    return result;
                }

                var descendant = FindChild<T>(child);
                if (descendant != null)
                {
                    return descendant;
                }
            }

            return null;
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
                CustomerName_CB.SelectedIndex = -1;
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
                comboBox.SelectedIndex = -1;
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
                newSCDlg.CustomerID = ProjectVM.TempProject.CustomerID;
                newSCDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = -1;
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
                comboBox.SelectedIndex = -1;
                newArchDlg.ShowDialog();
            }
        }

        private void PmCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
       
            ProjectManager pm = comboBox.SelectedItem as ProjectManager;

            if (pm != null)
            {
                if (PM_DataGrid.SelectedIndex >= 0)
                {
                    ProjectManager originPM = ProjectVM.ProjectManagerList[PM_DataGrid.SelectedIndex];
                    pm.ProjPmID = originPM.ProjPmID;
                    if (!string.IsNullOrEmpty(pm.PMName))
                    {
                        if (pm.PMName.Equals("New"))
                        {
                            NewPmDialog newPmDlg = new NewPmDialog();
                            newPmDlg.CustomerID = ProjectVM.TempProject.CustomerID;
                            newPmDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            comboBox.SelectedValue = originPM.ID;
                            newPmDlg.ShowDialog();
                        }
                        else
                        {
                            ProjectVM.ProjectManagerList[PM_DataGrid.SelectedIndex] = pm;
                        }
                    }
                }
            }
        }

        private void SuptCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;

            Superintendent supt = comboBox.SelectedItem as Superintendent;
            if(supt != null)
            {
                if (Supt_DataGrid.SelectedIndex >= 0)
                {
                    Superintendent originSupt = ProjectVM.SuperintendentList[Supt_DataGrid.SelectedIndex];
                    supt.ProjSupID = originSupt.ProjSupID;
                    if (!string.IsNullOrEmpty(supt.SupName))
                    {
                        if (supt.SupName.Equals("New"))
                        {
                            NewSuptDialog newSuptDlg = new NewSuptDialog();
                            newSuptDlg.CustomerID = ProjectVM.TempProject.CustomerID;
                            newSuptDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            comboBox.SelectedValue = originSupt.SupID;
                            newSuptDlg.ShowDialog();
                        } else
                        {
                            ProjectVM.SuperintendentList[Supt_DataGrid.SelectedIndex] = supt;
                        }
                    }
                }
            }
        }

        private void WorkSuptCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;

            if (selectedIndex == 0)
            {
                NewSuptDialog newSuptDlg = new NewSuptDialog();
                newSuptDlg.CustomerID = ProjectVM.TempProject.CustomerID;
                newSuptDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = -1;
                newSuptDlg.ShowDialog();
            }
            else
            {
                Superintendent supt = comboBox.SelectedItem as Superintendent;
                if (Supt_DataGrid.SelectedIndex >= 0)
                {
                    Superintendent originSupt = ProjectVM.SuperintendentList[Supt_DataGrid.SelectedIndex];
                    supt.ProjSupID = originSupt.ProjSupID;
                    ProjectVM.SuperintendentList[Supt_DataGrid.SelectedIndex] = supt;
                }
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
                comboBox.SelectedIndex = -1;
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
                comboBox.SelectedIndex = -1;
                newArchRepDlg.ShowDialog();
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
            CustomerName_CB.SelectedIndex = -1;
        }

        private void EstCombo_Loaded(object sender, RoutedEventArgs e)
        {
            Estimator_CB.SelectionChanged += EstimatorCB_Changed;
            Estimator_CB.SelectedIndex = -1;
        }

        private void ScCombo_Loaded(object sender, RoutedEventArgs e)
        {
            SubContact_CB.SelectionChanged += SubContactCB_Changed;
            SubContact_CB.SelectedIndex = -1;
        }

        private void ArchCombo_Loaded(object sender, RoutedEventArgs e)
        {
            Architect_CB.SelectionChanged += ArchCB_Changed;
            Architect_CB.SelectedIndex = -1;
        }

        private void PcCombo_Loaded(object sender, RoutedEventArgs e)
        {
            ProjectCoord_CB.SelectionChanged += ProjectCrdCB_Changed;
            ProjectCoord_CB.SelectedIndex = -1;
        }

        private void ArchRepCombo_Loaded(object sender, RoutedEventArgs e)
        {
            ArchRep_CB.SelectionChanged += ArchRepCB_Changed;
            ArchRep_CB.SelectedIndex = -1;
        }

        private void MaterialCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewMaterialDialog newMatDlg = new NewMaterialDialog();
                newMatDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = -1;
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
                comboBox.SelectedIndex = -1;
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
                comboBox.SelectedIndex = -1;
                newFreightDlg.ShowDialog();
            }
        }

        private void CrewCombo_Loaded(object sender, RoutedEventArgs e)
        {
            WorkCrew_CB.SelectionChanged += CrewCB_Changed;
            WorkCrew_CB.SelectedIndex = -1;
            Crew_CB.SelectionChanged += CrewCB_Changed;
            Crew_CB.SelectedIndex = -1;
        }

        private void CrewCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            if (selectedIndex == 0)
            {
                NewCrewDialog newCrewDlg = new NewCrewDialog();
                newCrewDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                comboBox.SelectedIndex = -1;
                newCrewDlg.ShowDialog();
            }
        }

        private void WorkSuptCombo_Loaded(object sender, RoutedEventArgs e)
        {
            WorkSupt_CB.SelectionChanged += WorkSuptCB_Changed;
            WorkSupt_CB.SelectedIndex = -1;
        }

        private void RemoveNoteItem(object sender, RoutedEventArgs e)
        {
            Note selectedItem = ProjectNote_DataGrid.SelectedItem as Note;
            int selectedIndex = ProjectNote_DataGrid.SelectedIndex;
            ProjectVM.ProjectNotes.RemoveAt(selectedIndex);
        }

        private void RemoveDescItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ProjectLink_DataGrid.SelectedIndex;
            if (selectedIndex != -1)
            {
                ProjectVM.ProjectLinks.RemoveAt(selectedIndex);
            }
        }

        private void RemovePmItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = PM_DataGrid.SelectedIndex;
            if (selectedIndex != -1)
            {
                ProjectVM.ProjectManagerList.RemoveAt(selectedIndex);
            }
        }

        private void RemoveSupItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = Supt_DataGrid.SelectedIndex;
            if (selectedIndex != -1)
            {
                ProjectVM.SuperintendentList.RemoveAt(selectedIndex);
            }
        }

        private void Project_FolderSelector(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();
            ProjectLink projectLink = ProjectLink_DataGrid.SelectedItem as ProjectLink;

            string currentFilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string selectedFolder = folderDialog.SelectedPath;
                projectLink.PathName = selectedFolder;
            }
        }

        private void Project_FileSelector(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ProjectLink projectLink = ProjectLink_DataGrid.SelectedItem as ProjectLink;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                projectLink.PathName = selectedFileName;
            }
        }
       
        private void CheckBox_HandleEvent(object sender, RoutedEventArgs e)
        {
            e.Handled = false;
        }

        private void CheckBox_PreviewClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void PmCombo_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            DataGridRow row = findComponentHelper.FindDataGridRow(comboBox);

            if (row != null)
            {
                ProjectManager data = row.DataContext as ProjectManager;

                if (row.GetIndex() == 0)
                {
                    comboBox.ItemsSource = ProjectVM.NewProjectManagers;
                    comboBox.SelectedValue = data.ID;
                }
            }
        }

        private void SuptCombo_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            DataGridRow row = findComponentHelper.FindDataGridRow(comboBox);

            if (row != null)
            {
                Superintendent data = row.DataContext as Superintendent;

                if (row.GetIndex() == 0)
                {
                    comboBox.ItemsSource = ProjectVM.NewSuperintendents;
                    comboBox.SelectedValue = data.SupID;
                }
            }
        }

        private void RemoveSovItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = Sov_DataGird.SelectedIndex;
            if (selectedIndex != -1)
            {
                ProjectVM.SovAcronyms.RemoveAt(selectedIndex);
            }
        }

        private void SovCombo_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            DataGridRow row = findComponentHelper.FindDataGridRow(comboBox);

            if (row != null)
            {
                SovAcronym data = row.DataContext as SovAcronym;

                if (row.GetIndex() == 0)
                {
                    comboBox.ItemsSource = ProjectVM.NewAcronyms;
                    comboBox.SelectedValue = data.SovAcronymName;
                }
            }
        }

        private void SovCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;

            Acronym acronym = comboBox.SelectedItem as Acronym;
            DataGridRow row = findComponentHelper.FindDataGridRow(comboBox);
           
            if (acronym != null)
            {
                
                if (Sov_DataGird.SelectedIndex >= 0)
                {
                    SovAcronym sovAcronym = row.DataContext as SovAcronym;
                    Acronym originAcronym = ProjectVM.Acronyms[Sov_DataGird.SelectedIndex];
                    Console.WriteLine("originAcronym.AcronymName->"+ originAcronym.AcronymName);
                    if (!string.IsNullOrEmpty(originAcronym.AcronymName))
                    {
                        if (acronym.AcronymName.Equals("New"))
                        {
                            NewSovDialog newSovDlg = new NewSovDialog();
                            newSovDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            comboBox.SelectedValue = sovAcronym.SovAcronymName;
                            newSovDlg.ShowDialog();
                        }
                        else
                        {
                            ProjectVM.SovAcronyms[Sov_DataGird.SelectedIndex].SovAcronymName = acronym.AcronymName;
                        }
                    }
                }
            }
        }
    }
}
