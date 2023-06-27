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
    /// Interaction logic for NewArchitectDialog.xaml
    /// </summary>
    public partial class NewArchitectDialog : Window
    {
        private NewArchitectViewModel NewArchitectVM;
        public NewArchitectDialog()
        {
            InitializeComponent();
            NewArchitectVM = new NewArchitectViewModel();
            this.DataContext = NewArchitectVM;
        }

        private void Close_NewArchitectDlg(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
