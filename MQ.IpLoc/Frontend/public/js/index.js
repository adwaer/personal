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
        $scope.search = function(){
            $scope.LocationApi.query( { id: $scope.searchPattern}, function(data){
                    console.log(data);
                });
        };

        function ctor() {
            $scope.LocationApi = resourceFactory
                .$promise
                .then(function (config) {
                    console.log(config);
                    return config.getFor('ip/location/:id', {id: '@id'});
                });
        }
        ctor();
    });
angular
    .module('requests', [])
    .factory('resourceFactory', function ($resource) {

        return {
            serviceHost: function(){
                return $resource('settings.json')
                    .get(function (data) {
                        return data.host;
                    });
            },
            getFor: function (uri) {
                this.serviceHost()
                    .$promise
                    .then(function (serviceHost) {
                        return $resource(serviceHost + uri);
                    });
            }
        };
    });
var app = angular.module('app',
    ['ngResource',
        'ngRoute',
        'ui.bootstrap',
        'requests',
        'locations'
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