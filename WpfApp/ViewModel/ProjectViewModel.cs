using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp.Model;

namespace WpfApp.ViewModel
{
    class ProjectViewModel : ViewModelBase
    {
        private Project project;
        private int _selectetProjectId;
        private int _selectetPMId;
        private ObservableCollection<Payment> _payments;
        public int SelectedProjectId
        {
            get { return _selectetProjectId; }
            set
            {
                _selectetProjectId = value;
                ChangeProject();
            }
        }

        public int SelectedPMId
        {
            get { return _selectetPMId; }
            set
            {
                _selectetPMId = value;
                //ChangePM();
            }
        }

        public ProjectViewModel()
        {
            project = new Project();
            LoadProjects();
        }

        public ObservableCollection<Project> Projects
        {
            get;
            set;
        }

        public ObservableCollection<Payment> Payments
        {
            get => _payments;
            set
            {
                if (value == _payments) return;
                _payments = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Customer> Customers
        {
            get;
            set;
        }

        public ObservableCollection<Architect> Architects
        {
            get;
            set;
        }

        public ObservableCollection<Crew> Crews
        {
            get;
            set;
        }

        public ObservableCollection<Salesman> Salesman
        {
            get;
            set;
        }

        public ObservableCollection<Estimator> Estimators
        {
            get;
            set;
        }

        public ObservableCollection<ArchRep> ArchReps
        {
            get;
            set;
        }

        public ObservableCollection<CustomerContact> CustomerContacts
        {
            get;
            set;
        }

        public ObservableCollection<PC> PCs
        {
            get;
            set;
        }

        public ObservableCollection<Superintendent> Superintendents
        {
            get;
            set;
        }

        public ObservableCollection<ProjectManager> ProjectManagers
        {
            get;
            set;
        }

        public string _projectName;
        public string _paymentNote;
        private DateTime _targetDate;
        private DateTime _completedDate;
        public string _jobNo;
        public Boolean _onHold;
        public string _address;
        public string _city;
        public string _state;
        public string _zip;
        public string _origTaxAmt;
        public string _origProfit;
        public string _origTotalMatCost;
        public string _billingDate;
        public int _selectedCustomerId;
        public int _selectedEstimatorID;
        public int _selectedProjectCoordID;
        public int _selectedArchRepID;
        public int _selectedCCID;
        public int _selectedArchitectID;
        public int _selectedCrewID;
        public int _selectedMCID;
        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (value == _projectName) return;
                _projectName = value;
                OnPropertyChanged();
            }
        }
        public string PaymentNote
        {
            get => _paymentNote;
            set
            {
                if (value == _paymentNote) return;
                _paymentNote = value;
                OnPropertyChanged();
            }
        }
        public DateTime TargetDate
        {
            get { return _targetDate; }
            set
            {
                if (_targetDate != value)
                {
                    _targetDate = value;
                    OnPropertyChanged(nameof(TargetDate));
                }
            }
        }
        public DateTime CompletedDate
        {
            get { return _completedDate; }
            set
            {
                if (_completedDate != value)
                {
                    _completedDate = value;
                    OnPropertyChanged(nameof(CompletedDate));
                }
            }
        }
        public string JobNo
        {
            get => _jobNo;
            set
            {
                if (value == _jobNo) return;
                _jobNo = value;
                OnPropertyChanged();
            }
        }
        public Boolean OnHold
        {
            get => _onHold;
            set
            {
                if (value == _onHold) return;
                _onHold = value;
                OnPropertyChanged();
            }
        }
        public String Address
        {
            get => _address;
            set
            {
                if (value == _address) return;
                _address = value;
                OnPropertyChanged();
            }
        }
        public String City
        {
            get => _city;
            set
            {
                if (value == _city) return;
                _city = value;
                OnPropertyChanged();
            }
        }
        public String State
        {
            get => _state;
            set
            {
                if (value == _state) return;
                _state = value;
                OnPropertyChanged();
            }
        }
        public string Zip
        {
            get => _zip;
            set
            {
                if (value == _zip) return;
                _zip = value;
                OnPropertyChanged();
            }
        }
        public string OrigTaxAmt
        {
            get => _origTaxAmt;
            set
            {
                if (value == _origTaxAmt) return;
                _origTaxAmt = value;
                OnPropertyChanged();
            }
        }
        public string OrigProfit
        {
            get => _origProfit;
            set
            {
                if (value == _origProfit) return;
                _origProfit = value;
                OnPropertyChanged();
            }
        }
        public string OrigTotalMatCost
        {
            get => _origTotalMatCost;
            set
            {
                if (value == _origTotalMatCost) return;
                _origTotalMatCost = value;
                OnPropertyChanged();
            }
        }
        public string BillingDate
        {
            get => _billingDate;
            set
            {
                if (value == _billingDate) return;
                _billingDate = value;
                OnPropertyChanged();
            }
        }
        public int SelectedCustomerId
        {
            get => _selectedCustomerId;
            set
            {
                if (value == _selectedCustomerId) return;
                _selectedCustomerId = value;
                OnPropertyChanged();
            }
        }
        public int SelectedEstimatorID
        {
            get => _selectedEstimatorID;
            set
            {
                if (value == _selectedEstimatorID) return;
                _selectedEstimatorID = value;
                OnPropertyChanged();
            }
        }
        public int SelectedProjectCoordID
        {
            get => _selectedProjectCoordID;
            set
            {
                if (value == _selectedProjectCoordID) return;
                _selectedProjectCoordID = value;
                OnPropertyChanged();
            }
        }
        public int SelectedArchRepID
        {
            get => _selectedArchRepID;
            set
            {
                if (value == _selectedArchRepID) return;
                _selectedArchRepID = value;
                OnPropertyChanged();
            }
        }
        public int SelectedCCID
        {
            get => _selectedCCID;
            set
            {
                if (value == _selectedCCID) return;
                _selectedCCID = value;
                OnPropertyChanged();
            }
        }
        public int SelectedArchitectID
        {
            get => _selectedArchitectID;
            set
            {
                if (value == _selectedArchitectID) return;
                _selectedArchitectID = value;
                OnPropertyChanged();
            }
        }
        public int SelectedCrewID
        {
            get => _selectedCrewID;
            set
            {
                if (value == _selectedCrewID) return;
                _selectedCrewID = value;
                OnPropertyChanged();
            }
        }
        public int SelectedMCID
        {
            get => _selectedMCID;
            set
            {
                if (value == _selectedMCID) return;
                _selectedMCID = value;
                OnPropertyChanged();
            }
        }

