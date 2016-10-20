using System;
using TPT_MMAS.Shared.Model;
using Windows.UI.Xaml.Data;

namespace TPT_MMAS.Shared.Converter
{
    /// <summary>
    /// Converts the PatientAdministrationStatus enum to a corresponding description.
    /// </summary>
    public class AdmissionRemarkToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            PatientAdministrationStatus status = (PatientAdministrationStatus)Enum.Parse(typeof(PatientAdministrationStatus), value.ToString());

            switch (status)
            {
                case PatientAdministrationStatus.MedicineSelected:
                    return "Ready for loading";
                case PatientAdministrationStatus.MedicineLoaded:
                    return "Medicine stored";
                case PatientAdministrationStatus.MedicineAdministered:
                    return "Medicine has been administered";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
