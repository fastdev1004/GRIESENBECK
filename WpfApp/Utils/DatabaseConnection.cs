using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Utils
{
    class DatabaseConnection
    {
        private SqlConnection connection;
        private string connectionString = @"Data Source = DESKTOP-VDIB57T\INSTANCE2023; user id=sa; password=qwe234ASD@#$; Initial Catalog = griesenbeck;";

        public SqlConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = new SqlConnection(connectionString);
                }
                return connection;
            }
        }

        public void Open()
        {
            Connection.Open();
        }

        public void Close()
        {
            Connection.Close();
        }
    }
}
