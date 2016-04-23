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