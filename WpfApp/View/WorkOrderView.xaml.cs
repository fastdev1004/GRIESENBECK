using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using WpfApp.Model;
using WpfApp.Utils;
using WpfApp.ViewModel;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for WorkOrderView.xaml
    /// </summary>
    /// 
    public partial class WorkOrderView : Page
    {
        private DatabaseConnection dbConnection;
        private ObservableCollection<Note> sb_notes;

        private WorkOrderViewModel WorkOrderVM;

        public WorkOrderView()
        {
            InitializeComponent();
            WorkOrderVM = new WorkOrderViewModel();
            sb_notes = new ObservableCollection<Note>();
            dbConnection = new DatabaseConnection();
            dbConnection.Open();
            this.DataContext = WorkOrderVM;
        }

        private void goBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }
    }
}
