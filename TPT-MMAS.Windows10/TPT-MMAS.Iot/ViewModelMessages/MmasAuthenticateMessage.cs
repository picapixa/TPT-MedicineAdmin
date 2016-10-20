using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Model;

namespace TPT_MMAS.Iot.ViewModelMessages
{
    public class MmasAuthenticateMessage
    {
        public string ServerIpAddress { get; set; }

        public Personnel User { get; set; }

        public MmasAuthenticateMessage(Personnel user)
        {
            User = user;
        }
    }
}
