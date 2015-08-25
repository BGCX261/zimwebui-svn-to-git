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
    public class GoogleMapDesigner : ControlDesigner
    {        
        #region Fields
        private GoogleMap component;        
        private PropertyEditorDialog editorDialog;
        private PropertyDescriptorCollection properties;

        private const string GoogleMapDesignerTemplateName = "ZimWeb.Web.UI.Resources.ControlTemplates.Backend.GoogleMapDesigner.ascx"; 
 
        #endregion

        #region Methods
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

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
                this.GoogleKey.Text = this.component.GoogleKey;
                this.Address.Text = this.component.Address;
                this.Heading.Text = this.component.Heading;

                if (this.Width != null)
                    this.Width.Text = this.component.Width.ToString();

                if (this.Height != null)
                    this.Height.Text = this.component.Height.ToString();

                if (this.EnableDirections != null)
                    this.EnableDirections.Checked = this.component.EnableDirections;
                
                if (this.Zoom != null)
                {
                    ListItem foundZoom= this.Zoom.Items.FindByValue(this.component.Zoom.ToString());
                    if (foundZoom != null) 
                        foundZoom.Selected = true;
                }

                this.UpdateMapButton.Click += new EventHandler(UpdateMapButton_Click);
            }

            SetMapPreview();            
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
            this.UpdateProperty(this.Heading, component);
            this.UpdateProperty(this.GoogleKey, component);
            this.UpdateProperty(this.Address, component);

            if (this.Zoom != null) 
                this.UpdateProperty(this.Zoom, component);

            if (this.EnableDirections != null) 
                this.UpdateProperty(this.EnableDirections, component);

            if (this.Width != null)
                this.UpdateProperty(this.Width, component);

            if (this.Height != null)
                this.UpdateProperty(this.Height, component);
        }

        private void SetMapPreview()
        {
            this.MapPreview.GoogleKey = this.component.GoogleKey;
            this.MapPreview.Address = this.component.Address;
            this.MapPreview.Heading = this.component.Heading;           
            this.MapPreview.Zoom = this.component.Zoom;
            this.MapPreview.Icon = this.component.Icon;
            this.MapPreview.IconPath = this.component.IconPath;            
            this.MapPreview.MarkerLatLong = this.component.MarkerLatLong;
            this.MapPreview.MarkerXMLFile = this.component.MarkerXMLFile;
            this.MapPreview.XmlMarkerIcon = this.component.XmlMarkerIcon;
            this.MapPreview.FindByAddressGiven = this.component.FindByAddressGiven;
            this.MapPreview.GoogleZoomPropertyType = this.component.GoogleZoomPropertyType;
            this.MapPreview.centerLatLong = this.component.centerLatLong;
            this.MapPreview.GeoXMLUrl = this.component.GeoXMLUrl;

            //this.MapPreview.ImageUrl = this.component.ImageUrl;
            //this.MapPreview.GoogleMapTypeControl = this.component.GoogleMapTypeControl;
            //this.MapPreview.GoogleOverviewMapControl = this.component.GoogleMapTypeControl;
            //this.MapPreview.GoogleScaleControl = this.component.GoogleScaleControl;           
        }

        private void BindZoomOptions()
        {
            if (this.Zoom != null && this.Zoom.Items.Count == 0)
            {
                for (int i = 0; i < 18; i++)
                {
                    this.Zoom.Items.Add(new ListItem(i.ToString(),i.ToString()));
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
        protected override string LayoutTemplateName
        {
            get
            {
                return GoogleMapDesigner.GoogleMapDesignerTemplateName;
            }
        }

        /// <summary> 
        /// Gets or sets the path to a custom layout template for the control. 
        /// </summary> 
        /// <value></value> 
        [WebSysTemplate(GoogleMapDesigner.GoogleMapDesignerTemplateName, "GoogleMapDesigner_Desc", "/ZimWeb.Web.UI", false, "2009-07-10")]
        public override string LayoutTemplatePath
        {
            get { return base.LayoutTemplatePath; }
            set { base.LayoutTemplatePath = value; }
        } 

        protected virtual GoogleMap MapPreview
        {
            get
            {
                return base.Container.GetControl<GoogleMap>("MapPreview", true);                                    
            }
        }

        protected virtual ITextControl GoogleKey
        {
            get
            {
                return base.Container.GetControl<ITextControl>("GoogleKey", true);                                    
            }
        }

        protected virtual ITextControl Heading
        {
            get
            {
                return base.Container.GetControl<ITextControl>("Heading", true); 
            }
        }

        protected virtual ITextControl Address
        {
            get
            {
                return base.Container.GetControl<ITextControl>("Address", true);                 
            }
        }

        protected virtual DropDownList Zoom
        {
            get
            {
                return base.Container.GetControl<DropDownList>("Zoom", false);                 
            }
        }

        protected virtual ITextControl Height
        {
            get
            {
                return base.Container.GetControl<ITextControl>("Height", false);                                 
            }
        }

        protected virtual ITextControl Width
        {
            get
            {
                return base.Container.GetControl<ITextControl>("Width", false);                               
            }
        }

        protected virtual CheckBox EnableDirections
        {
            get
            {
                return base.Container.GetControl<CheckBox>("EnableDirections", false);                               
            }
        }

        protected virtual IButtonControl UpdateMapButton
        {
            get
            {
                return base.Container.GetControl<IButtonControl>("UpdateMap", true);                               
            }
        }
        #endregion       
    }
}
