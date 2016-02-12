// bundle.config.js

var lazypipe = require("lazypipe");
var babel = require("gulp-babel");

var compatTransforms = lazypipe()
    .pipe(babel, {
        presets: ["es2015"]
    });

module.exports = {
    bundle: {
        "vendor": {
            scripts: [
                "bower_components/jquery/dist/jquery.min.js",
                "bower_components/angular/angular.min.js",
                "bower_components/angular-route/angular-route.min.js",
                "bower_components/angular-resource/angular-resource.min.js",
                "bower_components/angular-smart-table/dist/smart-table.min.js",
                "bower_components/underscore/underscore-min.js",
                "bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js",
                "bower_components/moment/min/moment.min.js",
                "bower_components/moment/locale/ru.js",
                "bower_components/angular-ui-calendar/src/calendar.js"
            ],
            styles: [
                "bower_components/bootstrap/dist/css/bootstrap.min.css",
                "bower_components/bootstrap/dist/css/bootstrap-theme.min.css"
            ],
            options: {
                rev: false
            }
        },
        "external": {
            scripts: "Frontend/external/**/*.js",
            styles: "Frontend/external/**/*.css",
            options: {
                rev: false
            }
        },
        "main": {
            scripts: "Frontend/main/**/*.js",
            styles: "Frontend/main/**/*.css",
            options: {
                rev: false
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
    }

};