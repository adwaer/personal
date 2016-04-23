var scripts = document.getElementsByTagName("script");
var currentScriptPath = scripts[scripts.length - 1].src;

angular
    .module('locations', ['resourceFactory'])
    .config([
        '$routeProvider', function ($routeProvider) {
            $routeProvider
                .when('/locatiobyip/', {
                    templateUrl: '/location_by_ip.html',
                    controller: currentScriptPath.replace('LocationModule.js', 'LocationByIpCtrl'),
                    reloadOnSearch: false
                });
        }
    ])
    .controller('LocationByIpCtrl', function ($scope, $routeParams, resourceFactory) {
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
function initApplication() {

    window.app = window.angular.module('app',
        [
            'ngResource', 'ngRoute', 'ui.bootstrap', 'requests'
        ])
        .config([
            '$routeProvider', function ($routeProvider) {
                $routeProvider
                    .when('/', {
                        templateUrl: 'index.html'
                    });
            }
        ])
        .config([
            '$httpProvider', function ($httpProvider) {
                $httpProvider.defaults.useXDomain = true;
                delete $httpProvider.defaults.headers.common['X-Requested-With'];
            }
        ]);

    debugger;
    window.angular.bootstrap(document, ['app']);
};

angular.element(document).ready(initApplication);