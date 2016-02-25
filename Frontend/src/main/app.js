function initApplication() {

    window.app = window.angular.module('app',
        [
            'ngResource', 'ngRoute', 'ui.bootstrap', 'cec.server', 'cec.requests', 'cec.resource'
        ])
        .config([
            '$routeProvider', function($routeProvider) {
                $routeProvider
                    .when('/', {
                        templateUrl: 'index.html'
                    });
            }
        ]);

    window.angular.bootstrap(document, ['app']);
};

angular.element(document).ready(initApplication);