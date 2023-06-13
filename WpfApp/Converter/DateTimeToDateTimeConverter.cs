using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Converter
{
    class DateTimeToDateTimeConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime = (DateTime)value;
            if (dateTime.Equals(DateTime.MinValue))
            {
                return null;
            }
            else
            {
                //string dateOnlyString = dateTime;
                return dateTime;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
