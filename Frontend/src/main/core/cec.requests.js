angular
    .module('cec.requests', [])
    .factory('serviceHost', function ($http) {
        return $http.get('../../../settings.json', function (data) {

        });
    })
    .factory('resource', function ($resource, serviceHost) {
        return $resource(serviceHost + '/api/resource/:id');
    });