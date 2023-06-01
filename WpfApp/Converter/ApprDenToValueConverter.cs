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
            Console.WriteLine(input);
            Console.WriteLine("ApprDenToValueConverter");
            if (input == "A")
            {
                Console.WriteLine("approved");
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
            return value.ToString();
        }
    }
}
