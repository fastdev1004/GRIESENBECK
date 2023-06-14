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
using WpfApp.Utils;
using WpfApp.Command;

namespace WpfApp.ViewModel
{
    class ProjectViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataSet ds;
        private string sqlquery;

        public ProjectViewModel()
        {
            dbConnection = new DatabaseConnection();
            dbConnection.Open();
            LoadProjects();
            ProjectManagerList = new ObservableCollection<ProjectManager>();
            SuperintendentList = new ObservableCollection<Superintendent>();
            ProjectLinks = new ObservableCollection<ProjectLink>();
            SovCOs = new ObservableCollection<SovCO>();
            SovMaterials = new ObservableCollection<SovMaterial>();
            ProjectMatTrackings = new ObservableCollection<ProjectMatTracking>();
            ProjectMtShips = new ObservableCollection<ProjectMatShip>();
            ProjectSelectionEnable = true;
            Note noteItem = new Note();
            ProjectNotes = new ObservableCollection<Note>();
            ProjectNotes.Add(noteItem);

            WorkOrderNotes = new ObservableCollection<Note>();
            WorkOrderNotes.Add(noteItem);


            this.NewProjectCommand = new RelayCommand((e)=> this.ClearProject());
        }

        private void ClearProject()
        {
            ProjectID = -1;
            ProjectName = "";
            CustomerID = -1;
            Proj_TargetDate = new DateTime();
            Proj_CompletedDate = new DateTime();
            Payments.Clear();
            PaymentNote = "";
            AdditionalInfoNote = "";
            BillingDate = "";
            MasterContractID = "";
            SelectedCrewID = -1;
            JobNo = "";
            OnHold = false;
            Address = "";
            City = "";
            State = "";
            Zip = "";
            OrigTaxAmt = "";
            OrigProfit = "";
            OrigTotalMatCost = "";
            ProjectManagerList.Clear();
            SuperintendentList.Clear();
            EstimatorID = -1;
            ArchitectID = -1;
            ProjectCoordID = -1;
            ArchRepID = -1;
            CCID = -1;
            ArchitectID = -1;
            ProjectNotes.Clear();
            ProjectLinks.Clear();
            Proj_Complete = false;
            SafetyBadging = "";
            InstallationNotes.Clear();
            ProjectCIPs.Clear();
            Contracts.Clear();
            ChangeOrders.Clear();

            //ProjectSelectionEnable = false;
            // SOV
            SovCOs.Clear();
            SovMaterials.Clear();
            // Track/Ship/Recv
            TrackShipRecvs.Clear();
            ProjectMatTrackings.Clear();
            ProjectMtShips.Clear();
            // Tracking
            TrackReports.Clear();
            TrackLaborReports.Clear();
            // Wok Orders
            ProjectWorkOrders.Clear();
            WorkOrders.Clear();

            ObservableCollection<Payment> _paymentItems = new ObservableCollection<Payment>();
            Payments = new ObservableCollection<Payment>();
            string[] paymentItems = { "Background Check", "Cert Pay Reqd", "CIP Project", "C3 Project", "P&P Bond", "GAP Bid Incl", "GAP Est Incl", "LCP Tracker", "Down Payment" };

            foreach (string item in paymentItems)
            {
                _paymentItems.Add(new Payment { Name = item, IsChecked = false });
            }
            Payments = _paymentItems;
        }

