using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace TPT_MMAS.Iot.ViewModelMessages
{
    public class ConfirmationMessage
    {
        public object Content { get; set; }

        public object Title { get; set; }

        public string PrimaryText { get; set; }

        public string SecondaryText { get; set; }


        public ConfirmationMessage(object content, object title, string primaryButtonText, string secondaryButtonText)
        {
            Content = content;
            Title = title;
            PrimaryText = primaryButtonText;
            SecondaryText = secondaryButtonText;
        }
    }
}
