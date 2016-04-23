var lr = require('tiny-lr'), // Минивебсервер для livereload
    gulp = require('gulp'), // Сообственно Gulp JS
    jade = require('gulp-jade'), // Плагин для Jade
    stylus = require('gulp-stylus'), // Плагин для Stylus
    livereload = require('gulp-livereload'), // Livereload для Gulp
    myth = require('gulp-myth'), // Плагин для Myth - http://www.myth.io/
    csso = require('gulp-csso'), // Минификация CSS
    imagemin = require('gulp-imagemin'), // Минификация изображений
    uglify = require('gulp-uglify'), // Минификация JS
    concat = require('gulp-concat'), // Склейка файлов
    connect = require('connect'), // Webserver
    server = lr(),
    serveStatic = require('serve-static');;

gulp.task('stylus', function() {
    gulp.src('./assets/stylus/**/*.css')
        .pipe(stylus({
            use: ['nib']
        })) // собираем stylus
        .on('error', console.log) // Если есть ошибки, выводим и продолжаем
        .pipe(myth()) // добавляем префиксы - http://www.myth.io/
        .pipe(gulp.dest('./public/css/')) // записываем css
        .pipe(livereload(server)); // даем команду на перезагрузку css
});

gulp.task('stylus_vendor', function() {
    gulp.src([
            './node_modules/angular-bootstrap/ui-bootstrap-csp.css',
            './node_modules/bootstrap/dist/css/bootstrap.min.css'
        ])
        .pipe(concat('vendor.css'))
        .pipe(stylus({
            use: ['nib']
        })) // собираем stylus
        .on('error', console.log) // Если есть ошибки, выводим и продолжаем
        .pipe(myth()) // добавляем префиксы - http://www.myth.io/
        .pipe(gulp.dest('./public/css/')) // записываем css
        .pipe(livereload(server)); // даем команду на перезагрузку css
});

// Собираем html из Jade

gulp.task('jade', function() {
    gulp.src(['./assets/template/*.jade', '!./assets/template/_*.jade'])
        .pipe(jade({
            pretty: true
        }))  // Собираем Jade только в папке ./assets/template/ исключая файлы с _*
        .on('error', console.log) // Если есть ошибки, выводим и продолжаем
        .pipe(gulp.dest('./public/')) // Записываем собранные файлы
        .pipe(livereload(server)); // даем команду на перезагрузку страницы
});



// Собираем JS
gulp.task('js', function() {
    gulp.src(['./assets/js/**/*.js', '!./assets/js/vendor/**/*.js'])
        .pipe(concat('index.js')) // Собираем все JS, кроме тех которые находятся в ./assets/js/vendor/**
        .pipe(gulp.dest('./public/js'))
        .pipe(livereload(server)); // даем команду на перезагрузку страницы
});

// Собираем vendor JS
gulp.task('js_vendor', function() {
    gulp.src(['./node_modules/jquery/dist/jquery.min.js',
            './node_modules/angular/angular.min.js',
            './node_modules/angular-route/angular-route.min.js',
            './node_modules/angular-resource/angular-resource.min.js',
            './node_modules/angular-bootstrap/ui-bootstrap-tpls.min.js'])
        .pipe(concat('vendor.js')) // Собираем все JS, кроме тех которые находятся в ./assets/js/vendor/**
        .pipe(gulp.dest('./public/js'))
        .pipe(livereload(server)); // даем команду на перезагрузку страницы
});

// Копируем и минимизируем изображения

gulp.task('images', function() {
    gulp.src('./assets/img/**/*')
        .pipe(imagemin())
        .pipe(gulp.dest('./public/img'))
});

// Локальный сервер для разработки
gulp.task('http-server', function() {
    var app = connect();
    app.use(serveStatic('./public'));
    app.use(require('connect-livereload')());
    app.listen('7778');

    console.log('Server listening on http://localhost:7778');
});

// Запуск сервера разработки gulp watch
gulp.task('watch', function() {
    // Предварительная сборка проекта
    gulp.run('stylus_vendor');
    gulp.run('stylus');
    gulp.run('jade');
    gulp.run('images');
    gulp.run('js');
    gulp.run('js_vendor');

    // Подключаем Livereload
    server.listen(35729, function(err) {
        if (err) return console.log(err);

        gulp.watch('assets/stylus/**/*.css', function() {
            gulp.run('stylus');
        });
        gulp.watch('assets/template/**/*.jade', function() {
            gulp.run('jade');
        });
        gulp.watch('assets/img/**/*', function() {
            gulp.run('images');
        });
        gulp.watch('assets/js/**/*', function() {
            gulp.run('js');
        });
    });
    gulp.run('http-server');
});

gulp.task('build', function() {
    // css vendor
    gulp.src([
            './node_modules/angular-bootstrap/ui-bootstrap-csp.css',
            './node_modules/bootstrap/dist/css/bootstrap.min.css'
        ])
        .pipe(concat('vendor.css'))
        .pipe(stylus({
            use: ['nib']
        })) // собираем stylus
        .pipe(myth()) // добавляем префиксы - http://www.myth.io/
        .pipe(csso()) // минимизируем css
        .pipe(gulp.dest('./build/css/')) // записываем css

    // css
    gulp.src('./assets/stylus/*.css')
        .pipe(stylus({
            use: ['nib']
        })) // собираем stylus
        .pipe(myth()) // добавляем префиксы - http://www.myth.io/
        .pipe(csso()) // минимизируем css
        .pipe(gulp.dest('./build/css/')) // записываем css

    // jade
    gulp.src(['./assets/template/*.jade', '!./assets/template/_*.jade'])
        .pipe(jade())
        .pipe(gulp.dest('./build/'))

    // js vendor
    gulp.src(['./node_modules/jquery/dist/jquery.min.js',
            './node_modules/angular/angular.min.js',
            './node_modules/angular-route/angular-route.min.js',
            './node_modules/angular-resource/angular-resource.min.js',
            './node_modules/angular-bootstrap/ui-bootstrap-tpls.min.js'])
        .pipe(concat('vendor.js'))
        .pipe(uglify())
        .pipe(gulp.dest('./build/js'));

    // js
    gulp.src(['./assets/js/**/*.js', '!./assets/js/vendor/**/*.js'])
        .pipe(concat('index.js'))
        .pipe(uglify())
        .pipe(gulp.dest('./build/js'));

    // image
    gulp.src('./assets/img/**/*')
        .pipe(imagemin())
        .pipe(gulp.dest('./build/img'))

});

gulp.task('default', ['watch'], function () { });