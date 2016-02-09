angular
    .module('cec.server', ['ngResource'])
    .factory('resourceFactory', function ($resource) {
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
                return $resource('/api/' + entityType + '/' + config.url, config.params, {
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
    })
    .factory('controllerFactory', function ($uibModal) {
        return {
            getFor: function (entityType, apiConfig, params) {
                return new (function () {
                    this.form = function ($scope, $routeScope, $routeParams, resourceFactory) {
                        throw 'Not implemented exception';
                        var modalInstance = $uibModal.open({
                            templateUrl: '/Crud/Save/' + entityType,
                            /*
                            controller: controller,
                            resolve: {
                                modal: function () {
                                    return $uibModal;
                                },
                                model: function () {
                                    return model;
                                },
                                callback: function () {
                                    return callback;
                                },
                                api: function () {
                                    return apiName;
                                }
                            }
                            */
                        });

                        modalInstance.opened.then(function () {
                            Loader.Hide();
                        });
                    };

                    this.display = function ($scope, resourceFactory) {
                        var resource = resourceFactory.displayFor(entityType);
                        resource.get(params, function(data) {
                            $scope.rowHeader = data;
                        });
                    };

                    this.list = function ($scope, $routeScope, $routeParams, resourceFactory) {
                        var resource = resourceFactory.getFor(entityType, apiConfig);
                        var me = this;

                        $scope.create = function () {
                            me.form($scope, $routeScope, $routeParams, resourceFactory);
                        };

                        $scope.conditionEqual = function () { return false; };
                        $scope.queryCondition = function () { return {}; };

                        $scope.editUrl = window.location.href;
                        // TODO: promise ???
                        $scope.updateRows = function () {
                            if ($scope.conditionEqual()) {
                                return;
                            }

                            Loader.Show();
                            resource.query($scope.queryCondition() || {}, function (data) {
                                $scope.decorate && $scope.decorate(data);
                                $scope.rows = data;
                                Loader.Hide();
                            });
                        };

                    };

                })();
            }
        }
    });