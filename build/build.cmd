@call dotnet --info

@call dotnet restore -v m ../

@if ERRORLEVEL 1 (
echo Error! Restoring dependicies failed.
exit /b 1
) else (
echo Restoring dependicies was successful.
)

@set project=..\src\TL.FeatureToggles.FeatureToggle\TL.FeatureToggles.csproj

@call dotnet build -c Release %project%
@if ERRORLEVEL 1 (
echo Error! Build sample library failed.
exit /b 1
)

@set project=..\tests\TL.FeatureToggles.Tests\TL.FeatureToggles.Tests.csproj

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build Tests failed.
exit /b 1
)
