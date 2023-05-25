using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp.Controls
{
    class CustomDatePicker:DatePicker
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (Template.FindName("PART_Button", this) is Button button)
            {
                button.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
