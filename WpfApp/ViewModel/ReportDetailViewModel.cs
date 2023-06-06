using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WpfApp.Model;
using WpfApp.Utils;
using System.Diagnostics;

namespace WpfApp.ViewModel
{
    class ReportDetailViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataSet ds;
        public string sqlquery;

        public ReportDetailViewModel()
        {
            dbConnection = new DatabaseConnection();
        }

        public void LoadActiveLaborData()
        {
            dbConnection.Open();
            // Track Labor List
            sqlquery = "SELECT tblLabor.*, tblProjects.Target_Date FROM tblProjects RIGHT JOIN (SELECT tblLab.SOV_Acronym, tblLab.Labor_Desc, tblProjectChangeOrders.CO_ItemNo, tblLab.Lab_Phase, tblLab.Complete, tblLab.Project_ID FROM tblProjectChangeOrders RIGHT JOIN (SELECT tblSOV.SOV_Acronym, tblLabor.Labor_Desc, tblSOV.CO_ID, tblSOV.Lab_Phase, tblSOV.Complete, tblSOV.Project_ID FROM tblLabor RIGHT JOIN (SELECT  tblProjectLabor.Labor_ID, tblProjectLabor.Lab_Phase, tblSOV.ProjSOV_ID, tblSOV.SOV_Acronym, tblSOV.CO_ID, tblProjectLabor.Complete, tblSOV.Project_ID FROM tblProjectLabor RIGHT JOIN (SELECT * FROM tblProjectSOV) AS tblSOV ON tblProjectLabor.ProjSOV_ID = tblSOV.ProjSOV_ID) AS tblSOV ON tblSOV.Labor_ID = tblLabor.Labor_ID) AS tblLab ON tblLab.CO_ID = tblProjectChangeOrders.CO_ID) AS tblLabor ON tblProjects.Project_ID=tblLabor.Project_ID";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<TrackLaborReport> sb_trackLaborReports = new ObservableCollection<TrackLaborReport>();
            foreach (DataRow row1 in ds.Tables[0].Rows)
            {
                string sovAcronym = "";
                string laborDesc = "";
                string coItemNo = "";
                string labPhase = "";
                DateTime targetDate = new DateTime();
                bool laborComplete = false;
                int projectID = 0;

                if (!row1.IsNull("Target_Date"))
                    targetDate = row1.Field<DateTime>("Target_Date");
                if (!row1.IsNull("SOV_Acronym"))
                    sovAcronym = row1["SOV_Acronym"].ToString();
                if (!row1.IsNull("Labor_Desc"))
                    laborDesc = row1["Labor_Desc"].ToString();
                if (!row1.IsNull("CO_ItemNo"))
                    coItemNo = row1["CO_ItemNo"].ToString();
                if (!row1.IsNull("Lab_Phase"))
                    labPhase = row1["Lab_Phase"].ToString();
                if (!row1.IsNull("Complete"))
                    laborComplete = row1.Field<Boolean>("Complete");
                if (!row1.IsNull("Project_ID"))
                    projectID = int.Parse(row1["Project_ID"].ToString());

                sb_trackLaborReports.Add(new TrackLaborReport
                {
                    SovAcronym = sovAcronym,
                    LaborDesc = laborDesc,
                    CoItemNo = coItemNo,
                    LabPhase = labPhase,
                    Complete = laborComplete,
                    TargetDate = targetDate.Date,
                    ProjectID = projectID
                });
            }
            TrackLaborReports = sb_trackLaborReports;

            // Labor View Detail Reports
            string whereClause = " WHERE 1=1";
            if (!string.IsNullOrEmpty(SelectedJobNo))
            {
                whereClause += $" AND tblProjects.Job_No = '{SelectedJobNo}'";
            }

            if (ProjectID != 0)
            {
                whereClause += $" AND tblProjects.Project_ID = '{ProjectID}'";
            }

            if (SelectedSalesmanID != 0)
            {
                whereClause += $" AND tblProjects.Salesman_ID = '{SelectedSalesmanID}'";
            }

            if (!SelectedDateFrom.Equals(DateTime.MinValue))
            {
                whereClause += $" AND tblProjects.Target_Date >= '{SelectedDateFrom}'";
            }

            if (!SelectedToDate.Equals(DateTime.MinValue))
            {
                whereClause += $" AND tblProjects.Date_Completed <= '{SelectedDateFrom}'";
            }

            //if (!string.IsNullOrEmpty(Keyword))
            //{
            //    whereClause += $" AND Project_Name = '{Keyword}'";
            //}

            sqlquery = "SELECT TOP 10 tblProjects.*, tblSalesmen.Salesman_Name FROM tblSalesmen RIGHT JOIN (SELECT tblprojects.*, tblCustomers.Full_Name FROM tblCustomers RIGHT JOIN(SELECT tblprojects.Project_ID, tblProjects.Project_Name, tblprojects.Job_No, tblprojects.Customer_ID, tblprojects.Salesman_ID FROM tblProjects RIGHT JOIN(SELECT DISTINCT Project_ID FROM tblProjectLabor) AS tblProjectID ON tblProjects.Project_ID = tblProjectID.Project_ID"+ whereClause +") AS tblProjects ON tblprojects.Customer_ID = tblCustomers.Customer_ID) AS tblProjects ON tblProjects.Salesman_ID = tblSalesmen.Salesman_ID ORDER BY Project_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportActiveLabor> st_activeLabors = new ObservableCollection<ReportActiveLabor>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                string _projectName = "";
                string _jobNo = "";
                string _customerName = "";
                string _salesmanName = "";

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Full_Name"))
                    _customerName = row["Full_Name"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();

                List<TrackLaborReport> _trackLaborReports = TrackLaborReports.Where(item => item.ProjectID == _projectID).ToList();

                st_activeLabors.Add(new ReportActiveLabor { ID = _projectID, ProjectName = _projectName, JobNo = _jobNo, CustomerName = _customerName, SalesmanName = _salesmanName, LaborReports= _trackLaborReports });
            }
            ReportActiveLabors = st_activeLabors;
            dbConnection.Close();
        }

