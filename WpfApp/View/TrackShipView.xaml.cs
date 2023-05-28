using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp.Model;
using WpfApp.ViewModel;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for TrackShipView.xaml
    /// </summary>
    public partial class TrackShipView : Page
    {
        private string sqlquery;
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataAdapter sda;
        private DataSet ds;

        private  TrackShipViewModel TrackShipVM;

        public TrackShipView()
        {
            InitializeComponent();
            TrackShipVM = new TrackShipViewModel();
            con = TrackShipVM.con;
            this.DataContext = TrackShipVM;
        }

        private void goBack(object sender, RoutedEventArgs e)
        {
            cmd.Dispose();
            con.Close();
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }

        private void SelectTrackShipList(object sender, SelectionChangedEventArgs e)
        {
            if (TrackShipList.SelectedItem != null)
            {
                string selectedId = TrackShipList.SelectedValue.ToString();
                Console.WriteLine(selectedId);
                Console.WriteLine("trackship");
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

                TrackingGrid.Children.Clear();
                ObservableCollection<ProjectMatTracking> sb_projectMatTrackings = new ObservableCollection<ProjectMatTracking>();

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
                        for (int i = 0; i < 21; i++)
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
                        Manufacturer_Combo.ItemsSource = TrackShipVM.Manufacturers;
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

                ObservableCollection<ProjectMatShip> sb_projectMtShip = new ObservableCollection<ProjectMatShip>();

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
                        FreightCo_ComBo.ItemsSource = TrackShipVM.FreightCos;
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
