using System;
using TPT_MMAS.Shared.Model;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace TPT_MMAS.Shared.Converter
{
    /// <summary>
    /// Converts the PatientAdministrationStatus enum value to an equivalent color representation.
    /// </summary>
    public class AdmissionRemarkToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            PatientAdministrationStatus status = (PatientAdministrationStatus) Enum.Parse(typeof(PatientAdministrationStatus), value.ToString());

            switch (status)
            {
                case PatientAdministrationStatus.Inactive:
                    return new SolidColorBrush(Color.FromArgb(255, 133, 149, 153)); // #859599
                case PatientAdministrationStatus.MedicineSelected:
                    return new SolidColorBrush(Color.FromArgb(255, 0, 111, 148)); // #006F94
                case PatientAdministrationStatus.MedicineLoaded:
                    return new SolidColorBrush(Color.FromArgb(255, 255, 140, 0)); // #FF8C00
                case PatientAdministrationStatus.MedicineAdministered:
                    return new SolidColorBrush(Color.FromArgb(255, 16, 124, 16)); // #13A10E
                default:
                    return new SolidColorBrush(Color.FromArgb(255, 0, 15, 71)); // #0027B4
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
