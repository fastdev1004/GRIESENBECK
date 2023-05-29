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

namespace WpfApp.ViewModel
{
    class ReportViewModel
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
        }

        public void LoadReports()
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

            cmd.Dispose();
            dbConnection.Close();
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

        private ObservableCollection<Crew> _keyword;

        public ObservableCollection<Crew> Keywords
        {
            get { return _keyword; }
            set
            {
                _keyword = value;
            }
        }
    }
}
