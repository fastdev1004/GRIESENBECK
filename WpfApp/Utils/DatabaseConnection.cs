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
        }

        public SqlCommand RunQuryNoParameters(string query)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                connection.Close();
            }
            return cmd;
        }

        //Create Installer
        public int RunQueryToCreateInstaller(string query, string name, string cell, string email, string oshaLevel, int crew, DateTime oshaDate, string oshaCert, DateTime facDate, string facCert, bool active)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Installer
        public SqlCommand RunQueryToUpdateInstaller(string query, string name, string cell, string email, string oshaLevel, int crew, DateTime oshaDate, string oshaCert, DateTime facDate, string facCert, bool active, int id)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create User
        public int RunQueryToCreateUser(string query, string userName, string personName, int level, int formOnOpen, string email, bool active)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update User
        public SqlCommand RunQueryToUpdateUser(string query, string userName, string personName, int level, int formOnOpen, string email, bool active, int userID)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create Freight CO
        public int RunQueryToCreateFreightCO(string query, string freightName, string phone, string email, string contact, string cell, bool active)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Freight CO
        public SqlCommand RunQueryToUpdateFreightCO(string query, string freightName, string phone, string email, string contact, string cell, bool active, int freightID)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Update Arch
        public SqlCommand RunQueryToUpdateArch(string query, string archCompany, string archContact, string archAddress, string archCity, string archState, string archZip, string archPhone, string archFax, string archCell, string archEmail, bool active, int archID)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create Arch
        public int RunQueryToCreateArch(string query, string archCompany, string archContact, string archAddress, string archCity, string archState, string archZip, string archPhone, string archFax, string archCell, string archEmail, bool active)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Crew
        public SqlCommand RunQueryToUpdateCrew(string query, string crewName, string crewPhone, string crewCell, string crewEmail, bool active, int crewID)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create Crew
        public int RunQueryToCreateCrew(string query, string crewName, string crewPhone, string crewCell, string crewEmail, bool active)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Salesman
        public SqlCommand RunQueryToUpdateSalesman(string query, string salesInit, string salesName, string salesPhone, string salesCell, string salesEmail, bool active, int salesID)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create Salesman
        public int RunQueryToCreateSalesman(string query, string salesInit, string salesName, string salesPhone, string salesCell, string salesEmail, bool active)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Labor
        public SqlCommand RunQueryToUpdateLabor(string query, string laborDesc, double unitPrice, bool active, int laborID)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create Labor
        public int RunQueryToCreateLabor(string query, string laborDesc, double unitPrice, bool active)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Material
        public SqlCommand RunQueryToUpdateMaterial(string query, string matDesc, bool active, int matID)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create Material
        public int RunQueryToCreateMaterial(string query, string matCode, string matDesc, bool active)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(matCode))
                        cmd.Parameters.AddWithValue("@Code", matCode);
                    else cmd.Parameters.AddWithValue("@Code", DBNull.Value);
                    if (!string.IsNullOrEmpty(matDesc))
                        cmd.Parameters.AddWithValue("@Desc", matDesc);
                    else cmd.Parameters.AddWithValue("@Desc", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Acronym
        public SqlCommand RunQueryToUpdateAcronym(string query, string sovName, string sovDesc, bool active, string selectedSovName)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create Acronym
        public string RunQueryToCreateAcronym(string query, string sovName, string sovDesc, bool active)
        {
            string insertedSovName = "";
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedSovName;
        }

        // Update Customer Relational info
        public SqlCommand RunQueryToUpdateCustAddInfo(string query, int selectedCustomerID, string name, string phone, string cell, string email, bool active, int infoID)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create Customer Relational info
        public int RunQueryToCreateCustAddInfo(string query, int selectedCustomerID, string name, string phone, string cell, string email, bool active)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                connection.Close();
            }
            return insertedID;
        }

        // Update Manuf
        public SqlCommand RunQueryToUpdateManuf(string query, string manufName, string address, string address2, string city, string state, string zip, string phone, string fax, string contactName, string contactPhone, string contactEmail, bool active, int manufID)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                connection.Close();
            }
            return cmd;
        }

        // Create Manuf
        public int RunQueryToCreateManuf(string query, string manufName, string address, string address2, string city, string state, string zip, string phone, string fax, string contactName, string contactPhone, string contactEmail, bool active)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Note
        public SqlCommand RunQueryToUpdateNote(string query, string note, DateTime notesDateAdded, string user, int noteID)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                connection.Close();
            }
            return cmd;
        }

        // Create Note
        public int RunQueryToCreateNote(string query, int notesPK, string notesPkDesc, string notesNote, DateTime notesDateAdded, string user)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Customer
        public SqlCommand RunQueryToUpdateCustomer(string query, string fullName, string shortName, string poNumber, string address, string city, string state, string zip, string phone, string fax, string email, bool active, int customerID)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                connection.Close();
            }
            return cmd;
        }

        // Create Customer
        public int RunQueryToCreateCustomer(string query, string fullName, string shortName, string poNumber, string address, string city, string state, string zip, string phone, string fax, string email, bool active)
        {
            try
            {
                connection.Open();
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
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                connection.Close();
            }
            return insertedID;
        }

        // Create Estimator
        public int RunQueryToCreateEstimator(string query, string initial, string name, string cell, string email, bool active)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(initial))
                        cmd.Parameters.AddWithValue("@Initial", initial);
                    else cmd.Parameters.AddWithValue("@Initial", DBNull.Value);
                    if (!string.IsNullOrEmpty(name))
                        cmd.Parameters.AddWithValue("@Name", name);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(cell))
                        cmd.Parameters.AddWithValue("@Cell", cell);
                    else cmd.Parameters.AddWithValue("@Cell", DBNull.Value);
                    if (!string.IsNullOrEmpty(email))
                        cmd.Parameters.AddWithValue("@Email", email);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                connection.Close();
            }
            return insertedID;
        }

        // Create Project Coord
        public int RunQueryToCreatePC(string query, string name, string email, bool active)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(name))
                        cmd.Parameters.AddWithValue("@Name", name);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(email))
                        cmd.Parameters.AddWithValue("@Email", email);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                connection.Close();
            }
            return insertedID;
        }

        // Create SOV
        public string RunQueryToCreateSOV(string query, string name, string desc, bool active)
        {
            string insertedSovName = "";
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(name))
                        cmd.Parameters.AddWithValue("@Name", name);
                    else cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                    if (!string.IsNullOrEmpty(desc))
                        cmd.Parameters.AddWithValue("@Desc", desc);
                    else cmd.Parameters.AddWithValue("@Desc", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);

                    insertedSovName = (string)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                connection.Close();
            }
            return insertedSovName;
        }

        // Create Material
        public int RunQueryToCreateMat(string query, string code, string desc, bool active)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(code))
                        cmd.Parameters.AddWithValue("@Code", code);
                    else cmd.Parameters.AddWithValue("@Code", DBNull.Value);
                    if (!string.IsNullOrEmpty(desc))
                        cmd.Parameters.AddWithValue("@Desc", desc);
                    else cmd.Parameters.AddWithValue("@Desc", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                connection.Close();
            }
            return insertedID;
        }

        // Create Freight CO
        public int RunQueryToCreateFreight(string query, string name, string phone, string cell, string email, string contact, bool active)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

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
                    if (!string.IsNullOrEmpty(contact))
                        cmd.Parameters.AddWithValue("@Contact", contact);
                    else cmd.Parameters.AddWithValue("@Contact", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", active);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Create Project
        public int RunQueryToCreateProject(string query, string name, string jobNo, int estimatorID, int pcID, int customerID, int ccID, int architectID, int crewID, string address, string city, string state, string zip, DateTime dateCompleted, DateTime targetDate, bool backgroundCheck, bool cip, bool certPayReqd, bool pnpBond, bool gapBid, bool gapEst, bool onHold, bool complete, bool payReqd, string payReqdNote, string addInfo, bool storedMat, int billingDate, bool c3, bool lcpTracker, string safetyBadging, int archRepID, string masterContract)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(name))
                        cmd.Parameters.AddWithValue("@ProjectName", name);
                    else cmd.Parameters.AddWithValue("@ProjectName", DBNull.Value);
                    if (!string.IsNullOrEmpty(jobNo))
                        cmd.Parameters.AddWithValue("@JobNo", jobNo);
                    else cmd.Parameters.AddWithValue("@JobNo", DBNull.Value);
                    if(estimatorID != -1)
                        cmd.Parameters.AddWithValue("@EstimatorID", estimatorID);
                    else cmd.Parameters.AddWithValue("@EstimatorID", DBNull.Value);
                    if (pcID != -1)
                        cmd.Parameters.AddWithValue("@PcID", pcID);
                    else cmd.Parameters.AddWithValue("@PcID", DBNull.Value);
                    if (customerID != -1)
                        cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    else cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                    if (ccID != -1)
                        cmd.Parameters.AddWithValue("@CcID", ccID);
                    else cmd.Parameters.AddWithValue("@CcID", DBNull.Value);
                    if (architectID != -1)
                        cmd.Parameters.AddWithValue("@ArchitectID", architectID);
                    else cmd.Parameters.AddWithValue("@ArchitectID", DBNull.Value);
                    if (crewID != -1)
                        cmd.Parameters.AddWithValue("@CrewID", crewID);
                    else cmd.Parameters.AddWithValue("@CrewID", DBNull.Value);
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
                    if (dateCompleted.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateCompleted", dateCompleted);
                    else cmd.Parameters.AddWithValue("@DateCompleted", DBNull.Value);
                    if (targetDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@TargetDate", dateCompleted);
                    else cmd.Parameters.AddWithValue("@TargetDate", DBNull.Value);
                    cmd.Parameters.AddWithValue("@BackgroundCheck", backgroundCheck);
                    cmd.Parameters.AddWithValue("@CipProject", cip);
                    cmd.Parameters.AddWithValue("@CertPayReqd", certPayReqd);
                    cmd.Parameters.AddWithValue("@PnpBond", pnpBond);
                    cmd.Parameters.AddWithValue("@GapBid", gapBid);
                    cmd.Parameters.AddWithValue("@GapEst", gapEst);
                    cmd.Parameters.AddWithValue("@OnHold", onHold);
                    cmd.Parameters.AddWithValue("@Complete", complete);
                    cmd.Parameters.AddWithValue("@PayReqd", payReqd);
                    if (!string.IsNullOrEmpty(payReqdNote))
                        cmd.Parameters.AddWithValue("@PayReqdNote", payReqdNote);
                    else cmd.Parameters.AddWithValue("@PayReqdNote", DBNull.Value);
                    if (!string.IsNullOrEmpty(addInfo))
                        cmd.Parameters.AddWithValue("@AddInfo", addInfo);
                    else cmd.Parameters.AddWithValue("@AddInfo", DBNull.Value);
                    cmd.Parameters.AddWithValue("@StoredMaterial", storedMat);
                    if (billingDate != 0)
                        cmd.Parameters.AddWithValue("@BillingDate", billingDate);
                    else cmd.Parameters.AddWithValue("@BillingDate", DBNull.Value);
                    cmd.Parameters.AddWithValue("@C3", c3);
                    cmd.Parameters.AddWithValue("@LcpTracker", lcpTracker);
                    if (!string.IsNullOrEmpty(safetyBadging))
                        cmd.Parameters.AddWithValue("@SafetyBadging", safetyBadging);
                    else cmd.Parameters.AddWithValue("@SafetyBadging", DBNull.Value);
                    if (archRepID != -1)
                        cmd.Parameters.AddWithValue("@ArchRepID", archRepID);
                    else cmd.Parameters.AddWithValue("@ArchRepID", DBNull.Value);
                    if (!string.IsNullOrEmpty(masterContract))
                        cmd.Parameters.AddWithValue("@MasterContract", masterContract);
                    else cmd.Parameters.AddWithValue("@MasterContract", DBNull.Value);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Project
        public SqlCommand RunQueryToUpdateProject(string query, string name, string jobNo, int estimatorID, int pcID, int customerID, int ccID, int architectID, int crewID, string address, string city, string state, string zip, DateTime dateCompleted, DateTime targetDate, bool backgroundCheck, bool cip, bool certPayReqd, bool pnpBond, bool gapBid, bool gapEst, bool onHold, bool complete, bool payReqd, string payReqdNote, string addInfo, bool storedMat, int billingDate, bool c3, bool lcpTracker, string safetyBadging, int archRepID, string masterContract, int projectID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(name))
                        cmd.Parameters.AddWithValue("@ProjectName", name);
                    else cmd.Parameters.AddWithValue("@ProjectName", DBNull.Value);
                    if (!string.IsNullOrEmpty(jobNo))
                        cmd.Parameters.AddWithValue("@JobNo", jobNo);
                    else cmd.Parameters.AddWithValue("@JobNo", DBNull.Value);
                    if (estimatorID != -1)
                        cmd.Parameters.AddWithValue("@EstimatorID", estimatorID);
                    else cmd.Parameters.AddWithValue("@EstimatorID", DBNull.Value);
                    if (pcID != -1)
                        cmd.Parameters.AddWithValue("@PcID", pcID);
                    else cmd.Parameters.AddWithValue("@PcID", DBNull.Value);
                    if (customerID != -1)
                        cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    else cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                    if (ccID != -1)
                        cmd.Parameters.AddWithValue("@CcID", ccID);
                    else cmd.Parameters.AddWithValue("@CcID", DBNull.Value);
                    if (architectID != -1)
                        cmd.Parameters.AddWithValue("@ArchitectID", architectID);
                    else cmd.Parameters.AddWithValue("@ArchitectID", DBNull.Value);
                    if (crewID != -1)
                        cmd.Parameters.AddWithValue("@CrewID", crewID);
                    else cmd.Parameters.AddWithValue("@CrewID", DBNull.Value);
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
                    if (!dateCompleted.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateCompleted", dateCompleted);
                    else cmd.Parameters.AddWithValue("@DateCompleted", DBNull.Value);
                    if (!targetDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@TargetDate", dateCompleted);
                    else cmd.Parameters.AddWithValue("@TargetDate", DBNull.Value);
                    if(backgroundCheck)
                        cmd.Parameters.AddWithValue("@BackgroundCheck", 1);
                    else cmd.Parameters.AddWithValue("@BackgroundCheck", 0);
                    if(cip)
                        cmd.Parameters.AddWithValue("@CipProject", 1);
                    else cmd.Parameters.AddWithValue("@CipProject", 0);
                    if (certPayReqd)
                        cmd.Parameters.AddWithValue("@CertPayReqd", 1);
                    else cmd.Parameters.AddWithValue("@CertPayReqd", 0);
                    if (pnpBond)
                        cmd.Parameters.AddWithValue("@PnpBond", 1);
                    else cmd.Parameters.AddWithValue("@PnpBond", 0);
                    if (gapBid)
                        cmd.Parameters.AddWithValue("@GapBid", 1);
                    else cmd.Parameters.AddWithValue("@GapBid", 0);
                    if (gapEst)
                        cmd.Parameters.AddWithValue("@GapEst", 1);
                    else cmd.Parameters.AddWithValue("@GapEst", 0);
                    if (onHold)
                        cmd.Parameters.AddWithValue("@OnHold", 1);
                    else cmd.Parameters.AddWithValue("@OnHold", 0);
                    if (complete)
                        cmd.Parameters.AddWithValue("@Complete", 1);
                    else cmd.Parameters.AddWithValue("@Complete", 0);
                    if (payReqd)
                        cmd.Parameters.AddWithValue("@PayReqd", 1);
                    else cmd.Parameters.AddWithValue("@PayReqd", 0);
                    if (!string.IsNullOrEmpty(payReqdNote))
                        cmd.Parameters.AddWithValue("@PayReqdNote", payReqdNote);
                    else cmd.Parameters.AddWithValue("@PayReqdNote", DBNull.Value);
                    if (!string.IsNullOrEmpty(addInfo))
                        cmd.Parameters.AddWithValue("@AddInfo", addInfo);
                    else cmd.Parameters.AddWithValue("@AddInfo", DBNull.Value);
                    cmd.Parameters.AddWithValue("@StoredMaterial", storedMat);
                    if (billingDate != 0)
                        cmd.Parameters.AddWithValue("@BillingDate", billingDate);
                    else cmd.Parameters.AddWithValue("@BillingDate", DBNull.Value);
                    if (c3)
                        cmd.Parameters.AddWithValue("@C3", 1);
                    else cmd.Parameters.AddWithValue("@C3", 0);
                    if (lcpTracker)
                        cmd.Parameters.AddWithValue("@LcpTracker", 1);
                    else cmd.Parameters.AddWithValue("@LcpTracker", 0);
                    if (!string.IsNullOrEmpty(safetyBadging))
                        cmd.Parameters.AddWithValue("@SafetyBadging", safetyBadging);
                    else cmd.Parameters.AddWithValue("@SafetyBadging", DBNull.Value);
                    if (archRepID != -1)
                        cmd.Parameters.AddWithValue("@ArchRepID", archRepID);
                    else cmd.Parameters.AddWithValue("@ArchRepID", DBNull.Value);
                    if (!string.IsNullOrEmpty(masterContract))
                        cmd.Parameters.AddWithValue("@MasterContract", masterContract);
                    else cmd.Parameters.AddWithValue("@MasterContract", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ProjectID", projectID);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create ProjPM
        public int RunQueryToCreateProjPM(string query, int projectID, int pmID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    cmd.Parameters.AddWithValue("@PmID", pmID);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update ProjPM
        public SqlCommand RunQueryToUpdateProjPM(string query, int projectID, int pmID, int projPmID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    cmd.Parameters.AddWithValue("@PmID", pmID);
                    cmd.Parameters.AddWithValue("@ProjPmID", projPmID);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create ProjSup
        public int RunQueryToCreateProjSup(string query, int projectID, int supID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    cmd.Parameters.AddWithValue("@SupID", supID);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update ProjSup
        public SqlCommand RunQueryToUpdateProjSup(string query, int projectID, int supID, int projSupID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    cmd.Parameters.AddWithValue("@SupID", supID);
                    cmd.Parameters.AddWithValue("@ProjSupID", projSupID);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create ProjectLink
        public int RunQueryToCreateProjectLink(string query, int projectID, string pathDesc, string pathName)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    if(!string.IsNullOrEmpty(pathDesc))
                        cmd.Parameters.AddWithValue("@PathDesc", pathDesc);
                    else cmd.Parameters.AddWithValue("@PathDesc", DBNull.Value);
                    if(!string.IsNullOrEmpty(pathName))
                        cmd.Parameters.AddWithValue("@PathName", pathName);
                    else cmd.Parameters.AddWithValue("@PathName", DBNull.Value);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update ProjectLink
        public SqlCommand RunQueryToUpdateProjectLink(string query, int projectID, string pathDesc, string pathName, int linkID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    cmd.Parameters.AddWithValue("@PathDesc", pathDesc);
                    cmd.Parameters.AddWithValue("@PathName", pathName);
                    cmd.Parameters.AddWithValue("@LinkID", linkID);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create ProjectSOV
        public int RunQueryToCreateProjectSOV(string query, int projectID, int coID, string sovAcronymName, bool matOnly)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    //cmd.Parameters.AddWithValue("@ProjSovID", projSovID);
                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    if (coID != 0)
                        cmd.Parameters.AddWithValue("@CoID", coID);
                    else cmd.Parameters.AddWithValue("@CoID", DBNull.Value);
                    if (!string.IsNullOrEmpty(sovAcronymName))
                        cmd.Parameters.AddWithValue("@SovAcronymName", sovAcronymName);
                    else cmd.Parameters.AddWithValue("@SovAcronymName", DBNull.Value);
                    cmd.Parameters.AddWithValue("@MatOnly", matOnly);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update ProjectSOV
        public SqlCommand RunQueryToUpdateProjectSOV(string query, int projSovID, int coID, string sovAcronymName, bool matOnly)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjSovID", projSovID);
                    if(coID != -1 && coID != 0)
                        cmd.Parameters.AddWithValue("@CoID", coID);
                    else cmd.Parameters.AddWithValue("@CoID", DBNull.Value);
                    if (!string.IsNullOrEmpty(sovAcronymName))
                        cmd.Parameters.AddWithValue("@SovAcronymName", sovAcronymName);
                    else cmd.Parameters.AddWithValue("@SovAcronymName", DBNull.Value);
                    if (matOnly)
                        cmd.Parameters.AddWithValue("@MatOnly", 1);
                    else cmd.Parameters.AddWithValue("@MatOnly", 0);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }
        
        // Update ProjectSOV
        public SqlCommand RunQueryToUpdateSOV(string query, string sovDesc, string sovAcronymName)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(sovAcronymName))
                        cmd.Parameters.AddWithValue("@SovAcronymName", sovAcronymName);
                    else cmd.Parameters.AddWithValue("@SovAcronymName", DBNull.Value);
                    if (!string.IsNullOrEmpty(sovDesc))
                        cmd.Parameters.AddWithValue("@SovDesc", sovDesc);
                    else cmd.Parameters.AddWithValue("@SovDesc", DBNull.Value);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create ProjectMat
        public int RunQueryToCreateProjectMat(string query, int projSovID, int matID, string matPhase, string matType, string color, int qtyReqd, double totalCost, bool matOnly, int projectID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjSovID", projSovID);
                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    if (matID != 0)
                        cmd.Parameters.AddWithValue("@MatID", matID);
                    else cmd.Parameters.AddWithValue("@MatID", DBNull.Value);
                    if (!string.IsNullOrEmpty(matPhase))
                        cmd.Parameters.AddWithValue("@MatPhase", matPhase);
                    else cmd.Parameters.AddWithValue("@MatPhase", DBNull.Value);
                    if (!string.IsNullOrEmpty(matType))
                        cmd.Parameters.AddWithValue("@MatType", matType);
                    else cmd.Parameters.AddWithValue("@MatType", DBNull.Value);
                    if (!string.IsNullOrEmpty(color))
                        cmd.Parameters.AddWithValue("@Color", color);
                    else cmd.Parameters.AddWithValue("@Color", DBNull.Value);
                    if (qtyReqd != 0)
                        cmd.Parameters.AddWithValue("@QtyReqd", qtyReqd);
                    else cmd.Parameters.AddWithValue("@QtyReqd", DBNull.Value);
                    if (totalCost != 0)
                        cmd.Parameters.AddWithValue("@TotalCost", totalCost);
                    else cmd.Parameters.AddWithValue("@TotalCost", DBNull.Value);
                    cmd.Parameters.AddWithValue("@MatOnly", matOnly);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update ProjectMat
        public SqlCommand RunQueryToUpdateProjectMat(string query, int projSovID, int matLine, int matID, string matPhase, string matType, string color, int qtyReqd, double totalCost, bool matLot, double matOrigRate, bool matOnly, int projectID, int projMatID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjSovID", projSovID);
                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    if (matLine != 0)
                        cmd.Parameters.AddWithValue("@MatLine", matLine);
                    else cmd.Parameters.AddWithValue("@MatLine", DBNull.Value);
                    if (matID != 0)
                        cmd.Parameters.AddWithValue("@MatID", matID);
                    else cmd.Parameters.AddWithValue("@MatID", DBNull.Value);
                    if (!string.IsNullOrEmpty(matPhase))
                        cmd.Parameters.AddWithValue("@MatPhase", matPhase);
                    else cmd.Parameters.AddWithValue("@MatPhase", DBNull.Value);
                    if (!string.IsNullOrEmpty(matType))
                        cmd.Parameters.AddWithValue("@MatType", matType);
                    else cmd.Parameters.AddWithValue("@MatType", DBNull.Value);
                    if (!string.IsNullOrEmpty(color))
                        cmd.Parameters.AddWithValue("@Color", color);
                    else cmd.Parameters.AddWithValue("@Color", DBNull.Value);
                    if (qtyReqd != 0)
                        cmd.Parameters.AddWithValue("@QtyReqd", qtyReqd);
                    else cmd.Parameters.AddWithValue("@QtyReqd", DBNull.Value);
                    if (totalCost != 0)
                        cmd.Parameters.AddWithValue("@TotalCost", totalCost);
                    else cmd.Parameters.AddWithValue("@TotalCost", DBNull.Value);
                    cmd.Parameters.AddWithValue("@MatLot", matLot);
                    if (matOrigRate != 0)
                        cmd.Parameters.AddWithValue("@MatOrigRate", matOrigRate);
                    else cmd.Parameters.AddWithValue("@MatOrigRate", DBNull.Value);
                    if (projMatID != 0)
                        cmd.Parameters.AddWithValue("@ProjMatID", projMatID);
                    else cmd.Parameters.AddWithValue("@ProjMatID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@MatOnly", matOnly);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create ProjectLab
        public int RunQueryToCreateProjectLab(string query, int projSovID, int projectID, int labLineNo, int laborID, string phase, double qtyReqd, double unitPrice, bool laborComplete)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjSovID", projSovID);
                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    if (labLineNo != 0)
                        cmd.Parameters.AddWithValue("@LabLineNo", labLineNo);
                    else cmd.Parameters.AddWithValue("@LabLineNo", DBNull.Value);
                    if (laborID != -1 && laborID != 0)
                        cmd.Parameters.AddWithValue("@LaborID", laborID);
                    else cmd.Parameters.AddWithValue("@MatPhase", DBNull.Value);
                    if (!string.IsNullOrEmpty(phase))
                        cmd.Parameters.AddWithValue("@LabPhase", phase);
                    else cmd.Parameters.AddWithValue("@LabPhase", DBNull.Value);
                    if (qtyReqd != 0)
                        cmd.Parameters.AddWithValue("@QtyReqd", qtyReqd);
                    else cmd.Parameters.AddWithValue("@QtyReqd", DBNull.Value);
                    if (unitPrice != 0)
                        cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                    else cmd.Parameters.AddWithValue("@UnitPrice", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Complete", laborComplete);

                   insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update ProjectLab
        public SqlCommand RunQueryToUpdateProjectLab(string query, int projSovID, int projectID, int labLineNo, int laborID, string phase, double qtyReqd, double unitPrice, bool laborComplete, int projLabID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjSovID", projSovID);
                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    if (labLineNo != 0)
                        cmd.Parameters.AddWithValue("@LabLineNo", labLineNo);
                    else cmd.Parameters.AddWithValue("@LabLineNo", DBNull.Value);
                    if (laborID != -1 && laborID != 0)
                        cmd.Parameters.AddWithValue("@LaborID", laborID);
                    else cmd.Parameters.AddWithValue("@MatPhase", DBNull.Value);
                    if (!string.IsNullOrEmpty(phase))
                        cmd.Parameters.AddWithValue("@LabPhase", phase);
                    else cmd.Parameters.AddWithValue("@LabPhase", DBNull.Value);
                    if (qtyReqd != 0)
                        cmd.Parameters.AddWithValue("@QtyReqd", qtyReqd);
                    else cmd.Parameters.AddWithValue("@QtyReqd", DBNull.Value);
                    if (unitPrice != 0)
                        cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                    else cmd.Parameters.AddWithValue("@UnitPrice", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Complete", laborComplete);
                    cmd.Parameters.AddWithValue("@ProjLabID", projLabID);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }
        
        // Create Project Mat Tracking
        public int RunQueryToCreateProjMatTracking(string query, int projMatID, int manufID, string orderNo, DateTime matReqdDate, string poNumber, double qtyOrd, DateTime dateOrd, bool takeFromStock, bool shipToJob, bool orderComplete, bool guarDim, bool needFM, DateTime fieldDim, DateTime shopReqDate, DateTime shopRecvdDate, DateTime rFF, DateTime submIssue, DateTime submAppr, DateTime reSubmit, int projectID, bool finalsRev, bool laborComplete, string manufLeadTime, bool noSubm)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjMatID", projMatID);
                    if (manufID != 0)
                        cmd.Parameters.AddWithValue("@ManufID", manufID);
                    else cmd.Parameters.AddWithValue("@ManufID", DBNull.Value);
                    if (!string.IsNullOrEmpty(orderNo))
                        cmd.Parameters.AddWithValue("@ManufOrderNo", orderNo);
                    else cmd.Parameters.AddWithValue("@ManufOrderNo", DBNull.Value);
                    if (!matReqdDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@MatReqdDate", matReqdDate);
                    else cmd.Parameters.AddWithValue("@MatReqdDate", DBNull.Value);
                    if (!string.IsNullOrEmpty(poNumber))
                        cmd.Parameters.AddWithValue("@PoNumber", poNumber);
                    else cmd.Parameters.AddWithValue("@PoNumber", DBNull.Value);
                    cmd.Parameters.AddWithValue("@QtyOrd", qtyOrd);
                    if (!dateOrd.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateOrd", dateOrd);
                    else cmd.Parameters.AddWithValue("@DateOrd", DBNull.Value);
                    cmd.Parameters.AddWithValue("@TakeFromStock", Convert.ToInt32(takeFromStock));
                    cmd.Parameters.AddWithValue("@ShipToJob", Convert.ToInt32(shipToJob));
                    cmd.Parameters.AddWithValue("@MatComplete", Convert.ToInt32(orderComplete));
                    cmd.Parameters.AddWithValue("@GuarDim", Convert.ToInt32(guarDim));
                    cmd.Parameters.AddWithValue("@FmNeeded", Convert.ToInt32(needFM));
                    if (!fieldDim.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@FieldDim", fieldDim);
                    else cmd.Parameters.AddWithValue("@FieldDim", DBNull.Value);
                    if (!shopReqDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@ShopReqDate", shopReqDate);
                    else cmd.Parameters.AddWithValue("@ShopReqDate", DBNull.Value);
                    if (!shopRecvdDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@ShopRecvdDate", shopRecvdDate);
                    else cmd.Parameters.AddWithValue("@ShopRecvdDate", DBNull.Value);
                    if (!rFF.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@ReleaseForFab", rFF);
                    else cmd.Parameters.AddWithValue("@ReleaseForFab", DBNull.Value);
                    if (!submIssue.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@SubmitIssue", submIssue);
                    else cmd.Parameters.AddWithValue("@SubmitIssue", DBNull.Value);
                    if (!submAppr.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@SubmitAppr", submAppr);
                    else cmd.Parameters.AddWithValue("@SubmitAppr", DBNull.Value);
                    if (!reSubmit.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@ResubmitDate", reSubmit);
                    else cmd.Parameters.AddWithValue("@ResubmitDate", DBNull.Value);
                    if (projectID != 0)
                        cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    else cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FinalsRev", Convert.ToInt32(finalsRev));
                    cmd.Parameters.AddWithValue("@LaborComplete", Convert.ToInt32(laborComplete));
                    if (!string.IsNullOrEmpty(manufLeadTime))
                        cmd.Parameters.AddWithValue("@ManufLeadTime", manufLeadTime);
                    else cmd.Parameters.AddWithValue("@ManufLeadTime", DBNull.Value);
                    cmd.Parameters.AddWithValue("@NoSubNeeded", Convert.ToInt32(noSubm));

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Project Mat Tracking
        public SqlCommand RunQueryToUpdateProjMatTracking(string query, int projMatID, int manufID, string orderNo, DateTime matReqdDate, string poNumber, double qtyOrd, DateTime dateOrd, bool takeFromStock, bool shipToJob, bool orderComplete, bool guarDim, bool needFM, DateTime fieldDim, DateTime shopReqDate, DateTime shopRecvdDate, DateTime rFF, DateTime submIssue, DateTime submAppr, DateTime reSubmit, int projectID, bool finalsRev, bool laborComplete, string manufLeadTime, bool noSubm, int projMtID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjMatID", projMatID);
                    if (manufID != 0)
                        cmd.Parameters.AddWithValue("@ManufID", manufID);
                    else cmd.Parameters.AddWithValue("@ManufID", DBNull.Value);
                    if (!string.IsNullOrEmpty(orderNo))
                        cmd.Parameters.AddWithValue("@ManufOrderNo", orderNo);
                    else cmd.Parameters.AddWithValue("@ManufOrderNo", DBNull.Value);
                    if (!matReqdDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@MatReqdDate", matReqdDate);
                    else cmd.Parameters.AddWithValue("@MatReqdDate", DBNull.Value);
                    if (!string.IsNullOrEmpty(poNumber))
                        cmd.Parameters.AddWithValue("@PoNumber", poNumber);
                    else cmd.Parameters.AddWithValue("@PoNumber", DBNull.Value);
                    cmd.Parameters.AddWithValue("@QtyOrd", qtyOrd);
                    if (!dateOrd.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateOrd", dateOrd);
                    else cmd.Parameters.AddWithValue("@DateOrd", DBNull.Value);
                    cmd.Parameters.AddWithValue("@TakeFromStock", Convert.ToInt32(takeFromStock));
                    cmd.Parameters.AddWithValue("@ShipToJob", Convert.ToInt32(shipToJob));
                    cmd.Parameters.AddWithValue("@MatComplete", Convert.ToInt32(orderComplete));
                    cmd.Parameters.AddWithValue("@GuarDim", Convert.ToInt32(guarDim));
                    cmd.Parameters.AddWithValue("@FmNeeded", Convert.ToInt32(needFM));
                    if (!fieldDim.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@FieldDim", fieldDim);
                    else cmd.Parameters.AddWithValue("@FieldDim", DBNull.Value);
                    if (!shopReqDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@ShopReqDate", shopReqDate);
                    else cmd.Parameters.AddWithValue("@ShopReqDate", DBNull.Value);
                    if (!shopRecvdDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@ShopRecvdDate", shopRecvdDate);
                    else cmd.Parameters.AddWithValue("@ShopRecvdDate", DBNull.Value);
                    if (!rFF.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@ReleaseForFab", rFF);
                    else cmd.Parameters.AddWithValue("@ReleaseForFab", DBNull.Value);
                    if (!submIssue.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@SubmitIssue", submIssue);
                    else cmd.Parameters.AddWithValue("@SubmitIssue", DBNull.Value);
                    if (!submAppr.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@SubmitAppr", submAppr);
                    else cmd.Parameters.AddWithValue("@SubmitAppr", DBNull.Value);
                    if (!reSubmit.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@ResubmitDate", reSubmit);
                    else cmd.Parameters.AddWithValue("@ResubmitDate", DBNull.Value);
                    if (projectID != 0)
                        cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    else cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FinalsRev", Convert.ToInt32(finalsRev));
                    cmd.Parameters.AddWithValue("@LaborComplete", Convert.ToInt32(laborComplete));
                    if (!string.IsNullOrEmpty(manufLeadTime))
                        cmd.Parameters.AddWithValue("@ManufLeadTime", manufLeadTime);
                    else cmd.Parameters.AddWithValue("@ManufLeadTime", DBNull.Value);
                    cmd.Parameters.AddWithValue("@NoSubNeeded", Convert.ToInt32(noSubm));
                    cmd.Parameters.AddWithValue("@ProjMtID", projMtID);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create Project Mat Ship
        public int RunQueryToCreateProjectMatShip(string query, int projMtID, DateTime schedShipDate, DateTime estDelivDate, DateTime estInstallDate, string estWeight, int estPallet, int freightCoID, string trackingNo, double qtyShipped, DateTime dateShipped, double qtyRecvd, DateTime dateRecvd, int noOfPallet, bool freightDamage, int shipProjectID, DateTime deliverToJob, string siteStroage, string storageDetail)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                   
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    cmd.Parameters.AddWithValue("@ProjMtID", projMtID);
                    if (!schedShipDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@SchedShipDate", schedShipDate);
                    else cmd.Parameters.AddWithValue("@SchedShipDate", DBNull.Value);
                    if (!estDelivDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@EstDelivDate", estDelivDate);
                    else cmd.Parameters.AddWithValue("@EstDelivDate", DBNull.Value);
                    if (!estInstallDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@EstInstallDate", estInstallDate);
                    else cmd.Parameters.AddWithValue("@EstInstallDate", DBNull.Value);
                    if (!string.IsNullOrEmpty(estWeight))
                        cmd.Parameters.AddWithValue("@EstWeight", estWeight);
                    else cmd.Parameters.AddWithValue("@EstWeight", DBNull.Value);
                    if (estPallet != 0)
                        cmd.Parameters.AddWithValue("@EstNoPallets", estPallet);
                    else cmd.Parameters.AddWithValue("@EstNoPallets", DBNull.Value);
                    if (freightCoID != 0)
                        cmd.Parameters.AddWithValue("@FreightCoID", freightCoID);
                    else cmd.Parameters.AddWithValue("@FreightCoID", DBNull.Value);
                    if (!string.IsNullOrEmpty(trackingNo))
                        cmd.Parameters.AddWithValue("@TrackingNo", trackingNo);
                    else cmd.Parameters.AddWithValue("@TrackingNo", DBNull.Value);
                    if (qtyShipped != 0)
                        cmd.Parameters.AddWithValue("@QtyShipped", qtyShipped);
                    else cmd.Parameters.AddWithValue("@QtyShipped", DBNull.Value);
                    if (!dateShipped.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateShipped", dateShipped);
                    else cmd.Parameters.AddWithValue("@DateShipped", DBNull.Value);
                    if (qtyRecvd != 0)
                        cmd.Parameters.AddWithValue("@QtyRecvd", qtyRecvd);
                    else cmd.Parameters.AddWithValue("@QtyRecvd", DBNull.Value);
                    if (!dateRecvd.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateRecvd", dateRecvd);
                    else cmd.Parameters.AddWithValue("@DateRecvd", DBNull.Value);
                    if (noOfPallet != 0)
                        cmd.Parameters.AddWithValue("@NoOfPallets", noOfPallet);
                    else cmd.Parameters.AddWithValue("@NoOfPallets", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FreightDamage", Convert.ToInt32(freightDamage));
                    if (shipProjectID != 0)
                        cmd.Parameters.AddWithValue("@ProjectID", shipProjectID);
                    else cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
                    if (!deliverToJob.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DelivertoJob", deliverToJob);
                    else cmd.Parameters.AddWithValue("@DelivertoJob", DBNull.Value);
                    if (!string.IsNullOrEmpty(siteStroage))
                        cmd.Parameters.AddWithValue("@SiteStorage", siteStroage);
                    else cmd.Parameters.AddWithValue("@SiteStorage", DBNull.Value);
                    if (!string.IsNullOrEmpty(storageDetail))
                        cmd.Parameters.AddWithValue("@StorageDetail", storageDetail);
                    else cmd.Parameters.AddWithValue("@StorageDetail", DBNull.Value);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Project Mat Ship
        public SqlCommand RunQueryToUpdateProjectMatShip(string query, DateTime schedShipDate, DateTime estDelivDate, DateTime estInstallDate, string estWeight, int estPallet, int freightCoID, string trackingNo, double qtyShipped, DateTime dateShipped, double qtyRecvd, DateTime dateRecvd, int noOfPallet, bool freightDamage, int shipProjectID, DateTime deliverToJob, string siteStroage, string storageDetail, int projMsID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {

                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!schedShipDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@SchedShipDate", schedShipDate);
                    else cmd.Parameters.AddWithValue("@SchedShipDate", DBNull.Value);
                    if (!estDelivDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@EstDelivDate", estDelivDate);
                    else cmd.Parameters.AddWithValue("@EstDelivDate", DBNull.Value);
                    if (!estInstallDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@EstInstallDate", estInstallDate);
                    else cmd.Parameters.AddWithValue("@EstInstallDate", DBNull.Value);
                    if (!string.IsNullOrEmpty(estWeight))
                        cmd.Parameters.AddWithValue("@EstWeight", estWeight);
                    else cmd.Parameters.AddWithValue("@EstWeight", DBNull.Value);
                    if (estPallet != 0)
                        cmd.Parameters.AddWithValue("@EstNoPallets", estPallet);
                    else cmd.Parameters.AddWithValue("@EstNoPallets", DBNull.Value);
                    if (freightCoID != 0)
                        cmd.Parameters.AddWithValue("@FreightCoID", freightCoID);
                    else cmd.Parameters.AddWithValue("@FreightCoID", DBNull.Value);
                    if (!string.IsNullOrEmpty(trackingNo))
                        cmd.Parameters.AddWithValue("@TrackingNo", trackingNo);
                    else cmd.Parameters.AddWithValue("@TrackingNo", DBNull.Value);
                    if (qtyShipped != 0)
                        cmd.Parameters.AddWithValue("@QtyShipped", trackingNo);
                    else cmd.Parameters.AddWithValue("@QtyShipped", DBNull.Value);
                    if (!dateShipped.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateShipped", dateShipped);
                    else cmd.Parameters.AddWithValue("@DateShipped", DBNull.Value);
                    if (qtyRecvd != 0)
                        cmd.Parameters.AddWithValue("@QtyRecvd", qtyRecvd);
                    else cmd.Parameters.AddWithValue("@QtyRecvd", DBNull.Value);
                    if (!dateRecvd.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateRecvd", dateRecvd);
                    else cmd.Parameters.AddWithValue("@DateRecvd", DBNull.Value);
                    if (noOfPallet != 0)
                        cmd.Parameters.AddWithValue("@NoOfPallets", noOfPallet);
                    else cmd.Parameters.AddWithValue("@NoOfPallets", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FreightDamage", Convert.ToInt32(freightDamage));
                    if (shipProjectID != 0)
                        cmd.Parameters.AddWithValue("@ProjectID", shipProjectID);
                    else cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
                    if (!deliverToJob.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DelivertoJob", deliverToJob);
                    else cmd.Parameters.AddWithValue("@DelivertoJob", DBNull.Value);
                    if (!string.IsNullOrEmpty(siteStroage))
                        cmd.Parameters.AddWithValue("@SiteStorage", siteStroage);
                    else cmd.Parameters.AddWithValue("@SiteStorage", DBNull.Value);
                    if (!string.IsNullOrEmpty(storageDetail))
                        cmd.Parameters.AddWithValue("@StorageDetail", storageDetail);
                    else cmd.Parameters.AddWithValue("@StorageDetail", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ProjMSID", projMsID);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Update Install Note
        public SqlCommand RunQueryToUpdateInstallNote(string query, string notesNote, string user, string userName, int noteProjectID, DateTime notesDateAdded, int notesID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(notesNote))
                        cmd.Parameters.AddWithValue("@InstallNote", notesNote);
                    else cmd.Parameters.AddWithValue("@InstallNote", DBNull.Value);
                    if (!string.IsNullOrEmpty(user))
                        cmd.Parameters.AddWithValue("@NotesUser", user);
                    else cmd.Parameters.AddWithValue("@NotesUser", DBNull.Value);
                    if (!string.IsNullOrEmpty(userName))
                        cmd.Parameters.AddWithValue("@NotesUserName", userName);
                    else cmd.Parameters.AddWithValue("@NotesUserName", DBNull.Value);
                    if (noteProjectID != 0)
                        cmd.Parameters.AddWithValue("@ProjectID", noteProjectID);
                    else cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
                    if (!notesDateAdded.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@NotesDateAdded", notesDateAdded);
                    else cmd.Parameters.AddWithValue("@NotesDateAdded", DBNull.Value);
                    cmd.Parameters.AddWithValue("@NotesID", notesID);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create Install Note
        public int RunQueryToCreateInstallNote(string query, string notesNote, string user, string userName, int noteProjectID, DateTime notesDateAdded)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!string.IsNullOrEmpty(notesNote))
                        cmd.Parameters.AddWithValue("@InstallNote", notesNote);
                    else cmd.Parameters.AddWithValue("@InstallNote", DBNull.Value);
                    if (!string.IsNullOrEmpty(user))
                        cmd.Parameters.AddWithValue("@NotesUser", user);
                    else cmd.Parameters.AddWithValue("@NotesUser", DBNull.Value);
                    if (!string.IsNullOrEmpty(userName))
                        cmd.Parameters.AddWithValue("@NotesUserName", userName);
                    else cmd.Parameters.AddWithValue("@NotesUserName", DBNull.Value);
                    if (noteProjectID != 0)
                        cmd.Parameters.AddWithValue("@ProjectID", noteProjectID);
                    else cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
                    if (!notesDateAdded.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@NotesDateAdded", notesDateAdded);
                    else cmd.Parameters.AddWithValue("@NotesDateAdded", DBNull.Value);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Create SC
        public int RunQueryToCreateSC(string query, int contractProjectID, string contractNumber, DateTime dateRecd, int amtOfContract, bool changeOrder, string changeOrderNo, DateTime signedoffbySales, DateTime signedoffbyoperations, DateTime givenAcctingforreview, DateTime givenforfinalsignature, DateTime dateReturnedback, DateTime returnedtoDawn, string returnedVia, string comment, DateTime dateProcessed, string scope)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (contractProjectID != 0)
                        cmd.Parameters.AddWithValue("@ProjectID", contractProjectID);
                    else cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
                    if (!string.IsNullOrEmpty(contractNumber))
                        cmd.Parameters.AddWithValue("@ContractNumber", contractNumber);
                    else cmd.Parameters.AddWithValue("@ContractNumber", DBNull.Value);
                    if (!dateRecd.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateRecd", dateRecd);
                    else cmd.Parameters.AddWithValue("@DateRecd", DBNull.Value);
                    if (amtOfContract != 0)
                        cmd.Parameters.AddWithValue("@AmtOfcontract", amtOfContract);
                    else cmd.Parameters.AddWithValue("@AmtOfcontract", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ChangeOrder", Convert.ToInt32(changeOrder));
                    if (!string.IsNullOrEmpty(changeOrderNo))
                        cmd.Parameters.AddWithValue("@ChangeOrderNo", changeOrderNo);
                    else cmd.Parameters.AddWithValue("@ChangeOrderNo", DBNull.Value);
                    if (!signedoffbySales.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@SignedoffbySales", signedoffbySales);
                    else cmd.Parameters.AddWithValue("@SignedoffbySales", DBNull.Value);
                    if (!signedoffbyoperations.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@Signedoffbyoperations", signedoffbyoperations);
                    else cmd.Parameters.AddWithValue("@Signedoffbyoperations", DBNull.Value);
                    if (!givenAcctingforreview.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@GivenAcctingforreview", givenAcctingforreview);
                    else cmd.Parameters.AddWithValue("@GivenAcctingforreview", DBNull.Value);
                    if (!givenforfinalsignature.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@Givenforfinalsignature", givenforfinalsignature);
                    else cmd.Parameters.AddWithValue("@Givenforfinalsignature", DBNull.Value);
                    if (!dateReturnedback.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@Datereturnedback", dateReturnedback);
                    else cmd.Parameters.AddWithValue("@Datereturnedback", DBNull.Value);
                    if (!returnedtoDawn.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@ReturnedtoDawn", returnedtoDawn);
                    else cmd.Parameters.AddWithValue("@ReturnedtoDawn", DBNull.Value);
                    if (!string.IsNullOrEmpty(comment))
                        cmd.Parameters.AddWithValue("@Comments", comment);
                    else cmd.Parameters.AddWithValue("@Comments", DBNull.Value);
                    if (!string.IsNullOrEmpty(returnedVia))
                        cmd.Parameters.AddWithValue("@ReturnedVia", returnedVia);
                    else cmd.Parameters.AddWithValue("@ReturnedVia", DBNull.Value);
                    if (!dateProcessed.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateProcessed", dateProcessed);
                    else cmd.Parameters.AddWithValue("@DateProcessed", DBNull.Value);
                    if (!string.IsNullOrEmpty(scope))
                        cmd.Parameters.AddWithValue("@Scope", scope);
                    else cmd.Parameters.AddWithValue("@Scope", DBNull.Value);
                   
                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update SC
        public SqlCommand RunQueryToUpdateSC(string query, int contractProjectID, string contractNumber, DateTime dateRecd, int amtOfContract, bool changeOrder, string changeOrderNo, DateTime signedoffbySales, DateTime signedoffbyoperations, DateTime givenAcctingforreview, DateTime givenforfinalsignature, DateTime dateReturnedback, DateTime returnedtoDawn, string returnedVia, string comment, DateTime dateProcessed, string scope, int scID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (contractProjectID != 0)
                        cmd.Parameters.AddWithValue("@ProjectID", contractProjectID);
                    else cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
                    if (!string.IsNullOrEmpty(contractNumber))
                        cmd.Parameters.AddWithValue("@ContractNumber", contractNumber);
                    else cmd.Parameters.AddWithValue("@ContractNumber", DBNull.Value);
                    if (!dateRecd.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateRecd", dateRecd);
                    else cmd.Parameters.AddWithValue("@DateRecd", DBNull.Value);
                    if (amtOfContract != 0)
                        cmd.Parameters.AddWithValue("@AmtOfcontract", amtOfContract);
                    else cmd.Parameters.AddWithValue("@AmtOfcontract", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ChangeOrder", Convert.ToInt32(changeOrder));
                    if (!string.IsNullOrEmpty(changeOrderNo))
                        cmd.Parameters.AddWithValue("@ChangeOrderNo", changeOrderNo);
                    else cmd.Parameters.AddWithValue("@ChangeOrderNo", DBNull.Value);
                    if (!signedoffbySales.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@SignedoffbySales", signedoffbySales);
                    else cmd.Parameters.AddWithValue("@SignedoffbySales", DBNull.Value);
                    if (!signedoffbyoperations.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@Signedoffbyoperations", signedoffbyoperations);
                    else cmd.Parameters.AddWithValue("@Signedoffbyoperations", DBNull.Value);
                    if (!givenAcctingforreview.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@GivenAcctingforreview", givenAcctingforreview);
                    else cmd.Parameters.AddWithValue("@GivenAcctingforreview", DBNull.Value);
                    if (!givenforfinalsignature.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@Givenforfinalsignature", givenforfinalsignature);
                    else cmd.Parameters.AddWithValue("@Givenforfinalsignature", DBNull.Value);
                    if (!dateReturnedback.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@Datereturnedback", dateReturnedback);
                    else cmd.Parameters.AddWithValue("@Datereturnedback", DBNull.Value);
                    if (!returnedtoDawn.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@ReturnedtoDawn", returnedtoDawn);
                    else cmd.Parameters.AddWithValue("@ReturnedtoDawn", DBNull.Value);
                    if (!string.IsNullOrEmpty(comment))
                        cmd.Parameters.AddWithValue("@Comments", comment);
                    else cmd.Parameters.AddWithValue("@Comments", DBNull.Value);
                    if (!string.IsNullOrEmpty(returnedVia))
                        cmd.Parameters.AddWithValue("@ReturnedVia", returnedVia);
                    else cmd.Parameters.AddWithValue("@ReturnedVia", DBNull.Value);
                    if (!dateProcessed.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateProcessed", dateProcessed);
                    else cmd.Parameters.AddWithValue("@DateProcessed", DBNull.Value);
                    if (!string.IsNullOrEmpty(scope))
                        cmd.Parameters.AddWithValue("@Scope", scope);
                    else cmd.Parameters.AddWithValue("@Scope", DBNull.Value);
                    cmd.Parameters.AddWithValue("@SCID", scID);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Update Change Order
        public SqlCommand RunQueryToUpdateCO(string query, int coProjectID, int coItemNo, DateTime coDate, string coAppDen, DateTime coDateAppDen, string coComment, int coID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (coProjectID != 0)
                        cmd.Parameters.AddWithValue("@ProjectID", coProjectID);
                    else cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
                    if (coItemNo != 0)
                        cmd.Parameters.AddWithValue("@CoItemNo", coItemNo);
                    else cmd.Parameters.AddWithValue("@CoItemNo", DBNull.Value);
                    if (!coDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@CoDate", coDate);
                    else cmd.Parameters.AddWithValue("@CoDate", DBNull.Value);
                    if (!string.IsNullOrEmpty(coAppDen))
                        cmd.Parameters.AddWithValue("@CoAppDen", coAppDen);
                    else cmd.Parameters.AddWithValue("@CoAppDen", DBNull.Value);
                    if (!coDateAppDen.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@CoDateAppDen", coDateAppDen);
                    else cmd.Parameters.AddWithValue("@CoDateAppDen", DBNull.Value);
                    if (!string.IsNullOrEmpty(coComment))
                        cmd.Parameters.AddWithValue("@CoComments", coComment);
                    else cmd.Parameters.AddWithValue("@CoComments", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CoID", coID);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }


        // Create Change Order
        public int RunQueryToCreateCO(string query, int coProjectID, int coItemNo, DateTime coDate, string coAppDen, DateTime coDateAppDen, string coComment)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (coProjectID != 0)
                        cmd.Parameters.AddWithValue("@ProjectID", coProjectID);
                    else cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
                    if (coItemNo != 0)
                        cmd.Parameters.AddWithValue("@CoItemNo", coItemNo);
                    else cmd.Parameters.AddWithValue("@CoItemNo", DBNull.Value);
                    if (!coDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@CoDate", coDate);
                    else cmd.Parameters.AddWithValue("@CoDate", DBNull.Value);
                    if (!string.IsNullOrEmpty(coAppDen))
                        cmd.Parameters.AddWithValue("@CoAppDen", coAppDen);
                    else cmd.Parameters.AddWithValue("@CoAppDen", DBNull.Value);
                    if (!coDateAppDen.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@CoDateAppDen", coDateAppDen);
                    else cmd.Parameters.AddWithValue("@CoDateAppDen", DBNull.Value);
                    if (!string.IsNullOrEmpty(coComment))
                        cmd.Parameters.AddWithValue("@CoComments", coComment);
                    else cmd.Parameters.AddWithValue("@CoComments", DBNull.Value);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Create CIP Type
        public int RunQueryToCreateCIP(string query, DateTime cipTargetDate, DateTime cipFormsRecD, DateTime cipFormsSent, DateTime cipCertRecD, double cipOriginalContractAmt, double cipFinalContractAmt, string cipCrewEnrolled, string cipNotes, bool cipExemptionApproved, int cipProjectID, string cipType, DateTime cipExemptionAppDate)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!cipTargetDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@TargetDate", cipTargetDate);
                    else cmd.Parameters.AddWithValue("@TargetDate", DBNull.Value);
                    if (!cipFormsRecD.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@FormsRecD", cipFormsRecD);
                    else cmd.Parameters.AddWithValue("@FormsRecD", DBNull.Value);
                    if (!cipFormsSent.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@FormsSent", cipFormsSent);
                    else cmd.Parameters.AddWithValue("@FormsSent", DBNull.Value);
                    if (!cipCertRecD.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@CertRecD", cipCertRecD);
                    else cmd.Parameters.AddWithValue("@CertRecD", DBNull.Value);
                    if (cipOriginalContractAmt != 0)
                        cmd.Parameters.AddWithValue("@OriginalContractAmt", cipOriginalContractAmt);
                    else cmd.Parameters.AddWithValue("@OriginalContractAmt", DBNull.Value);
                    if (cipFinalContractAmt != 0)
                        cmd.Parameters.AddWithValue("@FinalContractAmt", cipFinalContractAmt);
                    else cmd.Parameters.AddWithValue("@FinalContractAmt", DBNull.Value);
                    if (!string.IsNullOrEmpty(cipCrewEnrolled))
                        cmd.Parameters.AddWithValue("@CrewEnrolled", cipCrewEnrolled);
                    else cmd.Parameters.AddWithValue("@CrewEnrolled", DBNull.Value);
                    if (!string.IsNullOrEmpty(cipNotes))
                        cmd.Parameters.AddWithValue("@Notes", cipNotes);
                    else cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ExemptionApproved", Convert.ToInt32(cipExemptionApproved));
                    cmd.Parameters.AddWithValue("@ProjectID", cipProjectID);
                    if (!string.IsNullOrEmpty(cipType))
                        cmd.Parameters.AddWithValue("@CIPType", cipType);
                    else cmd.Parameters.AddWithValue("@CIPType", DBNull.Value);
                    if (!cipExemptionAppDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@ExemptionAppDate", cipExemptionAppDate);
                    else cmd.Parameters.AddWithValue("@ExemptionAppDate", DBNull.Value);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update CIP Type
        public SqlCommand RunQueryToUpdateCIP(string query, DateTime cipTargetDate, DateTime cipFormsRecD, DateTime cipFormsSent, DateTime cipCertRecD, double cipOriginalContractAmt, double cipFinalContractAmt, string cipCrewEnrolled, string cipNotes, bool cipExemptionApproved, int cipProjectID, string cipType, DateTime cipExemptionAppDate, int cipID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (!cipTargetDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@TargetDate", cipTargetDate);
                    else cmd.Parameters.AddWithValue("@TargetDate", DBNull.Value);
                    if (!cipFormsRecD.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@FormsRecD", cipFormsRecD);
                    else cmd.Parameters.AddWithValue("@FormsRecD", DBNull.Value);
                    if (!cipFormsSent.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@FormsSent", cipFormsSent);
                    else cmd.Parameters.AddWithValue("@FormsSent", DBNull.Value);
                    if (!cipCertRecD.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@CertRecD", cipCertRecD);
                    else cmd.Parameters.AddWithValue("@CertRecD", DBNull.Value);
                    if (cipOriginalContractAmt != 0)
                        cmd.Parameters.AddWithValue("@OriginalContractAmt", cipOriginalContractAmt);
                    else cmd.Parameters.AddWithValue("@OriginalContractAmt", DBNull.Value);
                    if (cipFinalContractAmt != 0)
                        cmd.Parameters.AddWithValue("@FinalContractAmt", cipFinalContractAmt);
                    else cmd.Parameters.AddWithValue("@FinalContractAmt", DBNull.Value);
                    if (!string.IsNullOrEmpty(cipCrewEnrolled))
                        cmd.Parameters.AddWithValue("@CrewEnrolled", cipCrewEnrolled);
                    else cmd.Parameters.AddWithValue("@CrewEnrolled", DBNull.Value);
                    if (!string.IsNullOrEmpty(cipNotes))
                        cmd.Parameters.AddWithValue("@Notes", cipNotes);
                    else cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ExemptionApproved", Convert.ToInt32(cipExemptionApproved));
                    cmd.Parameters.AddWithValue("@ProjectID", cipProjectID);
                    if (!string.IsNullOrEmpty(cipType))
                        cmd.Parameters.AddWithValue("@CIPType", cipType);
                    else cmd.Parameters.AddWithValue("@CIPType", DBNull.Value);
                    if (!cipExemptionAppDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@ExemptionAppDate", cipExemptionAppDate);
                    else cmd.Parameters.AddWithValue("@ExemptionAppDate", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CipID", cipID);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Update WorkOrder
        public SqlCommand RunQueryToUpdateWorkOrder(string query, int woNumber, int woProjectID, int woSupID, int woCrewID, DateTime woDateStarted, DateTime woDateCompleted, DateTime woSchedStartDate, DateTime woSchedComplDate, bool woComplete, int woID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (woNumber != 0)
                        cmd.Parameters.AddWithValue("@WoNumber", woNumber);
                    else cmd.Parameters.AddWithValue("@WoNumber", DBNull.Value);
                    if (woProjectID != 0)
                        cmd.Parameters.AddWithValue("@ProjectID", woProjectID);
                    else cmd.Parameters.AddWithValue("@ProjectID", DBNull.Value);
                    if (woSupID != 0)
                        cmd.Parameters.AddWithValue("@SupID", woSupID);
                    else cmd.Parameters.AddWithValue("@SupID", DBNull.Value);
                    if (woCrewID != 0)
                        cmd.Parameters.AddWithValue("@CrewID", woCrewID);
                    else cmd.Parameters.AddWithValue("@CrewID", DBNull.Value);
                    if (!woDateStarted.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateStarted", woDateStarted);
                    else cmd.Parameters.AddWithValue("@DateStarted", DBNull.Value);
                    if (!woDateCompleted.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@DateCompleted", woDateCompleted);
                    else cmd.Parameters.AddWithValue("@DateCompleted", DBNull.Value);
                    if (!woSchedStartDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@SchedStartDate", woSchedStartDate);
                    else cmd.Parameters.AddWithValue("@SchedStartDate", DBNull.Value);
                    if (!woSchedComplDate.Equals(DateTime.MinValue))
                        cmd.Parameters.AddWithValue("@SchedComplDate", woSchedComplDate);
                    else cmd.Parameters.AddWithValue("@SchedComplDate", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Complete", woComplete);
                    if (woID != 0)
                        cmd.Parameters.AddWithValue("@WoID", woNumber);
                    else cmd.Parameters.AddWithValue("@WoID", DBNull.Value);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create Work Orders Mat
        public int RunQueryToCreateWorkOrdersMat(string query, int woID, int projMsID, float matQty)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (woID != 0)
                        cmd.Parameters.AddWithValue("@WoID", woID);
                    else cmd.Parameters.AddWithValue("@WoID", DBNull.Value);
                    if (projMsID != 0)
                        cmd.Parameters.AddWithValue("@ProjMsID", projMsID);
                    else cmd.Parameters.AddWithValue("@ProjMsID", DBNull.Value);
                    if (matQty != 0)
                        cmd.Parameters.AddWithValue("@MatQty", matQty);
                    else cmd.Parameters.AddWithValue("@MatQty", DBNull.Value);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Work Orders Mat
        public SqlCommand RunQueryToUpdateWorkOrdersMat(string query, float matQty, int wodID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (wodID != 0)
                        cmd.Parameters.AddWithValue("@WodID", wodID);
                    else cmd.Parameters.AddWithValue("@WodID", DBNull.Value);
                    if (matQty != 0)
                        cmd.Parameters.AddWithValue("@MatQty", matQty);
                    else cmd.Parameters.AddWithValue("@MatQty", DBNull.Value);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
        }

        // Create Work Orders Lab
        public int RunQueryToCreateWorkOrdersLab(string query, int woID, int projLabID, float labQty)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (woID != 0)
                        cmd.Parameters.AddWithValue("@WoID", woID);
                    else cmd.Parameters.AddWithValue("@WoID", DBNull.Value);
                    if (projLabID != 0)
                        cmd.Parameters.AddWithValue("@ProjLabID", projLabID);
                    else cmd.Parameters.AddWithValue("@ProjLabID", DBNull.Value);
                    if (labQty != 0)
                        cmd.Parameters.AddWithValue("@LabQty", labQty);
                    else cmd.Parameters.AddWithValue("@LabQty", DBNull.Value);

                    insertedID = (int)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return insertedID;
        }

        // Update Work Orders Lab
        public SqlCommand RunQueryToUpdateWorkOrdersLab(string query, float labQty, int wodID)
        {
            try
            {
                connection.Open();
                if (connection != null)
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    if (labQty != 0)
                        cmd.Parameters.AddWithValue("@LabQty", labQty);
                    else cmd.Parameters.AddWithValue("@LabQty", DBNull.Value);
                    cmd.Parameters.AddWithValue("@WodID", wodID);

                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connection.Close();
            }
            return cmd;
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
