using Humanizer;
using System;
using Windows.UI.Xaml.Data;

namespace TPT_MMAS.Shared.Converter.Humanizr
{
    /// <summary>
    /// Pluralizes the text in the converter parameter depending on the binded value.
    /// 
    /// This converter utilizes the Humanizer library, which is under the MIT License.
    /// Code: https://github.com/Humanizr/Humanizer
    /// License: https://github.com/Humanizr/Humanizer/blob/master/LICENSE
    /// </summary>
    public class PluralizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int quantity = (int)value;
            string text = parameter as string;

            return (quantity > 1) ? text.Pluralize() : text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class PluralizeUppercasedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var quantity = (double)value;
            string text = parameter as string;

            return (quantity > 1) ? text.Pluralize().ToUpper() : text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
