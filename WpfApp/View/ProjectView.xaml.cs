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

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for ViewProject.xaml
    /// </summary>
    public partial class ProjectView : Page
    {
        private string sqlquery;
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataAdapter sda;
        private DataSet ds;
        private ObservableCollection<ProjectManager> st_mb;
        private ObservableCollection<Acronym> st_acronym;
        private ObservableCollection<int> st_projectOrder;
        private ObservableCollection<Material> st_material;
        private ObservableCollection<SovCO> st_SovCO;
        private ObservableCollection<TrackShipRecv> st_TrackShipRecv;
        private ObservableCollection<ProjectMatTracking> sb_projectMatTrackings;
        private ObservableCollection<ProjectMatShip> sb_projectMtShip;

        private ObservableCollection<Manufacturer> sb_manufacturers;
        private ObservableCollection<FreightCo> sb_freightCo;
        public ProjectView()
        {
            InitializeComponent();
            this.DataContext = new ProjectViewModel();
            string connectionString = @"Data Source = DESKTOP-VDIB57T\INSTANCE2023; user id=sa; password=qwe234ASD@#$; Initial Catalog = griesenbeck;";
            con = new SqlConnection(connectionString);
            con.Open();
            st_mb = new ObservableCollection<ProjectManager>();
            st_acronym = new ObservableCollection<Acronym>();
            st_projectOrder = new ObservableCollection<int>();
            st_material = new ObservableCollection<Material>();
            st_SovCO = new ObservableCollection<SovCO>();
            st_TrackShipRecv = new ObservableCollection<TrackShipRecv>();
            sb_projectMatTrackings = new ObservableCollection<ProjectMatTracking>();
            sb_manufacturers = new ObservableCollection<Manufacturer>();
            sb_projectMtShip = new ObservableCollection<ProjectMatShip>();
            sb_freightCo = new ObservableCollection<FreightCo>();
            loadProject();
        }

        public void loadProject()
        {
            // Manufacturer
            sqlquery = "SELECT Manuf_ID, Manuf_Name FROM tblManufacturers;";
            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int manufID = int.Parse(row["Manuf_ID"].ToString());
                string manufName = row["Manuf_Name"].ToString();
                sb_manufacturers.Add(new Manufacturer
                {
                    ID = manufID,
                    ManufacturerName = manufName,
                });
            }

            // FreightCo_Name
            sqlquery = "SELECT FreightCo_ID, FreightCo_Name FROM tblFreightCo ORDER BY FreightCo_Name;";
            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int freightID = int.Parse(row["FreightCo_ID"].ToString());
                string freightName = row["FreightCo_Name"].ToString();
                sb_freightCo.Add(new FreightCo
                {
                    FreightCoID = freightID,
                    FreightName = freightName,
                });
            }

            // Materials
            sqlquery = "Select * from tblMaterials";
            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int matID = int.Parse(row["Material_ID"].ToString());
                string matDesc = row["Material_Desc"].ToString();
                st_material.Add(new Material
                {
                    ID = matID,
                    MatDesc = matDesc,
                });
            }
        }

        private void goback(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }

        private void SelectProjectItem(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Project selectedItem = (Project)comboBox.SelectedItem;
            if (!comboBox.Items.Contains(comboBox.Text))
            {
                foreach(var item in comboBox.Items)
                {
                    //Console.WriteLine(item.);
                }
            }

            //try
            //{
                int selectedProjectID = selectedItem.ID;

                sqlquery = "SELECT PM_ID, PM_Name, PM_CellPhone, PM_Email from tblProjectManagers";
                cmd = new SqlCommand(sqlquery, con);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                
                st_mb.Clear();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int pmID = int.Parse(row["PM_ID"].ToString());
                    string pmName = row["PM_Name"].ToString();
                    string pmCellPhone = row["PM_CellPhone"].ToString();
                    string pmEmail = row["PM_Email"].ToString();
                    st_mb.Add(new ProjectManager
                    {
                        ID = pmID,
                        PMName = pmName,
                        PMCellPhone = pmCellPhone,
                        PMEmail = pmEmail
                    });
                }

                // data for Acronym ComboBox
                sqlquery = "SELECT * from tblScheduleOfValues";
                cmd = new SqlCommand(sqlquery, con);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
               
                st_acronym.Clear();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    //int projectID = int.Parse(row["Project_ID"].ToString());
                    string acronymName = row["SOV_Acronym"].ToString();
                    st_acronym.Add(new Acronym
                    {
                        //ProjectID = projectID,
                        AcronymName = acronymName,
                    });
                }

                // data for Change Orders ComboBox
                sqlquery = "select * from tblProjectChangeOrders where Project_ID = " + selectedProjectID.ToString();
                cmd = new SqlCommand(sqlquery, con);
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
                cmd = new SqlCommand(sqlquery, con);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);

                st_SovCO.Clear();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string sovAcronym = row["SOV_Acronym"].ToString();
                    string coItem = row["CO_ItemNo"].ToString();
                    st_SovCO.Add(new SovCO
                    {
                        SovAcronym = sovAcronym,
                        CoItem = coItem,
                    });
                }

                //con.Close();
                //cmd.Dispose();
                int rowCount = 1;
                int rowIndex = 0;

                // Project Grid 1
                sqlquery = "Select * from tblProjectPMs where Project_ID = " + selectedProjectID.ToString();
                cmd = new SqlCommand(sqlquery, con);
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
                    cmd = new SqlCommand(sqlquery, con);
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

                    pmComBoBox.ItemsSource = st_mb;
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

                // SOV Grid 1
                rowIndex = 0;
                sqlquery = "Select tblSOV.*, tblProjectChangeOrders.CO_ItemNo from (Select tblSOV.*, tblScheduleOfValues.SOV_Desc from tblScheduleOfValues Right JOIN (SELECT tblProjectSOV.* From tblProjects LEFT Join tblProjectSOV ON tblProjects.Project_ID = tblProjectSOV.Project_ID where tblProjects.Project_ID = " + selectedProjectID.ToString() + ") AS tblSOV ON tblSOV.SOV_Acronym = tblScheduleOfValues.SOV_Acronym Where tblScheduleOfValues.Active = 'true') AS tblSOV LEFT JOIN tblProjectChangeOrders ON tblProjectChangeOrders.CO_ID = tblSOV.CO_ID ORDER BY tblSOV.SOV_Acronym;";
                cmd = new SqlCommand(sqlquery, con);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                rowCount = ds.Tables[0].Rows.Count; // number of rows

                SOVGrid1.Children.Clear();
                for (int i = 0; i < rowCount; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                    SOVGrid1.RowDefinitions.Add(rowDef);
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ComboBox Acronym_ComboBox = new ComboBox();
                    ComboBox ChangeOrder_ComboBox = new ComboBox();
                    Label SOVItem_Label = new Label();
                    CheckBox Material_CheckBox = new CheckBox();

                    Acronym_ComboBox.ItemsSource = st_acronym;
                    Acronym_ComboBox.SelectedValuePath = "AcronymName";
                    Acronym_ComboBox.DisplayMemberPath = "AcronymName";
                    Acronym_ComboBox.SelectedValue = row["SOV_Acronym"].ToString();
                    Acronym_ComboBox.IsEditable = true;
                    Acronym_ComboBox.Margin = new Thickness(4, 1, 4, 1);

                    ChangeOrder_ComboBox.ItemsSource = st_projectOrder;
                    if(!row.IsNull("CO_ItemNo"))
                        ChangeOrder_ComboBox.SelectedValue = int.Parse(row["CO_ItemNo"].ToString());
                    ChangeOrder_ComboBox.Margin = new Thickness(4,1,4,1);
                    

                    SOVItem_Label.Content = row["SOV_Desc"].ToString();
                    SOVItem_Label.HorizontalAlignment = HorizontalAlignment.Left;
                    SOVItem_Label.Margin = new Thickness(4, 1, 4, 1);
                    Material_CheckBox.IsChecked = row.Field<Boolean>("Material_Only");
                    Material_CheckBox.HorizontalAlignment = HorizontalAlignment.Center;
                    Material_CheckBox.VerticalAlignment = VerticalAlignment.Center;
                    Material_CheckBox.Margin = new Thickness(4, 1, 4, 1);
                    
                    Grid secondGrid = new Grid();
                    ColumnDefinition colDef1 = new ColumnDefinition();
                    ColumnDefinition colDef2 = new ColumnDefinition();
                    ColumnDefinition colDef3 = new ColumnDefinition();
                    ColumnDefinition colDef4 = new ColumnDefinition();
                    secondGrid.ColumnDefinitions.Add(colDef1);
                    secondGrid.ColumnDefinitions.Add(colDef2);
                    secondGrid.ColumnDefinitions.Add(colDef3);
                    secondGrid.ColumnDefinitions.Add(colDef4);

                    secondGrid.ColumnDefinitions[0].Width = new GridLength(3, GridUnitType.Star);
                    secondGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                    secondGrid.ColumnDefinitions[2].Width = new GridLength(4, GridUnitType.Star);
                    secondGrid.ColumnDefinitions[3].Width = new GridLength(2, GridUnitType.Star);

                    Grid.SetColumn(Acronym_ComboBox, 0);
                    Grid.SetColumn(ChangeOrder_ComboBox, 1);
                    Grid.SetColumn(SOVItem_Label, 2);
                    Grid.SetColumn(Material_CheckBox, 3);

                    secondGrid.Children.Add(Acronym_ComboBox);
                    secondGrid.Children.Add(ChangeOrder_ComboBox);
                    secondGrid.Children.Add(SOVItem_Label);
                    secondGrid.Children.Add(Material_CheckBox);

                    Grid.SetRow(secondGrid, rowIndex);

                    SOVGrid1.Children.Add(secondGrid);

                    rowIndex += 1;
                }

                // SOVGrid2
                rowIndex = 0;
                sqlquery = "Select tblSOV.SOV_Acronym, tblSOV.CO_ItemNo, tblSOV.Material_Only, tblSOV.SOV_Desc, tblProjectMaterials.Mat_Phase, tblProjectMaterials.Mat_Type, tblProjectMaterials.Color_Selected, tblProjectMaterials.Qty_Reqd, tblProjectMaterials.TotalCost from(Select tblSOV.*, tblProjectChangeOrders.CO_ItemNo from(Select tblSOV.*, tblScheduleOfValues.SOV_Desc from tblScheduleOfValues Right JOIN(SELECT tblProjectSOV.* From tblProjects LEFT Join tblProjectSOV ON tblProjects.Project_ID = tblProjectSOV.Project_ID where tblProjects.Project_ID = " + selectedProjectID.ToString() + " ) AS tblSOV ON tblSOV.SOV_Acronym = tblScheduleOfValues.SOV_Acronym Where tblScheduleOfValues.Active = 'true') AS tblSOV LEFT JOIN tblProjectChangeOrders ON tblProjectChangeOrders.CO_ID = tblSOV.CO_ID) AS tblSOV LEFT JOIN tblProjectMaterials ON tblSOV.ProjSOV_ID = tblProjectMaterials.ProjSOV_ID ORDER BY tblSOV.SOV_Acronym DESC";
                cmd = new SqlCommand(sqlquery, con);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                rowCount = ds.Tables[0].Rows.Count; // number of rows
                SOVGrid2.Children.Clear();
                for (int i = 0; i < rowCount; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                    SOVGrid2.RowDefinitions.Add(rowDef);
                }
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ComboBox SOV_ComboBox = new ComboBox();
                    Label CO_Label = new Label();
                    CheckBox MatOnly_CheckBox = new CheckBox();
                    ComboBox Material_ComboBox = new ComboBox();
                    TextBox Phase_TextBox = new TextBox();
                    TextBox Type_TextBox = new TextBox();
                    TextBox Color_TextBox = new TextBox();
                    TextBox QtyReqd_TextBox = new TextBox();
                    TextBox TotalCost_TextBox = new TextBox();

                    SOV_ComboBox.Margin = new Thickness(4, 1, 4, 1);
                    CO_Label.Margin = new Thickness(4, 1, 4, 1);
                    MatOnly_CheckBox.Margin = new Thickness(4, 1, 4, 1);
                    Material_ComboBox.Margin = new Thickness(4, 1, 4, 1);
                    Phase_TextBox.Margin = new Thickness(4, 1, 4, 1);
                    Type_TextBox.Margin = new Thickness(4, 1, 4, 1);
                    Color_TextBox.Margin = new Thickness(4, 1, 4, 1);
                    QtyReqd_TextBox.Margin = new Thickness(4, 1, 4, 1);
                    TotalCost_TextBox.Margin = new Thickness(4, 1, 4, 1);

                    CO_Label.HorizontalAlignment = HorizontalAlignment.Center;
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

                    SOV_ComboBox.ItemsSource = st_SovCO;
                    //SOV_ComboBox.ItemTemplate = sovItemTemplate;
                    SOV_ComboBox.DisplayMemberPath = "SovAcronym";
                    SOV_ComboBox.SelectedValuePath = "SovAcronym";
                    SOV_ComboBox.SelectedValue = row["SOV_Acronym"].ToString();
                    SOV_ComboBox.IsEditable = true;
                    SOV_ComboBox.Margin = new Thickness(4, 1, 4, 1);

                    Material_ComboBox.ItemsSource = st_material;
                    Material_ComboBox.SelectedValuePath = "MatDesc";
                    Material_ComboBox.DisplayMemberPath = "MatDesc";
                    Material_ComboBox.SelectedValue = row["SOV_Desc"].ToString();
                    Material_ComboBox.IsEditable = true;
                    Material_ComboBox.Margin = new Thickness(4, 1, 4, 1);

                    if (!row.IsNull("Material_Only"))
                        MatOnly_CheckBox.IsChecked = row.Field<Boolean>("Material_Only");
                    if (!row.IsNull("CO_ItemNo"))
                        CO_Label.Content = row["CO_ItemNo"].ToString();
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
                    Grid.SetColumn(CO_Label, 1);
                    Grid.SetColumn(MatOnly_CheckBox, 2);
                    Grid.SetColumn(Material_ComboBox, 3);
                    Grid.SetColumn(Phase_TextBox, 4);
                    Grid.SetColumn(Type_TextBox, 5);
                    Grid.SetColumn(Color_TextBox, 6);
                    Grid.SetColumn(QtyReqd_TextBox, 7);
                    Grid.SetColumn(TotalCost_TextBox, 8);

                    secondGrid.Children.Add(SOV_ComboBox);
                    secondGrid.Children.Add(CO_Label);
                    secondGrid.Children.Add(MatOnly_CheckBox);
                    secondGrid.Children.Add(Material_ComboBox);
                    secondGrid.Children.Add(Phase_TextBox);
                    secondGrid.Children.Add(Type_TextBox);
                    secondGrid.Children.Add(Color_TextBox);
                    secondGrid.Children.Add(QtyReqd_TextBox);
                    secondGrid.Children.Add(TotalCost_TextBox);

                    Grid.SetRow(secondGrid, rowIndex);

                    SOVGrid2.Children.Add(secondGrid);
                    rowIndex += 1;
                }

                // TrackShipList
                rowIndex = 0;
                sqlquery = "Select tblMat.*, tblMaterials.Material_Desc from(Select tblSOV.SOV_Acronym, tblSOV.CO_ItemNo, tblSOV.Material_Only, tblSOV.SOV_Desc, tblProjectMaterials.ProjMat_ID, tblProjectMaterials.Mat_Phase, tblProjectMaterials.Mat_Type,tblProjectMaterials.Color_Selected, tblProjectMaterials.Qty_Reqd, tblProjectMaterials.TotalCost, Material_ID from(Select tblSOV.*, tblProjectChangeOrders.CO_ItemNo from(Select tblSOV.*, tblScheduleOfValues.SOV_Desc from tblScheduleOfValues Right JOIN(SELECT tblProjectSOV.* From tblProjects LEFT Join tblProjectSOV ON tblProjects.Project_ID = tblProjectSOV.Project_ID where tblProjects.Project_ID = " + selectedProjectID.ToString() + ") AS tblSOV ON tblSOV.SOV_Acronym = tblScheduleOfValues.SOV_Acronym Where tblScheduleOfValues.Active = 'true') AS tblSOV LEFT JOIN tblProjectChangeOrders ON tblProjectChangeOrders.CO_ID = tblSOV.CO_ID) AS tblSOV LEFT JOIN tblProjectMaterials ON tblSOV.ProjSOV_ID = tblProjectMaterials.ProjSOV_ID) AS tblMat LEFT JOIN tblMaterials ON tblMat.Material_ID = tblMaterials.Material_ID ORDER BY tblMaterials.Material_Desc;";
                cmd = new SqlCommand(sqlquery, con);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                rowCount = ds.Tables[0].Rows.Count; // number of rows
                
                st_TrackShipRecv.Clear();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    //int pmID = int.Parse(row["PM_ID"].ToString());
                    string sovAcronym = row["SOV_Acronym"].ToString();
                    //if (!row.IsNull("CO_ItemNo"))
                    //    coItemNo = int.Parse(row["CO_ItemNo"].ToString());
                    string coItemNo = row["CO_ItemNo"].ToString();
                    string materialDesc = row["Material_Desc"].ToString();
                    string materialOnly = row["Material_Only"].ToString();
                    string sovDesc = row["SOV_Desc"].ToString();
                    string matPhase = row["Mat_Phase"].ToString();
                    string matType = row["Mat_Type"].ToString();
                    string colorSelected = row["Color_Selected"].ToString();
                    string qtyReqd = "";
                    if (!row.IsNull("Qty_Reqd"))
                        qtyReqd = row["Qty_Reqd"].ToString();
                    string totalCost = row["TotalCost"].ToString();
                    string projmatID = row["ProjMat_ID"].ToString();

                    st_TrackShipRecv.Add(new TrackShipRecv
                    {
                        MaterialName = materialDesc,
                        SovName = sovAcronym,
                        ChangeOrder = coItemNo,
                        Phase = matPhase,
                        Type = matType,
                        Color = colorSelected,
                        QtyReqd = qtyReqd,
                        ProjMatID = projmatID
                    });

                    TrackShipList.ItemsSource = st_TrackShipRecv;
                    TrackShipList.SelectedValuePath = "ProjMatID";
                }
            //}
            //catch
            //{
            //    Console.Write("Selected item is not existed");
            //}

        }

        private void SelectPaymentComboBoxItem(object sender, MouseButtonEventArgs e)
        {
            ComboBoxItem item = sender as ComboBoxItem;
            Payment Payment = new Payment();
            Payment dataObject = item.DataContext as Payment;
            dataObject.IsChecked = !dataObject.IsChecked;
            
            //MessageBox.Show(dataObject.Name);
            e.Handled = true;
        }

        private void SelectPaymentCheckBox(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void SelectTrackShipList(object sender, SelectionChangedEventArgs e)
        {
            if (TrackShipList.SelectedItem != null)
            {
                string selectedId = TrackShipList.SelectedValue.ToString();

                int rowIndex = 0;
                int rowCount = 0;
                if (!string.IsNullOrEmpty(selectedId))
                {
                    sqlquery = "Select MatReqdDate, Qty_Ord, tblManufacturers.Manuf_ID, TakeFromStock, Manuf_LeadTime, Manuf_Order_No, PO_Number, ShopReqDate, ShopRecvdDate, SubmitIssue, Resubmit_Date, SubmitAppr, No_Sub_Needed, Ship_to_Job, FM_Needed, Guar_Dim, Field_Dim, Finals_Rev,  ReleasedForFab, MatComplete, LaborComplete from tblManufacturers RIGHT JOIN(Select * from tblProjectMaterialsTrack where ProjMat_ID = " + selectedId + ") AS tblProjMat ON tblManufacturers.Manuf_ID = tblProjMat.Manuf_ID;";
                    cmd = new SqlCommand(sqlquery, con);
                    sda = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    sda.Fill(ds);
                    rowCount = ds.Tables[0].Rows.Count; // number of rows
                }


                for (int i = 0; i < rowCount; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                    TrackingGrid.RowDefinitions.Add(rowDef);
                }

                sb_projectMatTrackings.Clear();
                TrackingGrid.Children.Clear();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime matReqdDate = new DateTime();
                    

                    if (!row.IsNull("MatReqdDate"))
                        matReqdDate = row.Field<DateTime>("MatReqdDate");
                    string qtyOrd = "";
                    if (!row.IsNull("Qty_Ord"))
                        qtyOrd = row.Field<int>("Qty_Ord").ToString();
                    int manufID = row.Field<int>("Manuf_ID");
                    bool stock = row.Field<Boolean>("TakeFromStock");
                    string leadTime = "";
                    if (!row.IsNull("Manuf_LeadTime"))
                        leadTime = row["Manuf_LeadTime"].ToString();
                    string manufOrd = "";
                    if (!row.IsNull("Manuf_Order_No"))
                        manufOrd = row["Manuf_Order_No"].ToString();
                    string gapPO = "";
                    if (!row.IsNull("PO_Number"))
                        gapPO = row["PO_Number"].ToString();
                    DateTime shopReq = new DateTime();
                    if (!row.IsNull("ShopReqDate"))
                        shopReq = row.Field<DateTime>("ShopReqDate");
                    DateTime shopRecv = new DateTime();
                    if (!row.IsNull("ShopRecvdDate"))
                        shopRecv = row.Field<DateTime>("ShopRecvdDate");
                    DateTime submIssue = new DateTime();
                    if (!row.IsNull("SubmitIssue"))
                        submIssue = row.Field<DateTime>("SubmitIssue");
                    DateTime reSubmit = new DateTime();
                    if (!row.IsNull("Resubmit_Date"))
                        reSubmit = row.Field<DateTime>("Resubmit_Date");
                    DateTime submAppr = new DateTime();
                    if (!row.IsNull("SubmitAppr"))
                        submAppr = row.Field<DateTime>("SubmitAppr");
                    bool noSubNeeded = row.Field<Boolean>("No_Sub_Needed");
                    bool shipToJob = row.Field<Boolean>("Ship_to_Job");
                    bool fmNeeded = row.Field<Boolean>("FM_Needed");
                    bool guarDim = row.Field<Boolean>("Guar_Dim");
                    DateTime fieldDim = new DateTime();
                    if (!row.IsNull("Field_Dim"))
                        fieldDim = row.Field<DateTime>("Field_Dim");
                    bool finalsRev = row.Field<Boolean>("Finals_Rev");
                    DateTime rff = new DateTime();
                    if (!row.IsNull("ReleasedForFab"))
                        rff = row.Field<DateTime>("ReleasedForFab");
                    bool matComplete = row.Field<Boolean>("MatComplete");
                    bool laborComplete = row.Field<Boolean>("LaborComplete");

                    sb_projectMatTrackings.Add(new ProjectMatTracking
                    {
                        ProjMat = int.Parse(selectedId),
                        MatReqdDate = matReqdDate,
                        QtyOrd = qtyOrd,
                        ManufacturerID = manufID,
                        TakeFromStock = stock,
                        LeadTime = leadTime,
                        ManuOrderNo = manufOrd,
                        PoNumber = gapPO,
                        ShopReqDate = shopReq,
                        ShopRecvdDate = shopRecv,
                        SubmAppr = submAppr,
                        NoSubm = noSubNeeded,
                        ShipToJob = shipToJob,
                        NeedFM = fmNeeded,
                        GuarDim = guarDim,
                        FieldDim = fieldDim,
                        FinalsRev = finalsRev,
                        RFF = rff,
                        OrderComplete = matComplete,
                        LaborComplete = laborComplete
                    });

                    foreach (ProjectMatTracking track in sb_projectMatTrackings)
                    {
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
                        ColumnDefinition colDef10 = new ColumnDefinition();
                        ColumnDefinition colDef11 = new ColumnDefinition();
                        ColumnDefinition colDef12 = new ColumnDefinition();
                        ColumnDefinition colDef13 = new ColumnDefinition();
                        ColumnDefinition colDef14 = new ColumnDefinition();
                        ColumnDefinition colDef15 = new ColumnDefinition();
                        ColumnDefinition colDef16 = new ColumnDefinition();
                        ColumnDefinition colDef17 = new ColumnDefinition();
                        ColumnDefinition colDef18 = new ColumnDefinition();
                        ColumnDefinition colDef19 = new ColumnDefinition();
                        ColumnDefinition colDef20 = new ColumnDefinition();
                        ColumnDefinition colDef21 = new ColumnDefinition();

                        //TrackingGrid
                        secondGrid.ColumnDefinitions.Add(colDef1);
                        secondGrid.ColumnDefinitions.Add(colDef2);
                        secondGrid.ColumnDefinitions.Add(colDef3);
                        secondGrid.ColumnDefinitions.Add(colDef4);
                        secondGrid.ColumnDefinitions.Add(colDef5);
                        secondGrid.ColumnDefinitions.Add(colDef6);
                        secondGrid.ColumnDefinitions.Add(colDef7);
                        secondGrid.ColumnDefinitions.Add(colDef8);
                        secondGrid.ColumnDefinitions.Add(colDef9);
                        secondGrid.ColumnDefinitions.Add(colDef10);
                        secondGrid.ColumnDefinitions.Add(colDef11);
                        secondGrid.ColumnDefinitions.Add(colDef12);
                        secondGrid.ColumnDefinitions.Add(colDef13);
                        secondGrid.ColumnDefinitions.Add(colDef14);
                        secondGrid.ColumnDefinitions.Add(colDef15);
                        secondGrid.ColumnDefinitions.Add(colDef16);
                        secondGrid.ColumnDefinitions.Add(colDef17);
                        secondGrid.ColumnDefinitions.Add(colDef18);
                        secondGrid.ColumnDefinitions.Add(colDef19);
                        secondGrid.ColumnDefinitions.Add(colDef20);
                        secondGrid.ColumnDefinitions.Add(colDef21);

                        secondGrid.ColumnDefinitions[0].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[1].Width = new GridLength(1.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[2].Width = new GridLength(4, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[3].Width = new GridLength(1, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[4].Width = new GridLength(2, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[5].Width = new GridLength(2, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[6].Width = new GridLength(2, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[7].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[8].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[9].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[10].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[11].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[12].Width = new GridLength(1, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[13].Width = new GridLength(1, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[14].Width = new GridLength(1, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[15].Width = new GridLength(1, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[16].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[17].Width = new GridLength(1, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[18].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[19].Width = new GridLength(1.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[20].Width = new GridLength(1.5, GridUnitType.Star);

                        DatePicker MatlReqd_Date = new DatePicker();
                        TextBox QtyOrd_TextBox = new TextBox();
                        ComboBox Manufacturer_Combo = new ComboBox();
                        CheckBox Stock_CheckBox = new CheckBox();
                        TextBox LeadTime_TextBox = new TextBox();
                        TextBox ManuOrd_TextBox = new TextBox();
                        TextBox Gap_TextBox = new TextBox();
                        DatePicker ShopReqst_Date = new DatePicker();
                        DatePicker ShopRecvd_Date = new DatePicker();
                        DatePicker Subm_Date = new DatePicker();
                        DatePicker Resubmit_Date = new DatePicker();
                        DatePicker SubmAppr_Date = new DatePicker();
                        CheckBox NoSubm_CheckBox = new CheckBox();
                        CheckBox ShipToJob_CheckBox = new CheckBox();
                        CheckBox NeedFM_CheckBox = new CheckBox();
                        CheckBox GuarDim_CheckBox = new CheckBox();
                        DatePicker FieldDim_Date = new DatePicker();
                        CheckBox FinalsRev_CheckBox = new CheckBox();
                        DatePicker RFF_Date = new DatePicker();
                        CheckBox Order_CheckBox = new CheckBox();
                        CheckBox Labor_CheckBox = new CheckBox();

                        //MatlReqd_Date.SetBinding(DatePicker.TextProperty, new Binding("MatReqdDate"));
                        //QtyOrd_TextBox.SetBinding(TextBox.TextProperty, new Binding("QtyOrd"));

                        MatlReqd_Date.Text = track.MatReqdDate.ToString();
                        QtyOrd_TextBox.Text = track.QtyOrd;
                        Manufacturer_Combo.ItemsSource = sb_manufacturers;
                        Manufacturer_Combo.DisplayMemberPath = "ManufacturerName";
                        Manufacturer_Combo.SelectedValuePath = "ID";
                        Manufacturer_Combo.SelectedValue = track.ManufacturerID;

                        Stock_CheckBox.IsChecked = track.TakeFromStock;
                        LeadTime_TextBox.Text = track.LeadTime.ToString();
                        ManuOrd_TextBox.Text = track.ManuOrderNo.ToString();
                        Gap_TextBox.Text = track.PoNumber;
                        ShopReqst_Date.Text = track.ShopReqDate.ToString();
                        ShopRecvd_Date.Text = track.ShopRecvdDate.ToString();
                        Subm_Date.Text = track.SubmIssue.ToString();
                        Resubmit_Date.Text = track.ReSubmit.ToString();
                        SubmAppr_Date.Text = track.SubmAppr.ToString();
                        NoSubm_CheckBox.IsChecked = track.NoSubm;
                        ShipToJob_CheckBox.IsChecked = track.ShipToJob;
                        NeedFM_CheckBox.IsChecked = track.NeedFM;
                        GuarDim_CheckBox.IsChecked = track.GuarDim;
                        FieldDim_Date.Text = track.FieldDim.ToString();
                        FinalsRev_CheckBox.IsChecked = track.FinalsRev;
                        RFF_Date.Text = track.RFF.ToString();
                        Order_CheckBox.IsChecked = track.OrderComplete;
                        Labor_CheckBox.IsChecked = track.LaborComplete;

                        Grid.SetColumn(MatlReqd_Date, 0);
                        Grid.SetColumn(QtyOrd_TextBox, 1);
                        Grid.SetColumn(Manufacturer_Combo, 2);
                        Grid.SetColumn(Stock_CheckBox, 3);
                        Grid.SetColumn(LeadTime_TextBox, 4);
                        Grid.SetColumn(ManuOrd_TextBox, 5);
                        Grid.SetColumn(Gap_TextBox, 6);
                        Grid.SetColumn(ShopReqst_Date, 7);
                        Grid.SetColumn(ShopRecvd_Date, 8);
                        Grid.SetColumn(Subm_Date, 9);
                        Grid.SetColumn(Resubmit_Date, 10);
                        Grid.SetColumn(SubmAppr_Date, 11);
                        Grid.SetColumn(NoSubm_CheckBox, 12);
                        Grid.SetColumn(ShipToJob_CheckBox, 13);
                        Grid.SetColumn(NeedFM_CheckBox, 14);
                        Grid.SetColumn(GuarDim_CheckBox, 15);
                        Grid.SetColumn(FieldDim_Date, 16);
                        Grid.SetColumn(FinalsRev_CheckBox, 17);
                        Grid.SetColumn(RFF_Date, 18);
                        Grid.SetColumn(Order_CheckBox, 19);
                        Grid.SetColumn(Labor_CheckBox, 20);

                        secondGrid.Children.Add(MatlReqd_Date);
                        secondGrid.Children.Add(QtyOrd_TextBox);
                        secondGrid.Children.Add(Manufacturer_Combo);
                        secondGrid.Children.Add(Stock_CheckBox);
                        secondGrid.Children.Add(LeadTime_TextBox);
                        secondGrid.Children.Add(ManuOrd_TextBox);
                        secondGrid.Children.Add(Gap_TextBox);
                        secondGrid.Children.Add(ShopReqst_Date);
                        secondGrid.Children.Add(ShopRecvd_Date);
                        secondGrid.Children.Add(Subm_Date);
                        secondGrid.Children.Add(Resubmit_Date);
                        secondGrid.Children.Add(SubmAppr_Date);
                        secondGrid.Children.Add(NoSubm_CheckBox);
                        secondGrid.Children.Add(ShipToJob_CheckBox);
                        secondGrid.Children.Add(NeedFM_CheckBox);
                        secondGrid.Children.Add(GuarDim_CheckBox);
                        secondGrid.Children.Add(FieldDim_Date);
                        secondGrid.Children.Add(FinalsRev_CheckBox);
                        secondGrid.Children.Add(RFF_Date);
                        secondGrid.Children.Add(Order_CheckBox);
                        secondGrid.Children.Add(Labor_CheckBox);

                        Grid.SetRow(secondGrid, rowIndex);

                        TrackingGrid.Children.Add(secondGrid);
                        rowIndex += 1;
                    }
                }

                if (!string.IsNullOrEmpty(selectedId))
                {
                    sqlquery = "SELECT * FROM tblProjectMaterialsShip RIGHT JOIN (SELECT ProjMT_ID FROM tblProjectMaterialsTrack WHERE ProjMat_ID = " + selectedId + ") AS tblMat ON tblProjectMaterialsShip.ProjMT_ID = tblMat.ProjMT_ID;";
                    cmd = new SqlCommand(sqlquery, con);
                    sda = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    sda.Fill(ds);
                    rowCount = ds.Tables[0].Rows.Count; // number of rows
                }

                ShippingGrid.Children.Clear();

                for (int i = 0; i < rowCount; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                    TrackingGrid.RowDefinitions.Add(rowDef);
                }

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DateTime schedShip = new DateTime();
                    DateTime estDeliv = new DateTime();
                    DateTime estInstall = new DateTime();
                    string estWeight = "";
                    int estNoPallets = 0;
                    int freightCoID = 0;
                    string tracking = "";
                    DateTime dateShipped = new DateTime();
                    int qtyShipped = 0;
                    int noOfPallet = 0;
                    DateTime dateRecvd = new DateTime();
                    int qtyRecvd = 0;
                    bool freightDamage = false;
                    DateTime deliverToJob = new DateTime();
                    string siteStroage = "";
                    string stroageDetail = "";

                    if (!row.IsNull("SchedShipDate"))
                        schedShip = row.Field<DateTime>("SchedShipDate");
                    if (!row.IsNull("EstDelivDate"))
                        estDeliv = row.Field<DateTime>("EstDelivDate");
                    if (!row.IsNull("EstInstallDate"))
                        estInstall = row.Field<DateTime>("EstInstallDate");
                    if (!row.IsNull("EstWeight"))
                        estWeight = row["EstWeight"].ToString();
                    if (!row.IsNull("EstNoPallets"))
                        estNoPallets = row.Field<int>("EstNoPallets");
                    if (!row.IsNull("FreightCo_ID"))
                        freightCoID = row.Field<int>("FreightCo_ID");
                    if (!row.IsNull("TrackingNo"))
                        tracking = row["TrackingNo"].ToString();
                    if (!row.IsNull("Qty_Shipped"))
                        dateShipped = row.Field<DateTime>("Qty_Shipped");
                    if (!row.IsNull("EstNoPallets"))
                        qtyShipped = row.Field<int>("Qty_Shipped");
                    if (!row.IsNull("NoOfPallets"))
                        noOfPallet = int.Parse(row["NoOfPallets"].ToString());
                    if (!row.IsNull("Date_Recvd"))
                        dateRecvd = row.Field<DateTime>("Date_Recvd");
                    if (!row.IsNull("Qty_Recvd"))
                        qtyRecvd = row.Field<int>("Qty_Recvd");
                    if (!row.IsNull("FreightDamage"))
                        freightDamage = row.Field<Boolean>("FreightDamage");
                    if (!row.IsNull("DelivertoJob"))
                        deliverToJob = row.Field<DateTime>("DelivertoJob");
                    if (!row.IsNull("SiteStorage"))
                        siteStroage = row["SiteStorage"].ToString();
                    if (!row.IsNull("StorageDetail"))
                        stroageDetail = row["StorageDetail"].ToString();

                    sb_projectMtShip.Clear();
                    sb_projectMtShip.Add(new ProjectMatShip
                    {
                        SchedShipDate = schedShip,
                        EstDelivDate = estDeliv,
                        EstInstallDate = estInstall,
                        EstWeight = estWeight,
                        EstPallet = estNoPallets,
                        FreightCoID = freightCoID,
                        TrackingNo = tracking,
                        DateShipped = dateShipped,
                        QtyShipped = qtyShipped,
                        NoOfPallet = noOfPallet,
                        DateRecvd = dateRecvd,
                        QtyRecvd = qtyRecvd,
                        FreightDamage = freightDamage,
                        DeliverToJob = deliverToJob,
                        SiteStroage = siteStroage,
                        StroageDetail = stroageDetail
                    });

                    foreach (ProjectMatShip ship in sb_projectMtShip)
                    {
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
                        ColumnDefinition colDef10 = new ColumnDefinition();
                        ColumnDefinition colDef11 = new ColumnDefinition();
                        ColumnDefinition colDef12 = new ColumnDefinition();
                        ColumnDefinition colDef13 = new ColumnDefinition();
                        ColumnDefinition colDef14 = new ColumnDefinition();
                        ColumnDefinition colDef15 = new ColumnDefinition();
                        ColumnDefinition colDef16 = new ColumnDefinition();

                        secondGrid.ColumnDefinitions.Add(colDef1);
                        secondGrid.ColumnDefinitions.Add(colDef2);
                        secondGrid.ColumnDefinitions.Add(colDef3);
                        secondGrid.ColumnDefinitions.Add(colDef4);
                        secondGrid.ColumnDefinitions.Add(colDef5);
                        secondGrid.ColumnDefinitions.Add(colDef6);
                        secondGrid.ColumnDefinitions.Add(colDef7);
                        secondGrid.ColumnDefinitions.Add(colDef8);
                        secondGrid.ColumnDefinitions.Add(colDef9);
                        secondGrid.ColumnDefinitions.Add(colDef10);
                        secondGrid.ColumnDefinitions.Add(colDef11);
                        secondGrid.ColumnDefinitions.Add(colDef12);
                        secondGrid.ColumnDefinitions.Add(colDef13);
                        secondGrid.ColumnDefinitions.Add(colDef14);
                        secondGrid.ColumnDefinitions.Add(colDef15);
                        secondGrid.ColumnDefinitions.Add(colDef16);

                        secondGrid.ColumnDefinitions[0].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[1].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[2].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[3].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[4].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[5].Width = new GridLength(4, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[6].Width = new GridLength(3, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[7].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[8].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[9].Width = new GridLength(2, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[10].Width = new GridLength(2.5, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[11].Width = new GridLength(2, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[12].Width = new GridLength(2, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[13].Width = new GridLength(2, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[14].Width = new GridLength(3, GridUnitType.Star);
                        secondGrid.ColumnDefinitions[15].Width = new GridLength(3, GridUnitType.Star);

                        DatePicker SchedShip_Date = new DatePicker();
                        DatePicker EstDeliv_Date = new DatePicker();
                        DatePicker EstInstall_Date = new DatePicker();
                        TextBox EstWeight_TextBox = new TextBox();
                        TextBox EstPallet_TextBox = new TextBox();
                        ComboBox FreightCo_ComBo = new ComboBox();
                        TextBox Tracking_TextBox = new TextBox();
                        DatePicker DateShipped_Date = new DatePicker();
                        TextBox QtyShipped_TextBox = new TextBox();
                        TextBox Pallet_TextBox = new TextBox();
                        DatePicker DateRecvd_Date = new DatePicker();
                        TextBox QtyRecvd_TextBox = new TextBox();
                        CheckBox FreightDamage_CheckBox = new CheckBox();
                        DatePicker DeliverToJob_Date = new DatePicker();
                        ComboBox SiteStroage_ComBo = new ComboBox();
                        TextBox StroageDetail_TextBox = new TextBox();

                        SchedShip_Date.Text = ship.SchedShipDate.ToString();
                        EstDeliv_Date.Text = ship.EstDelivDate.ToString();
                        EstInstall_Date.Text = ship.EstInstallDate.ToString();
                        EstWeight_TextBox.Text = ship.EstWeight.ToString();
                        EstPallet_TextBox.Text = ship.EstPallet.ToString();
                        //FreightCo_ComBo.Text = ship.FreightCoID.ToString();
                        FreightCo_ComBo.ItemsSource = sb_freightCo;
                        FreightCo_ComBo.DisplayMemberPath = "FreightName";
                        FreightCo_ComBo.SelectedValuePath = "FreightCoID";
                        //FreightCo_ComBo.SelectedValue = ship.FreightCoID;
                        FreightCo_ComBo.SelectedValue = 0;

                        Tracking_TextBox.Text = ship.TrackingNo.ToString();
                        DateShipped_Date.Text = ship.DateShipped.ToString();
                        QtyShipped_TextBox.Text = ship.QtyShipped.ToString();

                        Pallet_TextBox.Text = ship.NoOfPallet.ToString();
                        DateRecvd_Date.Text = ship.DateRecvd.ToString();
                        QtyRecvd_TextBox.Text = ship.QtyRecvd.ToString();
                        //FreightDamage_CheckBox.Text = ship.EstWeight.ToString();
                        DeliverToJob_Date.Text = ship.DeliverToJob.ToString();
                        //SiteStroage_ComBo.Text = ship.SiteStroage.ToString();
                        StroageDetail_TextBox.Text = ship.StroageDetail.ToString();




                        Grid.SetColumn(SchedShip_Date, 0);
                        Grid.SetColumn(EstDeliv_Date, 1);
                        Grid.SetColumn(EstInstall_Date, 2);
                        Grid.SetColumn(EstWeight_TextBox, 3);
                        Grid.SetColumn(EstPallet_TextBox, 4);
                        Grid.SetColumn(FreightCo_ComBo, 5);
                        Grid.SetColumn(Tracking_TextBox, 6);
                        Grid.SetColumn(DateShipped_Date, 7);
                        Grid.SetColumn(QtyShipped_TextBox, 8);
                        Grid.SetColumn(Pallet_TextBox, 9);
                        Grid.SetColumn(DateRecvd_Date, 10);
                        Grid.SetColumn(QtyRecvd_TextBox, 11);
                        Grid.SetColumn(FreightDamage_CheckBox, 12);
                        Grid.SetColumn(DeliverToJob_Date, 13);
                        Grid.SetColumn(SiteStroage_ComBo, 14);
                        Grid.SetColumn(StroageDetail_TextBox, 15);

                        secondGrid.Children.Add(SchedShip_Date);
                        secondGrid.Children.Add(EstDeliv_Date);
                        secondGrid.Children.Add(EstInstall_Date);
                        secondGrid.Children.Add(EstWeight_TextBox);
                        secondGrid.Children.Add(EstPallet_TextBox);
                        secondGrid.Children.Add(FreightCo_ComBo);
                        secondGrid.Children.Add(Tracking_TextBox);
                        secondGrid.Children.Add(DateShipped_Date);
                        secondGrid.Children.Add(QtyShipped_TextBox);
                        secondGrid.Children.Add(Pallet_TextBox);
                        secondGrid.Children.Add(DateRecvd_Date);
                        secondGrid.Children.Add(QtyRecvd_TextBox);
                        secondGrid.Children.Add(FreightDamage_CheckBox);
                        secondGrid.Children.Add(DeliverToJob_Date);
                        secondGrid.Children.Add(SiteStroage_ComBo);
                        secondGrid.Children.Add(StroageDetail_TextBox);

                        Grid.SetRow(secondGrid, rowIndex);

                        ShippingGrid.Children.Add(secondGrid);
                        rowIndex += 1;
                    }
                }
            }
        }
    }
}
