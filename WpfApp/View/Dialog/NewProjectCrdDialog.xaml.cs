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
    /// Interaction logic for NewProjectCrdDialog.xaml
    /// </summary>
    public partial class NewProjectCrdDialog : Window
    {
        private NewProjectCrdViewModel NewProjectCrdVM;

        public NewProjectCrdDialog()
        {
            InitializeComponent();
            NewProjectCrdVM = new NewProjectCrdViewModel();
            this.DataContext = NewProjectCrdVM;
        }

        private void Close_NewProjectCrdDlg(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
