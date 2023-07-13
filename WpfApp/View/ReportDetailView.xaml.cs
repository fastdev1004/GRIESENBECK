using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for ReportDetailView.xaml
    /// </summary>
    public partial class ReportDetailView : Page
    {
        private ReportDetailViewModel ReportDetailVM;
        private FindComponentHelper findComponentHelper;
        private DatabaseConnection dbConnection;
        private SqlCommand cmd = null;
        private ValidateFieldHelper validateFieldHelper;

        public ReportDetailView()
        {
            InitializeComponent();
            ReportDetailVM = new ReportDetailViewModel();
            findComponentHelper = new FindComponentHelper();
            dbConnection = new DatabaseConnection();
            validateFieldHelper = new ValidateFieldHelper();
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
            string tagName = textBlock.Tag as string;
            TextBox textBox = new TextBox();
            textBox.Text = textBlock.Text;
            textBox.LostFocus += TextBox_LostFocus;
            textBox.KeyUp += TextBox_KeyUp;
            textBox.Tag = tagName;

            if (tagName.Equals("BillingDue"))
            {
                textBox.PreviewTextInput += ValidateNumber;
            }

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
            newtextBlock.Tag = textBox.Tag;
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

        private void TextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string textBoxTagName = textBox.Tag as string;

            DataGrid dataGrid = findComponentHelper.FindVisualParent<DataGrid>(textBox);
            string dataGridTagName = dataGrid.Tag as string;
            DataGridRow dataGridRow = findComponentHelper.FindVisualParent<DataGridRow>(textBox);

            if (dataGridTagName.Equals("ActiveLabor"))
            {
                ReportActiveLabor rptActiveLabor = dataGridRow.DataContext as ReportActiveLabor;
                if (textBoxTagName.Equals("ProjectName"))
                {
                    int id = rptActiveLabor.ProjectID;
                    string projectName = textBox.Text;
                    rptActiveLabor.ProjectName = textBox.Text;
                    string sqlquery = "UPDATE tblProjects SET Project_Name=@ProjectName WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, projectName, "ProjectName");
                } else if (textBoxTagName.Equals("SalesmanName"))
                {
                    int id = rptActiveLabor.SalesmanID;
                    string salemanName = textBox.Text;
                    rptActiveLabor.SalesmanName = textBox.Text;
                    string sqlquery = "UPDATE tblSalesmen SET Salesman_Name=@SalesmanName WHERE Salesman_ID=@SalesmanID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, salemanName, "SalesmanName");
                } else if(textBoxTagName.Equals("JobNo"))
                {
                    int id = rptActiveLabor.ProjectID;
                    string jobNo = textBox.Text;
                    rptActiveLabor.JobNo = textBox.Text;
                    string sqlquery = "UPDATE tblProjects SET Job_No=@JobNo WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, jobNo, "JobNo");
                } else if (textBoxTagName.Equals("CustomerName"))
                {
                    int id = rptActiveLabor.CustomerID;
                    string customerName = textBox.Text;
                    rptActiveLabor.CustomerName = textBox.Text;
                    string sqlquery = "UPDATE tblCustomers SET Full_Name=@FullName WHERE Customer_ID=@CustomerID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, customerName, "CustomerName");
                }
            }
            else if (dataGridTagName.Equals("ActiveProject"))
            {
                ReportActiveProject rptActiveProject = dataGridRow.DataContext as ReportActiveProject;
                if (textBoxTagName.Equals("ProjectName"))
                {
                    int id = rptActiveProject.ProjectID;
                    string projectName = textBox.Text;
                    rptActiveProject.ProjectName = textBox.Text;
                    string sqlquery = "UPDATE tblProjects SET Project_Name=@ProjectName WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, projectName, "ProjectName");
                }
                else if (textBoxTagName.Equals("SalesmanName"))
                {
                    int id = rptActiveProject.SalesmanID;
                    string salemanName = textBox.Text;
                    rptActiveProject.SalesmanName = textBox.Text;
                    string sqlquery = "UPDATE tblSalesmen SET Salesman_Name=@SalesmanName WHERE Salesman_ID=@SalesmanID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, salemanName, "SalesmanName");
                }
                else if (textBoxTagName.Equals("JobNo"))
                {
                    int id = rptActiveProject.ProjectID;
                    string jobNo = textBox.Text;
                    rptActiveProject.JobNo = textBox.Text;
                    string sqlquery = "UPDATE tblProjects SET Job_No=@JobNo WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, jobNo, "JobNo");
                }
                else if (textBoxTagName.Equals("CustomerName"))
                {
                    int id = rptActiveProject.CustomerID;
                    string customerName = textBox.Text;
                    rptActiveProject.CustomerName = textBox.Text;
                    string sqlquery = "UPDATE tblCustomers SET Full_Name=@FullName WHERE Customer_ID=@CustomerID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, customerName, "CustomerName");
                }
            }
            else if (dataGridTagName.Equals("ChangeOrders"))
            {
                ReportCO rptCO = dataGridRow.DataContext as ReportCO;
                if (textBoxTagName.Equals("ProjectName"))
                {
                    int id = rptCO.ProjectID;
                    string projectName = textBox.Text;
                    rptCO.ProjectName = projectName;
                    string sqlquery = "UPDATE tblProjects SET Project_Name=@ProjectName WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, projectName, "ProjectName");
                }
                else if (textBoxTagName.Equals("SalesmanName"))
                {
                    int id = rptCO.SalesmanID;
                    string salemanName = textBox.Text;
                    rptCO.SalesmanName = salemanName;
                    string sqlquery = "UPDATE tblSalesmen SET Salesman_Name=@SalesmanName WHERE Salesman_ID=@SalesmanID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, salemanName, "SalesmanName");
                }
                else if (textBoxTagName.Equals("JobNo"))
                {
                    int id = rptCO.ProjectID;
                    string jobNo = textBox.Text;
                    rptCO.JobNo = jobNo;
                    string sqlquery = "UPDATE tblProjects SET Job_No=@JobNo WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, jobNo, "JobNo");
                }
                else if (textBoxTagName.Equals("CustomerName"))
                {
                    int id = rptCO.CustomerID;
                    string customerName = textBox.Text;
                    rptCO.CustomerName = customerName;
                    string sqlquery = "UPDATE tblCustomers SET Full_Name=@FullName WHERE Customer_ID=@CustomerID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, customerName, "CustomerName");
                }
            }
            else if (dataGridTagName.Equals("CIP"))
            {
                ReportCIP rptCIP = dataGridRow.DataContext as ReportCIP;

                if (textBoxTagName.Equals("SalesmanName"))
                {
                    int id = rptCIP.SalesmanID;
                    string salemanName = textBox.Text;
                    rptCIP.SalesmanName = salemanName;
                    string sqlquery = "UPDATE tblSalesmen SET Salesman_Name=@SalesmanName WHERE Salesman_ID=@SalesmanID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, salemanName, "SalesmanName");
                }
                else if (textBoxTagName.Equals("JobNo"))
                {
                    int id = rptCIP.ProjectID;
                    string jobNo = textBox.Text;
                    rptCIP.JobNo = jobNo;
                    string sqlquery = "UPDATE tblProjects SET Job_No=@JobNo WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, jobNo, "JobNo");
                }
                else if (textBoxTagName.Equals("CustomerName"))
                {
                    int id = rptCIP.CustomerID;
                    string customerName = textBox.Text;
                    rptCIP.CustomerName = customerName;
                    string sqlquery = "UPDATE tblCustomers SET Full_Name=@FullName WHERE Customer_ID=@CustomerID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, customerName, "CustomerName");
                }
            }
            else if (dataGridTagName.Equals("Contracts"))
            {
                ReportContract rptContract = dataGridRow.DataContext as ReportContract;

                if (textBoxTagName.Equals("ProjectName"))
                {
                    int id = rptContract.ProjectID;
                    string projectName = textBox.Text;
                    rptContract.ProjectName = projectName;
                    string sqlquery = "UPDATE tblProjects SET Project_Name=@ProjectName WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, projectName, "ProjectName");
                }
                else if (textBoxTagName.Equals("SalesmanName"))
                {
                    int id = rptContract.SalesmanID;
                    string salemanName = textBox.Text;
                    rptContract.SalesmanName = salemanName;
                    string sqlquery = "UPDATE tblSalesmen SET Salesman_Name=@SalesmanName WHERE Salesman_ID=@SalesmanID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, salemanName, "SalesmanName");
                }
                else if (textBoxTagName.Equals("JobNo"))
                {
                    int id = rptContract.ProjectID;
                    string jobNo = textBox.Text;
                    rptContract.JobNo = jobNo;
                    string sqlquery = "UPDATE tblProjects SET Job_No=@JobNo WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, jobNo, "JobNo");
                }
                else if (textBoxTagName.Equals("CustomerName"))
                {
                    int id = rptContract.CustomerID;
                    string customerName = textBox.Text;
                    rptContract.CustomerName = customerName;
                    string sqlquery = "UPDATE tblCustomers SET Full_Name=@FullName WHERE Customer_ID=@CustomerID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, customerName, "CustomerName");
                }
            }
            else if (dataGridTagName.Equals("FieldMeasure"))
            {
                ReportFieldMeasure rptFieldMeasure = dataGridRow.DataContext as ReportFieldMeasure;

                if (textBoxTagName.Equals("ProjectName"))
                {
                    int id = rptFieldMeasure.ProjectID;
                    string projectName = textBox.Text;
                    rptFieldMeasure.ProjectName = projectName;
                    string sqlquery = "UPDATE tblProjects SET Project_Name=@ProjectName WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, projectName, "ProjectName");
                }
                else if (textBoxTagName.Equals("SalesmanName"))
                {
                    int id = rptFieldMeasure.SalesmanID;
                    string salemanName = textBox.Text;
                    rptFieldMeasure.SalesmanName = salemanName;
                    string sqlquery = "UPDATE tblSalesmen SET Salesman_Name=@SalesmanName WHERE Salesman_ID=@SalesmanID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, salemanName, "SalesmanName");
                }
                else if (textBoxTagName.Equals("JobNo"))
                {
                    int id = rptFieldMeasure.ProjectID;
                    string jobNo = textBox.Text;
                    rptFieldMeasure.JobNo = jobNo;
                    string sqlquery = "UPDATE tblProjects SET Job_No=@JobNo WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, jobNo, "JobNo");
                }
                else if (textBoxTagName.Equals("CustomerName"))
                {
                    int id = rptFieldMeasure.CustomerID;
                    string customerName = textBox.Text;
                    rptFieldMeasure.CustomerName = customerName;
                    string sqlquery = "UPDATE tblCustomers SET Full_Name=@FullName WHERE Customer_ID=@CustomerID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, customerName, "CustomerName");
                }
            }
            else if (dataGridTagName.Equals("JobByArchRep"))
            {
                ReportJobArchRep rptJobArchRep = dataGridRow.DataContext as ReportJobArchRep;

                if (textBoxTagName.Equals("ProjectName"))
                {
                    int id = rptJobArchRep.ProjectID;
                    string projectName = textBox.Text;
                    rptJobArchRep.ProjectName = projectName;
                    string sqlquery = "UPDATE tblProjects SET Project_Name=@ProjectName WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, projectName, "ProjectName");
                }
                else if (textBoxTagName.Equals("SalesmanName"))
                {
                    int id = rptJobArchRep.SalesmanID;
                    string salemanName = textBox.Text;
                    rptJobArchRep.SalesmanName = salemanName;
                    string sqlquery = "UPDATE tblSalesmen SET Salesman_Name=@SalesmanName WHERE Salesman_ID=@SalesmanID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, salemanName, "SalesmanName");
                }
                else if (textBoxTagName.Equals("JobNo"))
                {
                    int id = rptJobArchRep.ProjectID;
                    string jobNo = textBox.Text;
                    rptJobArchRep.JobNo = jobNo;
                    string sqlquery = "UPDATE tblProjects SET Job_No=@JobNo WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, jobNo, "JobNo");
                }
                else if (textBoxTagName.Equals("CustomerName"))
                {
                    int id = rptJobArchRep.CustomerID;
                    string customerName = textBox.Text;
                    rptJobArchRep.CustomerName = customerName;
                    string sqlquery = "UPDATE tblCustomers SET Full_Name=@FullName WHERE Customer_ID=@CustomerID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, customerName, "CustomerName");
                }
                else if (textBoxTagName.Equals("Architect"))
                {
                    int id = rptJobArchRep.ArchitectID;
                    string architectName = textBox.Text;
                    rptJobArchRep.Architect = architectName;
                    string sqlquery = "UPDATE tblArchitects SET Arch_Company=@ArchCompany WHERE Architect_ID=@ArchitectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, architectName, "ArchitectName");
                }
                else if (textBoxTagName.Equals("ArchRep"))
                {
                    int id = rptJobArchRep.ArchRepID;
                    string archRepName = textBox.Text;
                    rptJobArchRep.ArchRep = archRepName;
                    string sqlquery = "UPDATE tblArchRep SET Arch_Rep_Name=@ArchRepName WHERE Arch_Rep_ID=@ArchRepID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, archRepName, "ArchRepName");
                }
            }
            else if (dataGridTagName.Equals("JobsByArchitect"))
            {
                ReportJobArchitect rptJobArch = dataGridRow.DataContext as ReportJobArchitect;

                if (textBoxTagName.Equals("ProjectName"))
                {
                    int id = rptJobArch.ProjectID;
                    string projectName = textBox.Text;
                    rptJobArch.ProjectName = projectName;
                    string sqlquery = "UPDATE tblProjects SET Project_Name=@ProjectName WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, projectName, "ProjectName");
                }
                else if (textBoxTagName.Equals("SalesmanName"))
                {
                    int id = rptJobArch.SalesmanID;
                    string salemanName = textBox.Text;
                    rptJobArch.SalesmanName = salemanName;
                    string sqlquery = "UPDATE tblSalesmen SET Salesman_Name=@SalesmanName WHERE Salesman_ID=@SalesmanID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, salemanName, "SalesmanName");
                }
                else if (textBoxTagName.Equals("JobNo"))
                {
                    int id = rptJobArch.ProjectID;
                    string jobNo = textBox.Text;
                    rptJobArch.JobNo = jobNo;
                    string sqlquery = "UPDATE tblProjects SET Job_No=@JobNo WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, jobNo, "JobNo");
                }
                else if (textBoxTagName.Equals("CustomerName"))
                {
                    int id = rptJobArch.CustomerID;
                    string customerName = textBox.Text;
                    rptJobArch.CustomerName = customerName;
                    string sqlquery = "UPDATE tblCustomers SET Full_Name=@FullName WHERE Customer_ID=@CustomerID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, customerName, "CustomerName");
                }
                else if (textBoxTagName.Equals("Architect"))
                {
                    int id = rptJobArch.ArchitectID;
                    string architectName = textBox.Text;
                    rptJobArch.Architect = architectName;
                    string sqlquery = "UPDATE tblArchitects SET Arch_Company=@ArchCompany WHERE Architect_ID=@ArchitectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, architectName, "ArchitectName");
                }
            }
            else if (dataGridTagName.Equals("JobsByManuf"))
            {
                ReportJobArchRep rptJobManuf= dataGridRow.DataContext as ReportJobArchRep;

                if (textBoxTagName.Equals("ProjectName"))
                {
                    int id = rptJobManuf.ProjectID;
                    string projectName = textBox.Text;
                    rptJobManuf.ProjectName = projectName;
                    string sqlquery = "UPDATE tblProjects SET Project_Name=@ProjectName WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, projectName, "ProjectName");
                }
                else if (textBoxTagName.Equals("SalesmanName"))
                {
                    int id = rptJobManuf.SalesmanID;
                    string salemanName = textBox.Text;
                    rptJobManuf.SalesmanName = salemanName;
                    string sqlquery = "UPDATE tblSalesmen SET Salesman_Name=@SalesmanName WHERE Salesman_ID=@SalesmanID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, salemanName, "SalesmanName");
                }
                else if (textBoxTagName.Equals("JobNo"))
                {
                    int id = rptJobManuf.ProjectID;
                    string jobNo = textBox.Text;
                    rptJobManuf.JobNo = jobNo;
                    string sqlquery = "UPDATE tblProjects SET Job_No=@JobNo WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, jobNo, "JobNo");
                }
                else if (textBoxTagName.Equals("CustomerName"))
                {
                    int id = rptJobManuf.CustomerID;
                    string customerName = textBox.Text;
                    rptJobManuf.CustomerName = customerName;
                    string sqlquery = "UPDATE tblCustomers SET Full_Name=@FullName WHERE Customer_ID=@CustomerID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, customerName, "CustomerName");
                }
                else if (textBoxTagName.Equals("Architect"))
                {
                    int id = rptJobManuf.ArchitectID;
                    string architectName = textBox.Text;
                    rptJobManuf.Architect = architectName;
                    string sqlquery = "UPDATE tblArchitects SET Arch_Company=@ArchCompany WHERE Architect_ID=@ArchitectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, architectName, "ArchitectName");
                }
            }
            else if (dataGridTagName.Equals("ActiveJobs"))
            {
                ReportActiveProject rptActiveJob = dataGridRow.DataContext as ReportActiveProject;

                if (textBoxTagName.Equals("ProjectName"))
                {
                    int id = rptActiveJob.ProjectID;
                    string projectName = textBox.Text;
                    rptActiveJob.ProjectName = projectName;
                    string sqlquery = "UPDATE tblProjects SET Project_Name=@ProjectName WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, projectName, "ProjectName");
                }
                else if (textBoxTagName.Equals("SalesmanName"))
                {
                    int id = rptActiveJob.SalesmanID;
                    string salemanName = textBox.Text;
                    rptActiveJob.SalesmanName = salemanName;
                    string sqlquery = "UPDATE tblSalesmen SET Salesman_Name=@SalesmanName WHERE Salesman_ID=@SalesmanID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, salemanName, "SalesmanName");
                }
                else if (textBoxTagName.Equals("JobNo"))
                {
                    int id = rptActiveJob.ProjectID;
                    string jobNo = textBox.Text;
                    rptActiveJob.JobNo = jobNo;
                    string sqlquery = "UPDATE tblProjects SET Job_No=@JobNo WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, jobNo, "JobNo");
                }
                else if (textBoxTagName.Equals("CustomerName"))
                {
                    int id = rptActiveJob.CustomerID;
                    string customerName = textBox.Text;
                    rptActiveJob.CustomerName = customerName;
                    string sqlquery = "UPDATE tblCustomers SET Full_Name=@FullName WHERE Customer_ID=@CustomerID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, customerName, "CustomerName");
                }
            }
            else if (dataGridTagName.Equals("PmMeetings"))
            {
                ReportPmMeeting rptPmMeeting = dataGridRow.DataContext as ReportPmMeeting;

                if (textBoxTagName.Equals("ProjectName"))
                {
                    int id = rptPmMeeting.ProjectID;
                    string projectName = textBox.Text;
                    rptPmMeeting.ProjectName = projectName;
                    string sqlquery = "UPDATE tblProjects SET Project_Name=@ProjectName WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, projectName, "ProjectName");
                }
                else if (textBoxTagName.Equals("JobNo"))
                {
                    int id = rptPmMeeting.ProjectID;
                    string jobNo = textBox.Text;
                    rptPmMeeting.JobNo = jobNo;
                    string sqlquery = "UPDATE tblProjects SET Job_No=@JobNo WHERE Project_ID=@ProjectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, jobNo, "JobNo");
                }
                else if (textBoxTagName.Equals("CustomerName"))
                {
                    int id = rptPmMeeting.CustomerID;
                    string customerName = textBox.Text;
                    rptPmMeeting.CustomerName = customerName;
                    string sqlquery = "UPDATE tblCustomers SET Full_Name=@FullName WHERE Customer_ID=@CustomerID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, customerName, "CustomerName");
                }
                else if (textBoxTagName.Equals("Architect"))
                {
                    int id = rptPmMeeting.ArchitectID;
                    string architectName = textBox.Text;
                    rptPmMeeting.ArchitectName = architectName;
                    string sqlquery = "UPDATE tblArchitects SET Arch_Company=@ArchCompany WHERE Architect_ID=@ArchitectID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, architectName, "ArchitectName");
                }
                else if (textBoxTagName.Equals("EstimatorName"))
                {
                    int id = rptPmMeeting.EstimatorID;
                    string estimatorName = textBox.Text;
                    rptPmMeeting.EstimatorName = estimatorName;
                    string sqlquery = "UPDATE tblEstimators SET Estimator_Name=@EstimatorName WHERE Estimator_ID=@EstimatorID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, estimatorName, "EstimatorName");
                }
                else if (textBoxTagName.Equals("PmName"))
                {
                    int id = rptPmMeeting.PmID;
                    string pmName = textBox.Text;
                    rptPmMeeting.PmName = pmName;
                    string sqlquery = "UPDATE tblProjectManagers SET PM_Name=@PmName WHERE PM_ID=@PmID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, pmName, "PmName");
                }
                else if (textBoxTagName.Equals("BillingDue"))
                {
                    int id = rptPmMeeting.ProjectID;
                    string billingDue = textBox.Text;
                    rptPmMeeting.BilingDue = int.Parse(billingDue);
                    string sqlquery = "UPDATE tblProjects SET Billing_Date="+ billingDue +" WHERE Project_ID="+ id;
                    cmd = dbConnection.RunQuryNoParameters(sqlquery);
                }
            }
            else if (dataGridTagName.Equals("CustomerContacts"))
            {
                ReportCustomerContact rptCustomerContact = dataGridRow.DataContext as ReportCustomerContact;

                if (textBoxTagName.Equals("CustomerName"))
                {
                    int id = rptCustomerContact.CustomerID;
                    string customerName = textBox.Text;
                    rptCustomerContact.FullName = customerName;
                    string sqlquery = "UPDATE tblCustomers SET Full_Name=@FullName WHERE Customer_ID=@CustomerID";
                    cmd = dbConnection.RunQueryToUpdateField(sqlquery, id, customerName, "CustomerName");
                }
            }
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            checkBox.Checked += CheckBox_Click;
            checkBox.Unchecked += CheckBox_Click;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            string checkBoxTagName = checkBox.Tag as string;

            DataGrid dataGrid = findComponentHelper.FindVisualParent<DataGrid>(checkBox);
            string dataGridTagName = dataGrid.Tag as string;
            DataGridRow dataGridRow = findComponentHelper.FindVisualParent<DataGridRow>(checkBox);
            
            if (dataGridTagName.Equals("FieldMeasure"))
            {
                if (checkBoxTagName.Equals("StoredMat"))
                {
                    ReportFieldMeasure rptFieldMeasure = dataGridRow.DataContext as ReportFieldMeasure;
                    int id = rptFieldMeasure.ProjectID;
                    bool? checkBox_Checked = checkBox.IsChecked;
                    rptFieldMeasure.StoredMat = checkBox_Checked ?? false;
                    string sqlquery = "UPDATE tblProjects SET Stored_Materials="+ Convert.ToInt32(checkBox_Checked) +" WHERE Project_ID=" + id;
                    cmd = dbConnection.RunQuryNoParameters(sqlquery);
                }
            }
            else if (dataGridTagName.Equals("PmMeetings"))
            {
                ReportPmMeeting rptPmMeeting = dataGridRow.DataContext as ReportPmMeeting;
                if (checkBoxTagName.Equals("CipProject"))
                {
                    int id = rptPmMeeting.ProjectID;
                    bool? checkBox_Checked = checkBox.IsChecked;
                    rptPmMeeting.IsCip = checkBox_Checked ?? false;
                    string sqlquery = "UPDATE tblProjects SET CIP_Project=" + Convert.ToInt32(checkBox_Checked) + " WHERE Project_ID=" + id;
                    cmd = dbConnection.RunQuryNoParameters(sqlquery);
                }
                else if (checkBoxTagName.Equals("C3"))
                {
                    int id = rptPmMeeting.ProjectID;
                    bool? checkBox_Checked = checkBox.IsChecked;
                    rptPmMeeting.IsC3 = checkBox_Checked ?? false;
                    string sqlquery = "UPDATE tblProjects SET C3=" + Convert.ToInt32(checkBox_Checked) + " WHERE Project_ID=" + id;
                    cmd = dbConnection.RunQuryNoParameters(sqlquery);
                }
                if (checkBoxTagName.Equals("ContractRecvd"))
                {
                    int id = rptPmMeeting.ProjectID;
                    bool? checkBox_Checked = checkBox.IsChecked;
                    rptPmMeeting.IsContractRecvd = checkBox_Checked ?? false;
                    string sqlquery = "UPDATE tblProjects SET Contract_Rcvd=" + Convert.ToInt32(checkBox_Checked) + " WHERE Project_ID=" + id;
                    cmd = dbConnection.RunQuryNoParameters(sqlquery);
                }
                if (checkBoxTagName.Equals("CertPayReqd"))
                {
                    int id = rptPmMeeting.ProjectID;
                    bool? checkBox_Checked = checkBox.IsChecked;
                    rptPmMeeting.IsCertPayroll = checkBox_Checked ?? false;
                    string sqlquery = "UPDATE tblProjects SET CertPay_Reqd=" + Convert.ToInt32(checkBox_Checked) + " WHERE Project_ID=" + id;
                    cmd = dbConnection.RunQuryNoParameters(sqlquery);
                }
                if (checkBoxTagName.Equals("StoredMat"))
                {
                    int id = rptPmMeeting.ProjectID;
                    bool? checkBox_Checked = checkBox.IsChecked;
                    rptPmMeeting.StoredMat = checkBox_Checked ?? false;
                    string sqlquery = "UPDATE tblProjects SET Stored_Materials=" + Convert.ToInt32(checkBox_Checked) + " WHERE Project_ID=" + id;
                    cmd = dbConnection.RunQuryNoParameters(sqlquery);
                }
            }
        }

        private void ValidateNumber(object sender, TextCompositionEventArgs e)
        {
            if (!validateFieldHelper.IsNumeric(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
