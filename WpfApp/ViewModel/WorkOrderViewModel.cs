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
    class WorkOrderViewModel : ViewModelBase
    {

        private DatabaseConnection dbConnection;
        private SqlCommand cmd = null;
        private SqlDataAdapter sda;
        private DataSet ds;
        private string sqlquery;

        public WorkOrderViewModel()
        {
            dbConnection = new DatabaseConnection();
            LoadWorkOrders();

            Note noteItem = new Note();
            WorkOrderNotes = new ObservableCollection<Note>();
            WorkOrderNotes.Add(noteItem);
        }

        private void LoadWorkOrders()
        {
            // Projects
            string sqlquery = "SELECT tblProjects.Project_ID, tblProjects.Project_Name, tblCustomers.Full_Name FROM tblProjects LEFT JOIN tblCustomers ON tblProjects.Customer_ID = tblCustomers.Customer_ID ORDER BY Project_Name ASC;";
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

            //Crew
            sqlquery = "SELECT Crew_ID, Crew_Name FROM tblInstallCrew;";
            cmd = dbConnection.RunQuryNoParameters(sqlquery);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Crew> st_crew = new ObservableCollection<Crew>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int Num;

                bool isNum = int.TryParse(row["Crew_ID"].ToString(), out Num); //c is your variable
                if (isNum)
                {
                    int crewID = int.Parse(row["Crew_ID"].ToString());
                    string crewName = row["Crew_Name"].ToString();
                    st_crew.Add(new Crew { ID = crewID, CrewName = crewName });
                }

            }
            Crews = st_crew;

            // Superintendent
            sqlquery = "SELECT * FROM tblSuperintendents";
            cmd = dbConnection.RunQuryNoParameters(sqlquery);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Superintendent> st_supt = new ObservableCollection<Superintendent>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int suptID = int.Parse(row["Sup_ID"].ToString());
                string suptName = row["Sup_Name"].ToString();
                string suptCellPhone = row["Sup_CellPhone"].ToString();
                string suptEmail = row["Sup_Email"].ToString();
                st_supt.Add(new Superintendent { SupID = suptID, SupName = suptName, SupCellPhone = suptCellPhone, SupEmail = suptEmail });
            }

            Superintendents = st_supt;
        }

        
        
        
        private int _projectId;
        private int _workOrderID;
        private int _crewID;
        private int _suptID;
        private int _woNumber;
        private DateTime _schedStartDate;
        private DateTime _schedComplDate;
        private DateTime _startDate;
        private DateTime _complDate;

        public ObservableCollection<Project> Projects
        {
            get;
            set;
        }

        public ObservableCollection<Crew> Crews
        {
            get;
            set;
        }

        private ObservableCollection<WorkOrder> _workOrders;

        public ObservableCollection<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set
            {
                if (_workOrders != value)
                {
                    _workOrders = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Superintendent> _superintendent;

        public ObservableCollection<Superintendent> Superintendents
        {
            get { return _superintendent; }
            set
            {
                if (_superintendent != value)
                {
                    _superintendent = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ProjectWorkOrder> _projectWorkOrder;

        public ObservableCollection<ProjectWorkOrder> ProjectWorkOrders
        {
            get { return _projectWorkOrder; }
            set
            {
                if (_projectWorkOrder != value)
                {
                    _projectWorkOrder = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ProjectLabor> _projectLabor;

        public ObservableCollection<ProjectLabor> ProjectLabors
        {
            get { return _projectLabor; }
            set
            {
                if (_projectLabor != value)
                {
                    _projectLabor = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Note> _workOrderNotes;

        public ObservableCollection<Note> WorkOrderNotes
        {
            get { return _workOrderNotes; }
            set
            {
                if (_workOrderNotes != value)
                {
                    _workOrderNotes = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ProjectID
        {
            get { return _projectId; }
            set
            {
                _projectId = value;
                ChangeProject();
            }
        }

        public int WorkOrderID
        {
            get { return _workOrderID; }
            set
            {
                if (_workOrderID != value)
                {
                    _workOrderID = value;
                    OnPropertyChanged();
                    ChangeWorkOrder();
                }
            }
        }

        public int CrewID
        {
            get { return _crewID; }
            set
            {
                if (_crewID != value)
                {
                    _crewID = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SuptID
        {
            get { return _suptID; }
            set
            {
                if (_suptID != value)
                {
                    _suptID = value;
                    OnPropertyChanged();
                }
            }
        }

        public int WoNumber
        {
            get { return _woNumber; }
            set
            {
                if (_woNumber != value)
                {
                    _woNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime SchedStartDate
        {
            get { return _schedStartDate; }
            set
            {
                if (_schedStartDate != value)
                {
                    _schedStartDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime SchedComplDate
        {
            get { return _schedComplDate; }
            set
            {
                if (_schedComplDate != value)
                {
                    _schedComplDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime ComplDate
        {
            get { return _complDate; }
            set
            {
                if (_complDate != value)
                {
                    _complDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ChangeProject()
        {
            // work orders
            sqlquery = "SELECT tblWO.WO_ID, tblWO.Wo_Nbr, tblInstallCrew.Crew_ID, tblInstallCrew.Crew_Name, tblWO.SchedStartDate, tblWO.SchedComplDate, tblWO.Sup_ID, tblWO.Date_Started, tblWO.Date_Completed FROM tblInstallCrew RIGHT JOIN (SELECT * FROM tblWorkOrders WHERE Project_ID = " + ProjectID.ToString() + ") AS tblWO ON tblInstallCrew.Crew_ID = tblWO.Crew_ID; ";

            cmd = dbConnection.RunQuryNoParameters(sqlquery);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            int rowCount = 0;
            rowCount = ds.Tables[0].Rows.Count; // number of rows

            ObservableCollection<WorkOrder> sb_workOrders = new ObservableCollection<WorkOrder>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int woID = 0;
                int woNbr = 0;
                int crewID = 0;
                int suptID = -1;
                string crewName = "";
                DateTime schedStartDate = new DateTime();
                DateTime schedCompDate = new DateTime();
                DateTime dateStarted = new DateTime();
                DateTime dateCompleted = new DateTime();

                woID = row.Field<int>("WO_ID");
                if (!row.IsNull("Wo_Nbr"))
                    woNbr = int.Parse(row["Wo_Nbr"].ToString());
                if (!row.IsNull("Crew_ID"))
                    crewID = row.Field<int>("Crew_ID");
                if (!row.IsNull("Crew_Name"))
                    crewName = row["Crew_Name"].ToString();
                if (!row.IsNull("SchedStartDate"))
                    schedStartDate = row.Field<DateTime>("SchedStartDate");
                if (!row.IsNull("SchedComplDate"))
                    schedCompDate = row.Field<DateTime>("SchedComplDate");
                if (!row.IsNull("Sup_ID"))
                    suptID = int.Parse(row["Sup_ID"].ToString());
                if (!row.IsNull("Date_Started"))
                    dateStarted = row.Field<DateTime>("Date_Started");
                if (!row.IsNull("Date_Completed"))
                    dateCompleted = row.Field<DateTime>("Date_Completed");
                sb_workOrders.Add(new WorkOrder
                {
                    WoID = woID,
                    WoNumber = woNbr,
                    CrewID = crewID,
                    CrewName = crewName,
                    SchedStartDate = schedStartDate,
                    SchedComplDate = schedCompDate,
                    SuptID = suptID,
                    DateStarted = dateStarted,
                    DateCompleted = dateCompleted
                });
            }

            WorkOrders = sb_workOrders;

            // Project WorkOrders List
            sqlquery = " SELECT tblWorkOrdersMat.Mat_Qty, tblMat.* FROM tblWorkOrdersMat RIGHT JOIN ( SELECT tblProjectSOV.SOV_Acronym, tblMat.* FROM tblProjectSOV RIGHT JOIN ( SELECT tblMat.*, tblProjectMaterialsShip.Qty_Recvd, tblProjectMaterialsShip.ProjMS_ID FROM tblProjectMaterialsShip RIGHT JOIN ( SELECT tblManufacturers.Manuf_Name, tblMat.* FROM tblManufacturers RIGHT JOIN (  SELECT tblMaterials.Material_Desc,tblMat.* FROM tblMaterials RIGHT JOIN (SELECT tblProjectMaterialsTrack.MatReqdDate, tblProjectMaterialsTrack.TakeFromStock, tblMat.*, tblProjectMaterialsTrack.ProjMT_ID, tblProjectMaterialsTrack.Manuf_ID, tblProjectMaterialsTrack.Qty_Ord  FROM tblProjectMaterialsTrack RIGHT JOIN (SELECT Project_ID, ProjMat_ID, ProjSOV_ID ,Material_ID, Qty_Reqd FROM tblProjectMaterials WHERE Project_ID = " + ProjectID.ToString() + ") AS tblMat ON tblProjectMaterialsTrack.ProjMat_ID = tblMat.ProjMat_ID) AS tblMat ON tblMaterials.Material_ID = tblMat.Material_ID) AS tblMat ON tblMat.Manuf_ID = tblManufacturers.Manuf_ID) AS tblMat ON tblMat.ProjMT_ID = tblProjectMaterialsShip.ProjMT_ID) AS tblMat ON tblMat.ProjSOV_ID = tblProjectSOV.ProjSOV_ID) AS tblMat ON tblWorkOrdersMat.ProjMS_ID = tblMat.ProjMS_ID ORDER BY Material_Desc";

            cmd = dbConnection.RunQuryNoParameters(sqlquery);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ProjectWorkOrder> sb_projectWorkOrder = new ObservableCollection<ProjectWorkOrder>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int _projectID = 0;
                string _sovAcronym = "";
                string _matName = "";
                string _manufName = "";
                bool _stock = false;
                DateTime _matlReqd = new DateTime();
                int _qtyReqd = 0;
                int _qtyOrd = 0;
                int _qtyRecvd = 0;
                int _matQty = 0;

                if (!row.IsNull("Project_ID"))
                    _projectID = row.Field<int>("Project_ID");
                if (!row.IsNull("SOV_Acronym"))
                    _sovAcronym = row["SOV_Acronym"].ToString();
                if (!row.IsNull("Material_Desc"))
                    _matName = row["Material_Desc"].ToString(); ;
                if (!row.IsNull("Manuf_Name"))
                    _manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("TakeFromStock"))
                    _stock = row.Field<Boolean>("TakeFromStock");
                if (!row.IsNull("MatReqdDate"))
                    _matlReqd = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Qty_Reqd"))
                    _qtyReqd = int.Parse(row["Qty_Reqd"].ToString());
                if (!row.IsNull("Qty_Ord"))
                    _qtyOrd = int.Parse(row["Qty_Ord"].ToString());
                if (!row.IsNull("Qty_Recvd"))
                    _qtyRecvd = int.Parse(row["Qty_Recvd"].ToString());
                if (!row.IsNull("Mat_Qty"))
                    _matQty = int.Parse(row["Mat_Qty"].ToString());

                sb_projectWorkOrder.Add(new ProjectWorkOrder
                {
                    ProjectID = _projectID,
                    SovAcronym = _sovAcronym,
                    MatName = _matName,
                    ManufName = _manufName,
                    Stock = _stock,
                    MatlReqd = _matlReqd,
                    QtyReqd = _qtyReqd,
                    QtyOrd = _qtyOrd,
                    QtyRecvd = _qtyRecvd,
                    MatQty = _matQty
                });
            }
            ProjectWorkOrders = sb_projectWorkOrder;

            // Project Labor List
            sqlquery = "SELECT CO_ItemNo, tblLab.* FROM tblProjectChangeOrders RIGHT JOIN (SELECT tblProjectSOV.SOV_Acronym, tblProjectSOV.CO_ID, tblLab.Labor_Desc, tblLab.Qty_Reqd, tblLab.UnitPrice, tblLab.Lab_Phase FROM tblProjectSOV RIGHT JOIN ( SELECT tblLabor.Labor_Desc, tblLab.*  FROM tblLabor RIGHT JOIN ( SELECT * FROM tblProjectLabor WHERE Project_ID = " + ProjectID.ToString() + ") AS tblLab ON tblLabor.Labor_ID = tblLab.Labor_ID) AS tblLab ON tblProjectSOV.ProjSOV_ID = tblLab.ProjSOV_ID) AS tblLab ON tblProjectChangeOrders.CO_ID = tblLab.CO_ID ORDER BY tblLab.SOV_Acronym";

            cmd = dbConnection.RunQuryNoParameters(sqlquery);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<ProjectLabor> sb_projectLabor = new ObservableCollection<ProjectLabor>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string _sovAcronym = "";
                int _laborID = 0;
                int _qtyReqd = 0;
                double _unitPrice = 0;
                //int    = 0;
                int _changeOrder = 0;
                string _phase = "";
                if (!row.IsNull("SOV_Acronym"))
                    _sovAcronym = row["SOV_Acronym"].ToString();
                if (!row.IsNull("Labor_ID"))
                    _laborID = int.Parse(row["Labor_ID"].ToString());
                if (!row.IsNull("Qty_Reqd"))
                    _qtyReqd = int.Parse(row["Qty_Reqd"].ToString());
                if (!row.IsNull("UnitPrice"))
                    _unitPrice = row.Field<double>("UnitPrice");
                if (!row.IsNull("CO_ItemNo"))
                    _changeOrder = int.Parse(row["CO_ItemNo"].ToString());
                if (!row.IsNull("Lab_Phase"))
                    _phase = row["Lab_Phase"].ToString();
                sb_projectLabor.Add(new ProjectLabor
                {
                    ProjectID = ProjectID,
                    SovAcronymName = _sovAcronym,
                    LaborID = _laborID,
                    QtyReqd = _qtyReqd,
                    UnitPrice = _unitPrice,
                    CoItemNo = _changeOrder,
                    Phase = _phase
                });
            }
            ProjectLabors = sb_projectLabor;
        }

        private void ChangeWorkOrder()
        {
            WorkOrder _workOrder = WorkOrders.Where(item => item.WoID == WorkOrderID).ToList()[0];
            CrewID = _workOrder.CrewID;
            SuptID = _workOrder.SuptID;
            WoNumber = _workOrder.WoNumber;
            SchedStartDate = _workOrder.SchedStartDate;
            SchedComplDate = _workOrder.SchedComplDate;
            StartDate = _workOrder.DateStarted;
            ComplDate = _workOrder.DateCompleted;

            // Work Order Notes
            sqlquery = "SELECT * FROM tblNotes WHERE Notes_PK = " + WorkOrderID + " AND Notes_PK_Desc = 'WorkOrder';";
            cmd = dbConnection.RunQuryNoParameters(sqlquery);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            ObservableCollection<Note> st_workOrderNotes = new ObservableCollection<Note>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string notesUser = "";
                string notesNote = "";
                string notesUserName = "";
                int notesID = 0;
                int notesPK = 0;
                DateTime notesDateAdded = new DateTime();
                string notesPKDesc = "";

                if (!row.IsNull("Notes_ID"))
                    notesID = row.Field<int>("Notes_ID");
                if (!row.IsNull("Notes_PK"))
                    notesPK = row.Field<int>("Notes_PK");
                if (!row.IsNull("Notes_Note"))
                    notesNote = row["Notes_Note"].ToString();
                if (!row.IsNull("Notes_User"))
                    notesUser = row["Notes_User"].ToString();
                if (!row.IsNull("Notes_UserName"))
                    notesUserName = row["Notes_UserName"].ToString();
                if (!row.IsNull("Notes_PK_Desc"))
                    notesPKDesc = row["Notes_PK_Desc"].ToString();
                if (!row.IsNull("Notes_DateAdded"))
                    notesDateAdded = row.Field<DateTime>("Notes_DateAdded");

                st_workOrderNotes.Add(new Note
                {
                    NoteID = notesID,
                    NotePK = notesPK,
                    NotesPKDesc = notesPKDesc,
                    NotesNote = notesNote,
                    NoteUser = notesUser,
                    NoteUserName = notesUserName,
                    NotesDateAdded = notesDateAdded
                });
            }

            WorkOrderNotes = st_workOrderNotes;
        }
    }
}
