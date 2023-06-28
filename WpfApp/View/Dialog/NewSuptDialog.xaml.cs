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
    /// Interaction logic for NewSuptDialog.xaml
    /// </summary>
    public partial class NewSuptDialog : Window
    {
        private NewSuptViewModel NewSuptVM;
        public int CustomerID { get; set; }

        public NewSuptDialog()
        {
            InitializeComponent();
            NewSuptVM = new NewSuptViewModel();
            this.DataContext = NewSuptVM;
            Loaded += LoadPage;
        }

        private void LoadPage(object sender, RoutedEventArgs e)
        {
            NewSuptVM.CustomerID = CustomerID;
            this.DataContext = NewSuptVM;
        }

        private void Close_NewSuptDlg(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RemoveNoteItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = SuptNote_DataGrid.SelectedIndex;
            NewSuptVM.SuptNotes.RemoveAt(selectedIndex);
        }
    }
}
