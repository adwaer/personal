angular
    .module('ngGoogleMap', [])
    .directive('ngGoogleMap', function($parse) {
        var marker;
        return {
            link: function (scope, element, attributes, model) {
                var map = new google.maps.Map(element[0], {
                    center: { lat: 55.763585, lng: 37.560883 },
                    zoom: 7
                });
				var marker;

                model.$formatters.push(function(value) {
					return positionRenderer(value, map);
				})
            },
            scope: true,
            restrict: 'AE',
            require: 'ngModel',
        };

        function positionRenderer(value, map) {
            if(!value){
                return value;
            }
            var coords = new google.maps.LatLng(value.Lat, value.Lon);

            if(marker){
                marker.setMap(null);
            }
            marker = new google.maps.Marker({
                position: coords,
                map: map,
                title:"Hello World!"
            });
            map.setCenter(coords);
			google.maps.event.trigger(map, 'resize')
            //map.setCenter(pos);

            return value;
        }
    });