angular
    .module('bets', ['requests', 'ui.bootstrap'])
    .config([
        '$routeProvider', function ($routeProvider) {
            $routeProvider
                .when('/betstoday/', {
                    templateUrl: '/bets_today.html',
                    reloadOnSearch: false
                })
                .when('/locatiosbycity/', {
                    templateUrl: '/locations_by_city.html',
                    reloadOnSearch: false
                });
        }
    ])
    .controller('BetsTodayCtrl', function ($scope, resourceFactory) {
        $scope.isLoading = false;

        $scope.search = function() {
            $scope.bets = undefined;
            $scope.isLoading = true;
            $scope.Error = 0;

            $scope.BetsApi.query()
                .$promise
                .then(function (data) {
                    $scope.bets = data;
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
                .getFor('api/bets')
                .then(function(resource){
                    $scope.BetsApi = resource;
                    $scope.search();
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