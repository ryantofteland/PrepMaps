if (typeof (PrepMaps) == "undefined") {
    var PrepMaps = new (function () {
        this.Core = {};
    })();
}


$.extend(PrepMaps.Core, function () {
    var _baseUrl = "";
    var _googleMap;
    var _blueMarkerImage = "";
    var _purpleMarkerImage = "";
    var _redMarkerImage = "";
    var _greenMarkerImage = "";
    var _yellowMarkerImage = "";

    //dictionary of all map markers to show/hide
    var _mapMarkerDictionary = {
        VeryLarge: [],
        Large: [],
        Medium: [],
        Small: [],
        Tiny: []
    };

    //document.ready stuff
    $(function () {

    });

    var _initializeApplication = function (baseUrl) {
        _baseUrl = baseUrl;
		_blueMarkerImage = _baseUrl + "content/images/marker-blue.png";
		_purpleMarkerImage = _baseUrl + "content/images/marker-purple.png";
		_redMarkerImage = _baseUrl + "content/images/marker-red.png";
		_greenMarkerImage = _baseUrl + "content/images/marker-green.png";
		_yellowMarkerImage = _baseUrl + "content/images/marker-yellow.png";
	
        _initializeGoogleMap();
    };

    var _initializeGoogleMap = function () {
        var latlng = new google.maps.LatLng(46.24, -94.8); //center of MN
        var myOptions = {
            zoom: 6,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        _googleMap = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
        _loadSchoolMarkers();
    };

    var _loadSchoolMarkers = function () {
        var url = _baseUrl + 'schoolsapi/CategorizedSchools';
        $.getJSON(url, function (data) {
            //iterate over each category collection
            $.each(data, function (categoryKey, schoolList) {
                $(schoolList).each(function (index, school) {
                    var marker = _createMapMarker(categoryKey, school);
                    _createMarkerInfoWindow(marker, school);
                    _mapMarkerDictionary[categoryKey].push(marker);
                });
            });

            _showCategoryMarkers("VeryLarge");
            _showCategoryMarkers("Large");
            _showCategoryMarkers("Medium");
            _showCategoryMarkers("Small");
            _showCategoryMarkers("Tiny");
        });
    };

    var _createMapMarker = function (category, school) {
        var location = new google.maps.LatLng(school.Latitude, school.Longitude);
        var marker = new google.maps.Marker(
            {
                position: location,
                title: school.Name,
                icon: _getIconPath(category)
            });

        return marker;
    };

    var _createMarkerInfoWindow = function (marker, school) {
        var template = $("#info-window-tmpl").clone();
        template.find(".title").text(school.Name);
        template.find(".enroll .value").text(school.EnrollmentCount);

        var infowindow = new google.maps.InfoWindow({
            content: template.html(),
            maxWidth: 800
        });        

        google.maps.event.addListener(marker, 'click', function () {
            infowindow.open(_googleMap, marker);
        });

    };

    var _getIconPath = function (category) {
        if (category === "VeryLarge") {
            return _blueMarkerImage;
        }
        else if (category === "Large") {
            return _purpleMarkerImage;
        }
        else if (category === "Medium") {
            return _redMarkerImage;
        }
        else if (category === "Small") {
            return _greenMarkerImage;
        }
        else if (category === "Tiny") {
            return _yellowMarkerImage;
        }
    };

    var _hideCategoryMarkers = function (category) {
        $(_mapMarkerDictionary[category]).each(function (index, marker) {
            marker.setMap(null);
        });
    };

    var _showCategoryMarkers = function (category) {
        $(_mapMarkerDictionary[category]).each(function (index, marker) {
            marker.setMap(_googleMap);
        });
    };
	
	var _setMapToCurrentLocation = function(latitude, longitude) {
		if (navigator.geolocation) {
			navigator.geolocation.getCurrentPosition(function(position) {
				var location = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
				_googleMap.setCenter(location);
				_googleMap.setZoom(12);
				
				var marker = new google.maps.Marker({
				  position: location, 
				  map: _googleMap, 
				  title:"You are here"
			  });
			});
		}			
	}

    return {
        InitializeApplication: _initializeApplication,
		SetMapToCurrentLocation: _setMapToCurrentLocation,
		ShowCategory: _showCategoryMarkers,
		HideCategory: _hideCategoryMarkers
    };

} ());
