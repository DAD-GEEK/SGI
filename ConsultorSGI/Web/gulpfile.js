/// <binding AfterBuild='Debug' Clean='clean' />
'use strict';

var gulp = require('gulp'),
    $ = require('gulp-load-plugins')(),
    del = require('del'),
    saveLicense = require('uglify-save-license');


var paths = {
    cssAdminLTE: './AdminLTE/dist/css/*.css',
    scriptsAdminLTE: './AdminLTE/dist/js/*.js',
    css: './Content/*.css',
    scripts: './Scripts/'
};

//Recursos
paths.archivosJS = [
    paths.scripts + 'app.init.js',
    paths.scripts + 'common/**/*.js',
    paths.scripts + 'custom/**/*.js',
    paths.scripts + 'modulos/**/*.js',
];

var vendor = {
    source: require('./vendor.json'),
    dest: './Vendor'
};

var cssnanoOpts = {
    safe: true,
    discardUnused: false, // no remove @font-face
    reduceIdents: false, // no change on @keyframes names
    zindex: false // no change z-index
};

var vendorUglifyOpts = {
    output: {
        comments: false,
    },
    mangle: {
        reserved: ['$super'], // rickshaw requires this
    }
};

gulp.task('vendor', function () {

    return gulp.src(vendor.source, {
        base: 'node_modules'
    })
        .pipe(gulp.dest(vendor.dest));

});

gulp.task('min:vendor', function () {

    var jsFilter = $.filter('**/*.js', {
        restore: true
    });
    var cssFilter = $.filter('**/*.css', {
        restore: true
    });

    return gulp.src(vendor.source, {
        base: 'node_modules'
    })
        .pipe(jsFilter)
        .pipe($.uglify(vendorUglifyOpts))
        .on('error', handleError)
        .pipe(jsFilter.restore)
        .pipe(cssFilter)
        .pipe($.cssnano(cssnanoOpts))
        .pipe(cssFilter.restore)
        .pipe(gulp.dest(vendor.dest));

});

gulp.task('clean:js', function (cb) {
    return del([paths.scripts + 'app.js'], { force: true });
});

gulp.task('clean:vendor', function (cb) {
    return del(vendor.dest, { force: true });
});

gulp.task('clean', gulp.series('clean:js'));

gulp.task('js', function () {
    return gulp.src(paths.archivosJS)
        .pipe($.sourcemaps.init())
        .pipe($.concat('app.js'))
        .pipe($.sourcemaps.write())
        .pipe(gulp.dest(paths.scripts));
});

gulp.task('min:js', function () {
    return gulp.src(paths.archivosJS)
        .pipe($.concat('app.js'))
        .pipe($.uglify({
            output: {
                comments: saveLicense
            }
        }))
        .pipe(gulp.dest(paths.scripts));
});

gulp.task('min:css', function () {
    return gulp.src(paths.css + '**/*.css')
        .pipe($.cssnano(cssnanoOpts))
        .pipe(gulp.dest(paths.css));
});

gulp.task('min', gulp.series('min:vendor', 'min:css', 'min:js'));
gulp.task('default', gulp.series('vendor', 'js'));

// Task names for each ConfigurationName
// see file Angle.csproj
gulp.task('Release', gulp.task('min'));
gulp.task('Debug', gulp.task('default'));


function handleError(err) {
    console.log(err.toString());
    this.emit('end');
}