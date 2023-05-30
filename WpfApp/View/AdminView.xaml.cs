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
    /// Interaction logic for AdministrationView.xaml
    /// </summary>
    public partial class AdminView : Page
    {
        private AdminViewModel AdminVM;

        public AdminView()
        {
            InitializeComponent();
            AdminVM = new AdminViewModel();
            
            this.DataContext = AdminVM;
        }

        private void goBack(object sender, RoutedEventArgs e)
        {
            //con.Close();
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }
    }
}
