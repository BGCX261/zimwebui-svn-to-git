using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Framework.Web.Design;
using Telerik.Cms.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;
using System.IO;
using Telerik.Libraries;
using System.Collections;
using Telerik.Cms.Engine;
using Telerik.Libraries.WebControls;
using System.ComponentModel;
using Telerik.Framework.Web;

namespace ZimWeb.Web.UI.Design
{
    class ImageRotatorControlDesigner : ControlDesigner
    {
        #region Fields
        private ImageRotatorControlDesignerContainer container;
        private ITemplate itemTemplate;
        private LibraryManager manager;
        private PropertyDescriptorCollection properties;
        private ImageRotator component;

        #endregion

        #region Properties
        public ITemplate ItemTemplate
        {
            get
            {
                return this.itemTemplate;
            }
            set
            {
                this.itemTemplate = value;
            }
        }
        public string ItemTemplatePath
        {
            get
            {
                string str = (string)this.ViewState["ItemTemplatePath"];
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
                return "~/Customised/Sitefinity/Admin/ControlTemplates/ControlEditor/ImageRotatorControlDesigner.ascx";
            }
            set
            {
                this.ViewState["ItemTemplatePath"] = value;
            }
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

        #endregion

        #region Methods
        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            InitializeTemplate();
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
            this.container.Controls.Add(child);

            this.Controls.Add(container);

            //bind to the list of items
            IList libraries = this.Manager.GetAllLibraries("Image", true);
            this.container.LibraryDropDown.DataSource = libraries;
            this.container.LibraryDropDown.SelectedIndexChanged += new EventHandler(LibraryDropDown_SelectedIndexChanged);
            this.container.LibraryDropDown.AutoPostBack = true;
            this.container.LibraryDropDown.DataBind();

            //bind the repeater
            this.container.ImagesRepeater.ItemDataBound += new RepeaterItemEventHandler(ImagesRepeater_ItemDataBound);
            this.container.ImagesRepeater.DataSource = child;
            this.container.ImagesRepeater.DataBind();
        }

        protected virtual void InitializeTemplate()
        {
            container = new ImageRotatorControlDesignerContainer(this);
            itemTemplate = ControlUtils.GetTemplate<DefaultItemTemplate>(ItemTemplatePath);
            this.itemTemplate.InstantiateIn(this.container);
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
            this.container.ImagesRepeater.DataBind();
        }

        void ImagesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType ==  ListItemType.AlternatingItem))
            {
                IContent dataItem = (IContent)e.Item.DataItem;
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
            if (this.container.LibraryDropDown.SelectedIndex > -1)
            {
                Guid[] guidArray = new Guid[] { new Guid(this.container.LibraryDropDown.SelectedValue) };
                e.InputParameters["parentIDs"] = guidArray;
            }
        }

        public override void OnSaving()
        {
            component.Items = this.container.SelectedItems.Value.TrimEnd(';');

            base.OnSaving();
        }

        #endregion

        #region Nested Types
        private class ImageRotatorControlDesignerContainer : GenericContainer<ImageRotatorControlDesigner>
        {
            // Fields            
            private DropDownList libraryDropDown;
            private Repeater imagesRepeater;
            private HiddenField selectedItems;

            // Methods
            public ImageRotatorControlDesignerContainer(ImageRotatorControlDesigner owner)
                : base(owner)
            {
            }

            // Properties
            public HiddenField SelectedItems
            {
                get
                {
                    if (this.selectedItems == null)
                    {
                        this.selectedItems = (HiddenField)base.FindControl(typeof(HiddenField), "selectedItems", true);
                    }
                    return this.selectedItems;
                }
            }

            public DropDownList LibraryDropDown
            {
                get
                {
                    if (this.libraryDropDown == null)
                    {
                        this.libraryDropDown = (DropDownList)base.FindControl(typeof(DropDownList), "libraryDropDown", true);
                    }
                    return this.libraryDropDown;
                }
            }

            public Repeater ImagesRepeater
            {
                get
                {
                    if (this.imagesRepeater == null)
                    {
                        this.imagesRepeater = (Repeater)base.FindControl(typeof(Repeater), "imagesRepeater", true);
                    }
                    return this.imagesRepeater;
                }
            }


        }

        private class DefaultItemTemplate : ITemplate
        {
            #region ITemplate Members

            public void InstantiateIn(Control container)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        #endregion
    }
}
