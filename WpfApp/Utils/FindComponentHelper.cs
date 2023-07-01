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
    class FindComponentHelper
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

        public static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            if (parent == null)
            {
                return null;
            }
            T parentT = parent as T;
            return parentT ?? FindVisualParent<T>(parent);
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

        public DataGridRow FindDataGridRow(DependencyObject dependencyObject)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null)
            {
                return null;
            }

            DataGridRow dataGridRow = parent as DataGridRow;
            if (dataGridRow != null)
            {
                return dataGridRow;
            }

            return FindDataGridRow(parent);
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null)
                return null;

            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
