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
            dbConnection.Open();
            //LoadData();
        }

        public void LoadData()
        {
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

            if (!string.IsNullOrEmpty(Keyword))
            {
                whereClause += $" AND Project_Name = '{Keyword}'";
            }

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
                OnPropertyChanged();
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
    }
}
