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

            //BitmapImage bitmap = new BitmapImage();
            //bitmap.BeginInit();
            //bitmap.UriSource = new Uri("/images/logo.png");
            //bitmap.EndInit();
            //logoImage.Source = bitmap;
        }

        private void close_win(object sender, RoutedEventArgs e)
        {
            var w = Application.Current.Windows[0];
            w.Close();
        }

        private void navigate_project(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("View/ProjectView.xaml", UriKind.Relative));
        }
    }
}