        private void LoadProjects()
        {
            string connectionString = @"Data Source = DESKTOP-VDIB57T\INSTANCE2023; user id=sa; password=qwe234ASD@#$; Initial Catalog = griesenbeck;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            string sqlquery = "SELECT tblProjects.Project_ID, tblProjects.Project_Name, tblCustomers.Full_Name FROM tblProjects LEFT JOIN tblCustomers ON tblProjects.Customer_ID = tblCustomers.Customer_ID ORDER BY Project_Name ASC;";
            SqlCommand cmd = new SqlCommand(sqlquery, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Project> st_mb = new ObservableCollection<Project>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (!string.IsNullOrEmpty(row["Project_Name"].ToString()))
                {
                    int projectID = int.Parse(row["Project_ID"].ToString());
                    string projectName = row["Project_Name"].ToString();

                    string fullName = row["Full_Name"].ToString();
                    st_mb.Add(new Project { ID = projectID, ProjectName = projectName, CustomerName = fullName });
                }

            }
            Projects = st_mb;
            // Payment
            ObservableCollection<Payment> _paymentItems = new ObservableCollection<Payment>();
            Payments = new ObservableCollection<Payment>();
            string[] paymentItems = { "Background Check", "Cert Pay Reqd", "CIP Project", "C3 Project", "P&P Bond", "GAP Bid Incl", "GAP Est Incl", "LCP Tracker", "Down Payment" };

            foreach (string item in paymentItems)
            {
                _paymentItems.Add(new Payment { Name = item, IsChecked = false });
            }
            Payments = _paymentItems;

            // Customer Address
            sqlquery = "SELECT Customer_ID, Full_Name, Address FROM tblCustomers Order by Full_Name";
            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Customer> st_customer = new ObservableCollection<Customer>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var m_test = row["Customer_ID"];
                int Num;

                bool isNum = int.TryParse(row["Customer_ID"].ToString(), out Num); //c is your variable
                if (isNum)
                {
                    int customerID = int.Parse(row["Customer_ID"].ToString());
                    //int customerID = 0;
                    string customerName = row["Full_Name"].ToString();
                    string customeAddress = row["Address"].ToString();
                    st_customer.Add(new Customer { ID = customerID, CustomerName = customerName, CustomerAddress = customeAddress });
                }
                
            }
            Customers = st_customer;

