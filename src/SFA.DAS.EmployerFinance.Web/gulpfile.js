// running gulp tasks in Rider...
// https://www.jetbrains.com/help/rider/Using_Gulp_Task_Runner.html
// and in visual studio...
// https://docs.microsoft.com/en-us/aspnet/core/client-side/using-gulp?view=aspnetcore-2.2

var gulp = require("gulp"),
	//fs = require("fs"),
	sass = require("gulp-sass");

gulp.task("sass", function () {
	return gulp.src('Styles/*.scss')
		.pipe(sass({
			includePaths: 'node_modules'
		}))
		.pipe(gulp.dest('wwwroot/css'));
});