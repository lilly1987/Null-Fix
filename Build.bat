@echo off
setlocal

dotnet build "%~dp0Source\NullFix.csproj" --configuration Release
set "BUILD_EXIT_CODE=%ERRORLEVEL%"

if "%BUILD_EXIT_CODE%"=="0" (
    echo Build succeeded: "%~dp0Assemblies\NullFix.dll"
) else (
    echo Build failed with exit code %BUILD_EXIT_CODE%.
)

exit /b %BUILD_EXIT_CODE%
