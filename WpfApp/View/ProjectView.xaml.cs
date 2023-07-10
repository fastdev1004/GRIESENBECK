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
        private ValidateFieldHelper validateFieldHelper;

        private ProjectViewModel ProjectVM;
        public ProjectView()
        {
            InitializeComponent();
            findComponentHelper = new FindComponentHelper();
            validateFieldHelper = new ValidateFieldHelper();
            ProjectVM = new ProjectViewModel();
            this.DataContext = ProjectVM;
            Loaded += LoadPage;
        }

        //private void WorkOrderNote_PreviewKeyUp(object sender, KeyEventArgs e)
        //{
        //    TextBox NotesNote = sender as TextBox;
        //    if (NotesNote != null)
        //    {
        //        DataGrid dataGrid = findComponentHelper.FindDataGrid(NotesNote);

        //        if (dataGrid != null)
        //        {
        //            TextBox lastRowTextBox = findComponentHelper.GetLastRowTextBox(dataGrid);
        //            if (lastRowTextBox != null)
        //            {
        //                string lastRowText = lastRowTextBox.Text;
        //                if (!string.IsNullOrEmpty(lastRowText))
        //                {
        //                    ObservableCollection<Note> notes = ProjectVM.WorkOrderNotes;
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
            int selectedDataGridIndex = PM_DataGrid.SelectedIndex;

            ProjectManager selectedPM = comboBox.SelectedItem as ProjectManager;
           
            if (selectedIndex >= 0)
            {
                if (selectedDataGridIndex >= 0)
                {
                    int fetchID = ProjectVM.FetchProjectManagerList[selectedDataGridIndex].FetchID;
                   
                    ProjectManager originPM = ProjectVM.ProjectManagerList.Where(item => item.FetchID == fetchID).First();
                    if (!string.IsNullOrEmpty(selectedPM.PMName))
                    {
                        if (selectedPM.PMName.Equals("New"))
                        {
                            NewPmDialog newPmDlg = new NewPmDialog();
                            newPmDlg.CustomerID = ProjectVM.TempProject.CustomerID;
                            newPmDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            comboBox.SelectedValue = selectedPM.ID;
                            newPmDlg.ShowDialog();
                        }
                        else
                        {
                            selectedPM.FetchID = originPM.FetchID;
                            selectedPM.ActionFlag = originPM.ActionFlag;
                            selectedPM.ProjPmID = originPM.ProjPmID;
                            ProjectVM.ProjectManagerList[fetchID] = selectedPM;

                            ProjectVM.FetchProjectManagerList = new ObservableCollection<ProjectManager>();

                            foreach (ProjectManager item in ProjectVM.ProjectManagerList)
                            {
                                if (item.ActionFlag != 3 && item.ActionFlag != 4)
                                {
                                    ProjectVM.FetchProjectManagerList.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SuptCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;
            int selectedDataGridIndex = Supt_DataGrid.SelectedIndex;

            Superintendent selectedSup = comboBox.SelectedItem as Superintendent;

            if (selectedIndex >= 0)
            {
                if (selectedDataGridIndex >= 0)
                {
                    int fetchID = ProjectVM.FetchSuperintendentList[selectedDataGridIndex].FetchID;

                    Superintendent originSup = ProjectVM.SuperintendentList.Where(item => item.FetchID == fetchID).First();
                    if (!string.IsNullOrEmpty(selectedSup.SupName))
                    {
                        if (selectedSup.SupName.Equals("New"))
                        {
                            NewPmDialog newPmDlg = new NewPmDialog();
                            newPmDlg.CustomerID = ProjectVM.TempProject.CustomerID;
                            newPmDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            comboBox.SelectedValue = selectedSup.SupID;
                            newPmDlg.ShowDialog();
                        }
                        else
                        {
                            selectedSup.FetchID = originSup.FetchID;
                            selectedSup.ActionFlag = originSup.ActionFlag;
                            selectedSup.ProjSupID = originSup.ProjSupID;
                            ProjectVM.SuperintendentList[fetchID] = selectedSup;
                            ProjectVM.FetchSuperintendentList = new ObservableCollection<Superintendent>();

                            foreach (Superintendent item in ProjectVM.SuperintendentList)
                            {
                                if (item.ActionFlag != 3 && item.ActionFlag != 4)
                                {
                                    ProjectVM.FetchSuperintendentList.Add(item);
                                }
                            }
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

        private void CrewCombo_Loaded(object sender, RoutedEventArgs e)
        {
            Crew_CB.SelectionChanged += CrewCB_Changed;
            Crew_CB.SelectedIndex = -1;
        }

        private void CrewCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;

            Crew crew = comboBox.SelectedItem as Crew;
            string tagName = comboBox.Tag as string;

            if (crew != null)
            {
                if (crew.CrewName.Equals("New"))
                {
                    NewCrewDialog newCrewDlg = new NewCrewDialog();
                    newCrewDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    //if(tagName.Equals("ProjectCrew"))
                    //    comboBox.SelectedValue = ProjectVM.TempProject.CrewID;
                    comboBox.SelectedIndex = -1;
                    newCrewDlg.ShowDialog();
                }
            }
        }

        private void RemoveProjectNoteItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ProjectNote_DataGrid.SelectedIndex;
            if (selectedIndex != -1)
            {
                Note note = ProjectVM.ProjectNotes.Where(item => item.FetchID == ProjectVM.FetchProjectNotes[selectedIndex].FetchID).First();
                if (ProjectVM.FetchProjectNotes[selectedIndex].ActionFlag == 1)
                {
                    note.ActionFlag = 4;
                }
                else
                {
                    note.ActionFlag = 3;
                }
                ProjectVM.FetchProjectNotes = new ObservableCollection<Note>();
                foreach (Note item in ProjectVM.ProjectNotes)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchProjectNotes.Add(item);
                    }
                }
            }
        }

        private void RemoveDescItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ProjectLink_DataGrid.SelectedIndex;
            if (selectedIndex != -1)
            {
                ProjectLink projectLink = ProjectVM.ProjectLinks.Where(item => item.FetchID == ProjectVM.FetchProjectLinks[selectedIndex].FetchID).First();
                if (ProjectVM.FetchProjectLinks[selectedIndex].ActionFlag == 1)
                {
                    projectLink.ActionFlag = 4;
                }
                else
                {
                    projectLink.ActionFlag = 3;
                }
                ProjectVM.FetchProjectLinks = new ObservableCollection<ProjectLink>();
                foreach (ProjectLink item in ProjectVM.ProjectLinks)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchProjectLinks.Add(item);
                    }
                }
            }
        }

        private void RemovePmItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = PM_DataGrid.SelectedIndex;
            if (selectedIndex != -1)
            {
                ProjectManager pm = ProjectVM.ProjectManagerList.Where(item => item.FetchID == ProjectVM.FetchProjectManagerList[selectedIndex].FetchID).First();
                if (ProjectVM.FetchProjectManagerList[selectedIndex].ActionFlag == 1)
                {
                    pm.ActionFlag = 4;
                }
                else
                {
                    pm.ActionFlag = 3;
                }
                ProjectVM.FetchProjectManagerList = new ObservableCollection<ProjectManager>();
                foreach (ProjectManager item in ProjectVM.ProjectManagerList)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchProjectManagerList.Add(item);
                    }
                }
            }
        }

        private void RemoveSupItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = Supt_DataGrid.SelectedIndex;
            if (selectedIndex != -1)
            {
                Superintendent sup = ProjectVM.SuperintendentList.Where(item => item.FetchID == ProjectVM.FetchSuperintendentList[selectedIndex].FetchID).First();
                if (ProjectVM.FetchSuperintendentList[selectedIndex].ActionFlag == 1)
                {
                    sup.ActionFlag = 4;
                } else
                {
                    sup.ActionFlag = 3;
                }
                ProjectVM.FetchSuperintendentList = new ObservableCollection<Superintendent>();
                foreach (Superintendent item in ProjectVM.SuperintendentList)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchSuperintendentList.Add(item);
                    }
                }
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
                SovAcronym sovAcronym = ProjectVM.SovAcronyms.Where(acronym => acronym.ID == ProjectVM.FetchSovAcronyms[selectedIndex].ID).First();
                if (ProjectVM.FetchSovAcronyms[selectedIndex].ActionFlag == 1)
                {
                    sovAcronym.ActionFlag = 4;
                }
                else
                {
                    sovAcronym.ActionFlag = 3;
                }
                ProjectVM.FetchSovAcronyms = new ObservableCollection<SovAcronym>();
                foreach (SovAcronym item in ProjectVM.SovAcronyms)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchSovAcronyms.Add(item);
                    }
                }
            }
        }

        private void SovAcronymCombo_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            DataGridRow row = findComponentHelper.FindDataGridRow(comboBox);

            string tagName = comboBox.Tag as string;

            if (row != null)
            {
                if (tagName.Equals("SovAcronym"))
                {
                    SovAcronym data = row.DataContext as SovAcronym;
                    if (row.GetIndex() == 0)
                    {
                        comboBox.ItemsSource = ProjectVM.NewAcronyms;
                        comboBox.SelectedValue = data.SovAcronymName;
                    }
                }
                else if (tagName.Equals("SovMaterial"))
                {
                    SovMaterial data = row.DataContext as SovMaterial;
                    if (row.GetIndex() == 0)
                    {
                        comboBox.ItemsSource = ProjectVM.NewAcronyms;
                        comboBox.SelectedValue = data.SovAcronymName;
                    }
                }
                else if (tagName.Equals("SovLabor"))
                {
                    ProjectLabor data = row.DataContext as ProjectLabor;
                    if (row.GetIndex() == 0)
                    {
                        comboBox.ItemsSource = ProjectVM.NewAcronyms;
                        comboBox.SelectedValue = data.SovAcronymName;
                    }
                }
            }
        }

        private void SovAcronymCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;

            Acronym acronym = comboBox.SelectedItem as Acronym;
            DataGridRow row = findComponentHelper.FindDataGridRow(comboBox);
            int selectedDataGridIndex = 0;
            string tagName = comboBox.Tag as string;
            switch (tagName)
            {
                case "SovAcronym":
                    selectedDataGridIndex = Sov_DataGird.SelectedIndex;
                    break;
                case "SovMaterial":
                    selectedDataGridIndex = SovMat_DataGird.SelectedIndex;
                    break;
                case "SovLabor":
                    selectedDataGridIndex = SovLab_DataGird.SelectedIndex;
                    break;
            }

            if (acronym != null)
            {
                if (selectedDataGridIndex >= 0)
                {
                    if (tagName.Equals("SovAcronym"))
                    {
                        SovAcronym sovAcronym = row.DataContext as SovAcronym;
                        if (acronym.AcronymName.Equals("New"))
                        {
                            NewSovDialog newSovDlg = new NewSovDialog();
                            newSovDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            comboBox.SelectedValue = sovAcronym.SovAcronymName;
                            newSovDlg.ShowDialog();
                        }
                        else
                        {
                            ProjectVM.SovAcronyms[selectedDataGridIndex].SovAcronymName = acronym.AcronymName;
                            ProjectVM.SovAcronyms[selectedDataGridIndex].SovDesc = acronym.AcronymDesc;
                        }
                    }
                    else if (tagName.Equals("SovMaterial"))
                    {
                        ProjectVM.FetchSovMaterials[selectedDataGridIndex].SovAcronymName = acronym.AcronymName;
                        ProjectVM.FetchSovMaterials[selectedDataGridIndex].ProjSovID = acronym.ProjSovID;
                        ProjectVM.FetchSovMaterials[selectedDataGridIndex].CoItemNo = acronym.CoItemNo;
                    }
                    else if (tagName.Equals("SovLabor"))
                    {
                        ProjectVM.FetchSovLabors[selectedDataGridIndex].SovAcronymName = acronym.AcronymName;
                        ProjectVM.FetchSovLabors[selectedDataGridIndex].ProjSovID = acronym.ProjSovID;
                        ProjectVM.FetchSovLabors[selectedDataGridIndex].CoItemNo = acronym.CoItemNo;
                        ProjectVM.FetchSovLabors[selectedDataGridIndex].MatOnly = acronym.MatOnly;
                    }
                }
            }
        }

        private void RemoveSovMatItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = SovMat_DataGird.SelectedIndex;
            if (selectedIndex != -1)
            {
                SovMaterial sovMaterial = ProjectVM.SovMaterials.Where(material => material.FetchID == ProjectVM.FetchSovMaterials[selectedIndex].FetchID).First();
                if (ProjectVM.FetchSovMaterials[selectedIndex].ActionFlag == 1)
                {
                    sovMaterial.ActionFlag = 4;
                } else
                {
                    sovMaterial.ActionFlag = 3;
                }
                ProjectVM.FetchSovMaterials = new ObservableCollection<SovMaterial>();
                foreach (SovMaterial item in ProjectVM.SovMaterials)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchSovMaterials.Add(item);
                    }
                }
            }
        }

        private void SovMaterialCombo_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            DataGridRow row = findComponentHelper.FindDataGridRow(comboBox);

            string tagName = comboBox.Tag as string;

            if (row != null)
            {
                if (tagName.Equals("SovMaterial"))
                {
                    SovMaterial data = row.DataContext as SovMaterial;
                    if (row.GetIndex() == 0)
                    {
                        comboBox.ItemsSource = ProjectVM.NewMaterials;
                        comboBox.SelectedValue = data.MatID;
                    }
                }
            }
        }

        private void MaterialCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;

            Material material = comboBox.SelectedItem as Material;
            DataGridRow row = findComponentHelper.FindDataGridRow(comboBox);
            int selectedDataGridIndex = 0;
            string tagName = comboBox.Tag as string;
            switch (tagName)
            {
                case "SovMaterial":
                    selectedDataGridIndex = SovMat_DataGird.SelectedIndex;
                    break;
            }

            if (material != null)
            {
                if (selectedDataGridIndex >= 0)
                {
                    if (tagName.Equals("SovMaterial"))
                    {
                        SovMaterial sovMat = row.DataContext as SovMaterial;
                        if (material.MatDesc.Equals("New"))
                        {
                            NewMaterialDialog newMatDlg = new NewMaterialDialog();
                            newMatDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            comboBox.SelectedValue = sovMat.MatID;
                            newMatDlg.ShowDialog();
                        }
                        else
                        {
                            SovMaterial currentSovMat = ProjectVM.SovMaterials.Where(mat => mat.FetchID == ProjectVM.FetchSovMaterials[selectedDataGridIndex].FetchID).First();
                            currentSovMat.MatID = material.ID;
                            ProjectVM.FetchSovMaterials = new ObservableCollection<SovMaterial>();
                            foreach (SovMaterial item in ProjectVM.SovMaterials)
                            {
                                if (item.ActionFlag != 3 && item.ActionFlag != 4)
                                {
                                    ProjectVM.FetchSovMaterials.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void TotalCost_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string originalText = textBox.Text;

            if (originalText.StartsWith("$"))
            {
                textBox.Text = originalText.Substring(1);
            }
        }

        private void RemoveSovLabItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = SovLab_DataGird.SelectedIndex;
            if (selectedIndex != -1)
            {
                ProjectLabor sovLabor = ProjectVM.ProjectLabors.Where(labor => labor.ID == ProjectVM.FetchSovLabors[selectedIndex].ID).First();
                if (ProjectVM.FetchSovLabors[selectedIndex].ActionFlag == 1)
                {
                    sovLabor.ActionFlag = 4;
                }
                else
                {
                    sovLabor.ActionFlag = 3;
                }
                ProjectVM.FetchSovLabors = new ObservableCollection<ProjectLabor>();
                foreach (ProjectLabor item in ProjectVM.ProjectLabors)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchSovLabors.Add(item);
                    }
                }
            }
        }

        private void RemoveProjMatTrackingItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = Tracking_DataGrid.SelectedIndex;
            if (selectedIndex != -1)
            {
                 ProjectMatTracking projMatTracking = ProjectVM.ProjectMatTrackings.Where(tracking => tracking.FetchID == ProjectVM.FetchProjectMatTrackings[selectedIndex].FetchID).First();
                if (ProjectVM.FetchProjectMatTrackings[selectedIndex].ActionFlag == 1)
                {
                    projMatTracking.ActionFlag = 4;
                }
                else
                {
                    projMatTracking.ActionFlag = 3;
                }
                ProjectVM.FetchProjectMatTrackings = new ObservableCollection<ProjectMatTracking>();
                foreach (ProjectMatTracking item in ProjectVM.ProjectMatTrackings)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchProjectMatTrackings.Add(item);
                    }
                }
            }
        }

        private void ManufCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;

            Manufacturer manuf = comboBox.SelectedItem as Manufacturer;
            DataGridRow row = findComponentHelper.FindDataGridRow(comboBox);
            int selectedDataGridIndex = 0;
            string tagName = comboBox.Tag as string;
            switch (tagName)
            {
                case "ProjectMatTracking":
                    selectedDataGridIndex = Tracking_DataGrid.SelectedIndex;
                    break;
            }

            if (manuf != null)
            {
                if (selectedDataGridIndex >= 0)
                {
                    if (tagName.Equals("ProjectMatTracking"))
                    {
                        ProjectMatTracking projMatTracking = row.DataContext as ProjectMatTracking;
                        if (manuf.ManufacturerName.Equals("New"))
                        {
                            NewManufDialog newMatDlg = new NewManufDialog();
                            newMatDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            comboBox.SelectedValue = ProjectVM.FetchProjectMatTrackings[selectedDataGridIndex].ManufacturerID;
                            newMatDlg.ShowDialog();
                        }
                        else
                        {
                            ProjectMatTracking currentProjMatTracking = ProjectVM.ProjectMatTrackings.Where(tracking => tracking.FetchID == ProjectVM.FetchProjectMatTrackings[selectedDataGridIndex].FetchID).First();

                            currentProjMatTracking.ManufacturerID = manuf.ID;
                            ProjectVM.FetchProjectMatTrackings = new ObservableCollection<ProjectMatTracking>();
                            foreach (ProjectMatTracking item in ProjectVM.ProjectMatTrackings)
                            {
                                if (item.ActionFlag != 3 && item.ActionFlag != 4)
                                {
                                    ProjectVM.FetchProjectMatTrackings.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void MatTrackingCombo_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            DataGridRow row = findComponentHelper.FindDataGridRow(comboBox);

            string tagName = comboBox.Tag as string;

            if (row != null)
            {
                if (tagName.Equals("ProjectMatTracking"))
                {
                    ProjectMatTracking data = row.DataContext as ProjectMatTracking;
                    if (row.GetIndex() == 0)
                    {
                        comboBox.ItemsSource = ProjectVM.NewManufacturers;
                        comboBox.SelectedValue = data.ManufacturerID;
                    }
                }
            }
        }

        private void RemoveProjMatShippingItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = Shipping_DataGrid.SelectedIndex;
            if (selectedIndex != -1)
            {
                ProjectMatShip projMtShipping = ProjectVM.ProjectMatShips.Where(shipping => shipping.FetchID == ProjectVM.FetchProjectMatShips[selectedIndex].FetchID).First();
                if (ProjectVM.FetchProjectMatShips[selectedIndex].ActionFlag == 1)
                {
                    projMtShipping.ActionFlag = 4;
                }
                else
                {
                    projMtShipping.ActionFlag = 3;
                }
                ProjectVM.FetchProjectMatShips = new ObservableCollection<ProjectMatShip>();
                foreach (ProjectMatShip item in ProjectVM.ProjectMatShips)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchProjectMatShips.Add(item);
                    }
                }
            }
        }

        private void FreightCoCombo_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            DataGridRow row = findComponentHelper.FindDataGridRow(comboBox);

            string tagName = comboBox.Tag as string;

            if (row != null)
            {
                if (tagName.Equals("ProjectMatShipping"))
                {
                    ProjectMatShip data = row.DataContext as ProjectMatShip;
                    if (row.GetIndex() == 0)
                    {
                        comboBox.ItemsSource = ProjectVM.NewFreightCos;
                        comboBox.SelectedValue = data.FreightCoID;
                    }
                }
            }
        }

        private void FreightCB_Changed(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            int selectedIndex = comboBox.SelectedIndex;

            FreightCo freight = comboBox.SelectedItem as FreightCo;
            DataGridRow row = findComponentHelper.FindDataGridRow(comboBox);
            int selectedDataGridIndex = 0;
            string tagName = comboBox.Tag as string;
            switch (tagName)
            {
                case "ProjectMatShipping":
                    selectedDataGridIndex = Shipping_DataGrid.SelectedIndex;
                    break;
            }

            if (freight != null)
            {
                if (selectedDataGridIndex >= 0)
                {
                    if (tagName.Equals("ProjectMatShipping"))
                    {
                        ProjectMatShip projMatShipping = row.DataContext as ProjectMatShip;
                        if (freight.FreightName.Equals("New"))
                        {
                            NewFreightDialog newFreightDlg = new NewFreightDialog();
                            newFreightDlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            comboBox.SelectedValue = ProjectVM.FetchProjectMatShips[selectedDataGridIndex].FreightCoID;
                            newFreightDlg.ShowDialog();
                        }
                        else
                        {
                            ProjectMatShip currentProjMatShipping = ProjectVM.ProjectMatShips.Where(tracking => tracking.FetchID == ProjectVM.FetchProjectMatShips[selectedDataGridIndex].FetchID).First();

                            currentProjMatShipping.FreightCoID = freight.FreightCoID;
                            ProjectVM.FetchProjectMatShips = new ObservableCollection<ProjectMatShip>();
                            foreach (ProjectMatShip item in ProjectVM.ProjectMatShips)
                            {
                                if (item.ActionFlag != 3 && item.ActionFlag != 4)
                                {
                                    ProjectVM.FetchProjectMatShips.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ValidateNumberEmpty(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!validateFieldHelper.IsNumeric(e.Text) || string.IsNullOrEmpty(textBox.Text))
            {
                e.Handled = true;
            }
        }

        private void ValidateNumber(object sender, TextCompositionEventArgs e)
        {
            if (!validateFieldHelper.IsNumeric(e.Text))
            {
                e.Handled = true;
            }
        }

        private void RemoveInstallNoteItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = InstallNote_DataGrid.SelectedIndex;
            MenuItem menuItem = sender as MenuItem;
            //DataGridRow row = findComponentHelper.FindDataGridRow(menuItem);
            //InstallationNote data = row.DataContext as InstallationNote;

            if (selectedIndex != -1)
            {
                InstallationNote note = ProjectVM.InstallationNotes.Where(item => item.FetchID == ProjectVM.FetchInstallationNotes[selectedIndex].FetchID).First();
                if (ProjectVM.FetchInstallationNotes[selectedIndex].ActionFlag == 1)
                {
                    note.ActionFlag = 4;
                }
                else
                {
                    note.ActionFlag = 3;
                }
                ProjectVM.FetchInstallationNotes = new ObservableCollection<InstallationNote>();
                foreach (InstallationNote item in ProjectVM.InstallationNotes)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchInstallationNotes.Add(item);
                    }
                }
            }
        }

        private void RemoveContractItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = Contract_DataGrid.SelectedIndex;
            if (selectedIndex != -1)
            {
                Contract contract = ProjectVM.Contracts.Where(item => item.FetchID == ProjectVM.FetchContracts[selectedIndex].FetchID).First();
                if (ProjectVM.FetchContracts[selectedIndex].ActionFlag == 1)
                {
                    contract.ActionFlag = 4;
                }
                else
                {
                    contract.ActionFlag = 3;
                }
                ProjectVM.FetchContracts = new ObservableCollection<Contract>();
                foreach (Contract item in ProjectVM.Contracts)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchContracts.Add(item);
                    }
                }
            }
        }

        private void RemoveChangeOrderItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ChangeOrder_DataGrid.SelectedIndex;
            if (selectedIndex != -1)
            {
                ChangeOrder changeOrder = ProjectVM.ChangeOrders.Where(item => item.FetchID == ProjectVM.FetchChangeOrders[selectedIndex].FetchID).First();
                if (ProjectVM.FetchChangeOrders[selectedIndex].ActionFlag == 1)
                {
                    changeOrder.ActionFlag = 4;
                }
                else
                {
                    changeOrder.ActionFlag = 3;
                }
                ProjectVM.FetchChangeOrders = new ObservableCollection<ChangeOrder>();
                foreach (ChangeOrder item in ProjectVM.ChangeOrders)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchChangeOrders.Add(item);
                    }
                }
            }
        }

        private void RemoveCipItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = CIP_DataGrid.SelectedIndex;
            if (selectedIndex != -1)
            {
                CIP cip = ProjectVM.ProjectCIPs.Where(item => item.FetchID == ProjectVM.FetchProjectCIPs[selectedIndex].FetchID).First();
                if (ProjectVM.FetchProjectCIPs[selectedIndex].ActionFlag == 1)
                {
                    cip.ActionFlag = 4;
                }
                else
                {
                    cip.ActionFlag = 3;
                }
                ProjectVM.FetchProjectCIPs = new ObservableCollection<CIP>();
                foreach (CIP item in ProjectVM.ProjectCIPs)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchProjectCIPs.Add(item);
                    }
                }
            }
        }

        private void RemoveWorkNoteItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = WorkOrderNote_DataGrid.SelectedIndex;
            if (selectedIndex != -1)
            {
                Note note = ProjectVM.FetchWorkOrderNotes.Where(item => item.FetchID == ProjectVM.FetchWorkOrderNotes[selectedIndex].FetchID).First();
                if (ProjectVM.FetchWorkOrderNotes[selectedIndex].ActionFlag == 1)
                {
                    note.ActionFlag = 4;
                }
                else
                {
                    note.ActionFlag = 3;
                }
                ProjectVM.FetchWorkOrderNotes = new ObservableCollection<Note>();
                foreach (Note item in ProjectVM.WorkOrderNotes)
                {
                    if (item.ActionFlag != 3 && item.ActionFlag != 4)
                    {
                        ProjectVM.FetchWorkOrderNotes.Add(item);
                    }
                }
            }
        }

        private void WorkOrderMat_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = (DataGridRow)sender;
            int count = ProjectVM.WorkOrderMaterials.Count;
            ProjectMaterial projMat = row.DataContext as ProjectMaterial;

            WorkOrderMaterial workOrderMat = new WorkOrderMaterial { WodID = 0, WoID = ProjectVM.WorkOrderID, ProjMsID = projMat.ProjMsID, SovAcronymName = projMat.SovAcronym, MatName = projMat.MatName, ManufName = projMat.ManufName, MatQty = 0, CoItemNo = projMat.CoItemNo, ShipDate = projMat.DateShipped, ActionFlag = 1, FetchID = count };

            ProjectVM.WorkOrderMaterials.Add(workOrderMat);
            ProjectVM.FetchWorkOrderMaterials = new ObservableCollection<WorkOrderMaterial>();
            foreach (WorkOrderMaterial item in ProjectVM.WorkOrderMaterials)
            {
                if (item.ActionFlag != 3 && item.ActionFlag != 4)
                {
                    ProjectVM.FetchWorkOrderMaterials.Add(item);
                }
            }
        }

        private void WorkOrderLab_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = (DataGridRow)sender;
            int count = ProjectVM.WorkOrderLabors.Count;
            ProjectLabor projLab = row.DataContext as ProjectLabor;

            WorkOrderLabor workOrderLab = new WorkOrderLabor { WodID = 0, WoID = ProjectVM.WorkOrderID, ProjLabID = projLab.ProjLabID, CoItemNo = projLab.CoItemNo, SovAcronymName = projLab.SovAcronymName, LabDesc = projLab.LaborDesc, LabPhase = projLab.LaborPhase, LabQty = projLab.QtyReqd, UnitPrice = projLab.UnitPrice, ActionFlag = 1, FetchID = count };

            ProjectVM.WorkOrderLabors.Add(workOrderLab);
            ProjectVM.FetchWorkOrderLabors = new ObservableCollection<WorkOrderLabor>();
            foreach (WorkOrderLabor item in ProjectVM.WorkOrderLabors)
            {
                if (item.ActionFlag != 3 && item.ActionFlag != 4)
                {
                    ProjectVM.FetchWorkOrderLabors.Add(item);
                }
            }
        }

        private void LabQtyTextInput(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int selectedWoDatagrid = WoLab_DataGird.SelectedIndex;

            WorkOrderLabor workOrderLabor = ProjectVM.WorkOrderLabors.Where(item => item.FetchID == ProjectVM.FetchWorkOrderLabors[selectedWoDatagrid].FetchID).First();
            workOrderLabor.LabQty = float.Parse(textBox.Text);
        }

        private void MatTextInput(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int selectedWoDatagrid = WoMat_DataGird.SelectedIndex;

            WorkOrderMaterial workOrderMat = ProjectVM.WorkOrderMaterials.Where(item => item.FetchID == ProjectVM.FetchWorkOrderMaterials[selectedWoDatagrid].FetchID).First();
            workOrderMat.MatQty = float.Parse(textBox.Text);
        }
    }
}
