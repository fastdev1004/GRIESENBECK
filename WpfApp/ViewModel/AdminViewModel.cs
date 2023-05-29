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
    class AdminViewModel:ViewModelBase
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
            LoadAdmin();
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
                st_acronym.Add(new Acronym { AcronymName = acronymName,  AcronymDesc = acronymDesc, Active = active });
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

                st_salesman.Add(new Salesman { ID = salesmanID, SalesmanName=name, Phone=phone, Cell=cell, Email=email, Active = active });
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

                st_crew.Add(new Crew { ID = crewID, CrewName = crewDesc, Phone = phone, Cell=cell, Email=email, Active = active });
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

                st_user.Add(new User { ID = id, UserName = name, PersonName=personName, Level=level, FromOnOpen=fromOnOpen, Email=email, Active = active });
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

                st_architect.Add(new Architect { ID = id, ArchCompany=company, Contact=contact, Address=address, City=city, State=state, Zip=zip, Phone=phone, Fax=fax, Cell=cell, Email=email, Active = active });
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

                st_freightCO.Add(new FreightCo { ID = id, FreightName=name, Phone=phone, Email=email,Contact=contact, Cell=cell, Active = active });
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
                if(!row.IsNull("Installer_Name"))
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

                st_installer.Add(new InHouseInstaller { ID = id, InstallerName = name, InstallerCell=cell, InstallerEmail=email, OSHALevel=oshaLevel, CrewID=crewID, OSHAExpireDate=oshaExpireDate, OSHACert= oshaCert, FirstAidCert= firstAidCert, FirstAidExpireDate= firstAidExpireDate, Active = active });
            }
            Installers = st_installer;

            // Customer
            sqlquery = "SELECT * FROM tblCustomers ORDER BY Full_Name";
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

                st_customers.Add(new Customer { ID = id, ShortName=shortName, FullName=fullName, PoBox=poBox, Address=address,City=city, State=state, Zip=zip, Phone=phone, Fax=fax, Email=email, Active = active });
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

                st_manufacturers.Add(new Manufacturer { ID = id, ManufacturerName=manufName, Address=address,Address2=address2,City=city, State=state, Zip=zip, Phone=phone, Fax=fax, ContactName=contactName, ContactEmail=contactEmail, Active = active });
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

                st_projectManagers.Add(new ProjectManager { ID = id, PMName=name, Phone=phone, PMCellPhone=cell, PMEmail=email, Active = active });
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
    }
}
