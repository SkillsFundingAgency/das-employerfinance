mkdir "wwwroot\fonts"
if exist "node_modules\govuk-frontend\assets\fonts\" copy "node_modules\govuk-frontend\assets\fonts\*" "wwwroot\fonts" /Y
mkdir "wwwroot\images"
if exist "node_modules\govuk-frontend\assets\images\" copy "node_modules\govuk-frontend\assets\images\*" "wwwroot\images" /Y
