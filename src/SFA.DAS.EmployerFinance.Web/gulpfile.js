// running gulp tasks in Rider...
// https://www.jetbrains.com/help/rider/Using_Gulp_Task_Runner.html
// and in visual studio...
// https://docs.microsoft.com/en-us/aspnet/core/client-side/using-gulp?view=aspnetcore-2.2

const { src, dest, parallel } = require('gulp');
const sass = require("gulp-sass");

function css() {
	return src('content/styles/*.scss')
		.pipe(sass({
			includePaths: 'node_modules'
		}))
		.pipe(dest('wwwroot/css'));
}

// function js() {
// 	return src('')
// 		.pipe(dest('wwwroot/js'))
// }

exports.css = css;
exports.default = parallel(css);