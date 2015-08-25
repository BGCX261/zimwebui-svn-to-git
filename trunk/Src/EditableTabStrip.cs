using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Cms.Web.UI;
using System.IO;
using System.Web.UI;
using Telerik.Web.UI;
using Telerik.Cms.Engine;
using Telerik.Libraries;
using System.Web;
using Telerik;
using System.Collections;
using Telerik.Framework.Web.Design;
using Telerik.Framework.Web;
using System.ComponentModel;

namespace ZimWeb.Web.UI
{
    [ControlDesigner("ZimWeb.Web.UI.Design.EditableTabstripDesigner")]
    class EditableTabStrip : CompositeControl, IEmptyControl, IEditableTabstrip
    {
        #region Fields
        private Container container;
        private string templatePath = "~/UserControls/Navigation/HomeTabstrip.ascx";
        private ITemplate template;
        #endregion

        #region IEditableTabstrip Members

        private Dictionary<string, string> tabs;

        [TypeConverter("Telerik.Samples.DictionaryConverter, Classes")]
        public Dictionary<string, string> Tabs
        {
            get
            {
                if (tabs == null)
                    tabs = new Dictionary<string, string>();
                return this.tabs;
            }
            set
            {
                this.tabs = value;
            }
        }

        #endregion


        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            InitializeTemplate();
            this.Controls.Add(this.container);
            
        }

        protected virtual void InitializeTemplate()
        {
            container = new Container(this);
            template = ControlUtils.GetTemplate<DefaultTemplate>(this.TemplatePath);
            this.template.InstantiateIn(this.container);

            if (HttpContext.Current.Request.QueryString["cmspagemode"] == "edit")
            {
                this.container.TabControl.OnClientTabSelected = "tabControl_TabSelected";
                this.container.EditorCode.Visible = true;
            }
            else
            {
                this.container.TabControl.MultiPageID = this.container.MultipageControl.UniqueID;
            }

            // If TABs is not null, we are going to loop through all the items, split the name from the value
            // and for each item we are going to create a new ListItem inside of our 
            if (Tabs != null)
            {
                foreach (KeyValuePair<string, string> tab in Tabs)
                {
                    RadTab tb = new RadTab();
                    tb.Text = tab.Key;
                    this.container.TabControl.Tabs.Add(tb);

                    RadPageView pg = new RadPageView();
                    pg.Controls.Add(new LiteralControl(tab.Value));
                    this.container.MultipageControl.PageViews.Add(pg);
                }
            }
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {

        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {

        }

        #region Properties

       

        public string TemplatePath
        {
            get
            {
                return this.templatePath;
            }
            set
            {
                this.templatePath = value;
            }
        }

        #endregion

        #region Nested Types

        private class Container : GenericContainer<EditableTabStrip>
        {
            public Container(EditableTabStrip owner) : base(owner) { }

            #region Fields
            private RadCodeBlock editorCode;
            private RadTabStrip tabControl;
            private RadMultiPage multipageControl;
            #endregion

            #region Properties
            public RadTabStrip TabControl
            {
                get
                {
                    if (tabControl == null)
                        tabControl = this.FindRequiredControl<RadTabStrip>("tabControl");
                    return tabControl;
                }
            }

            public RadMultiPage MultipageControl
            {
                get
                {
                    if (multipageControl == null)
                        multipageControl = this.FindRequiredControl<RadMultiPage>("multipageControl");
                    return multipageControl;
                }
            }

            public RadCodeBlock EditorCode
            {
                get
                {
                    if (editorCode == null)
                        editorCode = this.FindRequiredControl<RadCodeBlock>("EditorCode");
                    return editorCode;
                }
            }

            #endregion
        }

        private class DefaultTemplate : ITemplate
        {
            #region ITemplate Members

            public void InstantiateIn(Control container)
            {
                RadTabStrip tabControl = new RadTabStrip();
                tabControl.ID = "tabControl";
                container.Controls.Add(tabControl);
            }

            #endregion
        }


        #endregion

        #region IEmptyControl Members

        public bool IsEmpty
        {
            get
            {
                return false;
                //return (this.container.TabControl.Tabs.Count == 0);
            }
        }

        public string SetEmptyControlDefaultMessage()
        {
            //if (this.container.TabControl.Tabs.Count == 0)
            //{
            //    return "Empty tabs.";
            //}

            return string.Empty;
        }

        #endregion
    }
}

