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
    }
}
