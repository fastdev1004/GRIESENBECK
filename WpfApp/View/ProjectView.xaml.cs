using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using WpfApp.ViewModel;
using WpfApp.Model;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Data;
using WpfApp.Controls;
using System.Drawing;
using System.Windows.Media;
using WpfApp.Utils;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for ViewProject.xaml
    /// </summary>
    public partial class ProjectView : Page
    {
        private DatabaseConnection dbConnection;
        private string sqlquery;
        private SqlCommand cmd;
        private SqlDataAdapter sda;
        private DataSet ds;
        private ObservableCollection<int> st_projectOrder;
        private ObservableCollection<SovCOItem> st_SovCOItem;
        private ObservableCollection<Note> sb_notes;
        private ObservableCollection<InstallationNote> sb_installationNote;
        private ObservableCollection<Contract> sb_contract;
        private ObservableCollection<CIP> sb_cip;

        private ProjectViewModel ProjectVM;
        public ProjectView()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            dbConnection.Open();
            st_projectOrder = new ObservableCollection<int>();
            st_SovCOItem = new ObservableCollection<SovCOItem>();
            sb_notes = new ObservableCollection<Note>();
            sb_installationNote = new ObservableCollection<InstallationNote>();
            sb_contract = new ObservableCollection<Contract>();
            sb_cip = new ObservableCollection<CIP>();
            ProjectVM = new ProjectViewModel();
            this.DataContext = ProjectVM;
        }

        private void goBack(object sender, RoutedEventArgs e)
        {
            dbConnection.Close();
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }

        private void SelectProjectItem(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Project selectedItem = (Project)comboBox.SelectedItem;
            if (((ComboBox)sender).SelectedIndex != -1)
            {
                int selectedProjectID = selectedItem.ID;

                // data for Change Orders ComboBox
                sqlquery = "select * from tblProjectChangeOrders where Project_ID = " + selectedProjectID.ToString();
                cmd = new SqlCommand(sqlquery, dbConnection.Connection);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);

                st_projectOrder.Clear();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    st_projectOrder.Add(int.Parse(row["CO_ItemNo"].ToString()));
                }

                // data for SovCo
                sqlquery = "Select DISTINCT tblSOV.SOV_Acronym, tblSOV.CO_ItemNo from(Select tblSOV.*, tblProjectChangeOrders.CO_ItemNo from(Select tblSOV.*, tblScheduleOfValues.SOV_Desc from tblScheduleOfValues Right JOIN(SELECT tblProjectSOV.* From tblProjects LEFT Join tblProjectSOV ON tblProjects.Project_ID = tblProjectSOV.Project_ID where tblProjects.Project_ID = " + selectedProjectID.ToString() + " ) AS tblSOV ON tblSOV.SOV_Acronym = tblScheduleOfValues.SOV_Acronym Where tblScheduleOfValues.Active = 'true') AS tblSOV LEFT JOIN tblProjectChangeOrders ON tblProjectChangeOrders.CO_ID = tblSOV.CO_ID) AS tblSOV LEFT JOIN tblProjectMaterials ON tblSOV.ProjSOV_ID = tblProjectMaterials.ProjSOV_ID ORDER BY tblSOV.SOV_Acronym DESC";
                cmd = new SqlCommand(sqlquery, dbConnection.Connection);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);

                st_SovCOItem.Clear();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string sovAcronym = row["SOV_Acronym"].ToString();
                    string coItem = row["CO_ItemNo"].ToString();
                    st_SovCOItem.Add(new SovCOItem
                    {
                        SovAcronym = sovAcronym,
                        CoItem = coItem,
                    });
                }

                int rowCount = 1;
                int rowIndex = 0;

                // PM Grid
                sqlquery = "Select * from tblProjectPMs where Project_ID = " + selectedProjectID.ToString();
                cmd = new SqlCommand(sqlquery, dbConnection.Connection);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                rowCount = ds.Tables[0].Rows.Count; // number of rows
                PMGrid.Children.Clear();
                for (int i = 0; i < rowCount; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                    PMGrid.RowDefinitions.Add(rowDef);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sqlquery = "select * from tblProjectManagers where PM_ID =" + row["PM_ID"].ToString();
                    cmd = new SqlCommand(sqlquery, dbConnection.Connection);
                    sda = new SqlDataAdapter(cmd);
                    DataSet ds1 = new DataSet();
                    sda.Fill(ds1);
                    ComboBox pmComBoBox = new ComboBox();
                    DataRow firstRow = ds1.Tables[0].Rows[0];
                    Label la_cellPhone = new Label();

                    la_cellPhone.HorizontalAlignment = HorizontalAlignment.Left;
                    Label la_email = new Label();

                    la_cellPhone.Content = "";
                    la_email.Content = "";
                    if (!string.IsNullOrEmpty(firstRow["PM_CellPhone"].ToString()))
                    {
                        la_cellPhone.Content = firstRow["PM_CellPhone"];
                    }
                    if (!string.IsNullOrEmpty(firstRow["PM_Email"].ToString()))
                    {
                        la_email.Content = firstRow["PM_Email"];
                    }

                    pmComBoBox.ItemsSource = ProjectVM.ProjectManagers;
                    pmComBoBox.SelectedValuePath = "ID";
                    pmComBoBox.DisplayMemberPath = "PMName";
                    pmComBoBox.SelectedValue = int.Parse(row["PM_ID"].ToString());
                    pmComBoBox.IsEditable = true;

                    Grid myInnerGrid = new Grid();
                    RowDefinition rowDef1 = new RowDefinition();
                    rowDef1.Height = GridLength.Auto;
                    RowDefinition rowDef2 = new RowDefinition();
                    rowDef2.Height = GridLength.Auto;
                    myInnerGrid.RowDefinitions.Add(rowDef1);
                    myInnerGrid.RowDefinitions.Add(rowDef2);

                    Grid secondGrid = new Grid();
                    ColumnDefinition colDef1 = new ColumnDefinition();
                    ColumnDefinition colDef2 = new ColumnDefinition();
                    secondGrid.ColumnDefinitions.Add(colDef1);
                    secondGrid.ColumnDefinitions.Add(colDef2);
                    secondGrid.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
                    secondGrid.ColumnDefinitions[1].Width = new GridLength(5, GridUnitType.Star);
                    Grid.SetColumn(la_cellPhone, 0);
                    Grid.SetColumn(la_email, 1);
                    secondGrid.Children.Add(la_cellPhone);
                    secondGrid.Children.Add(la_email);

                    Grid.SetRow(pmComBoBox, 0);
                    Grid.SetRow(secondGrid, 1);
                    myInnerGrid.Children.Add(pmComBoBox);
                    myInnerGrid.Children.Add(secondGrid);

                    Grid.SetRow(myInnerGrid, rowIndex);
                    PMGrid.Children.Add(myInnerGrid);

                    rowIndex += 1;
                }

                rowCount = 1;
                rowIndex = 0;

                // Supt Grid
                sqlquery = "SELECT * FROM tblSuperintendents RIGHT JOIN (SELECT * FROM tblProjectSups WHERE Project_ID = " + selectedProjectID.ToString() + ") AS tblProject ON tblSuperintendents.Sup_ID = tblProject.Sup_ID;";
                cmd = new SqlCommand(sqlquery, dbConnection.Connection);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                rowCount = ds.Tables[0].Rows.Count; // number of rows
                SuptGrid.Children.Clear();

                for (int i = 0; i < rowCount; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                    SuptGrid.RowDefinitions.Add(rowDef);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ComboBox suptComBoBox = new ComboBox();

                    Label la_cellPhone = new Label();
                    la_cellPhone.HorizontalAlignment = HorizontalAlignment.Left;
                    Label la_email = new Label();

                    la_cellPhone.Content = "";
                    la_email.Content = "";

                    if (!row.IsNull("Sup_CellPhone"))
                    {
                        la_cellPhone.Content = row["Sup_CellPhone"].ToString();
                    }
                    if (!row.IsNull("Sup_Email"))
                    {
                        la_email.Content = row["Sup_Email"].ToString();
                    }

                    suptComBoBox.ItemsSource = ProjectVM.Superintendents;
                    suptComBoBox.SelectedValuePath = "SupID";
                    suptComBoBox.DisplayMemberPath = "SupName";
                    suptComBoBox.SelectedValue = int.Parse(row["Sup_ID"].ToString());
                    suptComBoBox.IsEditable = true;

                    Grid myInnerGrid = new Grid();
                    RowDefinition rowDef1 = new RowDefinition();
                    rowDef1.Height = GridLength.Auto;
                    RowDefinition rowDef2 = new RowDefinition();
                    rowDef2.Height = GridLength.Auto;
                    myInnerGrid.RowDefinitions.Add(rowDef1);
                    myInnerGrid.RowDefinitions.Add(rowDef2);

                    Grid secondGrid = new Grid();
                    ColumnDefinition colDef1 = new ColumnDefinition();
                    ColumnDefinition colDef2 = new ColumnDefinition();
                    secondGrid.ColumnDefinitions.Add(colDef1);
                    secondGrid.ColumnDefinitions.Add(colDef2);
                    secondGrid.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
                    secondGrid.ColumnDefinitions[1].Width = new GridLength(5, GridUnitType.Star);

                    Grid.SetColumn(la_cellPhone, 0);
                    Grid.SetColumn(la_email, 1);
                    secondGrid.Children.Add(la_cellPhone);
                    secondGrid.Children.Add(la_email);

                    Grid.SetRow(suptComBoBox, 0);
                    Grid.SetRow(secondGrid, 1);
                    myInnerGrid.Children.Add(suptComBoBox);
                    myInnerGrid.Children.Add(secondGrid);

                    Grid.SetRow(myInnerGrid, rowIndex);
                    SuptGrid.Children.Add(myInnerGrid);

                    rowIndex += 1;
                }

                // ProjectNote Grid
                sqlquery = "SELECT * FROM tblNotes WHERE Notes_PK_Desc = 'Project' AND Notes_PK =" + selectedProjectID.ToString();
                cmd = new SqlCommand(sqlquery, dbConnection.Connection);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                rowCount = ds.Tables[0].Rows.Count; // number of rows
                ProjectNoteGrid.Children.Clear();
                rowIndex = 0;

                for (int i = 0; i < rowCount; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                    ProjectNoteGrid.RowDefinitions.Add(rowDef);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Grid firstGrid = new Grid();
                    for (int i = 0; i < 2; i++)
                    {
                        RowDefinition rowDef = new RowDefinition();
                        rowDef.Height = GridLength.Auto;
                        firstGrid.RowDefinitions.Add(rowDef);
                    }

                    Label NoteDateAdded_LB = new Label();
                    TextBox ProjectNote_TB = new TextBox();
                    ProjectNote_TB.Height = 40;
                    ProjectNote_TB.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    NoteDateAdded_LB.Content = row["Notes_DateAdded"].ToString();
                    ProjectNote_TB.Text = row["Notes_Note"].ToString();

                    Grid.SetRow(NoteDateAdded_LB, 0);
                    Grid.SetRow(ProjectNote_TB, 1);

                    firstGrid.Children.Add(NoteDateAdded_LB);
                    firstGrid.Children.Add(ProjectNote_TB);

                    Grid.SetRow(firstGrid, rowIndex);
                    ProjectNoteGrid.Children.Add(firstGrid);

                    rowIndex += 1;
                }

                // SOVGrid2
                rowIndex = 0;
                sqlquery = "Select tblSOV.SOV_Acronym, tblSOV.CO_ItemNo, tblSOV.Material_Only, tblSOV.SOV_Desc, tblProjectMaterials.Mat_Phase, tblProjectMaterials.Mat_Type, tblProjectMaterials.Color_Selected, tblProjectMaterials.Qty_Reqd, tblProjectMaterials.TotalCost from(Select tblSOV.*, tblProjectChangeOrders.CO_ItemNo from(Select tblSOV.*, tblScheduleOfValues.SOV_Desc from tblScheduleOfValues Right JOIN(SELECT tblProjectSOV.* From tblProjects LEFT Join tblProjectSOV ON tblProjects.Project_ID = tblProjectSOV.Project_ID where tblProjects.Project_ID = " + selectedProjectID.ToString() + " ) AS tblSOV ON tblSOV.SOV_Acronym = tblScheduleOfValues.SOV_Acronym Where tblScheduleOfValues.Active = 'true') AS tblSOV LEFT JOIN tblProjectChangeOrders ON tblProjectChangeOrders.CO_ID = tblSOV.CO_ID) AS tblSOV LEFT JOIN tblProjectMaterials ON tblSOV.ProjSOV_ID = tblProjectMaterials.ProjSOV_ID ORDER BY tblSOV.SOV_Acronym DESC";
                cmd = new SqlCommand(sqlquery, dbConnection.Connection);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                rowCount = ds.Tables[0].Rows.Count; // number of rows
                for (int i = 0; i < rowCount; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ComboBox SOV_ComboBox = new ComboBox();
                    Label CO_LB = new Label();
                    CheckBox MatOnly_CheckBox = new CheckBox();
                    ComboBox Material_ComboBox = new ComboBox();
                    TextBox Phase_TextBox = new TextBox();
                    TextBox Type_TextBox = new TextBox();
                    TextBox Color_TextBox = new TextBox();
                    TextBox QtyReqd_TextBox = new TextBox();
                    TextBox TotalCost_TextBox = new TextBox();

                    SOV_ComboBox.Margin = new Thickness(4, 1, 4, 1);
                    CO_LB.Margin = new Thickness(4, 1, 4, 1);
                    MatOnly_CheckBox.Margin = new Thickness(4, 1, 4, 1);
                    Material_ComboBox.Margin = new Thickness(4, 1, 4, 1);
                    Phase_TextBox.Margin = new Thickness(4, 1, 4, 1);
                    Type_TextBox.Margin = new Thickness(4, 1, 4, 1);
                    Color_TextBox.Margin = new Thickness(4, 1, 4, 1);
                    QtyReqd_TextBox.Margin = new Thickness(4, 1, 4, 1);
                    TotalCost_TextBox.Margin = new Thickness(4, 1, 4, 1);

                    CO_LB.HorizontalAlignment = HorizontalAlignment.Center;
                    MatOnly_CheckBox.HorizontalAlignment = HorizontalAlignment.Center;
                    MatOnly_CheckBox.VerticalAlignment = VerticalAlignment.Center;
                    //QtyReqd_TextBox.HorizontalAlignment = HorizontalAlignment.Left;
                    //TotalCost_TextBox.HorizontalAlignment = HorizontalAlignment.Left;

                    //FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));

                    //var column1 = new FrameworkElementFactory(typeof(ColumnDefinition));
                    //column1.SetValue(ColumnDefinition.WidthProperty, new GridLength(1, GridUnitType.Star));
                    //var column2 = new FrameworkElementFactory(typeof(ColumnDefinition));
                    //column2.SetValue(ColumnDefinition.WidthProperty, new GridLength(1, GridUnitType.Star));

                    //gridFactory.AppendChild(column1);
                    //gridFactory.AppendChild(column2);

                    //FrameworkElementFactory labelBlockFactory1 = new FrameworkElementFactory(typeof(Label));
                    //labelBlockFactory1.SetBinding(Label.ContentProperty, new Binding("SovAcronym"));
                    //gridFactory.AppendChild(labelBlockFactory1);

                    //FrameworkElementFactory labeltextBlockFactory2 = new FrameworkElementFactory(typeof(Label));
                    //labeltextBlockFactory2.SetBinding(Label.ContentProperty, new Binding("CoItem"));
                    //labeltextBlockFactory2.SetValue(Grid.ColumnProperty, 1);
                    //gridFactory.AppendChild(labeltextBlockFactory2);

                    //DataTemplate sovItemTemplate = new DataTemplate();
                    //sovItemTemplate.DataType = typeof(SovCO);
                    //sovItemTemplate.VisualTree = gridFactory;
                    //ContentPresenter contentPresenter = new ContentPresenter();
                    //contentPresenter.ContentTemplate = sovItemTemplate;

                    SOV_ComboBox.ItemsSource = st_SovCOItem;
                    //SOV_ComboBox.ItemTemplate = sovItemTemplate;
                    SOV_ComboBox.DisplayMemberPath = "SovAcronym";
                    SOV_ComboBox.SelectedValuePath = "SovAcronym";
                    SOV_ComboBox.SelectedValue = row["SOV_Acronym"].ToString();
                    SOV_ComboBox.IsEditable = true;
                    SOV_ComboBox.Margin = new Thickness(4, 1, 4, 1);

                    Material_ComboBox.ItemsSource = ProjectVM.Materials;
                    Material_ComboBox.SelectedValuePath = "MatDesc";
                    Material_ComboBox.DisplayMemberPath = "MatDesc";
                    Material_ComboBox.SelectedValue = row["SOV_Desc"].ToString();
                    Material_ComboBox.IsEditable = true;
                    Material_ComboBox.Margin = new Thickness(4, 1, 4, 1);

                    if (!row.IsNull("Material_Only"))
                        MatOnly_CheckBox.IsChecked = row.Field<Boolean>("Material_Only");
                    if (!row.IsNull("CO_ItemNo"))
                        CO_LB.Content = row["CO_ItemNo"].ToString();
                    if (!row.IsNull("Mat_Phase"))
                        Phase_TextBox.Text = row["MAT_Phase"].ToString();
                    if (!row.IsNull("Mat_Type"))
                        Type_TextBox.Text = row["MAT_Type"].ToString();
                    if (!row.IsNull("Color_Selected"))
                        Color_TextBox.Text = row["Color_Selected"].ToString();
                    if (!row.IsNull("Qty_Reqd"))
                        QtyReqd_TextBox.Text = row["Qty_Reqd"].ToString();
                    if (!row.IsNull("TotalCost"))
                        TotalCost_TextBox.Text = row["TotalCost"].ToString();

                    Grid secondGrid = new Grid();
                    ColumnDefinition colDef1 = new ColumnDefinition();
                    ColumnDefinition colDef2 = new ColumnDefinition();
                    ColumnDefinition colDef3 = new ColumnDefinition();
                    ColumnDefinition colDef4 = new ColumnDefinition();
                    ColumnDefinition colDef5 = new ColumnDefinition();
                    ColumnDefinition colDef6 = new ColumnDefinition();
                    ColumnDefinition colDef7 = new ColumnDefinition();
                    ColumnDefinition colDef8 = new ColumnDefinition();
                    ColumnDefinition colDef9 = new ColumnDefinition();

                    secondGrid.ColumnDefinitions.Add(colDef1);
                    secondGrid.ColumnDefinitions.Add(colDef2);
                    secondGrid.ColumnDefinitions.Add(colDef3);
                    secondGrid.ColumnDefinitions.Add(colDef4);
                    secondGrid.ColumnDefinitions.Add(colDef5);
                    secondGrid.ColumnDefinitions.Add(colDef6);
                    secondGrid.ColumnDefinitions.Add(colDef7);
                    secondGrid.ColumnDefinitions.Add(colDef8);
                    secondGrid.ColumnDefinitions.Add(colDef9);

                    secondGrid.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
                    secondGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                    secondGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                    secondGrid.ColumnDefinitions[3].Width = new GridLength(4, GridUnitType.Star);
                    secondGrid.ColumnDefinitions[4].Width = new GridLength(3, GridUnitType.Star);
                    secondGrid.ColumnDefinitions[5].Width = new GridLength(3, GridUnitType.Star);
                    secondGrid.ColumnDefinitions[6].Width = new GridLength(2, GridUnitType.Star);
                    secondGrid.ColumnDefinitions[7].Width = new GridLength(1, GridUnitType.Star);
                    secondGrid.ColumnDefinitions[8].Width = new GridLength(2, GridUnitType.Star);

                    Grid.SetColumn(SOV_ComboBox, 0);
                    Grid.SetColumn(CO_LB, 1);
                    Grid.SetColumn(MatOnly_CheckBox, 2);
                    Grid.SetColumn(Material_ComboBox, 3);
                    Grid.SetColumn(Phase_TextBox, 4);
                    Grid.SetColumn(Type_TextBox, 5);
                    Grid.SetColumn(Color_TextBox, 6);
                    Grid.SetColumn(QtyReqd_TextBox, 7);
                    Grid.SetColumn(TotalCost_TextBox, 8);

                    secondGrid.Children.Add(SOV_ComboBox);
                    secondGrid.Children.Add(CO_LB);
                    secondGrid.Children.Add(MatOnly_CheckBox);
                    secondGrid.Children.Add(Material_ComboBox);
                    secondGrid.Children.Add(Phase_TextBox);
                    secondGrid.Children.Add(Type_TextBox);
                    secondGrid.Children.Add(Color_TextBox);
                    secondGrid.Children.Add(QtyReqd_TextBox);
                    secondGrid.Children.Add(TotalCost_TextBox);

                    Grid.SetRow(secondGrid, rowIndex);

                    rowIndex += 1;
                }

                // Installation Notes
                sqlquery = "SELECT * FROM tblInstallNotes WHERE Project_ID = " + selectedProjectID.ToString();

                cmd = new SqlCommand(sqlquery, dbConnection.Connection);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                rowCount = ds.Tables[0].Rows.Count; // number of rows
                rowIndex = 0;

                PreInstallGrid.Children.Clear();
                sb_installationNote.Clear();

                for (int i = 0; i < rowCount; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                    PreInstallGrid.RowDefinitions.Add(rowDef);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int id = row.Field<int>("InstallNotes_ID");
                    int projectID = row.Field<int>("Project_ID");
                    string installNote = row["Install_Note"].ToString();
                    DateTime installDateAdded = row.Field<DateTime>("InstallNotes_DateAdded");

                    sb_installationNote.Add(new InstallationNote
                    {
                        ID = id,
                        ProjectID = projectID,
                        InstallNote = installNote,
                        InstallDateAdded = installDateAdded
                    });
                }

                foreach (InstallationNote note in sb_installationNote)
                {
                    Grid secondGrid = new Grid();

                    for (int i = 0; i < 2; i++)
                    {
                        RowDefinition rowDef = new RowDefinition();
                        rowDef.Height = GridLength.Auto;
                        secondGrid.RowDefinitions.Add(rowDef);
                    }
                    Label _dateAdded = new Label();
                    _dateAdded.HorizontalAlignment = HorizontalAlignment.Right;
                    TextBox _installNote = new TextBox();
                    _installNote.Height = 60;
                    _installNote.AcceptsReturn = true;

                    _dateAdded.Content = note.InstallDateAdded;
                    _installNote.Text = note.InstallNote;

                    Grid.SetRow(_dateAdded, 0);
                    Grid.SetRow(_installNote, 1);

                    secondGrid.Children.Add(_dateAdded);
                    secondGrid.Children.Add(_installNote);

                    Grid.SetRow(secondGrid, rowIndex);

                    PreInstallGrid.Children.Add(secondGrid);
                    rowIndex += 1;
                }

                // Contracts
                sqlquery = "SELECT tblProj.Job_No, Contractnumber, ChangeOrder, ChangeOrderNo, DateRecD, DateProcessed, AmtOfcontract, SignedoffbySales, Signedoffbyoperations, GivenAcctingforreview, Givenforfinalsignature, Scope, ReturnedVia, ReturnedtoDawn, Comments FROM tblSC RIGHT JOIN (SELECT Project_ID, Job_No FROM tblProjects WHERE Project_ID = " + selectedProjectID.ToString() + ") AS tblProj ON tblSC.ProjectID = tblProj.Project_ID";

                cmd = new SqlCommand(sqlquery, dbConnection.Connection);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                rowCount = ds.Tables[0].Rows.Count; // number of rows
                rowIndex = 0;

                ContractGrid.Children.Clear();
                sb_contract.Clear();

                for (int i = 0; i < rowCount; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                    ContractGrid.RowDefinitions.Add(rowDef);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string _jobNo = "";
                    string _contractNumber = "";
                    bool _changeOrder = false;
                    string _changeOrderNo = "";
                    DateTime _dateRecd = new DateTime();
                    int _amtOfContract = 0;
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
                    if (!row.IsNull("Contractnumber"))
                        _changeOrder = row.Field<Boolean>("ChangeOrder"); ;
                    if (!row.IsNull("Contractnumber"))
                        _changeOrderNo = row["ChangeOrderNo"].ToString();
                    if (!row.IsNull("DateRecD"))
                        _dateRecd = row.Field<DateTime>("DateRecD");
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

                foreach (Contract contract in sb_contract)
                {
                    Grid firstGrid = new Grid();
                    Grid secondGrid = new Grid();
                    Grid thirdGrid = new Grid();

                    for (int i = 0; i < 5; i++)
                    {
                        RowDefinition rowDef1 = new RowDefinition();
                        RowDefinition rowDef2 = new RowDefinition();
                        rowDef1.Height = GridLength.Auto;
                        rowDef2.Height = GridLength.Auto;
                        firstGrid.RowDefinitions.Add(rowDef1);
                        secondGrid.RowDefinitions.Add(rowDef2);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        RowDefinition rowDef = new RowDefinition();
                        rowDef.Height = GridLength.Auto;
                        thirdGrid.RowDefinitions.Add(rowDef);
                    }

                    ColumnDefinition colDef1 = new ColumnDefinition();
                    ColumnDefinition colDef2 = new ColumnDefinition();
                    colDef1.Width = new GridLength(3, GridUnitType.Star);
                    colDef2.Width = new GridLength(2, GridUnitType.Star);

                    ColumnDefinition colDef3 = new ColumnDefinition();
                    ColumnDefinition colDef4 = new ColumnDefinition();
                    colDef3.Width = new GridLength(3, GridUnitType.Star);
                    colDef4.Width = new GridLength(2, GridUnitType.Star);

                    ColumnDefinition colDef5 = new ColumnDefinition();
                    ColumnDefinition colDef6 = new ColumnDefinition();
                    colDef5.Width = new GridLength(2, GridUnitType.Star);
                    colDef6.Width = new GridLength(3, GridUnitType.Star);

                    firstGrid.ColumnDefinitions.Add(colDef1);
                    firstGrid.ColumnDefinitions.Add(colDef2);

                    secondGrid.ColumnDefinitions.Add(colDef3);
                    secondGrid.ColumnDefinitions.Add(colDef4);

                    thirdGrid.ColumnDefinitions.Add(colDef5);
                    thirdGrid.ColumnDefinitions.Add(colDef6);

                    // first column
                    Label Gap_LB = new Label();
                    Label Contract_LB = new Label();
                    Label CO_LB = new Label();
                    Label CONo_Label = new Label();
                    Label RecdDate_LB = new Label();
                    TextBox Gap_TB = new TextBox();
                    TextBox Contract_TB = new TextBox();
                    CheckBox CO_CB = new CheckBox();
                    TextBox CONo_TB = new TextBox();
                    DatePicker RecdDate_DP = new DatePicker();

                    Gap_LB.Content = "GAP Job #";
                    Contract_LB.Content = "Contract #";
                    CO_LB.Content = "Change Order";
                    CONo_Label.Content = "Change Order #";
                    RecdDate_LB.Content = "Recd Date";

                    Gap_TB.Text = contract.JobNo;
                    Contract_TB.Text = contract.ContractNumber;
                    CO_CB.IsChecked = contract.ChangeOrder;
                    CONo_TB.Text = contract.ChangeOrderNo;
                    RecdDate_DP.Text = contract.DateRecd.ToString();

                    // second column
                    Label ProcessedDate_LB = new Label();
                    Label ContractAmt_LB = new Label();
                    Label SignedOffBySales_LB = new Label();
                    Label GivenAcctingForReview_LB = new Label();
                    Label GivenForFinalSignature_LB = new Label();
                    DatePicker ProcessedDate_TB = new DatePicker();
                    TextBox ContractAmt_TB = new TextBox();
                    DatePicker SignedOffBySales_DP = new DatePicker();
                    DatePicker GivenAcctingForReview_DP = new DatePicker();
                    DatePicker GivenForFinalSignature_DP = new DatePicker();

                    ProcessedDate_LB.Content = "Processed Date";
                    ContractAmt_LB.Content = "Contract Amt";
                    SignedOffBySales_LB.Content = "Signed off by Sales";
                    GivenAcctingForReview_LB.Content = "Given Accting for Review";
                    GivenForFinalSignature_LB.Content = "Given For Final Signature";

                    ProcessedDate_TB.Text = contract.DateProcessed.ToString();
                    ContractAmt_TB.Text = contract.AmtOfContract.ToString();
                    SignedOffBySales_DP.Text = contract.Signedoffbyoperations.ToString();
                    GivenAcctingForReview_DP.Text = contract.GivenAcctingforreview.ToString();
                    GivenForFinalSignature_DP.Text = contract.Givenforfinalsignature.ToString();

                    // third column
                    Label Scope_LB = new Label();
                    Label ReturnedVia_LB = new Label();
                    Label ReturnedDate_LB = new Label();
                    Label Notes_LB = new Label();
                    TextBox Scope_TB = new TextBox();
                    ComboBox ReturnedVia_CB = new ComboBox();
                    DatePicker ReturnedDate_DP = new DatePicker();
                    TextBox Notes_TB = new TextBox();
                    Scope_TB.AcceptsReturn = true;
                    Scope_TB.Height = 38;
                    Scope_TB.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    Notes_TB.AcceptsReturn = true;
                    Notes_TB.Height = 39;
                    Notes_TB.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    ReturnedVia_CB.ItemsSource = ProjectVM.ReturnedViaNames;
                    ReturnedVia_CB.DisplayMemberPath = "ReturnedViaName";
                    ReturnedVia_CB.SelectedValuePath = "ReturnedViaName";


                    Scope_LB.Content = "Scope";
                    ReturnedVia_LB.Content = "Returned Via";
                    ReturnedDate_LB.Content = "Returned Date";
                    Notes_LB.Content = "Notes";

                    Scope_TB.Text = contract.Scope;
                    ReturnedVia_CB.SelectedValue = contract.ReturnedVia;
                    ReturnedDate_DP.Text = contract.ReturnedDate.ToString();
                    Notes_TB.Text = contract.Comment.ToString();

                    // Grid set for thirdGrid
                    Grid.SetColumn(Scope_LB, 0);
                    Grid.SetColumn(ReturnedVia_LB, 0);
                    Grid.SetColumn(ReturnedDate_LB, 0);
                    Grid.SetColumn(Notes_LB, 0);

                    Grid.SetColumn(Scope_TB, 1);
                    Grid.SetColumn(ReturnedVia_CB, 1);
                    Grid.SetColumn(ReturnedDate_DP, 1);
                    Grid.SetColumn(Notes_TB, 1);

                    thirdGrid.Children.Add(Scope_LB);
                    thirdGrid.Children.Add(Scope_TB);
                    thirdGrid.Children.Add(ReturnedVia_LB);
                    thirdGrid.Children.Add(ReturnedVia_CB);
                    thirdGrid.Children.Add(ReturnedDate_LB);
                    thirdGrid.Children.Add(ReturnedDate_DP);
                    thirdGrid.Children.Add(Notes_LB);
                    thirdGrid.Children.Add(Notes_TB);

                    Grid.SetRow(Scope_LB, 0);
                    Grid.SetRow(Scope_TB, 0);
                    Grid.SetRow(ReturnedVia_LB, 1);
                    Grid.SetRow(ReturnedVia_CB, 1);
                    Grid.SetRow(ReturnedDate_LB, 2);
                    Grid.SetRow(ReturnedDate_DP, 2);
                    Grid.SetRow(Notes_LB, 3);
                    Grid.SetRow(Notes_TB, 3);

                    // Grid set for secondGrid
                    Grid.SetColumn(ProcessedDate_LB, 0);
                    Grid.SetColumn(ContractAmt_LB, 0);
                    Grid.SetColumn(SignedOffBySales_LB, 0);
                    Grid.SetColumn(GivenAcctingForReview_LB, 0);
                    Grid.SetColumn(GivenForFinalSignature_LB, 0);

                    Grid.SetColumn(ProcessedDate_TB, 1);
                    Grid.SetColumn(ContractAmt_TB, 1);
                    Grid.SetColumn(SignedOffBySales_DP, 1);
                    Grid.SetColumn(GivenAcctingForReview_DP, 1);
                    Grid.SetColumn(GivenForFinalSignature_DP, 1);

                    Grid.SetRow(ProcessedDate_LB, 0);
                    Grid.SetRow(ProcessedDate_TB, 0);
                    Grid.SetRow(ContractAmt_LB, 1);
                    Grid.SetRow(ContractAmt_TB, 1);
                    Grid.SetRow(SignedOffBySales_LB, 2);
                    Grid.SetRow(SignedOffBySales_DP, 2);
                    Grid.SetRow(GivenAcctingForReview_LB, 3);
                    Grid.SetRow(GivenAcctingForReview_DP, 3);
                    Grid.SetRow(GivenForFinalSignature_LB, 4);
                    Grid.SetRow(GivenForFinalSignature_DP, 4);

                    secondGrid.Children.Add(ProcessedDate_LB);
                    secondGrid.Children.Add(ProcessedDate_TB);
                    secondGrid.Children.Add(ContractAmt_LB);
                    secondGrid.Children.Add(ContractAmt_TB);
                    secondGrid.Children.Add(SignedOffBySales_LB);
                    secondGrid.Children.Add(SignedOffBySales_DP);
                    secondGrid.Children.Add(GivenAcctingForReview_LB);
                    secondGrid.Children.Add(GivenAcctingForReview_DP);
                    secondGrid.Children.Add(GivenForFinalSignature_LB);
                    secondGrid.Children.Add(GivenForFinalSignature_DP);

                    // Grid set for firstGrid
                    Grid.SetColumn(Gap_LB, 0);
                    Grid.SetColumn(Contract_LB, 0);
                    Grid.SetColumn(CO_LB, 0);
                    Grid.SetColumn(CONo_Label, 0);
                    Grid.SetColumn(RecdDate_LB, 0);

                    Grid.SetColumn(Gap_TB, 1);
                    Grid.SetColumn(Contract_TB, 1);
                    Grid.SetColumn(CO_CB, 1);
                    Grid.SetColumn(CONo_TB, 1);
                    Grid.SetColumn(RecdDate_DP, 1);

                    Grid.SetRow(Gap_LB, 0);
                    Grid.SetRow(Gap_TB, 0);
                    Grid.SetRow(Contract_LB, 1);
                    Grid.SetRow(Contract_TB, 1);
                    Grid.SetRow(CO_LB, 2);
                    Grid.SetRow(CO_CB, 2);
                    Grid.SetRow(CONo_Label, 3);
                    Grid.SetRow(CONo_TB, 3);
                    Grid.SetRow(RecdDate_LB, 4);
                    Grid.SetRow(RecdDate_DP, 4);

                    firstGrid.Children.Add(Gap_LB);
                    firstGrid.Children.Add(Gap_TB);
                    firstGrid.Children.Add(Contract_LB);
                    firstGrid.Children.Add(Contract_TB);
                    firstGrid.Children.Add(CO_LB);
                    firstGrid.Children.Add(CO_CB);
                    firstGrid.Children.Add(CONo_Label);
                    firstGrid.Children.Add(CONo_TB);
                    firstGrid.Children.Add(RecdDate_LB);
                    firstGrid.Children.Add(RecdDate_DP);

                    Grid.SetRow(firstGrid, rowIndex);
                    Grid.SetColumn(firstGrid, 0);

                    Grid.SetRow(secondGrid, rowIndex);
                    Grid.SetColumn(secondGrid, 1);

                    Grid.SetRow(thirdGrid, rowIndex);
                    Grid.SetColumn(thirdGrid, 2);

                    if (rowIndex % 2 != 0)
                    {
                        firstGrid.Background = new SolidColorBrush(Colors.Gainsboro);
                        secondGrid.Background = new SolidColorBrush(Colors.Gainsboro);
                        thirdGrid.Background = new SolidColorBrush(Colors.Gainsboro);
                    }
                    ContractGrid.Children.Add(firstGrid);
                    ContractGrid.Children.Add(secondGrid);
                    ContractGrid.Children.Add(thirdGrid);

                    Grid.SetRow(secondGrid, rowIndex);
                    rowIndex += 1;
                }
                // CIPGrid
                sqlquery = "SELECT Job_No, CIPType, TargetDate, OriginalContractAmt, FinalContractAmt, FormsRecD, FormsSent, CertRecD, ExemptionApproved, ExemptionAppDate, CrewEnrolled, Notes FROM tblCIPs RIGHT JOIN (SELECT Project_ID, Job_No FROM tblProjects WHERE Project_ID = " + selectedProjectID.ToString() + ") AS tblProjs ON tblCIPs.Project_ID = tblProjs.Project_ID";

                cmd = new SqlCommand(sqlquery, dbConnection.Connection);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                rowCount = ds.Tables[0].Rows.Count; // number of rows
                rowIndex = 0;

                CIPGrid.Children.Clear();
                sb_cip.Clear();

                for (int i = 0; i < rowCount; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                    CIPGrid.RowDefinitions.Add(rowDef);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string _jobNo = "";
                    string _cipType = "";
                    DateTime _targetDate = new DateTime();
                    double _originalContractAmt = 0.0;
                    double _finalContractAmt = 0.0;
                    DateTime _formsRecD = new DateTime();
                    DateTime _formsSent = new DateTime();
                    DateTime _certRecD = new DateTime();
                    bool _exemptionApproved = false;
                    DateTime _exemptionAppDate = new DateTime();
                    string _crewEnrolled = "";
                    string _notes = "";

                    if (!row.IsNull("Job_No"))
                        _jobNo = row["Job_No"].ToString();
                    if (!row.IsNull("CIPType"))
                        _cipType = row["CIPType"].ToString();
                    if (!row.IsNull("TargetDate"))
                        _targetDate = row.Field<DateTime>("TargetDate"); ;
                    if (!row.IsNull("OriginalContractAmt"))
                        _originalContractAmt = double.Parse(row["OriginalContractAmt"].ToString());
                    if (!row.IsNull("FinalContractAmt"))
                        _finalContractAmt = double.Parse(row["FinalContractAmt"].ToString());
                    if (!row.IsNull("FormsRecD"))
                        _formsRecD = row.Field<DateTime>("FormsRecD");
                    if (!row.IsNull("FormsSent"))
                        _formsSent = row.Field<DateTime>("FormsSent");
                    if (!row.IsNull("ExemptionApproved"))
                        _exemptionApproved = row.Field<Boolean>("ExemptionApproved");
                    if (!row.IsNull("ExemptionAppDate"))
                        _exemptionAppDate = row.Field<DateTime>("ExemptionAppDate");
                    if (!row.IsNull("CrewEnrolled"))
                        _crewEnrolled = row["CrewEnrolled"].ToString();
                    if (!row.IsNull("Notes"))
                        _notes = row["Notes"].ToString();
                    if (!row.IsNull("CertRecD"))
                        _certRecD = row.Field<DateTime>("CertRecD");

                    sb_cip.Add(new CIP
                    {
                        JobNo = _jobNo,
                        CipType = _cipType,
                        TargetDate = _targetDate,
                        OriginalContractAmt = _originalContractAmt,
                        FinalContractAmt = _finalContractAmt,
                        FormsRecD = _formsRecD,
                        FormsSent = _formsSent,
                        ExemptionApproved = _exemptionApproved,
                        ExemptionAppDate = _exemptionAppDate,
                        CrewEnrolled = _crewEnrolled,
                        CertRecD = _certRecD,
                        Notes = _notes
                    });
                }

                foreach (CIP cip in sb_cip)
                {

                    Grid firstGrid = new Grid();
                    Grid secondGrid = new Grid();
                    Grid thirdGrid = new Grid();
                    Grid exemptionGrid = new Grid();

                    for (int i = 0; i < 5; i++)
                    {
                        RowDefinition rowDef1 = new RowDefinition();
                        RowDefinition rowDef2 = new RowDefinition();
                        rowDef1.Height = GridLength.Auto;
                        rowDef2.Height = GridLength.Auto;
                        firstGrid.RowDefinitions.Add(rowDef1);
                        secondGrid.RowDefinitions.Add(rowDef2);
                    }

                    RowDefinition rowDef = new RowDefinition();
                    thirdGrid.RowDefinitions.Add(rowDef);

                    ColumnDefinition colDef1 = new ColumnDefinition();
                    ColumnDefinition colDef2 = new ColumnDefinition();
                    colDef1.Width = new GridLength(3, GridUnitType.Star);
                    colDef2.Width = new GridLength(2, GridUnitType.Star);

                    ColumnDefinition colDef3 = new ColumnDefinition();
                    ColumnDefinition colDef4 = new ColumnDefinition();
                    colDef3.Width = new GridLength(3, GridUnitType.Star);
                    colDef4.Width = new GridLength(2, GridUnitType.Star);

                    ColumnDefinition colDef5 = new ColumnDefinition();
                    ColumnDefinition colDef6 = new ColumnDefinition();
                    colDef5.Width = new GridLength(1, GridUnitType.Star);
                    colDef6.Width = new GridLength(5, GridUnitType.Star);

                    ColumnDefinition colDef7 = new ColumnDefinition();
                    ColumnDefinition colDef8 = new ColumnDefinition();
                    colDef7.Width = new GridLength(1, GridUnitType.Star);
                    colDef8.Width = new GridLength(4, GridUnitType.Star);

                    firstGrid.ColumnDefinitions.Add(colDef1);
                    firstGrid.ColumnDefinitions.Add(colDef2);

                    secondGrid.ColumnDefinitions.Add(colDef3);
                    secondGrid.ColumnDefinitions.Add(colDef4);

                    thirdGrid.ColumnDefinitions.Add(colDef5);
                    thirdGrid.ColumnDefinitions.Add(colDef6);

                    exemptionGrid.ColumnDefinitions.Add(colDef7);
                    exemptionGrid.ColumnDefinitions.Add(colDef8);

                    // first column
                    Label Gap_LB = new Label();
                    Label CIPType_LB = new Label();
                    Label TargetDate_LB = new Label();
                    Label OrgContAmt_LB = new Label();
                    Label FinalContAmt_LB = new Label();
                    TextBox Gap_TB = new TextBox();
                    ComboBox CIPType_CB = new ComboBox();
                    DatePicker TargetDate_DP = new DatePicker();
                    TextBox OrgContAmt_TB = new TextBox();
                    TextBox FinalContAmt_TB = new TextBox();

                    Gap_LB.Content = "GAP Job #";
                    CIPType_LB.Content = "CIP Type";
                    TargetDate_LB.Content = "Target Date";
                    OrgContAmt_LB.Content = "Original Contract Amount";
                    FinalContAmt_LB.Content = "Final Contract Amount";

                    Gap_TB.Text = cip.JobNo;
                    CIPType_CB.Text = cip.CipType;
                    TargetDate_DP.Text = cip.TargetDate.ToString();
                    OrgContAmt_TB.Text = cip.OriginalContractAmt.ToString();
                    FinalContAmt_TB.Text = cip.FinalContractAmt.ToString();

                    // second column
                    Label FormsRecvd_LB = new Label();
                    Label FormsSent_LB = new Label();
                    Label CertRecvd_LB = new Label();
                    Label ExamAppr_LB = new Label();
                    Label CrewEnrol_LB = new Label();
                    DatePicker FormsRecvd_DP = new DatePicker();
                    DatePicker FormsSent_DP = new DatePicker();
                    DatePicker CertRecvd_DP = new DatePicker();
                    CheckBox ExemAppr1_CH = new CheckBox();
                    DatePicker ExemAppr2_DP = new DatePicker();
                    ComboBox CrewEnrol_CB = new ComboBox();

                    FormsRecvd_LB.Content = "Forms Received";
                    FormsSent_LB.Content = "Forms Sent";
                    CertRecvd_LB.Content = "Certificate Received";
                    ExamAppr_LB.Content = "Exemption Approved";
                    CrewEnrol_LB.Content = "Crew Enrolled";

                    FormsRecvd_DP.Text = cip.FormsRecD.ToString();
                    FormsSent_DP.Text = cip.FormsSent.ToString();
                    CertRecvd_DP.Text = cip.CertRecD.ToString();
                    ExemAppr1_CH.IsChecked = cip.ExemptionApproved;
                    ExemAppr2_DP.Text = cip.ExemptionAppDate.ToString();
                    CrewEnrol_CB.ItemsSource = ProjectVM.Crews;
                    CrewEnrol_CB.DisplayMemberPath = "CrewName";
                    CrewEnrol_CB.SelectedValuePath = "CrewName";
                    CrewEnrol_CB.SelectedValue = cip.CrewEnrolled;

                    // exemption Grid
                    Grid.SetColumn(ExemAppr1_CH, 0);
                    Grid.SetColumn(ExemAppr2_DP, 1);
                    exemptionGrid.Children.Add(ExemAppr1_CH);
                    exemptionGrid.Children.Add(ExemAppr2_DP);

                    // third column
                    Label Notes_LB = new Label();
                    TextBox Notes_TB = new TextBox();
                    Notes_LB.Content = "Notes";
                    Notes_TB.Text = cip.Notes;

                    // Set Grid for third Column
                    Grid.SetColumn(Notes_LB, 0);
                    Grid.SetColumn(Notes_TB, 1);
                    thirdGrid.Children.Add(Notes_LB);
                    thirdGrid.Children.Add(Notes_TB);

                    // Set Grid for second Column
                    Grid.SetColumn(FormsRecvd_LB, 0);
                    Grid.SetColumn(FormsSent_LB, 0);
                    Grid.SetColumn(CertRecvd_LB, 0);
                    Grid.SetColumn(ExamAppr_LB, 0);
                    Grid.SetColumn(CrewEnrol_LB, 0);

                    Grid.SetColumn(FormsRecvd_DP, 1);
                    Grid.SetColumn(FormsSent_DP, 1);
                    Grid.SetColumn(CertRecvd_DP, 1);
                    Grid.SetColumn(exemptionGrid, 1);
                    Grid.SetColumn(CrewEnrol_CB, 1);

                    Grid.SetRow(FormsRecvd_LB, 0);
                    Grid.SetRow(FormsSent_LB, 1);
                    Grid.SetRow(CertRecvd_LB, 2);
                    Grid.SetRow(ExamAppr_LB, 3);
                    Grid.SetRow(CrewEnrol_LB, 4);

                    Grid.SetRow(FormsRecvd_DP, 0);
                    Grid.SetRow(FormsSent_DP, 1);
                    Grid.SetRow(CertRecvd_DP, 2);
                    Grid.SetRow(exemptionGrid, 3);
                    Grid.SetRow(CrewEnrol_CB, 4);

                    secondGrid.Children.Add(FormsRecvd_LB);
                    secondGrid.Children.Add(FormsSent_LB);
                    secondGrid.Children.Add(CertRecvd_LB);
                    secondGrid.Children.Add(ExamAppr_LB);
                    secondGrid.Children.Add(CrewEnrol_LB);
                    secondGrid.Children.Add(FormsRecvd_DP);
                    secondGrid.Children.Add(FormsSent_DP);
                    secondGrid.Children.Add(CertRecvd_DP);
                    secondGrid.Children.Add(exemptionGrid);
                    secondGrid.Children.Add(CrewEnrol_CB);

                    // Set Grid for first Column
                    Grid.SetColumn(Gap_LB, 0);
                    Grid.SetColumn(CIPType_LB, 0);
                    Grid.SetColumn(TargetDate_LB, 0);
                    Grid.SetColumn(OrgContAmt_LB, 0);
                    Grid.SetColumn(FinalContAmt_LB, 0);

                    Grid.SetColumn(Gap_TB, 1);
                    Grid.SetColumn(CIPType_CB, 1);
                    Grid.SetColumn(TargetDate_DP, 1);
                    Grid.SetColumn(OrgContAmt_TB, 1);
                    Grid.SetColumn(FinalContAmt_TB, 1);

                    Grid.SetRow(Gap_LB, 0);
                    Grid.SetRow(CIPType_LB, 1);
                    Grid.SetRow(TargetDate_LB, 2);
                    Grid.SetRow(OrgContAmt_LB, 3);
                    Grid.SetRow(FinalContAmt_LB, 4);

                    Grid.SetRow(Gap_TB, 0);
                    Grid.SetRow(CIPType_CB, 1);
                    Grid.SetRow(TargetDate_DP, 2);
                    Grid.SetRow(OrgContAmt_TB, 3);
                    Grid.SetRow(FinalContAmt_TB, 4);

                    firstGrid.Children.Add(Gap_LB);
                    firstGrid.Children.Add(CIPType_LB);
                    firstGrid.Children.Add(TargetDate_LB);
                    firstGrid.Children.Add(OrgContAmt_LB);
                    firstGrid.Children.Add(FinalContAmt_LB);
                    firstGrid.Children.Add(Gap_TB);
                    firstGrid.Children.Add(CIPType_CB);
                    firstGrid.Children.Add(TargetDate_DP);
                    firstGrid.Children.Add(OrgContAmt_TB);
                    firstGrid.Children.Add(FinalContAmt_TB);

                    Grid.SetRow(firstGrid, rowIndex);
                    Grid.SetRow(secondGrid, rowIndex);
                    Grid.SetColumn(secondGrid, 1);
                    Grid.SetRow(thirdGrid, rowIndex);
                    Grid.SetColumn(thirdGrid, 2);

                    CIPGrid.Children.Add(firstGrid);
                    CIPGrid.Children.Add(secondGrid);
                    CIPGrid.Children.Add(thirdGrid);

                    rowIndex += 1;
                }
            }
            else
            {
                PMGrid.Children.Clear();
                SuptGrid.Children.Clear();
                ProjectNoteGrid.Children.Clear();
                PreInstallGrid.Children.Clear();
                ContractGrid.Children.Clear();
                CIPGrid.Children.Clear();
                ProjectWorkOrderList.SelectedIndex = -1;
                WorkOrderNoteGrid.Children.Clear();

                ProjectName_TB.Clear();
                CustomerName_CB.SelectedIndex = -1;
                TargetDate_DP.SelectedDate = new DateTime();
                Complete_CH.IsChecked = false;
                Complete_DP.SelectedDate = new DateTime();
                Payment_CB.SelectedIndex = -1;
                Payment_CB.Text = "select";
                PaymentNote_TB.Clear();
                JobNo_TB.Clear();
                Hold_CH.IsChecked = false;

                MasterContract_CB.SelectedIndex = -1;
                Address_TB.Clear();
                City_TB.Clear();
                State_TB.Clear();
                Zip_TB.Clear();


                Estimator_CB.SelectedIndex = -1;
                ProjectCoord_CB.SelectedIndex = -1;
                ArchRep_CB.SelectedIndex = -1;
                SubContact_CB.SelectedIndex = -1;
                Architect_CB.SelectedIndex = -1;

                OrigAmt_LB.Content = "";
                OrigProfit_LB.Content = "";
                OrigTotalAmt_LB.Content = "";
            }
        }

        private void SelectPaymentComboBoxItem(object sender, MouseButtonEventArgs e)
        {
            ComboBoxItem item = sender as ComboBoxItem;
            Payment Payment = new Payment();
            Payment dataObject = item.DataContext as Payment;
            dataObject.IsChecked = !dataObject.IsChecked;
            
            e.Handled = true;
        }

        private void SelectPaymentCheckBox(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void SelectWorkOrdersList(object sender, SelectionChangedEventArgs e)
        {
            if (WOListView.SelectedItem != null)
            {
                WorkOrder selectedItem = (WorkOrder)WOListView.SelectedItem;
                int woID = selectedItem.WoID;

                int rowCount = 0;
                int rowIndex = 0;
                sqlquery = "SELECT * FROM tblNotes WHERE Notes_PK = " + woID + " AND Notes_PK_Desc = 'WorkOrder';";
                cmd = new SqlCommand(sqlquery, dbConnection.Connection);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                rowCount = ds.Tables[0].Rows.Count; // number of rows

                for (int i = 0; i < rowCount; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                    WorkOrderNoteGrid.RowDefinitions.Add(rowDef);
                }

                WorkOrderNoteGrid.Children.Clear();
                sb_notes.Clear();

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

                    sb_notes.Add(new Note
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

                foreach (Note note in sb_notes)
                {
                    Grid firstGrid = new Grid();
                    Grid secondGrid = new Grid();
                    for (int i = 0; i < 2; i++)
                    {
                        RowDefinition rowDef = new RowDefinition();
                        rowDef.Height = GridLength.Auto;
                        secondGrid.RowDefinitions.Add(rowDef);
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        ColumnDefinition colDef = new ColumnDefinition();
                        firstGrid.ColumnDefinitions.Add(colDef);
                    }

                    firstGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                    firstGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                    Label DateAdded_Label = new Label();
                    Label User_Label = new Label();
                    User_Label.HorizontalAlignment = HorizontalAlignment.Right;
                    TextBox Note_TextBox = new TextBox();

                    DateAdded_Label.Content = note.NotesDateAdded.ToString();
                    DateAdded_Label.HorizontalAlignment = HorizontalAlignment.Left;
                    User_Label.Content = note.NoteUser.ToString();

                    Grid.SetColumn(DateAdded_Label, 0);
                    Grid.SetColumn(User_Label, 1);

                    firstGrid.Children.Add(DateAdded_Label);
                    firstGrid.Children.Add(User_Label);

                    Note_TextBox.Text = note.NotesNote;
                    Note_TextBox.Height = 40;
                    Note_TextBox.AcceptsReturn = true;
                    Note_TextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

                    Grid.SetRow(firstGrid, 0);
                    Grid.SetRow(Note_TextBox, 1);

                    secondGrid.Children.Add(firstGrid);
                    secondGrid.Children.Add(Note_TextBox);

                    Grid.SetRow(secondGrid, rowIndex);

                    WorkOrderNoteGrid.Margin = new Thickness(10, 2, 10, 2);
                    WorkOrderNoteGrid.Children.Add(secondGrid);
                    rowIndex += 1;
                }
            }
        }
    }
}
