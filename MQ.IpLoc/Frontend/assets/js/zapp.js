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