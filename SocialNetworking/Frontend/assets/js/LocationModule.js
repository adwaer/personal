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