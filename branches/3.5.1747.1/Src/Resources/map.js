/* <![CDATA[ */
/*

-user click marker, marker display string and a link
-when link click, a javascript function is called
-use RegisterClientScriptInclude in google.cs for ToHere function
- when user click on tohere link, a new windowinfo is opened
-when direction is entered, a new window is opened and direction is shown
*/


function googlemapclass(cID, mID,directionId,linkId)
{
    //Fields
    var _mapID = mID;    
    var _directionId = directionId;    
    var _linkId = linkId;  
    var _clientID = cID;  
    
    var _Address;
    var _useAddress;
    var _Heading;
    var _centerLatLong;
    var _MarkerLatLong;
    var _zoom;           
    var _usemaptype;
    var _usemapscale;
    var _usemapoverview;
    var _usezoomMap;
    var _usedirection;
    var _WINDOW_HTML;
    var _icon;
    var _iconPath;
    var isIconDefault = true;   
    var marker;
    var _WINDOW_HTML_TOHere;
    var _WINDOW_HTML_Info;
    var location_html = "";
    var _markerXMLFile;    
    var _XmlMarkerIcon
    var _geoXMLUrl;
   
    //Properties     
    this.Address = Address;
    this.UseAddress = UseAddress;
    this.Heading = Heading;
    this.centerLatLong = centerLatLong;
    this.MarkerLatLong = MarkerLatLong;
    this.zoom = zoom;
    this.mapID = mapID;
    this.directionId = directionId;
    this.linkId = linkId;
    this.usemaptype = usemaptype;
    this.usemapscale = usemapscale; 
    this.usemapoverview = usemapoverview; 
    this.usezoomMap = usezoomMap;
    this.MarkerXMLFile = MarkerXMLFile;
    this.usedirection = usedirection;
    this.clientID = clientID;
    this.geoXMLUrl = geoXMLUrl;
    
    //Methods
    this.WINDOW_HTML = WINDOW_HTML;
    this.loadMap = loadMap;
    this.Icon = Icon;
    this.IconPath = IconPath;
    this.ToHere = ToHere;
    this.getDirections = getDirections;
    this.MapLocationClick = MapLocationClick;
    this.SetXmlFile = SetXmlFile;    
    this.XmlMarkerIcon = XmlMarkerIcon;
    
    //private variables
    var xmlmarkerinfo;
    var markerinfo = new Array();
    var index = 0;
    var xmladdress = new Array();
    var validAddressCount = new Array();
    
    function geoXMLUrl(myval)
    {
        _geoXMLUrl = myval;
    }        
            
    function IconPath(myval)
    {
        _iconPath = myval;
    }
    
    function Icon(myval)
    {
        _icon = myval;
    }
 
    function MarkerXMLFile(myval)
    {
        _markerXMLFile = myval;
    }
    
    function XmlMarkerIcon(myval)
    {
        _XmlMarkerIcon = myval;
    }    
    
    function WINDOW_HTML(myval)
    {
        _WINDOW_HTML = myval;
    }

    function Address( myVal ) 
    {
        _Address = myVal;
    }
    function UseAddress( myVal ) 
    {
        _useAddress = myVal;
    }
    function clientID( myval )
    {
        _clientID = myval;
    }
    function Heading( myVal ) 
    {
        _Heading = myVal;
    }
    function centerLatLong( myVal ) 
    {
        _centerLatLong = myVal;
    }
    function MarkerLatLong( myVal ) 
    {
        _MarkerLatLong = myVal;
    }
    function zoom( myVal ) 
    {
        _zoom = myVal;
    }
    function mapID( myVal ) 
    {
        _mapID = myVal;
    }
    
    function linkId( myVal ) 
    {
        _linkId = myVal;
    }
    
    function directionId( myVal ) 
    {
        _directionId = myVal;
    }
    
    function usedirection( myVal ) 
    {
        _usedirection = myVal;
    }
    
    function usemaptype( myVal ) 
    {
        _usemaptype = myVal;
    }
    function usemapscale( myVal ) 
    {
        _usemapscale = myVal;
    }
    
    function usemapoverview( myVal ) 
    {
        _usemapoverview = myVal;
    }
    function usezoomMap( myVal ) 
    {
        _usezoomMap = myVal;
    }
                  
    //map object variable
    var map;
    var gdir;
    function loadMap()
    {  
      var addresslength = _Address.length;
      var centerLatLongLength= _centerLatLong.length;
      var address = _Address;
      //to check browser compatibility
      if (typeof (GBrowserIsCompatible()) != "undefined" && GBrowserIsCompatible()) 
      {                    
            //if useAddress is true and address field is empty 
            if(_useAddress && addresslength == 0)
            {
                alert("Address field is empty");
            }
            //is centerLatLong is empty and useAddress=false                    
            else if(centerLatLongLength == 0 && !_useAddress)
            {        
                alert("Please set the centerLatLong property");
            }
            //address variable found in the .cs file                                  
            else
            {
                //call funtion to convert the address string
                //to actual coordinates
                getAddress(address);            
            }
        }     
    }
    
    function DirectionProperty()
    {
         // === create a GDirections Object ===
         //usually null in edit mode in sitefinity
         if(_directionId!= null)
         {
            gdir=new GDirections(map, document.getElementById(_directionId));

            // === Array for decoding the failure codes ===
            var reasons=[];
            reasons[G_GEO_SUCCESS]            = "Success";
            reasons[G_GEO_MISSING_ADDRESS]    = "Missing Address: The address was either missing or had no value.";
            reasons[G_GEO_UNKNOWN_ADDRESS]    = "Unknown Address:  No corresponding geographic location could be found for the specified address.";
            reasons[G_GEO_UNAVAILABLE_ADDRESS]= "Unavailable Address:  The geocode for the given address cannot be returned due to legal or contractual reasons.";
            reasons[G_GEO_BAD_KEY]            = "Bad Key: The API key is either invalid or does not match the domain for which it was given";
            reasons[G_GEO_TOO_MANY_QUERIES]   = "Too Many Queries: The daily geocoding quota for this site has been exceeded.";
            reasons[G_GEO_SERVER_ERROR]       = "Server error: The geocoding request could not be successfully processed.";
            reasons[G_GEO_BAD_REQUEST]        = "A directions request could not be successfully parsed.";
            reasons[G_GEO_MISSING_QUERY]      = "No query was specified in the input.";
            reasons[G_GEO_UNKNOWN_DIRECTIONS] = "The GDirections object could not compute directions between the points.";

            // === catch Directions errors ===
            GEvent.addListener(gdir, "error", function() 
            {
                var code = gdir.getStatus().code;
                var reason="Code "+code;
                if (reasons[code]) 
                {
                    reason = reasons[code]
                } 
                alert("Failed to obtain directions, "+reason);
            });            
        }
    }
    function createMap(centerPoint)
    {          
        var centerLatLongLength = _centerLatLong.length;
        var addresslength = _Address.length;
        var MarkerLatLonglength = _MarkerLatLong.length;            
        var splitMarker;
        var usedefinedmarker = 1;
        var currentMarker;
       
        map = new GMap2(document.getElementById(_mapID));
            
        DirectionProperty();
                             
        //this must follow the GEvent            
        if (!_useAddress) centerPoint=null;
                
        //if centerpoint is null meaning useAddress is set to false
        if(!centerPoint && centerLatLongLength > 0)
        {            
            //split the string centerLatLong with "," as separator 
            var splitresult = _centerLatLong.split(",")
              
            centerPoint=new GLatLng(parseFloat(splitresult[0]),parseFloat(splitresult[1])); 
                            
            //if user enter coordinates
            //then use it else use the one from the centerLatLongLength
            if(MarkerLatLonglength > 0)
            {
                splitMarker = _MarkerLatLong.split(",");
                usedefinedmarker = 0;                   
            }              
        }          
        if((addresslength > 0 && _useAddress) || (centerLatLongLength > 0 && !_useAddress) )
        {              
            //set the map on the desired location and specify the zoom int number                   
            // map.setCenter(new GLatLng(-31.981750,115.93116,13));
            map.setCenter(centerPoint,_zoom);
            map.enableDragging();
            map.enableScrollWheelZoom();
           
           //only create a marker if the address is supplied or the marker position is set
           if ((addresslength > 0) || MarkerLatLonglength > 0){        
                //will create an infomarker if use address is true
                if(usedefinedmarker == 1)
                {
                    currentMarker=createInfoMarker(map.getCenter()); 
                }                       
                //will create an infomarker if use address is false
                //and if MarkerLatLong length is greater than 0
                else if(usedefinedmarker == 0)
                {
                    var coordinates = new GLatLng(parseFloat(splitMarker[0]),parseFloat(splitMarker[1]));
                    currentMarker=createInfoMarker(coordinates); 
                }
            }
            //call function to set the google map properties
            GoogleMapProperties();
         }   

         SetXmlFile(_markerXMLFile);    
         
         //add a KML or GeoRSS overlays
         if (_geoXMLUrl.length > 0){
             var geoXml = new GGeoXml(_geoXMLUrl);
             map.addOverlay(geoXml);
         }
    }
    
    function GoogleMapProperties()
    {
        //add control to map which let us pan/zoom the map
        //and switch between Map and Satellite modes, respectively
                 
        //the control are added if the user
        //set them to true; 
        if(_usemapoverview)
        {
            map.addControl(new GOverviewMapControl());     
        }
        
        if(_usemaptype)
        {
            map.addControl(new GHierarchicalMapTypeControl());
            map.addMapType(G_PHYSICAL_MAP);     
        }
        if(_usemapscale)
        {
            map.addControl(new GScaleControl());    
        }

        //if user select large option
        if(_usezoomMap == "large")
        {
            map.addControl(new GLargeMapControl());    
        }
        else if(_usezoomMap == "small")
        {
            map.addControl(new GSmallMapControl());  
        }                           
    }
    
    //function use to create a marker icon
    //user have the choise to choose between the default one and the new one
    function createIcon()
    {
        isIconDefault= true;
        var icon = new GIcon();
        
        //if user does not specify the icon's path
        if(_iconPath.length == 0)
        {       
            //default
            if(_icon == "default")
            {
                isIconDefault=true;
         
                SetIconProperties(icon);
            }
            else if(_icon == "flag")
            {
                isIconDefault=false;
                //flag icon
                icon.image = "<% =WebResource("ZimWeb.Web.UI.Resources.Flagicon.png") %>";
                SetIconProperties(icon);
            }
            else if(_icon == "home")
            {
                isIconDefault=false;
                //home icon
                icon.image = "<% =WebResource("ZimWeb.Web.UI.Resources.homeicon.png") %>";
                //icon.image = "http://maps.google.com/mapfiles/kml/pal3/icon56.png";
                SetIconProperties(icon);
            }
            else if(_icon == "bubble")
            {
                isIconDefault=false;
                //bubble icon
                icon.image = "<% =WebResource("ZimWeb.Web.UI.Resources.bubbleicon.png") %>";
                SetIconProperties(icon);
            }
            else if(_icon == "building")
            {
                isIconDefault=false;
                //Building icon
                icon.image = "<% =WebResource("ZimWeb.Web.UI.Resources.buildingicon.png") %>";
                SetIconProperties(icon);
            }
            else if(_icon == "info")
            {
                isIconDefault=false;
                //info icon
                icon.image = "<% =WebResource("ZimWeb.Web.UI.Resources.Info_icon.png") %>";

                SetIconProperties(icon);
            }
            else if(_icon == "smiley")
            {
                isIconDefault=false;
                //smiley icon
                icon.image = "<% =WebResource("ZimWeb.Web.UI.Resources.smiley01.png") %>";

                SetIconProperties(icon);
            }
        }
        //if user specify the path where the icon is located
        else
        {
            isIconDefault=false;
            icon.image = _iconPath;
            SetIconProperties(icon);
        }
        return icon;
    }
    
    //function to set the icon properties
    function SetIconProperties(icon)
    {
        icon.iconAnchor = new GPoint(6,20);
        icon.infoWindowAnchor = new GPoint(5, 2);
    }
      
    function createInfoMarker(point)
    {     

        //call createIcon function
        var icon = createIcon();

        //use default icon
       if(isIconDefault)
        {
             // Creates a marker at the given point using default icon
             marker = new GMarker(point);
        }
        //if user do not choose default icon
        else
        {
            marker = new GMarker(point,icon);
        }      
         //if user turn on/off driving direction
        if(_usedirection)
        {
            windowhtml(point);                
        }                       
        
        //register an event listener                          
        GEvent.addListener(marker, "click", function() 
        {
            //function to edit the infowindow string
            _WINDOW_HTML_Info = _WINDOW_HTML;
        
            //if user turn on/off driving direction
            if(_usedirection)
            {
                _WINDOW_HTML_Info += _WINDOW_HTML_TOHere;
            }
            marker.openInfoWindowHtml(_WINDOW_HTML_Info); 
        });
        
        
        //Overlays are objects on the map that are tied to
        // latitude/longitude coordinates, so they move when 
        //you drag or zoom the map and when you switch map 
        //projections (such as when you switch from Map to Satellite mode).      
        map.addOverlay(marker); 
        var _infoaddressheading = _WINDOW_HTML;
        
        //if user turn on/off driving direction
        if(_usedirection)
        {
            _infoaddressheading +=_WINDOW_HTML_TOHere
        }
                
        marker.openInfoWindowHtml(_infoaddressheading);                      
        
        return marker;     
    }
    function windowhtml(point)
    {              
        var to_here_link = _clientID +".ToHere"+point+";";        
        _WINDOW_HTML_TOHere = '<div id="bottomInfo">Get directions: <a onclick="'+to_here_link+'" href="javascript:void(0)">To here</a></div>';
                
        if(_linkId != null)
        {               
            document.getElementById(_linkId).innerHTML = '<span>Get directions: <a onclick="'+to_here_link+'" href="javascript:void(0)">Open directions</a>';
        }                
    }
    
    function getAddress(address) {
    //You can access the Google Maps API geocoder via the GClientGeocoder object.
    //Use the GClientGeocoder.getLatLng() method to convert a string address
    // into a GLatLng
   
        var geocoder = new GClientGeocoder();  
        
        //convert the address into coordinates
        //the function parameter createMap will return the coordinates point   
        geocoder.getLatLng(address,createMap);
    }
    
    function ToHere(point)
    {
      map.addOverlay(marker); 
      
      //this is shown when user click on to here link
      marker.openInfoWindowHtml(_WINDOW_HTML+'<form class="directionsForm" style="text-align:left;" action="javascript:'+_clientID +'.getDirections()"><div id="bottomInfo"><strong>Get directions:</strong> To here<br />' +
           'Start address<br />' +
           '<input class="addrTxt" type="text" maxlength="100" id="'+_clientID+'_saddr" value="" />' +
           '<input class="addrGo" value="Go" type="submit" />'  +
           '<input type="hidden" id="'+_clientID+'_daddr" value="'+ marker.getPoint().lat()+ ',' + marker.getPoint().lng() + '" /></div></form>');           
    }
    
     // ===== request the directions =====

     function getDirections() 
     {          
        var fromAddress = document.getElementById(_clientID+"_saddr").value
        var toAddress = document.getElementById(_clientID+"_daddr").value      
        gdir.load("from: "+fromAddress+" to: "+toAddress);
     }
                
     function MapLocationClick()
     {     
        marker.openInfoWindowHtml(_infoaddressheading);
     }
     
     function SetXmlFile(xmlfile)
     {
        //xmlfile = "Files/Marker2.xml";
        if(xmlfile.length > 0)
        {
           //remove all markers
           map.clearOverlays();
           
           //reset index of info marker
           index = 0;
           
           //alert("in func 1" + index);
           var geocoder = new GClientGeocoder();
           
            if(_Address.length > 0)
            {
                //plot original marker.
                geocoder.getLatLng(_Address,createInfoMarker);                
            }
            //alert(xmlfile);
          var count = 0;
            
            GDownloadUrl(xmlfile, function(data, responseCode) 
            {
               //alert("in");
               var pointArray = new Array();               
                                          
               var xml = GXml.parse(data);
               var markers = xml.documentElement.getElementsByTagName("marker");
               
               
                for (var i = 0; i < markers.length; i++) 
                {   
                    //set the latitude and longditude if they exist in the xml file                                
                    if (markers[i].getAttribute("lat") && markers[i].getAttribute("lng"))
                        pointArray[i] = new GLatLng(parseFloat(markers[i].getAttribute("lat")),parseFloat(markers[i].getAttribute("lng")));                                        
                                                                               
                    //get the text to show in the info window
                    if (markers[i].getAttribute("text"))
                        markerinfo[i] = markers[i].getAttribute("text"); 
                    else
                        markerinfo[i] = markers[i].data;
                    
                    var sHTMLText = GXml.value(markers[i]);
                    if (sHTMLText.length > 0) markerinfo[i]=sHTMLText;
                                                             
                    xmladdress[i] = markers[i].getAttribute("address");
                                                          
                    if(xmladdress[i])
                    {
                        geocoder.getLatLng(xmladdress[i],ConvertAddress);
                        validAddressCount[count] = i;
                        count++;
                    }else{
                        if(pointArray[i] && markerinfo[i])
                            map.addOverlay(createXMLMarker(pointArray[i],markerinfo[i])); 
                    }                      
                }                
            }); 
         }         
     }
     
     function ConvertAddress(point)
     {               
        if(index < validAddressCount.length)
        {            
            if(point == null)
            {            
                alert(xmladdress[validAddressCount[index]]+" is not a valid address");
            }
            else
            {
                //alert(point + markerinfo[index] + index);                       
                map.addOverlay(createXMLMarker(point,markerinfo[validAddressCount[index]])); 
            }
            index++;
        }                           
     }
     
     //method that register the event when user click on marker
     function createXMLMarker(point,infoText)
     {
        var marker;
        marker= new GMarker(point);
        
        GEvent.addListener(marker, "click", function() 
        {       
            map.openInfoWindowHtml(point,infoText);
        });
        return marker;
    }
    
    //method that will read the location of the icon
    function SetIconLocation()
    {
        var icon = new GIcon();
        
        icon.image = _XmlMarkerIcon; //"<% =WebResource("ZimWeb.Web.UI.Resources.Flagicon.png") %>";
        SetIconProperties(icon);

        return icon;
    }
    
 }
 
/* ]]> */
