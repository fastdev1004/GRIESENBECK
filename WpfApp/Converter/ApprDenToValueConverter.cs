using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApp.Converter
{
    class ApprDenToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value?.ToString();
            if (input == "A")
            {
                return "Approved";
            }
            else if (input == "D")
            {
                return "Denied";
            }
            else
            {
                return "Unknown";
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
