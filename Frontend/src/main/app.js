function initApplication() {

    window.app = window.angular.module('app',
        [
            'ngResource', 'ngRoute', 'ui.bootstrap', 'cec.server', 'cec.resource'
        ])
        .config([
            '$routeProvider', function($routeProvider) {
                $routeProvider
                    .when('/', {
                        templateUrl: 'index.html'
                    });
            }
        ])
        .run(function($rootScope) {
            $rootScope.IsActive = function(uri) {
                return location.hash === '#' + uri;
            };
        });

    window.angular.bootstrap(document, ['app']);
};

angular.element(document).ready(initApplication);