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
    class CloseOutViewModel:ViewModelBase
    {
        private DatabaseConnection dbConnection;
        public SqlCommand cmd = null;
        public SqlDataAdapter sda;
        public DataSet ds;
        public string sqlquery;

        public CloseOutViewModel()
        {
            dbConnection = new DatabaseConnection();
            LoadCloseOuts();
        }

        private void LoadCloseOuts()
        {
            // Projects
            sqlquery = "SELECT tblProjects.Project_ID, tblProjects.Project_Name, tblCustomers.Full_Name FROM tblProjects LEFT JOIN tblCustomers ON tblProjects.Customer_ID = tblCustomers.Customer_ID ORDER BY Project_Name ASC;";

            cmd = dbConnection.RunQuryNoParameters(sqlquery);
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
                    st_mb.Add(new Project { ID = projectID, ProjectName = projectName});
                }
            }
            Projects = st_mb;

            // ReturnedViaNames
            sqlquery = "SELECT * FROM tblReturnedVia";
            cmd = dbConnection.RunQuryNoParameters(sqlquery);
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
        }

        private void ChangeProject()
        {
            sqlquery = "SELECT * FROM tblWarranties WHERE ProjectId = " + ProjectID.ToString();

            cmd = dbConnection.RunQuryNoParameters(sqlquery);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Warranty> sb_warranties = new ObservableCollection<Warranty>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _warrantyID = 0;
                string _docuReq = "";
                DateTime _complDate = new DateTime();
                DateTime _dateRecd = new DateTime();
                int _numOfCopy = 0;
                string _contractName = "";
                string _submVia = "";
                DateTime _dateSent = new DateTime();
                string _sentVia = "";
                string _notes = "";

                if (!row.IsNull("WarrantiID"))
                    _warrantyID = int.Parse(row["WarrantiID"].ToString());
                if (!row.IsNull("documentRequest"))
                    _docuReq = row["documentRequest"].ToString();
                if (!row.IsNull("CompletionDate"))
                    _complDate = row.Field<DateTime>("CompletionDate");
                if (!row.IsNull("DateRecD"))
                    _dateRecd = row.Field<DateTime>("DateRecD");
                if (!row.IsNull("NumberofCopiesrequested"))
                    _numOfCopy = int.Parse(row["NumberofCopiesrequested"].ToString());
                if (!row.IsNull("Contact"))
                    _contractName = row["Contact"].ToString();
                if (!row.IsNull("SubmitVia"))
                    _submVia = row["SubmitVia"].ToString();
                if (!row.IsNull("Datesent"))
                    _dateSent = row.Field<DateTime>("Datesent");
                if (!row.IsNull("SentVia"))
                    _sentVia = row["SentVia"].ToString();
                if (!row.IsNull("Notes"))
                    _notes = row["Notes"].ToString();

                sb_warranties.Add(new Warranty
                {
                    ID = _warrantyID,
                    DocuReq = _docuReq,
                    ComplDate = _complDate,
                    DateRecd = _dateRecd,
                    NumOfCopy = _numOfCopy,
                    ContractName = _contractName,
                    SubmVia = _submVia,
                    DateSent = _dateRecd,
                    SentVia = _sentVia,
                    Notes = _notes
                });
            }
            Warranties = sb_warranties;
        }

        public ObservableCollection<Project> Projects
        {
            get;
            set;
        }

        private int _projectId;
        public int ProjectID
        {
            get { return _projectId; }
            set
            {
                _projectId = value;
                ChangeProject();
            }
        }

        private ObservableCollection<ReturnedVia> _returnedVia;

        public ObservableCollection<ReturnedVia> ReturnedViaNames
        {
            get { return _returnedVia; }
            set
            {
                _returnedVia = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Warranty> _warranty;

        public ObservableCollection<Warranty> Warranties
        {
            get { return _warranty; }
            set
            {
                _warranty = value;
                OnPropertyChanged();
            }
        }
    }
}
