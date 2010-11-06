using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Linq;
using System.Collections.Generic;

namespace urlme.Core.Web.Handlers
{
    public class Cache : global::System.Web.IHttpHandler
    {
        bool System.Web.IHttpHandler.IsReusable
        {
            get
            {
                return true;
            }
        }

        void System.Web.IHttpHandler.ProcessRequest(global::System.Web.HttpContext context)
        {
            if (context.Request.QueryString["cacheId"] != null)
            {
                this.RemoveCacheItem(context.Request.QueryString["cacheId"].ToString(), context);
                context.Response.Redirect(context.Request.ServerVariables["SCRIPT_NAME"].ToString());
            }

            GetAndDisplayCacheItems(context);
        }

        private void RemoveCacheItem(string id, HttpContext context)
        {
            context.Cache.Remove(id);
        }

        public static void GetAndDisplayCacheItems(HttpContext context)
        {
            // this stringbuilder gets used throughout the method as needed.
            StringBuilder sb = new StringBuilder();

            // Grab the context item, for reference so it has local scope to
            // save having to get walk back and get the Context item
            HttpContext ctx = HttpContext.Current;
            IDictionaryEnumerator d = ctx.Cache.GetEnumerator();
            Dictionary<string, object> cacheItems = new Dictionary<string, object>();

            // Create Response String and Render Writers
            string pageName = context.Request.ServerVariables["SCRIPT_NAME"].ToString();
            StringBuilder resp = new StringBuilder();
            resp.Append("<html>\n\r");
            resp.Append("<head>\n\r");
            resp.Append("<title>Cache Items Manager</title>\n\r");
            resp.Append(GetJavaScript());
            resp.Append("</head>\n\r");
            resp.Append("<body>\n\r");
            resp.Append("<strong>There are [{-CACHECOUNT-}] items in cache on " + System.Environment.MachineName + "</strong><br/>");
            resp.Append("<form id=\"frm\" method=\"post\" action=\"" + pageName + "\">\n\r");
            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);
            bool itemsExist = false;
            int cacheCount = 0;

            while (d.MoveNext())
            {
                cacheItems.Add(d.Key.ToString(), d.Value);
            }

