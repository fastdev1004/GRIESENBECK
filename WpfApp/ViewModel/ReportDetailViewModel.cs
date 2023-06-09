using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WpfApp.Model;
using WpfApp.Utils;
using System.Diagnostics;

namespace WpfApp.ViewModel
{
    class ReportDetailViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        public SqlCommand cmd;
        public SqlDataAdapter sda;
        public DataSet ds;
        public string sqlquery;

        public ReportDetailViewModel()
        {
            dbConnection = new DatabaseConnection();
            dbConnection.Open();
        }

        public void LoadActiveLaborData()
        {
            // Track Labor List
            sqlquery = "SELECT tblLabor.*, tblProjects.Target_Date FROM tblProjects RIGHT JOIN (SELECT tblLab.SOV_Acronym, tblLab.Labor_Desc, tblProjectChangeOrders.CO_ItemNo, tblLab.Lab_Phase, tblLab.Complete, tblLab.Project_ID FROM tblProjectChangeOrders RIGHT JOIN (SELECT tblSOV.SOV_Acronym, tblLabor.Labor_Desc, tblSOV.CO_ID, tblSOV.Lab_Phase, tblSOV.Complete, tblSOV.Project_ID FROM tblLabor RIGHT JOIN (SELECT  tblProjectLabor.Labor_ID, tblProjectLabor.Lab_Phase, tblSOV.ProjSOV_ID, tblSOV.SOV_Acronym, tblSOV.CO_ID, tblProjectLabor.Complete, tblSOV.Project_ID FROM tblProjectLabor RIGHT JOIN (SELECT * FROM tblProjectSOV) AS tblSOV ON tblProjectLabor.ProjSOV_ID = tblSOV.ProjSOV_ID) AS tblSOV ON tblSOV.Labor_ID = tblLabor.Labor_ID) AS tblLab ON tblLab.CO_ID = tblProjectChangeOrders.CO_ID) AS tblLabor ON tblProjects.Project_ID=tblLabor.Project_ID";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<TrackLaborReport> sb_trackLaborReports = new ObservableCollection<TrackLaborReport>();
            foreach (DataRow row1 in ds.Tables[0].Rows)
            {
                string sovAcronym = "";
                string laborDesc = "";
                string coItemNo = "";
                string labPhase = "";
                DateTime targetDate = new DateTime();
                bool laborComplete = false;
                int projectID = 0;

                if (!row1.IsNull("Target_Date"))
                    targetDate = row1.Field<DateTime>("Target_Date");
                if (!row1.IsNull("SOV_Acronym"))
                    sovAcronym = row1["SOV_Acronym"].ToString();
                if (!row1.IsNull("Labor_Desc"))
                    laborDesc = row1["Labor_Desc"].ToString();
                if (!row1.IsNull("CO_ItemNo"))
                    coItemNo = row1["CO_ItemNo"].ToString();
                if (!row1.IsNull("Lab_Phase"))
                    labPhase = row1["Lab_Phase"].ToString();
                if (!row1.IsNull("Complete"))
                    laborComplete = row1.Field<Boolean>("Complete");
                if (!row1.IsNull("Project_ID"))
                    projectID = int.Parse(row1["Project_ID"].ToString());

                sb_trackLaborReports.Add(new TrackLaborReport
                {
                    SovAcronym = sovAcronym,
                    LaborDesc = laborDesc,
                    CoItemNo = coItemNo,
                    LabPhase = labPhase,
                    Complete = laborComplete,
                    TargetDate = targetDate.Date,
                    ProjectID = projectID
                });
            }
            TrackLaborReports = sb_trackLaborReports;

            // Labor View Detail Reports
            string whereClause = " WHERE 1=1";
            if (!string.IsNullOrEmpty(SelectedJobNo))
            {
                whereClause += $" AND tblProjects.Job_No = '{SelectedJobNo}'";
            }

            if (ProjectID != 0)
            {
                whereClause += $" AND tblProjects.Project_ID = '{ProjectID}'";
            }

            if (SelectedSalesmanID != 0)
            {
                whereClause += $" AND tblProjects.Salesman_ID = '{SelectedSalesmanID}'";
            }

            if (!SelectedDateFrom.Equals(DateTime.MinValue))
            {
                whereClause += $" AND tblProjects.Target_Date >= '{SelectedDateFrom}'";
            }

            if (!SelectedToDate.Equals(DateTime.MinValue))
            {
                whereClause += $" AND tblProjects.Date_Completed <= '{SelectedDateFrom}'";
            }

            //if (!string.IsNullOrEmpty(Keyword))
            //{
            //    whereClause += $" AND Project_Name = '{Keyword}'";
            //}

            sqlquery = "SELECT TOP 10 tblProjects.*, tblSalesmen.Salesman_Name FROM tblSalesmen RIGHT JOIN (SELECT tblprojects.*, tblCustomers.Full_Name FROM tblCustomers RIGHT JOIN(SELECT tblprojects.Project_ID, tblProjects.Project_Name, tblprojects.Job_No, tblprojects.Customer_ID, tblprojects.Salesman_ID FROM tblProjects RIGHT JOIN(SELECT DISTINCT Project_ID FROM tblProjectLabor) AS tblProjectID ON tblProjects.Project_ID = tblProjectID.Project_ID"+ whereClause +") AS tblProjects ON tblprojects.Customer_ID = tblCustomers.Customer_ID) AS tblProjects ON tblProjects.Salesman_ID = tblSalesmen.Salesman_ID ORDER BY Project_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportActiveLabor> st_activeLabors = new ObservableCollection<ReportActiveLabor>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                string _projectName = "";
                string _jobNo = "";
                string _customerName = "";
                string _salesmanName = "";

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Full_Name"))
                    _customerName = row["Full_Name"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();

                List<TrackLaborReport> _trackLaborReports = TrackLaborReports.Where(item => item.ProjectID == _projectID).ToList();

                st_activeLabors.Add(new ReportActiveLabor { ID = _projectID, ProjectName = _projectName, JobNo = _jobNo, CustomerName = _customerName, SalesmanName = _salesmanName, LaborReports= _trackLaborReports });
            }
            ReportActiveLabors = st_activeLabors;
        }

        public void LoadApprovedNotReleaseData()
        {
            string whereClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            //if (!string.IsNullOrEmpty(Keyword))
            //{
            //    whereClause += $" AND Project_Name = '{Keyword}'";
            //}

            sqlquery = "SELECT tblManufacturers.Manuf_Name, tblMat.* FROM tblManufacturers RIGHT JOIN (SELECT tblMaterials.Material_Desc, tblMat.* FROM tblMaterials RIGHT JOIN (SELECT tblSalesmen.Salesman_Name, tblMat.* FROM tblSalesmen RIGHT JOIN (SELECT tblMat.Project_ID, MatReqdDate, Job_No, Project_Name, Salesman_ID, Material_ID, Manuf_ID, SubmitAppr, Manuf_LeadTime FROM tblProjectMaterialsTrack INNER JOIN (SELECT tblMat.Project_ID, tblProjectMaterials.ProjMat_ID, Material_ID, Job_No, Project_Name, Salesman_ID FROM tblProjectMaterials INNER JOIN (SELECT Project_ID, Project_Name, Job_No, Salesman_ID FROM tblProjects " + whereClause + ") AS tblMat ON tblProjectMaterials.Project_ID = tblMat.Project_ID) AS tblMat ON tblProjectMaterialsTrack.ProjMat_ID = tblMat.ProjMat_ID) AS tblMat ON tblSalesmen.Salesman_ID = tblMat.Salesman_ID) AS tblMat ON tblMat.Material_ID = tblMaterials.Material_ID "+ whereClause2 +") AS tblMat ON tblMat.Manuf_ID = tblManufacturers.Manuf_ID" + whereClause1;

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportApproveNotRelease> sb_reportApproveNotReleases = new ObservableCollection<ReportApproveNotRelease>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                DateTime _matReqDate = new DateTime();
                string _jobNo = "";
                string _projectName = "";
                string _salesmanName = "";
                string _materialName = "";
                string _manufName = "";
                DateTime _submAppr = new DateTime();
                string _leadTime = "";

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matReqDate = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Material_Desc"))
                    _materialName = row["Material_Desc"].ToString();
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("SubmitAppr"))
                    _submAppr = row.Field<DateTime>("SubmitAppr");
                if (!row.IsNull("Manuf_LeadTime"))
                    _leadTime = row["Manuf_LeadTime"].ToString();

