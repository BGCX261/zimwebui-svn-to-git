using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Web;
using ZimWeb.Web.UI.Resources;

namespace ZimWeb.Web.UI
{
    /// <summary> 
    /// This class defines the attribute for specifying embedded templates 
    /// </summary> 
    [AttributeUsage(AttributeTargets.Property)]
    internal class WebSysTemplate : EmbeddedTemplateAttribute
    {
        public WebSysTemplate(string resourceName, string description
            , string defaultExtPaht, bool isFrontEnd, string lastModified)
            : base(resourceName, description, defaultExtPaht, isFrontEnd, lastModified)
        {
        }

        /// <summary> 
        /// Gets the description. 
        /// </summary> 
        /// <value>The description.</value> 
        public override string Description
        {
            get
            {
                return Messages.ResourceManager.GetString(base.Description);
            }
        }

        /// <summary> 
        /// When implemented in a derived class, gets a unique identifier for this <see cref="T:System.Attribute"/>. 
        /// </summary> 
        /// <value></value> 
        /// <returns>An <see cref="T:System.Object"/> that is a unique identifier for the attribute.</returns> 
        public override object TypeId
        {
            get
            {
                return typeof(EmbeddedTemplateAttribute);
            }
        }
    } 
 
}
