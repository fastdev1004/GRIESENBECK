using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp.Model;
using WpfApp.Utils;
using WpfApp.ViewModel;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for WorkOrderView.xaml
    /// </summary>
    /// 
    public partial class WorkOrderView : Page
    {
        private DatabaseConnection dbConnection;
        private NoteHelper noteHelper;

        private ObservableCollection<Note> sb_notes;
        private WorkOrderViewModel WorkOrderVM;

        public WorkOrderView()
        {
            InitializeComponent();
            WorkOrderVM = new WorkOrderViewModel();
            sb_notes = new ObservableCollection<Note>();
            dbConnection = new DatabaseConnection();
            noteHelper = new NoteHelper();
            dbConnection.Open();
            this.DataContext = WorkOrderVM;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }

        private void WorkOrderNote_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox NotesNote = sender as TextBox;
            if (NotesNote != null)
            {
                DataGrid dataGrid = noteHelper.FindDataGrid(NotesNote);

                if (dataGrid != null)
                {
                    TextBox lastRowTextBox = noteHelper.GetLastRowTextBox(dataGrid);
                    if (lastRowTextBox != null)
                    {
                        string lastRowText = lastRowTextBox.Text;
                        if (!string.IsNullOrEmpty(lastRowText))
                        {
                            ObservableCollection<Note> notes = WorkOrderVM.WorkOrderNotes;
                            Grid parentGrid = NotesNote.Parent as Grid;
                            Grid grandParentGrid = parentGrid.Parent as Grid;
                            Grid firstChildGrid = grandParentGrid.Children[0] as Grid;
                            TextBlock NotesDateAdded = firstChildGrid.Children[0] as TextBlock;
                            TextBlock NoteUserName = firstChildGrid.Children[1] as TextBlock;
                            NotesDateAdded.Text = DateTime.Now.ToString();
                            NoteUserName.Text = "smile";

                            Note item = new Note();
                            notes.Add(item);
                            e.Handled = true;
                        }
                    }
                }
            }
        }
    }
}
