// gulpfile.js 
"use strict";

var gulp = require("gulp");
var bundle = require("gulp-bundle-assets");
var del = require("del");
var notify = require("gulp-notify");
var path = require("path");
var directoryMap = require("gulp-directory-map");
var browserSync = require("browser-sync").create();

// settings
var destDir = "./Content/dist";
var pathPrefix = "/Content/dist/";
var statTemplatesDir = "Frontend/Stat/templates";
var siteUrl = "http://localhost:9165/";

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

gulp.task("watch", function () {
    browserSync.init({
        proxy: siteUrl
    });

    watchBundles();
});

gulp.task("reload-and-notify", function () {
    browserSync.reload();
    gulp.src(".").pipe(notify("bundling finished"));
});

gulp.task("stat-templates", function () {
    gulp.src(statTemplatesDir + "/**/*.html")
      .pipe(directoryMap({
          filename: "stat-templates.json"
          //prefix: statTemplatesDir
      }))
      .pipe(gulp.dest(destDir));
});

gulp.task("default", ["bundle", "stat-templates"], function () {
    //gulp.start("reload-and-notify");
});

function clean() {
    del(destDir + "/**/*.*");
    gulp.src(".").pipe(notify("cleanup finished"));
}

function watchBundles() {
    gulp.watch([destDir + "/*.*", "Views/**/*.cshtml", "Frontend/**/*.html"], ["reload-and-notify"]);

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
