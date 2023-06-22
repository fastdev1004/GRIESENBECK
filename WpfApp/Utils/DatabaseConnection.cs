using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Utils
{
    class DatabaseConnection
    {
        private SqlConnection connection;
        public SqlCommand cmd;
        private readonly string connectionString;
        private int insertedID;


        public DatabaseConnection()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
            connection = new SqlConnection(this.connectionString);
            connection.Open();
        }

        public SqlCommand RunQuryNoParameters(string query)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return cmd;
        }

        //Create Installer
        public int RunQueryToCreateInstaller(string query, string name, string cell, string email, string oshaLevel, int crew, DateTime oshaDate, string oshaCert, DateTime facDate, string facCert, bool active)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(name))
                        cmd.Parameters.AddWithValue("@Name", name);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(cell))
                        cmd.Parameters.AddWithValue("@Cell", cell);
                    else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                    if (!string.IsNullOrEmpty(email))
                        cmd.Parameters.AddWithValue("@Email", email);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    if (!string.IsNullOrEmpty(oshaLevel))
                        cmd.Parameters.AddWithValue("@OshaLevel", oshaLevel);
                    else cmd.Parameters.AddWithValue("@OshaLevel", DBNull.Value);
                    if (crew > 0)
                        cmd.Parameters.AddWithValue("@Crew", crew);
                    else cmd.Parameters.AddWithValue("@Crew", DBNull.Value);
                    if (!oshaDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@OshaDate", oshaDate);
                    else cmd.Parameters.AddWithValue("@OshaDate", DBNull.Value);
                    if (!string.IsNullOrEmpty(oshaCert))
                        cmd.Parameters.AddWithValue("@OshaCert", oshaCert);
                    else cmd.Parameters.AddWithValue("@OshaCert", DBNull.Value);
                    if (!facDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@FacDate", facDate);
                    else cmd.Parameters.AddWithValue("@FacDate", DBNull.Value);
                    if (!string.IsNullOrEmpty(facCert))
                        cmd.Parameters.AddWithValue("@FacCert", facCert);
                    else cmd.Parameters.AddWithValue("@FacCert", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);
                    insertedID = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return insertedID;
        }

        // Update Installer
        public SqlCommand RunQueryToUpdateInstaller(string query, string name, string cell, string email, string oshaLevel, int crew, DateTime oshaDate, string oshaCert, DateTime facDate, string facCert, bool active, int id)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(name))
                        cmd.Parameters.AddWithValue("@Name", name);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(cell))
                        cmd.Parameters.AddWithValue("@Cell", cell);
                    else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                    if (!string.IsNullOrEmpty(email))
                        cmd.Parameters.AddWithValue("@Email", email);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    if (!string.IsNullOrEmpty(oshaLevel))
                        cmd.Parameters.AddWithValue("@OshaLevel", oshaLevel);
                    else cmd.Parameters.AddWithValue("@OshaLevel", DBNull.Value);
                    if (crew > 0)
                        cmd.Parameters.AddWithValue("@Crew", crew);
                    else cmd.Parameters.AddWithValue("@Crew", DBNull.Value);
                    if (!oshaDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@OshaDate", oshaDate);
                    else cmd.Parameters.AddWithValue("@OshaDate", DBNull.Value);
                    if (!string.IsNullOrEmpty(oshaCert))
                        cmd.Parameters.AddWithValue("@OshaCert", oshaCert);
                    else cmd.Parameters.AddWithValue("@OshaCert", DBNull.Value);
                    if (!facDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@FacDate", facDate);
                    else cmd.Parameters.AddWithValue("@FacDate", DBNull.Value);
                    if (!string.IsNullOrEmpty(facCert))
                        cmd.Parameters.AddWithValue("@FacCert", facCert);
                    else cmd.Parameters.AddWithValue("@FacCert", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);
                    cmd.Parameters.AddWithValue("@InstallerID", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cmd;
        }

        // Create User
        public int RunQueryToCreateUser(string query, string userName, string personName, int level, int formOnOpen, string email, bool active)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(userName))
                        cmd.Parameters.AddWithValue("@UserName", userName);
                    else cmd.Parameters.AddWithValue("@UserName", DBNull.Value);
                    if (!string.IsNullOrEmpty(personName))
                        cmd.Parameters.AddWithValue("@PersonName", personName);
                    else cmd.Parameters.AddWithValue("@PersonName", DBNull.Value);
                    if (level <= 0)
                        cmd.Parameters.AddWithValue("@Level", level);
                    else cmd.Parameters.AddWithValue("@Level", DBNull.Value);
                    if (formOnOpen <= 0)
                        cmd.Parameters.AddWithValue("@FormOnOpen", formOnOpen);
                    else cmd.Parameters.AddWithValue("@FormOnOpen", DBNull.Value);
                    if (!string.IsNullOrEmpty(email))
                        cmd.Parameters.AddWithValue("@Email", email);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);

                    insertedID = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return insertedID;
        }

        // Update User
        public SqlCommand RunQueryToUpdateUser(string query, string userName, string personName, int level, int formOnOpen, string email, bool active, int userID)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

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
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cmd;
        }

        // Create Freight CO
        public int RunQueryToCreateFreightCO(string query, string freightName, string phone, string email, string contact, string cell, bool active)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(freightName))
                        cmd.Parameters.AddWithValue("@Name", freightName);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(phone))
                        cmd.Parameters.AddWithValue("@Phone", phone);
                    else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                    if (!string.IsNullOrEmpty(email))
                        cmd.Parameters.AddWithValue("@Email", email);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    if (!string.IsNullOrEmpty(contact))
                        cmd.Parameters.AddWithValue("@Contact", contact);
                    else cmd.Parameters.AddWithValue("@Contact", DBNull.Value);
                    if (!string.IsNullOrEmpty(cell))
                        cmd.Parameters.AddWithValue("@Cell", cell);
                    else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);

                    insertedID = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return insertedID;
        }

        // Update Freight CO
        public SqlCommand RunQueryToUpdateFreightCO(string query, string freightName, string phone, string email, string contact, string cell, bool active, int freightID)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(freightName))
                        cmd.Parameters.AddWithValue("@Name", freightName);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(phone))
                        cmd.Parameters.AddWithValue("@Phone", phone);
                    else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                    if (!string.IsNullOrEmpty(email))
                        cmd.Parameters.AddWithValue("@Email", email);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    if (!string.IsNullOrEmpty(contact))
                        cmd.Parameters.AddWithValue("@Contact", contact);
                    else cmd.Parameters.AddWithValue("@Contact", DBNull.Value);
                    if (!string.IsNullOrEmpty(cell))
                        cmd.Parameters.AddWithValue("@Cell", cell);
                    else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);
                    cmd.Parameters.AddWithValue("@FreightCoID", freightID);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cmd;
        }

        // Update Arch
        public SqlCommand RunQueryToUpdateArch(string query, string archCompany, string archContact, string archAddress, string archCity, string archState, string archZip, string archPhone, string archFax, string archCell, string archEmail, bool active, int archID)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

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

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cmd;
        }

        // Create Arch
        public int RunQueryToCreateArch(string query, string archCompany, string archContact, string archAddress, string archCity, string archState, string archZip, string archPhone, string archFax, string archCell, string archEmail, bool active)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

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

                    insertedID = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return insertedID;
        }

        // Update Crew
        public SqlCommand RunQueryToUpdateCrew(string query, string crewName, string crewPhone, string crewCell, string crewEmail, bool active, int crewID)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(crewName))
                        cmd.Parameters.AddWithValue("@Name", crewName);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(crewPhone))
                        cmd.Parameters.AddWithValue("@Phone", crewPhone);
                    else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                    if (!string.IsNullOrEmpty(crewCell))
                        cmd.Parameters.AddWithValue("@Cell", crewCell);
                    else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                    if (!string.IsNullOrEmpty(crewEmail))
                        cmd.Parameters.AddWithValue("@Email", crewEmail);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);
                    cmd.Parameters.AddWithValue("@CrewID", crewID);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cmd;
        }

        // Create Crew
        public int RunQueryToCreateCrew(string query, string crewName, string crewPhone, string crewCell, string crewEmail, bool active)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(crewName))
                        cmd.Parameters.AddWithValue("@Name", crewName);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(crewPhone))
                        cmd.Parameters.AddWithValue("@Phone", crewPhone);
                    else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                    if (!string.IsNullOrEmpty(crewCell))
                        cmd.Parameters.AddWithValue("@Cell", crewCell);
                    else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                    if (!string.IsNullOrEmpty(crewEmail))
                        cmd.Parameters.AddWithValue("@Email", crewEmail);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);


                    insertedID = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return insertedID;
        }

        // Update Salesman
        public SqlCommand RunQueryToUpdateSalesman(string query, string salesInit, string salesName, string salesPhone, string salesCell, string salesEmail, bool active, int salesID)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

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

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cmd;
        }

        // Create Salesman
        public int RunQueryToCreateSalesman(string query, string salesInit, string salesName, string salesPhone, string salesCell, string salesEmail, bool active)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

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


                    insertedID = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return insertedID;
        }

        // Update Labor
        public SqlCommand RunQueryToUpdateLabor(string query, string laborDesc, double unitPrice, bool active, int laborID)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(laborDesc))
                        cmd.Parameters.AddWithValue("@LaborDesc", laborDesc);
                    else cmd.Parameters.AddWithValue("@LaborDesc", DBNull.Value);
                    if (unitPrice >= 0.0)
                        cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                    else cmd.Parameters.AddWithValue("@UnitPrice", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);
                    cmd.Parameters.AddWithValue("@LaborID", laborID);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cmd;
        }

        // Create Labor
        public int RunQueryToCreateLabor(string query, string laborDesc, double unitPrice, bool active)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(laborDesc))
                        cmd.Parameters.AddWithValue("@LaborDesc", laborDesc);
                    else cmd.Parameters.AddWithValue("@LaborDesc", DBNull.Value);
                    if (unitPrice >= 0.0)
                        cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                    else cmd.Parameters.AddWithValue("@UnitPrice", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);


                    insertedID = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return insertedID;
        }

        // Update Material
        public SqlCommand RunQueryToUpdateMaterial(string query, string matDesc, bool active, int matID)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(matDesc))
                        cmd.Parameters.AddWithValue("@MatDesc", matDesc);
                    else cmd.Parameters.AddWithValue("@MatDesc", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);
                    cmd.Parameters.AddWithValue("@MatID", matID);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cmd;
        }

        // Create Material
        public int RunQueryToCreateMaterial(string query, string matDesc, bool active)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(matDesc))
                        cmd.Parameters.AddWithValue("@MatDesc", matDesc);
                    else cmd.Parameters.AddWithValue("@MatDesc", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);

                    insertedID = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return insertedID;
        }

        // Update Acronym
        public SqlCommand RunQueryToUpdateAcronym(string query, string sovName, string sovDesc, bool active, string selectedSovName)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(sovName))
                        cmd.Parameters.AddWithValue("@Name", sovName);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(sovDesc))
                        cmd.Parameters.AddWithValue("@Desc", sovDesc);
                    else cmd.Parameters.AddWithValue("@Desc", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);
                    if (!string.IsNullOrEmpty(selectedSovName))
                        cmd.Parameters.AddWithValue("@OriginName", selectedSovName);
                    else cmd.Parameters.AddWithValue("@OriginName", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cmd;
        }

        // Create Acronym
        public string RunQueryToCreateAcronym(string query, string sovName, string sovDesc, bool active)
        {
            string insertedSovName = "";
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(sovName))
                        cmd.Parameters.AddWithValue("@Name", sovName);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(sovDesc))
                        cmd.Parameters.AddWithValue("@Desc", sovDesc);
                    else cmd.Parameters.AddWithValue("@Desc", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);

                    insertedSovName = (string)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return insertedSovName;
        }

        // Update Customer Relational info
        public SqlCommand RunQueryToUpdateCustAddInfo(string query, int selectedCustomerID, string name, string phone, string cell, string email, bool active, int infoID)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (selectedCustomerID != 0)
                        cmd.Parameters.AddWithValue("@CustomerID", selectedCustomerID);
                    else cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                    if (!string.IsNullOrEmpty(name))
                        cmd.Parameters.AddWithValue("@Name", name);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(phone))
                        cmd.Parameters.AddWithValue("@Phone", phone);
                    else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                    if (!string.IsNullOrEmpty(cell))
                        cmd.Parameters.AddWithValue("@Cell", cell);
                    else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                    if (!string.IsNullOrEmpty(email))
                        cmd.Parameters.AddWithValue("@Email", email);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);
                    cmd.Parameters.AddWithValue("@ID", infoID);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cmd;
        }

        // Create Customer Relational info
        public int RunQueryToCreateCustAddInfo(string query, int selectedCustomerID, string name, string phone, string cell, string email, bool active)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (selectedCustomerID != 0)
                        cmd.Parameters.AddWithValue("@CustomerID", selectedCustomerID);
                    else cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                    if (!string.IsNullOrEmpty(name))
                        cmd.Parameters.AddWithValue("@Name", name);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(phone))
                        cmd.Parameters.AddWithValue("@Phone", phone);
                    else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                    if (!string.IsNullOrEmpty(cell))
                        cmd.Parameters.AddWithValue("@Cell", cell);
                    else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                    if (!string.IsNullOrEmpty(email))
                        cmd.Parameters.AddWithValue("@Email", email);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);

                    insertedID = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return insertedID;
        }

        // Update Manuf
        public SqlCommand RunQueryToUpdateManuf(string query, string manufName, string address, string address2, string city, string state, string zip, string phone, string fax, string contactName, string contactPhone, string contactEmail, bool active, int manufID)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

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
                    cmd.Parameters.AddWithValue("@Active", active);
                    if (manufID != 0)
                        cmd.Parameters.AddWithValue("@ManufID", manufID);
                    else cmd.Parameters.AddWithValue("@ManufID", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cmd;
        }

        // Create Manuf
        public int RunQueryToCreateManuf(string query, string manufName, string address, string address2, string city, string state, string zip, string phone, string fax, string contactName, string contactPhone, string contactEmail, bool active)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

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
                    cmd.Parameters.AddWithValue("@Active", active);


                    insertedID = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return insertedID;
        }

        // Update Note
        public SqlCommand RunQueryToUpdateNote(string query, string note, DateTime notesDateAdded, string user, int noteID)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(note))
                        cmd.Parameters.AddWithValue("@NotesNote", note);
                    else cmd.Parameters.AddWithValue("@NotesNote", DBNull.Value);
                    if (!notesDateAdded.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@NotesDateAdded", notesDateAdded);
                    else cmd.Parameters.AddWithValue("@NotesDateAdded", DBNull.Value);
                    if (!string.IsNullOrEmpty(user))
                        cmd.Parameters.AddWithValue("@NotesUser", user);
                    else cmd.Parameters.AddWithValue("@NotesUser", DBNull.Value);
                    if (noteID != 0)
                        cmd.Parameters.AddWithValue("@NotesID", noteID);
                    else cmd.Parameters.AddWithValue("@NotesID", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cmd;
        }

        // Create Note
        public int RunQueryToCreateNote(string query, int notesPK, string notesPkDesc, string notesNote, DateTime notesDateAdded, string user)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (notesPK != 0)
                        cmd.Parameters.AddWithValue("@NotesPK", notesPK);
                    else cmd.Parameters.AddWithValue("@NotesPK", DBNull.Value);
                    if (!string.IsNullOrEmpty(notesPkDesc))
                        cmd.Parameters.AddWithValue("@NotesDesc", notesPkDesc);
                    else cmd.Parameters.AddWithValue("@NotesDesc", DBNull.Value);
                    if (!string.IsNullOrEmpty(notesNote))
                        cmd.Parameters.AddWithValue("@NotesNote", notesNote);
                    else cmd.Parameters.AddWithValue("@NotesNote", DBNull.Value);
                    if (!notesDateAdded.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@NotesDateAdded", notesDateAdded);
                    else cmd.Parameters.AddWithValue("@NotesDateAdded", DBNull.Value);
                    if (!string.IsNullOrEmpty(user))
                        cmd.Parameters.AddWithValue("@NotesUser", user);
                    else cmd.Parameters.AddWithValue("@NotesUser", DBNull.Value);

                    insertedID = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return insertedID;
        }

        // Update Customer
        public SqlCommand RunQueryToUpdateCustomer(string query, string fullName, string shortName, string poNumber, string address, string city, string state, string zip, string phone, string fax, string email, bool active, int customerID)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

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

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cmd;
        }

        // Create Customer
        public int RunQueryToCreateCustomer(string query, string fullName, string shortName, string poNumber, string address, string city, string state, string zip, string phone, string fax, string email, bool active)
        {
            try
            {
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

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

                    insertedID = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return insertedID;
        }

        public void Open()
        {
           connection.Open();
        }

        public void Close()
        {
           connection.Close();
        }
    }
}
