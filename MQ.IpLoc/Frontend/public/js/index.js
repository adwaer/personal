angular
    .module('locations', [])
    .config([
        '$routeProvider', function ($routeProvider) {
            $routeProvider
                .when('/locatiobyip/', {
                    templateUrl: '/location_by_ip.html',
                    reloadOnSearch: false
                });
        }
    ])
    .controller('LocationByIpCtrl', function ($scope, $routeParams) {
        $scope.Hello = 'Hello from LocationByIpCtrl';
    });
angular
    .module('requests', [])
    .factory('resourceFactory', function ($resource) {

        $resource('settings.json')
            .get(function (data) {
                console.log(data);
            });

        return {
            getFor: function (entityType, config) {
                if (!config) {
                    config = {
                        url: '',
                        params: {},
                    };
                } else {
                    if (!config.params) {
                        throw "config must look like this: {url: ':id', { 'id': '@id'}}";
                    }
                }

                // TODO: provide debug json fallback here
                return $resource(serviceHost + '/api/' + entityType + '/' + config.url, config.params, {
                    query: {
                        method: 'GET',
                        isArray: true
                    },
                    update: {
                        method: 'PUT'
                    }
                });
            },
            displayFor: function (entityType) {
                // TODO: provide debug json fallback here
                return $resource('/api/metadata/displaynames/' + entityType);
            }
        };
    });
//.service('requestsService', function ($resource) {

//})
var app = angular.module('app',
    ['ngResource',
        'ngRoute',
        'ui.bootstrap',
        'requests',
        'locations'
    ])
    .controller('DefaultCtrl', ['$scope', function ($scope) {
        $scope.Hello = 'World';
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