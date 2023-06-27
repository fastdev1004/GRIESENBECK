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
    /// Interaction logic for NewEstimatorDialog.xaml
    /// </summary>
    public partial class NewEstimatorDialog : Window
    {
        private NewEstViewModel NewEstVM;

        public NewEstimatorDialog()
        {
            InitializeComponent();
            NewEstVM = new NewEstViewModel();
            this.DataContext = NewEstVM;
        }

        public void Close_NewEstDlg(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
