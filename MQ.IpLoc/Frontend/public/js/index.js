angular
    .module('locations', ['requests', 'ui.bootstrap'])
    .config([
        '$routeProvider', function ($routeProvider) {
            $routeProvider
                .when('/locatiobyip/', {
                    templateUrl: '/location_by_ip.html',
                    reloadOnSearch: false
                })
                .when('/locatiosbycity/', {
                    templateUrl: '/locations_by_city.html',
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
            $scope.Error = 0;

            $scope.LocationApi.get({id: $scope.searchPattern})
                .$promise
                .then(function (data) {
                    $scope.currentLocation = data;
                })
                .catch(function(){
                    $scope.Error = 'Произошла ошибка, возможно вы ввели некорректные данные';
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
    })
    .controller('LocationsByCityCtrl', function ($scope, resourceFactory, $uibModal) {
        $scope.searchPattern = '';
        $scope.currentLocation = undefined;
        $scope.isLoading = false;

        $scope.search = function() {
            $scope.rows = [];
            $scope.isLoading = true;
            $scope.Error = 0;

            $scope.LocationApi.query({id: $scope.searchPattern})
                .$promise
                .then(function (data) {
                    $scope.rows = data;
                })
                .catch(function(){
                    $scope.Error = 'Произошла ошибка, возможно вы ввели некорректные данные';
                })
                .finally(function(){
                    $scope.isLoading = false;
                });
        };

        $scope.showInMap = function(location){
            $scope.modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'map.html',
                controller: 'ModalCtrl',
                size: 500,
                resolve: {
                    location: function () {
                        return location;
                    }
                }
            });
        };

        function ctor() {
            resourceFactory
                .getFor('city/locations/:id', {id: '@id'})
                .then(function(resource){
                    $scope.LocationApi = resource;
                });
        }
        ctor();
    })
    .controller('ModalCtrl', function ($scope, $uibModalInstance, location) {
        $scope.currentLocation = location;

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };
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
        var marker;
        return {
            link: function (scope, element, attributes, model) {
                var map = new google.maps.Map(element[0], {
                    center: { lat: 55.763585, lng: 37.560883 },
                    zoom: 7
                });

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
            var coords = { lat: value.Lat, lng: value.Lon };

            if(marker){
                marker.setMap(null);
            }
            marker = new google.maps.Marker({
                position: coords,
                map: map,
                title:"Hello World!"
            });
			google.maps.event.trigger(map, 'resize')
            map.setCenter(coords);

            return value;
        }
    });