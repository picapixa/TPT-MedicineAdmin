﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;

namespace TPT_MMAS.Shared.Presenters
{
    /// <summary>
    /// The following code was taken from the IoTCoreDefaultApp sample by Microsoft
    /// and is bound under the MIT License.
    /// 
    /// https://github.com/ms-iot/samples/tree/develop/IoTCoreDefaultApp
    /// LICENSE: https://github.com/ms-iot/samples/blob/develop/LICENSE.txt
    /// </summary>
    public static class DeviceInfoPresenter
    {
        public static string GetDeviceName()
        {
            try
            {
                var hostname = NetworkInformation.GetHostNames()
                    .FirstOrDefault(x => x.Type == HostNameType.DomainName);
                if (hostname != null)
                {
                    return hostname.CanonicalName;
                }
            }
            catch (Exception)
            {
                // do nothing
                // in some (strange) cases NetworkInformation.GetHostNames() fails... maybe a bug in the API...
            }
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            return loader.GetString("NoDeviceName");
        }
    }
}
