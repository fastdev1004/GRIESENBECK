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
    /// Interaction logic for NewArchRepDialog.xaml
    /// </summary>
    public partial class NewArchRepDialog : Window
    {
        private NewArchRepViewModel NewArchRepVM;

        public NewArchRepDialog()
        {
            InitializeComponent();
            NewArchRepVM = new NewArchRepViewModel();
            this.DataContext = NewArchRepVM;
        }

        private void Close_NewArchRepDlg(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
