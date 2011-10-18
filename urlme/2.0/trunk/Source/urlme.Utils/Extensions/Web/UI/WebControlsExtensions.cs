using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace urlme.Utils.Extensions.Web.UI
{
    public static class WebControlsExtensions
    {
        public static void SetValue(this DropDownList ddl, string value)
        {
            ddl.ClearSelection();
            foreach (ListItem li in ddl.Items)
            {
                if (value == li.Value)
                {
                    li.Selected = true;
                    break;
                }
            }
        }
    }
}
