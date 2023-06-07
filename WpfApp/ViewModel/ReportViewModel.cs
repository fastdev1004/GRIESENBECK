using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Model;
using WpfApp.Utils;
using WpfApp.Command;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.ViewModel
{
    class ReportViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataSet ds;
        public string sqlquery;
       
        public ReportViewModel()
        {
            dbConnection = new DatabaseConnection();
            dbConnection.Open();
            LoadReports();
            Complete = 2;
        }

        private void LoadReports()
        {
            // Reports
            sqlquery = "SELECT * FROM tblReportsList WHERE Active = 1 ORDER BY Report_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Report> st_report = new ObservableCollection<Report>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int reportID = int.Parse(row["Report_ID"].ToString());
                string reportName = row["Report_Name"].ToString();
                string reportObjectName = row["Report_ObjectName"].ToString();
                string reportDateName = row["Report_DateName"].ToString();
                string reportDateOnTbl = row["Report_DateOnTbl"].ToString();

                st_report.Add(new Report { ID = reportID, ReportName = reportName, ReportObjectName = reportObjectName, ReportDateName= reportDateName, ReportDateOnTbl = reportDateOnTbl});
            }
            Reports = st_report;

            // Project
            sqlquery = "SELECT * FROM tblProjects ORDER BY Project_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Project> st_project = new ObservableCollection<Project>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (!string.IsNullOrEmpty(row["Project_Name"].ToString()))
                {
                    int projectID = int.Parse(row["Project_ID"].ToString());
                    string projectName = row["Project_Name"].ToString();

                    st_project.Add(new Project { ID = projectID, ProjectName = projectName });
                }
            }
            Projects = st_project;

            // SalesMan
            sqlquery = "SELECT * FROM tblSalesmen ORDER BY Salesman_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Salesman> st_salesman = new ObservableCollection<Salesman>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int salesManID = int.Parse(row["Salesman_ID"].ToString());
                string salesManName = row["Salesman_Name"].ToString();

                st_salesman.Add(new Salesman { ID = salesManID, SalesmanName = salesManName });
            }
            SalesMans = st_salesman;

            // Arch Rep
            sqlquery = "SELECT * FROM tblArchRep ORDER BY Arch_Rep_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ArchRep> st_archReps = new ObservableCollection<ArchRep>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int archRepID = int.Parse(row["Arch_Rep_ID"].ToString());
                string archRepName = row["Arch_Rep_Name"].ToString();

                st_archReps.Add(new ArchRep { ID = archRepID, ArchRepName = archRepName });
            }
            ArchReps = st_archReps;

            // Customers
            sqlquery = "SELECT * FROM tblCustomers ORDER BY Full_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Customer> st_customer = new ObservableCollection<Customer>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int customerID = int.Parse(row["Customer_ID"].ToString());
                string fullName = row["Full_Name"].ToString();

                st_customer.Add(new Customer { ID = customerID, FullName = fullName });
            }
            Customers = st_customer;

            // Manufacturer
            sqlquery = "SELECT * FROM tblManufacturers ORDER BY Manuf_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Manufacturer> st_manuf = new ObservableCollection<Manufacturer>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int manufID = int.Parse(row["Manuf_ID"].ToString());
                string manufName = row["Manuf_Name"].ToString();

                st_manuf.Add(new Manufacturer { ID = manufID, ManufacturerName = manufName });
            }
            Manufacturers = st_manuf;

            // Architect
            sqlquery = "SELECT * FROM tblArchitects ORDER BY Arch_Company";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Architect> st_architect = new ObservableCollection<Architect>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int archID = int.Parse(row["Architect_ID"].ToString());
                string archCompany = row["Arch_Company"].ToString();

                st_architect.Add(new Architect { ID = archID, ArchCompany = archCompany});
            }
            Architects = st_architect;

            // Material
            sqlquery = "SELECT * FROM tblMaterials ORDER BY Material_Desc";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Material> st_material = new ObservableCollection<Material>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int materialID = int.Parse(row["Material_ID"].ToString());
                string materialDesc = row["Material_Desc"].ToString();

                st_material.Add(new Material { ID = materialID, MatDesc = materialDesc });
            }
            Materials = st_material;

            // Crew
            sqlquery = "SELECT Crew_ID, Crew_Name FROM tblInstallCrew;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Crew> st_crew = new ObservableCollection<Crew>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int Num;

                bool isNum = int.TryParse(row["Crew_ID"].ToString(), out Num); //c is your variable
                if (isNum)
                {
                    int crewID = int.Parse(row["Crew_ID"].ToString());
                    string crewName = row["Crew_Name"].ToString();
                    st_crew.Add(new Crew { ID = crewID, CrewName = crewName });
                }

            }
            Crews = st_crew;

            // Job No
            sqlquery = "SELECT Job_No FROM tblProjects ORDER BY Job_No";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<string> sb_jobNo = new ObservableCollection<string>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (!row.IsNull("Job_No"))
                    sb_jobNo.Add(row["Job_No"].ToString());
            }
            JobNos = sb_jobNo;

            // ReportSelection
            sqlquery = "SELECT * FROM tblReportsSelections";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportSelection> sb_reportSelection = new ObservableCollection<ReportSelection>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                sb_reportSelection.Add(new ReportSelection
                {
                    ReportID = int.Parse(row["Report_ID"].ToString()),
                    SelecionName = row["Selection_NameOnTbl"].ToString(),
                    SelectionDataType = row["Selection_DataType"].ToString()
                });
            }

            ReportSelections = sb_reportSelection;

            // Complte Combo Box
            ObservableCollection<int> st_completeCB = new ObservableCollection<int>();
            st_completeCB.Add(1);
            st_completeCB.Add(0);
            st_completeCB.Add(2);
            CompleteCB = st_completeCB;

            cmd.Dispose();
            dbConnection.Close();
        }

        private void ChangeReportItem()
        {
            List<ReportSelection> _reportSelections = ReportSelections.Where(item => item.ReportID == ReportID).ToList();

            ArchRepCbEnable = false;
            ArchitectCbEnable = false;
            CompleteCbEnable = false;
            CrewCbEnable = false;
            CustomerCbEnable = false;
            DateDpEnable = false;
            JobCbEnable = false;
            ManufIDCbEnable = false;
            ManufDescCbEnable = false;
            ProjectCbEnable = false;
            MaterialCbEnable = false;
            SalesmanCbEnable = false;

            foreach (ReportSelection item in _reportSelections)
            {
                switch(item.SelecionName)
                {
                    case "Arch_Rep_ID":
                        ArchRepCbEnable = true;
                        break;
                    case "Architect_ID":
                        ArchitectCbEnable = true;
                        break;
                    case "Complete":
                        CompleteCbEnable = true;
                        break;
                    case "Crew_ID":
                        CrewCbEnable = true;
                        break;
                    case "Customer_ID":
                        CustomerCbEnable = true;
                        break;
                    case "Date":
                        DateDpEnable = true;
                        break;
                    case "Job_No":
                        JobCbEnable = true;
                        break;
                    case "Manuf_ID":
                        ManufIDCbEnable = true;
                        break;
                    case "Manuf_Desc":
                        ManufDescCbEnable = true;
                        break;
                    case "Project_ID":
                        ProjectCbEnable = true;
                        break;
                    case "Material_Desc":
                        MaterialCbEnable = true;
                        break;
                    case "Salesman_ID":
                        SalesmanCbEnable = true;
                        break;
                }
            }
        }

        private ObservableCollection<Report> _reports;

        public ObservableCollection<Report> Reports
        {
            get { return _reports; }
            set
            {
                _reports = value;
            }
        }

        public ObservableCollection<int> CompleteCB { get; set; }

        private int _reportID;

        public int ReportID
        {
            get { return _reportID; }
            set
            {
                if (_reportID != value)
                {
                    _reportID = value;
                    ChangeReportItem();
                    OnPropertyChanged();
                }
            }
        }

        private bool _projectCbEnable;

        public bool ProjectCbEnable
        {
            get { return _projectCbEnable; }
            set
            {
                if (_projectCbEnable != value)
                {
                    _projectCbEnable = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _jobCbEnable;

        public bool JobCbEnable
        {
            get { return _jobCbEnable; }
            set
            {
                if (_jobCbEnable != value)
                {
                    _jobCbEnable = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _salesmanCbEnable;

        public bool SalesmanCbEnable
        {
            get { return _salesmanCbEnable; }
            set
            {
                if (_salesmanCbEnable != value)
                {
                    _salesmanCbEnable = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _archRepCbEnable;

        public bool ArchRepCbEnable
        {
            get { return _archRepCbEnable; }
            set
            {
                if (_archRepCbEnable != value)
                {
                    _archRepCbEnable = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _customerCbEnable;

        public bool CustomerCbEnable
        {
            get { return _customerCbEnable; }
            set
            {
                if (_customerCbEnable != value)
                {
                    _customerCbEnable = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _manufIDCbEnable;

        public bool ManufIDCbEnable
        {
            get { return _manufIDCbEnable; }
            set
            {
                if (_manufIDCbEnable != value)
                {
                    _manufIDCbEnable = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _manufDescCbEnable;

        public bool ManufDescCbEnable
        {
            get { return _manufDescCbEnable; }
            set
            {
                if (_manufDescCbEnable != value)
                {
                    _manufDescCbEnable = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _architectCbEnable;

        public bool ArchitectCbEnable
        {
            get { return _architectCbEnable; }
            set
            {
                if (_architectCbEnable != value)
                {
                    _architectCbEnable = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _materialCbEnable;

        public bool MaterialCbEnable
        {
            get { return _materialCbEnable; }
            set
            {
                if (_materialCbEnable != value)
                {
                    _materialCbEnable = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _crewCbEnable;

        public bool CrewCbEnable
        {
            get { return _crewCbEnable; }
            set
            {
                if (_crewCbEnable != value)
                {
                    _crewCbEnable = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _completeCbEnable;

        public bool CompleteCbEnable
        {
            get { return _completeCbEnable; }
            set
            {
                if (_completeCbEnable != value)
                {
                    _completeCbEnable = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _dateDpEnable;

        public bool DateDpEnable
        {
            get { return _dateDpEnable; }
            set
            {
                if (_dateDpEnable != value)
                {
                    _dateDpEnable = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _projectID;

        public int ProjectID
        {
            get { return _projectID; }
            set
            {
                if (_projectID != value)
                {
                    _projectID = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _jobNo;

        public string JobNo
        {
            get { return _jobNo; }
            set
            {
                if (_jobNo != value)
                {
                    _jobNo = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _salesmanID;

        public int SalesmanID
        {
            get { return _salesmanID; }
            set
            {
                if (_salesmanID != value)
                {
                    _salesmanID = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _archRepID;

        public int ArchRepID
        {
            get { return _archRepID; }
            set
            {
                if (_archRepID != value)
                {
                    _archRepID = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _customerID;

        public int CustomerID
        {
            get { return _customerID; }
            set
            {
                if (_customerID != value)
                {
                    _customerID = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _manufID;

        public int ManufID
        {
            get { return _manufID; }
            set
            {
                if (_manufID != value)
                {
                    _manufID = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _architectID;

        public int ArchitectID
        {
            get { return _architectID; }
            set
            {
                if (_architectID != value)
                {
                    _architectID = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _materialID;

        public int MaterialID
        {
            get { return _materialID; }
            set
            {
                if (_materialID != value)
                {
                    _materialID = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _crewID;

        public int CrewID
        {
            get { return _crewID; }
            set
            {
                if (_crewID != value)
                {
                    _crewID = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _dateFrom;

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set
            {
                if (_dateFrom != value)
                {
                    _dateFrom = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _toDate;

        public DateTime ToDate
        {
            get { return _toDate; }
            set
            {
                if (_toDate != value)
                {
                    _toDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _keyword;

        public string Keyword
        {
            get { return _keyword; }
            set
            {
                if (_keyword != value)
                {
                    _keyword = value;
                    OnPropertyChanged();
                }
            }
        }

        //here

        private ObservableCollection<Project> _projects;

        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set
            {
                _projects = value;
            }
        }

        private ObservableCollection<Salesman> _salesMans;

        public ObservableCollection<Salesman> SalesMans
        {
            get { return _salesMans; }
            set
            {
                _salesMans = value;
            }
        }

        private ObservableCollection<ArchRep> _archRep;

        public ObservableCollection<ArchRep> ArchReps
        {
            get { return _archRep; }
            set
            {
                _archRep = value;
            }
        }

        private ObservableCollection<Customer> _customers;

        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
            }
        }

        private ObservableCollection<Manufacturer> _manuf;

        public ObservableCollection<Manufacturer> Manufacturers
        {
            get { return _manuf; }
            set
            {
                _manuf = value;
            }
        }

        private ObservableCollection<Architect> _architects;

        public ObservableCollection<Architect> Architects
        {
            get { return _architects; }
            set
            {
                _architects = value;
            }
        }

        private ObservableCollection<Material> _materials;

        public ObservableCollection<Material> Materials
        {
            get { return _materials; }
            set
            {
                _materials = value;
            }
        }

        private ObservableCollection<Crew> _crew;

        public ObservableCollection<Crew> Crews
        {
            get { return _crew; }
            set
            {
                _crew = value;
            }
        }

        public ObservableCollection<string> JobNos
        {
            get;
            set;
        }

        public ObservableCollection<ReportSelection> ReportSelections;


        private int _complete;

        public int Complete
        {
            get { return _complete; }
            set
            {
                if (_complete != value)
                {
                    _complete = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
