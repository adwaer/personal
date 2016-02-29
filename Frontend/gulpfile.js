// gulpfile.js 
"use strict";

var gulp = require("gulp");
var bundle = require("gulp-bundle-assets");
var del = require("del");
var notify = require("gulp-notify");
var path = require("path");
var uglify = require('gulp-uglify');
var debug = require('gulp-debug');

// settings
var destDir = "src/dist";
var pathPrefix = "src/dist/";
var siteUrl = "http://ps.ru/";

gulp.task("bundle", ["bundle-clean"], function () {
    return gulp.src("./bundle.config.js")
        .pipe(bundle())
        .pipe(bundle.results({
            dest: destDir,
            pathPrefix: pathPrefix
        }))
        .pipe(gulp.dest(destDir));
});

gulp.task("bundle-clean", function () {
    gulp.src(".").pipe(notify("bundling started"));
    clean();
});

gulp.task("clean", function () {
    clean();
});

gulp.task("watch-only", function () {
    watchBundles();
});

gulp.task("default", ["bundle"], function () {});

gulp.task('compress', function () {
    return gulp.src('lib/*.js')
      .pipe(uglify())
      .pipe(gulp.dest('dist'));
});

gulp.task("reload-and-notify", function () {
    //browserSync.reload();
    gulp.src(".").pipe(notify("bundling finished"));
});


function clean() {
    del(destDir + "/**/*.*");
    gulp.src(".").pipe(notify("cleanup finished"));
}
function watchBundles() {
    gulp.watch([destDir + "/*.*", "src/main/**/*.js", "src/**/*.html"], ["reload-and-notify"]);

    var destDirAbsolutePath = path.join(__dirname, destDir);

    bundle.watch({
        configPath: path.join(__dirname, "bundle.config.js"),
        results: {
            dest: destDirAbsolutePath,
            pathPrefix: pathPrefix
        },
        dest: destDirAbsolutePath
    });
}
