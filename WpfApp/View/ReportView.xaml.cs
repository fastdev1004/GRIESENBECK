using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp.ViewModel;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for ReportingView.xaml
    /// </summary>
    public partial class ReportView : Page
    {
        private ReportViewModel ReportVM;

        public ReportView()
        {
            InitializeComponent();
            ReportVM = new ReportViewModel();
            this.DataContext = ReportVM;
        }
        private void goBack(object sender, RoutedEventArgs e)
        {
            //con.Close();
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }
    }
}
