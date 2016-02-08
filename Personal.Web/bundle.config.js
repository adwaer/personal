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
            scripts: "bower_components/**/*.js",
            styles: "bower_components/**/*.css",
            options: {
                rev: false
            }
        },
        "main": {
            scripts: "Frontend/**/*.js",
            styles: "Frontend/**/*.css",
            options: {
                transforms: {
                    scripts: compatTransforms
                },
                pluginOptions: {
                    'gulp-uglify': {
                        mangle: false
                    }
                },
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