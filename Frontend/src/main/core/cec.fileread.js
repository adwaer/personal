angular
    .module('cec.fileread', [])
    .directive("fileread", [
        function() {
            return {
                scope: {
                    fileread: "="
                },
                link: function(scope, element, attributes) {
                    element.bind("change", function(changeEvent) {
                        //var reader = new FileReader();
                        //reader.onload = function (loadEvent) {
                        //    scope.$apply(function () {
                        //        scope.fileread = loadEvent.target.result;
                        //    });
                        //}
                        //scope.fileread = changeEvent.target.files[0];

                        var data = new FormData();
                        $.each(changeEvent.target.files, function(i, file) {
                            data.append('file-' + i, file);
                        });

                        Loader.Show();
                        jQuery.ajax({
                            url: '/api/file',
                            data: data,
                            cache: false,
                            contentType: false,
                            processData: false,
                            type: 'POST',
                            success: function(data) {
                                scope.fileread = data;
                                Loader.Hide();
                            },
                            always: function() {
                                Loader.Hide();
                            }
                        });

                    });
                }
            }
        }
    ]);