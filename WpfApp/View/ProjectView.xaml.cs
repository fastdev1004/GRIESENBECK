using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using WpfApp.ViewModel;
using WpfApp.Model;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Data;
using WpfApp.Controls;
using System.Drawing;
using System.Windows.Media;
using WpfApp.Utils;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for ViewProject.xaml
    /// </summary>
    public partial class ProjectView : Page
    {
        private DatabaseConnection dbConnection;
      
        private ProjectViewModel ProjectVM;
        public ProjectView()
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            ProjectVM = new ProjectViewModel();
            this.DataContext = ProjectVM;
            Loaded += LoadPage;
        }

        private void LoadPage(object sender, RoutedEventArgs e)
        {
            ProjectViewModel loadContext = DataContext as ProjectViewModel;

            if (loadContext != null)
            {
                ProjectVM = new ProjectViewModel();

                int projectID = loadContext.ProjectID;
                string navigationBackName = loadContext.NavigationBackName;

                ProjectVM.ProjectID = projectID;
                ProjectVM.NavigationBackName = navigationBackName;

                this.DataContext = ProjectVM;
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            //string navigationBackName = ProjectVM.NavigationBackName;
            //switch (navigationBackName)
            //{
            //    case "ReportDetailView":
            //        this.NavigationService.Navigate(new Uri("View/ReportDetailView.xaml", UriKind.Relative));
            //        break;
            //    default:
            //        this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
            //        break;
            //}
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }

        private void SelectPaymentComboBoxItem(object sender, MouseButtonEventArgs e)
        {
            ComboBoxItem item = sender as ComboBoxItem;
            Payment Payment = new Payment();
            Payment dataObject = item.DataContext as Payment;
            dataObject.IsChecked = !dataObject.IsChecked;
            
            e.Handled = true;
        }

        private void SelectPaymentCheckBox(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
