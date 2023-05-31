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
            sqlquery = "SELECT Customer_ID, Full_Name, Address FROM tblCustomers Order by Full_Name";
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
            if (!firstRow.IsNull("Project_Name"))
                ProjectName = firstRow["Project_Name"].ToString();
            if (!firstRow.IsNull("Target_Date"))
                TargetDate = firstRow.Field<DateTime>("Target_Date");
            if (!firstRow.IsNull("Date_Completed"))
                CompletedDate = firstRow.Field<DateTime>("Date_Completed");
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
                SelectedEstimatorID = firstRow.Field<int>("Estimator_ID");
            if (!firstRow.IsNull("PC_ID"))
                SelectedProjectCoordID = firstRow.Field<int>("PC_ID");
            if (!firstRow.IsNull("Arch_Rep_ID"))
                SelectedArchRepID = firstRow.Field<int>("Arch_Rep_ID");
            if (!firstRow.IsNull("CC_ID"))
                SelectedCCID = firstRow.Field<int>("CC_ID");
            if (!firstRow.IsNull("Architect_ID"))
                SelectedArchitectID = firstRow.Field<int>("Architect_ID");
            if (!firstRow.IsNull("Crew_ID"))
                SelectedCrewID = firstRow.Field<int>("Crew_ID");
            if (!firstRow.IsNull("Master_Contract"))
                if (firstRow.Field<string>("Master_Contract") == "Yes")
                    SelectedMCID = 0;
                else SelectedMCID = 1;
            if (!firstRow.IsNull("Customer_ID"))
                SelectedCustomerId = firstRow.Field<int>("Customer_ID");

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
            sqlquery = "Select MatReqdDate, tblManufacturers.Manuf_Name, Qty_Ord, tblProjMat.Mat_Phase, tblProjMat.Mat_Type, Manuf_LeadTime, PO_Number, ShopReqDate, ShopRecvdDate, SubmitIssue, Resubmit_Date, SubmitAppr,tblProjMat.Color_Selected, Guar_Dim, Field_Dim, ReleasedForFab, LaborComplete from tblManufacturers RIGHT JOIN(Select tblProjectMaterialsTrack.*, tblMat.Color_Selected, tblMat.Mat_Phase, tblMat.Mat_Type from tblProjectMaterialsTrack RIGHT JOIN(SELECT * FROM tblProjectMaterials WHERE tblProjectMaterials.Project_ID = " + ProjectID.ToString() + ") AS tblMat ON tblMat.ProjMat_ID = tblProjectMaterialsTrack.ProjMat_ID) AS tblProjMat ON tblManufacturers.Manuf_ID = tblProjMat.Manuf_ID ORDER BY MatReqdDate;";
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
                
                sb_changeOrder.Add(new ChangeOrder
                {
                    CoItemNo = int.Parse(row["CO_ItemNo"].ToString())
                });
            }

            ChangeOrders = sb_changeOrder;

            // SOVGrid2
            sqlquery = "Select tblSOV.SOV_Acronym, tblSOV.CO_ItemNo, tblSOV.Material_Only, tblSOV.SOV_Desc, tblProjectMaterials.Mat_Phase, tblProjectMaterials.Mat_Type, tblProjectMaterials.Color_Selected, tblProjectMaterials.Qty_Reqd, tblProjectMaterials.TotalCost from(Select tblSOV.*, tblProjectChangeOrders.CO_ItemNo from(Select tblSOV.*, tblScheduleOfValues.SOV_Desc from tblScheduleOfValues Right JOIN(SELECT tblProjectSOV.* From tblProjects LEFT Join tblProjectSOV ON tblProjects.Project_ID = tblProjectSOV.Project_ID where tblProjects.Project_ID = " + ProjectID.ToString() + " ) AS tblSOV ON tblSOV.SOV_Acronym = tblScheduleOfValues.SOV_Acronym Where tblScheduleOfValues.Active = 'true') AS tblSOV LEFT JOIN tblProjectChangeOrders ON tblProjectChangeOrders.CO_ID = tblSOV.CO_ID) AS tblSOV LEFT JOIN tblProjectMaterials ON tblSOV.ProjSOV_ID = tblProjectMaterials.ProjSOV_ID ORDER BY tblSOV.SOV_Acronym DESC";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ///////////////////////////////////////
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

            cmd.Dispose();
            dbConnection.Close();
        }

        private int _selectetProjectId;
        private int _selectetPMId;
        private ObservableCollection<Payment> _payments;
        public int ProjectID
        {
            get { return _selectetProjectId; }
            set
            {
                _selectetProjectId = value;
                Console.WriteLine(123123123);
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

        public ObservableCollection<ProjectManager> ProjectManagers
        {
            get;
            set;
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

        private int _suptID;
        public int SuptID
        {
            get { return _suptID; }
            set
            {
                if (_suptID != value)
                {
                    _suptID = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _woNumber;

        public int WoNumber
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

        private DateTime _schedStartDate;

        public DateTime SchedStartDate
        {
            get { return _schedStartDate; }
            set
            {
                if (_schedStartDate != value)
                {
                    _schedStartDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _schedComplDate;

        public DateTime SchedComplDate
        {
            get { return _schedComplDate; }
            set
            {
                if (_schedComplDate != value)
                {
                    _schedComplDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
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
                    Console.WriteLine("ProjectMatTrackings");
                    _projMatID = value;
                    OnPropertyChanged();
                    ChangeMaterial();
                }
            }
        }

        private DateTime _complDate;

        public DateTime ComplDate
        {
            get { return _complDate; }
            set
            {
                if (_complDate != value)
                {
                    _complDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ChangeWorkOrder()
        {
            WorkOrder _workOrder = WorkOrders.Where(item => item.WoID == WorkOrderID).ToList()[0];
            CrewID = _workOrder.CrewID;
            SuptID = _workOrder.SuptID;
            WoNumber = _workOrder.WoNumber;
            SchedStartDate = _workOrder.SchedStartDate;
            SchedComplDate = _workOrder.SchedComplDate;
            StartDate = _workOrder.DateStarted;
            ComplDate = _workOrder.DateCompleted;
            Console.WriteLine("WorkOrderID");
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
                Console.WriteLine(ProjectMatTrackings.Count);
                Console.WriteLine("ProjectMatTrackings");
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
            if (sb_projectMtShip.Count != 0)
                ProjectMtShips = sb_projectMtShip;
            else
            {
                //sb_projectMtShip.Add(new ProjectMatShip());
                //ProjectMtShips = sb_projectMtShip;
            }
        }
    }

    public class PaymentItem
    {
        public bool IsChecked { get; set; }
        public string Text { get; set; }
    }

}
