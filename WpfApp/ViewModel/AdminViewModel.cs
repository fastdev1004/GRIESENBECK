using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Command;
using WpfApp.Model;
using WpfApp.Utils;

namespace WpfApp.ViewModel
{
    class AdminViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataSet ds;
        public string sqlquery;


        public AdminViewModel()
        {
            dbConnection = new DatabaseConnection();
            dbConnection.Open();
            Note noteItem = new Note();
            ManufNotes = new ObservableCollection<Note>();
            ManufNotes.Add(noteItem);

            CustomerNotes = new ObservableCollection<Note>();
            CustomerNotes.Add(noteItem);

            PMNotes = new ObservableCollection<Note>();
            PMNotes.Add(noteItem);

            SuptNotes = new ObservableCollection<Note>();
            SuptNotes.Add(noteItem);

            SubmNotes = new ObservableCollection<Note>();
            SubmNotes.Add(noteItem);

            ProjectManager projectManager = new ProjectManager();
            CustPMs = new ObservableCollection<ProjectManager>();
            CustPMs.Add(projectManager);

            CustomerContact customerContact = new CustomerContact();
            CustContacts = new ObservableCollection<CustomerContact>();
            CustContacts.Add(customerContact);

            Superintendent superintendent = new Superintendent();
            CustSupts = new ObservableCollection<Superintendent>();
            CustSupts.Add(superintendent);
            LoadAdmin();

            this.ViewCustomerCommand = new RelayCommand((e) =>
            {
                int customerID = int.Parse(e.ToString());
                this.ViewCustomer(customerID);
            });
        }

