angular
    .module('locations', ['requests'])
    .config([
        '$routeProvider', function ($routeProvider) {
            $routeProvider
                .when('/locatiobyip/', {
                    templateUrl: '/location_by_ip.html',
                    reloadOnSearch: false
                });
        }
    ])
    .controller('LocationByIpCtrl', function ($scope, resourceFactory) {
        $scope.searchPattern = '';
        $scope.currentLocation = undefined;
        $scope.isLoading = false;

        $scope.search = function() {
            $scope.currentLocation = undefined;
            $scope.isLoading = true;

            $scope.LocationApi.get({id: $scope.searchPattern})
                .$promise
                .then(function (data) {
                    $scope.currentLocation = data;
                })
                .catch(function(){
                    alert('Произошла ошибка, возможно вы ввели некорректные данные');
                })
                .finally(function(){
                    $scope.isLoading = false;
                });
        };

        function ctor() {
            resourceFactory
                .getFor('ip/location/:id', {id: '@id'})
                .then(function(resource){
                    $scope.LocationApi = resource;
                });
        }
        ctor();
    });
angular
    .module('requests', [])
    .factory('resourceFactory', function ($resource) {

        var config = undefined;
        return {
            serviceHost: function(){
                if(config){
                    return config;
                }

                config = $resource('settings.json')
                    .get(function (data) {
                        return data.host;
                    });
                return config;
            },
            getFor: function (uri) {
                return this.serviceHost()
                    .$promise
                    .then(function (config) {
                        return $resource(config.host + uri);
                    });
            }
        };
    });
var app = angular.module('app',
    ['ngResource',
        'ngRoute',
        'ui.bootstrap',
        'requests',
        'locations',
        'ngGoogleMap'
    ])
    .controller('DefaultCtrl', ['$scope', function ($scope) {
        $scope.Header = 'Панел управления';
    }])
    .controller('SidebarCtrl', ['$scope', function ($scope) {
        $scope.isActive = function(hash){
            return location.hash == hash;
        };
    }])
    .config(['$routeProvider',
        function($routeProvider) {
            $routeProvider.
            when('/index', {
                templateUrl: 'default.html'
            })
            .otherwise({
                redirectTo: '/index'
            });
        }]);
//.config([
//    '$httpProvider', function ($httpProvider) {
//         $httpProvider.defaults.useXDomain = true;
//           delete $httpProvider.defaults.headers.common['X-Requested-With'];
//      }
//]);



angular.element(document).ready(function () {
    angular.bootstrap(document, ['app']);
});
angular
    .module('ngGoogleMap', [])
    .directive('ngGoogleMap', function($parse) {
        var map, marker;

        return {
            link: function (scope, element, attributes, model) {
                map = new google.maps.Map(element[0], {
                    center: { lat: 55.763585, lng: 37.560883 },
                    zoom: 7
                });

                model.$formatters.push(positionRenderer)
            },
            scope: true,
            restrict: 'AE',
            require: 'ngModel',
        };

        function positionRenderer(value) {
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
            //map.setCenter(pos);

            return value;
        }
    });