        public void LoadApprovedNotReleaseData()
        {
            dbConnection.Open();
            string whereClause = " WHERE 1=1";
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (!string.IsNullOrEmpty(SelectedJobNo))
            {
                whereClause += $" AND tblProjects.Job_No = '{SelectedJobNo}'";
            }

            if (ProjectID != 0)
            {
                whereClause += $" AND tblProjects.Project_ID = '{ProjectID}'";
            }

            if (SelectedSalesmanID != 0)
            {
                whereClause += $" AND tblProjects.Salesman_ID = '{SelectedSalesmanID}'";
            }

            if (SelectedSalesmanID != 0)
            {
                whereClause += $" AND tblProjects.Customer_ID = '{SelectedCustomerID}'";
            }

            if (!SelectedDateFrom.Equals(DateTime.MinValue))
            {
                whereClause += $" AND tblProjects.Target_Date >= '{SelectedDateFrom}'";
            }

            if (!SelectedToDate.Equals(DateTime.MinValue))
            {
                whereClause += $" AND tblProjects.Date_Completed <= '{SelectedDateFrom}'";
            }

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            //if (!string.IsNullOrEmpty(Keyword))
            //{
            //    whereClause += $" AND Project_Name = '{Keyword}'";
            //}

            sqlquery = "SELECT tblManufacturers.Manuf_Name, tblMat.* FROM tblManufacturers RIGHT JOIN (SELECT tblMaterials.Material_Desc, tblMat.* FROM tblMaterials RIGHT JOIN (SELECT tblSalesmen.Salesman_Name, tblMat.* FROM tblSalesmen RIGHT JOIN (SELECT tblMat.Project_ID, MatReqdDate, Job_No, Project_Name, Salesman_ID, Material_ID, Manuf_ID, SubmitAppr, Manuf_LeadTime FROM tblProjectMaterialsTrack INNER JOIN (SELECT tblMat.Project_ID, tblProjectMaterials.ProjMat_ID, Material_ID, Job_No, Project_Name, Salesman_ID FROM tblProjectMaterials INNER JOIN (SELECT Project_ID, Project_Name, Job_No, Salesman_ID FROM tblProjects " + whereClause + ") AS tblMat ON tblProjectMaterials.Project_ID = tblMat.Project_ID) AS tblMat ON tblProjectMaterialsTrack.ProjMat_ID = tblMat.ProjMat_ID) AS tblMat ON tblSalesmen.Salesman_ID = tblMat.Salesman_ID) AS tblMat ON tblMat.Material_ID = tblMaterials.Material_ID "+ whereClause2 +") AS tblMat ON tblMat.Manuf_ID = tblManufacturers.Manuf_ID" + whereClause1;

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportApproveNotRelease> sb_reportApproveNotReleases = new ObservableCollection<ReportApproveNotRelease>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                DateTime _matReqDate = new DateTime();
                string _jobNo = "";
                string _projectName = "";
                string _salesmanName = "";
                string _materialName = "";
                string _manufName = "";
                DateTime _submAppr = new DateTime();
                string _leadTime = "";

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matReqDate = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Material_Desc"))
                    _materialName = row["Material_Desc"].ToString();
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("SubmitAppr"))
                    _submAppr = row.Field<DateTime>("SubmitAppr");
                if (!row.IsNull("Manuf_LeadTime"))
                    _leadTime = row["Manuf_LeadTime"].ToString();

