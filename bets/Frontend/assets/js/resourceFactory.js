angular
    .module('requests', [])
    .factory('resourceFactory', function ($resource) {

        var config = undefined;
        return {
            serviceHost: function(){
                if(config){
                    return config;
                }

                config = $resource('settings.json')
                    .get(function (data) {
                        return data.host;
                    });
                return config;
            },
            getFor: function (uri) {
                return this.serviceHost()
                    .$promise
                    .then(function (config) {
                        return $resource(config.host + uri);
                    });
            }
        };
    });