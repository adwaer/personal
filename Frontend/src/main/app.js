function initApplication() {

    window.app = window.angular.module('app',
        [
            'ngResource', 'ngRoute', 'ui.bootstrap', 'cec.crud', 'cec.resource'
        ])
        .config([
            '$routeProvider', function($routeProvider) {
                $routeProvider
                    .when('/', {
                        templateUrl: 'index.html'
                    });
            }
        ])
        .config(['$httpProvider', function($httpProvider) {
            $httpProvider.defaults.useXDomain = true;
            delete $httpProvider.defaults.headers.common['X-Requested-With'];
        }]);

    window.angular.bootstrap(document, ['app']);
};

//angular.element(document).ready(initApplication);