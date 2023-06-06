using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp.ViewModel;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for ReportDetailView.xaml
    /// </summary>
    public partial class ReportDetailView : Page
    {

        private ReportDetailViewModel ReportDetailVM;
        public ReportDetailView()
        {
            InitializeComponent();
            Loaded += LoadPage;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }

        private void LoadPage(object sender, RoutedEventArgs e)
        {
            ReportDetailViewModel loadContext = DataContext as ReportDetailViewModel;
          
            if (loadContext != null)
            {
                ReportDetailVM = new ReportDetailViewModel();

                int reportID = loadContext.ReportID;
                int projectID = loadContext.ProjectID;
                string jobNo = loadContext.JobNo;
                int salesmanID = loadContext.SalesmanID;
                int archRepID = loadContext.ArchRepID;
                int customerID = loadContext.CustomerID;
                int architectID = loadContext.ArchitectID;
                int crewID = loadContext.CrewID;
                DateTime dateFrom = loadContext.DateFrom;
                DateTime toDate = loadContext.ToDate;
                string keyword = loadContext.Keyword;

                ReportDetailVM.ReportID = reportID;
                ReportDetailVM.ProjectID = projectID;
                ReportDetailVM.SelectedJobNo = jobNo;
                ReportDetailVM.SelectedSalesmanID = salesmanID;
                ReportDetailVM.SelectedArchRepID = archRepID;
                ReportDetailVM.SelectedCustomerID = customerID;
                ReportDetailVM.SelectedArchitectID = architectID;
                ReportDetailVM.SelectedCrewID = crewID;
                ReportDetailVM.SelectedDateFrom = dateFrom;
                ReportDetailVM.SelectedToDate = toDate;
                ReportDetailVM.Keyword = keyword;

                Console.WriteLine(reportID);
                Console.WriteLine("reportID");
                ReportDetailVM.LoadData();
                this.DataContext = ReportDetailVM;
            }
        }
    }
}
