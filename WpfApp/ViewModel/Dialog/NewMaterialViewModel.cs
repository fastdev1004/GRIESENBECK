using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.Command;
using WpfApp.Model;
using WpfApp.Utils;

namespace WpfApp.ViewModel.Dialog
{
    class NewMaterialViewModel : ViewModelBase
    {
        private DatabaseConnection dbConnection;
        private string sqlquery = "";

        public NewMaterialViewModel()
        {
            TempMat = new Material();
            dbConnection = new DatabaseConnection();
            this.SaveCommand = new RelayCommand((e) => this.SaveMaterial());
        }

        // Binding Variable
        private Material _tempMat;

        public Material TempMat
        {
            get => _tempMat;
            set
            {
                if (value == _tempMat) return;
                _tempMat = value;
                OnPropertyChanged();
            }
        }

        // Command
        public RelayCommand SaveCommand { get; set; }

        private void SaveMaterial()
        {
            sqlquery = "INSERT INTO tblMaterials(Material_Code, Material_Desc, Active) OUTPUT INSERTED.Material_ID VALUES (@Code, @Desc, @Active)";

            string matCode = TempMat.MatCode;
            string matDesc = TempMat.MatDesc;
            bool active = TempMat.Active;

            int insertedMatID = dbConnection.RunQueryToCreateMaterial(sqlquery, matCode, matDesc, active);

            MessageBox.Show("New Material is added successfully", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
