using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using Telerik.Cms.Web.UI;
using Telerik.Framework.Web.Design;
using System.Configuration;


namespace ZimWeb.Web.UI
{

    /// <summary>
    /// This is the Google map control.
    /// This class will make use of the jscript file found in the Resources folder. 
    /// variables that the script is using have been initialised
    /// 
    /// </summary>
    [ControlDesignerAttribute("ZimWeb.Web.UI.Design.GoogleMapDesigner, ZimWeb.Web.UI")]
    [ToolboxData("<{0}:GoogleMap runat=\"server\"></{0}:GoogleMap>")]
    [DefaultProperty("GoogleKey")]
    public class GoogleMap : CompositeControl
    {

        #region Fields
        private string _CenterLatLong;
        private string _MarkerLatLong;
        private int _Zoom = 4;
        private string _Key;
        private string _heading;
        private string _address;
        private bool _findByAddressGiven = true;
        private bool _googleMapTypeControl;
        private bool _googleScaleControl;
        private bool _googleOverviewMapControl;
        private bool _enableDirections = false;
        private GooglePropertyZoomType m_GooglePropertyZoomType;
        private GoogleMarkerIcon m_icon;
        private string _imageUrl;
        private string m_iconPath;
        private Panel mapPanel;
        private string _markerxmlfile;
        private string geoXMLUrl;
        private string _googleScript = "http://maps.google.com/maps?file=api&v=3&key=";

        private Position _directionLinkPostion = Position.Below;
        private Unit _directionPanelHeight;
        private Unit _directionLinkPanelHeight;
        private Unit _mapPanelHeight;


        private String _xmlmarkericon;

        #endregion

