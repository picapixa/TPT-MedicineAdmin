using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TPT_MMAS.Shared.Converter
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            DateTime v = new DateTime();
            if (value is string)
            {
                v = DateTime.Parse(value as string);
            }
            else
            {
                v = (DateTime)value;
            }

            return v.ToString(parameter as string, CultureInfo.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
