using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp.Model;
using WpfApp.ViewModel.Dialog;

namespace WpfApp.View.Dialog
{
    /// <summary>
    /// Interaction logic for AddNewCumerWin.xaml
    /// </summary>
    public partial class NewCustomerDialog : Window
    {
        private NewCustomerViewModel NewCustomerVM;

        public NewCustomerDialog()
        {
            InitializeComponent();
            NewCustomerVM = new NewCustomerViewModel();
            this.DataContext = NewCustomerVM;
        }

        private void Close_NewCustomerDlg(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
