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
    class ImportViewModel:ViewModelBase
    {
        private DatabaseConnection dbConnection;
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataSet ds;
        public string sqlquery;

        public ImportViewModel()
        {
            dbConnection = new DatabaseConnection();
            dbConnection.Open();
            LoadData();
        }

        public void LoadData()
        {
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

            cmd.Dispose();
            dbConnection.Close();
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

    }
}