            // render in alphabetic order
            foreach (string key in cacheItems.Keys.OrderBy(x => x))
            {
                object cacheValue = ctx.Cache[key];

                if (cacheValue != null)
                {
                    cacheCount += 1;
                    itemsExist = true;

                    // Create HTML table for display of each item in cache
                    System.Web.UI.HtmlControls.HtmlTable t = new System.Web.UI.HtmlControls.HtmlTable();

                    // Set General Table Properties
                    t.BorderColor = "FF0000";
                    t.CellPadding = 10;
                    t.CellSpacing = 0;
                    t.Border = 1;

                    System.Web.UI.HtmlControls.HtmlTableRow r = new System.Web.UI.HtmlControls.HtmlTableRow();
                    System.Web.UI.HtmlControls.HtmlTableCell cacheKeyTableCell = new System.Web.UI.HtmlControls.HtmlTableCell();
                    cacheKeyTableCell.VAlign = "top";


                    // Clear the stringbuilder and add the CacheItem Type and value
                    switch (cacheItems[key].ToString())
                    {
                        case "urlme.Core.Web.Caching.CacheItem":
                            Web.Caching.CacheItem ci = (Web.Caching.CacheItem)cacheItems[key];
                            sb.Length = 0;
                            sb.Append("<B>Cache Item Type: </B>\n\r");
                            sb.Append(cacheValue.GetType().ToString() + "\n\r");
                            sb.Append("<BR /><B>Value: </B>\n\r");
                            sb.Append(context.Server.HtmlEncode(ci.Value.GetType().ToString()) + "\n\r");
                            sb.Append("<BR /><B>Expires: </B>\n\r");
                            sb.Append(context.Server.HtmlEncode(ci.Expires.ToString()) + "\n\r");
                            break;
                        default:
                            sb.Length = 0;
                            sb.Append("<B>Cache Item Type: </B>\n\r");
                            sb.Append(cacheValue.GetType().ToString() + "\n\r");
                            sb.Append("<BR /><B>Value: </B>\n\r");
                            sb.Append(context.Server.HtmlEncode(cacheItems[key].ToString()) + "\n\r");
                            break;
                    }

                    // Add the Item Type
                    cacheKeyTableCell.Controls.Add(new LiteralControl(sb.ToString()));
                    r.Cells.Add(cacheKeyTableCell);

                    // Add the row to the HTML table
                    t.Rows.Add(r);

                    sw = new StringWriter();
                    tw = new HtmlTextWriter(sw);

                    // Start <P> Tag
                    resp.Append("<P>\n\r");

                    // Create and add Remove Link Button
                    resp.Append("<a href=\"" + pageName + "?cacheId=" + key + "\" onclick=\"return confirm('Are you sure you want to remove this cache item?');\">Remove</a>");

                    // Add a spacer between the remove link button and the open/close javascript
                    resp.Append(" ");

                    // Create and add the anchor tag to open/close javascript
                    System.Web.UI.HtmlControls.HtmlAnchor a = new System.Web.UI.HtmlControls.HtmlAnchor();
                    a.HRef = "javascript:OpenOrCloseSpan('span_" + key.Replace("\\", "-") + "');";
                    a.Controls.Add(new LiteralControl("Open/Close"));
                    a.RenderControl(tw);
                    resp.Append(sw.ToString());

                    // Create and add the Name of the Cache Item (preeceded with a space)
                    StringBuilder cacheItemDisplayName = new StringBuilder();
                    cacheItemDisplayName.Append(" ");
                    cacheItemDisplayName.Append(key);

                    // Add the Open Span Tag and ID for javascript to open and close
                    cacheItemDisplayName.Append("<span id=\"span_");
                    cacheItemDisplayName.Append(key.Replace("\\", "-"));
                    cacheItemDisplayName.Append("\" style=\"display:none;\">");
                    resp.Append(cacheItemDisplayName.ToString());

                    // Add the data table to the placeholder
                    sw = new StringWriter();
                    tw = new HtmlTextWriter(sw);
                    t.RenderControl(tw);
                    resp.Append(sw.ToString());

                    // Add the closing Span and P tags.
                    resp.Append("</span></P>");
                }
            }

            resp.Append("</form></body></html>");
            if (!itemsExist)
            {
                context.Response.Write("There are currently no items in cache.");
            }
            else
            {
                string ret = resp.ToString().Replace("[{-CACHECOUNT-}]", cacheCount.ToString());
                context.Response.Write(ret);
            }
        }

        public static void RemoveLinkButton_Click(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;

            // Cast sender as LinkButton and Check if still in cache
            if (ctx.Cache[((System.Web.UI.WebControls.LinkButton)sender).ID] != null)
            {
                // Remove item from Cache
                ctx.Cache.Remove(((System.Web.UI.WebControls.LinkButton)sender).ID);
            }

            Handlers.Cache.GetAndDisplayCacheItems(ctx);
        }

        private static string GetJavaScript()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\">\n\r");
            sb.Append("<!--\n\r");
            sb.Append("function OpenOrCloseSpan(spanTag)\n\r");
            sb.Append("{\n\r");
            sb.Append("var st = document.getElementById( spanTag );\n\r");
            sb.Append("//alert(st); \n\r");
            sb.Append("if ( st.style.display == 'none' ){\n\r");
            sb.Append("st.style.display = '';}\n\r");
            sb.Append("else{\n\r");
            sb.Append("st.style.display = 'none';}\n\r");
            sb.Append("}\n\r");
            sb.Append("// -->\n\r");
            sb.Append("</script>\n\r");
            return sb.ToString();
        }
    }
}