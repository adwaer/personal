// gulpfile.js 
"use strict";

var gulp = require("gulp");
var bundle = require("gulp-bundle-assets");
var del = require("del");
var notify = require("gulp-notify");
var path = require("path");
var uglify = require('gulp-uglify');
var debug = require('gulp-debug');
var jade = require('gulp-jade');


// settings
var destDir = "dist";
var pathPrefix = "dist/";
var siteUrl = "http://ps.ru/";

gulp.task("bundle", ["bundle-clean", 'jade', 'settings'], function () {
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

// jade
gulp.task('jade', function () {
    return gulp.src('src/jade/*.jade')
        .pipe(jade({
            pretty: true
        }))
        .pipe(gulp.dest(destDir))
        .pipe(notify('Jade update'));
});

gulp.task('settings', function () {
    return gulp.src('settings.json')
      .pipe(gulp.dest('dist'));
});

function clean() {
    del(destDir + "/**/*.*");
    gulp.src(".").pipe(notify("cleanup finished"));
}
function watchBundles() {
    gulp.watch([destDir + "/*.*", "src/js/**/*.js", "src/jade/**/*.jade"], ["reload-and-notify", 'jade']);

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
