$TDD_DIR = ".\CodeCoverage"
$PACKAGE_ROOT = Join-Path $env:USERPROFILE -ChildPath ".nuget\packages"
$COVERAGE_REPORT_TOOL = Join-Path $PACKAGE_ROOT -ChildPath "reportgenerator\5.5.1\tools\net10.0\ReportGenerator.exe"
$COVERAGE_REPORT = Join-Path $TDD_DIR -ChildPath "index.html"

Remove-Item -Path $TDD_DIR -Recurse -Force

dotnet build -c Release --nologo
dotnet test -c Release --nologo --no-build --collect:"XPlat Code Coverage" --results-directory $TDD_DIR

$REPORT = Get-ChildItem -Path $TDD_DIR -Filter coverage.cobertura.xml -Recurse

& $COVERAGE_REPORT_TOOL -reports:$REPORT -targetdir:$TDD_DIR -verbosity:Warning
# start $COVERAGE_REPORT