                sb_reportApproveNotReleases.Add(new ReportApproveNotRelease
                {
                    ProjectID = _projectID,
                    MatReqDate = _matReqDate,
                    JobNo = _jobNo,
                    ProjectName = _projectName,
                    SalesmanName = _salesmanName,
                    MaterialName = _materialName,
                    ManufName = _manufName,
                    SubmAppr = _submAppr,
                    LeadTime = _leadTime
                });
            }

            ReportApproveNotReleases = sb_reportApproveNotReleases;
        }

        public void LoadChangeOrders()
        {
            string whereClause = GetProjectWhereClasuse();

            sqlquery = "SELECT Project_ID, Salesman_ID, Job_No, Customer_ID, Project_Name, Complete, tblCOTracking.COID, tblCOTracking.Contractnumber, tblCOTracking.DateOfCO, tblCOTracking.AmtOfCO, tblCOTracking.ChangeOrderNo, tblCOTracking.SignedoffbySales, tblCOTracking.Givenforfinalsignature, tblCOTracking.ExecutedForReturn, tblCOTracking.ReturnedVia, tblCOTracking.Comments, tblCOTracking.DateProcessed, tblCOTracking.Scope, tblCOTracking.Datereturnedback FROM tblProjects INNER JOIN tblCOTracking ON tblProjects.Project_ID = tblCOTracking.ProjectID" + whereClause;
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportCOItem> st_reportCOItems = new ObservableCollection<ReportCOItem>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                string _contract = "";
                string _changeOrderNo = "";
                DateTime _dateOFCO = new DateTime();
                string _amtOfCO = "";
                DateTime _dateProcessed = new DateTime();
                DateTime _signedOffBySales = new DateTime();
                DateTime _finalSig = new DateTime();
                DateTime _returnedBack = new DateTime();
                string _returnedVia = "";
                string _scope = "";
                string _comment = "";

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("Contractnumber"))
                    _contract = row["Contractnumber"].ToString();
                if (!row.IsNull("ChangeOrderNo"))
                    _changeOrderNo = row["ChangeOrderNo"].ToString();
                if (!row.IsNull("DateOfCO"))
                    _dateOFCO = row.Field<DateTime>("DateOfCO");
                if (!row.IsNull("AmtOfCO"))
                    _amtOfCO = row["AmtOfCO"].ToString();
                if (!row.IsNull("DateProcessed"))
                    _dateProcessed = row.Field<DateTime>("DateProcessed");
                if (!row.IsNull("SignedoffbySales"))
                    _signedOffBySales = row.Field<DateTime>("SignedoffbySales");
                if (!row.IsNull("Givenforfinalsignature"))
                    _finalSig = row.Field<DateTime>("Givenforfinalsignature");
                if (!row.IsNull("Datereturnedback"))
                    _returnedBack = row.Field<DateTime>("Datereturnedback");
                if (!row.IsNull("ReturnedVia"))
                    _returnedVia = row["ReturnedVia"].ToString();
                if (!row.IsNull("Scope"))
                    _scope = row["Scope"].ToString();
                if (!row.IsNull("Comments"))
                    _comment = row["Comments"].ToString();

                st_reportCOItems.Add(new ReportCOItem
                {
                    ProjectID = _projectID,
                    Contract = _contract,
                    ChangeOrderNO = _changeOrderNo,
                    DateOFCO = _dateOFCO,
                    AmtOfCO = _amtOfCO,
                    DateProcessed = _dateProcessed,
                    SalesDate = _signedOffBySales,
                    FinalSig = _finalSig,
                    ReturnedBack = _returnedBack,
                    ReturnedVia = _returnedVia,
                    Scope = _scope,
                    Comment = _comment
                });
            }
            ReportCOItems = st_reportCOItems;


            sqlquery = "SELECT DISTINCT tblProjects.* FROM tblCOTracking INNER JOIN (SELECT tblCustomers.Full_Name, tblProjects.* FROM tblCustomers INNER JOIN (SELECT tblSalesmen.Salesman_Name, tblProjects.* FROM tblSalesmen RIGHT JOIN(SELECT Project_ID, Project_Name, Job_No, Salesman_ID, Customer_ID FROM tblProjects RIGHT JOIN(SELECT DISTINCT tblCOTracking.ProjectID FROM tblCOTracking) AS tblCOTracking ON tblCOTracking.ProjectID = tblProjects.Project_ID "+ whereClause +") AS tblProjects ON tblProjects.Salesman_ID = tblSalesmen.Salesman_ID) AS tblProjects ON tblProjects.Customer_ID = tblCustomers.Customer_ID) AS tblProjects ON tblCOTracking.ProjectID = tblProjects.Project_ID ORDER BY Project_ID;";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportCO> st_reportCOs = new ObservableCollection<ReportCO>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                string _jobNo = "";
                string _projectName = "";
                string _salesmanName = "";
                string _customerName = "";
                List<ReportCOItem> _reportCOItems;

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Full_Name"))
                    _customerName = row["Full_Name"].ToString();
                _reportCOItems = ReportCOItems.Where(item => item.ProjectID == _projectID).ToList();
                st_reportCOs.Add(new ReportCO
                {
                    ProjectID = _projectID,
                    JobNo = _jobNo,
                    ProjectName = _projectName,
                    SalesmanName = _salesmanName,
                    CustomerName = _customerName,
                    ReportCOItems = _reportCOItems
                });
            }
            ReportCOs = st_reportCOs;
        }

        public void LoadCIPData()
        {
            string whereClause = GetProjectWhereClasuse();

            sqlquery = "SELECT * FROM tblCIPs";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            ObservableCollection<CIP> sb_cip = new ObservableCollection<CIP>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                string _cipType = "";
                DateTime _formsRecD = new DateTime();
                DateTime _formsSent = new DateTime();
                DateTime _certRecD = new DateTime();
                string _crewEnrolled = "";
                DateTime _exemptionAppDate = new DateTime();
                string _notes = "";

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("CIPType"))
                    _cipType = row["CIPType"].ToString();
                if (!row.IsNull("FormsRecD"))
                    _formsRecD = row.Field<DateTime>("FormsRecD");
                if (!row.IsNull("FormsSent"))
                    _formsSent = row.Field<DateTime>("FormsSent");
                if (!row.IsNull("CertRecD"))
                    _certRecD = row.Field<DateTime>("CertRecD");
                if (!row.IsNull("CrewEnrolled"))
                    _crewEnrolled = row["CrewEnrolled"].ToString();
                if (!row.IsNull("ExemptionAppDate"))
                    _exemptionAppDate = row.Field<DateTime>("ExemptionAppDate");
                if (!row.IsNull("Notes"))
                    _notes = row["Notes"].ToString();
                sb_cip.Add(new CIP
                {
                    ProjectID = _projectID,
                    CipType = _cipType,
                    FormsRecD = _formsRecD,
                    FormsSent = _formsSent,
                    CertRecD = _certRecD,
                    CrewEnrolled = _crewEnrolled,
                    ExemptionAppDate = _exemptionAppDate,
                    Notes = _notes
                });
            }
            CIPs = sb_cip;

            sqlquery = "SELECT tblProjects.*, tblSalesmen.Salesman_Name FROM tblSalesmen RIGHT JOIN (SELECT tblProjects.*, tblCustomers.Full_Name FROM tblCustomers RIGHT JOIN(SELECT tblprojs.Project_ID, Job_No, CIPType, OriginalContractAmt, FinalContractAmt, FormsRecD, FormsSent, CertRecD, ExemptionApproved, ExemptionAppDate, CrewEnrolled, Notes, Customer_ID, Salesman_ID, tblProjs.Target_Date FROM tblCIPs INNER JOIN(SELECT Project_ID, Job_No, Salesman_ID, Customer_ID, Target_Date FROM tblProjects "+ whereClause +") AS tblProjs ON tblCIPs.Project_ID = tblProjs.Project_ID) AS tblProjects ON tblProjects.Customer_ID = tblCustomers.Customer_ID) AS tblProjects ON tblProjects.Salesman_ID = tblSalesmen.Salesman_ID";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportCIP> sb_reportCIP = new ObservableCollection<ReportCIP>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                string _jobNo = "";
                DateTime _targetDate = new DateTime();
                string _salesmanName = "";
                string _customerName = "";

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Target_Date"))
                    _targetDate = row.Field<DateTime>("Target_Date");
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Full_Name"))
                    _customerName = row["Full_Name"].ToString();

                List<CIP> sb_cipList = CIPs.Where(item => item.ProjectID == _projectID).ToList();

                sb_reportCIP.Add(new ReportCIP
                {
                    ProjectID = _projectID,
                    JobNo = _jobNo,
                    TargetDate = _targetDate,
                    SalesmanName = _salesmanName,
                    CustomerName = _customerName,
                    CIPs = sb_cipList
                });
            }

            ReportCIPs = sb_reportCIP;
        }

        public void LoadContractData()
        {
            string whereClause = GetProjectWhereClasuse();

            sqlquery = "SELECT Project_ID, Contractnumber, ChangeOrderNo, DateProcessed, SignedoffbySales, GivenAcctingforreview, Givenforfinalsignature, ReturnedtoDawn,  ReturnedVia, Comments FROM tblSC INNER JOIN (SELECT Project_ID, Project_Name, Job_No, Salesman_ID, Customer_ID FROM tblProjects) AS tblProjects ON tblProjects.Project_ID = tblSC.ProjectID";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Contract> sb_contract = new ObservableCollection<Contract>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                string _contractNumber = "";
                string _changeOrderNo = "";
                DateTime _dateProcessed = new DateTime();
                DateTime _signedoffbySales = new DateTime();
                DateTime _givenAcctingforreview = new DateTime();
                DateTime _givenforfinalsignature = new DateTime();
                string _returnedVia = "";
                DateTime _returnedDate = new DateTime();
                string _comment = "";

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("Contractnumber"))
                    _contractNumber = row["Contractnumber"].ToString();
                if (!row.IsNull("ChangeOrderNo"))
                    _changeOrderNo = row["ChangeOrderNo"].ToString();
                if (!row.IsNull("DateProcessed"))
                    _dateProcessed = row.Field<DateTime>("DateProcessed");
                if (!row.IsNull("SignedoffbySales"))
                    _signedoffbySales = row.Field<DateTime>("SignedoffbySales");
                if (!row.IsNull("GivenAcctingforreview"))
                    _givenAcctingforreview = row.Field<DateTime>("GivenAcctingforreview");
                if (!row.IsNull("Givenforfinalsignature"))
                    _givenforfinalsignature = row.Field<DateTime>("Givenforfinalsignature");
                if (!row.IsNull("ReturnedVia"))
                    _returnedVia = row["ReturnedVia"].ToString();
                if (!row.IsNull("ReturnedtoDawn"))
                    _returnedDate = row.Field<DateTime>("ReturnedtoDawn");
                if (!row.IsNull("Comments"))
                    _comment = row["Comments"].ToString();

                sb_contract.Add(new Contract
                {
                    ContractNumber = _contractNumber,
                    ChangeOrderNo = _changeOrderNo,
                    DateProcessed = _dateProcessed,
                    SignedoffbySales = _signedoffbySales,
                    GivenAcctingforreview = _givenAcctingforreview,
                    Givenforfinalsignature = _givenforfinalsignature,
                    ReturnedDate = _returnedDate,
                    ReturnedVia = _returnedVia,
                    Comment = _comment,
                    ProjectID = _projectID
                });
            }

            sqlquery = "SELECT tblCustomers.Full_Name, tblProject.* FROM tblCustomers RIGHT JOIN (    SELECT Salesman_Name, tblProject.*FROM tblSalesmen RIGHT JOIN(SELECT Project_ID, Project_Name, Job_No, Salesman_ID, Customer_ID FROM tblProjects RIGHT JOIN(SELECT DISTINCT(ProjectID) FROM tblSC) AS tblSC ON tblProjects.Project_ID = tblSC.ProjectID "+ whereClause +") AS tblProject ON tblProject.Salesman_ID = tblSalesmen.Salesman_ID) AS tblProject ON tblproject.Customer_ID = tblCustomers.Customer_ID; ";
           
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportContract> sb_reportContracts = new ObservableCollection<ReportContract>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                string _projectName = "";
                string _jobNo = "";
                string _salesmanName = "";
                string _customerName = "";
                List<Contract> _contracts;

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Full_Name"))
                    _customerName = row["Full_Name"].ToString();
                _contracts = sb_contract.Where(item => item.ProjectID == _projectID).ToList();

                sb_reportContracts.Add(new ReportContract
                {
                    ProjectID = _projectID,
                    JobNo = _jobNo,
                    ProjectName = _projectName,
                    SalesmanName = _salesmanName,
                    CustomerName = _customerName,
                    Contracts = _contracts
                });
            }
            ReportContracts = sb_reportContracts;
        }

        public void LoadCustomerContactData()
        {
            string whereCustomerClause = GetCustomerWhereClasuse();

            sqlquery = "SELECT tblProjectManagers.* FROM tblProjectManagers INNER JOIN tblCustomers ON tblProjectManagers.Customer_ID = tblCustomers.Customer_ID WHERE tblProjectManagers.Active = 1 ORDER BY tblCustomers.Full_Name;";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ProjectManager> sb_projectManagers = new ObservableCollection<ProjectManager>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int pmID = int.Parse(row["PM_ID"].ToString());
                string pmName = row["PM_Name"].ToString();
                string pmPhoneNumber = row["PM_Phone"].ToString();
                string pmCellPhone = row["PM_CellPhone"].ToString();
                string pmEmail = row["PM_Email"].ToString();
                int customerID = int.Parse(row["Customer_ID"].ToString());

                sb_projectManagers.Add(new ProjectManager
                {
                    ID = pmID,
                    PMName = pmName,
                    PMPhone = pmPhoneNumber,
                    PMCellPhone = pmCellPhone,
                    PMEmail = pmEmail,
                    CustomerID = customerID
                });
            }

            ProjectManagers = sb_projectManagers;
            sqlquery = "SELECT tblCustomers.Customer_ID, Full_Name FROM tblCustomers RIGHT JOIN (SELECT DISTINCT Customer_ID FROM tblProjectManagers) AS tblPM ON tblCustomers.Customer_ID = tblPM.Customer_ID "+ whereCustomerClause + " AND tblCustomers.Active = 1 ORDER BY Full_Name";
           
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            List<ProjectManager>  sb_pms = new List<ProjectManager>();
            ObservableCollection<ReportCustomerContact> sb_customerContacts = new ObservableCollection<ReportCustomerContact>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int customerID = 0;
                string fullName = "";

                if (!row.IsNull("Customer_ID"))
                    customerID = int.Parse(row["Customer_ID"].ToString());
                if (!row.IsNull("Full_Name"))
                    fullName = row["Full_Name"].ToString();

                sb_pms = ProjectManagers.Where(item => item.CustomerID == customerID).ToList();
                sb_customerContacts.Add(new ReportCustomerContact
                {
                    CustomerID = customerID,
                    FullName = fullName,
                    ProjectManagers = sb_pms
                });
            }
            ReportCustomerContacts = sb_customerContacts;
        }

        public void LoadFieldMeasureData()
        {
            string whereProjectClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            sqlquery = "SELECT tblProjectChangeOrders.CO_ItemNo, tblProjects.* FROM tblProjectChangeOrders RIGHT JOIN(SELECT tblProjectSOV.CO_ID, tblProjects.* FROM tblProjectSOV RIGHT JOIN(SELECT tblManufacturers.Manuf_Name, tblProjects.* FROM tblManufacturers RIGHT JOIN(SELECT tblProjectMaterialsTrack.MatReqdDate, tblProjectMaterialsTrack.Manuf_ID, tblProjectMaterialsTrack.SubmitAppr, tblProjectMaterialsTrack.FM_Needed, tblProjectMaterialsTrack.Field_Dim, tblProjectMaterialsTrack.Guar_Dim, tblProjectMaterialsTrack.ReleasedForFab, tblProjects.* FROM tblProjectMaterialsTrack INNER JOIN(SELECT tblProjectMaterials.ProjMat_ID, tblProjectMaterials.ProjSOV_ID, tblProjectMaterials.Mat_Only, tblProjectMaterials.Mat_Phase, tblProjectMaterials.Mat_Type, tblProjects.* FROM tblProjectMaterials RIGHT JOIN(SELECT tblSalesmen.Salesman_Name, tblProjects.* FROM tblSalesmen RIGHT JOIN(SELECT tblCustomers.Full_Name, tblProjects.* FROM tblCustomers RIGHT JOIN(SELECT tblProjects.Project_ID, tblProjects.Project_Name, tblProjects.Target_Date, tblProjects.Date_Completed, tblProjects.Address, tblProjects.State, tblProjects.ZIP, tblProjects.Job_No, tblProjects.Salesman_ID, tblProjects.Customer_ID, tblProjects.Stored_Materials, tblProjects.Billing_Date FROM tblProjects "+ whereProjectClause +") AS tblProjects ON tblProjects.Customer_ID = tblCustomers.Customer_ID) AS tblProjects ON tblProjects.Salesman_ID = tblSalesmen.Salesman_ID) AS tblProjects ON tblProjectMaterials.Project_ID = tblProjects.Project_ID) AS tblProjects ON tblProjects.ProjMat_ID = tblProjectMaterialsTrack.ProjMat_ID) AS tblProjects ON tblProjects.Manuf_ID = tblManufacturers.Manuf_ID "+ whereClause1 +") AS tblProjects ON tblProjects.ProjSOV_ID = tblProjectSOV.ProjSOV_ID) AS tblProjects ON tblProjectChangeOrders.CO_ID = tblProjects.CO_ID";

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            ObservableCollection<ProjectMatTracking> sb_matTrackings = new ObservableCollection<ProjectMatTracking>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                int _projMat = 0;
                DateTime _matReqdDate = new DateTime();
                bool _matOnly = false;
                string _manufName = "";
                string _matPhase = "";
                string _matType = "";
                string _orderNo = "";
                DateTime _submAppr = new DateTime();
                bool _needFM = false;
                DateTime _fieldDim = new DateTime();
                bool _guarDim = false;
                DateTime _rFF = new DateTime();

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("ProjMat_ID"))
                    _projMat = int.Parse(row["ProjMat_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matReqdDate = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Mat_Only"))
                    _matOnly = row.Field<Boolean>("Mat_Only");
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("Mat_Phase"))
                    _matPhase = row["Mat_Phase"].ToString();
                if (!row.IsNull("Mat_Type"))
                    _matType = row["Mat_Type"].ToString();
                if (!row.IsNull("CO_ItemNo"))
                    _orderNo = row["CO_ItemNo"].ToString();
                if (!row.IsNull("SubmitAppr"))
                    _submAppr = row.Field<DateTime>("SubmitAppr");
                if (!row.IsNull("FM_Needed"))
                    _needFM = row.Field<Boolean>("FM_Needed");
                if (!row.IsNull("Field_Dim"))
                    _fieldDim = row.Field<DateTime>("Field_Dim");
                if (!row.IsNull("Guar_Dim"))
                    _guarDim = row.Field<Boolean>("Guar_Dim");
                if (!row.IsNull("ReleasedForFab"))
                    _rFF = row.Field<DateTime>("ReleasedForFab");

                sb_matTrackings.Add(new ProjectMatTracking
                {
                    ProjectID = _projectID,
                    ProjMat = _projMat,
                    MatReqdDate = _matReqdDate,
                    MatOnly = _matOnly,
                    ManufName = _manufName,
                    MatPhase = _matPhase,
                    MatType = _matType,
                    ManuOrderNo = _orderNo,
                    SubmAppr = _submAppr,
                    NeedFM = _needFM,
                    FieldDim = _fieldDim,
                    GuarDim = _guarDim,
                    RFF = _rFF
                });
            }
            ProjectMatTrackings = sb_matTrackings;

            // Notes
            sqlquery = "SELECT * FROM tblNotes WHERE Notes_PK_Desc = 'Project'";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            ObservableCollection<Note> sb_projectNotes = new ObservableCollection<Note>();
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
                sb_projectNotes.Add(new Note
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

            // Field Measure
            sqlquery = "SELECT DISTINCT tblProjects.* FROM tblProjectMaterialsTrack INNER JOIN (SELECT tblProjectMaterials.ProjMat_ID, tblProjects.* FROM tblProjectMaterials INNER JOIN(SELECT tblSalesmen.Salesman_Name, tblProjects.* FROM tblSalesmen RIGHT JOIN(SELECT tblCustomers.Full_Name, tblProjects.* FROM tblCustomers RIGHT JOIN(SELECT tblProjects.Project_ID, tblProjects.Project_Name, tblProjects.Target_Date, tblProjects.Date_Completed, tblProjects.Address, tblProjects.State, tblProjects.ZIP, tblProjects.Job_No, tblProjects.Salesman_ID, tblProjects.Customer_ID, tblProjects.Stored_Materials, tblProjects.Billing_Date FROM tblProjects) AS tblProjects ON tblProjects.Customer_ID = tblCustomers.Customer_ID) AS tblProjects ON tblProjects.Salesman_ID = tblSalesmen.Salesman_ID) AS tblProjects ON tblProjectMaterials.Project_ID = tblProjects.Project_ID) AS tblProjects ON tblProjects.ProjMat_ID = tblProjectMaterialsTrack.ProjMat_ID; ";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportFieldMeasure> sb_reportFieldMeasures = new ObservableCollection<ReportFieldMeasure>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                string _projectName = "";
                DateTime _targetDate = new DateTime();
                DateTime _dateComplete = new DateTime();
                string _address = "";
                string _state = "";
                string _zip = "";
                string _jobNo = "";
                string _salesmanName = "";
                string _customerName = "";
                bool _storedMat = false;
                string _notes = "";
                int _billingDue = 0;
                List<ProjectMatTracking> _projectMatTrackings = new List<ProjectMatTracking>();

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Target_Date"))
                    _targetDate = row.Field<DateTime>("Target_Date");
                if (!row.IsNull("Date_Completed"))
                    _dateComplete = row.Field<DateTime>("Date_Completed");
                if (!row.IsNull("Address"))
                    _address = row["Address"].ToString();
                if (!row.IsNull("State"))
                    _state = row["State"].ToString();
                if (!row.IsNull("Zip"))
                    _zip = row["Zip"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Full_Name"))
                    _customerName = row["Full_Name"].ToString();
                if (!row.IsNull("Stored_Materials"))
                    _storedMat = row.Field<Boolean>("Stored_Materials");
                List<Note> sb_Notes = sb_projectNotes.Where(item => item.NotePK == _projectID).ToList();
                foreach(Note item in sb_Notes)
                {
                    _notes += item.NotesNote;
                }
                if (!row.IsNull("Billing_Date"))
                    _billingDue = int.Parse(row["Billing_Date"].ToString());

                _projectMatTrackings = ProjectMatTrackings.Where(item => item.ProjectID == _projectID).ToList();

                sb_reportFieldMeasures.Add(new ReportFieldMeasure
                {
                    ProjectID = _projectID,
                    ProjectName = _projectName,
                    TargetDate = _targetDate,
                    DateComplete = _dateComplete,
                    Address = _address,
                    State = _state,
                    Zip = _zip,
                    JobNo = _jobNo,
                    SalesmanName = _salesmanName,
                    CustomerName = _customerName,
                    StoredMat = _storedMat,
                    Notes = _notes,
                    BillingDue = _billingDue,
                    ProjectMatTrackings = _projectMatTrackings
                });
            }
            ReportFieldMeasures = sb_reportFieldMeasures;
        }

        public void LoadArchRepData()
        {
            string whereProjectClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            sqlquery = "SELECT tblManufacturers.Manuf_Name, tblManuf.* FROM tblManufacturers RIGHT JOIN (SELECT tblProjectChangeOrders.CO_ItemNo, tblCOItem.* FROM tblProjectChangeOrders RIGHT JOIN(SELECT tblMaterials.Material_Desc, tblProjects.* FROM tblMaterials RIGHT JOIN(SELECT tblManufacturers.Manuf_Name, tblprojects.* FROM tblManufacturers RIGHT JOIN(SELECT tblProjectMaterialsTrack.MatReqdDate, tblProjectMaterialsTrack.ProjMat_ID, CO_ID, Manuf_ID, Mat_Phase, Mat_Type, SOV_Acronym, Material_ID, ReleasedForFab, Complete, tblProjects.Project_ID FROM tblProjectMaterialsTrack INNER JOIN(SELECT tblProjectSOV.CO_ID, tblProjectSOV.SOV_Acronym, tblprojects.* FROM tblProjectSOV RIGHT JOIN(SELECT tblProjectMaterials.ProjMat_ID, tblProjectMaterials.Material_ID, tblProjectMaterials.ProjSOV_ID, tblProjectMaterials.Mat_Only, tblProjectMaterials.Mat_Phase, tblProjectMaterials.Mat_Type, tblProjects.* FROM tblProjectMaterials RIGHT JOIN(SELECT * FROM tblProjects) AS tblProjects ON tblProjectMaterials.Project_ID = tblProjects.Project_ID) AS tblProjects ON tblProjects.ProjSOV_ID = tblProjectSOV.ProjSOV_ID) AS tblProjects ON tblProjectMaterialsTrack.ProjMat_ID = tblProjects.ProjMat_ID) AS tblProjects ON tblProjects.Manuf_ID = tblManufacturers.Manuf_ID "+ whereClause1 +") AS tblProjects on tblProjects.Material_ID = tblMaterials.Material_ID) AS tblCOItem ON tblCOItem.CO_ID = tblProjectChangeOrders.CO_ID) AS tblManuf ON tblManuf.Manuf_ID = tblManufacturers.Manuf_ID";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            ObservableCollection<ProjectMatTracking> sb_matTrackings = new ObservableCollection<ProjectMatTracking>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                int _projMat = 0;
                DateTime _matReqdDate = new DateTime();
                string _manufName = "";
                string _matPhase = "";
                string _matType = "";
                string _matDesc = "";
                string _orderNo = "";
                DateTime _rFF = new DateTime();
                bool _orderComplete = false;

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if(!row.IsNull("ProjMat_ID"))
                    _projMat = int.Parse(row["ProjMat_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matReqdDate = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("CO_ItemNo"))
                    _orderNo = row["CO_ItemNo"].ToString();
                if (!row.IsNull("Mat_Phase"))
                    _matPhase = row["Mat_Phase"].ToString();
                if (!row.IsNull("Mat_Type"))
                    _matType = row["Mat_Type"].ToString();
                if (!row.IsNull("Material_Desc"))
                    _matDesc = row["Material_Desc"].ToString();
                if (!row.IsNull("ReleasedForFab"))
                      _rFF = row.Field<DateTime>("ReleasedForFab");
                if (!row.IsNull("Complete"))
                    _orderComplete = row.Field<Boolean>("Complete");
                sb_matTrackings.Add(new ProjectMatTracking
                {
                    ProjectID = _projectID,
                    ProjMat = _projMat,
                    MatReqdDate = _matReqdDate,
                    ManufName = _manufName,
                    ManuOrderNo = _orderNo,
                    MatPhase = _matPhase,
                    MatType = _matType,
                    MatDesc = _matDesc,
                    RFF = _rFF,
                    OrderComplete = _orderComplete
                });
            }
            ProjectMatTrackings = sb_matTrackings;

            sqlquery = "SELECT tblArchRep.Arch_Rep_Name, tblProjArchRep.* FROM tblArchRep RIGHT JOIN (SELECT tblArchitects.Arch_Company, tblProjArch.* FROM tblArchitects RIGHT JOIN(SELECT tblSalesmen.Salesman_Name, tblProjSalesman.* FROM tblSalesmen RIGHT JOIN(SELECT tblCustomers.Full_Name, tblProjCustomer.* FROM tblCustomers RIGHT JOIN(SELECT tblProjects.Project_ID, Project_Name, Target_Date, Arch_Rep_ID, Architect_ID, Customer_ID, Job_No, Salesman_ID, tblProjects.Address, tblProjects.State, tblProjects.ZIP FROM tblProjects INNER JOIN(SELECT DISTINCT tblProjectMat.Project_ID FROM tblProjectMaterialsTrack INNER JOIN(SELECT tblProjectMaterials.ProjMat_ID, tblProjectMat.* FROM tblProjectMaterials INNER JOIN(SELECT Project_ID, Project_Name, Customer_ID, Job_No, Salesman_ID FROM tblProjects " + whereProjectClause + ") AS tblProjectMat ON tblProjectMat.Project_ID = tblProjectMaterials.Project_ID) AS tblProjectMat ON tblProjectMat.ProjMat_ID = tblProjectMaterialsTrack.ProjMat_ID) AS tblProj ON tblProj.Project_ID = tblProjects.Project_ID) AS tblProjCustomer ON tblProjCustomer.Customer_ID = tblCustomers.Customer_ID) AS tblProjSalesman ON tblProjSalesman.Salesman_ID = tblSalesmen.Salesman_ID) AS tblProjArch ON tblProjArch.Architect_ID = tblArchitects.Architect_ID) AS tblProjArchRep ON tblProjArchRep.Arch_Rep_ID = tblArchRep.Arch_Rep_ID ORDER BY Target_Date";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportJobArchRep> sb_reportJobArch = new ObservableCollection<ReportJobArchRep>();
            foreach(DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                int _projMat = 0;
                DateTime _targetDate = new DateTime();
                string _architect = "";
                string _archRep = "";
                string _projectName = "";
                string _customerName = "";
                string _salesmanName = "";
                string _address = "";
                string _state = "";
                string _zip = "";
                string _jobNo = "";
                List<ProjectMatTracking> _projectMatTrackings = new List<ProjectMatTracking>();

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                //if (!row.IsNull("ProjMat_ID"))
                //    _projMat = int.Parse(row["ProjMat_ID"].ToString());
                if (!row.IsNull("Target_Date"))
                    _targetDate = row.Field<DateTime>("Target_Date");
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Arch_Company"))
                    _architect = row["Arch_Company"].ToString();
                if (!row.IsNull("Arch_Rep_Name"))
                    _archRep = row["Arch_Rep_Name"].ToString();
                if (!row.IsNull("Full_Name"))
                    _customerName = row["Full_Name"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Address"))
                    _address = row["Address"].ToString();
                if (!row.IsNull("State"))
                    _state = row["State"].ToString();
                if (!row.IsNull("Zip"))
                    _zip = row["Zip"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();

                _projectMatTrackings = ProjectMatTrackings.Where(item => item.ProjectID == _projectID).ToList();

                sb_reportJobArch.Add(new ReportJobArchRep
                {
                   ProjectID = _projectID,
                   //ProjectMatID = _projMat,
                   ProjectName = _projectName,
                   TargetDate = _targetDate,
                   Architect = _architect,
                   ArchRep = _archRep,
                   CustomerName = _customerName,
                   SalesmanName = _salesmanName,
                   Address = _address,
                   State = _state,
                   Zip = _zip,
                   JobNo = _jobNo,
                   ProjectMatTrackings = _projectMatTrackings
                });
            }
            ReportJobArchReps = sb_reportJobArch;
        }

        public void LoadArchitectData()
        {
            string whereProjectClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            sqlquery = "SELECT tblManufacturers.Manuf_Name, tblManuf.* FROM tblManufacturers RIGHT JOIN (SELECT tblProjectChangeOrders.CO_ItemNo, tblCOItem.* FROM tblProjectChangeOrders RIGHT JOIN(SELECT tblMaterials.Material_Desc, tblProjects.* FROM tblMaterials RIGHT JOIN(SELECT tblManufacturers.Manuf_Name, tblprojects.* FROM tblManufacturers RIGHT JOIN(SELECT tblProjectMaterialsTrack.MatReqdDate, tblProjectMaterialsTrack.ProjMat_ID, CO_ID, Manuf_ID, Mat_Phase, Mat_Type, SOV_Acronym, Material_ID, ReleasedForFab, Complete, tblProjects.Project_ID FROM tblProjectMaterialsTrack INNER JOIN(SELECT tblProjectSOV.CO_ID, tblProjectSOV.SOV_Acronym, tblprojects.* FROM tblProjectSOV RIGHT JOIN(SELECT tblProjectMaterials.ProjMat_ID, tblProjectMaterials.Material_ID, tblProjectMaterials.ProjSOV_ID, tblProjectMaterials.Mat_Only, tblProjectMaterials.Mat_Phase, tblProjectMaterials.Mat_Type, tblProjects.* FROM tblProjectMaterials RIGHT JOIN(SELECT * FROM tblProjects) AS tblProjects ON tblProjectMaterials.Project_ID = tblProjects.Project_ID) AS tblProjects ON tblProjects.ProjSOV_ID = tblProjectSOV.ProjSOV_ID) AS tblProjects ON tblProjectMaterialsTrack.ProjMat_ID = tblProjects.ProjMat_ID) AS tblProjects ON tblProjects.Manuf_ID = tblManufacturers.Manuf_ID "+ whereClause1 +") AS tblProjects on tblProjects.Material_ID = tblMaterials.Material_ID) AS tblCOItem ON tblCOItem.CO_ID = tblProjectChangeOrders.CO_ID) AS tblManuf ON tblManuf.Manuf_ID = tblManufacturers.Manuf_ID";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            ObservableCollection<ProjectMatTracking> sb_matTrackings = new ObservableCollection<ProjectMatTracking>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                int _projMat = 0;
                DateTime _matReqdDate = new DateTime();
                string _manufName = "";
                string _matPhase = "";
                string _matType = "";
                string _matDesc = "";
                string _orderNo = "";
                DateTime _rFF = new DateTime();
                bool _orderComplete = false;

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("ProjMat_ID"))
                    _projMat = int.Parse(row["ProjMat_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matReqdDate = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("CO_ItemNo"))
                    _orderNo = row["CO_ItemNo"].ToString();
                if (!row.IsNull("Mat_Phase"))
                    _matPhase = row["Mat_Phase"].ToString();
                if (!row.IsNull("Mat_Type"))
                    _matType = row["Mat_Type"].ToString();
                if (!row.IsNull("Material_Desc"))
                    _matDesc = row["Material_Desc"].ToString();
                if (!row.IsNull("ReleasedForFab"))
                    _rFF = row.Field<DateTime>("ReleasedForFab");
                if (!row.IsNull("Complete"))
                    _orderComplete = row.Field<Boolean>("Complete");
                sb_matTrackings.Add(new ProjectMatTracking
                {
                    ProjectID = _projectID,
                    ProjMat = _projMat,
                    MatReqdDate = _matReqdDate,
                    ManufName = _manufName,
                    ManuOrderNo = _orderNo,
                    MatPhase = _matPhase,
                    MatType = _matType,
                    MatDesc = _matDesc,
                    RFF = _rFF,
                    OrderComplete = _orderComplete
                });
            }
            ProjectMatTrackings = sb_matTrackings;

            sqlquery = "SELECT tblArchRep.Arch_Rep_Name, tblProjArchRep.* FROM tblArchRep RIGHT JOIN (SELECT tblArchitects.Arch_Company, tblProjArch.* FROM tblArchitects RIGHT JOIN(SELECT tblSalesmen.Salesman_Name, tblProjSalesman.* FROM tblSalesmen RIGHT JOIN(SELECT tblCustomers.Full_Name, tblProjCustomer.* FROM tblCustomers RIGHT JOIN(SELECT tblProjects.Project_ID, Project_Name, Target_Date, Arch_Rep_ID, Architect_ID, Customer_ID, Job_No, Salesman_ID, tblProjects.Address, tblProjects.State, tblProjects.ZIP FROM tblProjects INNER JOIN(SELECT DISTINCT tblProjectMat.Project_ID FROM tblProjectMaterialsTrack INNER JOIN(SELECT tblProjectMaterials.ProjMat_ID, tblProjectMat.* FROM tblProjectMaterials INNER JOIN(SELECT Project_ID, Project_Name, Customer_ID, Job_No, Salesman_ID FROM tblProjects " + whereProjectClause + ") AS tblProjectMat ON tblProjectMat.Project_ID = tblProjectMaterials.Project_ID) AS tblProjectMat ON tblProjectMat.ProjMat_ID = tblProjectMaterialsTrack.ProjMat_ID) AS tblProj ON tblProj.Project_ID = tblProjects.Project_ID) AS tblProjCustomer ON tblProjCustomer.Customer_ID = tblCustomers.Customer_ID) AS tblProjSalesman ON tblProjSalesman.Salesman_ID = tblSalesmen.Salesman_ID) AS tblProjArch ON tblProjArch.Architect_ID = tblArchitects.Architect_ID) AS tblProjArchRep ON tblProjArchRep.Arch_Rep_ID = tblArchRep.Arch_Rep_ID ORDER BY Project_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportJobArchRep> sb_reportJobArch = new ObservableCollection<ReportJobArchRep>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                int _projMat = 0;
                DateTime _targetDate = new DateTime();
                string _architect = "";
                string _archRep = "";
                string _projectName = "";
                string _customerName = "";
                string _salesmanName = "";
                string _address = "";
                string _state = "";
                string _zip = "";
                string _jobNo = "";
                List<ProjectMatTracking> _projectMatTrackings = new List<ProjectMatTracking>();

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                //if (!row.IsNull("ProjMat_ID"))
                //    _projMat = int.Parse(row["ProjMat_ID"].ToString());
                if (!row.IsNull("Target_Date"))
                    _targetDate = row.Field<DateTime>("Target_Date");
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Arch_Company"))
                    _architect = row["Arch_Company"].ToString();
                if (!row.IsNull("Arch_Rep_Name"))
                    _archRep = row["Arch_Rep_Name"].ToString();
                if (!row.IsNull("Full_Name"))
                    _customerName = row["Full_Name"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Address"))
                    _address = row["Address"].ToString();
                if (!row.IsNull("State"))
                    _state = row["State"].ToString();
                if (!row.IsNull("Zip"))
                    _zip = row["Zip"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();

                _projectMatTrackings = ProjectMatTrackings.Where(item => item.ProjectID == _projectID).ToList();

                sb_reportJobArch.Add(new ReportJobArchRep
                {
                    ProjectID = _projectID,
                    //ProjectMatID = _projMat,
                    ProjectName = _projectName,
                    TargetDate = _targetDate,
                    Architect = _architect,
                    ArchRep = _archRep,
                    CustomerName = _customerName,
                    SalesmanName = _salesmanName,
                    Address = _address,
                    State = _state,
                    Zip = _zip,
                    JobNo = _jobNo,
                    ProjectMatTrackings = _projectMatTrackings
                });
            }
            ReportJobArchReps = sb_reportJobArch;
        }

        public void LoadActiveProjectData()
        {
            string whereProjectClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            sqlquery = "SELECT SchedShipDate, tblProjMat.* FROM tblProjectMaterialsShip RIGHT JOIN (SELECT tblProjectChangeOrders.CO_ItemNo, tblCOItem.* FROM tblProjectChangeOrders RIGHT JOIN(SELECT tblMaterials.Material_Desc, tblProjects.* FROM tblMaterials RIGHT JOIN(SELECT tblManufacturers.Manuf_Name, tblprojects.* FROM tblManufacturers RIGHT JOIN(SELECT tblProjectMaterialsTrack.MatReqdDate, tblProjectMaterialsTrack.ProjMat_ID, tblProjectMaterialsTrack.ProjMT_ID, CO_ID, Manuf_ID, Mat_Phase, Mat_Type, SOV_Acronym, Material_ID, ReleasedForFab, Complete, tblProjects.Project_ID FROM tblProjectMaterialsTrack INNER JOIN(SELECT tblProjectSOV.CO_ID, tblProjectSOV.SOV_Acronym, tblprojects.* FROM tblProjectSOV RIGHT JOIN(SELECT tblProjectMaterials.ProjMat_ID, tblProjectMaterials.Material_ID, tblProjectMaterials.ProjSOV_ID, tblProjectMaterials.Mat_Only, tblProjectMaterials.Mat_Phase, tblProjectMaterials.Mat_Type, tblProjects.* FROM tblProjectMaterials RIGHT JOIN(SELECT * FROM tblProjects) AS tblProjects ON tblProjectMaterials.Project_ID = tblProjects.Project_ID) AS tblProjects ON tblProjects.ProjSOV_ID = tblProjectSOV.ProjSOV_ID) AS tblProjects ON tblProjectMaterialsTrack.ProjMat_ID = tblProjects.ProjMat_ID) AS tblProjects ON tblProjects.Manuf_ID = tblManufacturers.Manuf_ID) AS tblProjects on tblProjects.Material_ID = tblMaterials.Material_ID) AS tblCOItem ON tblCOItem.CO_ID = tblProjectChangeOrders.CO_ID) AS tblProjMat ON tblProjMat.ProjMT_ID = tblProjectMaterialsShip.ProjMT_ID";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ProjectMatTracking> sb_matTrackings = new ObservableCollection<ProjectMatTracking>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                int _projMat = 0;
                DateTime _matReqdDate = new DateTime();
                string _manufName = "";
                string _orderNo = "";
                string _matPhase = "";
                string _matType = "";
                string _matDesc = "";
                DateTime _rFF = new DateTime();
                DateTime _schedShip = new DateTime();
                bool _orderComplete = false;

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("ProjMat_ID"))
                    _projMat = int.Parse(row["ProjMat_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matReqdDate = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("CO_ItemNo"))
                    _orderNo = row["CO_ItemNo"].ToString();
                if (!row.IsNull("Mat_Phase"))
                    _matPhase = row["Mat_Phase"].ToString();
                if (!row.IsNull("Mat_Type"))
                    _matType = row["Mat_Type"].ToString();
                if (!row.IsNull("Material_Desc"))
                    _matDesc = row["Material_Desc"].ToString();
                if (!row.IsNull("ReleasedForFab"))
                    _rFF = row.Field<DateTime>("ReleasedForFab");
                if (!row.IsNull("SchedShipDate"))
                    _schedShip = row.Field<DateTime>("SchedShipDate");
                if (!row.IsNull("Complete"))
                    _orderComplete = row.Field<Boolean>("Complete");
                sb_matTrackings.Add(new ProjectMatTracking
                {
                    ProjectID = _projectID,
                    ProjMat = _projMat,
                    MatReqdDate = _matReqdDate,
                    ManufName = _manufName,
                    ManuOrderNo = _orderNo,
                    MatPhase = _matPhase,
                    MatType = _matType,
                    MatDesc = _matDesc,
                    RFF = _rFF,
                    SchedShip = _schedShip,
                    OrderComplete = _orderComplete
                });
            }
            ProjectMatTrackings = sb_matTrackings;

            sqlquery = "SELECT tblSalesmen.Salesman_Name, tblProjSalesman.* FROM tblSalesmen RIGHT JOIN (SELECT tblCustomers.Full_Name, tblProjCustomer.* FROM tblCustomers RIGHT JOIN(SELECT tblProjects.Project_ID, Project_Name, Customer_ID, Job_No, Salesman_ID FROM tblProjects INNER JOIN(SELECT DISTINCT tblProjectMat.Project_ID FROM tblProjectMaterialsTrack INNER JOIN(SELECT tblProjectMaterials.ProjMat_ID, tblProjectMat.* FROM tblProjectMaterials INNER JOIN(SELECT Project_ID, Project_Name, Customer_ID, Job_No, Salesman_ID FROM tblProjects) AS tblProjectMat ON tblProjectMat.Project_ID = tblProjectMaterials.Project_ID) AS tblProjectMat ON tblProjectMat.ProjMat_ID = tblProjectMaterialsTrack.ProjMat_ID) AS tblProj ON tblProj.Project_ID = tblProjects.Project_ID) AS tblProjCustomer ON tblProjCustomer.Customer_ID = tblCustomers.Customer_ID) AS tblProjSalesman ON tblProjSalesman.Salesman_ID = tblSalesmen.Salesman_ID ORDER BY Project_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportActiveProject> sb_reportActiveProject = new ObservableCollection<ReportActiveProject>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                int _projMat = 0;
                DateTime _targetDate = new DateTime();
                string _projectName = "";
                string _customerName = "";
                string _salesmanName = "";
                string _jobNo = "";
                List<ProjectMatTracking> _projectMatTrackings = new List<ProjectMatTracking>();

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Full_Name"))
                    _customerName = row["Full_Name"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();

                _projectMatTrackings = ProjectMatTrackings.Where(item => item.ProjectID == _projectID).ToList();

                sb_reportActiveProject.Add(new ReportActiveProject
                {
                    ProjectID = _projectID,
                    ProjectName = _projectName,
                    CustomerName = _customerName,
                    SalesmanName = _salesmanName,
                    JobNo = _jobNo,
                    ProjectMatTrackings = _projectMatTrackings
                });
            }
            ReportActiveProjects = sb_reportActiveProject;
        }

        public void LoadJobByManufacturerData()
        {
            string whereProjectClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            sqlquery = "SELECT tblManufacturers.Manuf_Name, tblManuf.* FROM tblManufacturers RIGHT JOIN (SELECT tblProjectChangeOrders.CO_ItemNo, tblCOItem.* FROM tblProjectChangeOrders RIGHT JOIN(SELECT tblMaterials.Material_Desc, tblProjects.* FROM tblMaterials RIGHT JOIN(SELECT tblManufacturers.Manuf_Name, tblprojects.* FROM tblManufacturers RIGHT JOIN(SELECT tblProjectMaterialsTrack.MatReqdDate, tblProjectMaterialsTrack.ProjMat_ID, CO_ID, Manuf_ID, Mat_Phase, Mat_Type, SOV_Acronym, Material_ID, ReleasedForFab, Complete, tblProjects.Project_ID FROM tblProjectMaterialsTrack INNER JOIN(SELECT tblProjectSOV.CO_ID, tblProjectSOV.SOV_Acronym, tblprojects.* FROM tblProjectSOV RIGHT JOIN(SELECT tblProjectMaterials.ProjMat_ID, tblProjectMaterials.Material_ID, tblProjectMaterials.ProjSOV_ID, tblProjectMaterials.Mat_Only, tblProjectMaterials.Mat_Phase, tblProjectMaterials.Mat_Type, tblProjects.* FROM tblProjectMaterials RIGHT JOIN(SELECT * FROM tblProjects) AS tblProjects ON tblProjectMaterials.Project_ID = tblProjects.Project_ID) AS tblProjects ON tblProjects.ProjSOV_ID = tblProjectSOV.ProjSOV_ID) AS tblProjects ON tblProjectMaterialsTrack.ProjMat_ID = tblProjects.ProjMat_ID) AS tblProjects ON tblProjects.Manuf_ID = tblManufacturers.Manuf_ID " + whereClause1 + ") AS tblProjects on tblProjects.Material_ID = tblMaterials.Material_ID) AS tblCOItem ON tblCOItem.CO_ID = tblProjectChangeOrders.CO_ID) AS tblManuf ON tblManuf.Manuf_ID = tblManufacturers.Manuf_ID";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            ObservableCollection<ProjectMatTracking> sb_matTrackings = new ObservableCollection<ProjectMatTracking>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                int _projMat = 0;
                DateTime _matReqdDate = new DateTime();
                string _manufName = "";
                string _matPhase = "";
                string _matType = "";
                string _matDesc = "";
                string _orderNo = "";
                DateTime _rFF = new DateTime();
                bool _orderComplete = false;

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("ProjMat_ID"))
                    _projMat = int.Parse(row["ProjMat_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matReqdDate = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("CO_ItemNo"))
                    _orderNo = row["CO_ItemNo"].ToString();
                if (!row.IsNull("Mat_Phase"))
                    _matPhase = row["Mat_Phase"].ToString();
                if (!row.IsNull("Mat_Type"))
                    _matType = row["Mat_Type"].ToString();
                if (!row.IsNull("Material_Desc"))
                    _matDesc = row["Material_Desc"].ToString();
                if (!row.IsNull("ReleasedForFab"))
                    _rFF = row.Field<DateTime>("ReleasedForFab");
                if (!row.IsNull("Complete"))
                    _orderComplete = row.Field<Boolean>("Complete");
                sb_matTrackings.Add(new ProjectMatTracking
                {
                    ProjectID = _projectID,
                    ProjMat = _projMat,
                    MatReqdDate = _matReqdDate,
                    ManufName = _manufName,
                    ManuOrderNo = _orderNo,
                    MatPhase = _matPhase,
                    MatType = _matType,
                    MatDesc = _matDesc,
                    RFF = _rFF,
                    OrderComplete = _orderComplete
                });
            }
            ProjectMatTrackings = sb_matTrackings;

            sqlquery = "SELECT tblArchRep.Arch_Rep_Name, tblProjArchRep.* FROM tblArchRep RIGHT JOIN (SELECT tblArchitects.Arch_Company, tblProjArch.* FROM tblArchitects RIGHT JOIN(SELECT tblSalesmen.Salesman_Name, tblProjSalesman.* FROM tblSalesmen RIGHT JOIN(SELECT tblCustomers.Full_Name, tblProjCustomer.* FROM tblCustomers RIGHT JOIN(SELECT tblProjects.Project_ID, Project_Name, Target_Date, Arch_Rep_ID, Architect_ID, Customer_ID, Job_No, Salesman_ID, tblProjects.Address, tblProjects.State, tblProjects.ZIP FROM tblProjects INNER JOIN(SELECT DISTINCT tblProjectMat.Project_ID FROM tblProjectMaterialsTrack INNER JOIN(SELECT tblProjectMaterials.ProjMat_ID, tblProjectMat.* FROM tblProjectMaterials INNER JOIN(SELECT Project_ID, Project_Name, Customer_ID, Job_No, Salesman_ID FROM tblProjects " + whereProjectClause + ") AS tblProjectMat ON tblProjectMat.Project_ID = tblProjectMaterials.Project_ID) AS tblProjectMat ON tblProjectMat.ProjMat_ID = tblProjectMaterialsTrack.ProjMat_ID) AS tblProj ON tblProj.Project_ID = tblProjects.Project_ID) AS tblProjCustomer ON tblProjCustomer.Customer_ID = tblCustomers.Customer_ID) AS tblProjSalesman ON tblProjSalesman.Salesman_ID = tblSalesmen.Salesman_ID) AS tblProjArch ON tblProjArch.Architect_ID = tblArchitects.Architect_ID) AS tblProjArchRep ON tblProjArchRep.Arch_Rep_ID = tblArchRep.Arch_Rep_ID ORDER BY Project_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportJobArchRep> sb_reportJobArch = new ObservableCollection<ReportJobArchRep>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                int _projMat = 0;
                DateTime _targetDate = new DateTime();
                string _architect = "";
                string _archRep = "";
                string _projectName = "";
                string _customerName = "";
                string _salesmanName = "";
                string _address = "";
                string _state = "";
                string _zip = "";
                string _jobNo = "";
                List<ProjectMatTracking> _projectMatTrackings = new List<ProjectMatTracking>();

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                //if (!row.IsNull("ProjMat_ID"))
                //    _projMat = int.Parse(row["ProjMat_ID"].ToString());
                if (!row.IsNull("Target_Date"))
                    _targetDate = row.Field<DateTime>("Target_Date");
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Arch_Company"))
                    _architect = row["Arch_Company"].ToString();
                if (!row.IsNull("Arch_Rep_Name"))
                    _archRep = row["Arch_Rep_Name"].ToString();
                if (!row.IsNull("Full_Name"))
                    _customerName = row["Full_Name"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Address"))
                    _address = row["Address"].ToString();
                if (!row.IsNull("State"))
                    _state = row["State"].ToString();
                if (!row.IsNull("Zip"))
                    _zip = row["Zip"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();

                _projectMatTrackings = ProjectMatTrackings.Where(item => item.ProjectID == _projectID).ToList();

                sb_reportJobArch.Add(new ReportJobArchRep
                {
                    ProjectID = _projectID,
                    //ProjectMatID = _projMat,
                    ProjectName = _projectName,
                    TargetDate = _targetDate,
                    Architect = _architect,
                    ArchRep = _archRep,
                    CustomerName = _customerName,
                    SalesmanName = _salesmanName,
                    Address = _address,
                    State = _state,
                    Zip = _zip,
                    JobNo = _jobNo,
                    ProjectMatTrackings = _projectMatTrackings
                });
            }
            ReportJobArchReps = sb_reportJobArch;
        }

        public void LoadOpenJobsData()
        {
            string whereProjectClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            sqlquery = "SELECT SchedShipDate, tblProjMat.* FROM tblProjectMaterialsShip RIGHT JOIN (SELECT tblProjectChangeOrders.CO_ItemNo, tblCOItem.* FROM tblProjectChangeOrders RIGHT JOIN(SELECT tblMaterials.Material_Desc, tblProjects.* FROM tblMaterials RIGHT JOIN(SELECT tblManufacturers.Manuf_Name, tblprojects.* FROM tblManufacturers RIGHT JOIN(SELECT tblProjectMaterialsTrack.MatReqdDate, tblProjectMaterialsTrack.ProjMat_ID, tblProjectMaterialsTrack.ProjMT_ID, CO_ID, Manuf_ID, Mat_Phase, Mat_Type, SOV_Acronym, Material_ID, ReleasedForFab, Complete, tblProjects.Project_ID FROM tblProjectMaterialsTrack INNER JOIN(SELECT tblProjectSOV.CO_ID, tblProjectSOV.SOV_Acronym, tblprojects.* FROM tblProjectSOV RIGHT JOIN(SELECT tblProjectMaterials.ProjMat_ID, tblProjectMaterials.Material_ID, tblProjectMaterials.ProjSOV_ID, tblProjectMaterials.Mat_Only, tblProjectMaterials.Mat_Phase, tblProjectMaterials.Mat_Type, tblProjects.* FROM tblProjectMaterials RIGHT JOIN(SELECT * FROM tblProjects) AS tblProjects ON tblProjectMaterials.Project_ID = tblProjects.Project_ID) AS tblProjects ON tblProjects.ProjSOV_ID = tblProjectSOV.ProjSOV_ID) AS tblProjects ON tblProjectMaterialsTrack.ProjMat_ID = tblProjects.ProjMat_ID) AS tblProjects ON tblProjects.Manuf_ID = tblManufacturers.Manuf_ID) AS tblProjects on tblProjects.Material_ID = tblMaterials.Material_ID) AS tblCOItem ON tblCOItem.CO_ID = tblProjectChangeOrders.CO_ID) AS tblProjMat ON tblProjMat.ProjMT_ID = tblProjectMaterialsShip.ProjMT_ID";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ProjectMatTracking> sb_matTrackings = new ObservableCollection<ProjectMatTracking>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                int _projMat = 0;
                DateTime _matReqdDate = new DateTime();
                string _manufName = "";
                string _orderNo = "";
                string _matPhase = "";
                string _matType = "";
                string _matDesc = "";
                DateTime _rFF = new DateTime();
                DateTime _schedShip = new DateTime();
                bool _orderComplete = false;

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("ProjMat_ID"))
                    _projMat = int.Parse(row["ProjMat_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matReqdDate = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("CO_ItemNo"))
                    _orderNo = row["CO_ItemNo"].ToString();
                if (!row.IsNull("Mat_Phase"))
                    _matPhase = row["Mat_Phase"].ToString();
                if (!row.IsNull("Mat_Type"))
                    _matType = row["Mat_Type"].ToString();
                if (!row.IsNull("Material_Desc"))
                    _matDesc = row["Material_Desc"].ToString();
                if (!row.IsNull("ReleasedForFab"))
                    _rFF = row.Field<DateTime>("ReleasedForFab");
                if (!row.IsNull("SchedShipDate"))
                    _schedShip = row.Field<DateTime>("SchedShipDate");
                if (!row.IsNull("Complete"))
                    _orderComplete = row.Field<Boolean>("Complete");
                sb_matTrackings.Add(new ProjectMatTracking
                {
                    ProjectID = _projectID,
                    ProjMat = _projMat,
                    MatReqdDate = _matReqdDate,
                    ManufName = _manufName,
                    ManuOrderNo = _orderNo,
                    MatPhase = _matPhase,
                    MatType = _matType,
                    MatDesc = _matDesc,
                    RFF = _rFF,
                    SchedShip = _schedShip,
                    OrderComplete = _orderComplete
                });
            }
            ProjectMatTrackings = sb_matTrackings;

            sqlquery = "SELECT tblSalesmen.Salesman_Name, tblProjSalesman.* FROM tblSalesmen RIGHT JOIN (SELECT tblCustomers.Full_Name, tblProjCustomer.* FROM tblCustomers RIGHT JOIN(SELECT tblProjects.Project_ID, Project_Name, Customer_ID, Job_No, Salesman_ID FROM tblProjects INNER JOIN(SELECT DISTINCT tblProjectMat.Project_ID FROM tblProjectMaterialsTrack INNER JOIN(SELECT tblProjectMaterials.ProjMat_ID, tblProjectMat.* FROM tblProjectMaterials INNER JOIN(SELECT Project_ID, Project_Name, Customer_ID, Job_No, Salesman_ID FROM tblProjects) AS tblProjectMat ON tblProjectMat.Project_ID = tblProjectMaterials.Project_ID) AS tblProjectMat ON tblProjectMat.ProjMat_ID = tblProjectMaterialsTrack.ProjMat_ID) AS tblProj ON tblProj.Project_ID = tblProjects.Project_ID) AS tblProjCustomer ON tblProjCustomer.Customer_ID = tblCustomers.Customer_ID) AS tblProjSalesman ON tblProjSalesman.Salesman_ID = tblSalesmen.Salesman_ID ORDER BY Project_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportActiveProject> sb_reportActiveProject = new ObservableCollection<ReportActiveProject>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                int _projMat = 0;
                DateTime _targetDate = new DateTime();
                string _projectName = "";
                string _customerName = "";
                string _salesmanName = "";
                string _jobNo = "";
                List<ProjectMatTracking> _projectMatTrackings = new List<ProjectMatTracking>();

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Full_Name"))
                    _customerName = row["Full_Name"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();

                _projectMatTrackings = ProjectMatTrackings.Where(item => item.ProjectID == _projectID).ToList();

                sb_reportActiveProject.Add(new ReportActiveProject
                {
                    ProjectID = _projectID,
                    ProjectName = _projectName,
                    CustomerName = _customerName,
                    SalesmanName = _salesmanName,
                    JobNo = _jobNo,
                    ProjectMatTrackings = _projectMatTrackings
                });
            }
            ReportActiveProjects = sb_reportActiveProject;
        }

        public void LoadPmMeetingData()
        {
            string whereProjectClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            sqlquery = "SELECT tblManufacturers.Manuf_Name, tblManuf.* FROM tblManufacturers RIGHT JOIN (SELECT tblProjectChangeOrders.CO_ItemNo, tblProjCO.* FROM tblProjectChangeOrders RIGHT JOIN(SELECT tblProjectMaterialsShip.SchedShipDate, tblProjectMaterialsShip.EstDelivDate, tblProjectMaterialsShip.Date_Recvd, tblProjShip.* FROM tblProjectMaterialsShip RIGHT JOIN(SELECT tblProjectSOV.CO_ID, tblProjSOV.* FROM tblProjectSOV RIGHT JOIN(Select tblProjectMaterialsTrack.ProjMT_ID, tblProjectMaterialsTrack.MatReqdDate, tblProjectMaterialsTrack.Manuf_ID, tblProjectMaterialsTrack.PO_Number, tblProjectMaterialsTrack.Manuf_LeadTime, tblProjectMaterialsTrack.ShopReqDate, tblProjectMaterialsTrack.ShopRecvdDate, tblProjectMaterialsTrack.SubmitIssue, tblProjectMaterialsTrack.SubmitAppr, tblProjectMaterialsTrack.No_Sub_Needed, tblProjectMaterialsTrack.FM_Needed, tblProjectMaterialsTrack.Field_Dim, tblProjectMaterialsTrack.ReleasedForFab, tblProjectMaterialsTrack.MatComplete, tblProjectMaterialsTrack.LaborComplete, tblMat.Mat_Only, tblMat.Color_Selected, tblMat.ProjSOV_ID, tblMat.Mat_Phase, tblMat.Mat_Type, tblMat.Project_ID from tblProjectMaterialsTrack INNER JOIN(SELECT * FROM tblProjectMaterials) AS tblMat ON tblMat.ProjMat_ID = tblProjectMaterialsTrack.ProjMat_ID) AS tblProjSOV ON tblProjectSOV.ProjSOV_ID = tblProjSOV.ProjSOV_ID) AS tblProjShip ON tblProjShip.ProjMT_ID = tblProjectMaterialsShip.ProjMT_ID) AS tblProjCO ON tblProjCO.CO_ID = tblProjectChangeOrders.CO_ID) AS tblManuf ON tblManuf.Manuf_ID = tblManufacturers.Manuf_ID" + whereClause1;
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ProjectMatTracking> sb_matTrackings = new ObservableCollection<ProjectMatTracking>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                DateTime _matReqdDate = new DateTime();
                bool _matOnly = false;
                string _manufName = "";
                string _orderNo = "";
                string _matPhase = "";
                string _matType = "";
                string _poNumber = "";
                string _leadTime = "";
                DateTime _shopReq = new DateTime();
                DateTime _shopRecv = new DateTime();
                DateTime _submIssue = new DateTime();
                DateTime _submAppr = new DateTime();
                bool _noSubm = false;
                string _color = "";
                bool _needFM = false;
                DateTime _fieldDim = new DateTime();
                DateTime _rFF = new DateTime();
                DateTime _schedShip = new DateTime();
                DateTime _estDeliv = new DateTime();
                DateTime _dateRecv = new DateTime();
                bool _matComplete = false;
                bool _orderComplete = false;

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matReqdDate = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Mat_Only"))
                    _matOnly = row.Field<Boolean>("Mat_Only");
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("CO_ItemNo"))
                    _orderNo = row["CO_ItemNo"].ToString();
                if (!row.IsNull("Mat_Phase"))
                    _matPhase = row["Mat_Phase"].ToString();
                if (!row.IsNull("Mat_Type"))
                    _matType = row["Mat_Type"].ToString();
                if (!row.IsNull("PO_Number"))
                    _poNumber = row["PO_Number"].ToString();
                if (!row.IsNull("Manuf_LeadTime"))
                    _leadTime = row["Manuf_LeadTime"].ToString();
                if (!row.IsNull("ShopReqDate"))
                    _shopReq = row.Field<DateTime>("ShopReqDate");
                if (!row.IsNull("ShopRecvdDate"))
                    _shopRecv = row.Field<DateTime>("ShopRecvdDate");
                if (!row.IsNull("SubmitIssue"))
                    _submIssue = row.Field<DateTime>("SubmitIssue");
                if (!row.IsNull("SubmitAppr"))
                    _submAppr = row.Field<DateTime>("SubmitAppr");
                if (!row.IsNull("No_Sub_Needed"))
                    _noSubm = row.Field<Boolean>("No_Sub_Needed");
                if (!row.IsNull("Color_Selected"))
                    _color = row["Color_Selected"].ToString();
                if (!row.IsNull("FM_Needed"))
                    _needFM = row.Field<Boolean>("FM_Needed");
                if (!row.IsNull("Field_Dim"))
                    _fieldDim = row.Field<DateTime>("Field_Dim");
                if (!row.IsNull("ReleasedForFab"))
                    _rFF = row.Field<DateTime>("ReleasedForFab");
                if (!row.IsNull("SchedShipDate"))
                    _schedShip = row.Field<DateTime>("SchedShipDate");
                if (!row.IsNull("SchedShipDate"))
                    _schedShip = row.Field<DateTime>("SchedShipDate");
                if (!row.IsNull("EstDelivDate"))
                    _estDeliv = row.Field<DateTime>("EstDelivDate");
                if (!row.IsNull("Date_Recvd"))
                    _dateRecv = row.Field<DateTime>("Date_Recvd");
                if (!row.IsNull("MatComplete"))
                    _matComplete = row.Field<Boolean>("MatComplete");
                if (!row.IsNull("LaborComplete"))
                    _orderComplete = row.Field<Boolean>("LaborComplete");
                sb_matTrackings.Add(new ProjectMatTracking
                {
                    ProjectID = _projectID,
                    MatReqdDate = _matReqdDate,
                    MatOnly = _matOnly,
                    ManufName = _manufName,
                    ManuOrderNo = _orderNo,
                    MatPhase = _matPhase,
                    MatType = _matType,
                    PoNumber = _poNumber,
                    LeadTime = _leadTime,
                    ShopReqDate = _shopReq,
                    ShopRecvdDate = _shopRecv,
                    SubmIssue = _submIssue,
                    SubmAppr = _submAppr,
                    NoSubm = _noSubm,
                    Color = _color,
                    NeedFM = _needFM,
                    FieldDim = _fieldDim,
                    RFF = _rFF,
                    SchedShip = _schedShip,
                    EstDeliv = _estDeliv,
                    DateRecv = _dateRecv,
                    OrderComplete = _matComplete,
                    LaborComplete = _orderComplete
                });
            }
            ProjectMatTrackings = sb_matTrackings;

            sqlquery = "SELECT tblCustomers.Full_Name, tblProjCustomer.* FROM tblCustomers RIGHT JOIN (SELECT Estimator_Name, tblProjEst.*FROM tblEstimators RIGHT JOIN(SELECT tblArchitects.Arch_Company, tblARch.* FROM tblArchitects RIGHT JOIN(SELECT tblProjectManagers.PM_Name, tblProjPM.* FROM tblProjectManagers RIGHT JOIN(SELECT PM_ID, tblProjPM.* FROM tblProjectPMs RIGHT JOIN(SELECT tblProjects.Project_ID, Project_Name, Target_Date, Complete, Architect_ID, Address, State, ZIP, Job_No, Estimator_ID, Customer_ID, Billing_Date, CIP_Project, C3, Contract_Rcvd, CertPay_Reqd, Stored_Materials FROM tblProjects INNER JOIN (SELECT DISTINCT tblProjectMat.Project_ID FROM tblProjectMaterialsTrack INNER JOIN(SELECT tblProjectMaterials.ProjMat_ID, tblProjectMat.* FROM tblProjectMaterials INNER JOIN(SELECT Project_ID, Project_Name, Customer_ID, Job_No, Salesman_ID FROM tblProjects) AS tblProjectMat ON tblProjectMat.Project_ID = tblProjectMaterials.Project_ID) AS tblProjectMat ON tblProjectMat.ProjMat_ID = tblProjectMaterialsTrack.ProjMat_ID) AS tblProj ON tblProj.Project_ID = tblProjects.Project_ID "+ whereProjectClause + ") AS tblProjPM ON tblProjPM.Project_ID = tblProjectPMs.Project_ID) AS tblProjPM ON tblProjPM.PM_ID = tblProjectManagers.PM_ID) AS tblARch ON tblARch.Architect_ID = tblArchitects.Architect_ID) AS tblProjEst ON tblProjEst.Estimator_ID = tblEstimators.Estimator_ID) AS tblProjCustomer ON tblProjCustomer.Customer_ID = tblCustomers.Customer_ID";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportPmMeeting> sb_reportPmMeetings = new ObservableCollection<ReportPmMeeting>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                string _projectName = "";
                DateTime _targetDate = new DateTime();
                bool _complete = false;
                string _architect = "";
                string _address = "";
                string _state = "";
                string _zip = "";
                string _jobNo = "";
                string _pmName = "";
                string _estimatorName = "";
                string _customerName = "";
                int _bilingDue = 0;
                bool _isCip = false;
                bool _isC3 = false;
                bool _isCertPayroll = false;
                bool _isContractRecvd = false;
                bool _storedMat = false;
                List<ProjectMatTracking> _projectMatTrackings = new List<ProjectMatTracking>();

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Target_Date"))
                    _targetDate = row.Field<DateTime>("Target_Date");
                if (!row.IsNull("Complete"))
                    _complete = row.Field<Boolean>("Complete");
                if (!row.IsNull("Arch_Company"))
                    _architect = row["Arch_Company"].ToString();
                if (!row.IsNull("Address"))
                    _address = row["Address"].ToString();
                if (!row.IsNull("State"))
                    _state = row["State"].ToString();
                if (!row.IsNull("Zip"))
                    _zip = row["Zip"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("PM_Name"))
                    _pmName = row["PM_Name"].ToString();
                if (!row.IsNull("Estimator_Name"))
                    _estimatorName = row["Estimator_Name"].ToString();
                if (!row.IsNull("Full_Name"))
                    _customerName = row["Full_Name"].ToString();
                if (!row.IsNull("Billing_Date"))
                    _bilingDue = int.Parse(row["Billing_Date"].ToString());
                if (!row.IsNull("CIP_Project"))
                    _isCip = row.Field<Boolean>("CIP_Project");
                if (!row.IsNull("C3"))
                    _isC3 = row.Field<Boolean>("C3");
                if (!row.IsNull("CertPay_Reqd"))
                    _isCertPayroll = row.Field<Boolean>("CertPay_Reqd");
                if (!row.IsNull("Contract_Rcvd"))
                    _isContractRecvd = row.Field<Boolean>("Contract_Rcvd");
                if (!row.IsNull("Stored_Materials"))
                    _storedMat = row.Field<Boolean>("Stored_Materials");

                _projectMatTrackings = ProjectMatTrackings.Where(item => item.ProjectID == _projectID).ToList();

                sb_reportPmMeetings.Add(new ReportPmMeeting
                {
                    ProjectID = _projectID,
                    ProjectName = _projectName,
                    TargetDate = _targetDate,
                    Complete = _complete,
                    ArchitectName = _architect,
                    Address = _address,
                    State = _state,
                    Zip = _zip,
                    JobNo = _jobNo,
                    PmName = _pmName,
                    EstimatorName = _estimatorName,
                    CustomerName = _customerName,
                    BilingDue = _bilingDue,
                    IsCip = _isCip,
                    IsC3 = _isC3,
                    IsCertPayroll = _isCertPayroll,
                    IsContractRecvd = _isContractRecvd,
                    StoredMat = _storedMat,
                    ProjectMatTrackings = _projectMatTrackings
                });
            }
            ReportPmMeetings = sb_reportPmMeetings;
        }
        
        public void LoadShipNotRecvData()
        {
            string whereProjectClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }
            
            sqlquery = "SELECT Manuf_Name, tblProjSales.* FROM tblManufacturers RIGHT JOIN (SELECT Salesman_Name, tblProjSales.*FROM tblSalesmen RIGHT JOIN(SELECT Job_No, Project_Name, Salesman_ID, tblProjMat.* FROM tblProjects RIGHT JOIN(SELECT Project_ID, Material_ID, Mat_Type, Mat_Phase, tblProjTrack.* FROM tblProjectMaterials RIGHT JOIN(SELECT MatReqdDate, ProjMat_ID, Manuf_ID, PO_Number, tblProjShip.* FROM tblProjectMaterialsTrack INNER JOIN(SELECT SchedShipDate, ProjMT_ID, EstDelivDate FROM tblProjectMaterialsShip WHERE Date_Recvd IS NULL) AS tblProjShip ON tblProjectMaterialsTrack.ProjMT_ID = tblProjShip.ProjMT_ID) AS tblProjTrack ON tblProjTrack.ProjMat_ID = tblProjectMaterials.ProjMat_ID) AS tblProjMat ON tblProjMat.Project_ID = tblProjects.Project_ID "+ whereProjectClause +") AS tblProjSales ON tblSalesmen.Salesman_ID = tblProjSales.Salesman_ID) AS tblProjSales ON tblProjSales.Salesman_ID = tblManufacturers.Manuf_ID ORDER BY Job_No";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportShipNotRecv> sb_reportShipNotRecvs = new ObservableCollection<ReportShipNotRecv>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                DateTime _matlReqd = new DateTime();
                string _jobNo = "";
                string _projectName = "";
                string _salesmanName = "";
                string _matType = "";
                string _phase = "";
                string _manufName = "";
                string _poNumber = "";
                DateTime _schedShip = new DateTime();
                DateTime _estDeliv = new DateTime();

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matlReqd = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Mat_Type"))
                    _matType = row["Mat_Type"].ToString();
                if (!row.IsNull("Mat_Phase"))
                    _phase = row["Mat_Phase"].ToString();
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("PO_Number"))
                    _poNumber = row["PO_Number"].ToString();
                if (!row.IsNull("SchedShipDate"))
                    _schedShip = row.Field<DateTime>("SchedShipDate");
                if (!row.IsNull("EstDelivDate"))
                    _estDeliv = row.Field<DateTime>("EstDelivDate");

                sb_reportShipNotRecvs.Add(new ReportShipNotRecv
                {
                    ProjectID = _projectID,
                    ProjectName = _projectName,
                    MatlReqd = _matlReqd,
                    JobNo = _jobNo,
                    SalesmanName = _salesmanName,
                    MatType = _matType,
                    Phase = _phase,
                    ManufName = _manufName,
                    PoNumber = _poNumber,
                    SchedShip = _schedShip,
                    EstDeliv = _estDeliv
                });
            }
            ReportShipNotRecvs = sb_reportShipNotRecvs;
        }

        public void LoadShopRecvData()
        {
            string whereProjectClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            sqlquery = "SELECT Manuf_Name, tblProjManuf.* FROM tblManufacturers RIGHT JOIN (SELECT Salesman_Name, tblProjSales.*FROM tblSalesmen RIGHT JOIN(SELECT tblMaterials.Material_Desc, tblProjMat.* FROM tblMaterials RIGHT JOIN(SELECT Job_No, Project_Name, Salesman_ID, tblProj.* FROM tblProjects RIGHT JOIN(SELECT MatReqdDate, ShopRecvdDate, SubmitIssue, Manuf_ID, tblProjectMaterials.Material_ID, tblProjectMaterials.Project_ID FROM tblProjectMaterials RIGHT JOIN(SELECT * FROM tblProjectMaterialsTrack WHERE ShopRecvdDate IS NOT NULL AND SubmitIssue is NULL AND No_Sub_Needed = 0) AS tblProjMat ON tblProjMat.ProjMat_ID = tblProjectMaterials.ProjMat_ID) AS tblProj ON tblProj.Project_ID = tblProjects.Project_ID "+ whereProjectClause +") AS tblProjMat ON tblProjMat.Material_ID = tblMaterials.Material_ID) AS tblProjSales ON tblProjSales.Salesman_ID = tblSalesmen.Salesman_ID) AS tblProjManuf ON tblProjManuf.Manuf_ID = tblManufacturers.Manuf_ID "+ whereClause1 +" ORDER BY MatReqdDate";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportShopRecv> sb_reportShopRecv = new ObservableCollection<ReportShopRecv>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                DateTime _matlReqd = new DateTime();
                string _jobNo = "";
                string _projectName = "";
                string _salesmanName = "";
                string _materialName = "";
                string _manufName = "";
                DateTime _shopRecvd = new DateTime();
                DateTime _submIssue = new DateTime();

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matlReqd = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Material_Desc"))
                    _materialName = row["Material_Desc"].ToString();
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("ShopRecvdDate"))
                    _shopRecvd = row.Field<DateTime>("ShopRecvdDate");
                if (!row.IsNull("SubmitIssue"))
                    _submIssue = row.Field<DateTime>("SubmitIssue");

                sb_reportShopRecv.Add(new ReportShopRecv
                {
                    ProjectID = _projectID,
                    ProjectName = _projectName,
                    MatlReqd = _matlReqd,
                    JobNo = _jobNo,
                    SalesmanName = _salesmanName,
                    MaterialName = _materialName,
                    ManufName = _manufName,
                    ShopRecvd = _shopRecvd,
                    SubmIssue = _submIssue
                });
            }
            ReportShopRecvs = sb_reportShopRecv;
        }
        
        public void LoadShopReqData()
        {
            string whereProjectClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            sqlquery = "SELECT Material_Desc, tblProjMat.* FROM tblMaterials RIGHT JOIN (SELECT Manuf_Name, tblProManuf.*FROM tblManufacturers RIGHT JOIN(SELECT Salesman_Name, tblProj.* FROM tblSalesmen RIGHT JOIN(SELECT Job_No, Project_Name, Salesman_ID, tblProj.* FROM tblProjects RIGHT JOIN(SELECT MatReqdDate, ShopRecvdDate, ShopReqDate, SubmitIssue, Manuf_ID, tblProjectMaterials.Material_ID, tblProjectMaterials.Project_ID FROM tblProjectMaterials RIGHT JOIN(SELECT * FROM tblProjectMaterialsTrack WHERE ShopReqDate IS NOT NULL AND No_Sub_Needed = 0 AND SubmitAppr IS NULL AND SubmitIssue IS NULL) AS tblProjMat ON tblProjMat.ProjMat_ID = tblProjectMaterials.ProjMat_ID) AS tblProj ON tblProj.Project_ID = tblProjects.Project_ID) AS tblProj ON tblProj.Salesman_ID = tblSalesmen.Salesman_ID) AS tblProManuf ON tblProManuf.Manuf_ID = tblManufacturers.Manuf_ID "+ whereClause1 +") AS tblProjMat ON tblProjMat.Material_ID = tblMaterials.Material_ID ORDER BY MatReqdDate";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportShopReq> sb_reportShopReq = new ObservableCollection<ReportShopReq>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                DateTime _matlReqd = new DateTime();
                string _jobNo = "";
                string _projectName = "";
                string _salesmanName = "";
                string _materialName = "";
                string _manufName = "";
                DateTime _shopRecvd = new DateTime();
                DateTime _shopReq = new DateTime();

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matlReqd = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Material_Desc"))
                    _materialName = row["Material_Desc"].ToString();
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("ShopRecvdDate"))
                    _shopRecvd = row.Field<DateTime>("ShopRecvdDate");
                if (!row.IsNull("ShopReqDate"))
                    _shopReq = row.Field<DateTime>("ShopReqDate");

                sb_reportShopReq.Add(new ReportShopReq
                {
                    ProjectID = _projectID,
                    ProjectName = _projectName,
                    MatlReqd = _matlReqd,
                    JobNo = _jobNo,
                    SalesmanName = _salesmanName,
                    MaterialName = _materialName,
                    ManufName = _manufName,
                    ShopRecvd = _shopRecvd,
                    ShopReq = _shopReq
                });
            }
            ReportShopReqs = sb_reportShopReq;
        }
        
        public void LoadSubmitNotApprData()
        {
            string whereProjectClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            sqlquery = "SELECT Material_Desc, tblProjMat.* FROM tblMaterials RIGHT JOIN (SELECT Manuf_Name, tblProManuf.*FROM tblManufacturers RIGHT JOIN(SELECT Salesman_Name, tblProj.* FROM tblSalesmen RIGHT JOIN(SELECT Job_No, Project_Name, Salesman_ID, tblProj.* FROM tblProjects RIGHT JOIN(SELECT MatReqdDate, ShopRecvdDate, ShopReqDate, SubmitIssue, Resubmit_Date, SubmitAppr, Manuf_ID, tblProjectMaterials.Material_ID, tblProjectMaterials.Project_ID FROM tblProjectMaterials RIGHT JOIN(SELECT * FROM tblProjectMaterialsTrack WHERE ShopReqDate IS NOT NULL AND No_Sub_Needed = 0 AND SubmitAppr IS NULL AND SubmitIssue IS NOT NULL) AS tblProjMat ON tblProjMat.ProjMat_ID = tblProjectMaterials.ProjMat_ID) AS tblProj ON tblProj.Project_ID = tblProjects.Project_ID) AS tblProj ON tblProj.Salesman_ID = tblSalesmen.Salesman_ID) AS tblProManuf ON tblProManuf.Manuf_ID = tblManufacturers.Manuf_ID "+ whereClause1 +") AS tblProjMat ON tblProjMat.Material_ID = tblMaterials.Material_ID";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportSubmit> sb_reportSubmits = new ObservableCollection<ReportSubmit>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                DateTime _matlReqd = new DateTime();
                string _jobNo = "";
                string _projectName = "";
                string _salesmanName = "";
                string _materialName = "";
                string _manufName = "";
                DateTime _submIssue = new DateTime();
                DateTime _reSubmit = new DateTime();
                DateTime _submAppr = new DateTime();

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matlReqd = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Material_Desc"))
                    _materialName = row["Material_Desc"].ToString();
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("SubmitIssue"))
                    _submIssue = row.Field<DateTime>("SubmitIssue");
                if (!row.IsNull("Resubmit_Date"))
                    _reSubmit = row.Field<DateTime>("Resubmit_Date");
                if (!row.IsNull("SubmitAppr"))
                    _submAppr = row.Field<DateTime>("SubmitAppr");

                sb_reportSubmits.Add(new ReportSubmit
                {
                    ProjectID = _projectID,
                    ProjectName = _projectName,
                    MatlReqd = _matlReqd,
                    JobNo = _jobNo,
                    SalesmanName = _salesmanName,
                    MaterialName = _materialName,
                    ManufName = _manufName,
                    SubmIssue = _submIssue,
                    ReSubmit = _reSubmit,
                    SubmAppr = _submAppr
                });
            }
            ReportSubmits = sb_reportSubmits;
        }

        public void LoadVendorData()
        {
            string whereProjectClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            sqlquery = "SELECT tblManufacturers.Manuf_ID, tblManufacturers.Manuf_Name, tblManufacturers.Contact_Name, tblManufacturers.Contact_Phone, tblManufacturers.Contact_Email FROM tblManufacturers" + whereClause1 + " ORDER BY Manuf_Name";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportVendor> sb_reportVendors = new ObservableCollection<ReportVendor>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _manufID = 0;
                string _manufName = "";
                string _contactName = "";
                string _contactPhone = "";
                string _contactEmail = "";

                if (!row.IsNull("Manuf_ID"))
                    _manufID = int.Parse(row["Manuf_ID"].ToString());
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("Contact_Name"))
                    _contactName = row["Contact_Name"].ToString();
                if (!row.IsNull("Contact_Phone"))
                    _contactPhone = row["Contact_Phone"].ToString();
                if (!row.IsNull("Contact_Email"))
                    _contactEmail = row["Contact_Email"].ToString();


                sb_reportVendors.Add(new ReportVendor
                {
                    ManufID = _manufID,
                    ManufName = _manufName,
                    ContactName = _contactName,
                    ContactPhone = _contactPhone,
                    ContactEmail = _contactEmail
                });
            }
            ReportVendors = sb_reportVendors;
        }

        public void LoadReleaseNotShipData()
        {
            string whereProjectClause = GetProjectWhereClasuse();
            string whereClause1 = " WHERE 1=1";
            string whereClause2 = " WHERE 1=1";

            if (SelectedManufID != 0)
            {
                whereClause1 += $" AND tblManufacturers.Manuf_ID = '{SelectedManufID}'";
            }

            if (SelectedMatID != 0)
            {
                whereClause2 += $" AND tblMaterials.Material_ID = '{SelectedMatID}'";
            }

            sqlquery = "SELECT tblMaterials.Material_Desc, tblProjMat.* FROM tblMaterials RIGHT JOIN (SELECT Salesman_Name, tblProjSales.*FROM tblSalesmen RIGHT JOIN(SELECT Project_Name, Salesman_ID, Job_No, tblProj.* FROM tblProjects RIGHT JOIN(SELECT Manuf_Name, tblProjManuf.* FROM tblManufacturers RIGHT JOIN(SELECT Material_ID, Mat_Phase, Project_ID, tblProjMat.* FROM tblProjectMaterials RIGHT JOIN(SELECT tblProjectMaterialsTrack.ProjMat_ID, tblProjectMaterialsTrack.Manuf_ID, MatReqdDate, ReleasedForFab, PO_Number FROM tblProjectMaterialsTrack LEFT JOIN tblProjectMaterialsShip ON tblProjectMaterialsTrack.ProjMT_ID = tblProjectMaterialsShip.ProjMT_ID WHERE tblProjectMaterialsShip.ProjMT_ID IS NULL AND tblProjectMaterialsTrack.ReleasedForFab IS NOT NULL) AS tblProjMat ON tblProjMat.ProjMat_ID = tblProjectMaterials.ProjMat_ID "+ whereClause2 +") AS tblProjManuf ON tblProjManuf.Manuf_ID = tblManufacturers.Manuf_ID "+ whereClause1  +") AS tblProj ON tblProj.Project_ID = tblProjects.Project_ID "+ whereProjectClause + ") AS tblProjSales ON tblProjSales.Salesman_ID = tblSalesmen.Salesman_ID) AS tblProjMat ON tblProjMat.Material_ID = tblMaterials.Material_ID ORDER BY MatReqdDate";
            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ReportReleaseNotShip> sb_reportReleases = new ObservableCollection<ReportReleaseNotShip>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                DateTime _matlReqd = new DateTime();
                string _jobNo = "";
                string _projectName = "";
                string _salesmanName = "";
                string _materialName = "";
                string _phase = "";
                string _manufName = "";
                string _poNumber = ""; 
                DateTime _rFF = new DateTime();

                if (!row.IsNull("Project_ID"))
                    _projectID = int.Parse(row["Project_ID"].ToString());
                if (!row.IsNull("MatReqdDate"))
                    _matlReqd = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Project_Name"))
                    _projectName = row["Project_Name"].ToString();
                if (!row.IsNull("Job_No"))
                    _jobNo = row["Job_No"].ToString();
                if (!row.IsNull("Salesman_Name"))
                    _salesmanName = row["Salesman_Name"].ToString();
                if (!row.IsNull("Material_Desc"))
                    _materialName = row["Material_Desc"].ToString();
                if (!row.IsNull("Mat_Phase"))
                    _phase = row["Mat_Phase"].ToString();
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("PO_Number"))
                    _poNumber = row["PO_Number"].ToString();
                if (!row.IsNull("ReleasedForFab"))
                    _rFF = row.Field<DateTime>("ReleasedForFab");

                sb_reportReleases.Add(new ReportReleaseNotShip
                {
                    ProjectID = _projectID,
                    ProjectName = _projectName,
                    MatlReqd = _matlReqd,
                    JobNo = _jobNo,
                    SalesmanName = _salesmanName,
                    MaterialName = _materialName,
                    Phase = _phase,
                    ManufName = _manufName,
                    PoNumber = _poNumber,
                    RFF = _rFF
                });
            }
            ReportReleaseNotShips = sb_reportReleases;
        }

        private int _projectID;

        public int ProjectID
        {
            get { return _projectID; }
            set
            {
                if (value == _projectID) return;
                _projectID = value;
                OnPropertyChanged();
            }
        }

        private int _reportID;

        public int ReportID
        {
            get { return _reportID; }
            set
            {
                if (value == _reportID) return;
                _reportID = value;
                ChangeReportID();
                OnPropertyChanged();
            }
        }
         
        private void ChangeReportID()
        {
            ActiveLaborListVisibility = false;
            ActiveProjectListVisibility = false;
            ApprovedVisibility = false;
            ChangeOrderVisibility = false;
            CONotRetVisibility = false;
            CipVisibility = false;
            ContractVisibility = false;
            ContractNotRetVisibility = false;
            CustomerContactVisibility = false;
            FieldMeasureVisibility = false;
            FinalFabVisibility = false;
            InstallForecastVisibility = false;
            JobArchRepVisibility = false;
            JobArchitectVisibility = false;
            JobManufVisibility = false;
            LeedVisibility = false;
            OpenJobVisibility = false;
            PastDueVisibility = false;
            ReleasedVisibility = false;
            ShipNotRecvVisibility = false;
            ShopRecvVisibility = false;
            SubmitVisibility = false;
            ShopReqVisibility = false;
            VendorVisibility = false;
            PmMeetingVisibility = false;
            switch (ReportID)
            {
                case 3:
                    ActiveProjectListVisibility = true;
                    LoadActiveProjectData();
                    break;
                case 7:
                    ShopReqVisibility = true;
                    LoadShopReqData();
                    break;
                case 8:
                    ShopRecvVisibility = true;
                    LoadShopRecvData();
                    break;
                case 9:
                    SubmitVisibility = true;
                    LoadSubmitNotApprData();
                    break;
                case 10:
                    ApprovedVisibility = true;
                    LoadApprovedNotReleaseData();
                    break;
                case 11:
                    ShipNotRecvVisibility = true;
                    LoadShipNotRecvData();
                    break;
                case 12:
                    ReleasedVisibility = true;
                    LoadReleaseNotShipData();
                    break;
                case 13:
                    ActiveLaborListVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 14:
                    OpenJobVisibility = true;
                    LoadOpenJobsData();
                    break;
                case 15:
                    VendorVisibility = true;
                    LoadVendorData();
                    break;
                case 17:
                    LeedVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 18:
                    ContractVisibility = true;
                    LoadContractData();
                    break;
                case 19:
                    CipVisibility = true;
                    LoadCIPData();
                    break;
                case 20:
                    FinalFabVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 21:
                    JobManufVisibility = true;
                    LoadJobByManufacturerData();
                    break;
                case 22:
                    PmMeetingVisibility = true;
                    LoadPmMeetingData();
                    break;
                case 23:
                    ChangeOrderVisibility = true;
                    LoadChangeOrders();
                    break;
                case 24:
                    ContractNotRetVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 25:
                    CONotRetVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 27:
                    CustomerContactVisibility = true;
                    LoadCustomerContactData();
                    break;
                case 28:
                    InstallForecastVisibility = true;
                    LoadActiveLaborData();
                    break;
                case 29:
                    FieldMeasureVisibility = true;
                    LoadFieldMeasureData();
                    break;
                case 30:
                    JobArchitectVisibility = true;
                    LoadArchitectData();
                    break;
                case 31:
                    JobArchRepVisibility = true;
                    LoadArchRepData();
                    break;
                case 32:
                    PastDueVisibility = true;
                    LoadActiveLaborData();
                    break;
                default:
                    break;
            }
        }

        private string _jobNo;

        public string JobNo
        {
            get { return _jobNo; }
            set
            {
                if (value == _jobNo) return;
                _jobNo = value;
                OnPropertyChanged();
            }
        }

        private string _selectedJobNo;

        public string SelectedJobNo
        {
            get { return _selectedJobNo; }
            set
            {
                if (value == _selectedJobNo) return;
                _selectedJobNo = value;
                OnPropertyChanged();
            }
        }

        private int _salesmanID;

        public int SalesmanID
        {
            get { return _salesmanID; }
            set
            {
                if (value == _salesmanID) return;
                _salesmanID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedSalesmanID;

        public int SelectedSalesmanID
        {
            get { return _selectedSalesmanID; }
            set
            {
                if (value == _selectedSalesmanID) return;
                _selectedSalesmanID = value;
                OnPropertyChanged();
            }
        }

        private int _archRepID;

        public int ArchRepID
        {
            get { return _archRepID; }
            set
            {
                if (value == _archRepID) return;
                _archRepID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedArchRepID;

        public int SelectedArchRepID
        {
            get { return _selectedArchRepID; }
            set
            {
                if (value == _selectedArchRepID) return;
                _selectedArchRepID = value;
                OnPropertyChanged();
            }
        }

        private int _customerID;

        public int CustomerID
        {
            get { return _customerID; }
            set
            {
                if (value == _customerID) return;
                _customerID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedCustomerID;

        public int SelectedCustomerID
        {
            get { return _selectedCustomerID; }
            set
            {
                if (value == _selectedCustomerID) return;
                _selectedCustomerID = value;
                OnPropertyChanged();
            }
        }

        private int _complete;

        public int Complete
        {
            get { return _complete; }
            set
            {
                if (value == _complete) return;
                _complete = value;
                OnPropertyChanged();
            }
        }

        private int _selectedComplete;

        public int SelectedComplete
        {
            get { return _selectedComplete; }
            set
            {
                if (value == _selectedComplete) return;
                _selectedComplete = value;
                OnPropertyChanged();
            }
        }

        private string _customerName;

        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                if (value == _customerName) return;
                _customerName = value;
                OnPropertyChanged();
            }
        }

        private int _architectID;

        public int ArchitectID
        {
            get { return _architectID; }
            set
            {
                if (value == _architectID) return;
                _architectID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedArchitectID;

        public int SelectedArchitectID
        {
            get { return _selectedArchitectID; }
            set
            {
                if (value == _selectedArchitectID) return;
                _selectedArchitectID = value;
                OnPropertyChanged();
            }
        }

        private int _manufID;

        public int ManufID
        {
            get { return _manufID; }
            set
            {
                if (value == _manufID) return;
                _manufID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedManufID;

        public int SelectedManufID
        {
            get { return _selectedManufID; }
            set
            {
                if (value == _selectedManufID) return;
                _selectedManufID = value;
                OnPropertyChanged();
            }
        }

        private int _materialID;

        public int MaterialID
        {
            get { return _materialID; }
            set
            {
                if (value == _materialID) return;
                _materialID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedMatID;

        public int SelectedMatID
        {
            get { return _selectedMatID; }
            set
            {
                if (value == _selectedMatID) return;
                _selectedMatID = value;
                OnPropertyChanged();
            }
        }

        private int _crewID;

        public int CrewID
        {
            get { return _crewID; }
            set
            {
                if (value == _crewID) return;
                _crewID = value;
                OnPropertyChanged();
            }
        }

        private int _selectedCrewID;

        public int SelectedCrewID
        {
            get { return _selectedCrewID; }
            set
            {
                if (value == _selectedCrewID) return;
                _selectedCrewID = value;
                OnPropertyChanged();
            }
        }

        private DateTime _dateFrom;

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set
            {
                if (value == _dateFrom) return;
                _dateFrom = value;
                OnPropertyChanged();
            }
        }

        private DateTime _selectedDateFrom;

        public DateTime SelectedDateFrom
        {
            get { return _selectedDateFrom; }
            set
            {
                if (value == _selectedDateFrom) return;
                _selectedDateFrom = value;
                OnPropertyChanged();
            }
        }

        private DateTime _toDate;

        public DateTime ToDate
        {
            get { return _toDate; }
            set
            {
                if (value == _toDate) return;
                _toDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _selectedToDate;

        public DateTime SelectedToDate
        {
            get { return _selectedToDate; }
            set
            {
                if (value == _selectedToDate) return;
                _selectedToDate = value;
                OnPropertyChanged();
            }
        }

        private string _keyword;

        public string Keyword
        {
            get { return _keyword; }
            set
            {
                if (value == _keyword) return;
                _keyword = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ReportActiveLabor> _reportActiveLabors;

        public ObservableCollection<ReportActiveLabor> ReportActiveLabors
        {
            get { return _reportActiveLabors; }
            set
            {
                if (value == _reportActiveLabors) return;
                _reportActiveLabors = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TrackLaborReport> _trackLaborReports;

        public ObservableCollection<TrackLaborReport> TrackLaborReports
        {
            get { return _trackLaborReports; }
            set
            {
                if (value == _trackLaborReports) return;
                _trackLaborReports = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ReportApproveNotRelease> _reportApproveNotReleases;

        public ObservableCollection<ReportApproveNotRelease> ReportApproveNotReleases
        {
            get { return _reportApproveNotReleases; }
            set
            {
                if (value == _reportApproveNotReleases) return;
                _reportApproveNotReleases = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ProjectManager> _projectManagers;

        public ObservableCollection<ProjectManager> ProjectManagers
        {
            get { return _projectManagers; }
            set
            {
                if (_projectManagers != value)
                {
                    _projectManagers = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ProjectMatTracking> _projectMatTrackings;

        public ObservableCollection<ProjectMatTracking> ProjectMatTrackings
        {
            get { return _projectMatTrackings; }
            set
            {
                if (_projectMatTrackings != value)
                {
                    _projectMatTrackings = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ReportFieldMeasure> _reportFieldMeasures;

        public ObservableCollection<ReportFieldMeasure> ReportFieldMeasures
        {
            get { return _reportFieldMeasures; }
            set
            {
                if (_reportFieldMeasures != value)
                {
                    _reportFieldMeasures = value;
                    OnPropertyChanged();
                }
            }
        }

        
        private ObservableCollection<ReportJobArchRep> _reportJobArchReps;

        public ObservableCollection<ReportJobArchRep> ReportJobArchReps
        {
            get { return _reportJobArchReps; }
            set
            {
                if (_reportJobArchReps != value)
                {
                    _reportJobArchReps = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ReportActiveProject> _reportActiveProjects;

        public ObservableCollection<ReportActiveProject> ReportActiveProjects
        {
            get { return _reportActiveProjects; }
            set
            {
                if (_reportActiveProjects != value)
                {
                    _reportActiveProjects = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private ObservableCollection<ReportPmMeeting> _reportPmMeetings;

        public ObservableCollection<ReportPmMeeting> ReportPmMeetings
        {
            get { return _reportPmMeetings; }
            set
            {
                if (_reportPmMeetings != value)
                {
                    _reportPmMeetings = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ReportShipNotRecv> _reportShipNotRecvs;

        public ObservableCollection<ReportShipNotRecv> ReportShipNotRecvs
        {
            get { return _reportShipNotRecvs; }
            set
            {
                if (_reportShipNotRecvs != value)
                {
                    _reportShipNotRecvs = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private ObservableCollection<ReportShopRecv> _reportShopRecvs;

        public ObservableCollection<ReportShopRecv> ReportShopRecvs
        {
            get { return _reportShopRecvs; }
            set
            {
                if (_reportShopRecvs != value)
                {
                    _reportShopRecvs = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ReportShopReq> _reportShopReqs;

        public ObservableCollection<ReportShopReq> ReportShopReqs
        {
            get { return _reportShopReqs; }
            set
            {
                if (_reportShopReqs != value)
                {
                    _reportShopReqs = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ReportSubmit> _reportSubmits;

        public ObservableCollection<ReportSubmit> ReportSubmits
        {
            get { return _reportSubmits; }
            set
            {
                if (_reportSubmits != value)
                {
                    _reportSubmits = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ReportVendor> _reportVendors;

        public ObservableCollection<ReportVendor> ReportVendors
        {
            get { return _reportVendors; }
            set
            {
                if (_reportVendors != value)
                {
                    _reportVendors = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ReportReleaseNotShip> _reportReleaseNotShips;

        public ObservableCollection<ReportReleaseNotShip> ReportReleaseNotShips
        {
            get { return _reportReleaseNotShips; }
            set
            {
                if (_reportReleaseNotShips != value)
                {
                    _reportReleaseNotShips = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _activeProjectListVisibility;

        public bool ActiveProjectListVisibility
        {
            get { return _activeProjectListVisibility; }
            set
            {
                if (value == _activeProjectListVisibility) return;
                _activeProjectListVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _activeLaborListVisibility;

        public bool ActiveLaborListVisibility
        {
            get { return _activeLaborListVisibility; }
            set
            {
                if (value == _activeLaborListVisibility) return;
                _activeLaborListVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _approvedVisibility;

        public bool ApprovedVisibility
        {
            get { return _approvedVisibility; }
            set
            {
                if (value == _approvedVisibility) return;
                _approvedVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _changeOrderVisibility;

        public bool ChangeOrderVisibility
        {
            get { return _changeOrderVisibility; }
            set
            {
                if (value == _changeOrderVisibility) return;
                _changeOrderVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _coNotRetVisibility;

        public bool CONotRetVisibility
        {
            get { return _coNotRetVisibility; }
            set
            {
                if (value == _coNotRetVisibility) return;
                _coNotRetVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _cipVisibility;

        public bool CipVisibility
        {
            get { return _cipVisibility; }
            set
            {
                if (value == _cipVisibility) return;
                _cipVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _contractVisibility;

        public bool ContractVisibility
        {
            get { return _contractVisibility; }
            set
            {
                if (value == _contractVisibility) return;
                _contractVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _contractNotRetVisibility;

        public bool ContractNotRetVisibility
        {
            get { return _contractNotRetVisibility; }
            set
            {
                if (value == _contractNotRetVisibility) return;
                _contractNotRetVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _customerContactVisibility;

        public bool CustomerContactVisibility
        {
            get { return _customerContactVisibility; }
            set
            {
                if (value == _customerContactVisibility) return;
                _customerContactVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _fieldMeasureVisibility;

        public bool FieldMeasureVisibility
        {
            get { return _fieldMeasureVisibility; }
            set
            {
                if (value == _fieldMeasureVisibility) return;
                _fieldMeasureVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _finalFabVisibility;

        public bool FinalFabVisibility
        {
            get { return _finalFabVisibility; }
            set
            {
                if (value == _finalFabVisibility) return;
                _finalFabVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _installForecastVisibility;

        public bool InstallForecastVisibility
        {
            get { return _installForecastVisibility; }
            set
            {
                if (value == _installForecastVisibility) return;
                _installForecastVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _jobArchRepVisibility;

        public bool JobArchRepVisibility
        {
            get { return _jobArchRepVisibility; }
            set
            {
                if (value == _jobArchRepVisibility) return;
                _jobArchRepVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _jobArchitectVisibility;

        public bool JobArchitectVisibility
        {
            get { return _jobArchitectVisibility; }
            set
            {
                if (value == _jobArchitectVisibility) return;
                _jobArchitectVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _jobManufVisibility;

        public bool JobManufVisibility
        {
            get { return _jobManufVisibility; }
            set
            {
                if (value == _jobManufVisibility) return;
                _jobManufVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _leedVisibility;

        public bool LeedVisibility
        {
            get { return _leedVisibility; }
            set
            {
                if (value == _leedVisibility) return;
                _leedVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _openJobVisibility;

        public bool OpenJobVisibility
        {
            get { return _openJobVisibility; }
            set
            {
                if (value == _openJobVisibility) return;
                _openJobVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _pastDueVisibility;

        public bool PastDueVisibility
        {
            get { return _pastDueVisibility; }
            set
            {
                if (value == _pastDueVisibility) return;
                _pastDueVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _pmMeetingVisibility;

        public bool PmMeetingVisibility
        {
            get { return _pmMeetingVisibility; }
            set
            {
                if (value == _pmMeetingVisibility) return;
                _pmMeetingVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _releasedVisibility;

        public bool ReleasedVisibility
        {
            get { return _releasedVisibility; }
            set
            {
                if (value == _releasedVisibility) return;
                _releasedVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _ShipNotRecvVisibility;

        public bool ShipNotRecvVisibility
        {
            get { return _ShipNotRecvVisibility; }
            set
            {
                if (value == _ShipNotRecvVisibility) return;
                _ShipNotRecvVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _shopRecvVisibility;

        public bool ShopRecvVisibility
        {
            get { return _shopRecvVisibility; }
            set
            {
                if (value == _shopRecvVisibility) return;
                _shopRecvVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _shopReqVisibility;

        public bool ShopReqVisibility
        {
            get { return _shopReqVisibility; }
            set
            {
                if (value == _shopReqVisibility) return;
                _shopReqVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _submitVisibility;

        public bool SubmitVisibility
        {
            get { return _submitVisibility; }
            set
            {
                if (value == _submitVisibility) return;
                _submitVisibility = value;
                OnPropertyChanged();
            }
        }

        private bool _vendorVisibility;

        public bool VendorVisibility
        {
            get { return _vendorVisibility; }
            set
            {
                if (value == _vendorVisibility) return;
                _vendorVisibility = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ReportCOItem> _reportCOItem;

        public ObservableCollection<ReportCOItem> ReportCOItems
        {
            get { return _reportCOItem; }
            set
            {
                if (value == _reportCOItem) return;
                _reportCOItem = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ReportCO> _reportCO;

        public ObservableCollection<ReportCO> ReportCOs
        {
            get { return _reportCO; }
            set
            {
                if (value == _reportCO) return;
                _reportCO = value;
                OnPropertyChanged();
            }
        }
        

        private ObservableCollection<ReportContract> _reportContracts;

        public ObservableCollection<ReportContract> ReportContracts
        {
            get { return _reportContracts; }
            set
            {
                if (value == _reportContracts) return;
                _reportContracts = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<CIP> _cip;

        public ObservableCollection<CIP> CIPs
        {
            get { return _cip; }
            set
            {
                if (value == _cip) return;
                _cip = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ReportCIP> _reportCIP;

        public ObservableCollection<ReportCIP> ReportCIPs
        {
            get { return _reportCIP; }
            set
            {
                if (value == _reportCIP) return;
                _reportCIP = value;
                OnPropertyChanged();
            }
        }
        
        private ObservableCollection<ReportCustomerContact> _reportCustomerContacts;

        public ObservableCollection<ReportCustomerContact> ReportCustomerContacts
        {
            get { return _reportCustomerContacts; }
            set
            {
                if (value == _reportCustomerContacts) return;
                _reportCustomerContacts = value;
                OnPropertyChanged();
            }
        }

        private string GetProjectWhereClasuse()
        {
            string whereClause = " WHERE 1=1";
            if (!string.IsNullOrEmpty(SelectedJobNo))
            {
                whereClause += $" AND tblProjects.Job_No = '{SelectedJobNo}'";
            }

            if (ProjectID != 0)
            {
                whereClause += $" AND tblProjects.Project_ID = '{ProjectID}'";
            }

            if (SelectedSalesmanID != 0)
            {
                whereClause += $" AND tblProjects.Salesman_ID = '{SelectedSalesmanID}'";
            }

            if (SelectedCustomerID != 0)
            {
                whereClause += $" AND tblProjects.Customer_ID = '{SelectedCustomerID}'";
            }

            if (!SelectedDateFrom.Equals(DateTime.MinValue))
            {
                whereClause += $" AND tblProjects.Target_Date >= '{SelectedDateFrom}'";
            }

            if (!SelectedToDate.Equals(DateTime.MinValue))
            {
                whereClause += $" AND tblProjects.Date_Completed <= '{SelectedDateFrom}'";
            }

            if (SelectedComplete != 2)
            {
                whereClause += $" AND tblProjects.Complete = '{SelectedComplete}'";
            }
            return whereClause;
        }

        private string GetCustomerWhereClasuse()
        {
            string whereClause = " WHERE 1=1";
           
            if (SelectedCustomerID != 0)
            {
                whereClause += $" AND tblCustomers.Customer_ID = '{SelectedCustomerID}'";
            }

            return whereClause;
        }
    }
}
