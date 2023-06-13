using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp.Utils
{
    class NoteHelper
    {
        public TextBox GetLastRowTextBox(DataGrid dataGrid)
        {
            var lastRow = dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.Items.Count - 1) as DataGridRow;
            if (lastRow != null)
            {
                var textBox = FindVisualChild<TextBox>(lastRow).LastOrDefault();
                if (textBox != null)
                {
                    return textBox;
                }
            }
            return null;
        }

        public IEnumerable<T> FindVisualChild<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
                {
                    var child = VisualTreeHelper.GetChild(dependencyObject, i);

                    if (child is T result)
                    {
                        yield return result;
                    }

                    foreach (var childOfChild in FindVisualChild<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }

            yield break;
        }

        public DataGrid FindDataGrid(DependencyObject dependencyObject)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null)
            {
                return null;
            }

            DataGrid dataGrid = parent as DataGrid;
            if (dataGrid != null)
            {
                return dataGrid;
            }

            return FindDataGrid(parent);
        }  
    }
}
