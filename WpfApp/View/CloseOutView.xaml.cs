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
    }
}
