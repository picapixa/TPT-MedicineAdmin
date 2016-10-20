using System;
using TPT_MMAS.Shared.Interface;

namespace TPT_MMAS.Shared.Common.TPT
{
    /// <summary>
    /// An object stored at runtime that has the URIs for the APIs the client will connect to.
    /// </summary>
    public class ApiSettings
    {
        public Uri HospitalApiBaseUri { get; set; }
        public Uri ImsApiBaseUri { get; set; }
        public string StationCode { get; set; }
    }
}
