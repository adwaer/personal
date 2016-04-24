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