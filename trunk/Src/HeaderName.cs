using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Cms;
using Telerik.Cms.Web.UI;

namespace ZimWeb.Web.UI
{
    /// <summary>
    /// Shows the latest title for the page
    /// </summary>
    public class HeaderName : CompositeControl
    {
      
        private Label lblHeaderName = new Label();

        protected override void CreateChildControls()
        {
            if (Page != null)
            {                
                lblHeaderName.EnableViewState = true;

                SiteMapNode node = SiteMap.Provider.CurrentNode;
                if (node != null)
                {
                    lblHeaderName.Text = node.Title;
                    lblHeaderName.Visible = true;                  
                }
           }
                

        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            base.RenderContents(output);

            if (lblHeaderName != null)
            {
                output.WriteLine(lblHeaderName.Text);
            }
        }
    }

   
}