        public void ViewCustomer(int customerID)
        {
            sqlquery = "SELECT * FROM tblCustomers WHERE Customer_ID=" + customerID;
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            DataRow firstRow = ds.Tables[0].Rows[0];
            SelectedCustomerID = customerID;
            if (!firstRow.IsNull("Full_Name"))
                SelectedFullName = firstRow["Full_Name"].ToString();
            if (!firstRow.IsNull("Short_Name"))
                SelectedShortName = firstRow["Short_Name"].ToString();
            if (!firstRow.IsNull("PO_Box"))
                SelectedPoNumber = firstRow["PO_Box"].ToString();
            if (!firstRow.IsNull("Address"))
                SelectedAddress = firstRow["Address"].ToString();
            if (!firstRow.IsNull("City"))
                SelectedCity = firstRow["City"].ToString();
            if (!firstRow.IsNull("State"))
                SelectedState = firstRow["State"].ToString();
            if (!firstRow.IsNull("ZIP"))
                SelectedZip = firstRow["ZIP"].ToString();
            if (!firstRow.IsNull("Phone"))
                SelectedPhone = firstRow["Phone"].ToString();
            if (!firstRow.IsNull("FAX"))
                SelectedFax = firstRow["FAX"].ToString();
            if (!firstRow.IsNull("Email"))
                SelectedEmail = firstRow["Email"].ToString();
            if (!firstRow.IsNull("Active"))
                SelectedActive = firstRow.Field<Boolean>("Active");

            sqlquery = "SELECT * FROM tblNotes WHERE Notes_PK_Desc='Customer' AND Notes_PK=" + customerID;
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Note> st_customerNotes = new ObservableCollection<Note>();
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
                st_customerNotes.Add(new Note
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
            st_customerNotes.Add(new Note());
            CustomerNotes = st_customerNotes;

            // Project Manager Notes
            sqlquery = "SELECT * FROM tblNotes INNER JOIN (SELECT * FROM tblProjectManagers WHERE Customer_ID = "+ customerID +") AS tblProjManager ON tblNotes.Notes_PK = tblProjManager.PM_ID WHERE Notes_PK_Desc = 'ProjectManager'";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Note> st_pmNotes = new ObservableCollection<Note>();
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
                st_pmNotes.Add(new Note
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
            st_pmNotes.Add(new Note());
            PMNotes = st_pmNotes;

            // Superintendents Notes
            sqlquery = "SELECT * FROM tblNotes INNER JOIN (SELECT * FROM tblSuperintendents WHERE Customer_ID = "+ customerID +") AS tblSupt ON tblNotes.Notes_PK = tblSupt.Sup_ID WHERE Notes_PK_Desc = 'Superintendents'";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Note> st_suptNotes = new ObservableCollection<Note>();
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
                st_suptNotes.Add(new Note
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
            st_suptNotes.Add(new Note());
            SuptNotes = st_pmNotes;

            // CustomerContacts Notes
            sqlquery = "SELECT * FROM tblNotes INNER JOIN (SELECT * FROM tblCustomerContacts WHERE Customer_ID = " + customerID + ") AS tblSupt ON tblNotes.Notes_PK = tblSupt.CC_ID WHERE Notes_PK_Desc = 'CustomerContact'";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Note> st_ccNotes = new ObservableCollection<Note>();
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
                st_ccNotes.Add(new Note
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
            st_ccNotes.Add(new Note());
            SubmNotes = st_ccNotes;

            // Project Managers for customers
            sqlquery = "select * from tblProjectManagers WHERE Customer_ID="+ customerID;
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            ObservableCollection<ProjectManager> st_pm = new ObservableCollection<ProjectManager>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _id = 0;
                int _customerID = 0;
                string _name = "";
                string _phone = "";
                string _cellPhone = "";
                string _email = "";
                bool _active = false;

                if (!row.IsNull("PM_ID"))
                    _id = int.Parse(row["PM_ID"].ToString());
                if (!row.IsNull("Customer_ID"))
                    _customerID = int.Parse(row["Customer_ID"].ToString());
                if (!row.IsNull("PM_Name"))
                    _name = row["PM_Name"].ToString();
                if (!row.IsNull("PM_Phone"))
                    _phone = row["PM_Phone"].ToString();
                if (!row.IsNull("PM_CellPhone"))
                    _cellPhone = row["PM_CellPhone"].ToString();
                if (!row.IsNull("PM_Email"))
                    _email = row["PM_Email"].ToString();
                if (!row.IsNull("Active"))
                    _active = row.Field<Boolean>("Active");
                st_pm.Add(new ProjectManager
                {
                    ID = _id,
                    PMName = _name,
                    PMPhone = _phone,
                    PMCellPhone = _cellPhone,
                    PMEmail = _email,
                    Active = _active
                });
            }

            st_pm.Add(new ProjectManager());
            CustPMs = st_pm;

            sqlquery = "SELECT * FROM tblSuperintendents WHERE Customer_ID=" + customerID;
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            ObservableCollection<Superintendent> st_supt = new ObservableCollection<Superintendent>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _id = 0;
                int _customerID = 0;
                string _name = "";
                string _phone = "";
                string _cellPhone = "";
                string _email = "";
                bool _active = false;

                if (!row.IsNull("Sup_ID"))
                    _id = int.Parse(row["Sup_ID"].ToString());
                if (!row.IsNull("Customer_ID"))
                    _customerID = int.Parse(row["Customer_ID"].ToString());
                if (!row.IsNull("Sup_Name"))
                    _name = row["Sup_Name"].ToString();
                if (!row.IsNull("Sup_Phone"))
                    _phone = row["Sup_Phone"].ToString();
                if (!row.IsNull("Sup_CellPhone"))
                    _cellPhone = row["Sup_CellPhone"].ToString();
                if (!row.IsNull("Sup_Email"))
                    _email = row["Sup_Email"].ToString();
                if (!row.IsNull("Active"))
                    _active = row.Field<Boolean>("Active");
                st_supt.Add(new Superintendent
                {
                    SupID = _id,
                    SupName = _name,
                    SupPhone = _phone,
                    SupCellPhone = _cellPhone,
                    SupEmail = _email,
                    Active = _active
                });
            }

            st_supt.Add(new Superintendent());
            CustSupts = st_supt;

            sqlquery = "SELECT * FROM tblCustomerContacts WHERE Customer_ID=" + customerID;
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            ObservableCollection<CustomerContact> st_customerContacts = new ObservableCollection<CustomerContact>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _id = 0;
                int _customerID = 0;
                string _name = "";
                string _phone = "";
                string _cellPhone = "";
                string _email = "";
                bool _active = false;

                if (!row.IsNull("CC_ID"))
                    _id = int.Parse(row["CC_ID"].ToString());
                if (!row.IsNull("Customer_ID"))
                    _customerID = int.Parse(row["Customer_ID"].ToString());
                if (!row.IsNull("CC_Name"))
                    _name = row["CC_Name"].ToString();
                if (!row.IsNull("CC_Phone"))
                    _phone = row["CC_Phone"].ToString();
                if (!row.IsNull("CC_CellPhone"))
                    _cellPhone = row["CC_CellPhone"].ToString();
                if (!row.IsNull("CC_Email"))
                    _email = row["CC_Email"].ToString();
                if (!row.IsNull("Active"))
                    _active = row.Field<Boolean>("Active");
                st_customerContacts.Add(new CustomerContact
                {
                    ID = _id,
                    CCName = _name,
                    CCPhone = _phone,
                    CCCell = _cellPhone,
                    CCEmail = _email,
                    CCActive = _active
                });
            }

            st_customerContacts.Add(new CustomerContact());
            CustContacts = st_customerContacts;
        }

        public void LoadAdmin()
        {
            // Projects
            sqlquery = "SELECT * FROM tblScheduleOfValues ORDER BY SOV_Acronym";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Acronym> st_acronym = new ObservableCollection<Acronym>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string acronymName = row["SOV_Acronym"].ToString();
                string acronymDesc = row["SOV_Desc"].ToString();
                bool active = row.Field<Boolean>("Active");
                st_acronym.Add(new Acronym { AcronymName = acronymName, AcronymDesc = acronymDesc, Active = active });
            }
            Acronyms = st_acronym;

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
                bool active = row.Field<Boolean>("Active");

                st_material.Add(new Material { ID = materialID, MatDesc = materialDesc, Active = active });
            }
            Materials = st_material;

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

