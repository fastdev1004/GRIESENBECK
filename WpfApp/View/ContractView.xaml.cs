using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp.Model;
using WpfApp.Utils;
using WpfApp.ViewModel;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for ContractView.xaml
    /// </summary>
    public partial class ContractView : Page
    {
        private DatabaseConnection dbConnection;
        private SqlCommand cmd;
        private SqlDataAdapter sda;
        private DataSet ds;
        private string sqlquery;

        private ContractViewModel ContractVM;

        public ContractView()
        {
            InitializeComponent();
            ContractVM = new ContractViewModel();
            dbConnection = new DatabaseConnection();
            dbConnection.Open();
            this.DataContext = ContractVM;
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


            sqlquery = "SELECT tblProj.Job_No, Contractnumber, ChangeOrder, ChangeOrderNo, DateRecD, DateProcessed, AmtOfcontract, SignedoffbySales, Signedoffbyoperations, GivenAcctingforreview, Givenforfinalsignature, Scope, ReturnedVia, ReturnedtoDawn, Comments FROM tblSC RIGHT JOIN (SELECT Project_ID, Job_No FROM tblProjects WHERE Project_ID = " + selectedProjectID.ToString() + ") AS tblProj ON tblSC.ProjectID = tblProj.Project_ID";

            int rowCount = 0;
            int rowIndex = 0;

            cmd = new SqlCommand(sqlquery, dbConnection.Connection);
            sda = new SqlDataAdapter(cmd);
            ds = new DataSet();
            sda.Fill(ds);
            rowCount = ds.Tables[0].Rows.Count; // number of rows

            ContractGrid.Children.Clear();

            for (int i = 0; i < rowCount; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = GridLength.Auto;
                ContractGrid.RowDefinitions.Add(rowDef);
            }

            ObservableCollection<Contract> sb_contract = new ObservableCollection<Contract>();

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
                ReturnedVia_CB.ItemsSource = ContractVM.ReturnedViaNames;
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
        }
    }
}
