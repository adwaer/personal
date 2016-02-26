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
                "bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js",
                "src/dril/js/jquery.nicescroll.min.js",
                "src/dril/js/jquery.jribbble-1.0.1.ugly.js",
                "src/dril/js/drifolio.js",
                "src/dril/js/wow.min.js"
            ],
            styles: [
                "bower_components/angular-bootstrap/ui-bootstrap-csp.css",
                "src/dril/css/preloader.css",
                "src/dril/css/style.css",
                "src/dril/css/responsive.css",
                "src/dril/css/animate.css",
                "src/dril/css/simple-line-icons.css"
            ],
            options: {
                rev: false
            }
        },
        "main": {
            scripts: "src/main/**/*.js",
            styles: "src/main/**/*.css",
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