                sb_reportApproveNotReleases.Add(new ReportApproveNotRelease
                {
                    ProjectID = _projectID,
                    MatReqDate = _matReqDate,
                    JobNo = _jobNo,
                    ProjectName = _projectName,
                    SalesmanName = _salesmanName,
                    MaterialName = _materialName,
                    ManufName = _manufName,
                    SubmAppr = _submAppr,
                    LeadTime = _leadTime
                });
            }

            ReportApproveNotReleases = sb_reportApproveNotReleases;

            //ReportApproveNotReleases
            dbConnection.Close();
        }

        private int _projectID;

        public int ProjectID
        {
            get { return _projectID; }
            set
            {
                if (value == _projectID) return;
                _projectID = value;
                OnPropertyChanged();
            }
        }

        private int _reportID;

        public int ReportID
        {
            get { return _reportID; }
            set
            {
                if (value == _reportID) return;
                _reportID = value;
                ChangeReportID();
                OnPropertyChanged();
            }
        }
         
        private void ChangeReportID()
        {
            ActiveLaborListVisibility = false;
            ApprovedVisibility = false;
            ChangeOrderVisibility = false;
            CONotRetVisibility = false;
            CipVisibility = false;
            ContractVisibility = false;
            ContractNotRetVisibility = false;
            CustomerContactVisibility = false;
            FieldMeasureVisibility = false;
            FinalFabVisibility = false;
            InstallForecastVisibility = false;
            JobArchRepVisibility = false;
            JobArchitectVisibility = false;
            JobManufVisibility = false;
            LeedVisibility = false;
            OpenJobVisibility = false;
            PastDueVisibility = false;
            PmVisibility = false;
            ReleasedVisibility = false;
            ShipVisibility = false;
            ShopRecvVisibility = false;
            SubmitVisibility = false;
            ShopReqVisibility = false;
            VendorVisibility = false;
            switch (ReportID)
            {
                case 13:
                    ActiveLaborListVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 10:
                    ApprovedVisibility = true;
                    LoadApprovedNotReleaseData();
                    break;
                case 23:
                    ChangeOrderVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 25:
                    CONotRetVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 19:
                    CipVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 18:
                    ContractVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 24:
                    ContractNotRetVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 27:
                    CustomerContactVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 29:
                    FieldMeasureVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 20:
                    FinalFabVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 28:
                    InstallForecastVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 31:
                    JobArchRepVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 30:
                    JobArchitectVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 21:
                    JobManufVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 17:
                    LeedVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 14:
                    OpenJobVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 32:
                    PastDueVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 22:
                    PmVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 12:
                    ReleasedVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 11:
                    ShipVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 8:
                    ShopRecvVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 7:
                    ShopReqVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 9:
                    SubmitVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 15:
                    VendorVisibility = true;
                    LoadActiveLaborData();
                    break;
                default:
                    break;
            }
        }

        private string _jobNo;

        public string JobNo
        {
            get { return _jobNo; }
            set
            {
                if (value == _jobNo) return;
                _jobNo = value;
                OnPropertyChanged();
            }
        }

        private string _selectedJobNo;

        public string SelectedJobNo
        {
            get { return _selectedJobNo; }
            set
            {
                if (value == _selectedJobNo) return;
                _selectedJobNo = value;
                OnPropertyChanged();
            }
        }

        private int _salesmanID;

        public int SalesmanID
        {
            get { return _salesmanID; }
            set
            {
                if (value == _salesmanID) return;
                _salesmanID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedSalesmanID;

        public int SelectedSalesmanID
        {
            get { return _selectedSalesmanID; }
            set
            {
                if (value == _selectedSalesmanID) return;
                _selectedSalesmanID = value;
                OnPropertyChanged();
            }
        }

        private int _archRepID;

        public int ArchRepID
        {
            get { return _archRepID; }
            set
            {
                if (value == _archRepID) return;
                _archRepID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedArchRepID;

        public int SelectedArchRepID
        {
            get { return _selectedArchRepID; }
            set
            {
                if (value == _selectedArchRepID) return;
                _selectedArchRepID = value;
                OnPropertyChanged();
            }
        }

        private int _customerID;

        public int CustomerID
        {
            get { return _customerID; }
            set
            {
                if (value == _customerID) return;
                _customerID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedCustomerID;

        public int SelectedCustomerID
        {
            get { return _selectedCustomerID; }
            set
            {
                if (value == _selectedCustomerID) return;
                _selectedCustomerID = value;
                OnPropertyChanged();
            }
        }

        private string _customerName;

        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                if (value == _customerName) return;
                _customerName = value;
                OnPropertyChanged();
            }
        }

        private int _architectID;

        public int ArchitectID
        {
            get { return _architectID; }
            set
            {
                if (value == _architectID) return;
                _architectID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedArchitectID;

        public int SelectedArchitectID
        {
            get { return _selectedArchitectID; }
            set
            {
                if (value == _selectedArchitectID) return;
                _selectedArchitectID = value;
                OnPropertyChanged();
            }
        }

        private int _manufID;

        public int ManufID
        {
            get { return _manufID; }
            set
            {
                if (value == _manufID) return;
                _manufID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedManufID;

        public int SelectedManufID
        {
            get { return _selectedManufID; }
            set
            {
                if (value == _selectedManufID) return;
                _selectedManufID = value;
                OnPropertyChanged();
            }
        }

        private int _materialID;

        public int MaterialID
        {
            get { return _materialID; }
            set
            {
                if (value == _materialID) return;
                _materialID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedMatID;

        public int SelectedMatID
        {
            get { return _selectedMatID; }
            set
            {
                if (value == _selectedMatID) return;
                _selectedMatID = value;
                OnPropertyChanged();
            }
        }

        private int _crewID;

        public int CrewID
        {
            get { return _crewID; }
            set
            {
                if (value == _crewID) return;
                _crewID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedCrewID;

        public int SelectedCrewID
        {
            get { return _selectedCrewID; }
            set
            {
                if (value == _selectedCrewID) return;
                _selectedCrewID = value;
                OnPropertyChanged();
            }
        }

        private DateTime _dateFrom;

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set
            {
                if (value == _dateFrom) return;
                _dateFrom = value;
                OnPropertyChanged();
            }
        }

        private DateTime _selectedDateFrom;

        public DateTime SelectedDateFrom
        {
            get { return _selectedDateFrom; }
            set
            {
                if (value == _selectedDateFrom) return;
                _selectedDateFrom = value;
                OnPropertyChanged();
            }
        }

        private DateTime _toDate;

        public DateTime ToDate
        {
            get { return _toDate; }
            set
            {
                if (value == _toDate) return;
                _toDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _selectedToDate;

        public DateTime SelectedToDate
        {
            get { return _selectedToDate; }
            set
            {
                if (value == _selectedToDate) return;
                _selectedToDate = value;
                OnPropertyChanged();
            }
        }

        private string _keyword;

        public string Keyword
        {
            get { return _keyword; }
            set
            {
                if (value == _keyword) return;
                _keyword = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ReportActiveLabor> _reportActiveLabors;

        public ObservableCollection<ReportActiveLabor> ReportActiveLabors
        {
            get { return _reportActiveLabors; }
            set
            {
                if (value == _reportActiveLabors) return;
                _reportActiveLabors = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TrackLaborReport> _trackLaborReports;

        public ObservableCollection<TrackLaborReport> TrackLaborReports
        {
            get { return _trackLaborReports; }
            set
            {
                if (value == _trackLaborReports) return;
                _trackLaborReports = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ReportApproveNotRelease> _reportApproveNotReleases;

        public ObservableCollection<ReportApproveNotRelease> ReportApproveNotReleases
        {
            get { return _reportApproveNotReleases; }
            set
            {
                if (value == _reportApproveNotReleases) return;
                _reportApproveNotReleases = value;
                OnPropertyChanged();
            }
        }

        private bool _activeLaborListVisibility;

        public bool ActiveLaborListVisibility
        {
            get { return _activeLaborListVisibility; }
            set
            {
                if (value == _activeLaborListVisibility) return;
                _activeLaborListVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _approvedVisibility;

        public bool ApprovedVisibility
        {
            get { return _approvedVisibility; }
            set
            {
                if (value == _approvedVisibility) return;
                _approvedVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _changeOrderVisibility;

        public bool ChangeOrderVisibility
        {
            get { return _changeOrderVisibility; }
            set
            {
                if (value == _changeOrderVisibility) return;
                _changeOrderVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _coNotRetVisibility;

        public bool CONotRetVisibility
        {
            get { return _coNotRetVisibility; }
            set
            {
                if (value == _coNotRetVisibility) return;
                _coNotRetVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _cipVisibility;

        public bool CipVisibility
        {
            get { return _cipVisibility; }
            set
            {
                if (value == _cipVisibility) return;
                _cipVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _contractVisibility;

        public bool ContractVisibility
        {
            get { return _contractVisibility; }
            set
            {
                if (value == _contractVisibility) return;
                _contractVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _contractNotRetVisibility;

        public bool ContractNotRetVisibility
        {
            get { return _contractNotRetVisibility; }
            set
            {
                if (value == _contractNotRetVisibility) return;
                _contractNotRetVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _customerContactVisibility;

        public bool CustomerContactVisibility
        {
            get { return _customerContactVisibility; }
            set
            {
                if (value == _customerContactVisibility) return;
                _customerContactVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _fieldMeasureVisibility;

        public bool FieldMeasureVisibility
        {
            get { return _fieldMeasureVisibility; }
            set
            {
                if (value == _fieldMeasureVisibility) return;
                _fieldMeasureVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _finalFabVisibility;

        public bool FinalFabVisibility
        {
            get { return _finalFabVisibility; }
            set
            {
                if (value == _finalFabVisibility) return;
                _finalFabVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _installForecastVisibility;

        public bool InstallForecastVisibility
        {
            get { return _installForecastVisibility; }
            set
            {
                if (value == _installForecastVisibility) return;
                _installForecastVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _jobArchRepVisibility;

        public bool JobArchRepVisibility
        {
            get { return _jobArchRepVisibility; }
            set
            {
                if (value == _jobArchRepVisibility) return;
                _jobArchRepVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _jobArchitectVisibility;

        public bool JobArchitectVisibility
        {
            get { return _jobArchitectVisibility; }
            set
            {
                if (value == _jobArchitectVisibility) return;
                _jobArchitectVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _jobManufVisibility;

        public bool JobManufVisibility
        {
            get { return _jobManufVisibility; }
            set
            {
                if (value == _jobManufVisibility) return;
                _jobManufVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _leedVisibility;

        public bool LeedVisibility
        {
            get { return _leedVisibility; }
            set
            {
                if (value == _leedVisibility) return;
                _leedVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _openJobVisibility;

        public bool OpenJobVisibility
        {
            get { return _openJobVisibility; }
            set
            {
                if (value == _openJobVisibility) return;
                _openJobVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _pastDueVisibility;

        public bool PastDueVisibility
        {
            get { return _pastDueVisibility; }
            set
            {
                if (value == _pastDueVisibility) return;
                _pastDueVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _pmVisibility;

        public bool PmVisibility
        {
            get { return _pmVisibility; }
            set
            {
                if (value == _pmVisibility) return;
                _pmVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _releasedVisibility;

        public bool ReleasedVisibility
        {
            get { return _releasedVisibility; }
            set
            {
                if (value == _releasedVisibility) return;
                _releasedVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _shipVisibility;

        public bool ShipVisibility
        {
            get { return _shipVisibility; }
            set
            {
                if (value == _shipVisibility) return;
                _shipVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _shopRecvVisibility;

        public bool ShopRecvVisibility
        {
            get { return _shopRecvVisibility; }
            set
            {
                if (value == _shopRecvVisibility) return;
                _shopRecvVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _shopReqVisibility;

        public bool ShopReqVisibility
        {
            get { return _shopReqVisibility; }
            set
            {
                if (value == _shopReqVisibility) return;
                _shopReqVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _submitVisibility;

        public bool SubmitVisibility
        {
            get { return _submitVisibility; }
            set
            {
                if (value == _submitVisibility) return;
                _submitVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _vendorVisibility;

        public bool VendorVisibility
        {
            get { return _vendorVisibility; }
            set
            {
                if (value == _vendorVisibility) return;
                _vendorVisibility = value;
                OnPropertyChanged();
            }
        }
    }
}
