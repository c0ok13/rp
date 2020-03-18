@ECHO OFF
set arg1=%1
echo %arg1%
echo.
set mainPath=%~dp0

set PostFormPath=%mainPath%src\PostForm
set backendPath=%mainPath%src\BackendApi
set semver=product-%arg1%_version
set semverPath=%mainPath%%semver%
if not exist %semver% mkdir %semver%

cd ..
echo %mainPath%
cd %backendPath%
dotnet publish --configuration Release --output %semverPath%
cd %PostFormPath%
dotnet publish --configuration Release --output %semverPath%


cd %semverPath%

break>start.bat
echo start dotnet PostForm.dll>> start.bat

echo start dotnet BackendApi.dll>> start.bat

break>stop.bat
echo TASKKILL /IM dotnet.exe >>stop.bat
if not exist config mkdir config
cd config
break>config.json
echo { "Api" : {"port" : 5000},"Service" : {"port" : 5003}}>>config.json