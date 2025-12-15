# Run Both Web and API Projects
# This script starts both the Web application and API simultaneously

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Starting CommonArchitecture Projects" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Get the script directory
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$solutionRoot = Split-Path -Parent $scriptPath

# Project paths
$apiPath = Join-Path $solutionRoot "src\CommonArchitecture.API"
$webPath = Join-Path $solutionRoot "src\CommonArchitecture.Web"

# Check if projects exist
if (-not (Test-Path $apiPath)) {
    Write-Host "ERROR: API project not found at $apiPath" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $webPath)) {
    Write-Host "ERROR: Web project not found at $webPath" -ForegroundColor Red
    exit 1
}

Write-Host "API Project: $apiPath" -ForegroundColor Green
Write-Host "Web Project: $webPath" -ForegroundColor Green
Write-Host ""

# Start API in a new window
Write-Host "Starting API (http://localhost:5089)..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$apiPath'; Write-Host 'API Server Starting...' -ForegroundColor Cyan; dotnet run"

# Wait a bit for API to start
Write-Host "Waiting 5 seconds for API to initialize..." -ForegroundColor Yellow
Start-Sleep -Seconds 5

# Start Web in a new window
Write-Host "Starting Web Application (http://localhost:5000)..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$webPath'; Write-Host 'Web Application Starting...' -ForegroundColor Cyan; dotnet run"

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "Both projects are starting!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "API:  http://localhost:5089" -ForegroundColor Cyan
Write-Host "Web:  http://localhost:5000" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press any key to stop all projects..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

# Stop all dotnet processes (optional - user can close windows manually)
Write-Host "To stop the servers, close the PowerShell windows." -ForegroundColor Yellow
