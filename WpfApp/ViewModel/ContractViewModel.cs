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
    class ContractViewModel:ViewModelBase
    {
        private DatabaseConnection dbConnection;
        public SqlCommand cmd = null;
        public SqlDataAdapter sda;
        public DataSet ds;
        public string sqlquery;

        public ContractViewModel()
        {
            dbConnection = new DatabaseConnection();
            LoadContracts();
        }

        public void LoadContracts()
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
                    st_mb.Add(new Project { ID = projectID, ProjectName = projectName });
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

        public void ChangeProject()
        {
            // Contracts
            sqlquery = "SELECT tblProj.Job_No, Contractnumber, ChangeOrder, ChangeOrderNo, DateRecD, DateProcessed, AmtOfcontract, SignedoffbySales, Signedoffbyoperations, GivenAcctingforreview, Givenforfinalsignature, Scope, ReturnedVia, ReturnedtoDawn, Comments FROM tblSC RIGHT JOIN (SELECT Project_ID, Job_No FROM tblProjects WHERE Project_ID = " + ProjectID.ToString() + ") AS tblProj ON tblSC.ProjectID = tblProj.Project_ID";
            
            cmd = dbConnection.RunQuryNoParameters(sqlquery);
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

            // Project Change Orders
            sqlquery = "select * from tblProjectChangeOrders where Project_ID = " + ProjectID.ToString();
            cmd = dbConnection.RunQuryNoParameters(sqlquery);
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
        }

        public ObservableCollection<ReturnedVia> ReturnedViaNames
        {
            get;
            set;
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
    }
}
