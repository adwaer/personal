angular
    .module('locations', ['resourceFactory'])
    .config([
        '$routeProvider', function ($routeProvider) {
            $routeProvider
                .when('/locatiobyip/', {
                    templateUrl: 'location_by_ip.html',
                    controller: 'LocationByIpCtrl'
                });
        }
    ])
    .controller('LocationByIpCtrl', function ($scope, $routeParams, resourceFactory) {
        $scope.Hello = 'Hello from LocationByIpCtrl';
    });