using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp.Model;
using WpfApp.ViewModel;
using WpfApp.Utils;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for ColseOutView.xaml
    /// </summary>
    public partial class CloseOutView : Page
    {
        private DatabaseConnection dbConnection;
        private SqlCommand cmd;
        private SqlDataAdapter sda;
        private DataSet ds;
        private string sqlquery;

        private CloseOutViewModel CloseOutVM;

        public CloseOutView()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            dbConnection.Open();
            CloseOutVM = new CloseOutViewModel();
            this.DataContext = CloseOutVM;
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
            int selectedProjectID = selectedItem.ID;

            sqlquery = "SELECT * FROM tblWarranties WHERE ProjectId = " + selectedProjectID.ToString();

            int rowCount = 0;
            int rowIndex = 0;

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            rowCount = ds.Tables[0].Rows.Count; // number of rows

            CloseOutGrid.Children.Clear();

            for (int i = 0; i < rowCount; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = GridLength.Auto;
                CloseOutGrid.RowDefinitions.Add(rowDef);
            }

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

            foreach (Warranty warranty in sb_warranties)
            {
                Grid ItemGrid = new Grid();
                Grid firstGrid = new Grid();
                Grid secondGrid = new Grid();
                Grid thirdGrid = new Grid();

                ItemGrid.Margin = new Thickness(4, 4, 4, 4);

                for (int i = 0; i < 3; i++)
                {
                    RowDefinition rowDef1 = new RowDefinition();
                    RowDefinition rowDef2 = new RowDefinition();
                    RowDefinition rowDef3 = new RowDefinition();
                    rowDef1.Height = GridLength.Auto;
                    rowDef2.Height = GridLength.Auto;
                    rowDef3.Height = GridLength.Auto;
                    firstGrid.RowDefinitions.Add(rowDef1);
                    secondGrid.RowDefinitions.Add(rowDef2);
                    thirdGrid.RowDefinitions.Add(rowDef3);
                }

                ColumnDefinition colDef1 = new ColumnDefinition();
                ColumnDefinition colDef2 = new ColumnDefinition();
                colDef1.Width = new GridLength(2, GridUnitType.Star);
                colDef2.Width = new GridLength(3, GridUnitType.Star);

                ColumnDefinition colDef3 = new ColumnDefinition();
                ColumnDefinition colDef4 = new ColumnDefinition();
                colDef3.Width = new GridLength(2, GridUnitType.Star);
                colDef4.Width = new GridLength(3, GridUnitType.Star);

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

                for(int i= 0; i < 3; i++)
                {
                    ColumnDefinition colDef = new ColumnDefinition();
                    ItemGrid.ColumnDefinitions.Add(colDef);
                }

                // first column
                Label DocuReq_LB = new Label();
                Label RecdDate_LB = new Label();
                Label ComlDate_LB = new Label();
                TextBox DocuReq_TB = new TextBox();
                DatePicker RecdDate_DP = new DatePicker();
                DatePicker ComlDate_DP = new DatePicker();

                DocuReq_LB.Content = "Document Request";
                RecdDate_LB.Content = "Request Recd Date";
                ComlDate_LB.Content = "Substantial Completion Date";

                DocuReq_TB.Text = warranty.DocuReq;
                DocuReq_TB.AcceptsReturn = true;
                DocuReq_TB.Height = 40;
                DocuReq_TB.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

                RecdDate_DP.DisplayDate = warranty.DateRecd;
                ComlDate_DP.DisplayDate = warranty.ComplDate;

                // second column
                Label NumOfCopy_LB = new Label();
                Label ContactName_LB = new Label();
                Label SubmVia_LB = new Label();
                TextBox NumOfCopy_TB = new TextBox();
                TextBox ContactName_TB = new TextBox();
                ComboBox SubmVia_CB = new ComboBox();

                NumOfCopy_LB.Content = "Processed Date";
                ContactName_LB.Content = "Contract Amt";
                SubmVia_LB.Content = "Signed off by Sales";

                NumOfCopy_TB.Text = warranty.NumOfCopy.ToString();
                ContactName_TB.Text = warranty.ContractName;
                SubmVia_CB.ItemsSource = CloseOutVM.ReturnedViaNames;
                SubmVia_CB.DisplayMemberPath = "ReturnedViaName";
                SubmVia_CB.SelectedValuePath = "ReturnedViaName";
                SubmVia_CB.SelectedValue = warranty.SubmVia;

                // third column
                Label DateSent_LB = new Label();
                Label SentVia_LB = new Label();
                Label Notes_LB = new Label();
                DatePicker DateSent_DP = new DatePicker();
                ComboBox SentVia_CB = new ComboBox();
                TextBox Notes_TB = new TextBox();

                DateSent_LB.Content = "Date Sent";
                SentVia_LB.Content = "Sent Via";
                Notes_LB.Content = "Notes";

                DateSent_DP.DisplayDate = warranty.DateSent;
                SentVia_CB.ItemsSource = CloseOutVM.ReturnedViaNames;
                SentVia_CB.DisplayMemberPath = "ReturnedViaName";
                SentVia_CB.SelectedValuePath = "ReturnedViaName";
                SentVia_CB.SelectedValue = warranty.SentVia;
                Notes_TB.AcceptsReturn = true;
                Notes_TB.Height = 40;
                Notes_TB.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
               

                // Grid set for thirdGrid
                Grid.SetColumn(DateSent_LB, 0);
                Grid.SetColumn(SentVia_LB, 0);
                Grid.SetColumn(Notes_LB, 0);

                Grid.SetColumn(DateSent_DP, 1);
                Grid.SetColumn(Notes_TB, 1);
                Grid.SetColumn(SentVia_CB, 1);

                Grid.SetRow(DateSent_LB, 0);
                Grid.SetRow(SentVia_LB, 1);
                Grid.SetRow(Notes_LB, 2);
                Grid.SetRow(DateSent_DP, 0);
                Grid.SetRow(SentVia_CB, 1);
                Grid.SetRow(Notes_TB, 2);

                thirdGrid.Children.Add(DateSent_LB);
                thirdGrid.Children.Add(SentVia_LB);
                thirdGrid.Children.Add(Notes_LB);
                thirdGrid.Children.Add(DateSent_DP);
                thirdGrid.Children.Add(Notes_TB);
                thirdGrid.Children.Add(SentVia_CB);
                

                // Grid set for secondGrid
                Grid.SetColumn(NumOfCopy_LB, 0);
                Grid.SetColumn(ContactName_LB, 0);
                Grid.SetColumn(SubmVia_LB, 0);

                Grid.SetColumn(NumOfCopy_TB, 1);
                Grid.SetColumn(ContactName_TB, 1);
                Grid.SetColumn(SubmVia_CB, 1);

                Grid.SetRow(NumOfCopy_LB, 0);
                Grid.SetRow(NumOfCopy_TB, 0);
                Grid.SetRow(ContactName_LB, 1);
                Grid.SetRow(ContactName_TB, 1);
                Grid.SetRow(SubmVia_LB, 2);
                Grid.SetRow(SubmVia_CB, 2);

                secondGrid.Children.Add(NumOfCopy_LB);
                secondGrid.Children.Add(NumOfCopy_TB);
                secondGrid.Children.Add(ContactName_LB);
                secondGrid.Children.Add(ContactName_TB);
                secondGrid.Children.Add(SubmVia_LB);
                secondGrid.Children.Add(SubmVia_CB);

                // Grid set for firstGrid
                Grid.SetColumn(DocuReq_LB, 0);
                Grid.SetColumn(RecdDate_LB, 0);
                Grid.SetColumn(ComlDate_LB, 0);

                Grid.SetColumn(DocuReq_TB, 1);
                Grid.SetColumn(RecdDate_DP, 1);
                Grid.SetColumn(ComlDate_DP, 1);

                Grid.SetRow(DocuReq_LB, 0);
                Grid.SetRow(DocuReq_TB, 0);
                Grid.SetRow(RecdDate_LB, 1);
                Grid.SetRow(RecdDate_DP, 1);
                Grid.SetRow(ComlDate_LB, 2);
                Grid.SetRow(ComlDate_DP, 2);

                firstGrid.Children.Add(DocuReq_LB);
                firstGrid.Children.Add(DocuReq_TB);
                firstGrid.Children.Add(RecdDate_LB);
                firstGrid.Children.Add(RecdDate_DP);
                firstGrid.Children.Add(ComlDate_LB);
                firstGrid.Children.Add(ComlDate_DP);

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

                ItemGrid.Children.Add(firstGrid);
                ItemGrid.Children.Add(secondGrid);
                ItemGrid.Children.Add(thirdGrid);

                Grid.SetRow(ItemGrid, rowIndex);

                CloseOutGrid.Children.Add(ItemGrid);
                rowIndex += 1;
            }
        }
    }
}
