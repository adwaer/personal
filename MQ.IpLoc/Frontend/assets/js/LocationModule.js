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