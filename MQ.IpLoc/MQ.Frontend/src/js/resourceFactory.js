angular
    .module('requests')
    .factory('resourceFactory', function ($resource) {

        $resource('settings.json')
            .get(function (data) {
                console.log(data);
            });

        return {
            getFor: function (entityType, config) {
                if (!config) {
                    config = {
                        url: '',
                        params: {},
                    };
                } else {
                    if (!config.params) {
                        throw "config must look like this: {url: ':id', { 'id': '@id'}}";
                    }
                }

                // TODO: provide debug json fallback here
                return $resource(serviceHost + '/api/' + entityType + '/' + config.url, config.params, {
                    query: {
                        method: 'GET',
                        isArray: true
                    },
                    update: {
                        method: 'PUT'
                    }
                });
            },
            displayFor: function (entityType) {
                // TODO: provide debug json fallback here
                return $resource('/api/metadata/displaynames/' + entityType);
            }
        };
    });
//.service('requestsService', function ($resource) {

//})