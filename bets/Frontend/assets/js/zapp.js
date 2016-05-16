var app = angular.module('app',
    ['ngResource',
        'ngRoute',
        'ui.bootstrap',
        'requests',
        'bets',
        'ngGoogleMap'
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
        }])
    .filter('datetime', function($filter) {
        return function (input) {
            if (input == null) {
                return "";
            }

            var _date = $filter('date')(new Date(input),
                'MMM dd yyyy - HH:mm:ss');

            return _date.toUpperCase();

        };
    });
//.config([
//    '$httpProvider', function ($httpProvider) {
//         $httpProvider.defaults.useXDomain = true;
//           delete $httpProvider.defaults.headers.common['X-Requested-With'];
//      }
//]);



angular.element(document).ready(function () {
    angular.bootstrap(document, ['app']);
});