using System;
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
            SelectedTempCustIndex = 0;
            SelectedCustRowIndex = -1;
            ActionCustState = "Clear";
            ActionManufState = "ClearManuf";
            ActionCustName = "CreateCustomer";
            ActionManufName = "CreateManuf";
            ActionPmName = "";
            UpdateComponent = "Detail";
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

        private void CreateSubm()
        {
            if (SelectedCustomerID != 0)
            {
                sqlquery = "INSERT INTO tblCustomerContacts(Customer_ID, CC_Name, CC_Phone, CC_CellPhone, CC_Email, Active) OUTPUT INSERTED.CC_ID VALUES (@CustomerID, @CCName, @CCPhone, @CCCell, @CCEmail, @Active)";
                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (SelectedCustomerID != 0)
                        cmd.Parameters.AddWithValue("@CustomerID", SelectedCustomerID);
                    else cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                    if (!string.IsNullOrEmpty(CurrentSubmName))
                        cmd.Parameters.AddWithValue("@CCName", CurrentSubmName);
                    else cmd.Parameters.AddWithValue("@CCName", DBNull.Value);
                    if (!string.IsNullOrEmpty(CurrentSubmPhone))
                        cmd.Parameters.AddWithValue("@CCPhone", CurrentSubmPhone);
                    else cmd.Parameters.AddWithValue("@CCPhone", DBNull.Value);
                    if (!string.IsNullOrEmpty(CurrentSubmCell))
                        cmd.Parameters.AddWithValue("@CCCell", CurrentSubmCell);
                    else cmd.Parameters.AddWithValue("@CCCell", DBNull.Value);
                    if (!string.IsNullOrEmpty(CurrentSubmEmail))
                        cmd.Parameters.AddWithValue("@CCEmail", CurrentSubmEmail);
                    else cmd.Parameters.AddWithValue("@CCEmail", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", CurrentPmActive);
                }

                try
                {
                    int insertedNoteId = (int)cmd.ExecuteScalar();
                    CurrentCCID = insertedNoteId;
                    SelectedCustPmID = CurrentCCID;
                    ActionSubmName = "UpdateSubm";
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                if (!ActionCustState.Equals("Clear"))
                    MessageBox.Show("Customer Full Name is Required");
            }
        }

        private void UpdateSubm()
        {
            if (SelectedCustomerID != 0)
            {
                string sqlquery = "UPDATE tblCustomerContacts SET ";
                List<string> itemList = new List<string>();
                if (!string.IsNullOrEmpty(CurrentSubmName))
                {
                    itemList.Add("CC_Name=@CCName");
                }
                if (!string.IsNullOrEmpty(CurrentSubmPhone))
                {
                    itemList.Add("CC_Phone=@CCPhone");
                }
                if (!string.IsNullOrEmpty(CurrentSubmCell))
                {
                    itemList.Add("CC_CellPhone=@CCCell");
                }
                if (!string.IsNullOrEmpty(CurrentSubmEmail))
                {
                    itemList.Add("CC_Email=@CCEmail");
                }

                itemList.Add("Active=@CCActive");
                sqlquery += string.Join(", ", itemList);
                sqlquery += " WHERE CC_ID=@CCID";

                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (!string.IsNullOrEmpty(CurrentSubmName))
                        cmd.Parameters.AddWithValue("@CCName", CurrentSubmName);
                    if (!string.IsNullOrEmpty(CurrentSubmPhone))
                        cmd.Parameters.AddWithValue("@CCPhone", CurrentSubmPhone);
                    if (!string.IsNullOrEmpty(CurrentSubmCell))
                        cmd.Parameters.AddWithValue("@CCCell", CurrentSubmCell);
                    if (!string.IsNullOrEmpty(CurrentSubmEmail))
                        cmd.Parameters.AddWithValue("@CCEmail", CurrentSubmEmail);

                    int submID = 0;

                    switch (ActionState)
                    {
                        case "AddRow":
                            submID = CurrentCCID;
                            SelectedCustSubmID = CurrentCCID;
                            break;
                        case "UpdateRow":
                            submID = SelectedCustSubmID;
                            break;
                    }

                    cmd.Parameters.AddWithValue("@CCID", submID);
                    cmd.Parameters.AddWithValue("@CCActive", CurrentSubmActive);


                    int rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            else
            {
                if (!ActionCustState.Equals("Clear"))
                    MessageBox.Show("Customer Full Name is Required");
            }
        }

        private void CreateSup()
        {
            if (SelectedCustomerID != 0)
            {
                sqlquery = "INSERT INTO tblSuperintendents(Customer_ID, Sup_Name, Sup_Phone, Sup_CellPhone, Sup_Email, Active) OUTPUT INSERTED.Sup_ID VALUES (@CustomerID, @SupName, @SupPhone, @SupCell, @SupEmail, @Active)";
                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (SelectedCustomerID != 0)
                        cmd.Parameters.AddWithValue("@CustomerID", SelectedCustomerID);
                    else cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                    if (!string.IsNullOrEmpty(CurrentSupName))
                        cmd.Parameters.AddWithValue("@SupName", CurrentSupName);
                    else cmd.Parameters.AddWithValue("@SupName", DBNull.Value);
                    if (!string.IsNullOrEmpty(CurrentSupPhone))
                        cmd.Parameters.AddWithValue("@SupPhone", CurrentSupPhone);
                    else cmd.Parameters.AddWithValue("@SupPhone", DBNull.Value);
                    if (!string.IsNullOrEmpty(CurrentSupCell))
                        cmd.Parameters.AddWithValue("@SupCell", CurrentSupCell);
                    else cmd.Parameters.AddWithValue("@SupCell", DBNull.Value);
                    if (!string.IsNullOrEmpty(CurrentSupEmail))
                        cmd.Parameters.AddWithValue("@SupEmail", CurrentSupEmail);
                    else cmd.Parameters.AddWithValue("@SupEmail", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", CurrentPmActive);
                }

                try
                {
                    int insertedNoteId = (int)cmd.ExecuteScalar();
                    CurrentSupID = insertedNoteId;
                    SelectedCustSupID = CurrentSupID;
                    ActionPmName = "UpdatePM";
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                if (!ActionCustState.Equals("Clear"))
                    MessageBox.Show("Customer Full Name is Required");
            }
        }

        private void UpdateSup()
        {
            if (SelectedCustomerID != 0)
            {
                string sqlquery = "UPDATE tblSuperintendents SET ";
                List<string> itemList = new List<string>();
                if (!string.IsNullOrEmpty(CurrentSupName))
                {
                    itemList.Add("Sup_Name=@SupName");
                }
                if (!string.IsNullOrEmpty(CurrentSupPhone))
                {
                    itemList.Add("Sup_Phone=@SupPhone");
                }
                if (!string.IsNullOrEmpty(CurrentSupCell))
                {
                    itemList.Add("Sup_CellPhone=@SupCell");
                }
                if (!string.IsNullOrEmpty(CurrentSupEmail))
                {
                    itemList.Add("Sup_Email=@SupEmail");
                }

                itemList.Add("Active=@SupActive");
                sqlquery += string.Join(", ", itemList);
                sqlquery += " WHERE Sup_ID=@SupID";

                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (!string.IsNullOrEmpty(CurrentSupName))
                        cmd.Parameters.AddWithValue("@SupName", CurrentSupName);
                    if (!string.IsNullOrEmpty(CurrentSupPhone))
                        cmd.Parameters.AddWithValue("@SupPhone", CurrentSupPhone);
                    if (!string.IsNullOrEmpty(CurrentSupCell))
                        cmd.Parameters.AddWithValue("@SupCell", CurrentSupCell);
                    if (!string.IsNullOrEmpty(CurrentSupEmail))
                        cmd.Parameters.AddWithValue("@SupEmail", CurrentSupEmail);
                    int supID = 0;

                    switch (ActionState)
                    {
                        case "AddRow":
                            supID = CurrentSupID;
                            SelectedCustSupID = CurrentSupID;
                            break;
                        case "UpdateRow":
                            supID = SelectedCustSupID;
                            break;
                    }

                    cmd.Parameters.AddWithValue("@SupID", supID);
                    cmd.Parameters.AddWithValue("@SupActive", CurrentSupActive);

                    int rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            else
            {
                if (!ActionCustState.Equals("Clear"))
                    MessageBox.Show("Customer Full Name is Required");
            }
        }

        private void CreatePM()
        {
            if (SelectedCustomerID != 0)
            {
                sqlquery = "INSERT INTO tblProjectManagers(Customer_ID, PM_Name, PM_Phone, PM_CellPhone, PM_Email, Active) OUTPUT INSERTED.PM_ID VALUES (@CustomerID, @PmName, @PmPhone, @PmCell, @PmEmail, @Active)";
                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (SelectedCustomerID != 0)
                        cmd.Parameters.AddWithValue("@CustomerID", SelectedCustomerID);
                    else cmd.Parameters.AddWithValue("@CustomerID", DBNull.Value);
                    if (!string.IsNullOrEmpty(CurrentPmName))
                        cmd.Parameters.AddWithValue("@PmName", CurrentPmName);
                    else cmd.Parameters.AddWithValue("@PmName", DBNull.Value);
                    if (!string.IsNullOrEmpty(CurrentPmPhone))
                        cmd.Parameters.AddWithValue("@PmPhone", CurrentPmPhone);
                    else cmd.Parameters.AddWithValue("@PmPhone", DBNull.Value);
                    if (!string.IsNullOrEmpty(CurrentPmCell))
                        cmd.Parameters.AddWithValue("@PmCell", CurrentPmCell);
                    else cmd.Parameters.AddWithValue("@PmCell", DBNull.Value);
                    if (!string.IsNullOrEmpty(CurrentPmEmail))
                        cmd.Parameters.AddWithValue("@PmEmail", CurrentPmEmail);
                    else cmd.Parameters.AddWithValue("@PmEmail", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", CurrentPmActive);
                }

                try
                {
                    int insertedPmId = (int)cmd.ExecuteScalar();
                    CurrentPmID = insertedPmId;
                    SelectedCustPmID = CurrentPmID;
                    ActionPmName = "UpdatePM";
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                if (!ActionCustState.Equals("Clear"))
                    MessageBox.Show("Customer Full Name is Required");
            }
        }

        private void UpdatePM()
        {
            string sqlquery = "UPDATE tblProjectManagers SET ";

            List<string> itemList = new List<string>();
            if (!string.IsNullOrEmpty(CurrentPmName))
            {
                itemList.Add("PM_Name=@PmName");
            }
            if (!string.IsNullOrEmpty(CurrentPmPhone))
            {
                itemList.Add("PM_Phone=@PmPhone");
            }
            if (!string.IsNullOrEmpty(CurrentPmCell))
            {
                itemList.Add("PM_CellPhone=@PmCell");
            }
            if (!string.IsNullOrEmpty(CurrentPmEmail))
            {
                itemList.Add("PM_Email=@PmEmail");
            }

            itemList.Add("Active=@PmActive");

            sqlquery += string.Join(", ", itemList);
            sqlquery += " WHERE PM_ID=@PmID";
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(CurrentPmName))
                    cmd.Parameters.AddWithValue("@PmName", CurrentPmName);
                if (!string.IsNullOrEmpty(CurrentPmPhone))
                    cmd.Parameters.AddWithValue("@PmPhone", CurrentPmPhone);
                if (!string.IsNullOrEmpty(CurrentPmCell))
                    cmd.Parameters.AddWithValue("@PmCell", CurrentPmCell);
                if (!string.IsNullOrEmpty(CurrentPmEmail))
                    cmd.Parameters.AddWithValue("@PmEmail", CurrentPmEmail);
                int pmID = 0;
                switch (ActionState)
                {
                    case "AddRow":
                        pmID = CurrentPmID;
                        SelectedCustPmID = CurrentPmID;
                        break;
                    case "UpdateRow":
                        pmID = SelectedCustPmID;
                        break;
                }
                cmd.Parameters.AddWithValue("@PmID", pmID);
                cmd.Parameters.AddWithValue("@PmActive", CurrentPmActive);

                int rowsAffected = cmd.ExecuteNonQuery();
            }
        }

        private void UpdateManuf()
        {
            if (!string.IsNullOrEmpty(SelectedManufName))
            {
                sqlquery = "UPDATE tblManufacturers SET Manuf_Name=@ManufName, Address=@Address, Address2=@Address2, City=@City, State=@State, ZIP=@Zip, Phone=@Phone, FAX=@Fax, Contact_Name=@ContactName, Contact_Phone=@ContactPhone, Contact_Email=@ContactEmail, Active=@Active WHERE Manuf_ID=@ManufID";
                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (!string.IsNullOrEmpty(SelectedManufName))
                        cmd.Parameters.AddWithValue("@ManufName", SelectedManufName);
                    else cmd.Parameters.AddWithValue("@ManufName", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufAddress))
                        cmd.Parameters.AddWithValue("@Address", SelectedManufAddress);
                    else cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufAddress2))
                        cmd.Parameters.AddWithValue("@Address2", SelectedManufAddress2);
                    else cmd.Parameters.AddWithValue("@Address2", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufCity))
                        cmd.Parameters.AddWithValue("@City", SelectedManufCity);
                    else cmd.Parameters.AddWithValue("@City", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufState))
                        cmd.Parameters.AddWithValue("@State", SelectedManufState);
                    else cmd.Parameters.AddWithValue("@State", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufZip))
                        cmd.Parameters.AddWithValue("@Zip", SelectedManufZip);
                    else cmd.Parameters.AddWithValue("@Zip", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufPhone))
                        cmd.Parameters.AddWithValue("@Phone", SelectedManufPhone);
                    else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufFax))
                        cmd.Parameters.AddWithValue("@Fax", SelectedManufFax);
                    else cmd.Parameters.AddWithValue("@Fax", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufContactName))
                        cmd.Parameters.AddWithValue("@ContactName", SelectedManufContactName);
                    else cmd.Parameters.AddWithValue("@ContactName", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufContactPhone))
                        cmd.Parameters.AddWithValue("@ContactPhone", SelectedManufContactPhone);
                    else cmd.Parameters.AddWithValue("@ContactPhone", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufContactEmail))
                        cmd.Parameters.AddWithValue("@ContactEmail", SelectedManufContactEmail);
                    else cmd.Parameters.AddWithValue("@ContactEmail", DBNull.Value);
                    if (SelectedManufID != 0)
                        cmd.Parameters.AddWithValue("@ManufID", SelectedManufID);
                    else cmd.Parameters.AddWithValue("@ManufID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", SelectedManufActive);

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
            } else
            {
                if (!ActionManufState.Equals("ClearManuf"))
                    MessageBox.Show("Manufacturer Name is required.");
            }
        }

        private void CreateManuf()
        {
            if (!string.IsNullOrEmpty(SelectedManufName))
            {
                sqlquery = "INSERT INTO tblManufacturers(Manuf_Name, Address, Address2, City, State, ZIP, Phone, FAX, Contact_Name, Contact_Phone, Contact_Email, Active) OUTPUT INSERTED.Manuf_ID VALUES (@ManufName, @Address, @Address2, @City, @State, @Zip, @Phone, @Fax, @ContactName, @ContactPhone, @ContactEmail, @Active)";

                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (!string.IsNullOrEmpty(SelectedManufName))
                        cmd.Parameters.AddWithValue("@ManufName", SelectedManufName);
                    else cmd.Parameters.AddWithValue("@ManufName", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufAddress))
                        cmd.Parameters.AddWithValue("@Address", SelectedManufAddress);
                    else cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufAddress2))
                        cmd.Parameters.AddWithValue("@Address2", SelectedManufAddress2);
                    else cmd.Parameters.AddWithValue("@Address2", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufCity))
                        cmd.Parameters.AddWithValue("@City", SelectedManufCity);
                    else cmd.Parameters.AddWithValue("@City", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufState))
                        cmd.Parameters.AddWithValue("@State", SelectedManufState);
                    else cmd.Parameters.AddWithValue("@State", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufZip))
                        cmd.Parameters.AddWithValue("@Zip", SelectedManufZip);
                    else cmd.Parameters.AddWithValue("@Zip", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufPhone))
                        cmd.Parameters.AddWithValue("@Phone", SelectedManufPhone);
                    else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufFax))
                        cmd.Parameters.AddWithValue("@Fax", SelectedManufFax);
                    else cmd.Parameters.AddWithValue("@Fax", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufContactName))
                        cmd.Parameters.AddWithValue("@ContactName", SelectedManufContactName);
                    else cmd.Parameters.AddWithValue("@ContactName", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufContactPhone))
                        cmd.Parameters.AddWithValue("@ContactPhone", SelectedManufContactPhone);
                    else cmd.Parameters.AddWithValue("@ContactPhone", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedManufContactEmail))
                        cmd.Parameters.AddWithValue("@ContactEmail", SelectedManufContactEmail);
                    else cmd.Parameters.AddWithValue("@ContactEmail", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", SelectedManufActive);

                    try
                    {
                        int insertedCustomerId = (int)cmd.ExecuteScalar();
                        SelectedManufID = insertedCustomerId;
                        ActionManufName = "UpdateManuf";
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e);
                    }
                    ActionManufState = "";
                }
            } else
            {
                if(!ActionManufState.Equals("ClearManuf"))
                    MessageBox.Show("Manufacturer Name is required.");
            }
        }

        private void UpdateNote()
        {
            sqlquery = "UPDATE tblNotes SET Notes_Note=@NotesNote, Notes_DateAdded=@NotesDateAdded, Notes_User=@NotesUser WHERE Notes_ID=@NotesID";

            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (!string.IsNullOrEmpty(CurrentNotesNote))
                    cmd.Parameters.AddWithValue("@NotesNote", CurrentNotesNote);
                else cmd.Parameters.AddWithValue("@NotesNote", DBNull.Value);
                if (!CurrentNotesDateAdded.Equals(DateTime.MinValue))
                    cmd.Parameters.AddWithValue("@NotesDateAdded", CurrentNotesDateAdded);
                else cmd.Parameters.AddWithValue("@NotesDateAdded", DBNull.Value);
                if (!string.IsNullOrEmpty(CurrnetNoteUser))
                    cmd.Parameters.AddWithValue("@NotesUser", CurrnetNoteUser);
                else cmd.Parameters.AddWithValue("@NotesUser", DBNull.Value);
                if (CurrentNoteID != 0)
                    cmd.Parameters.AddWithValue("@NotesID", CurrentNoteID);
                else cmd.Parameters.AddWithValue("@NotesID", DBNull.Value);
            }
            int rowsAffected = cmd.ExecuteNonQuery();
        }

        private void CreateNote()
        {
            sqlquery = "INSERT INTO tblNotes(Notes_PK, Notes_PK_Desc, Notes_Note, Notes_DateAdded, Notes_User) OUTPUT INSERTED.Notes_ID VALUES (@NotesPK, @NotesDesc, @NotesNote, @NotesDateAdded, @NotesUser)";
            string notesDesc = "";
            int notesPK = 0;
            switch (CurrentNotesDesc)
            {
                case "Customer":
                    notesDesc = "Customer";
                    notesPK = SelectedCustomerID;
                    break;
                case "ProjectManager":
                    notesDesc = "ProjectManager";
                    notesPK = SelectedCustPmID;
                    break;
                case "Superintendent":
                    notesDesc = "Superintendent";
                    notesPK = SelectedCustSupID;
                    break;
                case "CustomerContact":
                    notesDesc = "CustomerContact";
                    notesPK = SelectedCustSubmID;
                    break;
                case "Manuf":
                    notesDesc = "Manuf";
                    notesPK = SelectedManufID;
                    break;

            }
            using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
            {
                if (notesPK != 0)
                    cmd.Parameters.AddWithValue("@NotesPK", notesPK);
                else cmd.Parameters.AddWithValue("@NotesPK", DBNull.Value);
                if (!string.IsNullOrEmpty(CurrentNotesDesc))
                    cmd.Parameters.AddWithValue("@NotesDesc", notesDesc);
                else cmd.Parameters.AddWithValue("@NotesDesc", DBNull.Value);
                if (!string.IsNullOrEmpty(CurrentNotesNote))
                    cmd.Parameters.AddWithValue("@NotesNote", CurrentNotesNote);
                else cmd.Parameters.AddWithValue("@NotesNote", DBNull.Value);
                if (!CurrentNotesDateAdded.Equals(DateTime.MinValue))
                    cmd.Parameters.AddWithValue("@NotesDateAdded", CurrentNotesDateAdded);
                else cmd.Parameters.AddWithValue("@NotesDateAdded", DBNull.Value);
                if (!string.IsNullOrEmpty(CurrnetNoteUser))
                    cmd.Parameters.AddWithValue("@NotesUser", CurrnetNoteUser);
                else cmd.Parameters.AddWithValue("@NotesUser", DBNull.Value);

                try
                {
                    int insertedNoteId = (int)cmd.ExecuteScalar();
                    CurrentNoteID = insertedNoteId;
                    ActionNoteState = "UpdateNote";
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void UpdateCustomer()
        {
            int customerID = (UpdateComponent.Equals("Table")) ? TempCustomer.ID : SelectedCustomerID;
            string fullName = (UpdateComponent.Equals("Table")) ? TempCustomer.FullName : SelectedCustFullName;
            string shortName = (UpdateComponent.Equals("Table")) ? TempCustomer.ShortName : SelectedCustShortName;
            string poNumber = (UpdateComponent.Equals("Table")) ? TempCustomer.PoBox : SelectedCustPoNumber;
            string address = (UpdateComponent.Equals("Table")) ? TempCustomer.Address : SelectedCustAddress;
            string city = (UpdateComponent.Equals("Table")) ? TempCustomer.City : SelectedCustCity;
            string state = (UpdateComponent.Equals("Table")) ? TempCustomer.State : SelectedCustState;
            string zip = (UpdateComponent.Equals("Table")) ? TempCustomer.Zip : SelectedCustZip;
            string phone = (UpdateComponent.Equals("Table")) ? TempCustomer.Phone : SelectedCustPhone;
            string fax = (UpdateComponent.Equals("Table")) ? TempCustomer.Fax : SelectedCustFax;
            string email = (UpdateComponent.Equals("Table")) ? TempCustomer.Email : SelectedCustEmail;
            bool active = (UpdateComponent.Equals("Table")) ? TempCustomer.Active : SelectedCustActive;
            
            if (!string.IsNullOrEmpty(fullName))
            {
                sqlquery = "UPDATE tblCustomers SET Short_Name=@ShortName, Full_Name=@FullName, PO_Box=@PoNumber, Address=@Address, City=@City, State=@State, ZIP=@Zip, Phone=@Phone, FAX=@Fax, Email=@Email, Active=@Active WHERE Customer_ID=@CustomerID";
                Console.WriteLine("SelectedCustRowIndex->"+ SelectedCustRowIndex + "Customers count->" + Customers.Count);
                // Update datatable for Customers
                //if (SelectedCustRowIndex >= 0)
                //{
                //    Customers[SelectedCustRowIndex].FullName = SelectedCustFullName;
                //    Customers[SelectedCustRowIndex].ShortName = SelectedCustShortName;
                //    Customers[SelectedCustRowIndex].PoBox = SelectedCustPoNumber;
                //    Customers[SelectedCustRowIndex].Address = SelectedCustAddress;
                //    Customers[SelectedCustRowIndex].City = SelectedCustCity;
                //    Customers[SelectedCustRowIndex].State = SelectedCustState;
                //    Customers[SelectedCustRowIndex].Zip = SelectedCustZip;
                //    Customers[SelectedCustRowIndex].Phone = SelectedCustPhone;
                //    Customers[SelectedCustRowIndex].Fax = SelectedCustFax;
                //    Customers[SelectedCustRowIndex].Email = SelectedCustEmail;
                //    Customers[SelectedCustRowIndex].Active = SelectedCustActive;
                //}
                //Console.WriteLine(Customers[SelectedCustRowIndex].FullName);
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

        private void CreateCustomer()
        {
            if (!string.IsNullOrEmpty(SelectedCustFullName))
            {
                sqlquery = "INSERT INTO tblCustomers(Short_Name, Full_Name, PO_Box, Address, City, State, ZIP, Phone, FAX, Email, Active) OUTPUT INSERTED.Customer_ID VALUES (@ShortName, @FullName, @PoNumber, @Address, @City, @State, @Zip, @Phone, @Fax, @Email, @Active)";
               
                using (cmd = new SqlCommand(sqlquery, dbConnection.Connection))
                {
                    if (!string.IsNullOrEmpty(SelectedCustFullName))
                        cmd.Parameters.AddWithValue("@FullName", SelectedCustFullName);
                    else cmd.Parameters.AddWithValue("@FullName", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedCustShortName))
                        cmd.Parameters.AddWithValue("@ShortName", SelectedCustShortName);
                    else cmd.Parameters.AddWithValue("@ShortName", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedCustPoNumber))
                        cmd.Parameters.AddWithValue("@PoNumber", SelectedCustPoNumber);
                    else cmd.Parameters.AddWithValue("@PoNumber", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedCustAddress))
                        cmd.Parameters.AddWithValue("@Address", SelectedCustAddress);
                    else cmd.Parameters.AddWithValue("@Address", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedCustCity))
                        cmd.Parameters.AddWithValue("@City", SelectedCustCity);
                    else cmd.Parameters.AddWithValue("@City", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedCustState))
                        cmd.Parameters.AddWithValue("@State", SelectedCustState);
                    else cmd.Parameters.AddWithValue("@State", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedCustZip))
                        cmd.Parameters.AddWithValue("@Zip", SelectedCustZip);
                    else cmd.Parameters.AddWithValue("@Zip", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedCustPhone))
                        cmd.Parameters.AddWithValue("@Phone", SelectedCustPhone);
                    else cmd.Parameters.AddWithValue("@Phone", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedCustFax))
                        cmd.Parameters.AddWithValue("@Fax", SelectedCustFax);
                    else cmd.Parameters.AddWithValue("@Fax", DBNull.Value);
                    if (!string.IsNullOrEmpty(SelectedCustEmail))
                        cmd.Parameters.AddWithValue("@Email", SelectedCustEmail);
                    else cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", SelectedCustActive);

                        
                    try
                    {
                        int insertedCustomerId = (int)cmd.ExecuteScalar();
                        SelectedCustomerID = insertedCustomerId;
                        ActionCustName = "UpdateCustomer";
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e);
                    }
                }
                ActionCustState = "";
            }
            else
            {
                if(!ActionCustState.Equals("Clear"))
                    MessageBox.Show("Customer Full Name is required");
            }
        }

        public void ClearManuf()
        {
            ActionManufName = "CreateManuf";
            ActionManufState = "ClearManuf";
            SelectedManufName = "";
            SelectedManufAddress = "";
            SelectedManufAddress2 = "";
            SelectedManufCity = "";
            SelectedManufState = "";
            SelectedManufZip = "";
            SelectedManufPhone = "";
            SelectedManufFax = "";
            SelectedManufContactName = "";
            SelectedManufContactPhone = "";
            SelectedManufContactEmail = "";
            SelectedManufActive = false;

            Note noteItem = new Note();
            ManufNotes = new ObservableCollection<Note>();
            ManufNotes.Add(noteItem);
        }

        public void ClearCustomer()
        {
            ActionCustState = "Clear";
            ActionCustName = "CreateCustomer";
            SelectedCustomerID = 0;
            SelectedCustFullName = "";
            SelectedCustShortName = "";
            SelectedCustPoNumber = "";
            SelectedCustAddress = "";
            SelectedCustCity = "";
            SelectedCustState = "";
            SelectedCustZip = "";
            SelectedCustPhone = "";
            SelectedCustFax = "";
            SelectedCustEmail = "";
            SelectedCustActive = false;
            UpdateComponent = "Detail";
            Note noteItem = new Note();
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
                SelectedManufName = firstRow["Manuf_Name"].ToString();
            else SelectedManufName = "";
            if (!firstRow.IsNull("Address"))
                SelectedManufAddress = firstRow["Address"].ToString();
            else SelectedManufAddress = "";
            if (!firstRow.IsNull("Address2"))
                SelectedManufAddress2 = firstRow["Address2"].ToString();
            else SelectedManufAddress2 = "";
            if (!firstRow.IsNull("City"))
                SelectedManufCity = firstRow["City"].ToString();
            else SelectedManufCity = "";
            if (!firstRow.IsNull("State"))
                SelectedManufState = firstRow["State"].ToString();
            else SelectedManufState = "";
            if (!firstRow.IsNull("ZIP"))
                SelectedManufZip = firstRow["ZIP"].ToString();
            else SelectedManufZip = "";
            if (!firstRow.IsNull("Phone"))
                SelectedManufPhone = firstRow["Phone"].ToString();
            else SelectedManufPhone = "";
            if (!firstRow.IsNull("FAX"))
                SelectedManufFax = firstRow["FAX"].ToString();
            else SelectedManufFax = "";
            if (!firstRow.IsNull("Contact_Name"))
                SelectedManufContactName = firstRow["Contact_Name"].ToString();
            else SelectedManufContactName = "";
            if (!firstRow.IsNull("Contact_Phone"))
                SelectedManufContactPhone = firstRow["Contact_Phone"].ToString();
            else SelectedManufContactPhone = "";
            if (!firstRow.IsNull("Contact_Email"))
                SelectedManufContactEmail = firstRow["Contact_Email"].ToString();
            else SelectedManufContactEmail = "";
            SelectedManufActive = firstRow.Field<Boolean>("Active");

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
            ActionCustName = "UpdateCustomer";
            SelectedCustomerID = customerID;
            ActionState = "UpdateRow";

            if (!firstRow.IsNull("Full_Name"))
                SelectedCustFullName = firstRow["Full_Name"].ToString();
            if (!firstRow.IsNull("Short_Name"))
                SelectedCustShortName = firstRow["Short_Name"].ToString();
            else SelectedCustShortName = "";
            if (!firstRow.IsNull("PO_Box"))
                SelectedCustPoNumber = firstRow["PO_Box"].ToString();
            else SelectedCustPoNumber = "";
            if (!firstRow.IsNull("Address"))
                SelectedCustAddress = firstRow["Address"].ToString();
            else SelectedCustAddress = "";
            if (!firstRow.IsNull("City"))
                SelectedCustCity = firstRow["City"].ToString();
            else SelectedCustAddress = "";
            if (!firstRow.IsNull("State"))
                SelectedCustState = firstRow["State"].ToString();
            else SelectedCustState = "";
            if (!firstRow.IsNull("ZIP"))
                SelectedCustZip = firstRow["ZIP"].ToString();
            else SelectedCustZip = "";
            if (!firstRow.IsNull("Phone"))
                SelectedCustPhone = firstRow["Phone"].ToString();
            else SelectedCustPhone = "";
            if (!firstRow.IsNull("FAX"))
                SelectedCustFax = firstRow["FAX"].ToString();
            else SelectedCustFax = "";
            if (!firstRow.IsNull("Email"))
                SelectedCustEmail = firstRow["Email"].ToString();
            else SelectedCustEmail = "";
            SelectedCustActive = firstRow.Field<Boolean>("Active");

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

                st_salesman.Add(new Salesman { ID = salesmanID, SalesmanName = name, Phone = phone, Cell = cell, Email = email, Active = active });
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

                st_crew.Add(new Crew { ID = crewID, CrewName = crewDesc, Phone = phone, Cell = cell, Email = email, Active = active });
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

                st_user.Add(new User { ID = id, UserName = name, PersonName = personName, Level = level, FromOnOpen = fromOnOpen, Email = email, Active = active });
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

                st_architect.Add(new Architect { ID = id, ArchCompany = company, Contact = contact, Address = address, City = city, State = state, Zip = zip, Phone = phone, Fax = fax, Cell = cell, Email = email, Active = active });
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

                st_freightCO.Add(new FreightCo { ID = id, FreightName = name, Phone = phone, Email = email, Contact = contact, Cell = cell, Active = active });
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

            cmd.Dispose();
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

        public int SelectedPMRowIndex { get; set; }

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

        private string _selectedCustFullName;

        public string SelectedCustFullName
        {
            get { return _selectedCustFullName; }
            set
            {
                _selectedCustFullName = value;
                OnPropertyChanged();
                switch (ActionCustName)
                {
                    case "CreateCustomer":
                        CreateCustomer();
                        break;
                    case "UpdateCustomer":
                        UpdateCustomer();
                        break;
                }
            }
        }

        private string _selectedCustShortName;

        public string SelectedCustShortName
        {
            get { return _selectedCustShortName; }
            set
            {
                _selectedCustShortName = value;
                OnPropertyChanged();
                switch (ActionCustName)
                {
                    case "CreateCustomer":
                        CreateCustomer();
                        break;
                    case "UpdateCustomer":
                        UpdateCustomer();
                        break;
                }
            }
        }

        private bool _selectedCustActive;

        public bool SelectedCustActive
        {
            get { return _selectedCustActive; }
            set
            {
                _selectedCustActive = value;
                OnPropertyChanged();
                switch (ActionCustName)
                {
                    case "CreateCustomer":
                        CreateCustomer();
                        break;
                    case "UpdateCustomer":
                        UpdateCustomer();
                        break;
                }
            }
        }

        private string _selectedCustPoNumber;

        public string SelectedCustPoNumber
        {
            get { return _selectedCustPoNumber; }
            set
            {
                _selectedCustPoNumber = value;
                OnPropertyChanged();
                switch (ActionCustName)
                {
                    case "CreateCustomer":
                        CreateCustomer();
                        break;
                    case "UpdateCustomer":
                        UpdateCustomer();
                        break;
                }
            }
        }

        private string _selectedCustAddress;

        public string SelectedCustAddress
        {
            get { return _selectedCustAddress; }
            set
            {
                _selectedCustAddress = value;
                OnPropertyChanged();
                switch (ActionCustName)
                {
                    case "CreateCustomer":
                        CreateCustomer();
                        break;
                    case "UpdateCustomer":
                        UpdateCustomer();
                        break;
                }
            }
        }

        private string _selectedCustCity;

        public string SelectedCustCity
        {
            get { return _selectedCustCity; }
            set
            {
                _selectedCustCity = value;
                OnPropertyChanged();
                switch (ActionCustName)
                {
                    case "CreateCustomer":
                        CreateCustomer();
                        break;
                    case "UpdateCustomer":
                        UpdateCustomer();
                        break;
                }
            }
        }

        private string _selectedCustPhone;

        public string SelectedCustPhone
        {
            get { return _selectedCustPhone; }
            set
            {
                _selectedCustPhone = value;
                OnPropertyChanged();
                switch (ActionCustName)
                {
                    case "CreateCustomer":
                        CreateCustomer();
                        break;
                    case "UpdateCustomer":
                        UpdateCustomer();
                        break;
                }
            }
        }

        private string _selectedCustState;

        public string SelectedCustState
        {
            get { return _selectedCustState; }
            set
            {
                _selectedCustState = value;
                OnPropertyChanged();
                switch (ActionCustName)
                {
                    case "CreateCustomer":
                        CreateCustomer();
                        break;
                    case "UpdateCustomer":
                        UpdateCustomer();
                        break;
                }
            }
        }

        private string _selectedCustZip;

        public string SelectedCustZip
        {
            get { return _selectedCustZip; }
            set
            {
                _selectedCustZip = value;
                OnPropertyChanged();
                switch (ActionCustName)
                {
                    case "CreateCustomer":
                        CreateCustomer();
                        break;
                    case "UpdateCustomer":
                        UpdateCustomer();
                        break;
                }
            }
        }

        private string _selectedCustFax;

        public string SelectedCustFax
        {
            get { return _selectedCustFax; }
            set
            {
                _selectedCustFax = value;
                OnPropertyChanged();
                switch (ActionCustName)
                {
                    case "CreateCustomer":
                        CreateCustomer();
                        break;
                    case "UpdateCustomer":
                        UpdateCustomer();
                        break;
                }
            }
        }

        private string _selectedCustEmail;

        public string SelectedCustEmail
        {
            get { return _selectedCustEmail; }
            set
            {
                _selectedCustEmail = value;
                OnPropertyChanged();
                switch (ActionCustName)
                {
                    case "CreateCustomer":
                        CreateCustomer();
                        break;
                    case "UpdateCustomer":
                        UpdateCustomer();
                        break;
                }
            }
        }

        private bool _selectedManufActive;

        public bool SelectedManufActive
        {
            get { return _selectedManufActive; }
            set
            {
                _selectedManufActive = value;
                OnPropertyChanged();
                switch (ActionManufName)
                {
                    case "CreateManuf":
                        CreateManuf();
                        break;
                    case "UpdateManuf":
                        UpdateManuf();
                        break;
                }
            }
        }

        private string _selectedManufName;

        public string SelectedManufName
        {
            get { return _selectedManufName; }
            set
            {
                _selectedManufName = value;
                OnPropertyChanged();
                switch (ActionManufName)
                {
                    case "CreateManuf":
                        CreateManuf();
                        break;
                    case "UpdateManuf":
                        UpdateManuf();
                        break;
                }
            }
        }

        private string _selectedManufAddress;

        public string SelectedManufAddress
        {
            get { return _selectedManufAddress; }
            set
            {
                _selectedManufAddress = value;
                OnPropertyChanged();
                switch (ActionManufName)
                {
                    case "CreateManuf":
                        CreateManuf();
                        break;
                    case "UpdateManuf":
                        UpdateManuf();
                        break;
                }
            }
        }

        private string _selectedManufAddress2;

        public string SelectedManufAddress2
        {
            get { return _selectedManufAddress2; }
            set
            {
                _selectedManufAddress2 = value;
                OnPropertyChanged();
                switch (ActionManufName)
                {
                    case "CreateManuf":
                        CreateManuf();
                        break;
                    case "UpdateManuf":
                        UpdateManuf();
                        break;
                }
            }
        }

        private string _selectedManufCity;

        public string SelectedManufCity
        {
            get { return _selectedManufCity; }
            set
            {
                _selectedManufCity = value;
                OnPropertyChanged();
                switch (ActionManufName)
                {
                    case "CreateManuf":
                        CreateManuf();
                        break;
                    case "UpdateManuf":
                        UpdateManuf();
                        break;
                }
            }
        }

        private string _selectedManufState;

        public string SelectedManufState
        {
            get { return _selectedManufState; }
            set
            {
                _selectedManufState = value;
                OnPropertyChanged();
                switch (ActionManufName)
                {
                    case "CreateManuf":
                        CreateManuf();
                        break;
                    case "UpdateManuf":
                        UpdateManuf();
                        break;
                }
            }
        }

        private string _selectedManufZip;

        public string SelectedManufZip
        {
            get { return _selectedManufZip; }
            set
            {
                _selectedManufZip = value;
                OnPropertyChanged();
                switch (ActionManufName)
                {
                    case "CreateManuf":
                        CreateManuf();
                        break;
                    case "UpdateManuf":
                        UpdateManuf();
                        break;
                }
            }
        }

        private string _selectedManufPhone;

        public string SelectedManufPhone
        {
            get { return _selectedManufPhone; }
            set
            {
                _selectedManufPhone = value;
                OnPropertyChanged();
                switch (ActionManufName)
                {
                    case "CreateManuf":
                        CreateManuf();
                        break;
                    case "UpdateManuf":
                        UpdateManuf();
                        break;
                }
            }
        }

        private string _selectedManufFax;

        public string SelectedManufFax
        {
            get { return _selectedManufFax; }
            set
            {
                _selectedManufFax = value;
                OnPropertyChanged();
                switch (ActionManufName)
                {
                    case "CreateManuf":
                        CreateManuf();
                        break;
                    case "UpdateManuf":
                        UpdateManuf();
                        break;
                }
            }
        }

        private string _selectedManufContactName;

        public string SelectedManufContactName
        {
            get { return _selectedManufContactName; }
            set
            {
                _selectedManufContactName = value;
                OnPropertyChanged();
                switch (ActionManufName)
                {
                    case "CreateManuf":
                        CreateManuf();
                        break;
                    case "UpdateManuf":
                        UpdateManuf();
                        break;
                }
            }
        }

        private string _selectedManufContactPhone;

        public string SelectedManufContactPhone
        {
            get { return _selectedManufContactPhone; }
            set
            {
                _selectedManufContactPhone = value;
                OnPropertyChanged();
                switch (ActionManufName)
                {
                    case "CreateManuf":
                        CreateManuf();
                        break;
                    case "UpdateManuf":
                        UpdateManuf();
                        break;
                }
            }
        }

        private string _selectedManufContactEmail;

        public string SelectedManufContactEmail
        {
            get { return _selectedManufContactEmail; }
            set
            {
                _selectedManufContactEmail = value;
                OnPropertyChanged();
                switch (ActionManufName)
                {
                    case "CreateManuf":
                        CreateManuf();
                        break;
                    case "UpdateManuf":
                        UpdateManuf();
                        break;
                }
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

        private string _ActionPmName;

        public string ActionPmName
        {
            get { return _ActionPmName; }
            set
            {
                _ActionPmName = value;
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

        private string _currentNotesNote;

        public string CurrentNotesNote
        {
            get { return _currentNotesNote; }
            set
            {
                _currentNotesNote = value;
                OnPropertyChanged();
                switch (ActionNoteState)
                {
                    case "CreateNote":
                        CreateNote();
                        break;
                    case "UpdateNote":
                        UpdateNote();
                        break;
                }
            }
        }

        private DateTime _currentNotesDateAdded;

        public DateTime CurrentNotesDateAdded
        {
            get { return _currentNotesDateAdded; }
            set
            {
                _currentNotesDateAdded = value;
                OnPropertyChanged();

            }
        }

        private string _currnetNoteUser;

        public string CurrnetNoteUser
        {
            get { return _currnetNoteUser; }
            set
            {
                _currnetNoteUser = value;
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

        private string _currentPmName;

        public string CurrentPmName
        {
            get { return _currentPmName; }
            set
            {
                _currentPmName = value;
                OnPropertyChanged();
                switch (ActionPmName)
                {
                    case "CreatePM":
                        CreatePM();
                        break;
                    case "UpdatePM":
                        UpdatePM();
                        break;
                }
            }
        }

        private string _currentPmPhone;

        public string CurrentPmPhone
        {
            get { return _currentPmPhone; }
            set
            {
                _currentPmPhone = value;
                OnPropertyChanged();
                switch (ActionPmName)
                {
                    case "CreatePM":
                        CreatePM();
                        break;
                    case "UpdatePM":
                        UpdatePM();
                        break;
                }
            }
        }

        private int _currentPmID;

        public int CurrentPmID
        {
            get { return _currentPmID; }
            set
            {
                _currentPmID = value;
                OnPropertyChanged();
            }
        }

        private string _currentPmCell;

        public string CurrentPmCell
        {
            get { return _currentPmCell; }
            set
            {
                _currentPmCell = value;
                OnPropertyChanged();
                switch (ActionPmName)
                {
                    case "CreatePM":
                        CreatePM();
                        break;
                    case "UpdatePM":
                        UpdatePM();
                        break;
                }
            }
        }

        private bool _currentPmActive;

        public bool CurrentPmActive
        {
            get { return _currentPmActive; }
            set
            {
                _currentPmActive = value;
                OnPropertyChanged();
                switch (ActionPmName)
                {
                    case "CreatePM":
                        CreatePM();
                        break;
                    case "UpdatePM":
                        UpdatePM();
                        break;
                }
            }
        }

        private string _currentPmEmail;

        public string CurrentPmEmail
        {
            get { return _currentPmEmail; }
            set
            {
                _currentPmEmail = value;
                OnPropertyChanged();
                switch (ActionPmName)
                {
                    case "CreatePM":
                        CreatePM();
                        break;
                    case "UpdatePM":
                        UpdatePM();
                        break;
                }
            }
        }

        private string _currentSupName;

        public string CurrentSupName
        {
            get { return _currentSupName; }
            set
            {
                _currentSupName = value;
                OnPropertyChanged();
                switch (ActionSupName)
                {
                    case "CreateSup":
                        CreateSup();
                        break;
                    case "UpdateSup":
                        UpdateSup();
                        break;
                }
            }
        }

        private string _currentSupPhone;

        public string CurrentSupPhone
        {
            get { return _currentSupPhone; }
            set
            {
                _currentSupPhone = value;
                OnPropertyChanged();
                switch (ActionSupName)
                {
                    case "CreateSup":
                        CreateSup();
                        break;
                    case "UpdateSup":
                        UpdateSup();
                        break;
                }
            }
        }

        private string _currentSupCell;

        public string CurrentSupCell
        {
            get { return _currentSupCell; }
            set
            {
                _currentSupCell = value;
                OnPropertyChanged();
                switch (ActionSupName)
                {
                    case "CreateSup":
                        CreateSup();
                        break;
                    case "UpdateSup":
                        UpdateSup();
                        break;
                }
            }
        }

        private string _currentSupEmail;

        public string CurrentSupEmail
        {
            get { return _currentSupEmail; }
            set
            {
                _currentSupEmail = value;
                OnPropertyChanged();
                switch (ActionSupName)
                {
                    case "CreateSup":
                        CreateSup();
                        break;
                    case "UpdateSup":
                        UpdateSup();
                        break;
                }
            }
        }

        private bool _currentSupActive;

        public bool CurrentSupActive
        {
            get { return _currentSupActive; }
            set
            {
                _currentSupActive = value;
                OnPropertyChanged();
                switch (ActionSupName)
                {
                    case "CreateSup":
                        CreateSup();
                        break;
                    case "UpdateSup":
                        UpdateSup();
                        break;
                }
            }
        }

        private int _currentSupID;

        public int CurrentSupID
        {
            get { return _currentSupID; }
            set
            {
                _currentSupID = value;
                OnPropertyChanged();
            }
        }

        private string _currentSubmName;

        public string CurrentSubmName
        {
            get { return _currentSubmName; }
            set
            {
                _currentSubmName = value;
                OnPropertyChanged();
                switch (ActionSubmName)
                {
                    case "CreateSubm":
                        CreateSubm();
                        break;
                    case "UpdateSubm":
                        UpdateSubm();
                        break;
                }
            }
        }

        private string _currentSubmPhone;

        public string CurrentSubmPhone
        {
            get { return _currentSubmPhone; }
            set
            {
                _currentSubmPhone = value;
                OnPropertyChanged();
                switch (ActionSubmName)
                {
                    case "CreateSubm":
                        CreateSubm();
                        break;
                    case "UpdateSubm":
                        UpdateSubm();
                        break;
                }
            }
        }

        private string _currentSubmCell;

        public string CurrentSubmCell
        {
            get { return _currentSubmCell; }
            set
            {
                _currentSubmCell = value;
                OnPropertyChanged();
                switch (ActionSubmName)
                {
                    case "CreateSubm":
                        CreateSubm();
                        break;
                    case "UpdateSubm":
                        UpdateSubm();
                        break;
                }
            }
        }

        private string _currentSubmEmail;

        public string CurrentSubmEmail
        {
            get { return _currentSubmEmail; }
            set
            {
                _currentSubmEmail = value;
                OnPropertyChanged();
                switch (ActionSubmName)
                {
                    case "CreateSubm":
                        CreateSubm();
                        break;
                    case "UpdateSubm":
                        UpdateSubm();
                        break;
                }
            }
        }

        private int _currentCCID;

        public int CurrentCCID
        {
            get { return _currentCCID; }
            set
            {
                _currentCCID = value;
                OnPropertyChanged();
            }
        }
        private bool _currentSubmActive;

        public bool CurrentSubmActive
        {
            get { return _currentSubmActive; }
            set
            {
                _currentSubmActive = value;
                OnPropertyChanged();
                switch (ActionSubmName)
                {
                    case "CreateSubm":
                        CreateSubm();
                        break;
                    case "UpdateSubm":
                        UpdateSubm();
                        break;
                }
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

        public int SelectedTempCustIndex { get; set; }

        private bool _isInitialLoad = false;

        public bool IsInitialLoad
        {
            get { return _isInitialLoad; }
            set
            {
                _isInitialLoad = value;
            }
        }
    }
}