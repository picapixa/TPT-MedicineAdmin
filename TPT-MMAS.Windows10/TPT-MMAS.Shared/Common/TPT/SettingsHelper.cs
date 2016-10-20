using Windows.Storage;

namespace TPT_MMAS.Shared.Common.TPT
{
    public class SettingsHelper
    {
        /// <summary>
        /// Gets the local settings stored.
        /// </summary>
        /// <param name="key">The key of the setting to be retrieved.</param>
        /// <returns></returns>
        public static string GetLocalSetting(string key)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            return localSettings.Values[key] as string;
        }

        /// <summary>
        /// Sets the value of the setting based on the specified key.
        /// </summary>
        /// <param name="key">The key of the setting</param>
        /// <param name="value">The specified value to be stored</param>
        public static void SetLocalSetting(string key, string value)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[key] = value;
        }
    }
}
