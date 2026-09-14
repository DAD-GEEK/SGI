/// <binding AfterBuild='Release' Clean='clean' />
'use strict';

var gulp = require('gulp'),
    uglify = require('gulp-uglify'),
    concat = require('gulp-concat'),
    del = require('del');


var paths = {

    scripts: './Scripts/'
};

//Recursos
paths.archivosJS = [
    paths.scripts + 'app.init.js',
    paths.scripts + 'common/**/*.js',
    paths.scripts + 'custom/**/*.js',
    paths.scripts + 'modulos/**/*.js',
];


//Limpiar archivos
gulp.task('limpiar:js', function (cb) {
    return del([paths.scripts + 'app.js'], { force: true });
});

gulp.task('clean', gulp.task('limpiar:js'));

//Concatenar todos los recursos JS en un solo archivo
gulp.task('generar:js', function () {
    return gulp.src(paths.archivosJS)
        .pipe(concat('app.js'))
        .pipe(gulp.dest(paths.scripts));
});

//Minificar app.js
gulp.task('minificar:js', function () {
    return gulp.src(paths.archivosJS)
        .pipe(concat('app.js'))
        .pipe(uglify())
        .pipe(gulp.dest(paths.scripts));
});


gulp.task('minificar', gulp.task('minificar:js'));
gulp.task('default', gulp.task('generar:js'));

gulp.task('Release', gulp.task('minificar'));
gulp.task('Debug', gulp.task('default'));

function handleError(err) {
    console.log(err.toString());
    this.emit('end');
}