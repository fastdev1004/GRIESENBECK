using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApp.Converter
{
    class YesNoBothConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int input = (int)value;
            switch(input)
            {
                case 0:
                    return "No";
                case 1:
                    return "Yes";
                case 2:
                    return "Both";
            }
            return "Yes";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (stringValue == "Yes")
                {
                    return 1;
                }
                else if (stringValue == "No")
                {
                    return 0;
                }
                return 2;
            }
            return Binding.DoNothing;
        }
    }
}
