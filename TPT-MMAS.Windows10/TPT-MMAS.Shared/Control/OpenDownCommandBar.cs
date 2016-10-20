using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TPT_MMAS.Shared.Control
{
    public class OpenDownCommandBarVisualStateManager : VisualStateManager
    {
        protected override bool GoToStateCore(Windows.UI.Xaml.Controls.Control control, FrameworkElement templateRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
        {
            //replace OpenUp state change with OpenDown one and continue as normal
            if (!string.IsNullOrWhiteSpace(stateName) && stateName.EndsWith("OpenUp"))
            {
                stateName = stateName.Substring(0, stateName.Length - 6) + "OpenDown";
            }
            return base.GoToStateCore(control, templateRoot, stateName, group, state, useTransitions);
        }
    }


    /// <summary>
    /// Implements a command bar that is forced to open downwards.
    /// 
    /// Source: http://stackoverflow.com/questions/33290361/how-to-make-the-commandbar-open-towards-the-bottom-of-the-screen
    /// </summary>
    public class OpenDownCommandBar : CommandBar
    {
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            if (layoutRoot != null)
            {
                VisualStateManager.SetCustomVisualStateManager(layoutRoot, new OpenDownCommandBarVisualStateManager());
            }
        }
    }
}
