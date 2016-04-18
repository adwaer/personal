// bundle.config.js

var lazypipe = require("lazypipe");
var babel = require("gulp-babel");
var annotate = require('gulp-ng-annotate');

var compatTransforms = lazypipe()
    .pipe(babel, {
        presets: ["es2015"]
    });

var annotateTransforms = lazypipe()
	.pipe(annotate);

module.exports = {
    bundle: {
        "vendor": {
            scripts: [
                "bower_components/jquery/dist/jquery.min.js",
                "bower_components/angular/angular.min.js",
                "bower_components/angular-route/angular-route.min.js",
                "bower_components/angular-resource/angular-resource.min.js",
                "bower_components/underscore/underscore-min.js",
                "bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js"
            ],
            styles: [
                "bower_components/angular-bootstrap/ui-bootstrap-csp.css"
            ],
            options: {
                rev: false
            }
        },
        "main": {
            scripts: "src/js/**/*.js",
            styles: "src/css/**/*.css",
            options: {
            	rev: false,
				transforms: {
					scripts: annotateTransforms
				}
            }
        }
        //,
        //"load-gulp-bundles": {
        //    scripts: 'Scripts/load-gulp-bundles.js',
        //    options: {
        //        transforms: {
        //            scripts: compatTransforms
        //        },
        //        rev: false
        //    }
        //}
    },
    //copy: function() {
    //    return gulp.src('src/**/*.woff')
    //        .pipe(gulp.dest('fonts'));
    //}

};