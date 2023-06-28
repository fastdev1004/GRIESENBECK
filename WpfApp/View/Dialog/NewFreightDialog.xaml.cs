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
    /// Interaction logic for NewFreightDialog.xaml
    /// </summary>
    public partial class NewFreightDialog : Window
    {
        private NewFreightViewModel NewFreightVM;
        public NewFreightDialog()
        {
            InitializeComponent();
            NewFreightVM = new NewFreightViewModel();
            this.DataContext = NewFreightVM;
        }

        private void Close_Dlg(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
