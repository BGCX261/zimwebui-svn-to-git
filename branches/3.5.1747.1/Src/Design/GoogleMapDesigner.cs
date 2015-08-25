using System;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;

using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Telerik.Cms.Web.UI;
using Telerik.Framework.Web.Design;

namespace ZimWeb.Web.UI.Design
{
    class GoogleMapDesigner : ControlDesigner
    {        
        #region Fields
        private GoogleMap component;
        private Container cnt;
        private PropertyEditorDialog editorDialog;
        private ITemplate itemTemplate;
        private PropertyDescriptorCollection properties;
        #endregion

        #region Methods
        protected override void CreateChildControls()
        {            
            this.Controls.Clear();

            //get the container
            this.cnt = new Container(this);

            //get the template set up
            if ((this.itemTemplate == null) && (this.Page != null))
            {
                string itemTemplatePath = this.ItemTemplatePath;
                if (File.Exists(this.Page.MapPath(itemTemplatePath)))
                {
                    this.itemTemplate = this.Page.LoadTemplate(itemTemplatePath);
                }
                else
                {
                    this.itemTemplate = new DefaultItemTemplate();
                }
            }
            this.itemTemplate.InstantiateIn(this.cnt);
            if (base.DesignedControl != null)
            {
                //set up the editor properties
                this.component = (GoogleMap)base.DesignedControl;
                this.editorDialog = new PropertyEditorDialog();
                this.editorDialog.TypeContainer = this.component;
                this.editorDialog.PropertyChanged += new PropertyValueChangedEventHandler(this.EditorDialog_PropertyChanged);
                this.Controls.Add(this.editorDialog);
                this.properties = TypeDescriptor.GetProperties(this.component);

                //bind the zoom drop down
                BindZoomOptions();

                //set the properties of the controls in the designer
                this.cnt.GoogleKey.Text = this.component.GoogleKey;
                this.cnt.Address.Text = this.component.Address;
                this.cnt.Heading.Text = this.component.Heading;

                if (this.cnt.Width != null)
                    this.cnt.Width.Text = this.component.Width.ToString();

                if (this.cnt.Height != null)
                    this.cnt.Height.Text = this.component.Height.ToString();

                if (this.cnt.EnableDirections != null)
                    this.cnt.EnableDirections.Checked = this.component.EnableDirections;
                
                if (this.cnt.Zoom != null)
                {
                    ListItem foundZoom= this.cnt.Zoom.Items.FindByValue(this.component.Zoom.ToString());
                    if (foundZoom != null) 
                        foundZoom.Selected = true;
                }

                this.cnt.UpdateMapButton.Click += new EventHandler(UpdateMapButton_Click);
            }

            SetMapPreview();
            this.Controls.Add(this.cnt);
        }

        void UpdateMapButton_Click(object sender, EventArgs e)
        {
            SetProperites();
                  
            SetMapPreview();

            base.OnPropertyChanged(EventArgs.Empty);
            this.RecreateChildControls();    
        }

        public override void OnSaving()
        {
            SetProperites();

            base.OnSaving();
        }

        private void SetProperites()
        {
            //save the changed properties
            this.UpdateProperty(this.cnt.Heading, component);
            this.UpdateProperty(this.cnt.GoogleKey, component);
            this.UpdateProperty(this.cnt.Address, component);

            if (this.cnt.Zoom != null) 
                this.UpdateProperty(this.cnt.Zoom, component);

            if (this.cnt.EnableDirections != null) 
                this.UpdateProperty(this.cnt.EnableDirections, component);

            if (this.cnt.Width != null)
                this.UpdateProperty(this.cnt.Width, component);

            if (this.cnt.Height != null)
                this.UpdateProperty(this.cnt.Height, component);
        }

        private void SetMapPreview()
        {
            this.cnt.MapPreview.GoogleKey = this.component.GoogleKey;
            this.cnt.MapPreview.Address = this.component.Address;
            this.cnt.MapPreview.Heading = this.component.Heading;           
            this.cnt.MapPreview.Zoom = this.component.Zoom;
            this.cnt.MapPreview.Icon = this.component.Icon;
            this.cnt.MapPreview.IconPath = this.component.IconPath;            
            this.cnt.MapPreview.MarkerLatLong = this.component.MarkerLatLong;
            this.cnt.MapPreview.MarkerXMLFile = this.component.MarkerXMLFile;
            this.cnt.MapPreview.XmlMarkerIcon = this.component.XmlMarkerIcon;
            this.cnt.MapPreview.FindByAddressGiven = this.component.FindByAddressGiven;
            this.cnt.MapPreview.GoogleZoomPropertyType = this.component.GoogleZoomPropertyType;
            this.cnt.MapPreview.centerLatLong = this.component.centerLatLong;
            this.cnt.MapPreview.GeoXMLUrl = this.component.GeoXMLUrl;

            //this.cnt.MapPreview.ImageUrl = this.component.ImageUrl;
            //this.cnt.MapPreview.GoogleMapTypeControl = this.component.GoogleMapTypeControl;
            //this.cnt.MapPreview.GoogleOverviewMapControl = this.component.GoogleMapTypeControl;
            //this.cnt.MapPreview.GoogleScaleControl = this.component.GoogleScaleControl;           
        }

