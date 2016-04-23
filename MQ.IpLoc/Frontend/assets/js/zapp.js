function initApplication() {

    window.app = window.angular.module('app',
        [
            'ngResource', 'ngRoute', 'ui.bootstrap', 'requests'
        ])
        .config([
            '$routeProvider', function ($routeProvider) {
                $routeProvider
                    .when('/', {
                        templateUrl: 'index.html'
                    });
            }
        ])
        .config([
            '$httpProvider', function ($httpProvider) {
                $httpProvider.defaults.useXDomain = true;
                delete $httpProvider.defaults.headers.common['X-Requested-With'];
            }
        ]);

    debugger;
    window.angular.bootstrap(document, ['app']);
};

angular.element(document).ready(initApplication);