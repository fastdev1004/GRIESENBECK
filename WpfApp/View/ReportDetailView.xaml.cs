using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                    case "ActionLabor":
                        ReportDetailVM.LoadActiveLaborData();
                        break;
                    case "ChangeOrder":
                        ReportDetailVM.LoadChangeOrders();
                        break;
                    case "CIP":
                        ReportDetailVM.LoadCIPData();
                        break;
                    case "Contracts":
                        ReportDetailVM.LoadContractData();
                        break;
                    case "FieldMeasure":
                        ReportDetailVM.LoadFieldMeasureData();
                        break;
                    case "OpenJobsList":
                        ReportDetailVM.LoadOpenJobsData();
                        break;
                    case "PmMeeting":
                        ReportDetailVM.LoadPmMeetingData();
                        break;
                    case "ActiveProject":
                        ReportDetailVM.LoadActiveProjectData();
                        break;
                }
            }
        }

        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            e.Handled = true;
            DataGridColumn clickedColumn = e.Column;
            string clickedColumnName = clickedColumn.SortMemberPath;
            string sortColumn = "";
            string sortType = "ASC";
            string sortClause = " ORDER BY";
            string tagName = dataGrid.Tag as string;

            ListSortDirection direction = (clickedColumn.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;

            if (e.Column.SortDirection == null || e.Column.SortDirection == ListSortDirection.Descending)
            {
                e.Column.SortDirection = ListSortDirection.Ascending;
                sortType = "ASC";
            }
            else
            {
                e.Column.SortDirection = ListSortDirection.Descending;
                sortType = "DESC";
            }
            switch (clickedColumnName)
            {
                case "ManufName":
                    sortColumn = "Manuf_Name";
                    break;
                case "Contact":
                    sortColumn = "Contact_Name";
                    break;
                case "Phone":
                    sortColumn = "Contact_Phone";
                    break;
                case "ContactEmail":
                    sortColumn = "Contact_Email";
                    break;
                case "MatlReqd":
                    sortColumn = "MatReqdDate";
                    break;
                case "JobNo":
                    sortColumn = "Job_No";
                    break;
                case "ProjectName":
                    sortColumn = "Project_Name";
                    break;
                case "SalesmanName":
                    sortColumn = "Salesman_Name";
                    break;
                case "MaterialName":
                    sortColumn = "Material_Desc";
                    break;
                case "MatType":
                    sortColumn = "Mat_Type";
                    break;
                case "MatPhase":
                    sortColumn = "Mat_Phase";
                    break;
                case "PoNumber":
                    sortColumn = "PO_Number";
                    break;
                case "RFF":
                    sortColumn = "ReleasedForFab";
                    break;
                case "SchedShip":
                    sortColumn = "SchedShipDate";
                    break;
                case "EstDeliv":
                    sortColumn = "EstDelivDate";
                    break;
                case "ShopsRecvd":
                    sortColumn = "ShopRecvdDate";
                    break;
                case "ShopsReqd":
                    sortColumn = "ShopReqDate";
                    break;
                case "SubmIssue":
                    sortColumn = "SubmitIssue";
                    break;
                case "Resubmit":
                    sortColumn = "Resubmit_Date";
                    break;
                case "SubmAppr":
                    sortColumn = "SubmitAppr";
                    break;

            }
            sortClause = " ORDER BY " + sortColumn + " " + sortType;
            switch (tagName)
            {
                case "RptVendor":
                    ReportDetailVM.LoadVendorData(sortClause);
                    break;
                case "RptReleasedNotShip":
                    ReportDetailVM.LoadReleaseNotShipData(sortClause);
                    break;
                case "RptShipNotRecv":
                    ReportDetailVM.LoadShipNotRecvData(sortClause);
                    break;
                case "RptShopNotSubm":
                    ReportDetailVM.LoadShopRecvData(sortClause);
                    break;
                case "RptShopReqd":
                    ReportDetailVM.LoadShopReqData(sortClause);
                    break;
                case "RptSubmNotAppr":
                    ReportDetailVM.LoadSubmitNotApprData(sortClause);
                    break;
            }

            if (sortType.Equals("ASC"))
            {
                e.Column.SortDirection = ListSortDirection.Ascending;
            } else
            {
                e.Column.SortDirection = ListSortDirection.Descending;
            }
        }
    }
}
