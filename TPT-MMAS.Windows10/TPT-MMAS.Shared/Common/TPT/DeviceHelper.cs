using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TPT_MMAS.Shared.Common.TPT
{
    public class DeviceHelper
    {
        /// <summary>
        /// Returns true if the application is running on Windows 10 IoT Core; false otherwise.
        /// </summary>
        /// <returns></returns>
        public static bool IsAppRunningOnIot()
        {
            string deviceFamily = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
            return (deviceFamily == "Windows.IoT");
        }
    }
}
