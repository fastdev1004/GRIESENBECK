﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

            CustPMNotes = new ObservableCollection<Note>();
            CustPMNotes.Add(noteItem);

            CustSuptNotes = new ObservableCollection<Note>();
            CustSuptNotes.Add(noteItem);

            CustSubmNotes = new ObservableCollection<Note>();
            CustSubmNotes.Add(noteItem);

            ProjectManager projectManager = new ProjectManager();
            CustPMs = new ObservableCollection<ProjectManager>();
            CustPMs.Add(projectManager);

            CustomerContact customerContact = new CustomerContact();
            CustContacts = new ObservableCollection<CustomerContact>();
            CustContacts.Add(customerContact);

            Superintendent superintendent = new Superintendent();
            CustSupts = new ObservableCollection<Superintendent>();
            CustSupts.Add(superintendent);

            TempCustomer = new Customer();
            TempDetailCustomer = new Customer();
            TempManuf = new Manufacturer();
            TempDetailManuf = new Manufacturer();
            TempAcronym = new Acronym();
            TempMaterial = new Material();
            TempInstaller = new InHouseInstaller();
            TempCreateCustomer = new Customer();

            //TempCustomerNote = new Note();
            //TempSubmNote = new Note();
            //TempSupNote = new Note();
            //TempPMNote = new Note();
            TempNote = new Note();
            SelectedTempCustIndex = 0;
            SelectedCustRowIndex = -1;
            ActionCustState = "Clear";
            ActionManufState = "ClearManuf";
            ActionCustName = "CreateCustomer";
            ActionManufName = "CreateManuf";
            ActionPmName = "";
            UpdateComponent = "Detail";
            ActionState = "";
            ActionSubState = "";
            ActionNoteState = "";

            SelectedManufID = -1;
            SelectedCustomerID = -1;
            LoadAdmin();
            //IsInitialLoad = true;
            this.ViewCustomerCommand = new RelayCommand((e) =>
            {
                int customerID = int.Parse(e.ToString());
                this.ViewCustomer(customerID);
            });

            this.ViewManufCommand = new RelayCommand((e) =>
            {
                int manufID = int.Parse(e.ToString());
                this.ViewManuf(manufID);
            });

            this.NewCustomerCommand = new RelayCommand((e) =>
            {
                this.ClearCustomer();
            });

            this.NewManufCommand = new RelayCommand((e) =>
            {
                this.ClearManuf();
            });

        }

        public void CreateInstaller()
        {
            sqlquery = "INSERT INTO tblInHouseInstallers(Installer_Name, Installer_Cell, Installer_Email, OSHA_Level, Crew, [OSHA Expiration], OSHA_Cert, FirstAidCPR_Expiration, FirstAidCPR_Cert, Active) OUTPUT INSERTED.Installer_ID VALUES (@Name, @Cell, @Email, @OshaLevel, @Crew, @OshaDate, @OshaCert, @FacDate, @FacCert, @Active)";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(TempInstaller.InstallerName))
                    cmd.Parameters.AddWithValue("@Name", TempInstaller.InstallerName);
                else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                if (!string.IsNullOrEmpty(TempInstaller.InstallerCell))
                    cmd.Parameters.AddWithValue("@Cell", TempInstaller.InstallerCell);
                else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                if (!string.IsNullOrEmpty(TempInstaller.InstallerEmail))
                    cmd.Parameters.AddWithValue("@Email", TempInstaller.InstallerEmail);
                else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                if (!string.IsNullOrEmpty(TempInstaller.OSHALevel))
                    cmd.Parameters.AddWithValue("@OshaLevel", TempInstaller.OSHALevel);
                else cmd.Parameters.AddWithValue("@OshaLevel", DBNull.Value);
                if (TempInstaller.CrewID != 0)
                    cmd.Parameters.AddWithValue("@Crew", TempInstaller.CrewID);
                else cmd.Parameters.AddWithValue("@Crew", DBNull.Value);
                if (!TempInstaller.OSHAExpireDate.Equals(DateTime.MinValue))
                    cmd.Parameters.AddWithValue("@OshaDate", TempInstaller.OSHAExpireDate);
                else cmd.Parameters.AddWithValue("@OshaDate", DBNull.Value);
                if (!string.IsNullOrEmpty(TempInstaller.OSHACert))
                    cmd.Parameters.AddWithValue("@OshaCert", TempInstaller.OSHACert);
                else cmd.Parameters.AddWithValue("@OshaCert", DBNull.Value);
                if (!TempInstaller.FirstAidExpireDate.Equals(DateTime.MinValue))
                    cmd.Parameters.AddWithValue("@FacDate", TempInstaller.FirstAidExpireDate);
                else cmd.Parameters.AddWithValue("@FacDate", DBNull.Value);
                if (!string.IsNullOrEmpty(TempInstaller.FirstAidCert))
                    cmd.Parameters.AddWithValue("@FacCert", TempInstaller.FirstAidCert);
                else cmd.Parameters.AddWithValue("@FacCert", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempInstaller.Active);

                try
                {
                    int insertedInstallerID = (int)cmd.ExecuteScalar();
                    SelectedInstallerID = insertedInstallerID;
                    int totalCount = Installers.Count;
                    CurrentIndex = totalCount - 2;
                    TempInstaller.ID = SelectedInstallerID;
                    Installers[totalCount - 2] = TempInstaller;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void UpdateInstaller()
        {
            sqlquery = "UPDATE tblInHouseInstallers SET Installer_Name=@Name, Installer_Cell=@Cell, Installer_Email=@Email, OSHA_Level=@OshaLevel, Crew=@Crew, [OSHA Expiration]=@OshaDate, OSHA_Cert=@OshaCert, FirstAidCPR_Expiration=@FacDate, FirstAidCPR_Cert=@FacCert, Active=@Active WHERE Installer_ID=@InstallerID";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(TempInstaller.InstallerName))
                    cmd.Parameters.AddWithValue("@Name", TempInstaller.InstallerName);
                else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                if (!string.IsNullOrEmpty(TempInstaller.InstallerCell))
                    cmd.Parameters.AddWithValue("@Cell", TempInstaller.InstallerCell);
                else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                if (!string.IsNullOrEmpty(TempInstaller.InstallerEmail))
                    cmd.Parameters.AddWithValue("@Email", TempInstaller.InstallerEmail);
                else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                if (!string.IsNullOrEmpty(TempInstaller.OSHALevel))
                    cmd.Parameters.AddWithValue("@OshaLevel", TempInstaller.OSHALevel);
                else cmd.Parameters.AddWithValue("@OshaLevel", DBNull.Value);
                if (TempInstaller.CrewID > 0)
                    cmd.Parameters.AddWithValue("@Crew", TempInstaller.CrewID);
                else cmd.Parameters.AddWithValue("@Crew", DBNull.Value);
                if (!TempInstaller.OSHAExpireDate.Equals(DateTime.MinValue))
                    cmd.Parameters.AddWithValue("@OshaDate", TempInstaller.OSHAExpireDate);
                else cmd.Parameters.AddWithValue("@OshaDate", DBNull.Value);
                if (!string.IsNullOrEmpty(TempInstaller.OSHACert))
                    cmd.Parameters.AddWithValue("@OshaCert", TempInstaller.OSHACert);
                else cmd.Parameters.AddWithValue("@OshaCert", DBNull.Value);
                if (!TempInstaller.FirstAidExpireDate.Equals(DateTime.MinValue))
                    cmd.Parameters.AddWithValue("@FacDate", TempInstaller.FirstAidExpireDate);
                else cmd.Parameters.AddWithValue("@FacDate", DBNull.Value);
                if (!string.IsNullOrEmpty(TempInstaller.FirstAidCert))
                    cmd.Parameters.AddWithValue("@FacCert", TempInstaller.FirstAidCert);
                else cmd.Parameters.AddWithValue("@FacCert", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempInstaller.Active);
                cmd.Parameters.AddWithValue("@InstallerID", TempInstaller.ID);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void CreateUser()
        {
            sqlquery = "INSERT INTO tblUsers(User_Name, User_PersonName, User_Level, User_FormOnOpen, User_Email, Active) OUTPUT INSERTED.User_ID VALUES (@UserName, @PersonName, @Level, @FormOnOpen, @Email, @Active)";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(TempUser.UserName))
                    cmd.Parameters.AddWithValue("@UserName", TempUser.UserName);
                else cmd.Parameters.AddWithValue("@UserName", DBNull.Value);
                if (!string.IsNullOrEmpty(TempUser.PersonName))
                    cmd.Parameters.AddWithValue("@PersonName", TempUser.PersonName);
                else cmd.Parameters.AddWithValue("@PersonName", DBNull.Value);
                if (TempUser.Level <= 0)
                    cmd.Parameters.AddWithValue("@Level", TempUser.Level);
                else cmd.Parameters.AddWithValue("@Level", DBNull.Value);
                if (TempUser.FormOnOpen <= 0)
                    cmd.Parameters.AddWithValue("@FormOnOpen", TempUser.FormOnOpen);
                else cmd.Parameters.AddWithValue("@FormOnOpen", DBNull.Value);
                if (!string.IsNullOrEmpty(TempUser.Email))
                    cmd.Parameters.AddWithValue("@Email", TempUser.Email);
                else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempUser.Active);

                try
                {
                    int insertedUserID = (int)cmd.ExecuteScalar();
                    SelectedUserID = insertedUserID;
                    int totalCount = Users.Count;
                    CurrentIndex = totalCount - 2;
                    TempUser.ID = insertedUserID;
                    Users[totalCount - 2] = TempUser;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void UpdateUser()
        {
            int userID = TempUser.ID;
            string userName = TempUser.UserName;
            string personName = TempUser.PersonName;
            int level = TempUser.Level;
            int formOnOpen = TempUser.FormOnOpen;
            string email = TempUser.Email;
            bool active = TempUser.Active;

            sqlquery = "UPDATE tblUsers SET User_Name=@UserName, User_PersonName=@PersonName, User_Level=@Level, User_FormOnOpen=@FormOnOpen, User_Email=@Email, Active=@Active WHERE User_ID=@UserID";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(userName))
                    cmd.Parameters.AddWithValue("@UserName", userName);
                else cmd.Parameters.AddWithValue("@UserName", DBNull.Value);
                if (!string.IsNullOrEmpty(personName))
                    cmd.Parameters.AddWithValue("@PersonName", personName);
                else cmd.Parameters.AddWithValue("@PersonName", DBNull.Value);
                if (level > 0)
                    cmd.Parameters.AddWithValue("@Level", level);
                else cmd.Parameters.AddWithValue("@Level", DBNull.Value);
                if (formOnOpen >= 0)
                    cmd.Parameters.AddWithValue("@FormOnOpen", formOnOpen);
                else cmd.Parameters.AddWithValue("@FormOnOpen", DBNull.Value);
                if (!string.IsNullOrEmpty(email))
                    cmd.Parameters.AddWithValue("@Email", email);
                else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@Active", active);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void CreateFreightCo()
        {
            sqlquery = "INSERT INTO tblFreightCo(FreightCo_Name, FreightCo_Phone, FreightCo_Email, FreightCo_Contact, FreightCo_Cell, Active) OUTPUT INSERTED.FreightCo_ID VALUES (@Name, @Phone, @Email, @Contact, @Cell, @Active)";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(TempFreightCo.FreightName))
                    cmd.Parameters.AddWithValue("@Name", TempFreightCo.FreightName);
                else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                if (!string.IsNullOrEmpty(TempFreightCo.Phone))
                    cmd.Parameters.AddWithValue("@Phone", TempFreightCo.Phone);
                else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                if (!string.IsNullOrEmpty(TempFreightCo.Email))
                    cmd.Parameters.AddWithValue("@Email", TempFreightCo.Email);
                else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                if (!string.IsNullOrEmpty(TempFreightCo.Contact))
                    cmd.Parameters.AddWithValue("@Contact", TempFreightCo.Contact);
                else cmd.Parameters.AddWithValue("@Contact", DBNull.Value);
                if (!string.IsNullOrEmpty(TempFreightCo.Cell))
                    cmd.Parameters.AddWithValue("@Cell", TempFreightCo.Cell);
                else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempFreightCo.Active);

                try
                {
                    int insertedFgtCoID = (int)cmd.ExecuteScalar();
                    SelectedFreightCoID = insertedFgtCoID;
                    int totalCount = FreightCos.Count;
                    CurrentIndex = totalCount - 2;
                    TempFreightCo.ID = insertedFgtCoID;
                    FreightCos[totalCount - 2] = TempFreightCo;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void UpdateFreightCo()
        {
            int freightCoID = TempFreightCo.ID;
            string freightName = TempFreightCo.FreightName;
            string freightPhone = TempFreightCo.Phone;
            string freightEmail = TempFreightCo.Email;
            string freightContact = TempFreightCo.Contact;
            string fregithCell = TempFreightCo.Cell;
            bool active = TempFreightCo.Active;

            sqlquery = "UPDATE tblFreightCo SET FreightCo_Name=@Name, FreightCo_Phone=@Phone, FreightCo_Email=@Email, FreightCo_Contact=@Contact, FreightCo_Cell=@Email, Active=@Active WHERE FreightCo_ID=@FreightCoID";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(TempFreightCo.FreightName))
                    cmd.Parameters.AddWithValue("@Name", TempFreightCo.FreightName);
                else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                if (!string.IsNullOrEmpty(TempFreightCo.Phone))
                    cmd.Parameters.AddWithValue("@Phone", TempFreightCo.Phone);
                else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                if (!string.IsNullOrEmpty(TempFreightCo.Email))
                    cmd.Parameters.AddWithValue("@Email", TempFreightCo.Email);
                else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                if (!string.IsNullOrEmpty(TempFreightCo.Contact))
                    cmd.Parameters.AddWithValue("@Contact", TempFreightCo.Contact);
                else cmd.Parameters.AddWithValue("@Contact", DBNull.Value);
                if (!string.IsNullOrEmpty(TempFreightCo.Cell))
                    cmd.Parameters.AddWithValue("@Cell", TempFreightCo.Cell);
                else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempFreightCo.Active);
                cmd.Parameters.AddWithValue("@FreightCoID", TempFreightCo.ID);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void UpdateArch()
        {
            //if (!string.IsNullOrEmpty(TempCrew.CrewName))
            //{
            int archID = TempArch.ID;
            string archCompany = TempArch.ArchCompany;
            string archContact = TempArch.Contact;
            string archAddress = TempArch.Address;
            string archCity = TempArch.City;
            string archState = TempArch.State;
            string archZip = TempArch.Zip;
            string archPhone = TempArch.Phone;
            string archFax = TempArch.Fax;
            string archCell = TempArch.Cell;
            string archEmail = TempArch.Email;
            bool active = TempArch.Active;

            sqlquery = "UPDATE tblArchitects SET Arch_Company=@Company, Arch_Contact=@Contact, Arch_Address=@Address, Arch_City=@City, Arch_State=@State, Arch_ZIP=@Zip, Arch_Phone=@Phone, Arch_FAX=@Fax, Arch_Cell=@Cell, Arch_Email=@Email, Active=@Active WHERE Architect_ID=@ArchitectID";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(archCompany))
                    cmd.Parameters.AddWithValue("@Company", archCompany);
                else cmd.Parameters.AddWithValue("@Company", DBNull.Value);
                if (!string.IsNullOrEmpty(archContact))
                    cmd.Parameters.AddWithValue("@Contact", archContact);
                else cmd.Parameters.AddWithValue("@Contact", DBNull.Value);
                if (!string.IsNullOrEmpty(archAddress))
                    cmd.Parameters.AddWithValue("@Address", archAddress);
                else cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                if (!string.IsNullOrEmpty(archCity))
                    cmd.Parameters.AddWithValue("@City", archCity);
                else cmd.Parameters.AddWithValue("@City", DBNull.Value);
                if (!string.IsNullOrEmpty(archState))
                    cmd.Parameters.AddWithValue("@State", archState);
                else cmd.Parameters.AddWithValue("@State", DBNull.Value);
                if (!string.IsNullOrEmpty(archZip))
                    cmd.Parameters.AddWithValue("@Zip", archZip);
                else cmd.Parameters.AddWithValue("@Zip", DBNull.Value);
                if (!string.IsNullOrEmpty(archPhone))
                    cmd.Parameters.AddWithValue("@Phone", archPhone);
                else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                if (!string.IsNullOrEmpty(archFax))
                    cmd.Parameters.AddWithValue("@Fax", archFax);
                else cmd.Parameters.AddWithValue("@Fax", DBNull.Value);
                if (!string.IsNullOrEmpty(archCell))
                    cmd.Parameters.AddWithValue("@Cell", archCell);
                else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                if (!string.IsNullOrEmpty(archEmail))
                    cmd.Parameters.AddWithValue("@Email", archEmail);
                else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", active);
                cmd.Parameters.AddWithValue("@ArchitectID", archID);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
            //}
            //else
            //{
            //    MessageBox.Show("Crew Name is required.");
            //}
        }

        public void CreateArch()
        {
            sqlquery = "INSERT INTO tblArchitects(Arch_Company, Arch_Contact, Arch_Address, Arch_City, Arch_State, Arch_ZIP, Arch_Phone, Arch_FAX, Arch_Cell, Arch_Email, Active) OUTPUT INSERTED.Architect_ID VALUES (@Company, @Contact, @Address, @City, @State, @Zip, @Phone, @Fax, @Cell, @Email, @Active)";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(TempArch.ArchCompany))
                    cmd.Parameters.AddWithValue("@Company", TempArch.ArchCompany);
                else cmd.Parameters.AddWithValue("@Company", DBNull.Value);
                if (!string.IsNullOrEmpty(TempArch.Contact))
                    cmd.Parameters.AddWithValue("@Contact", TempArch.Contact);
                else cmd.Parameters.AddWithValue("@Contact", DBNull.Value);
                if (!string.IsNullOrEmpty(TempArch.Address))
                    cmd.Parameters.AddWithValue("@Address", TempArch.Address);
                else cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                if (!string.IsNullOrEmpty(TempArch.City))
                    cmd.Parameters.AddWithValue("@City", TempArch.City);
                else cmd.Parameters.AddWithValue("@City", DBNull.Value);
                if (!string.IsNullOrEmpty(TempArch.State))
                    cmd.Parameters.AddWithValue("@State", TempArch.State);
                else cmd.Parameters.AddWithValue("@State", DBNull.Value);
                if (!string.IsNullOrEmpty(TempArch.Zip))
                    cmd.Parameters.AddWithValue("@Zip", TempArch.Zip);
                else cmd.Parameters.AddWithValue("@Zip", DBNull.Value);
                if (!string.IsNullOrEmpty(TempArch.Phone))
                    cmd.Parameters.AddWithValue("@Phone", TempArch.Phone);
                else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                if (!string.IsNullOrEmpty(TempArch.Fax))
                    cmd.Parameters.AddWithValue("@Fax", TempArch.Fax);
                else cmd.Parameters.AddWithValue("@Fax", DBNull.Value);
                if (!string.IsNullOrEmpty(TempArch.Cell))
                    cmd.Parameters.AddWithValue("@Cell", TempArch.Cell);
                else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                if (!string.IsNullOrEmpty(TempArch.Email))
                    cmd.Parameters.AddWithValue("@Email", TempArch.Email);
                else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempArch.Active);

                try
                {
                    int insertedArchID = (int)cmd.ExecuteScalar();
                    SelectedArchID = insertedArchID;
                    int totalCount = Architects.Count;
                    CurrentIndex = totalCount - 2;
                    TempArch.ID = insertedArchID;
                    Architects[totalCount - 2] = TempArch;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void CreateCrew()
        {
            sqlquery = "INSERT INTO tblInstallCrew(Crew_Name, Crew_Phone, Crew_Cell, Crew_Email, Active) OUTPUT INSERTED.Crew_ID VALUES (@Name, @Phone, @Cell, @Email, @Active)";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(TempCrew.CrewName))
                    cmd.Parameters.AddWithValue("@Name", TempCrew.CrewName);
                else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                if (!string.IsNullOrEmpty(TempCrew.Phone))
                    cmd.Parameters.AddWithValue("@Phone", TempCrew.Phone);
                else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                if (!string.IsNullOrEmpty(TempCrew.Cell))
                    cmd.Parameters.AddWithValue("@Cell", TempCrew.Cell);
                else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                if (!string.IsNullOrEmpty(TempCrew.Email))
                    cmd.Parameters.AddWithValue("@Email", TempCrew.Email);
                else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempCrew.Active);

                try
                {
                    int insertedCrewID = (int)cmd.ExecuteScalar();
                    SelectedCrewID = insertedCrewID;
                    int totalCount = Crews.Count;
                    CurrentIndex = totalCount - 2;
                    TempCrew.ID = insertedCrewID;
                    Crews[totalCount - 2] = TempCrew;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void UpdateCrew()
        {
            //if (!string.IsNullOrEmpty(TempCrew.CrewName))
            //{
            int crewID = TempCrew.ID;
            string crewName = TempCrew.CrewName;
            string crewPhone = TempCrew.Phone;
            string crewCell = TempCrew.Cell;
            string crewEmail = TempCrew.Email;
            bool active = TempCrew.Active;

            sqlquery = "UPDATE tblInstallCrew SET Crew_Name=@Name, Crew_Phone=@Phone, Crew_Cell=@Cell, Crew_Email=@Email, Active=@Active WHERE Crew_ID=@CrewID";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(crewName))
                    cmd.Parameters.AddWithValue("@Name", crewName);
                else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                if (!string.IsNullOrEmpty(crewPhone))
                    cmd.Parameters.AddWithValue("@Phone", crewPhone);
                else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                if (!string.IsNullOrEmpty(crewCell))
                    cmd.Parameters.AddWithValue("@Cell", crewCell);
                else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                if (!string.IsNullOrEmpty(crewName))
                    cmd.Parameters.AddWithValue("@Email", crewEmail);
                else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", active);
                cmd.Parameters.AddWithValue("@CrewID", crewID);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
            //}
            //else
            //{
            //    MessageBox.Show("Crew Name is required.");
            //}
        }

        public void UpdateSalesman()
        {
            if (!string.IsNullOrEmpty(TempSalesman.SalesmanName))
            {
                int salesID = TempSalesman.ID;
                string salesInit = TempSalesman.Init;
                string salesName = TempSalesman.SalesmanName;
                string salesPhone = TempSalesman.Phone;
                string salesCell = TempSalesman.Cell;
                string salesEmail = TempSalesman.Email;
                bool active = TempSalesman.Active;

                sqlquery = "UPDATE tblSalesmen SET Salesman_Init=@Init, Salesman_Name=@Name, Phone=@Phone, Cell=@Cell, Salesman_Email=@Email, Active=@Active WHERE Salesman_ID=@SalesID";
                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (!string.IsNullOrEmpty(salesInit))
                        cmd.Parameters.AddWithValue("@Init", salesInit);
                    else cmd.Parameters.AddWithValue("@Init", DBNull.Value);
                    if (!string.IsNullOrEmpty(salesName))
                        cmd.Parameters.AddWithValue("@Name", salesName);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(salesPhone))
                        cmd.Parameters.AddWithValue("@Phone", salesPhone);
                    else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                    if (!string.IsNullOrEmpty(salesCell))
                        cmd.Parameters.AddWithValue("@Cell", salesCell);
                    else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                    if (!string.IsNullOrEmpty(salesEmail))
                        cmd.Parameters.AddWithValue("@Email", salesEmail);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);
                    cmd.Parameters.AddWithValue("@SalesID", salesID);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            else
            {
                MessageBox.Show("Salesman Name is required.");
            }
        }

        public void CreateSalesman()
        {
            sqlquery = "INSERT INTO tblSalesmen(Salesman_Init, Salesman_Name, Phone, Cell, Salesman_Email, Active) OUTPUT INSERTED.Salesman_ID VALUES (@Init, @Name, @Phone, @Cell, @Email, @Active)";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(TempSalesman.Init))
                    cmd.Parameters.AddWithValue("@Init", TempSalesman.Init);
                else cmd.Parameters.AddWithValue("@Init", DBNull.Value);
                if (!string.IsNullOrEmpty(TempSalesman.SalesmanName))
                    cmd.Parameters.AddWithValue("@Name", TempSalesman.SalesmanName);
                else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                if (!string.IsNullOrEmpty(TempSalesman.Phone))
                    cmd.Parameters.AddWithValue("@Phone", TempSalesman.Phone);
                else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                if (!string.IsNullOrEmpty(TempSalesman.Cell))
                    cmd.Parameters.AddWithValue("@Cell", TempSalesman.Cell);
                else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                if (!string.IsNullOrEmpty(TempSalesman.Email))
                    cmd.Parameters.AddWithValue("@Email", TempSalesman.Email);
                else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempSalesman.Active);

                try
                {
                    int insertedSalesID = (int)cmd.ExecuteScalar();
                    SelectedSalesmanID = insertedSalesID;
                    int totalCount = Salesmans.Count;
                    CurrentIndex = totalCount - 2;
                    TempSalesman.ID = insertedSalesID;
                    Salesmans[totalCount - 2] = TempSalesman;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void CreateLabor()
        {
            sqlquery = "INSERT INTO tblLabor(Labor_Desc, Labor_UnitPrice, Active) OUTPUT INSERTED.Labor_ID VALUES (@LaborDesc, @UnitPrice, @Active)";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(TempLabor.LaborDesc))
                    cmd.Parameters.AddWithValue("@LaborDesc", TempLabor.LaborDesc);
                else cmd.Parameters.AddWithValue("@LaborDesc", DBNull.Value);
                if (TempLabor.UnitPrice >= 0.0)
                    cmd.Parameters.AddWithValue("@UnitPrice", TempLabor.UnitPrice);
                else cmd.Parameters.AddWithValue("@UnitPrice", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempLabor.Active);

                try
                {
                    int insertedMatID = (int)cmd.ExecuteScalar();
                    SelectedLaborID = insertedMatID;
                    int totalCount = Labors.Count;
                    CurrentIndex = totalCount - 2;
                    TempLabor.ID = insertedMatID;
                    Labors[totalCount - 2] = TempLabor;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void UpdateLabor()
        {
            if (!string.IsNullOrEmpty(TempLabor.LaborDesc))
            {
                int laborID = TempLabor.ID;
                string laborDesc = TempLabor.LaborDesc;
                double unitPrice = TempLabor.UnitPrice;
                bool active = TempLabor.Active;

                sqlquery = "UPDATE tblLabor SET Labor_Desc=@LaborDesc, Labor_UnitPrice=@UnitPrice, Active=@Active WHERE Labor_ID=@LaborID";
                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (!string.IsNullOrEmpty(laborDesc))
                        cmd.Parameters.AddWithValue("@LaborDesc", laborDesc);
                    else cmd.Parameters.AddWithValue("@LaborDesc", DBNull.Value);
                    if (TempLabor.UnitPrice >= 0.0)
                        cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                    else cmd.Parameters.AddWithValue("@UnitPrice", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);
                    cmd.Parameters.AddWithValue("@LaborID", laborID);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            else
            {
                MessageBox.Show("Labor Desc is required.");
            }
        }

        public void CreateMaterial()
        {
            sqlquery = "INSERT INTO tblMaterials(Material_Desc, Active) OUTPUT INSERTED.Material_ID VALUES (@MatDesc, @Active)";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(TempMaterial.MatDesc))
                    cmd.Parameters.AddWithValue("MatDesc", TempMaterial.MatDesc);
                else cmd.Parameters.AddWithValue("@MatDesc", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempMaterial.Active);
                try
                {
                    int insertedMatID = (int)cmd.ExecuteScalar();
                    SelectedMaterialID = insertedMatID;
                    int totalCount = Materials.Count;
                    CurrentIndex = totalCount - 2;
                    TempMaterial.ID = insertedMatID;
                    Materials[totalCount - 2] = TempMaterial;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void UpdateMaterial()
        {
            if (!string.IsNullOrEmpty(TempMaterial.MatDesc))
            {
                string matDesc = TempMaterial.MatDesc;
                int matID = TempMaterial.ID;
                bool active = TempMaterial.Active;
                sqlquery = "UPDATE tblMaterials SET Material_Desc=@MatDesc, Active=@Active WHERE Material_ID=@MatID";
                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (!string.IsNullOrEmpty(matDesc))
                        cmd.Parameters.AddWithValue("@MatDesc", matDesc);
                    else cmd.Parameters.AddWithValue("@MatDesc", DBNull.Value);
                    cmd.Parameters.AddWithValue("@MatID", matID);
                    cmd.Parameters.AddWithValue("@Active", active);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            else
            {
                MessageBox.Show("Material is required.");
            }
        }

        public void CreateAcronym()
        {
            if (!string.IsNullOrEmpty(TempAcronym.AcronymName))
            {
                sqlquery = "INSERT INTO tblScheduleOfValues(SOV_Acronym, SOV_Desc, Active) OUTPUT INSERTED.SOV_Acronym VALUES (@SovAcronym, @SovDesc , @Active)";
                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (!string.IsNullOrEmpty(TempAcronym.AcronymName))
                        cmd.Parameters.AddWithValue("@SovAcronym", TempAcronym.AcronymName);
                    else cmd.Parameters.AddWithValue("@SovAcronym", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempAcronym.AcronymDesc))
                        cmd.Parameters.AddWithValue("@SovDesc", TempAcronym.AcronymDesc);
                    else cmd.Parameters.AddWithValue("@SovDesc", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", TempAcronym.Active);
                    try
                    {
                        string insertedSov = (string)cmd.ExecuteScalar();
                        SelectedSovName = insertedSov;
                        int totalCount = Acronyms.Count;
                        CurrentIndex = totalCount - 2;
                        Acronyms[totalCount - 2] = TempAcronym;
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            else
            {
                MessageBox.Show("Sov Name is required.");
            }
        }

        public void UpdateAcronym()
        {
            if (!string.IsNullOrEmpty(SelectedSovName))
            {
                string sovName = TempAcronym.AcronymName;
                string sovDesc = TempAcronym.AcronymDesc;
                bool active = TempAcronym.Active;

                sqlquery = "UPDATE tblScheduleOfValues SET SOV_Acronym=@Name, SOV_Desc=@Desc, Active=@Active WHERE SOV_Acronym=@OriginName";
                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (!string.IsNullOrEmpty(sovName))
                        cmd.Parameters.AddWithValue("@Name", sovName);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(sovDesc))
                        cmd.Parameters.AddWithValue("@Desc", sovDesc);
                    else cmd.Parameters.AddWithValue("@Desc", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedSovName))
                        cmd.Parameters.AddWithValue("@OriginName", SelectedSovName);
                    else cmd.Parameters.AddWithValue("@OriginName", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            else
            {
                MessageBox.Show("Sov Name is required.");
            }
        }

        public void CreateCustCC()
        {
            sqlquery = "INSERT INTO tblCustomerContacts(Customer_ID, CC_Name, CC_Phone, CC_CellPhone, CC_Email, Active) OUTPUT INSERTED.CC_ID VALUES (@CustomerID, @CCName, @CCPhone, @CCCell, @CCEmail, @Active)";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (SelectedCustomerID != 0)
                    cmd.Parameters.AddWithValue("@CustomerID", SelectedCustomerID);
                else cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                if (!string.IsNullOrEmpty(TempCC.CCName))
                    cmd.Parameters.AddWithValue("@CCName", TempCC.CCName);
                else cmd.Parameters.AddWithValue("@CCName", DBNull.Value);
                if (!string.IsNullOrEmpty(TempCC.CCPhone))
                    cmd.Parameters.AddWithValue("@CCPhone", TempCC.CCPhone);
                else cmd.Parameters.AddWithValue("@CCPhone", DBNull.Value);
                if (!string.IsNullOrEmpty(TempCC.CCCell))
                    cmd.Parameters.AddWithValue("@CCCell", TempCC.CCCell);
                else cmd.Parameters.AddWithValue("@CCCell", DBNull.Value);
                if (!string.IsNullOrEmpty(TempCC.CCEmail))
                    cmd.Parameters.AddWithValue("@CCEmail", TempCC.CCEmail);
                else cmd.Parameters.AddWithValue("@CCEmail", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempCC.CCActive);

                try
                {
                    int insertedSubmId = (int)cmd.ExecuteScalar();
                    SelectedCustSubmID = insertedSubmId;
                    TempCC.ID = insertedSubmId;
                    int totalCount = CustContacts.Count;
                    CurrentIndex = totalCount - 2;
                    ActionSubState = "UpdateRow";
                    CustContacts[totalCount - 2] = TempCC;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void UpdateCustCC()
        {
            sqlquery = "UPDATE tblCustomerContacts SET Customer_ID=@CustomerID, CC_Name=@CCName, CC_Phone=@CCPhone, CC_CellPhone=@CCCell, CC_Email=@CCEmail, Active=@Active WHERE CC_ID=@CCID";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                cmd.Parameters.AddWithValue("@CCID", TempCC.ID);
                if (SelectedCustomerID != 0)
                    cmd.Parameters.AddWithValue("@CustomerID", SelectedCustomerID);
                else cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                if (!string.IsNullOrEmpty(TempCC.CCName))
                    cmd.Parameters.AddWithValue("@CCName", TempCC.CCName);
                else cmd.Parameters.AddWithValue("@CCName", DBNull.Value);
                if (!string.IsNullOrEmpty(TempCC.CCPhone))
                    cmd.Parameters.AddWithValue("@CCPhone", TempCC.CCPhone);
                else cmd.Parameters.AddWithValue("@CCPhone", DBNull.Value);
                if (!string.IsNullOrEmpty(TempCC.CCCell))
                    cmd.Parameters.AddWithValue("@CCCell", TempCC.CCCell);
                else cmd.Parameters.AddWithValue("@CCCell", DBNull.Value);
                if (!string.IsNullOrEmpty(TempCC.CCEmail))
                    cmd.Parameters.AddWithValue("@CCEmail", TempCC.CCEmail);
                else cmd.Parameters.AddWithValue("@CCEmail", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempCC.CCActive);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void CreateCustSup()
        {
            sqlquery = "INSERT INTO tblSuperintendents(Customer_ID, Sup_Name, Sup_Phone, Sup_CellPhone, Sup_Email, Active) OUTPUT INSERTED.Sup_ID VALUES (@CustomerID, @SupName, @SupPhone, @SupCell, @SupEmail, @Active)";

            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (SelectedCustomerID != 0)
                    cmd.Parameters.AddWithValue("@CustomerID", SelectedCustomerID);
                else cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                if (!string.IsNullOrEmpty(TempSup.SupName))
                    cmd.Parameters.AddWithValue("@SupName", TempSup.SupName);
                else cmd.Parameters.AddWithValue("@SupName", DBNull.Value);
                if (!string.IsNullOrEmpty(TempSup.SupPhone))
                    cmd.Parameters.AddWithValue("@SupPhone", TempSup.SupPhone);
                else cmd.Parameters.AddWithValue("@SupPhone", DBNull.Value);
                if (!string.IsNullOrEmpty(TempSup.SupCellPhone))
                    cmd.Parameters.AddWithValue("@SupCell", TempSup.SupCellPhone);
                else cmd.Parameters.AddWithValue("@SupCell", DBNull.Value);
                if (!string.IsNullOrEmpty(TempSup.SupEmail))
                    cmd.Parameters.AddWithValue("@SupEmail", TempSup.SupEmail);
                else cmd.Parameters.AddWithValue("@SupEmail", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempSup.Active);

                try
                {
                    int insertedSupId = (int)cmd.ExecuteScalar();
                    SelectedCustSupID = insertedSupId;
                    TempSup.SupID = insertedSupId;
                    int totalCount = CustSupts.Count;
                    CurrentIndex = totalCount - 2;
                    ActionSubState = "UpdateRow";
                    CustSupts[totalCount - 2] = TempSup;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void UpdateCustSup()
        {
            sqlquery = "UPDATE tblSuperintendents SET Customer_ID=@CustomerID, Sup_Name=@SupName, Sup_Phone=@SupPhone, Sup_CellPhone=@SupCell, Sup_Email=@SupEmail, Active=@Active WHERE Sup_ID=@SupID";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                cmd.Parameters.AddWithValue("@SupID", TempSup.SupID);
                if (SelectedCustomerID != 0)
                    cmd.Parameters.AddWithValue("@CustomerID", SelectedCustomerID);
                else cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                if (!string.IsNullOrEmpty(TempSup.SupName))
                    cmd.Parameters.AddWithValue("@SupName", TempSup.SupName);
                else cmd.Parameters.AddWithValue("@SupName", DBNull.Value);
                if (!string.IsNullOrEmpty(TempSup.SupPhone))
                    cmd.Parameters.AddWithValue("@SupPhone", TempSup.SupPhone);
                else cmd.Parameters.AddWithValue("@SupPhone", DBNull.Value);
                if (!string.IsNullOrEmpty(TempSup.SupCellPhone))
                    cmd.Parameters.AddWithValue("@SupCell", TempSup.SupCellPhone);
                else cmd.Parameters.AddWithValue("@SupCell", DBNull.Value);
                if (!string.IsNullOrEmpty(TempSup.SupEmail))
                    cmd.Parameters.AddWithValue("@SupEmail", TempSup.SupEmail);
                else cmd.Parameters.AddWithValue("@SupEmail", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempSup.Active);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void CreateCustPM()
        {
            sqlquery = "INSERT INTO tblProjectManagers(Customer_ID, PM_Name, PM_Phone, PM_CellPhone, PM_Email, Active) OUTPUT INSERTED.PM_ID VALUES (@CustomerID, @PmName, @PmPhone, @PmCell, @PmEmail, @Active)";

            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (SelectedCustomerID != 0)
                    cmd.Parameters.AddWithValue("@CustomerID", SelectedCustomerID);
                else cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                if (!string.IsNullOrEmpty(TempPM.PMName))
                    cmd.Parameters.AddWithValue("@PmName", TempPM.PMName);
                else cmd.Parameters.AddWithValue("@PmName", DBNull.Value);
                if (!string.IsNullOrEmpty(TempPM.PMPhone))
                    cmd.Parameters.AddWithValue("@PmPhone", TempPM.PMPhone);
                else cmd.Parameters.AddWithValue("@PmPhone", DBNull.Value);
                if (!string.IsNullOrEmpty(TempPM.PMCellPhone))
                    cmd.Parameters.AddWithValue("@PmCell", TempPM.PMCellPhone);
                else cmd.Parameters.AddWithValue("@PmCell", DBNull.Value);
                if (!string.IsNullOrEmpty(TempPM.PMEmail))
                    cmd.Parameters.AddWithValue("@PmEmail", TempPM.PMEmail);
                else cmd.Parameters.AddWithValue("@PmEmail", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempPM.Active);

                try
                {
                    int insertedPMId = (int)cmd.ExecuteScalar();
                    SelectedCustPmID = insertedPMId;
                    TempPM.ID = insertedPMId;
                    int totalCount = CustPMs.Count;
                    CurrentIndex = totalCount - 2;
                    ActionSubState = "UpdateRow";
                    CustPMs[totalCount - 2] = TempPM;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void UpdateCustPM()
        {
            sqlquery = "UPDATE tblProjectManagers SET Customer_ID=@CustomerID, PM_Name=@PmName, PM_Phone=@PmPhone, PM_CellPhone=@PmCell, PM_Email=@PmEmail, Active=@Active WHERE PM_ID=@PmID";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                cmd.Parameters.AddWithValue("@PmID", TempPM.ID);
                if (SelectedCustomerID != 0)
                    cmd.Parameters.AddWithValue("@CustomerID", SelectedCustomerID);
                else cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                if (!string.IsNullOrEmpty(TempPM.PMName))
                    cmd.Parameters.AddWithValue("@PmName", TempPM.PMName);
                else cmd.Parameters.AddWithValue("@PmName", DBNull.Value);
                if (!string.IsNullOrEmpty(TempPM.PMPhone))
                    cmd.Parameters.AddWithValue("@PmPhone", TempPM.PMPhone);
                else cmd.Parameters.AddWithValue("@PmPhone", DBNull.Value);
                if (!string.IsNullOrEmpty(TempPM.PMCellPhone))
                    cmd.Parameters.AddWithValue("@PmCell", TempPM.PMCellPhone);
                else cmd.Parameters.AddWithValue("@PmCell", DBNull.Value);
                if (!string.IsNullOrEmpty(TempPM.PMEmail))
                    cmd.Parameters.AddWithValue("@PmEmail", TempPM.PMEmail);
                else cmd.Parameters.AddWithValue("@PmEmail", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", TempPM.Active);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void UpdateManuf()
        {
            int manufID = (UpdateComponent.Equals("Table")) ? TempManuf.ID : TempDetailManuf.ID;
            string manufName = (UpdateComponent.Equals("Table")) ? TempManuf.ManufacturerName : TempDetailManuf.ManufacturerName;
            string address = (UpdateComponent.Equals("Table")) ? TempManuf.Address : TempDetailManuf.Address;
            string address2 = (UpdateComponent.Equals("Table")) ? TempManuf.Address2 : TempDetailManuf.Address2;
            string city = (UpdateComponent.Equals("Table")) ? TempManuf.City : TempDetailManuf.City;
            string state = (UpdateComponent.Equals("Table")) ? TempManuf.State : TempDetailManuf.State;
            string zip = (UpdateComponent.Equals("Table")) ? TempManuf.Zip : TempDetailManuf.Zip;
            string phone = (UpdateComponent.Equals("Table")) ? TempManuf.Phone : TempDetailManuf.Phone;
            string fax = (UpdateComponent.Equals("Table")) ? TempManuf.Fax : TempDetailManuf.Fax;
            string contactName = (UpdateComponent.Equals("Table")) ? TempManuf.ContactName : TempDetailManuf.ContactName;
            string contactPhone = (UpdateComponent.Equals("Table")) ? TempManuf.ContactPhone : TempDetailManuf.ContactPhone;
            string contactEmail = (UpdateComponent.Equals("Table")) ? TempManuf.ContactEmail : TempDetailManuf.ContactEmail;
            bool active = (UpdateComponent.Equals("Table")) ? TempManuf.Active : TempDetailManuf.Active;

            sqlquery = "UPDATE tblManufacturers SET Manuf_Name=@ManufName, Address=@Address, Address2=@Address2, City=@City, State=@State, ZIP=@Zip, Phone=@Phone, FAX=@Fax, Contact_Name=@ContactName, Contact_Phone=@ContactPhone, Contact_Email=@ContactEmail, Active=@Active WHERE Manuf_ID=@ManufID";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(manufName))
                    cmd.Parameters.AddWithValue("@ManufName", manufName);
                else cmd.Parameters.AddWithValue("@ManufName", DBNull.Value);
                if (!string.IsNullOrEmpty(address))
                    cmd.Parameters.AddWithValue("@Address", address);
                else cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                if (!string.IsNullOrEmpty(address2))
                    cmd.Parameters.AddWithValue("@Address2", address2);
                else cmd.Parameters.AddWithValue("@Address2", DBNull.Value);
                if (!string.IsNullOrEmpty(city))
                    cmd.Parameters.AddWithValue("@City", city);
                else cmd.Parameters.AddWithValue("@City", DBNull.Value);
                if (!string.IsNullOrEmpty(state))
                    cmd.Parameters.AddWithValue("@State", state);
                else cmd.Parameters.AddWithValue("@State", DBNull.Value);
                if (!string.IsNullOrEmpty(zip))
                    cmd.Parameters.AddWithValue("@Zip", zip);
                else cmd.Parameters.AddWithValue("@Zip", DBNull.Value);
                if (!string.IsNullOrEmpty(phone))
                    cmd.Parameters.AddWithValue("@Phone", phone);
                else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                if (!string.IsNullOrEmpty(fax))
                    cmd.Parameters.AddWithValue("@Fax", fax);
                else cmd.Parameters.AddWithValue("@Fax", DBNull.Value);
                if (!string.IsNullOrEmpty(contactName))
                    cmd.Parameters.AddWithValue("@ContactName", contactName);
                else cmd.Parameters.AddWithValue("@ContactName", DBNull.Value);
                if (!string.IsNullOrEmpty(contactPhone))
                    cmd.Parameters.AddWithValue("@ContactPhone", contactPhone);
                else cmd.Parameters.AddWithValue("@ContactPhone", DBNull.Value);
                if (!string.IsNullOrEmpty(contactEmail))
                    cmd.Parameters.AddWithValue("@ContactEmail", contactEmail);
                else cmd.Parameters.AddWithValue("@ContactEmail", DBNull.Value);
                if (manufID != 0)
                    cmd.Parameters.AddWithValue("@ManufID", manufID);
                else cmd.Parameters.AddWithValue("@ManufID", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", active);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();

                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
                ActionManufState = "";
            }
        }

        public void CreateManuf()
        {
            if (!string.IsNullOrEmpty(TempCreateManuf.ManufacturerName))
            {
                sqlquery = "INSERT INTO tblManufacturers(Manuf_Name, Address, Address2, City, State, ZIP, Phone, FAX, Contact_Name, Contact_Phone, Contact_Email, Active) OUTPUT INSERTED.Manuf_ID VALUES (@ManufName, @Address, @Address2, @City, @State, @Zip, @Phone, @Fax, @ContactName, @ContactPhone, @ContactEmail, @Active)";

                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (!string.IsNullOrEmpty(TempCreateManuf.ManufacturerName))
                        cmd.Parameters.AddWithValue("@ManufName", TempCreateManuf.ManufacturerName);
                    else cmd.Parameters.AddWithValue("@ManufName", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateManuf.Address))
                        cmd.Parameters.AddWithValue("@Address", TempCreateManuf.Address);
                    else cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateManuf.Address2))
                        cmd.Parameters.AddWithValue("@Address2", TempCreateManuf.Address2);
                    else cmd.Parameters.AddWithValue("@Address2", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateManuf.City))
                        cmd.Parameters.AddWithValue("@City", TempCreateManuf.City);
                    else cmd.Parameters.AddWithValue("@City", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateManuf.State))
                        cmd.Parameters.AddWithValue("@State", TempCreateManuf.State);
                    else cmd.Parameters.AddWithValue("@State", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateManuf.Zip))
                        cmd.Parameters.AddWithValue("@Zip", TempCreateManuf.Zip);
                    else cmd.Parameters.AddWithValue("@Zip", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateManuf.Phone))
                        cmd.Parameters.AddWithValue("@Phone", TempCreateManuf.Phone);
                    else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateManuf.Fax))
                        cmd.Parameters.AddWithValue("@Fax", TempCreateManuf.Fax);
                    else cmd.Parameters.AddWithValue("@Fax", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateManuf.ContactName))
                        cmd.Parameters.AddWithValue("@ContactName", TempCreateManuf.ContactName);
                    else cmd.Parameters.AddWithValue("@ContactName", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateManuf.ContactPhone))
                        cmd.Parameters.AddWithValue("@ContactPhone", TempCreateManuf.ContactPhone);
                    else cmd.Parameters.AddWithValue("@ContactPhone", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateManuf.ContactEmail))
                        cmd.Parameters.AddWithValue("@ContactEmail", TempCreateManuf.ContactEmail);
                    else cmd.Parameters.AddWithValue("@ContactEmail", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", TempCreateManuf.Active);

                    try
                    {
                        int insertedManufId = (int)cmd.ExecuteScalar();
                        SelectedManufID = insertedManufId;
                        TempDetailManuf.ID = insertedManufId;
                        UpdateComponent = "Detail";
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e);
                    }
                    ActionManufState = "";
                }
            }
            else
            {
                if (!ActionManufState.Equals("ClearManuf"))
                    MessageBox.Show("Manufacturer Name is required.");
            }
        }

        public void UpdateNote()
        {
            sqlquery = "UPDATE tblNotes SET Notes_Note=@NotesNote, Notes_DateAdded=@NotesDateAdded, Notes_User=@NotesUser WHERE Notes_ID=@NotesID";

            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(TempNote.NotesNote))
                    cmd.Parameters.AddWithValue("@NotesNote", TempNote.NotesNote);
                else cmd.Parameters.AddWithValue("@NotesNote", DBNull.Value);
                if (!TempNote.NotesDateAdded.Equals(DateTime.MinValue))
                    cmd.Parameters.AddWithValue("@NotesDateAdded", TempNote.NotesDateAdded);
                else cmd.Parameters.AddWithValue("@NotesDateAdded", DBNull.Value);
                if (!string.IsNullOrEmpty(TempNote.NoteUser))
                    cmd.Parameters.AddWithValue("@NotesUser", TempNote.NoteUser);
                else cmd.Parameters.AddWithValue("@NotesUser", DBNull.Value);
                if (TempNote.NoteID != 0)
                    cmd.Parameters.AddWithValue("@NotesID", TempNote.NoteID);
                else cmd.Parameters.AddWithValue("@NotesID", DBNull.Value);
            }
            int rowsAffected = cmd.ExecuteNonQuery();
        }

        public void CreateNote()
        {
            sqlquery = "INSERT INTO tblNotes(Notes_PK, Notes_PK_Desc, Notes_Note, Notes_DateAdded, Notes_User) OUTPUT INSERTED.Notes_ID VALUES (@NotesPK, @NotesDesc, @NotesNote, @NotesDateAdded, @NotesUser)";
            int notesPK = 0;
            ObservableCollection<Note> notes = new ObservableCollection<Note>();
          
            switch (TempNote.NotesPKDesc)
            {
                case "Customer":
                    notesPK = SelectedCustomerID;
                    notes = CustomerNotes;
                    break;
                case "ProjectManager":
                    notesPK = SelectedCustPmID;
                    notes = CustPMNotes;
                    break;
                case "Superintendent":
                    notesPK = SelectedCustSupID;
                    notes = CustSuptNotes;
                    break;
                case "CustomerContact":
                    notesPK = SelectedCustSubmID;
                    notes = CustSubmNotes;
                    break;
                case "Manuf":
                    notesPK = SelectedManufID;
                    notes = ManufNotes;
                    break;
            }
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (notesPK != 0)
                    cmd.Parameters.AddWithValue("@NotesPK", notesPK);
                else cmd.Parameters.AddWithValue("@NotesPK", DBNull.Value);
                if (!string.IsNullOrEmpty(TempNote.NotesPKDesc))
                    cmd.Parameters.AddWithValue("@NotesDesc", TempNote.NotesPKDesc);
                else cmd.Parameters.AddWithValue("@NotesDesc", DBNull.Value);
                if (!string.IsNullOrEmpty(TempNote.NotesNote))
                    cmd.Parameters.AddWithValue("@NotesNote", TempNote.NotesNote);
                else cmd.Parameters.AddWithValue("@NotesNote", DBNull.Value);
                if (!TempNote.NotesDateAdded.Equals(DateTime.MinValue))
                    cmd.Parameters.AddWithValue("@NotesDateAdded", TempNote.NotesDateAdded);
                else cmd.Parameters.AddWithValue("@NotesDateAdded", DBNull.Value);
                if (!string.IsNullOrEmpty(TempNote.NoteUser))
                    cmd.Parameters.AddWithValue("@NotesUser", TempNote.NoteUser);
                else cmd.Parameters.AddWithValue("@NotesUser", DBNull.Value);

                try
                {
                    int insertedNoteId = (int)cmd.ExecuteScalar();
                    CurrentNoteID = insertedNoteId;
                    TempNote.NoteID = insertedNoteId;
                    TempNote.NotePK = notesPK;

                    notes[notes.Count - 2] = TempNote;
                    ActionNoteState = "UpdateRow";
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void UpdateCustomer()
        {
            int customerID = (UpdateComponent.Equals("Table")) ? TempCustomer.ID : TempDetailCustomer.ID;
            string fullName = (UpdateComponent.Equals("Table")) ? TempCustomer.FullName : TempDetailCustomer.FullName;
            string shortName = (UpdateComponent.Equals("Table")) ? TempCustomer.ShortName : TempDetailCustomer.ShortName;
            string poNumber = (UpdateComponent.Equals("Table")) ? TempCustomer.PoBox : TempDetailCustomer.PoBox;
            string address = (UpdateComponent.Equals("Table")) ? TempCustomer.Address : TempDetailCustomer.Address;
            string city = (UpdateComponent.Equals("Table")) ? TempCustomer.City : TempDetailCustomer.City;
            string state = (UpdateComponent.Equals("Table")) ? TempCustomer.State : TempDetailCustomer.State;
            string zip = (UpdateComponent.Equals("Table")) ? TempCustomer.Zip : TempDetailCustomer.Zip;
            string phone = (UpdateComponent.Equals("Table")) ? TempCustomer.Phone : TempDetailCustomer.Phone;
            string fax = (UpdateComponent.Equals("Table")) ? TempCustomer.Fax : TempDetailCustomer.Fax;
            string email = (UpdateComponent.Equals("Table")) ? TempCustomer.Email : TempDetailCustomer.Email;
            bool active = (UpdateComponent.Equals("Table")) ? TempCustomer.Active : TempDetailCustomer.Active;

            if (!string.IsNullOrEmpty(fullName))
            {
                sqlquery = "UPDATE tblCustomers SET Short_Name=@ShortName, Full_Name=@FullName, PO_Box=@PoNumber, Address=@Address, City=@City, State=@State, ZIP=@Zip, Phone=@Phone, FAX=@Fax, Email=@Email, Active=@Active WHERE Customer_ID=@CustomerID";
               
                for (int i = 0; i < Customers.Count - 1; i++)
                {
                   Customer _customer = Customers[i];
                   if (_customer.ID == customerID)
                   {
                       Console.WriteLine("customerID->" + customerID);
                       Customers[i] = _customer;
                   }
                }

                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (!string.IsNullOrEmpty(fullName))
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                    if (!string.IsNullOrEmpty(shortName))
                        cmd.Parameters.AddWithValue("@ShortName", shortName);
                    else cmd.Parameters.AddWithValue("@ShortName", DBNull.Value);
                    if (!string.IsNullOrEmpty(poNumber))
                        cmd.Parameters.AddWithValue("@PoNumber", poNumber);
                    else cmd.Parameters.AddWithValue("@PoNumber", DBNull.Value);
                    if (!string.IsNullOrEmpty(address))
                        cmd.Parameters.AddWithValue("@Address", address);
                    else cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                    if (!string.IsNullOrEmpty(city))
                        cmd.Parameters.AddWithValue("@City", city);
                    else cmd.Parameters.AddWithValue("@City", DBNull.Value);
                    if (!string.IsNullOrEmpty(state))
                        cmd.Parameters.AddWithValue("@State", state);
                    else cmd.Parameters.AddWithValue("@State", DBNull.Value);
                    if (!string.IsNullOrEmpty(zip))
                        cmd.Parameters.AddWithValue("@Zip", zip);
                    else cmd.Parameters.AddWithValue("@Zip", DBNull.Value);
                    if (!string.IsNullOrEmpty(phone))
                        cmd.Parameters.AddWithValue("@Phone", phone);
                    else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                    if (!string.IsNullOrEmpty(fax))
                        cmd.Parameters.AddWithValue("@Fax", fax);
                    else cmd.Parameters.AddWithValue("@Fax", DBNull.Value);
                    if (!string.IsNullOrEmpty(email))
                        cmd.Parameters.AddWithValue("@Email", email);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    ActionCustName = "UpdateCustomer";
                }
                ActionCustState = "";
            }
            else
            {
                if (!ActionCustState.Equals("Clear"))
                    MessageBox.Show("Customer Full Name is required");
            }
        }

        public void CreateCustomer()
        {
            if (!string.IsNullOrEmpty(TempCreateCustomer.FullName))
            {
                sqlquery = "INSERT INTO tblCustomers(Short_Name, Full_Name, PO_Box, Address, City, State, ZIP, Phone, FAX, Email, Active) OUTPUT INSERTED.Customer_ID VALUES (@ShortName, @FullName, @PoNumber, @Address, @City, @State, @Zip, @Phone, @Fax, @Email, @Active)";

                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (!string.IsNullOrEmpty(TempCreateCustomer.FullName))
                        cmd.Parameters.AddWithValue("@FullName", TempCreateCustomer.FullName);
                    else cmd.Parameters.AddWithValue("@FullName", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateCustomer.ShortName))
                        cmd.Parameters.AddWithValue("@ShortName", TempCreateCustomer.ShortName);
                    else cmd.Parameters.AddWithValue("@ShortName", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateCustomer.PoBox))
                        cmd.Parameters.AddWithValue("@PoNumber", TempCreateCustomer.PoBox);
                    else cmd.Parameters.AddWithValue("@PoNumber", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateCustomer.Address))
                        cmd.Parameters.AddWithValue("@Address", TempCreateCustomer.Address);
                    else cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateCustomer.City))
                        cmd.Parameters.AddWithValue("@City", TempCreateCustomer.City);
                    else cmd.Parameters.AddWithValue("@City", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateCustomer.State))
                        cmd.Parameters.AddWithValue("@State", TempCreateCustomer.State);
                    else cmd.Parameters.AddWithValue("@State", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateCustomer.Zip))
                        cmd.Parameters.AddWithValue("@Zip", TempCreateCustomer.Zip);
                    else cmd.Parameters.AddWithValue("@Zip", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateCustomer.Phone))
                        cmd.Parameters.AddWithValue("@Phone", TempCreateCustomer.Phone);
                    else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateCustomer.Fax))
                        cmd.Parameters.AddWithValue("@Fax", TempCreateCustomer.Fax);
                    else cmd.Parameters.AddWithValue("@Fax", DBNull.Value);
                    if (!string.IsNullOrEmpty(TempCreateCustomer.Email))
                        cmd.Parameters.AddWithValue("@Email", TempCreateCustomer.Email);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", TempCreateCustomer.Active);

                    try
                    {
                        int insertedCustomerId = (int)cmd.ExecuteScalar();
                        SelectedCustomerID = insertedCustomerId;
                        TempDetailCustomer = TempCreateCustomer;
                        TempDetailCustomer.ID = SelectedCustomerID;
                        UpdateComponent = "Detail";
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            else
            {
                if (!ActionCustState.Equals("Clear"))
                   MessageBox.Show("Customer Full Name is required");
            }
        }

        public void ClearManuf()
        {
            ActionManufName = "CreateManuf";
            ActionManufState = "ClearManuf";
            UpdateComponent = "Detail";
            TempDetailManuf = new Manufacturer();
            SelectedManufID = -1;
            Note noteItem = new Note();
            ManufNotes = new ObservableCollection<Note>();
            ManufNotes.Add(noteItem);
        }

        public void ClearCustomer()
        {
            ActionCustState = "Clear";
            SelectedCustomerID = 0;
            UpdateComponent = "Detail";
            Note noteItem = new Note();
            CustomerNotes = new ObservableCollection<Note>();
            CustomerNotes.Add(noteItem);
            SelectedCustomerID = -1;
            TempDetailCustomer = new Customer();

            CustPMNotes = new ObservableCollection<Note>();
            CustPMNotes.Add(noteItem);

            CustSuptNotes = new ObservableCollection<Note>();
            CustSuptNotes.Add(noteItem);

            CustSubmNotes = new ObservableCollection<Note>();
            CustSubmNotes.Add(noteItem);

            ProjectManager projectManager = new ProjectManager();
            CustPMs = new ObservableCollection<ProjectManager>();
            CustPMs.Add(projectManager);

            CustomerContact customerContact = new CustomerContact();
            CustContacts = new ObservableCollection<CustomerContact>();
            CustContacts.Add(customerContact);

            Superintendent superintendent = new Superintendent();
            CustSupts = new ObservableCollection<Superintendent>();
            CustSupts.Add(superintendent);
        }

        public void ViewManuf(int manufID)
        {
            sqlquery = "SELECT * FROM tblManufacturers WHERE Manuf_ID=" + manufID;
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            DataRow firstRow = ds.Tables[0].Rows[0];
            SelectedManufID = int.Parse(firstRow["Manuf_ID"].ToString());
            ActionManufName = "UpdateManuf";
            if (!firstRow.IsNull("Manuf_Name"))
                TempDetailManuf.ManufacturerName = firstRow["Manuf_Name"].ToString();
            else TempDetailManuf.ManufacturerName = "";
            if (!firstRow.IsNull("Address"))
                TempDetailManuf.Address = firstRow["Address"].ToString();
            else TempDetailManuf.Address = "";
            if (!firstRow.IsNull("Address2"))
                TempDetailManuf.Address2 = firstRow["Address2"].ToString();
            else TempDetailManuf.Address2 = "";
            if (!firstRow.IsNull("City"))
                TempDetailManuf.City = firstRow["City"].ToString();
            else TempDetailManuf.City = "";
            if (!firstRow.IsNull("State"))
                TempDetailManuf.State = firstRow["State"].ToString();
            else TempDetailManuf.State = "";
            if (!firstRow.IsNull("ZIP"))
                TempDetailManuf.Zip = firstRow["ZIP"].ToString();
            else TempDetailManuf.Zip = "";
            if (!firstRow.IsNull("Phone"))
                TempDetailManuf.Phone = firstRow["Phone"].ToString();
            else TempDetailManuf.Phone = "";
            if (!firstRow.IsNull("FAX"))
                TempDetailManuf.Fax = firstRow["FAX"].ToString();
            else TempDetailManuf.Fax = "";
            if (!firstRow.IsNull("Contact_Name"))
                TempDetailManuf.ContactName = firstRow["Contact_Name"].ToString();
            else TempDetailManuf.ContactName = "";
            if (!firstRow.IsNull("Contact_Phone"))
                TempDetailManuf.ContactPhone = firstRow["Contact_Phone"].ToString();
            else TempDetailManuf.ContactPhone = "";
            if (!firstRow.IsNull("Contact_Email"))
                TempDetailManuf.ContactEmail = firstRow["Contact_Email"].ToString();
            else TempDetailManuf.ContactEmail = "";
            TempDetailManuf.Active = firstRow.Field<Boolean>("Active");

            TempDetailManuf.ID = manufID;

            sqlquery = "SELECT * FROM tblNotes WHERE Notes_PK_Desc='Manuf' AND Notes_PK=" + manufID;
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Note> st_manufNotes = new ObservableCollection<Note>();
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
                st_manufNotes.Add(new Note
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
            st_manufNotes.Add(new Note());
            ManufNotes = st_manufNotes;
        }

        public void ViewCustomer(int customerID)
        {
            sqlquery = "SELECT * FROM tblCustomers WHERE Customer_ID=" + customerID;
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            UpdateComponent = "Detail";

            DataRow firstRow = ds.Tables[0].Rows[0];
            SelectedCustomerID = customerID;
            ActionState = "UpdateRow";
            ActionSubState = "";
            ActionNoteState = "";

            TempDetailCustomer.ID = int.Parse(firstRow["Customer_ID"].ToString());
            if (!firstRow.IsNull("Full_Name"))
                TempDetailCustomer.FullName = firstRow["Full_Name"].ToString();
            if (!firstRow.IsNull("Short_Name"))
                TempDetailCustomer.ShortName = firstRow["Short_Name"].ToString();
            else TempDetailCustomer.ShortName = "";
            if (!firstRow.IsNull("PO_Box"))
                TempDetailCustomer.PoBox = firstRow["PO_Box"].ToString();
            else TempDetailCustomer.PoBox = "";
            if (!firstRow.IsNull("Address"))
                TempDetailCustomer.Address = firstRow["Address"].ToString();
            else TempDetailCustomer.Address = "";
            if (!firstRow.IsNull("City"))
                TempDetailCustomer.City = firstRow["City"].ToString();
            else TempDetailCustomer.City = "";
            if (!firstRow.IsNull("State"))
                TempDetailCustomer.State = firstRow["State"].ToString();
            else TempDetailCustomer.State = "";
            if (!firstRow.IsNull("ZIP"))
                TempDetailCustomer.Zip = firstRow["ZIP"].ToString();
            else TempDetailCustomer.Zip = "";
            if (!firstRow.IsNull("Phone"))
                TempDetailCustomer.Phone = firstRow["Phone"].ToString();
            else TempDetailCustomer.Phone = "";
            if (!firstRow.IsNull("FAX"))
                TempDetailCustomer.Fax = firstRow["FAX"].ToString();
            else TempDetailCustomer.Fax = "";
            if (!firstRow.IsNull("Email"))
                TempDetailCustomer.Email = firstRow["Email"].ToString();
            else TempDetailCustomer.Email = "";
            TempDetailCustomer.Active = firstRow.Field<Boolean>("Active");
            
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

            // Project Managers for customers
            sqlquery = "select * from tblProjectManagers WHERE Customer_ID=" + customerID;
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                firstRow = ds.Tables[0].Rows[0];
                SelectedCustPmID = int.Parse(firstRow["PM_ID"].ToString());
            }
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

            // Project Manager Notes
            sqlquery = "SELECT * FROM tblNotes INNER JOIN (SELECT * FROM tblProjectManagers WHERE Customer_ID = " + customerID + ") AS tblProjManager ON tblNotes.Notes_PK = tblProjManager.PM_ID WHERE Notes_PK_Desc = 'ProjectManager'";
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
            CustPMNotes = st_pmNotes;

            // Superintendents Notes
            sqlquery = "SELECT * FROM tblNotes INNER JOIN (SELECT * FROM tblSuperintendents WHERE Customer_ID = " + customerID + ") AS tblSupt ON tblNotes.Notes_PK = tblSupt.Sup_ID WHERE Notes_PK_Desc = 'Superintendent'";
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
            CustSuptNotes = st_suptNotes;

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
            CustSubmNotes = st_ccNotes;

            // Superintendents
            sqlquery = "SELECT * FROM tblSuperintendents WHERE Customer_ID=" + customerID;
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                firstRow = ds.Tables[0].Rows[0];
                SelectedCustSupID = int.Parse(firstRow["Sup_ID"].ToString());
            }
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

            // Customer Contacts
            sqlquery = "SELECT * FROM tblCustomerContacts WHERE Customer_ID=" + customerID;
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                firstRow = ds.Tables[0].Rows[0];
                SelectedCustSubmID = int.Parse(firstRow["CC_ID"].ToString());
            }
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

            st_acronym.Add(new Acronym());
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

            st_material.Add(new Material());
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
                double unitPrice = 0.0;
                if (!row.IsNull("Labor_UnitPrice"))
                    unitPrice = row.Field<double>("Labor_UnitPrice");
                //double unitPrice = 0.0;
                bool active = row.Field<Boolean>("Active");

                st_labor.Add(new Labor { ID = laborID, LaborDesc = laborDesc, UnitPrice = unitPrice, Active = active });
            }

            st_labor.Add(new Labor());
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

                st_salesman.Add(new Salesman { ID = salesmanID, Init = init, SalesmanName = name, Phone = phone, Cell = cell, Email = email, Active = active });
            }

            st_salesman.Add(new Salesman());
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

            st_crew.Add(new Crew());
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
                int formOnOpen = -1;
                int level = 0;
                int id = int.Parse(row["User_ID"].ToString());
                string name = row["User_Name"].ToString();
                string personName = row["User_PersonName"].ToString();
                if (!row.IsNull("User_Level"))
                    level = int.Parse(row["User_Level"].ToString());
                if (!row.IsNull("User_FormOnOpen"))
                    formOnOpen = int.Parse(row["User_FormOnOpen"].ToString());
                string email = row["User_Email"].ToString();
                bool active = row.Field<Boolean>("Active");

                st_user.Add(new User { ID = id, UserName = name, PersonName = personName, Level = level, FormOnOpen = formOnOpen, Email = email, Active = active });
            }

            st_user.Add(new User());
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

            st_architect.Add(new Architect());
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

            st_freightCO.Add(new FreightCo());
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

            st_installer.Add(new InHouseInstaller());
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

            // FormOnOpen
            sqlquery = "SELECT * FROM tblApplOptions WHERE OptCat = 'FormOnOpen';";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ApplOption> st_formOnOpens = new ObservableCollection<ApplOption>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int optID = int.Parse(row["Option_ID"].ToString());
                string optCat = row["OptCat"].ToString();
                string optTxtVal = row["OptTxtVal"].ToString();
                string optNumVal = row["OptNumVal"].ToString();
                string optDesc = row["OptDesc"].ToString();
                string optLong = row["OptLong"].ToString();

                st_formOnOpens.Add(new ApplOption { OptID = optID, OptCat = optCat, OptTxtVal = optTxtVal, OptNumVal = optNumVal, OptDesc = optDesc, OptLong = optLong });
            }
            FormOnOpens = st_formOnOpens;

            // OHSA Level
            sqlquery = "SELECT DISTINCT OSHA_Level FROM tblInHouseInstallers;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<OSHA> st_oshas = new ObservableCollection<OSHA>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string levelName = row["OSHA_Level"].ToString();

                st_oshas.Add(new OSHA { Name = levelName });
            }
            OSHAs = st_oshas;
            //cmd.Dispose();
            //dbConnection.Close();
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

        private ObservableCollection<ApplOption> _formOnOpens;

        public ObservableCollection<ApplOption> FormOnOpens
        {
            get { return _formOnOpens; }
            set
            {
                _formOnOpens = value;
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

        private ObservableCollection<Note> _custPmNotes;

        public ObservableCollection<Note> CustPMNotes
        {
            get { return _custPmNotes; }
            set
            {
                _custPmNotes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Note> _custSubmNotes;

        public ObservableCollection<Note> CustSubmNotes
        {
            get { return _custSubmNotes; }
            set
            {
                _custSubmNotes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Note> _custSuptNotes;

        public ObservableCollection<Note> CustSuptNotes
        {
            get { return _custSuptNotes; }
            set
            {
                _custSuptNotes = value;
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

        private ObservableCollection<OSHA> _osha;

        public ObservableCollection<OSHA> OSHAs
        {
            get { return _osha; }
            set
            {
                _osha = value;
                OnPropertyChanged();
            }
        }

        public int SelectedPMRowIndex { get; set; }

        public int SelectedAcronymRowIndex { get; set; }

        public int SelectedSubmRowIndex { get; set; }

        public int SelectedSuptRowIndex { get; set; }

        public int SelectedSubmNoteRowIndex { get; set; }

        public int SelectedSuptNoteRowIndex { get; set; }

        public int SelectedPMNoteRowIndex { get; set; }

        public int SelectedCustNoteRowIndex { get; set; }

        public int SelectedManufNoteRowIndex { get; set; }

        public RelayCommand ViewCustomerCommand { get; set; }

        public RelayCommand ViewManufCommand { get; set; }

        public RelayCommand NewCustomerCommand { get; set; }

        public RelayCommand NewManufCommand { get; set; }

        private int _selectedManufID;

        public int SelectedManufID
        {
            get { return _selectedManufID; }
            set
            {
                _selectedManufID = value;
                OnPropertyChanged();

            }
        }

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

        private int _selectedCustPmID;

        public int SelectedCustPmID
        {
            get { return _selectedCustPmID; }
            set
            {
                _selectedCustPmID = value;
                OnPropertyChanged();

            }
        }

        private string _selectedSovName;

        public string SelectedSovName
        {
            get { return _selectedSovName; }
            set
            {
                _selectedSovName = value;
                OnPropertyChanged();

            }
        }

        private int _selectedMaterialID;

        public int SelectedMaterialID
        {
            get { return _selectedMaterialID; }
            set
            {
                _selectedMaterialID = value;
                OnPropertyChanged();

            }
        }

        private int _selectedLaborID;

        public int SelectedLaborID
        {
            get { return _selectedLaborID; }
            set
            {
                _selectedLaborID = value;
                OnPropertyChanged();

            }
        }

        private int _selectedSalesmanID;

        public int SelectedSalesmanID
        {
            get { return _selectedSalesmanID; }
            set
            {
                _selectedSalesmanID = value;
                OnPropertyChanged();

            }
        }

        private int _selectedCrewID;

        public int SelectedCrewID
        {
            get { return _selectedCrewID; }
            set
            {
                _selectedCrewID = value;
                OnPropertyChanged();

            }
        }

        private int _selectedArchID;

        public int SelectedArchID
        {
            get { return _selectedArchID; }
            set
            {
                _selectedArchID = value;
                OnPropertyChanged();

            }
        }

        private int _selectedFreightCoID;

        public int SelectedFreightCoID
        {
            get { return _selectedFreightCoID; }
            set
            {
                _selectedFreightCoID = value;
                OnPropertyChanged();

            }
        }

        private int _selectedUserID;

        public int SelectedUserID
        {
            get { return _selectedUserID; }
            set
            {
                _selectedUserID = value;
                OnPropertyChanged();

            }
        }

        private int _selectedInstallerID;

        public int SelectedInstallerID
        {
            get { return _selectedInstallerID; }
            set
            {
                _selectedInstallerID = value;
                OnPropertyChanged();

            }
        }

        private int _selectedCustSuptID;

        public int SelectedCustSupID
        {
            get { return _selectedCustSuptID; }
            set
            {
                _selectedCustSuptID = value;
                OnPropertyChanged();

            }
        }

        private int _selectedCustSubmID;

        public int SelectedCustSubmID
        {
            get { return _selectedCustSubmID; }
            set
            {
                _selectedCustSubmID = value;
                OnPropertyChanged();

            }
        }

        private string _ActionCustName;

        public string ActionCustName
        {
            get { return _ActionCustName; }
            set
            {
                _ActionCustName = value;
                OnPropertyChanged();

            }
        }

        private string _actionState;

        public string ActionState
        {
            get { return _actionState; }
            set
            {
                _actionState = value;
                OnPropertyChanged();

            }
        }

        private string _actionSubState;

        public string ActionSubState
        {
            get { return _actionSubState; }
            set
            {
                _actionSubState = value;
                OnPropertyChanged();

            }
        }

        private string _ActionManufName;

        public string ActionManufName
        {
            get { return _ActionManufName; }
            set
            {
                _ActionManufName = value;
                OnPropertyChanged();
            }
        }

        private string _actionPmName;

        public string ActionPmName
        {
            get { return _actionPmName; }
            set
            {
                _actionPmName = value;
                OnPropertyChanged();
            }
        }

        private string _actionName;

        public string ActionName
        {
            get { return _actionName; }
            set
            {
                _actionName = value;
                OnPropertyChanged();
            }
        }

        private string _ActionSupName;

        public string ActionSupName
        {
            get { return _ActionSupName; }
            set
            {
                _ActionSupName = value;
                OnPropertyChanged();
            }
        }

        private string _ActionSubmName;

        public string ActionSubmName
        {
            get { return _ActionSubmName; }
            set
            {
                _ActionSubmName = value;
                OnPropertyChanged();
            }
        }

        private string _actionNoteState;

        public string ActionNoteState
        {
            get { return _actionNoteState; }
            set
            {
                _actionNoteState = value;
                OnPropertyChanged();

            }
        }

        private string _currentNotesDesc;

        public string CurrentNotesDesc
        {
            get { return _currentNotesDesc; }
            set
            {
                _currentNotesDesc = value;
                OnPropertyChanged();
            }
        }

        private int _currentNoteID;

        public int CurrentNoteID
        {
            get { return _currentNoteID; }
            set
            {
                _currentNoteID = value;
                OnPropertyChanged();
            }
        }

        private int _currnetNoteID;

        public int CurrnetNoteID
        {
            get { return _currnetNoteID; }
            set
            {
                _currnetNoteID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedCustRowIndex;

        public int SelectedCustRowIndex
        {
            get { return _selectedCustRowIndex; }
            set
            {
                _selectedCustRowIndex = value;
                OnPropertyChanged();

            }
        }

        private int _selectedCustListID;

        public int SelectedCustListID
        {
            get { return _selectedCustListID; }
            set
            {
                _selectedCustListID = value;
                OnPropertyChanged();
            }
        }

        private string _updateComponent;

        public string UpdateComponent
        {
            get { return _updateComponent; }
            set
            {
                _updateComponent = value;
                OnPropertyChanged();
            }
        }

        private string ActionCustState { get; set; }

        private string ActionManufState { get; set; }

        private Customer _tempCustomer;

        public Customer TempCustomer
        {
            get { return _tempCustomer; }
            set
            {
                _tempCustomer = value;
                OnPropertyChanged();
            }
        }

        private Customer _tempDetailCustomer;

        public Customer TempDetailCustomer
        {
            get { return _tempDetailCustomer; }
            set
            {
                _tempDetailCustomer = value;
                OnPropertyChanged();
            }
        }

        private Customer _tempCreateCustomer;

        public Customer TempCreateCustomer
        {
            get { return _tempCreateCustomer; }
            set
            {
                _tempCreateCustomer = value;
                OnPropertyChanged();
            }
        }

        private Manufacturer _tempDetailManuf;

        public Manufacturer TempDetailManuf
        {
            get { return _tempDetailManuf; }
            set
            {
                _tempDetailManuf = value;
                OnPropertyChanged();
            }
        }

        private Manufacturer _tempCreateManuf;

        public Manufacturer TempCreateManuf
        {
            get { return _tempCreateManuf; }
            set
            {
                _tempCreateManuf = value;
                OnPropertyChanged();
            }
        }

        private Manufacturer _tempManuf;

        public Manufacturer TempManuf
        {
            get { return _tempManuf; }
            set
            {
                _tempManuf = value;
                OnPropertyChanged();
            }
        }

        private Acronym _tempAcronym;

        public Acronym TempAcronym
        {
            get { return _tempAcronym; }
            set
            {
                _tempAcronym = value;
                OnPropertyChanged();
            }
        }

        private Material _tempMaterial;

        public Material TempMaterial
        {
            get { return _tempMaterial; }
            set
            {
                _tempMaterial = value;
                OnPropertyChanged();
            }
        }

        private ProjectManager _tempPM;

        public ProjectManager TempPM
        {
            get { return _tempPM; }
            set
            {
                _tempPM = value;
                OnPropertyChanged();
            }
        }

        private Note _tempNote;

        public Note TempNote
        {
            get { return _tempNote; }
            set
            {
                _tempNote = value;
                OnPropertyChanged();
            }
        }

        private Note _tempCustomerNote;

        public Note TempCustomerNote
        {
            get { return _tempCustomerNote; }
            set
            {
                _tempCustomerNote = value;
                OnPropertyChanged();
            }
        }

        private Note _tempSupNote;

        public Note TempSupNote
        {
            get { return _tempSupNote; }
            set
            {
                _tempSupNote = value;
                OnPropertyChanged();
            }
        }

        private Note _tempSubmNote;

        public Note TempSubmNote
        {
            get { return _tempSubmNote; }
            set
            {
                _tempSubmNote = value;
                OnPropertyChanged();
            }
        }

        private Superintendent _tempSup;

        public Superintendent TempSup
        {
            get { return _tempSup; }
            set
            {
                _tempSup = value;
                OnPropertyChanged();
            }
        }

        private CustomerContact _tempCC;

        public CustomerContact TempCC
        {
            get { return _tempCC; }
            set
            {
                _tempCC = value;
                OnPropertyChanged();
            }
        }

        private Labor _tempLabor;

        public Labor TempLabor
        {
            get { return _tempLabor; }
            set
            {
                _tempLabor = value;
                OnPropertyChanged();
            }
        }

        private Salesman _tempSalesman;

        public Salesman TempSalesman
        {
            get { return _tempSalesman; }
            set
            {
                _tempSalesman = value;
                OnPropertyChanged();
            }
        }

        private Crew _tempCrew;

        public Crew TempCrew
        {
            get { return _tempCrew; }
            set
            {
                _tempCrew = value;
                OnPropertyChanged();
            }
        }

        private FreightCo _tempFreightCo;

        public FreightCo TempFreightCo
        {
            get { return _tempFreightCo; }
            set
            {
                _tempFreightCo = value;
                OnPropertyChanged();
            }
        }

        private User _tempUser;

        public User TempUser
        {
            get { return _tempUser; }
            set
            {
                _tempUser = value;
                OnPropertyChanged();
            }
        }

        private InHouseInstaller _tempInstaller;

        public InHouseInstaller TempInstaller
        {
            get { return _tempInstaller; }
            set
            {
                _tempInstaller = value;
                OnPropertyChanged();
            }
        }

        private Architect _tempArch;

        public Architect TempArch
        {
            get { return _tempArch; }
            set
            {
                _tempArch = value;
                OnPropertyChanged();
            }
        }

        public int SelectedTempCustIndex { get; set; }

        public int SelectedTempAcronymIndex { get; set; }

        public int SelectedTempManufIndex { get; set; }

        public int CurrentIndex { get; set; }
    }
}