using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Web.Management;

namespace ZimWeb.Web.UI
{
    /// <summary>
    /// Summary description for CustomEventMonitor
    /// </summary>
    public class CustomEventMonitor :WebRequestErrorEvent
    {

        public CustomEventMonitor(string message, object eventSource, Exception ex)
            : base(message, eventSource, WebEventCodes.WebExtendedBase, ex)
        {
            //
            // TODO: Add constructor logic here
            //
        }


    }
}