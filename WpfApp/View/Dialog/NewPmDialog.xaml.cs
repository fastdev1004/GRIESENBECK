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
    /// Interaction logic for NewPmDialog.xaml
    /// </summary>
    public partial class NewPmDialog : Window
    {
        private NewPmViewModel NewPmVM;
        public int CustomerID { get; set; }
        public NewPmDialog()
        {
            InitializeComponent();
            NewPmVM = new NewPmViewModel();
            this.DataContext = NewPmVM;
            Loaded += LoadPage;
        }

        private void LoadPage(object sender, RoutedEventArgs e)
        {
            NewPmVM.CustomerID = CustomerID;
            this.DataContext = NewPmVM;
        }

        private void Close_NewPmDlg(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