        #region Methods
        protected override void CreateChildControls()
        {
            //panel to display the google map
            mapPanel = new Panel();
            //add the id to mappanel
            mapPanel.ID = "MapPanel";

            //panel to display the direction
            Panel mapdirection = new Panel();
            mapdirection.ID = "MapDirection";

            //panel that will show the google map infowindow when user click on it
            Panel link = new Panel();
            link.ID = "link";

            //style for the google map
            mapPanel.Style[HtmlTextWriterStyle.Height] = this.MapPanelHeight.ToString();
            mapPanel.Style[HtmlTextWriterStyle.Width] = this.Width.ToString();
            mapPanel.Style[HtmlTextWriterStyle.Color] = "black";
            mapPanel.Style[HtmlTextWriterStyle.BackgroundColor] = "white";
            mapPanel.CssClass = "map";

            //style for the direction            
            mapdirection.Style[HtmlTextWriterStyle.Overflow] = "auto";
            mapdirection.Style[HtmlTextWriterStyle.Height] = this.DirectionPanelHeight.ToString();
            mapdirection.CssClass = "directions";

            //style for the linkpanel            
            link.CssClass = "DirectionLink";
            
            if (_enableDirections && this.DirectionPosition == Position.Above)
            {
                this.Controls.Add(link);                
            }
            this.Controls.Add(mapPanel);
            if (_enableDirections)
            {
                if (this.DirectionPosition == Position.Below)
                    this.Controls.Add(link); 
                this.Controls.Add(mapdirection);                
            }
            //variables that are used by javascript and put on the html
            string clientScript = string.Empty;

            //if page is not null, (usually null in edit mode)
            if (Page != null)
            {
                //getting the jscript resource file.
                Page.ClientScript.RegisterClientScriptResource(typeof(GoogleMap), "ZimWeb.Web.UI.Resources.map.js");

                //create object
                //clientScript += "\n var " + this.ClientID + "= new googlemapclass(\"" + this.ClientID + "\" ); ";                

                clientScript += "\n var " + this.ClientID + "= new googlemapclass(\"" + this.ClientID + "\",\"" + mapPanel.ClientID + "\",\"" + mapdirection.ClientID + "\",\"" + link.ClientID + "\" ); ";

                clientScript += "\n " + this.ClientID + ".Address(\"" + this.Address + "\"); ";
                clientScript += "\n " + this.ClientID + ".UseAddress(" + this.FindByAddressGiven.ToString().ToLower() + ");";
                clientScript += "\n " + this.ClientID + ".Heading(\"" + this.Heading + "\"); ";
                clientScript += "\n " + this.ClientID + ".centerLatLong(\"" + this.centerLatLong + "\");";
                clientScript += "\n " + this.ClientID + ".MarkerLatLong(\"" + this.MarkerLatLong + "\");";
                clientScript += "\n " + this.ClientID + ".mapID(\"" + mapPanel.ClientID + "\");";
                clientScript += "\n " + this.ClientID + ".linkId(\"" + link.ClientID + "\");";
                clientScript += "\n " + this.ClientID + ".directionId(\"" + mapdirection.ClientID + "\");";

                clientScript += "\n " + this.ClientID + ".zoom(" + this.Zoom + ");";
                clientScript += "\n " + this.ClientID + ".usemaptype(" + this.GoogleMapTypeControl.ToString().ToLower() + ");";

                clientScript += "\n " + this.ClientID + ".usedirection(" + this.EnableDirections.ToString().ToLower() + ");";

                clientScript += "\n " + this.ClientID + ".Icon(\"" + this.Icon.ToString().ToLower() + "\");";
                clientScript += "\n " + this.ClientID + ".IconPath(\"" + (string.IsNullOrEmpty(IconPath) ? string.Empty : ResolveUrl(this.IconPath.ToString())) + "\");";

                clientScript += "\n " + this.ClientID + ".usemapscale(" + this.GoogleScaleControl.ToString().ToLower() + ");";
                clientScript += "\n " + this.ClientID + ".usemapoverview(" + this.GoogleOverviewMapControl.ToString().ToLower() + ");";
                clientScript += "\n " + this.ClientID + ".usezoomMap(\"" + this.GoogleZoomPropertyType.ToString().ToLower() + "\");";
                clientScript += "\n " + this.ClientID + ".WINDOW_HTML(\"" + (string.IsNullOrEmpty(ImageUrl) ? string.Empty : "<img style=\\\"float:left\\\" src=\\\"" + ResolveUrl(ImageUrl) + "\\\" alt=\\\"\\\" />") +
                              "<strong>" + Heading + "<\\/strong><p>" + (Address == null ? "" : Address.Replace(",", "<br \\/>")) + "<\\/p>" + "\");";

                clientScript += "\n " + this.ClientID + ".MarkerXMLFile(\"" + (string.IsNullOrEmpty(this.MarkerXMLFile) ? string.Empty : ResolveUrl(this.MarkerXMLFile.ToString())) + "\"); ";

                clientScript += "\n " + this.ClientID + ".XmlMarkerIcon(\"" + this.XmlMarkerIcon + "\"); ";
                clientScript += "\n " + this.ClientID + ".geoXMLUrl(\"" + (string.IsNullOrEmpty(this.GeoXMLUrl) ? string.Empty : ResolveUrl(this.GeoXMLUrl.ToString())) + "\"); ";



                clientScript += "\n " + this.ClientID + ".ToHere(point)";
                clientScript += "\n " + this.ClientID + ".getDirections()";
                clientScript += "\n " + this.ClientID + ".MapLocationClick()";
                clientScript += "\n " + this.ClientID + ".SetXmlFile(xmlfile)";

                

                //check if the website is being access on the local machine
                if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"] == "localhost")
                {
                    //this.GoogleKey = "ABQIAAAAUlXzwxePjJShpwZJc37epBS7UBYnl6ZPNCaLk0Tj593rswEhoRS_rcgYeUnk8gpZPKLOspY2dSVxug";

                    this.GoogleKey = "ABQIAAAAUlXzwxePjJShpwZJc37epBT80mqI3io-2TciEr20tAPjMUp_TRQAHpVIuWcx3SZpg_2n8hiV9yqmfA";
                }
                //check if the website is being access on the dev server
                else if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"] == "dev.zimweb.com.au")
                {
                    this.GoogleKey = "ABQIAAAAQELMLB0vrC58EUV-ncCaxRQO-u1BuOyC5NsCFJznDqNstfuEhRQTNJtRfJlqutVHH4SauKVWx-8C1Q";
                }
                //check if the website is being access on the dev server
                else if (HttpContext.Current.Request.ServerVariables["SERVER_NAME"] == "asus-laptop")
                {
                    this.GoogleKey = "ABQIAAAAUlXzwxePjJShpwZJc37epBQxmlusOUSZ6S6Qeu_myzzpVJct0hSNcdGpbksYb0Y0lojy82lHk1rhQw";
                }

                //add the key
                string googleInclude = GoogleScript + this.GoogleKey;
                Page.ClientScript.RegisterClientScriptInclude(typeof(GoogleMap), "googleInclude", googleInclude);

                //initialised the variables that are being used in the jscript
                Page.ClientScript.RegisterClientScriptBlock(typeof(GoogleMap), "clientScript_" + ID, clientScript, true);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            //set the height of the control to that of the 2 controls
            base.Height = Unit.Parse(((this.EnableDirections ? this.DirectionPanelHeight.Value + this.DirectionLinkPanelHeight.Value : 0) + this.MapPanelHeight.Value).ToString());

            base.OnPreRender(e);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (Page != null)
            {
                //add the javascript needed
                Page.ClientScript.RegisterStartupScript(typeof(ZimWeb.Web.UI.GoogleMap), "googleMapLoad_" + this.ClientID, "\r\nif(window.attachEvent)\r\n{\r\n\twindow.attachEvent('onload'," + this.ClientID + ".loadMap);\r\n}\r\nelse if (window.addEventListener)\r\n{\r\n\twindow.addEventListener('load'," + this.ClientID + ".loadMap, false);\r\n}", true);

                Page.ClientScript.RegisterStartupScript(typeof(ZimWeb.Web.UI.GoogleMap), "onunload", "window.onunload=GUnload;", true);

            }
            base.RenderContents(output);
        }

        #endregion

        #region Properties
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        [Category("Behavior")]
        public int Zoom
        {
            get
            {
                return _Zoom;
            }
            set
            {
                _Zoom = value;
            }
        }
        /// <summary>
        /// The Google API key
        /// </summary>
        /// <remarks>
        /// This can be obtained from http://www.google.com/apis/maps/signup.html this must be done for each domain.
        /// </remarks>
        [Description("The Google API key that can be obtained from http://www.google.com/apis/maps/signup.html")]
        public string GoogleKey
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ZimWeb.Web.UI.GoogleMaps.GoogleKey"]))
                    _Key = ConfigurationManager.AppSettings["ZimWeb.Web.UI.GoogleMaps.GoogleKey"];

                return _Key;
            }
            set
            {
                _Key = value;
            }
        }
           [Description("The Google API Script location by default at version 2")]
        public string GoogleScript
        {
            get
            {
                return _googleScript;
            }
            set
            {
                _googleScript = value;
            }
        }           

