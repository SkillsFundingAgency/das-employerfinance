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

function js() {
	return src('node_modules/govuk-frontend/all.js')
		.pipe(dest('content/javascript/govuk-frontend'))
}

exports.css = css;
exports.js = js;
exports.default = parallel(css, js);