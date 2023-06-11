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

            this.NavigationService.Navigate(new Uri("View/ReportView.xaml", UriKind.Relative));
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
                int manufID = loadContext.ManufID;
                int architectID = loadContext.ArchitectID;
                int matID = loadContext.MaterialID;
                int crewID = loadContext.CrewID;
                int complete = loadContext.Complete;
                DateTime dateFrom = loadContext.DateFrom;
                DateTime toDate = loadContext.ToDate;
                string keyword = loadContext.Keyword;

                ReportDetailVM.ProjectID = projectID;
                ReportDetailVM.SelectedJobNo = jobNo;
                ReportDetailVM.SelectedSalesmanID = salesmanID;
                ReportDetailVM.SelectedArchRepID = archRepID;
                ReportDetailVM.SelectedManufID = manufID;
                ReportDetailVM.SelectedCustomerID = customerID;
                ReportDetailVM.SelectedArchitectID = architectID;
                ReportDetailVM.SelectedMatID = matID;
                ReportDetailVM.SelectedCrewID = crewID;
                ReportDetailVM.SelectedDateFrom = dateFrom;
                ReportDetailVM.SelectedToDate = toDate;
                ReportDetailVM.SelectedComplete = complete;
                ReportDetailVM.Keyword = keyword;
                ReportDetailVM.ReportID = reportID;

                this.DataContext = ReportDetailVM;
            }
        }
    }
}