        private void LoadProjects()
        {
            sqlquery = "SELECT tblProjects.Project_ID, tblProjects.Project_Name, tblCustomers.Full_Name FROM tblProjects LEFT JOIN tblCustomers ON tblProjects.Customer_ID = tblCustomers.Customer_ID ORDER BY Project_Name ASC;";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
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
            sqlquery = "SELECT Customer_ID, Full_Name, Address FROM tblCustomers Order by Short_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
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
                    string fullName = row["Full_Name"].ToString();
                    string address = row["Address"].ToString();
                    st_customer.Add(new Customer { ID = customerID, FullName = fullName, Address = address });
                }
                
            }
            Customers = st_customer;

            // Architect
            sqlquery = "SELECT Architect_ID, Arch_Company FROM tblArchitects;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
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

            // Salesman
            sqlquery = "SELECT Salesman_ID, Salesman_Name FROM tblSalesmen;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
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
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
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
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
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
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
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
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
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
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
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
                st_supt.Add(new Superintendent { SupID = suptID, SupName = suptName, SupCellPhone = suptCellPhone, SupEmail = suptEmail });
            }

            Superintendents = st_supt;

            // ReturnedViaNames
            sqlquery = "SELECT * FROM tblReturnedVia";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReturnedVia> st_returnedVia = new ObservableCollection<ReturnedVia>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int viaID = int.Parse(row["ID"].ToString());
                string viaName = row["ReturnedVia"].ToString();
                st_returnedVia.Add(new ReturnedVia { ID = viaID, ReturnedViaName = viaName });
            }

            ReturnedViaNames = st_returnedVia;

            // ProjectManager
            sqlquery = "select * from tblProjectManagers";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            ObservableCollection<ProjectManager> st_pm = new ObservableCollection<ProjectManager>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int pmID = int.Parse(row["PM_ID"].ToString());
                string pmName = row["PM_Name"].ToString();
                string pmCellPhone = row["PM_CellPhone"].ToString();
                string pmEmail = row["PM_Email"].ToString();
                st_pm.Add(new ProjectManager
                {
                    ID = pmID,
                    PMName = pmName,
                    PMCellPhone = pmCellPhone,
                    PMEmail = pmEmail
                });
            }

            ProjectManagers = st_pm;

            // Manufacturer
            sqlquery = "SELECT Manuf_ID, Manuf_Name FROM tblManufacturers;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            ObservableCollection<Manufacturer> st_manufacturers = new ObservableCollection<Manufacturer>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int manufID = int.Parse(row["Manuf_ID"].ToString());
                string manufName = row["Manuf_Name"].ToString();
                st_manufacturers.Add(new Manufacturer
                {
                    ID = manufID,
                    ManufacturerName = manufName,
                });
            }

            Manufacturers = st_manufacturers;

            // FreightCo_Name
            ObservableCollection<FreightCo> sb_freightCo = new ObservableCollection<FreightCo>();

            sqlquery = "SELECT FreightCo_ID, FreightCo_Name FROM tblFreightCo ORDER BY FreightCo_Name;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int freightID = int.Parse(row["FreightCo_ID"].ToString());
                string freightName = row["FreightCo_Name"].ToString();
                sb_freightCo.Add(new FreightCo
                {
                    ID = freightID,
                    FreightName = freightName,
                });
            }

            FreightCos = sb_freightCo;

            // Materials
            ObservableCollection<Material> st_material = new ObservableCollection<Material>();

            sqlquery = "Select * from tblMaterials";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int matID = int.Parse(row["Material_ID"].ToString());
                string matDesc = row["Material_Desc"].ToString();
                st_material.Add(new Material
                {
                    ID = matID,
                    MatDesc = matDesc,
                });
            }

            Materials = st_material;

            // Acronym
            ObservableCollection<Acronym> st_acronym = new ObservableCollection<Acronym>();

            sqlquery = "SELECT * from tblScheduleOfValues";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string acronymName = "";
                string acronymDesc = "";
                if(!row.IsNull("SOV_Acronym"))
                    acronymName = row["SOV_Acronym"].ToString();
                if (!row.IsNull("SOV_Desc"))
                    acronymDesc = row["SOV_Desc"].ToString();

                st_acronym.Add(new Acronym
                {
                    AcronymName = acronymName,
                    AcronymDesc = acronymDesc
                });
            }
            Acronyms = st_acronym;
            
            // CIP Type
            ObservableCollection<string> st_cip = new ObservableCollection<string>();

            sqlquery = "SELECT DISTINCT CIPType FROM tblCIPs";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string cipType = "";
                if(!row.IsNull("CIPType"))
                {
                    cipType = row["CIPType"].ToString();
                    st_cip.Add(cipType);
                }
            }

            CIPTypes = st_cip;

            // CIP Enroll
            ObservableCollection<string> st_crewEnroll = new ObservableCollection<string>();

            sqlquery = "SELECT DISTINCT CrewEnrolled FROM tblCIPs ";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string cipType = "";
                if (!row.IsNull("CrewEnrolled"))
                {
                    cipType = row["CrewEnrolled"].ToString();
                    st_crewEnroll.Add(cipType);
                }
            }

            CrewEnrolls = st_crewEnroll;

            // ApprDen Type
            ObservableCollection<string> st_apprDen = new ObservableCollection<string>();

            sqlquery = "SELECT DISTINCT CO_AppDEn FROM tblProjectChangeOrders";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string apprDen = "";
                if (!row.IsNull("CO_AppDEn"))
                {
                    apprDen = row["CO_AppDEn"].ToString();
                    st_apprDen.Add(apprDen);
                }
            }

            ApprDens = st_apprDen;

            ObservableCollection<string> st_masterContracts = new ObservableCollection<string>();
            st_masterContracts.Add("Yes");
            st_masterContracts.Add("No");
            MasterContracts = st_masterContracts;

            cmd.Dispose();
            dbConnection.Close();
        }

        private void ChangeProject()
        {
            string sqlquery = "SELECT tblProjects.*, tblCustomers.Full_Name, tblCustomers.Customer_ID FROM tblProjects LEFT JOIN tblCustomers ON tblProjects.Customer_ID = tblCustomers.Customer_ID WHERE tblProjects.Project_ID = " + ProjectID.ToString() + ";";
            SqlCommand cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            DataRow firstRow = ds.Tables[0].Rows[0];
            ProjectName = "";
            Proj_TargetDate = new DateTime();
            Proj_CompletedDate = new DateTime();
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
            EstimatorID = -1;
            ProjectCoordID = -1;
            ArchRepID = -1;
            ArchitectID = -1;
            CCID = -1;
            SelectedCrewID = -1;
            MasterContractID = "";
            AdditionalInfoNote = "";
            Proj_Complete = false;
            if (!firstRow.IsNull("Project_Name"))
                ProjectName = firstRow["Project_Name"].ToString();
            if (!firstRow.IsNull("Target_Date"))
                Proj_TargetDate = firstRow.Field<DateTime>("Target_Date");
            if (!firstRow.IsNull("Date_Completed"))
                Proj_CompletedDate = firstRow.Field<DateTime>("Date_Completed");
            if (!firstRow.IsNull("Pay_Reqd_Note"))
                PaymentNote = firstRow["Pay_Reqd_Note"].ToString();
            if (!firstRow.IsNull("Job_No"))
                JobNo = firstRow["Job_No"].ToString();
            if (!firstRow.IsNull("Address"))
                Address = firstRow["Address"].ToString();
            if (!firstRow.IsNull("City"))
                City = firstRow["City"].ToString();
            if (!firstRow.IsNull("State"))
                State = firstRow["State"].ToString();
            if (!firstRow.IsNull("Zip"))
                Zip = firstRow["Zip"].ToString();
            if (!firstRow.IsNull("OrigTaxAmt"))
                OrigTaxAmt = firstRow["OrigTaxAmt"].ToString();
            if (!firstRow.IsNull("OrigProfit"))
                OrigProfit = firstRow["OrigProfit"].ToString();
            if (!firstRow.IsNull("OrigTotalMatCost"))
                OrigTotalMatCost = firstRow["OrigTotalMatCost"].ToString();
            if (!firstRow.IsNull("On_Hold"))
                OnHold = firstRow.Field<Boolean>("On_Hold");
            if (!firstRow.IsNull("Billing_Date"))
                BillingDate = firstRow["Billing_Date"].ToString();
            if (!firstRow.IsNull("Estimator_ID"))
                EstimatorID = firstRow.Field<int>("Estimator_ID");
            if (!firstRow.IsNull("PC_ID"))
                ProjectCoordID = firstRow.Field<int>("PC_ID");
            if (!firstRow.IsNull("Arch_Rep_ID"))
                ArchRepID = firstRow.Field<int>("Arch_Rep_ID");
            if (!firstRow.IsNull("CC_ID"))
                CCID = firstRow.Field<int>("CC_ID");
            if (!firstRow.IsNull("Architect_ID"))
                ArchitectID = firstRow.Field<int>("Architect_ID");
            if (!firstRow.IsNull("Crew_ID"))
                SelectedCrewID = firstRow.Field<int>("Crew_ID");
            if (!firstRow.IsNull("Master_Contract"))
                MasterContractID = firstRow["Master_Contract"].ToString();
            if (!firstRow.IsNull("Customer_ID"))
                CustomerID = firstRow.Field<int>("Customer_ID");
            if (!firstRow.IsNull("Addtl_Info"))
                AdditionalInfoNote = firstRow["Addtl_Info"].ToString();
            if (!firstRow.IsNull("Safety_Badging"))
                SafetyBadging = firstRow["Safety_Badging"].ToString();
            if(!firstRow.IsNull("Complete"))
                Proj_Complete = firstRow.Field<Boolean>("Complete");

            string[] paymentItems = { "Background Check", "Cert Pay Reqd", "CIP Project", "C3 Project", "P&P Bond", "GAP Bid Incl", "GAP Est Incl", "LCP Tracker", "Down Payment" };

            //PaymentItem
            DataTemplate myDataTemplate = new DataTemplate();
            FrameworkElementFactory checkBoxFactory = new FrameworkElementFactory(typeof(CheckBox));

            ObservableCollection<Payment> _paymentItems = new ObservableCollection<Payment>();
            foreach (string item in paymentItems)
            {
                switch(item)
                {
                    case "Background Check":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(firstRow["BackGroundCheck"].ToString()) });
                        break;
                    case "Cert Pay Reqd":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(firstRow["CertPay_Reqd"].ToString()) });
                        break;
                    case "CIP Project":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(firstRow["CIP_Project"].ToString()) });
                        break;
                    case "C3 Project":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(firstRow["C3"].ToString()) });
                        break;
                    case "P&P Bond":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(firstRow["PnP_Bond"].ToString()) });
                        break;
                    case "GAP Bid Incl":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(firstRow["GapBid_Incl"].ToString()) });
                        break;
                    case "GAP Est Incl":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(firstRow["GapEst_Incl"].ToString()) });
                        break;
                    case "LCP Tracker":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(firstRow["LCPTracker"].ToString()) });
                        break;
                    case "Down Payment":
                        _paymentItems.Add(new Payment { Name = item, IsChecked = bool.Parse(firstRow["Pay_Reqd"].ToString()) });
                        break;
                }
            }
            Payments = _paymentItems;

            // TrackShipRecv
            ObservableCollection<TrackShipRecv> st_TrackShipRecv = new ObservableCollection<TrackShipRecv>();

            sqlquery = "Select tblMat.*, tblMaterials.Material_Desc from(Select tblSOV.SOV_Acronym, tblSOV.CO_ItemNo, tblSOV.Material_Only, tblSOV.SOV_Desc, tblProjectMaterials.ProjMat_ID, tblProjectMaterials.Mat_Phase, tblProjectMaterials.Mat_Type,tblProjectMaterials.Color_Selected, tblProjectMaterials.Qty_Reqd, tblProjectMaterials.TotalCost, Material_ID from(Select tblSOV.*, tblProjectChangeOrders.CO_ItemNo from(Select tblSOV.*, tblScheduleOfValues.SOV_Desc from tblScheduleOfValues Right JOIN(SELECT tblProjectSOV.* From tblProjects LEFT Join tblProjectSOV ON tblProjects.Project_ID = tblProjectSOV.Project_ID where tblProjects.Project_ID = " + ProjectID.ToString() + ") AS tblSOV ON tblSOV.SOV_Acronym = tblScheduleOfValues.SOV_Acronym Where tblScheduleOfValues.Active = 'true') AS tblSOV LEFT JOIN tblProjectChangeOrders ON tblProjectChangeOrders.CO_ID = tblSOV.CO_ID) AS tblSOV LEFT JOIN tblProjectMaterials ON tblSOV.ProjSOV_ID = tblProjectMaterials.ProjSOV_ID) AS tblMat LEFT JOIN tblMaterials ON tblMat.Material_ID = tblMaterials.Material_ID ORDER BY tblMaterials.Material_Desc;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string sovAcronym = row["SOV_Acronym"].ToString();
                string coItemNo = row["CO_ItemNo"].ToString();
                string materialDesc = row["Material_Desc"].ToString();
                string materialOnly = row["Material_Only"].ToString();
                string sovDesc = row["SOV_Desc"].ToString();
                string matPhase = row["Mat_Phase"].ToString();
                string matType = row["Mat_Type"].ToString();
                string colorSelected = row["Color_Selected"].ToString();
                string qtyReqd = "";
                if (!row.IsNull("Qty_Reqd"))
                    qtyReqd = row["Qty_Reqd"].ToString();
                string totalCost = row["TotalCost"].ToString();
                int projmatID = 0;
                if (!row.IsNull("ProjMat_ID"))
                    projmatID = int.Parse(row["ProjMat_ID"].ToString());

                st_TrackShipRecv.Add(new TrackShipRecv
                {
                    MaterialName = materialDesc,
                    SovName = sovAcronym,
                    ChangeOrder = coItemNo,
                    Phase = matPhase,
                    Type = matType,
                    Color = colorSelected,
                    QtyReqd = qtyReqd,
                    ProjMatID = projmatID
                });
            }
            TrackShipRecvs = st_TrackShipRecv;

            // Project WorkOrders List
            sqlquery = " SELECT tblWorkOrdersMat.Mat_Qty, tblMat.* FROM tblWorkOrdersMat RIGHT JOIN ( SELECT tblProjectSOV.SOV_Acronym, tblMat.* FROM tblProjectSOV RIGHT JOIN ( SELECT tblMat.*, tblProjectMaterialsShip.Qty_Recvd, tblProjectMaterialsShip.ProjMS_ID FROM tblProjectMaterialsShip RIGHT JOIN ( SELECT tblManufacturers.Manuf_Name, tblMat.* FROM tblManufacturers RIGHT JOIN (  SELECT tblMaterials.Material_Desc,tblMat.* FROM tblMaterials RIGHT JOIN (SELECT tblProjectMaterialsTrack.MatReqdDate, tblProjectMaterialsTrack.TakeFromStock, tblMat.*, tblProjectMaterialsTrack.ProjMT_ID, tblProjectMaterialsTrack.Manuf_ID, tblProjectMaterialsTrack.Qty_Ord  FROM tblProjectMaterialsTrack RIGHT JOIN (SELECT Project_ID, ProjMat_ID, ProjSOV_ID ,Material_ID, Qty_Reqd FROM tblProjectMaterials WHERE Project_ID = " + ProjectID.ToString() + ") AS tblMat ON tblProjectMaterialsTrack.ProjMat_ID = tblMat.ProjMat_ID) AS tblMat ON tblMaterials.Material_ID = tblMat.Material_ID) AS tblMat ON tblMat.Manuf_ID = tblManufacturers.Manuf_ID) AS tblMat ON tblMat.ProjMT_ID = tblProjectMaterialsShip.ProjMT_ID) AS tblMat ON tblMat.ProjSOV_ID = tblProjectSOV.ProjSOV_ID) AS tblMat ON tblWorkOrdersMat.ProjMS_ID = tblMat.ProjMS_ID ORDER BY Material_Desc";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ProjectWorkOrder> sb_projectWorkOrder = new ObservableCollection<ProjectWorkOrder>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                string _sovAcronym = "";
                string _matName = "";
                string _manufName = "";
                bool _stock = false;
                DateTime _matlReqd = new DateTime();
                int _qtyReqd = 0;
                int _qtyOrd = 0;
                int _qtyRecvd = 0;
                int _matQty = 0;

                if (!row.IsNull("Project_ID"))
                    _projectID = row.Field<int>("Project_ID");
                if (!row.IsNull("SOV_Acronym"))
                    _sovAcronym = row["SOV_Acronym"].ToString();
                if (!row.IsNull("Material_Desc"))
                    _matName = row["Material_Desc"].ToString(); ;
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("TakeFromStock"))
                    _stock = row.Field<Boolean>("TakeFromStock");
                if (!row.IsNull("MatReqdDate"))
                    _matlReqd = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Qty_Reqd"))
                    _qtyReqd = int.Parse(row["Qty_Reqd"].ToString());
                if (!row.IsNull("Qty_Ord"))
                    _qtyOrd = int.Parse(row["Qty_Ord"].ToString());
                if (!row.IsNull("Qty_Recvd"))
                    _qtyRecvd = int.Parse(row["Qty_Recvd"].ToString());
                if (!row.IsNull("Mat_Qty"))
                    _matQty = int.Parse(row["Mat_Qty"].ToString());

                sb_projectWorkOrder.Add(new ProjectWorkOrder
                {
                    ProjectID = _projectID,
                    SovAcronym = _sovAcronym,
                    MatName = _matName,
                    ManufName = _manufName,
                    Stock = _stock,
                    MatlReqd = _matlReqd,
                    QtyReqd = _qtyReqd,
                    QtyOrd = _qtyOrd,
                    QtyRecvd = _qtyRecvd,
                    MatQty = _matQty
                });
            }
            ProjectWorkOrders = sb_projectWorkOrder;

            // Project Labor List
            sqlquery = "SELECT CO_ItemNo, tblLab.* FROM tblProjectChangeOrders RIGHT JOIN(SELECT tblProjectSOV.SOV_Acronym, tblProjectSOV.CO_ID, tblLab.Labor_Desc, tblLab.Qty_Reqd, tblLab.UnitPrice, tblLab.Lab_Phase, tblLab.Complete FROM tblProjectSOV RIGHT JOIN (SELECT tblLabor.Labor_Desc, tblLab.* FROM tblLabor RIGHT JOIN (SELECT* FROM tblProjectLabor WHERE Project_ID = " + ProjectID.ToString() + ") AS tblLab ON tblLabor.Labor_ID = tblLab.Labor_ID) AS tblLab ON tblProjectSOV.ProjSOV_ID = tblLab.ProjSOV_ID) AS tblLab ON tblProjectChangeOrders.CO_ID = tblLab.CO_ID ORDER BY tblLab.SOV_Acronym Desc";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ProjectLabor> sb_projectLabor = new ObservableCollection<ProjectLabor>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string _sovAcronym = "";
                string _labor = "";
                double _qtyReqd = 0;
                double _unitPrice = 0;
                bool _complete = false;
                //int    = 0;
                int _changeOrder = 0;
                string _phase = "";
                if (!row.IsNull("SOV_Acronym"))
                    _sovAcronym = row["SOV_Acronym"].ToString();
                if (!row.IsNull("Labor_Desc"))
                    _labor = row["Labor_Desc"].ToString();
                if (!row.IsNull("Qty_Reqd"))
                    _qtyReqd = double.Parse(row["Qty_Reqd"].ToString());
                if (!row.IsNull("UnitPrice"))
                    _unitPrice = row.Field<double>("UnitPrice");
                if (!row.IsNull("CO_ItemNo"))
                    _changeOrder = int.Parse(row["CO_ItemNo"].ToString());
                if (!row.IsNull("Lab_Phase"))
                    _phase = row["Lab_Phase"].ToString();
                if (!row.IsNull("Complete"))
                    _complete = row.Field<Boolean>("Complete");

                sb_projectLabor.Add(new ProjectLabor
                {
                    ProjectID = ProjectID,
                    SovAcronym = _sovAcronym,
                    Labor = _labor,
                    QtyReqd = _qtyReqd,
                    UnitPrice = _unitPrice,
                    ChangeOrder = _changeOrder,
                    Phase = _phase,
                    Complete = _complete
                });
            }
            ProjectLabors = sb_projectLabor;

            // work orders
            sqlquery = "SELECT tblWO.WO_ID, tblWO.Wo_Nbr, tblInstallCrew.Crew_ID, tblInstallCrew.Crew_Name, tblWO.SchedStartDate, tblWO.SchedComplDate, tblWO.Sup_ID, tblWO.Date_Started, tblWO.Date_Completed FROM tblInstallCrew RIGHT JOIN (SELECT * FROM tblWorkOrders WHERE Project_ID = " + ProjectID.ToString() + ") AS tblWO ON tblInstallCrew.Crew_ID = tblWO.Crew_ID; ";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            int rowCount = 0;
            rowCount = ds.Tables[0].Rows.Count; // number of rows

            ObservableCollection<WorkOrder> sb_workOrders = new ObservableCollection<WorkOrder>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int woID = 0;
                int woNbr = 0;
                int crewID = 0;
                int suptID = -1;
                string crewName = "";
                DateTime schedStartDate = new DateTime();
                DateTime schedCompDate = new DateTime();
                DateTime dateStarted = new DateTime();
                DateTime dateCompleted = new DateTime();

                woID = row.Field<int>("WO_ID");
                if (!row.IsNull("Wo_Nbr"))
                    woNbr = int.Parse(row["Wo_Nbr"].ToString());
                if (!row.IsNull("Crew_ID"))
                    crewID = row.Field<int>("Crew_ID");
                if (!row.IsNull("Crew_Name"))
                    crewName = row["Crew_Name"].ToString();
                if (!row.IsNull("SchedStartDate"))
                    schedStartDate = row.Field<DateTime>("SchedStartDate");
                if (!row.IsNull("SchedComplDate"))
                    schedCompDate = row.Field<DateTime>("SchedComplDate");
                if (!row.IsNull("Sup_ID"))
                    suptID = int.Parse(row["Sup_ID"].ToString());
                if (!row.IsNull("Date_Started"))
                    dateStarted = row.Field<DateTime>("Date_Started");
                if (!row.IsNull("Date_Completed"))
                    dateCompleted = row.Field<DateTime>("Date_Completed");
                sb_workOrders.Add(new WorkOrder
                {
                    WoID = woID,
                    WoNumber = woNbr,
                    CrewID = crewID,
                    CrewName = crewName,
                    SchedStartDate = schedStartDate,
                    SchedComplDate = schedCompDate,
                    SuptID = suptID,
                    DateStarted = dateStarted,
                    DateCompleted = dateCompleted
                });
            }

            WorkOrders = sb_workOrders;

            // Tracking Report1
            sqlquery = "Select MatReqdDate, tblManufacturers.Manuf_Name, Qty_Ord, tblProjMat.Mat_Phase, tblProjMat.Mat_Type, Manuf_LeadTime, PO_Number, ShopReqDate, ShopRecvdDate, SubmitIssue, Resubmit_Date, SubmitAppr,tblProjMat.Color_Selected, Guar_Dim, Field_Dim, ReleasedForFab, LaborComplete from tblManufacturers RIGHT JOIN(Select tblProjectMaterialsTrack.*, tblMat.Color_Selected, tblMat.Mat_Phase, tblMat.Mat_Type from tblProjectMaterialsTrack INNER JOIN(SELECT * FROM tblProjectMaterials WHERE tblProjectMaterials.Project_ID = " + ProjectID.ToString() + ") AS tblMat ON tblMat.ProjMat_ID = tblProjectMaterialsTrack.ProjMat_ID) AS tblProjMat ON tblManufacturers.Manuf_ID = tblProjMat.Manuf_ID ORDER BY MatReqdDate;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<TrackReport> sb_trackReports = new ObservableCollection<TrackReport>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DateTime matReqdDate = new DateTime();

                string manufName = "";
                string qtyOrd = "";
                string phase = "";
                string type = "";
                string leadTime = "";
                string gapPO = "";
                DateTime shopReq = new DateTime();
                DateTime shopRecv = new DateTime();
                DateTime submIssue = new DateTime();
                DateTime reSubmit = new DateTime();
                DateTime submAppr = new DateTime();
                string color = "";
                bool guarDim = false;
                DateTime fieldDim = new DateTime();
                DateTime rff = new DateTime();
                bool laborComplete = false;

                if (!row.IsNull("MatReqdDate"))
                    matReqdDate = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Manuf_Name"))
                    manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("Qty_Ord"))
                    qtyOrd = row["Qty_Ord"].ToString();
                if (!row.IsNull("Mat_Phase"))
                    phase = row["Mat_Phase"].ToString();
                if (!row.IsNull("Mat_Type"))
                    type = row["Mat_Type"].ToString();
                if (!row.IsNull("Manuf_LeadTime"))
                    leadTime = row["Manuf_LeadTime"].ToString();
                if (!row.IsNull("PO_Number"))
                    gapPO = row["PO_Number"].ToString();
                if (!row.IsNull("ShopReqDate"))
                    shopReq = row.Field<DateTime>("ShopReqDate");
                if (!row.IsNull("ShopRecvdDate"))
                    shopRecv = row.Field<DateTime>("ShopRecvdDate");
                if (!row.IsNull("SubmitIssue"))
                    submIssue = row.Field<DateTime>("SubmitIssue");
                if (!row.IsNull("Resubmit_Date"))
                    reSubmit = row.Field<DateTime>("Resubmit_Date");
                if (!row.IsNull("SubmitAppr"))
                    submAppr = row.Field<DateTime>("SubmitAppr");
                if (!row.IsNull("Color_Selected"))
                    color = row["Color_Selected"].ToString();
                if (!row.IsNull("Guar_Dim"))
                    guarDim = row.Field<Boolean>("Guar_Dim");
                if (!row.IsNull("Field_Dim"))
                    fieldDim = row.Field<DateTime>("Field_Dim");
                if (!row.IsNull("ReleasedForFab"))
                    rff = row.Field<DateTime>("ReleasedForFab");
                if (!row.IsNull("LaborComplete"))
                    laborComplete = row.Field<Boolean>("LaborComplete");

                sb_trackReports.Add(new TrackReport
                {
                    //ProjMat = int.Parse(selectedId),
                    MatReqdDate = matReqdDate,
                    ManufacturerName = manufName,
                    QtyOrd = qtyOrd,
                    Phase = phase,
                    Type = type,
                    ManufLeadTime = leadTime,
                    PoNumber = gapPO,
                    ShopReqDate = shopReq,
                    ShopRecvdDate = shopRecv,
                    SubmIssue = submIssue,
                    ReSubmit = reSubmit,
                    SubmAppr = submAppr,
                    Color = color,
                    GuarDim = guarDim,
                    FieldDim = fieldDim,
                    RFF = rff,
                    LaborComplete = laborComplete
                });

                TrackReports = sb_trackReports;
            }

            // Track Labor Report
            sqlquery = "SELECT tblLab.SOV_Acronym, tblLab.Labor_Desc, tblProjectChangeOrders.CO_ItemNo, tblLab.Lab_Phase, tblLab.Complete FROM tblProjectChangeOrders RIGHT JOIN (SELECT tblSOV.SOV_Acronym, tblLabor.Labor_Desc, tblSOV.CO_ID, tblSOV.Lab_Phase, tblSOV.Complete FROM tblLabor RIGHT JOIN (SELECT  tblProjectLabor.Labor_ID, tblProjectLabor.Lab_Phase, tblSOV.ProjSOV_ID, tblSOV.SOV_Acronym, tblSOV.CO_ID, tblProjectLabor.Complete FROM tblProjectLabor RIGHT JOIN (SELECT * FROM tblProjectSOV WHERE Project_ID = " + ProjectID.ToString() + ") AS tblSOV ON tblProjectLabor.ProjSOV_ID = tblSOV.ProjSOV_ID) AS tblSOV ON tblSOV.Labor_ID = tblLabor.Labor_ID) AS tblLab ON tblLab.CO_ID = tblProjectChangeOrders.CO_ID; ";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<TrackLaborReport> sb_trackLaborReports = new ObservableCollection<TrackLaborReport>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string sovAcronym = "";
                string laborDesc = "";
                string coItemNo = "";
                string labPhase = "";
                bool laborComplete = false;


                if (!row.IsNull("SOV_Acronym"))
                    sovAcronym = row["SOV_Acronym"].ToString();
                if (!row.IsNull("Labor_Desc"))
                    laborDesc = row["Labor_Desc"].ToString();
                if (!row.IsNull("CO_ItemNo"))
                    coItemNo = row["CO_ItemNo"].ToString();
                if (!row.IsNull("Lab_Phase"))
                    labPhase = row["Lab_Phase"].ToString();
                if (!row.IsNull("Complete"))
                    laborComplete = row.Field<Boolean>("Complete");

                sb_trackLaborReports.Add(new TrackLaborReport
                {
                    SovAcronym = sovAcronym,
                    LaborDesc = laborDesc,
                    CoItemNo = coItemNo,
                    LabPhase = labPhase,
                    Complete = laborComplete
                });
            }

            TrackLaborReports = sb_trackLaborReports;

            // SOV Grid 1
            sqlquery = "Select tblSOV.*, tblProjectChangeOrders.CO_ItemNo from (Select tblSOV.*, tblScheduleOfValues.SOV_Desc from tblScheduleOfValues Right JOIN (SELECT tblProjectSOV.* From tblProjects LEFT Join tblProjectSOV ON tblProjects.Project_ID = tblProjectSOV.Project_ID where tblProjects.Project_ID = " + ProjectID.ToString() + ") AS tblSOV ON tblSOV.SOV_Acronym = tblScheduleOfValues.SOV_Acronym Where tblScheduleOfValues.Active = 'true') AS tblSOV LEFT JOIN tblProjectChangeOrders ON tblProjectChangeOrders.CO_ID = tblSOV.CO_ID ORDER BY tblSOV.SOV_Acronym;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<SovCO> sb_sovCO= new ObservableCollection<SovCO>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string sovAcronym = "";
                int coItemNo = -1;
                string sovDesc = "";
                bool matOnly = false;


                if (!row.IsNull("SOV_Acronym"))
                    sovAcronym = row["SOV_Acronym"].ToString();
                if (!row.IsNull("CO_ItemNo"))
                    coItemNo = int.Parse(row["CO_ItemNo"].ToString());
                if (!row.IsNull("SOV_Desc"))
                    sovDesc = row["SOV_Desc"].ToString();
                if (!row.IsNull("Material_Only"))
                    matOnly = row.Field<Boolean>("Material_Only");

                sb_sovCO.Add(new SovCO
                {
                    SovAcronym = sovAcronym,
                    CoItemNo = coItemNo,
                    SovDesc = sovDesc,
                    MatOnly = matOnly
                });
            }

            SovCOs = sb_sovCO;

            // Project Change Orders
            sqlquery = "select * from tblProjectChangeOrders where Project_ID = " + ProjectID.ToString();
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ChangeOrder> sb_changeOrder = new ObservableCollection<ChangeOrder>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                int _coID = 0;
                int _coItemNo = 0;
                DateTime _coDate = new DateTime();
                string _coAppDen = "";
                DateTime _coDateAppDen = new DateTime();
                string _coComment = "";

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("CO_ID"))
                    _coID = int.Parse(row["CO_ID"].ToString());
                if (!row.IsNull("CO_ItemNo"))
                    _coItemNo = int.Parse(row["CO_ItemNo"].ToString());
                if (!row.IsNull("CO_Date"))
                    _coDate = row.Field<DateTime>("CO_Date");
                if (!row.IsNull("CO_DateAppDen"))
                    _coDateAppDen = row.Field<DateTime>("CO_DateAppDen");
                if (!row.IsNull("CO_AppDen"))
                    _coAppDen = row["CO_AppDen"].ToString();
                if (!row.IsNull("CO_Comments"))
                    _coComment = row["CO_Comments"].ToString();

                sb_changeOrder.Add(new ChangeOrder
                {
                    CoItemNo = _coItemNo,
                    ProjectID = _projectID,
                    CoID = _coID,
                    CoDate = _coDate,
                    CoDateAppDen = _coDateAppDen,
                    CoAppDen = _coAppDen,
                    CoComment = _coComment
                });
            }

            ChangeOrders = sb_changeOrder;

            // SOVGrid2
            sqlquery = "Select tblSOV.SOV_Acronym, tblSOV.CO_ItemNo, tblSOV.Material_Only, tblSOV.SOV_Desc, tblProjectMaterials.Mat_Phase, tblProjectMaterials.Mat_Type, tblProjectMaterials.Color_Selected, tblProjectMaterials.Qty_Reqd, tblProjectMaterials.TotalCost from(Select tblSOV.*, tblProjectChangeOrders.CO_ItemNo from(Select tblSOV.*, tblScheduleOfValues.SOV_Desc from tblScheduleOfValues Right JOIN(SELECT tblProjectSOV.* From tblProjects LEFT Join tblProjectSOV ON tblProjects.Project_ID = tblProjectSOV.Project_ID where tblProjects.Project_ID = " + ProjectID.ToString() + " ) AS tblSOV ON tblSOV.SOV_Acronym = tblScheduleOfValues.SOV_Acronym Where tblScheduleOfValues.Active = 'true') AS tblSOV LEFT JOIN tblProjectChangeOrders ON tblProjectChangeOrders.CO_ID = tblSOV.CO_ID) AS tblSOV LEFT JOIN tblProjectMaterials ON tblSOV.ProjSOV_ID = tblProjectMaterials.ProjSOV_ID ORDER BY tblSOV.SOV_Acronym DESC";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ///SovMaterials
            ObservableCollection<SovMaterial> sb_sovMaterial = new ObservableCollection<SovMaterial>();
            foreach(DataRow row in ds.Tables[0].Rows)
            {
                string _acronymName = "";
                string _acronymDesc = "";
                int _coItemNo =0;
                bool _matOnly = false;
                string _matPhase = "";
                string _matType = "";
                string _color = "";
                int _qtyReqd = 0;
                double _totalCost = 0;

                if (!row.IsNull("SOV_Acronym"))
                    _acronymName = row["SOV_Acronym"].ToString();
                if (!row.IsNull("SOV_Desc"))
                    _acronymDesc = row["SOV_DESC"].ToString();
                if (!row.IsNull("CO_ItemNo"))
                    _coItemNo = int.Parse(row["CO_ItemNo"].ToString());
                if (!row.IsNull("Material_Only"))
                    _matOnly = row.Field<Boolean>("Material_Only");
                if (!row.IsNull("Mat_Phase"))
                    _matPhase = row["Mat_Phase"].ToString();
                if (!row.IsNull("Mat_Type"))
                    _matType = row["Mat_Type"].ToString();
                if (!row.IsNull("Color_Selected"))
                    _color = row["Color_Selected"].ToString();
                if (!row.IsNull("Qty_Reqd"))
                    _qtyReqd = int.Parse(row["Qty_Reqd"].ToString());
                if (!row.IsNull("TotalCost"))
                    _totalCost = double.Parse(row["TotalCost"].ToString());
                sb_sovMaterial.Add(new SovMaterial
                {
                    AcronymName = _acronymName,
                    AcronymDesc = _acronymDesc,
                    CoItemNo = _coItemNo,
                    MatOnly = _matOnly,
                    MatPhase = _matPhase,
                    MatType = _matType,
                    Color = _color,
                    QtyReqd = _qtyReqd,
                    TotalCost = _totalCost
                });
            }

            SovMaterials = sb_sovMaterial;

            // Labor
            sqlquery = "SELECT * FROM tblLabor ORDER BY Labor_Desc";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Labor> st_labor = new ObservableCollection<Labor>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int laborID = int.Parse(row["Labor_ID"].ToString());
                string laborDesc = row["Labor_Desc"].ToString();
                //double unitPrice = row.Field<float>("Labor_UnitPrice");
                double unitPrice = 0.0;
                bool active = row.Field<Boolean>("Active");

                st_labor.Add(new Labor { ID = laborID, LaborDesc = laborDesc, UnitPrice = unitPrice, Active = active });
            }
            Labors = st_labor;

            // Installation Notes
            sqlquery = "SELECT * FROM tblInstallNotes WHERE Project_ID = " + ProjectID.ToString();

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            rowCount = ds.Tables[0].Rows.Count; // number of rows

            ObservableCollection<InstallationNote> sb_installationNote = new ObservableCollection<InstallationNote>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int id = row.Field<int>("InstallNotes_ID");
                int projectID = row.Field<int>("Project_ID");
                string installNote = row["Install_Note"].ToString();
                DateTime installDateAdded = row.Field<DateTime>("InstallNotes_DateAdded");

                sb_installationNote.Add(new InstallationNote
                {
                    ID = id,
                    ProjectID = projectID,
                    InstallNote = installNote,
                    InstallDateAdded = installDateAdded
                });
            }

            InstallationNotes = sb_installationNote;

            // Contracts
            sqlquery = "SELECT tblProj.Job_No, Contractnumber, ChangeOrder, ChangeOrderNo, DateRecD, DateProcessed, AmtOfcontract, SignedoffbySales, Signedoffbyoperations, GivenAcctingforreview, Givenforfinalsignature, Scope, ReturnedVia, ReturnedtoDawn, Comments FROM tblSC RIGHT JOIN (SELECT Project_ID, Job_No FROM tblProjects WHERE Project_ID = " + ProjectID.ToString() + ") AS tblProj ON tblSC.ProjectID = tblProj.Project_ID";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Contract> sb_contract = new ObservableCollection<Contract>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string _jobNo = "";
                string _contractNumber = "";
                bool _changeOrder = false;
                string _changeOrderNo = "";
                DateTime _dateRecd = new DateTime();
                int _amtOfContract = 0;
                DateTime _dateProcessed = new DateTime();
                DateTime _signedoffbySales = new DateTime();
                DateTime _signedoffbyoperations = new DateTime();
                DateTime _givenAcctingforreview = new DateTime();
                DateTime _givenforfinalsignature = new DateTime();
                string _scope = "";
                string _returnedVia = "";
                DateTime _returnedDate = new DateTime();
                string _comment = "";

                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Contractnumber"))
                    _contractNumber = row["Contractnumber"].ToString();
                if (!row.IsNull("ChangeOrder"))
                    _changeOrder = row.Field<Boolean>("ChangeOrder"); ;
                if (!row.IsNull("ChangeOrderNo"))
                    _changeOrderNo = row["ChangeOrderNo"].ToString();
                if (!row.IsNull("DateRecD"))
                    _dateRecd = row.Field<DateTime>("DateRecD");
                if (!row.IsNull("DateProcessed"))
                    _dateProcessed = row.Field<DateTime>("DateProcessed");
                if (!row.IsNull("AmtOfcontract"))
                    _amtOfContract = int.Parse(row["AmtOfcontract"].ToString());
                if (!row.IsNull("SignedoffbySales"))
                    _signedoffbySales = row.Field<DateTime>("SignedoffbySales");
                if (!row.IsNull("Signedoffbyoperations"))
                    _signedoffbyoperations = row.Field<DateTime>("Signedoffbyoperations");
                if (!row.IsNull("GivenAcctingforreview"))
                    _givenAcctingforreview = row.Field<DateTime>("GivenAcctingforreview");
                if (!row.IsNull("Givenforfinalsignature"))
                    _givenforfinalsignature = row.Field<DateTime>("Givenforfinalsignature");
                if (!row.IsNull("Scope"))
                    _scope = row["Scope"].ToString();
                if (!row.IsNull("ReturnedVia"))
                    _returnedVia = row["ReturnedVia"].ToString();
                if (!row.IsNull("ReturnedtoDawn"))
                    _returnedDate = row.Field<DateTime>("ReturnedtoDawn");
                if (!row.IsNull("Comments"))
                    _comment = row["Comments"].ToString();

                sb_contract.Add(new Contract
                {
                    JobNo = _jobNo,
                    ContractNumber = _contractNumber,
                    ChangeOrder = _changeOrder,
                    ChangeOrderNo = _changeOrderNo,
                    DateRecd = _dateRecd,
                    AmtOfContract = _amtOfContract,
                    DateProcessed = _dateProcessed,
                    Signedoffbyoperations = _signedoffbyoperations,
                    SignedoffbySales = _signedoffbySales,
                    GivenAcctingforreview = _givenAcctingforreview,
                    Givenforfinalsignature = _givenforfinalsignature,
                    Scope = _scope,
                    ReturnedDate = _returnedDate,
                    ReturnedVia = _returnedVia,
                    Comment = _comment
                });
            }
            Contracts = sb_contract;

            // Project Manager List
            sqlquery = "select * from tblProjectManagers RIGHT JOIN (Select PM_ID from tblProjectPMs where Project_ID = "+ ProjectID.ToString() +") AS tblProject ON tblProjectManagers.PM_ID = tblProject.PM_ID";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ProjectManager> sb_pm = new ObservableCollection<ProjectManager>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int pmID = int.Parse(row["PM_ID"].ToString());
                string pmName = row["PM_Name"].ToString();
                string pmCellPhone = row["PM_CellPhone"].ToString();
                string pmEmail = row["PM_Email"].ToString();
                sb_pm.Add(new ProjectManager
                {
                    ID = pmID,
                    PMName = pmName,
                    PMCellPhone = pmCellPhone,
                    PMEmail = pmEmail
                });
            }

            ProjectManagerList = sb_pm;

            // Superintendent List
            sqlquery = "SELECT * FROM tblSuperintendents RIGHT JOIN (SELECT * FROM tblProjectSups WHERE Project_ID = " + ProjectID.ToString() + ") AS tblProject ON tblSuperintendents.Sup_ID = tblProject.Sup_ID";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Superintendent> sb_supt = new ObservableCollection<Superintendent>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _supID = 0;
                string _supName = "";
                string _supPhone = "";
                string _cellPhone = "";
                string _email = "";
                bool _active = false;

                if (!row.IsNull("Sup_ID"))
                    _supID = int.Parse(row["Sup_ID"].ToString());
                if (!row.IsNull("Sup_Name"))
                    _supName = row["Sup_Name"].ToString();
                if (!row.IsNull("Sup_Phone"))
                    _supPhone = row["Sup_Phone"].ToString();
                if (!row.IsNull("Sup_CellPhone"))
                    _cellPhone = row["Sup_CellPhone"].ToString();
                if (!row.IsNull("Sup_Email"))
                    _email = row["Sup_Email"].ToString();
                if (!row.IsNull("Active"))
                    _active = row.Field<Boolean>("Active");

                sb_supt.Add(new Superintendent
                {
                   SupID = _supID,
                   SupName = _supName,
                   SupPhone = _supPhone,
                   SupCellPhone = _cellPhone,
                   SupEmail = _email,
                   Active = _active
                });
            }
            SuperintendentList = sb_supt;

            // ProjectNote Grid
            sqlquery = "SELECT * FROM tblNotes WHERE Notes_PK_Desc = 'Project' AND Notes_PK =" + ProjectID.ToString();
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Note> sb_projectnotes = new ObservableCollection<Note>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _notesID = 0;
                int _notesPK = 0;
                string _notesPKDesc = "";
                string _notesNote = "";
                DateTime _notesDateAdded = new DateTime();
                string _notesUser = "";
                string _notesUserName = "";

                if (!row.IsNull("Notes_ID"))
                    _notesID = int.Parse(row["Notes_ID"].ToString());
                if (!row.IsNull("Notes_PK"))
                    _notesPK = int.Parse(row["Notes_PK"].ToString());
                if (!row.IsNull("Notes_PK_Desc"))
                    _notesPKDesc = row["Notes_PK_Desc"].ToString();
                if (!row.IsNull("Notes_Note"))
                    _notesNote = row["Notes_Note"].ToString();
                if (!row.IsNull("Notes_DateAdded"))
                    _notesDateAdded = row.Field<DateTime>("Notes_DateAdded");
                if (!row.IsNull("Notes_User"))
                    _notesUser = row["Notes_User"].ToString();
                if (!row.IsNull("Notes_UserName"))
                    _notesUserName = row["Notes_UserName"].ToString();
                sb_projectnotes.Add(new Note
                {
                    NoteID = _notesID,
                    NotePK = _notesPK,
                    NotesPKDesc = _notesPKDesc,
                    NotesNote = _notesNote,
                    NotesDateAdded = _notesDateAdded,
                    NoteUser = _notesUser,
                    NoteUserName = _notesUserName
                });
            }
            Note noteItem = new Note();
            sb_projectnotes.Add(noteItem);
            ProjectNotes = sb_projectnotes;

            // CIPGrid
            sqlquery = "SELECT Job_No, CIPType, TargetDate, OriginalContractAmt, FinalContractAmt, FormsRecD, FormsSent, CertRecD, ExemptionApproved, ExemptionAppDate, CrewEnrolled, Notes FROM tblCIPs RIGHT JOIN (SELECT Project_ID, Job_No FROM tblProjects WHERE Project_ID = " + ProjectID .ToString() + ") AS tblProjs ON tblCIPs.Project_ID = tblProjs.Project_ID";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<CIP> sb_cips = new ObservableCollection<CIP>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string _jobNo = "";
                string _cipType = "";
                DateTime _targetDate = new DateTime();
                double _originalContractAmt = 0;
                double _finalContractAmt = 0;
                DateTime _formsRecD = new DateTime();
                DateTime _formsSent = new DateTime();
                DateTime _certRecD = new DateTime();
                bool _exemptionApproved = false;
                DateTime _exemptionAppDate = new DateTime();
                string _crewEnrolled = "";
                string _notes = "";

                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("CIPType"))
                    _cipType = row["CIPType"].ToString();
                if (!row.IsNull("TargetDate"))
                    _targetDate = row.Field<DateTime>("TargetDate");
                if (!row.IsNull("OriginalContractAmt"))
                    _originalContractAmt = double.Parse(row["OriginalContractAmt"].ToString());
                if (!row.IsNull("FinalContractAmt"))
                    _finalContractAmt = double.Parse(row["FinalContractAmt"].ToString());
                if (!row.IsNull("FormsRecD"))
                    _formsRecD = row.Field<DateTime>("FormsRecD");
                if (!row.IsNull("FormsSent"))
                    _formsSent = row.Field<DateTime>("FormsSent");
                if (!row.IsNull("CertRecD"))
                    _certRecD = row.Field<DateTime>("CertRecD");
                if (!row.IsNull("ExemptionApproved"))
                    _exemptionApproved = row.Field<Boolean>("ExemptionApproved");
                if (!row.IsNull("ExemptionAppDate"))
                    _exemptionAppDate = row.Field<DateTime>("ExemptionAppDate");
                if (!row.IsNull("CrewEnrolled"))
                    _crewEnrolled = row["CrewEnrolled"].ToString();
                if (!row.IsNull("Notes"))
                    _notes = row["Notes"].ToString();

                sb_cips.Add(new CIP
                {
                    JobNo = _jobNo,
                    CipType = _cipType,
                    TargetDate = _targetDate,
                    OriginalContractAmt = _originalContractAmt,
                    FinalContractAmt = _finalContractAmt,
                    FormsRecD = _formsRecD,
                    FormsSent = _formsSent,
                    CertRecD = _certRecD,
                    ExemptionApproved = _exemptionApproved,
                    ExemptionAppDate = _exemptionAppDate,
                    CrewEnrolled = _crewEnrolled,
                    Notes = _notes
                });
            }

            ProjectCIPs = sb_cips;

            // Project Link
            sqlquery = "SELECT * FROM tblProjectLinks WHERE Project_ID = " + ProjectID.ToString();

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ProjectLink> sb_projectLinks = new ObservableCollection<ProjectLink>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _linkID = 0;
                int _projectID = 0;
                string _pathDesc = "";
                string _pathName = "";

                if (!row.IsNull("Link_ID"))
                    _linkID = int.Parse(row["Link_ID"].ToString());
                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("PathDesc"))
                    _pathDesc = row["PathDesc"].ToString();
                if (!row.IsNull("PathName"))
                    _pathName = row["PathName"].ToString();

                sb_projectLinks.Add(new ProjectLink
                {
                    LinkID = _linkID,
                    ProjectID = _projectID,
                    PathDesc = _pathDesc,
                    PathName = _pathName
                });
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ProjectLinks = sb_projectLinks;
            }

            // Path Desc
            sqlquery = "SELECT DISTINCT PathDesc FROM tblProjectLinks ORDER BY PathDesc";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<string> sb_pathDesc= new ObservableCollection<string>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                sb_pathDesc.Add(row["PathDesc"].ToString());
            }

            PathDescritpions = sb_pathDesc;

            cmd.Dispose();
            dbConnection.Close();
        }

        private ObservableCollection<Payment> _payments;
        
        private int _projectID;
        public int ProjectID
        {
            get { return _projectID; }
            set
            {
                if (value == _projectID || value == -1) return;
                _projectID = value;
                OnPropertyChanged();
                ChangeProject();
            }
        }

        private int _selectetPMId;

        public int ProjectManagerID
        {
            get { return _selectetPMId; }
            set
            {
                _selectetPMId = value;
                //ChangePM();
            }
        }

        public ObservableCollection<Project> Projects
        {
            get;
            set;
        }

        public ObservableCollection<string> CIPTypes
        {
            get;
            set;
        }

        public ObservableCollection<string> CrewEnrolls
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


        public ObservableCollection<string> JobNos
        {
            get;
            set;
        }
        private ObservableCollection<ProjectManager> _projectManagers;

        public ObservableCollection<ProjectManager> ProjectManagers
        {
            get => _projectManagers;
            set
            {
                if (value == _projectManagers) return;
                _projectManagers = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Superintendent> Superintendents
        {
            get;
            set;
        }

        public ObservableCollection<Manufacturer> Manufacturers
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

        public ObservableCollection<FreightCo> FreightCos
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

        public ObservableCollection<Material> Materials
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

        public ObservableCollection<Acronym> Acronyms
        {
            get;
            set;
        }

        public ObservableCollection<ReturnedVia> ReturnedViaNames
        {
            get;
            set;
        }

        private ObservableCollection<TrackShipRecv> _trackShipRecvs;

        public ObservableCollection<TrackShipRecv> TrackShipRecvs
        {
            get => _trackShipRecvs;
            set
            {
                if (value == _trackShipRecvs) return;
                _trackShipRecvs = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProjectWorkOrder> _projectWorkOrder;

        public ObservableCollection<ProjectWorkOrder> ProjectWorkOrders
        {
            get { return _projectWorkOrder; }
            set
            {
                if (_projectWorkOrder != value)
                {
                    _projectWorkOrder = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ProjectLabor> _projectLabor;

        public ObservableCollection<ProjectLabor> ProjectLabors
        {
            get { return _projectLabor; }
            set
            {
                if (_projectLabor != value)
                {
                    _projectLabor = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<WorkOrder> _workOrders;

        public ObservableCollection<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set
            {
                if (_workOrders != value)
                {
                    _workOrders = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ProjectMatTracking> _projectMatTracking;

        public ObservableCollection<ProjectMatTracking> ProjectMatTrackings
        {
            get { return _projectMatTracking; }
            set
            {
                if (_projectMatTracking != value)
                {
                    _projectMatTracking = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ProjectMatShip> _projectMtShip;

        public ObservableCollection<ProjectMatShip> ProjectMtShips
        {
            get { return _projectMtShip; }
            set
            {
                if (_projectMtShip != value)
                {
                    _projectMtShip = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<TrackReport> _trackReports;

        public ObservableCollection<TrackReport> TrackReports
        {
            get { return _trackReports; }
            set
            {
                if (_trackReports != value)
                {
                    _trackReports = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private ObservableCollection<TrackLaborReport> _trackLaborReports;

        public ObservableCollection<TrackLaborReport> TrackLaborReports
        {
            get { return _trackLaborReports; }
            set
            {
                if (_trackLaborReports != value)
                {
                    _trackLaborReports = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<SovCO> _sovCO;

        public ObservableCollection<SovCO> SovCOs
        {
            get { return _sovCO; }
            set
            {
                if (_sovCO != value)
                {
                    _sovCO = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ChangeOrder> _changeOrders;

        public ObservableCollection<ChangeOrder> ChangeOrders
        {
            get => _changeOrders;
            set
            {
                if (value == _changeOrders) return;
                _changeOrders = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SovMaterial> _sovMaterials;

        public ObservableCollection<SovMaterial> SovMaterials
        {
            get => _sovMaterials;
            set
            {
                if (value == _sovMaterials) return;
                _sovMaterials = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Labor> _labors;

        public ObservableCollection<Labor> Labors
        {
            get { return _labors; }
            set
            {
                _labors = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<InstallationNote> _installationNotes;

        public ObservableCollection<InstallationNote> InstallationNotes
        {
            get { return _installationNotes; }
            set
            {
                _installationNotes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Contract> _contracts;

        public ObservableCollection<Contract> Contracts
        {
            get { return _contracts; }
            set
            {
                _contracts = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _apprDen;
        public ObservableCollection<string> ApprDens
        {
            get { return _apprDen; }
            set
            {
                if (_apprDen != value)
                {
                    _apprDen = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ProjectManager> _projectManagerList;
        public ObservableCollection<ProjectManager> ProjectManagerList
        {
            get { return _projectManagerList; }
            set
            {
                if (_projectManagerList != value)
                {
                    _projectManagerList = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Superintendent> _superintendentLIst;
        public ObservableCollection<Superintendent> SuperintendentList
        {
            get { return _superintendentLIst; }
            set
            {
                if (_superintendentLIst != value)
                {
                    _superintendentLIst = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Note> _projectnotes;
        public ObservableCollection<Note> ProjectNotes
        {
            get { return _projectnotes; }
            set
            {
                if (_projectnotes != value)
                {
                    _projectnotes = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<CIP> _projectCIP;
        public ObservableCollection<CIP> ProjectCIPs
        {
            get { return _projectCIP; }
            set
            {
                if (_projectCIP != value)
                {
                    _projectCIP = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Note> _workOrderNotes;

        public ObservableCollection<Note> WorkOrderNotes
        {
            get { return _workOrderNotes; }
            set
            {
                if (_workOrderNotes != value)
                {
                    _workOrderNotes = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ProjectLink> _projectLinks;

        public ObservableCollection<ProjectLink> ProjectLinks
        {
            get { return _projectLinks; }
            set
            {
                if (_projectLinks != value)
                {
                    _projectLinks = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> _pathDesc;

        public ObservableCollection<string> PathDescritpions
        {
            get { return _pathDesc; }
            set
            {
                if (_pathDesc != value)
                {
                    _pathDesc = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _workOrderID;

        public int WorkOrderID
        {
            get { return _workOrderID; }
            set
            {
                if (_workOrderID != value)
                {
                    _workOrderID = value;
                    OnPropertyChanged();
                    ChangeWorkOrder();
                }
            }
        }

        
        private string _paymentNote;
        private DateTime _targetDate;
        private DateTime _completedDate;
        private string _jobNo;
        private Boolean _onHold;
        private string _address;
        private string _city;
        private string _state;
        private string _zip;
        private string _origTaxAmt;
        private string _origProfit;
        private string _origTotalMatCost;
        private string _billingDate;
        private int _customerID;
        private int _estimatorID;
        private int _projectCoordID;
        private int _archRepID;
        private int _CCID;
        private int _architectID;
        private int _selectedCrewID;
        private string _masterContractID;

        private string _projectName;

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

        private string _customerName;

        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (value == _customerName) return;
                _customerName = value;
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
        public DateTime Proj_TargetDate
        {
            get { return _targetDate; }
            set
            {
                if (_targetDate != value)
                {
                    _targetDate = value;
                    OnPropertyChanged(nameof(Proj_TargetDate));
                }
            }
        }
        public DateTime Proj_CompletedDate
        {
            get { return _completedDate; }
            set
            {
                if (_completedDate != value)
                {
                    _completedDate = value;
                    OnPropertyChanged(nameof(Proj_CompletedDate));
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
        public int CustomerID
        {
            get => _customerID;
            set
            {
                if (value == _customerID) return;
                _customerID = value;
                OnPropertyChanged();
            }
        }
        public int EstimatorID
        {
            get => _estimatorID;
            set
            {
                if (value == _estimatorID) return;
                _estimatorID = value;
                OnPropertyChanged();
            }
        }
        public int ProjectCoordID
        {
            get => _projectCoordID;
            set
            {
                if (value == _projectCoordID) return;
                _projectCoordID = value;
                OnPropertyChanged();
            }
        }
        public int ArchRepID
        {
            get => _archRepID;
            set
            {
                if (value == _archRepID) return;
                _archRepID = value;
                OnPropertyChanged();
            }
        }
        public int CCID
        {
            get => _CCID;
            set
            {
                if (value == _CCID) return;
                _CCID = value;
                OnPropertyChanged();
            }
        }
        public int ArchitectID
        {
            get => _architectID;
            set
            {
                if (value == _architectID) return;
                _architectID = value;
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
        public string MasterContractID
        {
            get => _masterContractID;
            set
            {
                if (value == _masterContractID) return;
                _masterContractID = value;
                OnPropertyChanged();
            }
        }

        private int _woCrewID;

        public int WO_CrewID
        {
            get { return _woCrewID; }
            set
            {
                if (_woCrewID != value)
                {
                    _woCrewID = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _woSuptID;
        public int WO_SuptID
        {
            get { return _woSuptID; }
            set
            {
                if (_woSuptID != value)
                {
                    _woSuptID = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _woNumber;

        public int WO_Number
        {
            get { return _woNumber; }
            set
            {
                if (_woNumber != value)
                {
                    _woNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _woSchedStartDate;

        public DateTime WO_SchedStartDate
        {
            get { return _woSchedStartDate; }
            set
            {
                if (_woSchedStartDate != value)
                {
                    _woSchedStartDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _woSchedComplDate;

        public DateTime WO_SchedComplDate
        {
            get { return _woSchedComplDate; }
            set
            {
                if (_woSchedComplDate != value)
                {
                    _woSchedComplDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _woStartDate;

        public DateTime WO_StartDate
        {
            get { return _woStartDate; }
            set
            {
                if (_woStartDate != value)
                {
                    _woStartDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _projMatID;

        public int ProjMatID
        {
            get { return _projMatID; }
            set
            {
                if (_projMatID != value)
                {
                    _projMatID = value;
                    OnPropertyChanged();
                    ChangeMaterial();
                }
            }
        }

        private DateTime _woComplDate;

        public DateTime WO_ComplDate
        {
            get { return _woComplDate; }
            set
            {
                if (_woComplDate != value)
                {
                    _woComplDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _additionalInfoNote;

        public string AdditionalInfoNote
        {
            get { return _additionalInfoNote; }
            set
            {
                if (_additionalInfoNote != value)
                {
                    _additionalInfoNote = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _safetyBadging;

        public string SafetyBadging
        {
            get { return _safetyBadging; }
            set
            {
                if (_safetyBadging != value)
                {
                    _safetyBadging = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _coAppDen;

        public string CoAppDen
        {
            get { return _coAppDen; }
            set
            {
                if (_coAppDen != value)
                {
                    _coAppDen = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _projComplete;

        public bool Proj_Complete
        {
            get { return _projComplete; }
            set
            {
                if (_projComplete != value)
                {
                    _projComplete = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _projectSelectionEnable;

        public bool ProjectSelectionEnable
        {
            get { return _projectSelectionEnable; }
            set
            {
                if (_projectSelectionEnable != value)
                    _projectSelectionEnable = value;
                OnPropertyChanged();
            }
        }

        private string _navigationBackName;

        public string NavigationBackName
        {
            get { return _navigationBackName; }
            set
            {
                if (value == _navigationBackName) return;
                _navigationBackName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> MasterContracts { get; set; }

        // Command
        public RelayCommand NewProjectCommand { get; set; }

        private void ChangeWorkOrder()
        {
            WorkOrder _workOrder = WorkOrders.Where(item => item.WoID == WorkOrderID).ToList()[0];
            WO_Number = _workOrder.WoNumber;
            WO_CrewID = _workOrder.CrewID;
            WO_SuptID = _workOrder.SuptID;
            WO_SchedStartDate = _workOrder.SchedStartDate;
            WO_SchedComplDate = _workOrder.SchedComplDate;
            WO_StartDate = _workOrder.DateStarted;
            WO_ComplDate = _workOrder.DateCompleted;

            // Work Order Notes
            sqlquery = "SELECT * FROM tblNotes WHERE Notes_PK = " + WorkOrderID + " AND Notes_PK_Desc = 'WorkOrder';";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Note> st_workOrderNotes = new ObservableCollection<Note>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string notesUser = "";
                string notesNote = "";
                string notesUserName = "";
                int notesID = 0;
                int notesPK = 0;
                DateTime notesDateAdded = new DateTime();
                string notesPKDesc = "";

                if (!row.IsNull("Notes_ID"))
                    notesID = row.Field<int>("Notes_ID");
                if (!row.IsNull("Notes_PK"))
                    notesPK = row.Field<int>("Notes_PK");
                if (!row.IsNull("Notes_Note"))
                    notesNote = row["Notes_Note"].ToString();
                if (!row.IsNull("Notes_User"))
                    notesUser = row["Notes_User"].ToString();
                if (!row.IsNull("Notes_UserName"))
                    notesUserName = row["Notes_UserName"].ToString();
                if (!row.IsNull("Notes_PK_Desc"))
                    notesPKDesc = row["Notes_PK_Desc"].ToString();
                if (!row.IsNull("Notes_DateAdded"))
                    notesDateAdded = row.Field<DateTime>("Notes_DateAdded");

                st_workOrderNotes.Add(new Note
                {
                    NoteID = notesID,
                    NotePK = notesPK,
                    NotesPKDesc = notesPKDesc,
                    NotesNote = notesNote,
                    NoteUser = notesUser,
                    NoteUserName = notesUserName,
                    NotesDateAdded = notesDateAdded
                });
            }

            WorkOrderNotes = st_workOrderNotes;
        }

        private void ChangeMaterial()
        {
            // ProjectMatTracking 
            sqlquery = "Select MatReqdDate, Qty_Ord, tblManufacturers.Manuf_ID, TakeFromStock, Manuf_LeadTime, Manuf_Order_No, PO_Number, ShopReqDate, ShopRecvdDate, SubmitIssue, Resubmit_Date, SubmitAppr, No_Sub_Needed, Ship_to_Job, FM_Needed, Guar_Dim, Field_Dim, Finals_Rev,  ReleasedForFab, MatComplete, LaborComplete from tblManufacturers RIGHT JOIN(Select * from tblProjectMaterialsTrack where ProjMat_ID = " + ProjMatID.ToString() + ") AS tblProjMat ON tblManufacturers.Manuf_ID = tblProjMat.Manuf_ID;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ProjectMatTracking> sb_projectMatTrackings = new ObservableCollection<ProjectMatTracking>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DateTime matReqdDate = new DateTime();

                if (!row.IsNull("MatReqdDate"))
                    matReqdDate = row.Field<DateTime>("MatReqdDate");
                string qtyOrd = "";
                if (!row.IsNull("Qty_Ord"))
                    qtyOrd = row.Field<int>("Qty_Ord").ToString();
                int manufID = row.Field<int>("Manuf_ID");
                bool stock = row.Field<Boolean>("TakeFromStock");
                string leadTime = "";
                if (!row.IsNull("Manuf_LeadTime"))
                    leadTime = row["Manuf_LeadTime"].ToString();
                string manufOrd = "";
                if (!row.IsNull("Manuf_Order_No"))
                    manufOrd = row["Manuf_Order_No"].ToString();
                string gapPO = "";
                if (!row.IsNull("PO_Number"))
                    gapPO = row["PO_Number"].ToString();
                DateTime shopReq = new DateTime();
                if (!row.IsNull("ShopReqDate"))
                    shopReq = row.Field<DateTime>("ShopReqDate");
                DateTime shopRecv = new DateTime();
                if (!row.IsNull("ShopRecvdDate"))
                    shopRecv = row.Field<DateTime>("ShopRecvdDate");
                DateTime submIssue = new DateTime();
                if (!row.IsNull("SubmitIssue"))
                    submIssue = row.Field<DateTime>("SubmitIssue");
                DateTime reSubmit = new DateTime();
                if (!row.IsNull("Resubmit_Date"))
                    reSubmit = row.Field<DateTime>("Resubmit_Date");
                DateTime submAppr = new DateTime();
                if (!row.IsNull("SubmitAppr"))
                    submAppr = row.Field<DateTime>("SubmitAppr");
                bool noSubNeeded = row.Field<Boolean>("No_Sub_Needed");
                bool shipToJob = row.Field<Boolean>("Ship_to_Job");
                bool fmNeeded = row.Field<Boolean>("FM_Needed");
                bool guarDim = row.Field<Boolean>("Guar_Dim");
                DateTime fieldDim = new DateTime();
                if (!row.IsNull("Field_Dim"))
                    fieldDim = row.Field<DateTime>("Field_Dim");
                bool finalsRev = row.Field<Boolean>("Finals_Rev");
                DateTime rff = new DateTime();
                if (!row.IsNull("ReleasedForFab"))
                    rff = row.Field<DateTime>("ReleasedForFab");
                bool matComplete = row.Field<Boolean>("MatComplete");
                bool laborComplete = row.Field<Boolean>("LaborComplete");

                sb_projectMatTrackings.Add(new ProjectMatTracking
                {
                    ProjMat = ProjectID,
                    MatReqdDate = matReqdDate,
                    QtyOrd = qtyOrd,
                    ManufacturerID = manufID,
                    TakeFromStock = stock,
                    LeadTime = leadTime,
                    ManuOrderNo = manufOrd,
                    PoNumber = gapPO,
                    ShopReqDate = shopReq,
                    ShopRecvdDate = shopRecv,
                    SubmIssue = submIssue,
                    ReSubmit = reSubmit,
                    SubmAppr = submAppr,
                    NoSubm = noSubNeeded,
                    ShipToJob = shipToJob,
                    NeedFM = fmNeeded,
                    GuarDim = guarDim,
                    FieldDim = fieldDim,
                    FinalsRev = finalsRev,
                    RFF = rff,
                    OrderComplete = matComplete,
                    LaborComplete = laborComplete
                });
                ProjectMatTrackings = sb_projectMatTrackings;
            }

            // Project Ship
            sqlquery = "SELECT * FROM tblProjectMaterialsShip RIGHT JOIN (SELECT ProjMT_ID FROM tblProjectMaterialsTrack WHERE ProjMat_ID = " + ProjMatID.ToString() + ") AS tblMat ON tblProjectMaterialsShip.ProjMT_ID = tblMat.ProjMT_ID;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ProjectMatShip> sb_projectMtShip = new ObservableCollection<ProjectMatShip>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DateTime schedShip = new DateTime();
                DateTime estDeliv = new DateTime();
                DateTime estInstall = new DateTime();
                string estWeight = "";
                int estNoPallets = 0;
                int freightCoID = 0;
                string tracking = "";
                DateTime dateShipped = new DateTime();
                int qtyShipped = 0;
                int noOfPallet = 0;
                DateTime dateRecvd = new DateTime();
                int qtyRecvd = 0;
                bool freightDamage = false;
                DateTime deliverToJob = new DateTime();
                string siteStroage = "";
                string stroageDetail = "";

                if (!row.IsNull("SchedShipDate"))
                    schedShip = row.Field<DateTime>("SchedShipDate");
                if (!row.IsNull("EstDelivDate"))
                    estDeliv = row.Field<DateTime>("EstDelivDate");
                if (!row.IsNull("EstInstallDate"))
                    estInstall = row.Field<DateTime>("EstInstallDate");
                if (!row.IsNull("EstWeight"))
                    estWeight = row["EstWeight"].ToString();
                if (!row.IsNull("EstNoPallets"))
                    estNoPallets = row.Field<int>("EstNoPallets");
                if (!row.IsNull("FreightCo_ID"))
                    freightCoID = row.Field<int>("FreightCo_ID");
                if (!row.IsNull("TrackingNo"))
                    tracking = row["TrackingNo"].ToString();
                if (!row.IsNull("Qty_Shipped"))
                    dateShipped = row.Field<DateTime>("Qty_Shipped");
                if (!row.IsNull("EstNoPallets"))
                    qtyShipped = row.Field<int>("Qty_Shipped");
                if (!row.IsNull("NoOfPallets"))
                    noOfPallet = int.Parse(row["NoOfPallets"].ToString());
                if (!row.IsNull("Date_Recvd"))
                    dateRecvd = row.Field<DateTime>("Date_Recvd");
                if (!row.IsNull("Qty_Recvd"))
                    qtyRecvd = row.Field<int>("Qty_Recvd");
                if (!row.IsNull("FreightDamage"))
                    freightDamage = row.Field<Boolean>("FreightDamage");
                if (!row.IsNull("DelivertoJob"))
                    deliverToJob = row.Field<DateTime>("DelivertoJob");
                if (!row.IsNull("SiteStorage"))
                    siteStroage = row["SiteStorage"].ToString();
                if (!row.IsNull("StorageDetail"))
                    stroageDetail = row["StorageDetail"].ToString();

                sb_projectMtShip.Add(new ProjectMatShip
                {
                    SchedShipDate = schedShip,
                    EstDelivDate = estDeliv,
                    EstInstallDate = estInstall,
                    EstWeight = estWeight,
                    EstPallet = estNoPallets,
                    FreightCoID = freightCoID,
                    TrackingNo = tracking,
                    DateShipped = dateShipped,
                    QtyShipped = qtyShipped,
                    NoOfPallet = noOfPallet,
                    DateRecvd = dateRecvd,
                    QtyRecvd = qtyRecvd,
                    FreightDamage = freightDamage,
                    DeliverToJob = deliverToJob,
                    SiteStroage = siteStroage,
                    StroageDetail = stroageDetail
                });
            }
            ProjectMtShips = sb_projectMtShip;
        }
    }

    public class PaymentItem
    {
        public bool IsChecked { get; set; }
        public string Text { get; set; }
    }

}
