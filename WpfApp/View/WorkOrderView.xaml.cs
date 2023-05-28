using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.Model;
using WpfApp.ViewModel;

namespace WpfApp.View
{
    /// <summary>
    /// Interaction logic for WorkOrderView.xaml
    /// </summary>
    /// 
    public partial class WorkOrderView : Page
    {
        private string sqlquery;
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataAdapter sda;
        private DataSet ds;
        private ObservableCollection<Note> sb_notes;
        private ObservableCollection<WorkOrder> sb_workOrders;

        private WorkOrderViewModel WorkOrderVM;

        public WorkOrderView()
        {
            InitializeComponent();
            WorkOrderVM = new WorkOrderViewModel();
            sb_workOrders = new ObservableCollection<WorkOrder>();
            sb_notes = new ObservableCollection<Note>();
            con = WorkOrderVM.con;
            con.Open();
            this.DataContext = WorkOrderVM;
        }

        private void goBack(object sender, RoutedEventArgs e)
        {
            cmd.Dispose();
            con.Close();
            this.NavigationService.Navigate(new Uri("View/Start.xaml", UriKind.Relative));
        }

        private void SelectWorkOrdersList(object sender, SelectionChangedEventArgs e)
        {
            if (WOListView.SelectedItem != null)
            {
                WorkOrder selectedItem = (WorkOrder)WOListView.SelectedItem;
                int woID = selectedItem.WoID;

                int rowCount = 0;
                int rowIndex = 0;
                sqlquery = "SELECT * FROM tblNotes WHERE Notes_PK = " + woID + " AND Notes_PK_Desc = 'WorkOrder';";
                cmd = new SqlCommand(sqlquery, con);
                sda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sda.Fill(ds);
                rowCount = ds.Tables[0].Rows.Count; // number of rows

                for (int i = 0; i < rowCount; i++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = GridLength.Auto;
                    WorkOrderNoteGrid.RowDefinitions.Add(rowDef);
                }

                WorkOrderNoteGrid.Children.Clear();
                sb_notes.Clear();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string notesUser = "";
                    string notesNote = "";
                    string notesUserName = "";
                    int notesID = 0;
                    int notesPK = 0;
                    DateTime notesDateAdded = new DateTime();
                    string notesPKDesc = "";

                    if (!row.IsNull("Notes_ID"))
                        notesID = row.Field<int>("Notes_ID");
                    if (!row.IsNull("Notes_PK"))
                        notesPK = row.Field<int>("Notes_PK");
                    if (!row.IsNull("Notes_Note"))
                        notesNote = row["Notes_Note"].ToString();
                    if (!row.IsNull("Notes_User"))
                        notesUser = row["Notes_User"].ToString();
                    if (!row.IsNull("Notes_UserName"))
                        notesUserName = row["Notes_UserName"].ToString();
                    if (!row.IsNull("Notes_PK_Desc"))
                        notesPKDesc = row["Notes_PK_Desc"].ToString();
                    if (!row.IsNull("Notes_DateAdded"))
                        notesDateAdded = row.Field<DateTime>("Notes_DateAdded");

                    sb_notes.Add(new Note
                    {
                        NoteID = notesID,
                        NotePK = notesPK,
                        NotesPKDesc = notesPKDesc,
                        NotesNote = notesNote,
                        NoteUser = notesUser,
                        NoteUserName = notesUserName,
                        NotesDateAdded = notesDateAdded
                    });
                }

                foreach (Note note in sb_notes)
                {
                    Grid firstGrid = new Grid();
                    Grid secondGrid = new Grid();
                    for (int i = 0; i < 2; i++)
                    {
                        RowDefinition rowDef = new RowDefinition();
                        rowDef.Height = GridLength.Auto;
                        secondGrid.RowDefinitions.Add(rowDef);
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        ColumnDefinition colDef = new ColumnDefinition();
                        firstGrid.ColumnDefinitions.Add(colDef);
                    }

                    firstGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                    firstGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                    Label DateAdded_Label = new Label();
                    Label User_Label = new Label();
                    User_Label.HorizontalAlignment = HorizontalAlignment.Right;
                    TextBox Note_TextBox = new TextBox();

                    DateAdded_Label.Content = note.NotesDateAdded.ToString();
                    DateAdded_Label.HorizontalAlignment = HorizontalAlignment.Left;
                    User_Label.Content = note.NoteUser.ToString();

                    Grid.SetColumn(DateAdded_Label, 0);
                    Grid.SetColumn(User_Label, 1);

                    firstGrid.Children.Add(DateAdded_Label);
                    firstGrid.Children.Add(User_Label);

                    Note_TextBox.Text = note.NotesNote;
                    Note_TextBox.Height = 40;
                    Note_TextBox.AcceptsReturn = true;
                    Note_TextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

                    Grid.SetRow(firstGrid, 0);
                    Grid.SetRow(Note_TextBox, 1);

                    secondGrid.Children.Add(firstGrid);
                    secondGrid.Children.Add(Note_TextBox);

                    Grid.SetRow(secondGrid, rowIndex);

                    WorkOrderNoteGrid.Margin = new Thickness(10, 2, 10, 2);
                    WorkOrderNoteGrid.Children.Add(secondGrid);
                    rowIndex += 1;
                }
            }
        }
    }
}
