using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

using Telerik.Cms.Engine;
using Telerik.Framework.Web.Design;
using Telerik.Libraries;
using Telerik.Libraries.WebControls;

namespace ZimWeb.Web.UI.Design
{
    class ImageRotatorControlDesigner : ControlDesigner
    {
        #region Fields
        private LibraryManager manager;
        private PropertyDescriptorCollection properties;
        private ImageRotator component;
        private const string DesignerTemplateName = "ZimWeb.Web.UI.Resources.ControlTemplates.Backend.ImageRotatorControlDesigner.ascx"; 

        #endregion

        #region Properties

        protected override string LayoutTemplateName
        {
            get
            {
                return ImageRotatorControlDesigner.DesignerTemplateName;
            }
        }

        /// <summary> 
        /// Gets or sets the path to a custom layout template for the control. 
        /// </summary> 
        /// <value></value> 
        [WebSysTemplate(ImageRotatorControlDesigner.DesignerTemplateName, "ImageRotatorMapDesigner_Desc", "/ZimWeb.Web.UI", false, "2009-07-10")]
        public override string LayoutTemplatePath
        {
            get { return base.LayoutTemplatePath; }
            set { base.LayoutTemplatePath = value; }
        } 

        public string ProviderName
        {
            get
            {
                object providerName = this.ViewState["ProviderName"];
                if (providerName == null)
                {
                    return string.Empty;
                }
                return (string)providerName;
            }
            set
            {
                this.ViewState["ProviderName"] = value;
            }
        }

        private LibraryManager Manager
        {
            get
            {
                if (this.manager == null)
                    manager = new LibraryManager(this.ProviderName);

                return manager;
            }
        }

        /// <summary>
        /// Gets or sets the component which is of ImageRotator type.
        /// </summary>
        /// <value>The component which is of ImageRotator type.</value>
        protected ImageRotator Component
        {
            get
            {
                return component;
            }
            set
            {
                component = value;
            }
        }

        // Properties
        public HiddenField SelectedItems
        {
            get
            {
                return base.Container.GetControl<HiddenField>("selectedItems", true);                  
            }
        }

        public DropDownList LibraryDropDown
        {
            get
            {
                return base.Container.GetControl<DropDownList>("libraryDropDown", true);                  
            }
        }

        public Repeater ImagesRepeater
        {
            get
            {
                return base.Container.GetControl<Repeater>("imagesRepeater", true);                
            }
        }

        #endregion

        #region Methods
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            
            InitializeComponent();

            ObjectDataSource child = new ObjectDataSource();
            child.ID = "ContentDataSource";
            child.TypeName = "Telerik.Libraries.LibraryManager";
            child.SelectMethod = "GetContent";
            child.SelectCountMethod = "ContentCount";
            child.MaximumRowsParameterName = "max";
            child.StartRowIndexParameterName = "from";
            child.SortParameterName = "sortExp";
            child.EnablePaging = true;
            child.Selecting += new ObjectDataSourceSelectingEventHandler(this.dataSource_Selecting);
            child.ObjectCreating += new ObjectDataSourceObjectEventHandler(this.dataSource_ObjectCreating);
            this.Controls.Add(child);
         

            //bind to the list of items
            IList libraries = this.Manager.GetAllLibraries("Image", true);
            this.LibraryDropDown.DataSource = libraries;
            this.LibraryDropDown.SelectedIndexChanged += new EventHandler(LibraryDropDown_SelectedIndexChanged);
            this.LibraryDropDown.AutoPostBack = true;
            this.LibraryDropDown.DataBind();

            //bind the repeater
            this.ImagesRepeater.ItemDataBound += new RepeaterItemEventHandler(ImagesRepeater_ItemDataBound);
            this.ImagesRepeater.DataSource = child;
            this.ImagesRepeater.DataBind();

            //output the iD
            ScriptManager.RegisterStartupScript(this, typeof(ImageRotatorControlDesigner), "clientID", "var m_hiddenSelection = '" + this.SelectedItems.ClientID + "';", true); 
        }

        protected virtual void InitializeComponent()
        {
            if (DesignedControl != null)
            {
                component = (ImageRotator)DesignedControl;
                properties = TypeDescriptor.GetProperties(component);
            }
        }

        void LibraryDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            //rebind the repeater
            this.ImagesRepeater.DataBind();
        }

        void ImagesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType ==  ListItemType.AlternatingItem))
            {                
                Control control = null;
                control = e.Item.FindControl("contentView");
                if ((control != null) && (control is ItemView))
                {
                    (control as ItemView).ContentID = ((IContent)e.Item.DataItem).ID;
                }

                control = e.Item.FindControl("selectCheck");
                if ((control != null) && (control is CheckBox))
                {
                    Guid id = ((IContent)e.Item.DataItem).ID;
                    CheckBox chkBox = (CheckBox)control;
                    chkBox.Attributes["onclick"] = "setSelected(\"" + id.ToString() + ";\", !this.checked)";

                    if (component.Items !=null && component.Items.Contains(id.ToString()))
                        chkBox.Checked = true;
                }                
            }
        }

        private void dataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = this.Manager;
        }

        private void dataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //TODO enable paging
            // e.Arguments.StartRowIndex = (this.currentPagerPage * this.ItemsPerPage) - this.ItemsPerPage;
            //e.Arguments.MaximumRows = this.ItemsPerPage;
            if (this.LibraryDropDown.SelectedIndex > -1)
            {
                Guid[] guidArray = new Guid[] { new Guid(this.LibraryDropDown.SelectedValue) };
                e.InputParameters["parentIDs"] = guidArray;
            }
        }

        public override void OnSaving()
        {
            component.Items = this.SelectedItems.Value.TrimEnd(';');

            base.OnSaving();
        }

        #endregion

     
    }
}