        [Browsable(true)]
        public override string CssClass
        {
            get
            {
                return base.CssClass;
            }
            set
            {
                base.CssClass = value;
            }
        }

        //hidden
        [Browsable(false)]
        public override Unit Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
            }
        }        

        /// <summary>
        /// Sets the height for the Map div
        /// </summary>
        [Category("Layout")]
        [Browsable(true)]
        [Description("The height of just the Map panel of the control")]
        public Unit MapPanelHeight
        {
            get
            {
                if (_mapPanelHeight.IsEmpty) _mapPanelHeight = Unit.Pixel(350);
                return _mapPanelHeight;
            }
            set
            {
                _mapPanelHeight = value;
            }
        }

        /// <summary>
        /// Set the height for the Directions div when visible
        /// </summary>
        [Category("Layout")]
        [Browsable(true)]
        [Description("The height of just the Directions panel if it has been enabled")]
        public Unit DirectionPanelHeight
        {
            get
            {
                if (_directionPanelHeight.IsEmpty) _directionPanelHeight = Unit.Pixel(200);
                return _directionPanelHeight;
            }
            set
            {
                _directionPanelHeight = value;
            }
        }

        /// <summary>
        /// Set the height for the DirectionsLink div when visible
        /// </summary>
        [Category("Layout")]
        [Browsable(true)]
        [Description("The height of just the Directions panel if it has been enabled")]
        public Unit DirectionLinkPanelHeight
        {
            get
            {
                if (_directionLinkPanelHeight.IsEmpty) _directionLinkPanelHeight = Unit.Pixel(18);
                return _directionLinkPanelHeight;
            }
            set
            {
                _directionLinkPanelHeight = value;
            }
        }

        public override Unit Width
        {
            get
            {
                if (base.Width.IsEmpty) base.Width = Unit.Percentage(100);
                return base.Width;
            }
            set
            {
                base.Width = value;
            }
        }

        [Browsable(true), Description("The URL of GeoRSS or KML file to add as an overlay")]        
        [Category("Behavior")]
        [Editor("System.Web.UI.Design.UrlEditor", typeof(UITypeEditor))]
        [UrlProperty]
        public string GeoXMLUrl{
            get
            {
                return geoXMLUrl;
            }
            set
            {
                geoXMLUrl = value;
            }
        }

        #region Directions
        [Category("Behavior")]
        [Browsable(true)]
        [Description("Enabled the directions functionality")]
        public bool EnableDirections
        {
            get
            {
                return _enableDirections;
            }
            set
            {
                _enableDirections = value;
            }
        }

        [Category("Layout")]
        [Browsable(true)]
        [Description("Where to position the open directions link currently either above or below")]        
        public Position DirectionPosition
        {
            get
            {                
                return _directionLinkPostion;
            }
            set
            {
                _directionLinkPostion = value;
            }
        }
        #endregion

        

        #region Marker
        [Editor("System.Web.UI.Design.ImageUrlEditor", typeof(UITypeEditor))]
        [Category("Marker")]
        [DefaultValue("")]
        [UrlProperty]
        [Browsable(true)]
        public virtual string ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                _imageUrl = value;
            }
        }

        [Category("Marker")]
        [Browsable(true)]
        [Description("The header (in bold) of the default marker")]
        public string Heading
        {
            get
            {
                return _heading;
            }
            set
            {
                _heading = value;
            }
        }

        [Category("Marker")]
        [Editor("System.Web.UI.Design.UrlEditor", typeof(UITypeEditor))]
        [UrlProperty]
        [Browsable(true), Description("The URL of the XML file that contains the coordinates ")]
        public string MarkerXMLFile
        {
            get
            {
                return _markerxmlfile;
            }
            set
            {
                _markerxmlfile = value;
            }
        }

        [Category("Marker")]
        [Editor("System.Web.UI.Design.ImageUrlEditor", typeof(UITypeEditor))]
        [UrlProperty]
        [Browsable(true), Description("The URL of the XML file that contains the coordinates ")]
        public string XmlMarkerIcon
        {
            get
            {
                return _xmlmarkericon;
            }
            set
            {
                _xmlmarkericon = value;
            }
        }



        #endregion

        #region Positioning
        [Category("Positioning")]
        [Browsable(true)]
        public string centerLatLong
        {
            get
            {
                // if (_CenterLatLong == null) return "0,0";

                return _CenterLatLong;
            }
            set
            {
                _CenterLatLong = value;
            }
        }
        [Category("Positioning")]
        [Browsable(true)]
        public string MarkerLatLong
        {
            get
            {
                // if (_MarkerLatLong == null) return "0,0";
                return _MarkerLatLong;
            }
            set
            {
                _MarkerLatLong = value;
            }
        }

        [Category("Positioning")]
        [Browsable(true)]
        //The Address of the wanted location
        public string Address
        {
            get
            {
                if (_address == null) return "Australia";
                return _address.Replace("\r\n", "");
            }
            set
            {
                _address = value;
            }
        }
        [Category("Positioning")]
        [Browsable(true)]
        public bool FindByAddressGiven
        {
            get
            {
                return _findByAddressGiven;
            }
            set
            {
                _findByAddressGiven = value;
            }
        }
        #endregion

        #region Map Controls
        [Category("Map Controls")]
        [Browsable(true)]
        //property that allow the user to switch between diffent menu option
        //e.g map, satellite, hybrid
        public bool GoogleMapTypeControl
        {
            get
            {
                return _googleMapTypeControl;
            }
            set
            {
                _googleMapTypeControl = value;
            }
        }
        [Category("Map Controls")]
        [Browsable(true)]
        //property to be allow the user see the scale control
        public bool GoogleScaleControl
        {
            get
            {
                return _googleScaleControl;
            }
            set
            {
                _googleScaleControl = value;
            }
        }
        [Category("Map Controls")]
        [Browsable(true)]
        public bool GoogleOverviewMapControl
        {
            get
            {
                return _googleOverviewMapControl;
            }
            set
            {
                _googleOverviewMapControl = value;
            }
        }

        [Category("Map Controls")]
        [Browsable(true)]
        public GooglePropertyZoomType GoogleZoomPropertyType
        {
            get
            {
                return m_GooglePropertyZoomType;
            }
            set
            {
                m_GooglePropertyZoomType = value;
            }
        }
        [Category("Icon")]
        [Browsable(true)]
        public GoogleMarkerIcon Icon
        {
            get
            {
                return m_icon;
            }
            set
            {
                m_icon = value;
            }
        }
        [Editor("System.Web.UI.Design.ImageUrlEditor", typeof(UITypeEditor))]
        [DefaultValue("")]
        [UrlProperty]
        [Category("Icon")]
        [Browsable(true)]
        public virtual string IconPath
        {
            get
            {
                return m_iconPath;
            }
            set
            {
                m_iconPath = value;
            }
        }

        #endregion

        #endregion

        #region Enums
        public enum GooglePropertyZoomType
        {
            None,
            Small,
            Large
        }
        public enum GoogleMarkerIcon
        {
            Default,
            Flag,
            Home,
            Bubble,
            Building,
            Info,
            Smiley
        }
        #endregion

        #region Nested Types

        public enum Position
        {
            Above = 1,
            Below = 2
        }

        #endregion
    }
}