            // Architect
            sqlquery = "SELECT Architect_ID, Arch_Company FROM tblArchitects;";
            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Architect> st_architect = new ObservableCollection<Architect>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int Num;

                bool isNum = int.TryParse(row["Architect_ID"].ToString(), out Num); //c is your variable
                if (isNum)
                {
                    int architectID = int.Parse(row["Architect_ID"].ToString());
                    string archCompany = row["Arch_Company"].ToString();
                    st_architect.Add(new Architect { ID = architectID, ArchCompany = archCompany });
                }

            }
            Architects = st_architect;

            //Crew
            sqlquery = "SELECT Crew_ID, Crew_Name FROM tblInstallCrew;";
            cmd = new SqlCommand(sqlquery, con);
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

            // Salesman
            sqlquery = "SELECT Salesman_ID, Salesman_Name FROM tblSalesmen;";
            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Salesman> st_salesman = new ObservableCollection<Salesman>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int Num;

                bool isNum = int.TryParse(row["Salesman_ID"].ToString(), out Num); //c is your variable
                if (isNum)
                {
                    int salesmanID = int.Parse(row["Salesman_ID"].ToString());
                    string salesmanName = row["Salesman_Name"].ToString();
                    st_salesman.Add(new Salesman { ID = salesmanID, SalesmanName = salesmanName });
                }

            }
            Salesman = st_salesman;

            // Estimator
            sqlquery = "SELECT Estimator_ID, Estimator_Name FROM tblEstimators;";
            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Estimator> st_estimator = new ObservableCollection<Estimator>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int Num;

                bool isNum = int.TryParse(row["Estimator_ID"].ToString(), out Num); //c is your variable
                if (isNum)
                {
                    int estimatorID = int.Parse(row["Estimator_ID"].ToString());
                    string estimatorName = row["Estimator_Name"].ToString();
                    st_estimator.Add(new Estimator { ID = estimatorID, EstimatorName = estimatorName });
                }

            }
            Estimators = st_estimator;

            // ArchRep
            sqlquery = "SELECT Arch_Rep_ID, Arch_Rep_Name FROM tblArchRep;";
            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ArchRep> st_archRep = new ObservableCollection<ArchRep>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int Num;

                bool isNum = int.TryParse(row["Arch_Rep_ID"].ToString(), out Num); //c is your variable
                if (isNum)
                {
                    int archRepID = int.Parse(row["Arch_Rep_ID"].ToString());
                    string archRepName = row["Arch_Rep_Name"].ToString();
                    st_archRep.Add(new ArchRep { ID = archRepID, ArchRepName = archRepName });
                }

            }
            ArchReps = st_archRep;

            // Customer Contacts
            sqlquery = "SELECT CC_ID, Customer_ID, CC_Name FROM tblCustomerContacts;";
            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<CustomerContact> st_customerContact = new ObservableCollection<CustomerContact>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int ccID = int.Parse(row["CC_ID"].ToString());
                int customerID = int.Parse(row["Customer_ID"].ToString());
                string ccName= row["CC_Name"].ToString();
                st_customerContact.Add(new CustomerContact { ID = ccID, CustomerID = customerID, CCName = ccName});

            }
            CustomerContacts = st_customerContact;

            // PC
            sqlquery = "SELECT PC_ID, PC_Name FROM tblPCs;";
            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<PC> st_pc = new ObservableCollection<PC>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int pcID = int.Parse(row["PC_ID"].ToString());
                string pcName = row["PC_Name"].ToString();
                st_pc.Add(new PC { ID = pcID, PCName = pcName });

            }

            PCs = st_pc;

            // Superintendent
            sqlquery = "SELECT * FROM tblSuperintendents";
            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Superintendent> st_supt = new ObservableCollection<Superintendent>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int suptID = int.Parse(row["Sup_ID"].ToString());
                string suptName = row["Sup_Name"].ToString();
                string suptCellPhone = row["Sup_CellPhone"].ToString();
                string suptEmail = row["Sup_Email"].ToString();
                st_supt.Add(new Superintendent { SupID = suptID, SupName = suptName, SupCellPhone = suptCellPhone, SupEmail = suptEmail});
            }

            Superintendents = st_supt;
            // ProjectManager
            //sqlquery = "select * from tblProjectManagers";
            //cmd = new SqlCommand(sqlquery, con);
            //sda = new SqlDataAdapter(cmd);
            //ds = new DataSet();
            //sda.Fill(ds);
            //ObservableCollection<ProjectManager> st_pm = new ObservableCollection<ProjectManager>();
            //foreach (DataRow row in ds.Tables[0].Rows)
            //{
            //    int pmID = int.Parse(row["PM_ID"].ToString());
            //    string pmName = row["PM_Name"].ToString();
            //    string pmCellPhone = row["PM_CellPhone"].ToString();
            //    string pmEmail = row["PM_Email"].ToString();
            //    st_pm.Add(new ProjectManager
            //    {
            //        ID = pmID,
            //        PMName = pmName,
            //        PMCellPhone = pmCellPhone,
            //        PMEmail = pmEmail
            //    });
            //}

            //ProjectManagers = st_pm;


            cmd.Dispose();
            con.Close();
        }

        private void ChangeProject()
        {
            string connectionString = @"Data Source = DESKTOP-VDIB57T\INSTANCE2023; user id=sa; password=qwe234ASD@#$; Initial Catalog = griesenbeck;";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            string sqlquery = "SELECT tblProjects.*, tblCustomers.Full_Name, tblCustomers.Customer_ID FROM tblProjects LEFT JOIN tblCustomers ON tblProjects.Customer_ID = tblCustomers.Customer_ID WHERE tblProjects.Project_ID = " + SelectedProjectId.ToString() + ";";
            SqlCommand cmd = new SqlCommand(sqlquery, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            var row = ds.Tables[0].Rows[0];
            ProjectName = "";
            TargetDate = new DateTime();
            CompletedDate = new DateTime();
            PaymentNote = "";
            JobNo = "";
            Address = "";
            City = "";
            State = "";
            Zip = "";
            OrigTaxAmt = "";
            OrigProfit = "";
            OrigTotalMatCost = "";
            OnHold = false;
            BillingDate = "";
            SelectedEstimatorID = -1;
            SelectedProjectCoordID = -1;
            SelectedArchRepID = -1;
            SelectedArchitectID = -1;
            SelectedCCID = -1;
            SelectedCrewID = -1;
            SelectedMCID = -1;
            if (!row.IsNull("Project_Name"))
                ProjectName = row["Project_Name"].ToString();
            if (!row.IsNull("Target_Date"))
                TargetDate = row.Field<DateTime>("Target_Date");
            if (!row.IsNull("Date_Completed"))
                CompletedDate = row.Field<DateTime>("Date_Completed");
            if (!row.IsNull("Pay_Reqd_Note"))
                PaymentNote = row["Pay_Reqd_Note"].ToString();
            if (!row.IsNull("Job_No"))
                JobNo = row["Job_No"].ToString();
            if (!row.IsNull("Address"))
                Address = row["Address"].ToString();
            if (!row.IsNull("City"))
                City = row["City"].ToString();
            if (!row.IsNull("State"))
                State = row["State"].ToString();
            if (!row.IsNull("Zip"))
                Zip = row["Zip"].ToString();
            if (!row.IsNull("OrigTaxAmt"))
                OrigTaxAmt = row["OrigTaxAmt"].ToString();
            if (!row.IsNull("OrigProfit"))
                OrigProfit = row["OrigProfit"].ToString();
            if (!row.IsNull("OrigTotalMatCost"))
                OrigTotalMatCost = row["OrigTotalMatCost"].ToString();
            if (!row.IsNull("On_Hold"))
                OnHold = row.Field<Boolean>("On_Hold");
            if (!row.IsNull("Billing_Date"))
                BillingDate = row["Billing_Date"].ToString();
            if (!row.IsNull("Estimator_ID"))
                SelectedEstimatorID = row.Field<int>("Estimator_ID");
            if (!row.IsNull("PC_ID"))
                SelectedProjectCoordID = row.Field<int>("PC_ID");
            if (!row.IsNull("Arch_Rep_ID"))
                SelectedArchRepID = row.Field<int>("Arch_Rep_ID");
            if (!row.IsNull("CC_ID"))
                SelectedCCID = row.Field<int>("CC_ID");
            if (!row.IsNull("Architect_ID"))
                SelectedArchitectID = row.Field<int>("Architect_ID");
            if (!row.IsNull("Crew_ID"))
                SelectedCrewID = row.Field<int>("Crew_ID");
            if (!row.IsNull("Master_Contract"))
                if (row.Field<string>("Master_Contract") == "Yes")
                    SelectedMCID = 0;
                else SelectedMCID = 1;
            if (!row.IsNull("Customer_ID"))
                SelectedCustomerId = row.Field<int>("Customer_ID");

            string[] paymentItems = { "Background Check", "Cert Pay Reqd", "CIP Project", "C3 Project", "P&P Bond", "GAP Bid Incl", "GAP Est Incl", "LCP Tracker", "Down Payment" };

            //PaymentItem
            DataTemplate myDataTemplate = new DataTemplate();
            FrameworkElementFactory checkBoxFactory = new FrameworkElementFactory(typeof(CheckBox));
            //checkBoxFactory.SetBinding(CheckBox.IsCheckedProperty, new Bind("));

            ObservableCollection<Payment> _paymentItems = new ObservableCollection<Payment>();
            foreach (string item in paymentItems)
            {
                switch(item)
                {
                    case "Background Check":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(row["BackGroundCheck"].ToString()) });
                        break;
                    case "Cert Pay Reqd":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(row["CertPay_Reqd"].ToString()) });
                        break;
                    case "CIP Project":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(row["CIP_Project"].ToString()) });
                        break;
                    case "C3 Project":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(row["C3"].ToString()) });
                        break;
                    case "P&P Bond":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(row["PnP_Bond"].ToString()) });
                        break;
                    case "GAP Bid Incl":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(row["GapBid_Incl"].ToString()) });
                        break;
                    case "GAP Est Incl":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(row["GapEst_Incl"].ToString()) });
                        break;
                    case "LCP Tracker":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(row["LCPTracker"].ToString()) });
                        break;
                    case "Down Payment":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(row["Pay_Reqd"].ToString()) });
                        break;
                }
            }
            Payments = _paymentItems;

            cmd.Dispose();
            con.Close();
        }

        
    }

    public class PaymentItem
    {
        public bool IsChecked { get; set; }
        public string Text { get; set; }
    }

}
