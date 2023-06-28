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
    /// Interaction logic for NewMaterialDialog.xaml
    /// </summary>
    public partial class NewMaterialDialog : Window
    {
        private NewMaterialViewModel NewMaterialVM;
        public NewMaterialDialog()
        {
            InitializeComponent();
            NewMaterialVM = new NewMaterialViewModel();
            this.DataContext = NewMaterialVM;
        }

        private void Close_Dlg(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
