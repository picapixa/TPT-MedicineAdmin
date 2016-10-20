using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Iot.ViewModelMessages
{
    public class ErrorDetectedMessage
    {
        public string Content { get; set; }

        public string Title { get; set; }

        public ErrorDetectedMessage(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}
