using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp.Utils;
using WpfApp.ViewModel;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for ReportDetailView.xaml
    /// </summary>
    public partial class ReportDetailView : Page
    {

        private ReportDetailViewModel ReportDetailVM;
        private FindComponentHelper findComponentHelper;
        private bool datePickerLoaded = false;

        public ReportDetailView()
        {
            InitializeComponent();
            ReportDetailVM = new ReportDetailViewModel();
            findComponentHelper = new FindComponentHelper();
            this.DataContext = ReportDetailVM;
            Loaded += LoadPage;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("View/ReportView.xaml", UriKind.Relative));
        }

        private void LoadPage(object sender, RoutedEventArgs e)
        {
            ReportDetailViewModel loadContext = DataContext as ReportDetailViewModel;

            if (loadContext != null)
            {
                ReportDetailVM = new ReportDetailViewModel();

                int reportID = loadContext.ReportID;
                int projectID = loadContext.ProjectID;
                string jobNo = loadContext.JobNo;
                int salesmanID = loadContext.SalesmanID;
                int archRepID = loadContext.ArchRepID;
                int customerID = loadContext.CustomerID;
                int manufID = loadContext.ManufID;
                int architectID = loadContext.ArchitectID;
                int matID = loadContext.MaterialID;
                int crewID = loadContext.CrewID;
                int complete = loadContext.Complete;
                DateTime dateFrom = loadContext.DateFrom;
                DateTime toDate = loadContext.ToDate;
                string keyword = loadContext.Keyword;

                ReportDetailVM.ProjectID = projectID;
                ReportDetailVM.SelectedJobNo = jobNo;
                ReportDetailVM.SelectedSalesmanID = salesmanID;
                ReportDetailVM.SelectedArchRepID = archRepID;
                ReportDetailVM.SelectedManufID = manufID;
                ReportDetailVM.SelectedCustomerID = customerID;
                ReportDetailVM.SelectedArchitectID = architectID;
                ReportDetailVM.SelectedMatID = matID;
                ReportDetailVM.SelectedCrewID = crewID;
                ReportDetailVM.SelectedDateFrom = dateFrom;
                ReportDetailVM.SelectedToDate = toDate;
                ReportDetailVM.SelectedComplete = complete;
                ReportDetailVM.Keyword = keyword;
                ReportDetailVM.ReportID = reportID;

                this.DataContext = ReportDetailVM;
            }
        }

        private void GoProject(object sender, RoutedEventArgs e)
        {
            int parameterValue = (int)((Button)sender).CommandParameter;
            ProjectView projectPage = new ProjectView();
            ProjectViewModel projectVM = new ProjectViewModel();
            //projectVM.NavigationBackName = "ReportDetailView";
            projectVM.ProjectID = parameterValue;
            projectPage.DataContext = projectVM;
            this.NavigationService.Navigate(projectPage);
        }

        private void FieldMeasureTargetDate_Changed(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;
            var selectedDate = datePicker.SelectedDate;

            if (selectedDate is DateTime)
            {
                ReportDetailVM.SelectedDateFrom = (DateTime)selectedDate;
                ReportDetailVM.LoadFieldMeasureData();
            }
        }

        private void FieldMeasureTargetDate_Loaded(object sender, RoutedEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;
            datePicker.SelectedDateChanged += FieldMeasureTargetDate_Changed;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            TextBox textBox = new TextBox();
            textBox.Text = textBlock.Text;
            textBox.LostFocus += TextBox_LostFocus;

            Grid parentGrid = findComponentHelper.FindVisualParent<Grid>(textBlock);

            if (parentGrid != null)
            {
                if (parentGrid.Children.Count == 2)
                {
                    Grid.SetColumn(textBox, 1);

                    int textBlockIndex = parentGrid.Children.IndexOf(textBlock);
                    parentGrid.Children.RemoveAt(textBlockIndex);
                    parentGrid.Children.Insert(textBlockIndex, textBox);

                    textBox.Focus();
                }
            }

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            TextBlock newtextBlock = new TextBlock();
            newtextBlock.Text = textBox.Text;
            newtextBlock.MouseLeftButtonDown += TextBlock_MouseLeftButtonDown;

            Grid parentGrid = (Grid)textBox.Parent;

            if (parentGrid.Children.Count == 2)
            {
                Grid.SetColumn(newtextBlock, 1);

                int textBoxIndex = parentGrid.Children.IndexOf(textBox);
                parentGrid.Children.RemoveAt(textBoxIndex);
                parentGrid.Children.Insert(textBoxIndex, newtextBlock);
            }
        }

        private void CheckBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void JobsTDate_Loaded(object sender, RoutedEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;
            datePicker.SelectedDateChanged += JobsTDate_Changed;
        }

        private void JobsTDate_Changed(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;
            string tagName = datePicker.Tag as string;
            var selectedDate = datePicker.SelectedDate;

            if (selectedDate is DateTime)
            {
                ReportDetailVM.SelectedDateFrom = (DateTime)selectedDate;
                switch(tagName)
                {
                    case "JobArchRep":
                        ReportDetailVM.LoadJobByArchRepData();
                        break;
                    case "JobArchitect":
                        ReportDetailVM.LoadJobByArchitectData();
                        break;
                    case "JobManuf":
                        ReportDetailVM.LoadJobByManufacturerData();
                        break;
                }
            }
        }
    }
}
