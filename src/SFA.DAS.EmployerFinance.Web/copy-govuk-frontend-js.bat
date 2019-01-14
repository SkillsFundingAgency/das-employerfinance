mkdir "wwwroot\js\govuk-frontend"
if exist "node_modules\govuk-frontend\all.js" copy "node_modules\govuk-frontend\all.js" "wwwroot\js\govuk-frontend" /Y
