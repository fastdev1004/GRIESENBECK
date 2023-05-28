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
        private ObservableCollection<int> st_projectOrder;
        private ObservableCollection<SovCO> st_SovCO;
        private ObservableCollection<TrackShipRecv> st_TrackShipRecv;
        private ObservableCollection<ProjectMatTracking> sb_projectMatTrackings;
        private ObservableCollection<ProjectMatShip> sb_projectMtShip;
        private ObservableCollection<TrackReport> sb_trackReports;
        private ObservableCollection<TrackLaborReport> sb_trackLaborReports;
        private ObservableCollection<WorkOrder> sb_workOrders;
        private ObservableCollection<Note> sb_notes;
        private ObservableCollection<InstallationNote> sb_installationNote;
        private ObservableCollection<Contract> sb_contract;
        private ObservableCollection<CIP> sb_cip;
        private ObservableCollection<ProjectWorkOrder> sb_projectWorkOrder;
        private ObservableCollection<ProjectLabor> sb_projectLabor;

        private ProjectViewModel ProjectVM;
        public ProjectView()
        {
            InitializeComponent();
            ProjectVM = new ProjectViewModel();
            this.DataContext = ProjectVM;
            con = ProjectVM.con;
            con.Open();
            st_projectOrder = new ObservableCollection<int>();
            st_SovCO = new ObservableCollection<SovCO>();
            st_TrackShipRecv = new ObservableCollection<TrackShipRecv>();
            sb_projectMatTrackings = new ObservableCollection<ProjectMatTracking>();
            sb_projectMtShip = new ObservableCollection<ProjectMatShip>();
            sb_trackReports = new ObservableCollection<TrackReport>();
            sb_trackLaborReports = new ObservableCollection<TrackLaborReport>();
            sb_workOrders = new ObservableCollection<WorkOrder>();
            sb_notes = new ObservableCollection<Note>();
            sb_installationNote = new ObservableCollection<InstallationNote>();
            sb_contract = new ObservableCollection<Contract>();
            sb_cip = new ObservableCollection<CIP>();
            sb_projectWorkOrder = new ObservableCollection<ProjectWorkOrder>();
            sb_projectLabor = new ObservableCollection<ProjectLabor>();
        }

        private void goBack(object sender, RoutedEventArgs e)
        {
            cmd.Dispose();
            con.Close();
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }

        private void SelectProjectItem(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Project selectedItem = (Project)comboBox.SelectedItem;
            if (!comboBox.Items.Contains(comboBox.Text))
            {
                foreach (var item in comboBox.Items)
                {
                    //Console.WriteLine(item.);
                }
            }

            int selectedProjectID = selectedItem.ID;

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

            int rowCount = 1;
            int rowIndex = 0;

            // PM Grid
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

            // Supt Grid
            //sqlquery = "Select * from tblProjectPMs where Project_ID = " + selectedProjectID.ToString();
            //cmd = new SqlCommand(sqlquery, con);
            //sda = new SqlDataAdapter(cmd);
            //ds = new DataSet();
            //sda.Fill(ds);
            //rowCount = ds.Tables[0].Rows.Count; // number of rows
            //PMGrid.Children.Clear();
            //for (int i = 0; i < rowCount; i++)
            //{
            //    RowDefinition rowDef = new RowDefinition();
            //    rowDef.Height = GridLength.Auto;
            //    PMGrid.RowDefinitions.Add(rowDef);
            //}

            // ProjectNote Grid
            sqlquery = "SELECT * FROM tblNotes WHERE Notes_PK_Desc = 'Project' AND Notes_PK =" + selectedProjectID.ToString();
            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            rowCount = ds.Tables[0].Rows.Count; // number of rows
            ProjectNoteGrid.Children.Clear();
            for (int i = 0; i < rowCount; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = GridLength.Auto;
                ProjectNoteGrid.RowDefinitions.Add(rowDef);
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Grid firstGrid = new Grid();
                for(int i = 0; i<2; i++)
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

                Acronym_ComboBox.ItemsSource = ProjectVM.Acronyms;
                Acronym_ComboBox.SelectedValuePath = "AcronymName";
                Acronym_ComboBox.DisplayMemberPath = "AcronymName";
                Acronym_ComboBox.SelectedValue = row["SOV_Acronym"].ToString();
                Acronym_ComboBox.IsEditable = true;
                Acronym_ComboBox.Margin = new Thickness(4, 1, 4, 1);

                ChangeOrder_ComboBox.ItemsSource = st_projectOrder;
                if (!row.IsNull("CO_ItemNo"))
                    ChangeOrder_ComboBox.SelectedValue = int.Parse(row["CO_ItemNo"].ToString());
                ChangeOrder_ComboBox.Margin = new Thickness(4, 1, 4, 1);

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

                SOV_ComboBox.ItemsSource = st_SovCO;
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
                string sovAcronym = row["SOV_Acronym"].ToString();
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

            // Tracking Report Grid1
            sqlquery = "Select MatReqdDate, tblManufacturers.Manuf_Name, Qty_Ord, tblProjMat.Mat_Phase, tblProjMat.Mat_Type, Manuf_LeadTime, PO_Number, ShopReqDate, ShopRecvdDate, SubmitIssue, Resubmit_Date, SubmitAppr,tblProjMat.Color_Selected, Guar_Dim, Field_Dim, ReleasedForFab, LaborComplete from tblManufacturers RIGHT JOIN(Select tblProjectMaterialsTrack.*, tblMat.Color_Selected, tblMat.Mat_Phase, tblMat.Mat_Type from tblProjectMaterialsTrack RIGHT JOIN(SELECT * FROM tblProjectMaterials WHERE tblProjectMaterials.Project_ID = " + selectedProjectID.ToString() + ") AS tblMat ON tblMat.ProjMat_ID = tblProjectMaterialsTrack.ProjMat_ID) AS tblProjMat ON tblManufacturers.Manuf_ID = tblProjMat.Manuf_ID ORDER BY MatReqdDate;";
            rowIndex = 0;

            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            rowCount = ds.Tables[0].Rows.Count; // number of rows

            for (int i = 0; i < rowCount; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = GridLength.Auto;
                TrackingReportGrid1.RowDefinitions.Add(rowDef);
            }

            TrackingReportGrid1.Children.Clear();
            sb_trackReports.Clear();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DateTime matReqdDate = new DateTime();

                string manufName = "";
                string qtyOrd = "";
                string phase = "";
                string type = "";
                string leadTime = "";
                string gapPO = "";
                DateTime shopReq = new DateTime();
                DateTime shopRecv = new DateTime();
                DateTime submIssue = new DateTime();
                DateTime reSubmit = new DateTime();
                DateTime submAppr = new DateTime();
                string color = "";
                bool guarDim = false;
                DateTime fieldDim = new DateTime();
                DateTime rff = new DateTime();
                bool laborComplete = false;

                if (!row.IsNull("MatReqdDate"))
                    matReqdDate = row.Field<DateTime>("MatReqdDate");
                if (!row.IsNull("Manuf_Name"))
                    manufName = row["Manuf_Name"].ToString();
                if (!row.IsNull("Qty_Ord"))
                    qtyOrd = row["Qty_Ord"].ToString();
                if (!row.IsNull("Mat_Phase"))
                    phase = row["Mat_Phase"].ToString();
                if (!row.IsNull("Mat_Type"))
                    type = row["Mat_Type"].ToString();
                if (!row.IsNull("Manuf_LeadTime"))
                    leadTime = row["Manuf_LeadTime"].ToString();
                if (!row.IsNull("PO_Number"))
                    gapPO = row["PO_Number"].ToString();
                if (!row.IsNull("ShopReqDate"))
                    shopReq = row.Field<DateTime>("ShopReqDate");
                if (!row.IsNull("ShopRecvdDate"))
                    shopRecv = row.Field<DateTime>("ShopRecvdDate");
                if (!row.IsNull("SubmitIssue"))
                    submIssue = row.Field<DateTime>("SubmitIssue");
                if (!row.IsNull("Resubmit_Date"))
                    reSubmit = row.Field<DateTime>("Resubmit_Date");
                if (!row.IsNull("SubmitAppr"))
                    submAppr = row.Field<DateTime>("SubmitAppr");
                if (!row.IsNull("Color_Selected"))
                    color = row["Color_Selected"].ToString();
                if (!row.IsNull("Guar_Dim"))
                    guarDim = row.Field<Boolean>("Guar_Dim");
                if (!row.IsNull("Field_Dim"))
                    fieldDim = row.Field<DateTime>("Field_Dim");
                if (!row.IsNull("ReleasedForFab"))
                    rff = row.Field<DateTime>("ReleasedForFab");
                if (!row.IsNull("LaborComplete"))
                    laborComplete = row.Field<Boolean>("LaborComplete");

                sb_trackReports.Add(new TrackReport
                {
                    //ProjMat = int.Parse(selectedId),
                    MatReqdDate = matReqdDate,
                    ManufacturerName = manufName,
                    QtyOrd = qtyOrd,
                    Phase = phase,
                    Type = type,
                    ManufLeadTime = leadTime,
                    PoNumber = gapPO,
                    ShopReqDate = shopReq,
                    ShopRecvdDate = shopRecv,
                    SubmIssue = submIssue,
                    ReSubmit = reSubmit,
                    SubmAppr = submAppr,
                    Color = color,
                    GuarDim = guarDim,
                    FieldDim = fieldDim,
                    RFF = rff,
                    LaborComplete = laborComplete
                });
            }

            foreach (TrackReport report in sb_trackReports)
            {
                // Create the row dynamically
                DatePicker MatlReqd_Date = new DatePicker();
                Label Manufacturer_TextBox = new Label();
                //Manufacturer_TextBox.IsReadOnly = true;
                Label QtyOrd_TextBox = new Label();
                Label Phase_TextBox = new Label();
                Label Type_TextBox = new Label();
                Label LeadTime_TextBox = new Label();
                Label PONumber_TextBox = new Label();
                DatePicker ShopReqst_Date = new DatePicker();
                DatePicker ShopRecvd_Date = new DatePicker();
                DatePicker Subm_Date = new DatePicker();
                DatePicker Resubmit_Date = new DatePicker();
                DatePicker SubmAppr_Date = new DatePicker();
                Label Color_TextBox = new Label();
                CheckBox GuarDim_CheckBox = new CheckBox();
                DatePicker FieldDim_Date = new DatePicker();
                DatePicker RFF_Date = new DatePicker();
                CheckBox Labor_CheckBox = new CheckBox();

                //Manufacturer_TextBox.IsReadOnly = true;
                //Type_TextBox.IsReadOnly = true;
                //LeadTime_TextBox.IsReadOnly = true;
                //Color_TextBox.IsReadOnly = true;

                MatlReqd_Date.Text = report.MatReqdDate.ToString();
                Manufacturer_TextBox.Content = report.ManufacturerName;
                QtyOrd_TextBox.Content = report.QtyOrd;
                Phase_TextBox.Content = report.Phase;
                Type_TextBox.Content = report.Type;
                LeadTime_TextBox.Content = report.ManufLeadTime;
                PONumber_TextBox.Content = report.PoNumber;
                ShopReqst_Date.Text = report.ShopReqDate.ToString();
                ShopRecvd_Date.Text = report.ShopRecvdDate.ToString();
                Subm_Date.Text = report.SubmIssue.ToString();
                Resubmit_Date.Text = report.ReSubmit.ToString();
                SubmAppr_Date.Text = report.SubmAppr.ToString();
                Color_TextBox.Content = report.Color;
                GuarDim_CheckBox.IsChecked = report.GuarDim;
                FieldDim_Date.Text = report.FieldDim.ToString();
                RFF_Date.Text = report.RFF.ToString();
                Labor_CheckBox.IsChecked = report.LaborComplete;

                Grid secondGrid = new Grid();
                //TrackingGrid
                for (int i = 0; i < 18; i++)
                {
                    secondGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }

                secondGrid.ColumnDefinitions[0].Width = new GridLength(2.5, GridUnitType.Star);
                secondGrid.ColumnDefinitions[1].Width = new GridLength(4, GridUnitType.Star);
                secondGrid.ColumnDefinitions[2].Width = new GridLength(1.5, GridUnitType.Star);
                secondGrid.ColumnDefinitions[3].Width = new GridLength(3, GridUnitType.Star);
                secondGrid.ColumnDefinitions[4].Width = new GridLength(3, GridUnitType.Star);
                secondGrid.ColumnDefinitions[5].Width = new GridLength(2, GridUnitType.Star);
                secondGrid.ColumnDefinitions[6].Width = new GridLength(2, GridUnitType.Star);
                secondGrid.ColumnDefinitions[7].Width = new GridLength(2.5, GridUnitType.Star);
                secondGrid.ColumnDefinitions[8].Width = new GridLength(2.5, GridUnitType.Star);
                secondGrid.ColumnDefinitions[9].Width = new GridLength(2.5, GridUnitType.Star);
                secondGrid.ColumnDefinitions[10].Width = new GridLength(2.5, GridUnitType.Star);
                secondGrid.ColumnDefinitions[11].Width = new GridLength(2.5, GridUnitType.Star);
                secondGrid.ColumnDefinitions[12].Width = new GridLength(2, GridUnitType.Star);
                secondGrid.ColumnDefinitions[13].Width = new GridLength(1.5, GridUnitType.Star);
                secondGrid.ColumnDefinitions[14].Width = new GridLength(2.5, GridUnitType.Star);
                secondGrid.ColumnDefinitions[15].Width = new GridLength(2.5, GridUnitType.Star);
                secondGrid.ColumnDefinitions[16].Width = new GridLength(2, GridUnitType.Star);

                Grid.SetColumn(MatlReqd_Date, 0);
                Grid.SetColumn(Manufacturer_TextBox, 1);
                Grid.SetColumn(QtyOrd_TextBox, 2);
                Grid.SetColumn(Phase_TextBox, 3);
                Grid.SetColumn(Type_TextBox, 4);
                Grid.SetColumn(LeadTime_TextBox, 5);
                Grid.SetColumn(PONumber_TextBox, 6);
                Grid.SetColumn(ShopReqst_Date, 7);
                Grid.SetColumn(ShopRecvd_Date, 8);
                Grid.SetColumn(Subm_Date, 9);
                Grid.SetColumn(Resubmit_Date, 10);
                Grid.SetColumn(SubmAppr_Date, 11);
                Grid.SetColumn(Color_TextBox, 12);
                Grid.SetColumn(GuarDim_CheckBox, 13);
                Grid.SetColumn(FieldDim_Date, 14);
                Grid.SetColumn(RFF_Date, 15);
                Grid.SetColumn(Labor_CheckBox, 16);

                secondGrid.Children.Add(MatlReqd_Date);
                secondGrid.Children.Add(Manufacturer_TextBox);
                secondGrid.Children.Add(QtyOrd_TextBox);
                secondGrid.Children.Add(Phase_TextBox);
                secondGrid.Children.Add(Type_TextBox);
                secondGrid.Children.Add(LeadTime_TextBox);
                secondGrid.Children.Add(PONumber_TextBox);
                secondGrid.Children.Add(ShopReqst_Date);
                secondGrid.Children.Add(ShopRecvd_Date);
                secondGrid.Children.Add(Subm_Date);
                secondGrid.Children.Add(Resubmit_Date);
                secondGrid.Children.Add(SubmAppr_Date);
                secondGrid.Children.Add(Color_TextBox);
                secondGrid.Children.Add(GuarDim_CheckBox);
                secondGrid.Children.Add(FieldDim_Date);
                secondGrid.Children.Add(RFF_Date);
                secondGrid.Children.Add(Labor_CheckBox);

                if (rowIndex % 2 != 0)
                {
                    secondGrid.Background = new SolidColorBrush(Colors.Gainsboro);
                }
                Grid.SetRow(secondGrid, rowIndex);

                TrackingReportGrid1.Children.Add(secondGrid);

                rowIndex += 1;
            }

            // Tracking Report Grid2
            sqlquery = "SELECT tblLab.SOV_Acronym, tblLab.Labor_Desc, tblProjectChangeOrders.CO_ItemNo, tblLab.Lab_Phase, tblLab.Complete FROM tblProjectChangeOrders RIGHT JOIN (SELECT tblSOV.SOV_Acronym, tblLabor.Labor_Desc, tblSOV.CO_ID, tblSOV.Lab_Phase, tblSOV.Complete FROM tblLabor RIGHT JOIN (SELECT  tblProjectLabor.Labor_ID, tblProjectLabor.Lab_Phase, tblSOV.ProjSOV_ID, tblSOV.SOV_Acronym, tblSOV.CO_ID, tblProjectLabor.Complete FROM tblProjectLabor RIGHT JOIN (SELECT * FROM tblProjectSOV WHERE Project_ID = " + selectedProjectID.ToString() + ") AS tblSOV ON tblProjectLabor.ProjSOV_ID = tblSOV.ProjSOV_ID) AS tblSOV ON tblSOV.Labor_ID = tblLabor.Labor_ID) AS tblLab ON tblLab.CO_ID = tblProjectChangeOrders.CO_ID; ";
            rowIndex = 0;

            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            rowCount = ds.Tables[0].Rows.Count; // number of rows

            TrackingReportGrid2.Children.Clear();
            sb_trackLaborReports.Clear();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string sovAcronym = "";
                string laborDesc = "";
                string coItemNo = "";
                string labPhase = "";
                bool laborComplete = false;


                if (!row.IsNull("SOV_Acronym"))
                    sovAcronym = row["SOV_Acronym"].ToString();
                if (!row.IsNull("Labor_Desc"))
                    laborDesc = row["Labor_Desc"].ToString();
                if (!row.IsNull("CO_ItemNo"))
                    coItemNo = row["CO_ItemNo"].ToString();
                if (!row.IsNull("Lab_Phase"))
                    labPhase = row["Lab_Phase"].ToString();
                if (!row.IsNull("Complete"))
                    laborComplete = row.Field<Boolean>("Complete");

                sb_trackLaborReports.Add(new TrackLaborReport
                {
                    //ProjMat = int.Parse(selectedId),
                    SovAcronym = sovAcronym,
                    LaborDesc = laborDesc,
                    CoItemNo = coItemNo,
                    LabPhase = labPhase,
                    Complete = laborComplete
                });

            }

            for (int i = 0; i < rowCount; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = GridLength.Auto;
                TrackingReportGrid2.RowDefinitions.Add(rowDef);
            }

            foreach (TrackLaborReport laborReport in sb_trackLaborReports)
            {
                Grid secondGrid = new Grid();

                Label SovAcronym_Label = new Label();
                Label LaborDesc_Label = new Label();
                Label ChangeOrder_Label = new Label();
                Label Phase_Label = new Label();
                CheckBox Complete_CheckBox = new CheckBox();

                SovAcronym_Label.Content = laborReport.SovAcronym;
                LaborDesc_Label.Content = laborReport.LaborDesc;
                ChangeOrder_Label.Content = laborReport.CoItemNo;
                Phase_Label.Content = laborReport.LabPhase;
                Complete_CheckBox.IsChecked = laborReport.Complete;

                Grid.SetColumn(SovAcronym_Label, 0);
                Grid.SetColumn(LaborDesc_Label, 1);
                Grid.SetColumn(ChangeOrder_Label, 2);
                Grid.SetColumn(Phase_Label, 3);
                Grid.SetColumn(Complete_CheckBox, 4);

                for (int i = 0; i < 5; i++)
                {
                    secondGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }

                secondGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                secondGrid.ColumnDefinitions[1].Width = new GridLength(2, GridUnitType.Star);
                secondGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                secondGrid.ColumnDefinitions[3].Width = new GridLength(1, GridUnitType.Star);
                secondGrid.ColumnDefinitions[4].Width = new GridLength(1, GridUnitType.Star);

                secondGrid.Children.Add(SovAcronym_Label);
                secondGrid.Children.Add(LaborDesc_Label);
                secondGrid.Children.Add(ChangeOrder_Label);
                secondGrid.Children.Add(Phase_Label);
                secondGrid.Children.Add(Complete_CheckBox);

                if (rowIndex % 2 != 0)
                {
                    secondGrid.Background = new SolidColorBrush(Colors.Gainsboro);
                }
                Grid.SetRow(secondGrid, rowIndex);

                TrackingReportGrid2.Children.Add(secondGrid);

                rowIndex += 1;
            }

            // WOListView
            sqlquery = "SELECT tblWO.WO_ID, tblWO.Wo_Nbr, tblInstallCrew.Crew_ID, tblInstallCrew.Crew_Name, tblWO.SchedStartDate, tblWO.SchedComplDate, tblWO.Sup_ID, tblWO.Date_Started, tblWO.Date_Completed FROM tblInstallCrew RIGHT JOIN (SELECT * FROM tblWorkOrders WHERE Project_ID = " + selectedProjectID.ToString() + ") AS tblWO ON tblInstallCrew.Crew_ID = tblWO.Crew_ID; ";

            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            rowCount = ds.Tables[0].Rows.Count; // number of rows

            sb_workOrders.Clear();

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
                if(!row.IsNull("Wo_Nbr"))
                    woNbr = int.Parse(row["Wo_Nbr"].ToString());
                if(!row.IsNull("Crew_ID"))
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
            WOListView.ItemsSource = sb_workOrders;
            WOListView.SelectedValuePath = "WoID";

            // Installation Notes
            sqlquery = "SELECT * FROM tblInstallNotes WHERE Project_ID = " + selectedProjectID.ToString();

            cmd = new SqlCommand(sqlquery, con);
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

                for(int i = 0; i<2; i++)
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
            sqlquery = "SELECT tblProj.Job_No, Contractnumber, ChangeOrder, ChangeOrderNo, DateRecD, DateProcessed, AmtOfcontract, SignedoffbySales, Signedoffbyoperations, GivenAcctingforreview, Givenforfinalsignature, Scope, ReturnedVia, ReturnedtoDawn, Comments FROM tblSC RIGHT JOIN (SELECT Project_ID, Job_No FROM tblProjects WHERE Project_ID = " + selectedProjectID.ToString() +") AS tblProj ON tblSC.ProjectID = tblProj.Project_ID";

            cmd = new SqlCommand(sqlquery, con);
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

            foreach(DataRow row in ds.Tables[0].Rows)
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
                if(!row.IsNull("Contractnumber"))
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

            foreach(Contract contract in sb_contract)
            {
                Grid firstGrid = new Grid();
                Grid secondGrid = new Grid();
                Grid thirdGrid = new Grid();

                for(int i=0; i<5; i++)
                {
                    RowDefinition rowDef1 = new RowDefinition();
                    RowDefinition rowDef2 = new RowDefinition();
                    rowDef1.Height = GridLength.Auto;
                    rowDef2.Height = GridLength.Auto;
                    firstGrid.RowDefinitions.Add(rowDef1);
                    secondGrid.RowDefinitions.Add(rowDef2);
                }
                for(int i=0; i<4; i++)
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

            cmd = new SqlCommand(sqlquery, con);
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

            foreach(CIP cip in sb_cip)
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

            // ProjMatList
            sqlquery = " SELECT tblWorkOrdersMat.Mat_Qty, tblMat.* FROM tblWorkOrdersMat RIGHT JOIN ( SELECT tblProjectSOV.SOV_Acronym, tblMat.* FROM tblProjectSOV RIGHT JOIN ( SELECT tblMat.*, tblProjectMaterialsShip.Qty_Recvd, tblProjectMaterialsShip.ProjMS_ID FROM tblProjectMaterialsShip RIGHT JOIN ( SELECT tblManufacturers.Manuf_Name, tblMat.* FROM tblManufacturers RIGHT JOIN (  SELECT tblMaterials.Material_Desc,tblMat.* FROM tblMaterials RIGHT JOIN (SELECT tblProjectMaterialsTrack.MatReqdDate, tblProjectMaterialsTrack.TakeFromStock, tblMat.*, tblProjectMaterialsTrack.ProjMT_ID, tblProjectMaterialsTrack.Manuf_ID, tblProjectMaterialsTrack.Qty_Ord  FROM tblProjectMaterialsTrack RIGHT JOIN (SELECT Project_ID, ProjMat_ID, ProjSOV_ID ,Material_ID, Qty_Reqd FROM tblProjectMaterials WHERE Project_ID = "+ selectedProjectID.ToString() + ") AS tblMat ON tblProjectMaterialsTrack.ProjMat_ID = tblMat.ProjMat_ID) AS tblMat ON tblMaterials.Material_ID = tblMat.Material_ID) AS tblMat ON tblMat.Manuf_ID = tblManufacturers.Manuf_ID) AS tblMat ON tblMat.ProjMT_ID = tblProjectMaterialsShip.ProjMT_ID) AS tblMat ON tblMat.ProjSOV_ID = tblProjectSOV.ProjSOV_ID) AS tblMat ON tblWorkOrdersMat.ProjMS_ID = tblMat.ProjMS_ID ORDER BY Material_Desc";

            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            rowCount = ds.Tables[0].Rows.Count; // number of rows
            rowIndex = 0;

            sb_projectWorkOrder.Clear();
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

            ProjectWorkOrderList.ItemsSource = sb_projectWorkOrder;
            ProjectWorkOrderList.SelectedValuePath = "ProjectID";

            // Project Labor List
            sqlquery = " SELECT CO_ItemNo, tblLab.* FROM tblProjectChangeOrders RIGHT JOIN (SELECT tblProjectSOV.SOV_Acronym, tblProjectSOV.CO_ID, tblLab.Labor_Desc, tblLab.Qty_Reqd, tblLab.UnitPrice, tblLab.Lab_Phase FROM tblProjectSOV RIGHT JOIN ( SELECT tblLabor.Labor_Desc, tblLab.*  FROM tblLabor RIGHT JOIN ( SELECT * FROM tblProjectLabor WHERE Project_ID = "+ selectedProjectID.ToString() + ") AS tblLab ON tblLabor.Labor_ID = tblLab.Labor_ID) AS tblLab ON tblProjectSOV.ProjSOV_ID = tblLab.ProjSOV_ID) AS tblLab ON tblProjectChangeOrders.CO_ID = tblLab.CO_ID ORDER BY tblLab.SOV_Acronym";

            cmd = new SqlCommand(sqlquery, con);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            rowCount = ds.Tables[0].Rows.Count; // number of rows
            rowIndex = 0;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string _sovAcronym = "";
                string _labor = "";
                int _qtyReqd = 0;
                double _unitPrice = 0;
                //int _total = 0;
                int _changeOrder = 0;
                string _phase = "";
                if (!row.IsNull("SOV_Acronym"))
                    _sovAcronym = row["SOV_Acronym"].ToString();
                if (!row.IsNull("Labor_Desc"))
                    _labor = row["Labor_Desc"].ToString();
                if (!row.IsNull("Qty_Reqd"))
                    _qtyReqd =  int.Parse(row["Qty_Reqd"].ToString());
                if (!row.IsNull("UnitPrice"))
                    Console.WriteLine(row["UnitPrice"].GetType());
                    //_unitPrice = double.Parse(row["UnitPrice"].ToString());
                if (!row.IsNull("CO_ItemNo"))
                    _changeOrder = int.Parse(row["CO_ItemNo"].ToString());
                if (!row.IsNull("Lab_Phase"))
                    _phase = row["Lab_Phase"].ToString();
                sb_projectLabor.Add(new ProjectLabor
                {
                    ProjectID = selectedProjectID,
                    SovAcronym = _sovAcronym,
                    Labor = _labor,
                    QtyReqd = _qtyReqd,
                    UnitPrice = _unitPrice,
                    ChangeOrder = _changeOrder,
                    Phase = _phase
                });
            }
            ProjectLaborList.ItemsSource = sb_projectLabor;
            ProjectLaborList.SelectedValuePath = "ProjectID";
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

                        //TrackingGrid
                        for(int i=0; i<21; i++)
                        {
                            secondGrid.ColumnDefinitions.Add(new ColumnDefinition());
                        }

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
                        TextBox Gap_TB = new TextBox();
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

                        MatlReqd_Date.Text = track.MatReqdDate.Date.ToShortDateString();
                        QtyOrd_TextBox.Text = track.QtyOrd;
                        Manufacturer_Combo.ItemsSource = ProjectVM.Manufacturers;
                        Manufacturer_Combo.DisplayMemberPath = "ManufacturerName";
                        Manufacturer_Combo.SelectedValuePath = "ID";
                        Manufacturer_Combo.SelectedValue = track.ManufacturerID;

                        Stock_CheckBox.IsChecked = track.TakeFromStock;
                        LeadTime_TextBox.Text = track.LeadTime.ToString();
                        ManuOrd_TextBox.Text = track.ManuOrderNo.ToString();
                        Gap_TB.Text = track.PoNumber;
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
                        Grid.SetColumn(Gap_TB, 6);
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
                        secondGrid.Children.Add(Gap_TB);
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

                        for (int i = 0; i < 16; i++)
                        {
                            secondGrid.ColumnDefinitions.Add(new ColumnDefinition());
                        }

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
                        FreightCo_ComBo.ItemsSource = ProjectVM.FreightCos;
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

        private void SelectWorkOrdersList(object sender, SelectionChangedEventArgs e)
        {
            if (WOListView.SelectedItem != null)
            {
                WorkOrder selectedItem = (WorkOrder)WOListView.SelectedItem;
                //string selectedId = TrackShipList.SelectedValue.ToString();
                int woNumber = selectedItem.WoNumber;
                int woID = selectedItem.WoID;
                int crewID = selectedItem.CrewID;
                DateTime schedStartDate = selectedItem.SchedStartDate;
                DateTime schedComplDate = selectedItem.SchedComplDate;
                int suptID = selectedItem.SuptID;
                DateTime startDate = selectedItem.DateStarted;
                DateTime complDate = selectedItem.DateCompleted;

                WoNbr_TextBox.Text = woNumber.ToString();
                Crew_ComBo.SelectedValue = crewID;
                SchedStartDate_Datepicker.Text = schedStartDate.ToString();
                SchedComplDate_Datepicker.Text = schedComplDate.ToString();
                Supt_ComBo.SelectedValue = suptID;
                WoStartDate_Datepicker.Text = startDate.ToString();
                WoComplDate_Datepicker.Text = complDate.ToString();
                

                int rowCount = 0;
                int rowIndex = 0;
                sqlquery = "SELECT * FROM tblNotes WHERE Notes_PK = " + woID + " AND Notes_PK_Desc = 'WorkOrder';";
                cmd = new SqlCommand(sqlquery, con);
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
                    TextBox Note_TextBox = new TextBox();

                    DateAdded_Label.Content= note.NotesDateAdded.ToString();
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