        private void BindZoomOptions()
        {
            if (this.cnt.Zoom != null && this.cnt.Zoom.Items.Count == 0)
            {
                for (int i = 0; i < 18; i++)
                {
                    this.cnt.Zoom.Items.Add(new ListItem(i.ToString(),i.ToString()));
                }
            }
        }
        
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
        }

        void EditorDialog_PropertyChanged(object source, PropertyValueChangedEventArgs e)
        {

            PropertyDescriptor desc;
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this.component);
            this.SetProperty(component, properties, e.PropertyName, e.PropertyValue, out desc);

            SetMapPreview();
            base.OnPropertyChanged(EventArgs.Empty);
            this.RecreateChildControls();            
        }
        
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
                return "~/Customised/Sitefinity/Admin/ControlTemplates/ControlEditor/GoogleMapDesigner.ascx";
            }
            set
            {
                this.ViewState["ItemTemplatePath"] = value;
            }
        } 
        #endregion

        #region Nested Types
        private class DefaultItemTemplate : ITemplate
        {
            // Methods
            public void InstantiateIn(Control container)
            {
               //add ctrlProps div
                HtmlGenericControl ctrlProps = new HtmlGenericControl("div");
                ctrlProps.Attributes.Add("class", "ctrlProps");
                container.Controls.Add(ctrlProps);

                HtmlGenericControl ctrlContent = new HtmlGenericControl("div");
                ctrlContent.Attributes.Add("class", "ctrlContent");
                ctrlProps.Controls.Add(ctrlContent);

                HtmlGenericControl ctrlContentImage = new HtmlGenericControl("div");
                ctrlContentImage.Attributes.Add("class", "ctrlContentImage");
                ctrlContent.Controls.Add(ctrlContentImage);

                //first column
                HtmlGenericControl ctrlContentImageColumn1 = new HtmlGenericControl("div");
                ctrlContentImageColumn1.Attributes.Add("class", "ctrlContentImageColumn");
                ctrlContentImage.Controls.Add(ctrlContentImageColumn1);

                //GoogleKey
                HtmlGenericControl pTag = new HtmlGenericControl("p");
                ctrlContentImageColumn1.Controls.Add(pTag);
                Label GoogleKeyLabel = new Label();
                GoogleKeyLabel.Text = "Google Key";
                TextBox GoogleKey = new TextBox();
                GoogleKey.ID = "GoogleKey";
                GoogleKey.CssClass = "txt";
                GoogleKeyLabel.AssociatedControlID = GoogleKey.ID;
                pTag.Controls.Add(GoogleKeyLabel);
                pTag.Controls.Add(GoogleKey);
                HtmlGenericControl emTag = new HtmlGenericControl("em");
                pTag.Controls.Add(emTag);
                HtmlAnchor anchor = new HtmlAnchor();
                anchor.Target = "_blank";
                anchor.HRef = "http://code.google.com/apis/maps/signup.html";
                anchor.InnerText = "Get a Google Maps key here";
                emTag.Controls.Add(anchor);

                //Heading
                 pTag = new HtmlGenericControl("p");
                ctrlContentImageColumn1.Controls.Add(pTag);                
                Label HeadingLabel = new Label();
                HeadingLabel.Text = "Heading";
                TextBox Heading = new TextBox();
                Heading.ID="Heading";
                Heading.CssClass= "txt";
                HeadingLabel.AssociatedControlID=Heading.ID;
                pTag.Controls.Add(HeadingLabel);
                pTag.Controls.Add(Heading);

                //Address
                pTag = new HtmlGenericControl("p");
                ctrlContentImageColumn1.Controls.Add(pTag);
                Label AddressLabel = new Label();
                AddressLabel.Text = "Address";
                TextBox Address = new TextBox();
                Address.ID = "Address";
                Address.CssClass = "txt";
                Address.TextMode = TextBoxMode.MultiLine;
                AddressLabel.AssociatedControlID = Address.ID;
                pTag.Controls.Add(AddressLabel);
                pTag.Controls.Add(Address);

                //Zoom
                pTag = new HtmlGenericControl("p");
                ctrlContentImageColumn1.Controls.Add(pTag);
                Label ZoomLabel = new Label();
                ZoomLabel.Text = "Zoom";
                DropDownList Zoom = new DropDownList();
                Zoom.ID = "Zoom";
                ZoomLabel.AssociatedControlID = Zoom.ID;
                pTag.Controls.Add(ZoomLabel);
                pTag.Controls.Add(Zoom);                

                //Update button
                LinkButton UpdateMapButton = new LinkButton();
                UpdateMapButton.ID="UpdateMap";                
                UpdateMapButton.CssClass = "CmsButLeft light";
                ctrlContentImageColumn1.Controls.Add(UpdateMapButton);
                HtmlGenericControl strongTag = new HtmlGenericControl("strong");
                strongTag.Attributes.Add("class", "CmsButRight light");
                strongTag.InnerText = "Update Map";
                UpdateMapButton.Controls.Add(strongTag);

                //second column
                HtmlGenericControl ctrlContentImageColumn2 = new HtmlGenericControl("div");
                ctrlContentImageColumn2.Attributes.Add("class", "ctrlContentImageColumn");
                ctrlContentImage.Controls.Add(ctrlContentImageColumn2);

                GoogleMap MapPreview = new GoogleMap();
                MapPreview.ID = "MapPreview";
                ctrlContentImageColumn2.Controls.Add(MapPreview);

            }
        }

        private class Container : GenericContainer<GoogleMapDesigner>
        {
            // Fields            
            private ITextControl googleKey;
            private ITextControl heading;
            private ITextControl address;
            private DropDownList zoom;
            private GoogleMap mapPreview;
            private IButtonControl updateMap;
            private ITextControl height;
            private ITextControl width;
            private CheckBox enableDirections;

            // Methods
            public Container(GoogleMapDesigner owner)
                : base(owner)
            {
            }

            // Properties
            public GoogleMap MapPreview
            {
                get
                {
                    if (this.mapPreview == null)
                    {
                        this.mapPreview = (GoogleMap)base.FindControl(typeof(GoogleMap), "MapPreview", true);
                    }
                    return this.mapPreview;
                }
            }

            public ITextControl GoogleKey
            {
                get
                {
                    if (this.googleKey == null)
                    {
                        this.googleKey = (ITextControl)base.FindControl(typeof(ITextControl), "GoogleKey", true);
                    }
                    return this.googleKey;
                }
            }

            public ITextControl Heading
            {
                get
                {
                    if (this.heading == null)
                    {
                        this.heading = (ITextControl)base.FindControl(typeof(ITextControl), "Heading", true);
                    }
                    return this.heading;
                }
            }

            public ITextControl Address
            {
                get
                {
                    if (this.address == null)
                    {
                        this.address = (ITextControl)base.FindControl(typeof(ITextControl), "Address", true);
                    }
                    return this.address;
                }
            }

            public DropDownList Zoom
            {
                get
                {
                    if (this.zoom == null)
                    {
                        this.zoom = (DropDownList)base.FindControl(typeof(DropDownList), "Zoom", false);
                    }
                    return this.zoom;
                }
            }

            public ITextControl Height
            {
                get
                {
                    if (this.height == null)
                    {
                        this.height = (ITextControl)base.FindControl(typeof(ITextControl), "Height", false);
                    }
                    return this.height;
                }
            }

            public ITextControl Width
            {
                get
                {
                    if (this.width == null)
                    {
                        this.width = (ITextControl)base.FindControl(typeof(ITextControl), "Width", false);
                    }
                    return this.width;
                }
            }

            public CheckBox EnableDirections
            {
                get
                {
                    if (this.enableDirections == null)
                    {
                        this.enableDirections = (CheckBox)base.FindControl(typeof(CheckBox), "EnableDirections", false);
                    }
                    return this.enableDirections;
                }
            }

            public IButtonControl UpdateMapButton
            {
                get
                {
                    if (this.updateMap == null)
                    {
                        this.updateMap = (IButtonControl)base.FindControl(typeof(IButtonControl), "UpdateMap", true);
                    }
                    return this.updateMap;
                }
            }
        } 
        #endregion
    }
}
