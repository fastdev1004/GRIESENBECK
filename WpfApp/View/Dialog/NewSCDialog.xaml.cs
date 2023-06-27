using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp.ViewModel.Dialog;

namespace WpfApp.View.Dialog
{
    /// <summary>
    /// Interaction logic for NewSCDialog.xaml
    /// </summary>
    public partial class NewSCDialog : Window
    {
        private NewSCViewModel NewScVM;
        public int CustomerID { get; set; }

        public NewSCDialog()
        {
            InitializeComponent();
            NewScVM = new NewSCViewModel();
            NewScVM.CustomerID = CustomerID;
            this.DataContext = NewScVM;
            Loaded += LoadPage;
        }

        private void LoadPage(object sender, RoutedEventArgs e)
        {
            NewScVM.CustomerID = CustomerID;
            this.DataContext = NewScVM;
        }

        public void Close_NewSCDlg(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
