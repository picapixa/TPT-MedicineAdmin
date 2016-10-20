using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Iot.ViewModelMessages
{
    public class LoggingOutMessage
    {
        public bool RequestedFromDevice { get; set; }
        public LoggingOutMessage(bool isLocallyRequested)
        {
            RequestedFromDevice = isLocallyRequested;
        }
    }
}
