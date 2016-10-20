using Humanizer;
using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace TPT_MMAS.Shared.Converter.Humanizr
{
    /// <summary>
    /// Converts the binded DateTime value to relative timing.
    /// 
    /// This converter utilizes the Humanizer library, which is under the MIT License.
    /// Code: https://github.com/Humanizr/Humanizer
    /// License: https://github.com/Humanizr/Humanizer/blob/master/LICENSE
    /// </summary>
    public class RelativeDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            DateTime v = new DateTime();
            v = (value is string) ? DateTime.Parse(value as string) : (DateTime)value;

            string output = "";
            TimeSpan difference = v - DateTime.Now;

            double timeDifferenceInHours = difference.TotalHours;
            if (timeDifferenceInHours > 24)
                output = "in " + difference.Humanize(precision: 2, maxUnit: Humanizer.Localisation.TimeUnit.Day, minUnit: Humanizer.Localisation.TimeUnit.Hour);
            else if (timeDifferenceInHours <= -24)
                output = difference.Humanize(precision: 2, maxUnit: Humanizer.Localisation.TimeUnit.Day, minUnit: Humanizer.Localisation.TimeUnit.Hour) + " ago";
            else
                output = v.ToUniversalTime().Humanize(culture: CultureInfo.InvariantCulture);
            
            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
