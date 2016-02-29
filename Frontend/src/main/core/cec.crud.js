angular
    .module('cec.crud', ['ngResource'])
    .factory('serviceHost', function () {
        return "http://localhost:7777/";
    })
    .factory('resourceFactory', function($resource, serviceHost) {
        return {
            getFor: function(entityType, config) {
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
            displayFor: function(entityType) {
                // TODO: provide debug json fallback here
                return $resource('/api/metadata/displaynames/' + entityType);
            }
        };
    });