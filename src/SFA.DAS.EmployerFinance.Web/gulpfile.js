// running gulp tasks in Rider...
// https://www.jetbrains.com/help/rider/Using_Gulp_Task_Runner.html

var gulp = require("gulp"),
	//fs = require("fs"),
	sass = require("gulp-sass");

gulp.task("sass", function () {
	return gulp.src('Styles/application.scss')
		.pipe(sass())
		.pipe(gulp.dest('wwwroot/css'));
});