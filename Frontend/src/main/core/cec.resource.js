angular
    .module('cec.resource', ['cec.crud'])
    .service('resourceService', function (resourceFactory) {

        var resources = {};

        function getValue(resourceSet, name) {
            for (var i = 0; i < resourceSet.length; i++) {
                var resource = resourceSet[i];
                if (resource._key === name) {
                    return resource._value;
                }
            }
        }

        var loadings = {};
        var waiters = {};

        this.Get = function(type, name, callback) {
            if (resources.hasOwnProperty(type)) {
                callback(getValue(resources[type], name));
                return;
            }

            if (loadings[type]) {
                waiters[type].push({ callback: callback, name: name });
                return;
            }

            loadings[type] = true;
            waiters[type] = [];

            var data = resourceFactory.getFor('resource')
                .query({ id: type }, function() {
                    resources[type] = data;
                    callback(getValue(data, name));

                    var w = waiters[type];
                    for (var i = 0; i < w.length; i++) {
                        var waiter = w[i];
                        waiter.callback(getValue(data, waiter.name));
                    }
                });
        };
    })
    .directive("resource", function (resourceService) {
        return {
            //scope: {
            //    resource: "@",
            //    resouceTag: "@"
            //},
            link: function (scope, element, attrs) {
                var parts = attrs.resource.split(/\.(.+)/);
                resourceService.Get(parts[0], parts[1], function (value) {

                    if (attrs.resourceSelector) {
                        element = element.find(attrs.resourceSelector);
                    }

                    if (attrs.resourceTag) {
                        element.attr(attrs.resourceTag, value);
                    } else {
                        element.text(value);
                    }
                });
            }
        }
    });