angular
    .module('requests', [])
    .factory('resourceFactory', function ($resource) {

        return {
            serviceHost: function(){
                return $resource('settings.json')
                    .get(function (data) {
                        return data.host;
                    });
            },
            getFor: function (uri) {
                this.serviceHost()
                    .$promise
                    .then(function (serviceHost) {
                        return $resource(serviceHost + uri);
                    });
            }
        };
    });