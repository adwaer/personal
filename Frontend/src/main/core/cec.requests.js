angular
    .module('cec.requests', [])
    .factory('serviceHost', function () {
    	return "http://localhost:7777/";
	})
    .factory('resource', function ($resource, serviceHost) {
    	return $resource(serviceHost + '/api/resource/:id');
    });