            // Salesmen
            sqlquery = "SELECT * FROM tblSalesmen ORDER BY Salesman_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Salesman> st_salesman = new ObservableCollection<Salesman>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int salesmanID = int.Parse(row["Salesman_ID"].ToString());
                string init = row["Salesman_Init"].ToString();
                string name = row["Salesman_Name"].ToString();
                string phone = row["Phone"].ToString();
                string cell = row["Cell"].ToString();
                string email = row["Salesman_Email"].ToString();
                bool active = row.Field<Boolean>("Active");

                st_salesman.Add(new Salesman { ID = salesmanID, SalesmanName = name, Phone = phone, Cell = cell, Email = email, Active = active });
            }
            Salesmans = st_salesman;

            // Crew
            sqlquery = "SELECT * FROM tblInstallCrew ORDER BY Crew_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Crew> st_crew = new ObservableCollection<Crew>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int crewID = int.Parse(row["Crew_ID"].ToString());
                string crewDesc = row["Crew_Name"].ToString();
                string phone = row["Crew_Phone"].ToString();
                string cell = row["Crew_Cell"].ToString();
                string email = row["Crew_Email"].ToString();
                bool active = row.Field<Boolean>("Active");

                st_crew.Add(new Crew { ID = crewID, CrewName = crewDesc, Phone = phone, Cell = cell, Email = email, Active = active });
            }
            Crews = st_crew;

            // User
            sqlquery = "SELECT * FROM tblUsers ORDER BY User_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<User> st_user = new ObservableCollection<User>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int id = int.Parse(row["User_ID"].ToString());
                string name = row["User_Name"].ToString();
                string personName = row["User_PersonName"].ToString();
                int level = int.Parse(row["User_Level"].ToString());
                string fromOnOpen = row["User_FormOnOpen"].ToString();
                string email = row["User_Email"].ToString();
                bool active = row.Field<Boolean>("Active");

                st_user.Add(new User { ID = id, UserName = name, PersonName = personName, Level = level, FromOnOpen = fromOnOpen, Email = email, Active = active });
            }
            Users = st_user;

            // Architect
            sqlquery = "SELECT * FROM tblArchitects ORDER BY Arch_Company";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Architect> st_architect = new ObservableCollection<Architect>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int id = int.Parse(row["Architect_ID"].ToString());
                string company = row["Arch_Company"].ToString();
                string contact = row["Arch_Contact"].ToString();
                string address = row["Arch_Address"].ToString();
                string city = row["Arch_City"].ToString();
                string state = row["Arch_State"].ToString();
                string zip = row["Arch_ZIP"].ToString();
                string phone = row["Arch_Phone"].ToString();
                string fax = row["Arch_FAX"].ToString();
                string cell = row["Arch_Cell"].ToString();
                string email = row["Arch_Email"].ToString();
                bool active = row.Field<Boolean>("Active");

                st_architect.Add(new Architect { ID = id, ArchCompany = company, Contact = contact, Address = address, City = city, State = state, Zip = zip, Phone = phone, Fax = fax, Cell = cell, Email = email, Active = active });
            }
            Architects = st_architect;

            // Freight CO
            sqlquery = "SELECT * FROM tblFreightCo ORDER BY FreightCo_Name;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<FreightCo> st_freightCO = new ObservableCollection<FreightCo>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int id = int.Parse(row["FreightCo_ID"].ToString());
                string name = row["FreightCo_Name"].ToString();
                string phone = row["FreightCo_Phone"].ToString();
                string email = row["FreightCo_Email"].ToString();
                string contact = row["FreightCo_Contact"].ToString();
                string cell = row["FreightCo_Cell"].ToString();
                bool active = row.Field<Boolean>("Active");

                st_freightCO.Add(new FreightCo { ID = id, FreightName = name, Phone = phone, Email = email, Contact = contact, Cell = cell, Active = active });
            }
            FreightCos = st_freightCO;

            // House Installer
            sqlquery = "SELECT * FROM tblInHouseInstallers ORDER BY Installer_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<InHouseInstaller> st_installer = new ObservableCollection<InHouseInstaller>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int id = 0;
                string name = "";
                string cell = "";
                string email = "";
                string oshaLevel = "";
                int crewID = -1;
                DateTime oshaExpireDate = new DateTime();
                string oshaCert = "";
                DateTime firstAidExpireDate = new DateTime();
                string firstAidCert = "";

                id = int.Parse(row["Installer_ID"].ToString());
                if (!row.IsNull("Installer_Name"))
                    name = row["Installer_Name"].ToString();
                if (!row.IsNull("Installer_Cell"))
                    cell = row["Installer_Cell"].ToString();
                if (!row.IsNull("Installer_Email"))
                    email = row["Installer_Email"].ToString();
                if (!row.IsNull("OSHA_Level"))
                    oshaLevel = row["OSHA_Level"].ToString();
                if (!row.IsNull("Crew"))
                    crewID = int.Parse(row["Crew"].ToString());
                if (!row.IsNull("OSHA Expiration"))
                    oshaExpireDate = row.Field<DateTime>("OSHA Expiration");
                if (!row.IsNull("OSHA_Cert"))
                    oshaCert = row["OSHA_Cert"].ToString();
                if (!row.IsNull("FirstAidCPR_Expiration"))
                    firstAidExpireDate = row.Field<DateTime>("FirstAidCPR_Expiration");
                if (!row.IsNull("FirstAidCPR_Cert"))
                    firstAidCert = row["FirstAidCPR_Cert"].ToString();
                bool active = row.Field<Boolean>("Active");

                st_installer.Add(new InHouseInstaller { ID = id, InstallerName = name, InstallerCell = cell, InstallerEmail = email, OSHALevel = oshaLevel, CrewID = crewID, OSHAExpireDate = oshaExpireDate, OSHACert = oshaCert, FirstAidCert = firstAidCert, FirstAidExpireDate = firstAidExpireDate, Active = active });
            }
            Installers = st_installer;

            // Customer
            sqlquery = "SELECT * FROM tblCustomers ORDER BY Short_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Customer> st_customers = new ObservableCollection<Customer>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int id = int.Parse(row["Customer_ID"].ToString());
                string shortName = row["Short_Name"].ToString();
                string fullName = row["Full_Name"].ToString();
                string poBox = row["PO_Box"].ToString();
                string address = row["Address"].ToString();
                string city = row["City"].ToString();
                string state = row["State"].ToString();
                string zip = row["ZIP"].ToString();
                string phone = row["Phone"].ToString();
                string fax = row["FAX"].ToString();
                string email = row["Email"].ToString();
                bool active = row.Field<Boolean>("Active");

                st_customers.Add(new Customer { ID = id, ShortName = shortName, FullName = fullName, PoBox = poBox, Address = address, City = city, State = state, Zip = zip, Phone = phone, Fax = fax, Email = email, Active = active });
            }
            Customers = st_customers;

            // Manafacturer
            sqlquery = "SELECT * FROM tblManufacturers ORDER BY Manuf_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Manufacturer> st_manufacturers = new ObservableCollection<Manufacturer>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int id = int.Parse(row["Manuf_ID"].ToString());
                string manufName = row["Manuf_Name"].ToString();
                string address = row["Address"].ToString();
                string address2 = row["Address2"].ToString();
                string city = row["City"].ToString();
                string state = row["State"].ToString();
                string zip = row["Zip"].ToString();
                string phone = row["Phone"].ToString();
                string fax = row["Fax"].ToString();
                string contactName = row["Contact_Name"].ToString();
                string contactEmail = row["Contact_Email"].ToString();
                bool active = row.Field<Boolean>("Active");

                st_manufacturers.Add(new Manufacturer { ID = id, ManufacturerName = manufName, Address = address, Address2 = address2, City = city, State = state, Zip = zip, Phone = phone, Fax = fax, ContactName = contactName, ContactEmail = contactEmail, Active = active });
            }
            Manufacturers = st_manufacturers;

            // Project Managers
            sqlquery = "SELECT * FROM tblProjectManagers ORDER BY PM_Name;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ProjectManager> st_projectManagers = new ObservableCollection<ProjectManager>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int id = int.Parse(row["PM_ID"].ToString());
                string name = row["PM_Name"].ToString();
                string phone = row["PM_Phone"].ToString();
                string cell = row["PM_CellPhone"].ToString();
                string email = row["PM_Email"].ToString();
                bool active = row.Field<Boolean>("Active");

                st_projectManagers.Add(new ProjectManager { ID = id, PMName = name, PMPhone = phone, PMCellPhone = cell, PMEmail = email, Active = active });
            }
            ProjectManagers = st_projectManagers;

            // Superintendents
            sqlquery = "SELECT * FROM tblSuperintendents ORDER BY Sup_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Superintendent> st_superintendents = new ObservableCollection<Superintendent>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int id = int.Parse(row["Sup_ID"].ToString());
                string name = row["Sup_Name"].ToString();
                string phone = row["Sup_Phone"].ToString();
                string cell = row["Sup_CellPhone"].ToString();
                string email = row["Sup_Email"].ToString();
                bool active = row.Field<Boolean>("Active");

                st_superintendents.Add(new Superintendent { SupID = id, SupName = name, SupPhone = phone, SupCellPhone = cell, SupEmail = email, Active = active });
            }
            Superintendents = st_superintendents;

            cmd.Dispose();
            dbConnection.Close();
        }

        private ObservableCollection<Acronym> _acronyms;

        public ObservableCollection<Acronym> Acronyms
        {
            get { return _acronyms; }
            set
            {
                _acronyms = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Material> _materials;

        public ObservableCollection<Material> Materials
        {
            get { return _materials; }
            set
            {
                _materials = value;
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

        private ObservableCollection<Salesman> _salesmen;

        public ObservableCollection<Salesman> Salesmans
        {
            get { return _salesmen; }
            set
            {
                _salesmen = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Crew> _crew;

        public ObservableCollection<Crew> Crews
        {
            get { return _crew; }
            set
            {
                _crew = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<User> _users;

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Architect> _architects;

        public ObservableCollection<Architect> Architects
        {
            get { return _architects; }
            set
            {
                _architects = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FreightCo> _freightCo;

        public ObservableCollection<FreightCo> FreightCos
        {
            get { return _freightCo; }
            set
            {
                _freightCo = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<InHouseInstaller> _installers;

        public ObservableCollection<InHouseInstaller> Installers
        {
            get { return _installers; }
            set
            {
                _installers = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Customer> _customer;

        public ObservableCollection<Customer> Customers
        {
            get { return _customer; }
            set
            {
                _customer = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Manufacturer> _manufacturers;

        public ObservableCollection<Manufacturer> Manufacturers
        {
            get { return _manufacturers; }
            set
            {
                _manufacturers = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProjectManager> _projectManagers;

        public ObservableCollection<ProjectManager> ProjectManagers
        {
            get { return _projectManagers; }
            set
            {
                _projectManagers = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Superintendent> _superintendents;

        public ObservableCollection<Superintendent> Superintendents
        {
            get { return _superintendents; }
            set
            {
                _superintendents = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Note> _manufNotes;

        public ObservableCollection<Note> ManufNotes
        {
            get { return _manufNotes; }
            set
            {
                _manufNotes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Note> _customerNotes;

        public ObservableCollection<Note> CustomerNotes
        {
            get { return _customerNotes; }
            set
            {
                _customerNotes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Note> _pmNotes;

        public ObservableCollection<Note> PMNotes
        {
            get { return _pmNotes; }
            set
            {
                _pmNotes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Note> _submNotes;

        public ObservableCollection<Note> SubmNotes
        {
            get { return _submNotes; }
            set
            {
                _submNotes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Note> _suptNotes;

        public ObservableCollection<Note> SuptNotes
        {
            get { return _suptNotes; }
            set
            {
                _suptNotes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProjectManager> _custPMs;

        public ObservableCollection<ProjectManager> CustPMs
        {
            get { return _custPMs; }
            set
            {
                _custPMs = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Superintendent> _custSupts;

        public ObservableCollection<Superintendent> CustSupts
        {
            get { return _custSupts; }
            set
            {
                _custSupts = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CustomerContact> _custContact;

        public ObservableCollection<CustomerContact> CustContacts
        {
            get { return _custContact; }
            set
            {
                _custContact = value;
                OnPropertyChanged();
            }
        }

        public int SelectedPMRowIndex { get; set; }

        public int SelectedSubmRowIndex { get; set; }

        public int SelectedSuptRowIndex { get; set; }

        public int SelectedSubmNoteRowIndex { get; set; }

        public int SelectedSuptNoteRowIndex { get; set; }

        public int SelectedPMNoteRowIndex { get; set; }

        public int SelectedCustNoteRowIndex { get; set; }

        public int SelectedManufNoteRowIndex { get; set; }

        public RelayCommand ViewCustomerCommand { get; set; }

        private int _selectedCustomerID;

        public int SelectedCustomerID
        {
            get { return _selectedCustomerID; }
            set
            {
                _selectedCustomerID = value;
                OnPropertyChanged();

            }
        }

        private string _selectedFullName;

        public string SelectedFullName
        {
            get { return _selectedFullName; }
            set
            {
                _selectedFullName = value;
                OnPropertyChanged();

            }
        }

        private string _selectedShortName;

        public string SelectedShortName
        {
            get { return _selectedShortName; }
            set
            {
                _selectedShortName = value;
                OnPropertyChanged();

            }
        }

        private bool _selectedActive;

        public bool SelectedActive
        {
            get { return _selectedActive; }
            set
            {
                _selectedActive = value;
                OnPropertyChanged();

            }
        }

        private string _selectedPoNumber;

        public string SelectedPoNumber
        {
            get { return _selectedPoNumber; }
            set
            {
                _selectedPoNumber = value;
                OnPropertyChanged();

            }
        }

        private string _selectedAddress;

        public string SelectedAddress
        {
            get { return _selectedAddress; }
            set
            {
                _selectedAddress = value;
                OnPropertyChanged();

            }
        }

        private string _selectedCity;

        public string SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;
                OnPropertyChanged();

            }
        }

        private string _selectedPhone;

        public string SelectedPhone
        {
            get { return _selectedPhone; }
            set
            {
                _selectedPhone = value;
                OnPropertyChanged();

            }
        }

        private string _selectedState;

        public string SelectedState
        {
            get { return _selectedState; }
            set
            {
                _selectedState = value;
                OnPropertyChanged();

            }
        }

        private string _selectedZip;

        public string SelectedZip
        {
            get { return _selectedZip; }
            set
            {
                _selectedZip = value;
                OnPropertyChanged();

            }
        }

        private string _selectedFax;

        public string SelectedFax
        {
            get { return _selectedFax; }
            set
            {
                _selectedFax = value;
                OnPropertyChanged();

            }
        }

        private string _selectedEmail;

        public string SelectedEmail
        {
            get { return _selectedEmail; }
            set
            {
                _selectedEmail = value;
                OnPropertyChanged();

            }
        }
    }
}