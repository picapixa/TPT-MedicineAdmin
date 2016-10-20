using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.ViewModelMessages
{
    public class ListViewBaseNavigationMessage
    {
        public int Index { get; set; }
        public object PassedItem { get; set; }

        public ListViewBaseNavigationMessage(int index, object item)
        {
            Index = index;
            PassedItem = item;
        }
    }
}
