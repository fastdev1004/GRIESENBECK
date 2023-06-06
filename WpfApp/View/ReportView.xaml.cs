using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp.ViewModel;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for ReportingView.xaml
    /// </summary>
    public partial class ReportView : Page
    {
        private ReportViewModel ReportVM;
        private ReportDetailViewModel ReportDetailVM;

        public ReportView()
        {
            InitializeComponent();
            ReportVM = new ReportViewModel();
            ReportDetailVM = new ReportDetailViewModel();
            this.DataContext = ReportVM;
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }

        private void GoReportDetail(object sender, RoutedEventArgs e)
        {
            //dbConnection.Close();
            int reportID = ReportVM.ReportID;
            int projectID = ReportVM.ProjectID;
            string jobNo = ReportVM.JobNo;
            int salesmanID = ReportVM.SalesmanID;
            int archRepID = ReportVM.ArchRepID;
            int customerID = ReportVM.CustomerID;
            int manufID = ReportVM.ManufID;
            int architectID = ReportVM.ArchitectID;
            int materialID = ReportVM.MaterialID;
            int crewID = ReportVM.CrewID;
            //int complete = ReportVM.;
            DateTime dateFrom = ReportVM.DateFrom;
            DateTime toDate = ReportVM.ToDate;
            string keyWord = ReportVM.Keyword;

            ReportDetailVM.ReportID = reportID;
            ReportDetailVM.ProjectID = projectID;
            ReportDetailVM.JobNo = jobNo;
            ReportDetailVM.SalesmanID = salesmanID;
            ReportDetailVM.ArchRepID = archRepID;
            ReportDetailVM.CustomerID = customerID;
            ReportDetailVM.ManufID = manufID;
            ReportDetailVM.ArchitectID = architectID;
            ReportDetailVM.MaterialID = materialID;
            ReportDetailVM.CrewID = crewID;
            ReportDetailVM.DateFrom = dateFrom;
            ReportDetailVM.ToDate = toDate;
            ReportDetailVM.Keyword = keyWord;
            ReportDetailView reportDetailPage = new ReportDetailView();
            reportDetailPage.DataContext = ReportDetailVM;

            Console.WriteLine(reportID);
            Console.WriteLine("Go Report Detail");
            if (reportID != 0)
            {
                this.NavigationService.Navigate(reportDetailPage);
            }
            else
            {
                MessageBox.Show("This is not a valid report, please see your administrator");
            }
            //switch(reportID)
            //{
            //    case 13:
            //        break;
            //    default:
            //        break;
            //}
        }

        private void DatePicker_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;
            if (datePicker != null)
            {
                datePicker.SelectedDate = DateTime.Today;
            }
        }
    }
}
