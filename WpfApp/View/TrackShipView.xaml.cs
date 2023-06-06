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
using WpfApp.Utils;
using WpfApp.ViewModel;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for TrackShipView.xaml
    /// </summary>
    public partial class TrackShipView : Page
    {
        private DatabaseConnection dbConnection;
        private  TrackShipViewModel TrackShipVM;

        public TrackShipView()
        {
            InitializeComponent();
            TrackShipVM = new TrackShipViewModel();
            dbConnection = new DatabaseConnection();
            dbConnection.Open();
            this.DataContext = TrackShipVM;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            dbConnection.Close();
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }
    }
}
