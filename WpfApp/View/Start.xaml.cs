using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Page
    {
        public Start()
        {
            InitializeComponent();
        }

        private void close_win(object sender, RoutedEventArgs e)
        {
            var w = Application.Current.Windows[0];
            w.Close();
        }

        private void navigateProject(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("View/ProjectView.xaml", UriKind.Relative));
        }

        private void navigateWorkOrder(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("View/WorkOrderView.xaml", UriKind.Relative));
        }
        
        private void navigateTrackShip(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("View/TrackShipView.xaml", UriKind.Relative));
        }
    }
}
