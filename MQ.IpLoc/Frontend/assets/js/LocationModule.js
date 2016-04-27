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