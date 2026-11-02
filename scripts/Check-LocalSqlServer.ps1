$ErrorActionPreference = "Continue"

Write-Host ""
Write-Host "Ingenio local QA - prerequisite check" -ForegroundColor Cyan
Write-Host "This script is read-only. It does not install or configure software."
Write-Host ""

Write-Host "== SQL Server services =="
$services = Get-Service -Name 'MSSQL*','SQLBrowser','SQLAgent*' -ErrorAction SilentlyContinue
if ($services) {
    $services | Sort-Object Name | Format-Table Name, Status, DisplayName -AutoSize
} else {
    Write-Host "No SQL Server services were detected."
}

Write-Host ""
Write-Host "== SQL command-line tools =="
foreach ($tool in @('sqlcmd','ssms')) {
    $found = Get-Command $tool -ErrorAction SilentlyContinue
    if ($found) {
        Write-Host "$tool : $($found.Source)" -ForegroundColor Green
    } else {
        Write-Host "$tool : not found in PATH" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "== .NET SDKs =="
$dotnet = Get-Command dotnet -ErrorAction SilentlyContinue
if ($dotnet) {
    dotnet --list-sdks
} else {
    Write-Host "dotnet CLI not found." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "== Registered SQL Server instances =="
$instanceKey = 'HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL'
if (Test-Path $instanceKey) {
    $instances = Get-ItemProperty $instanceKey
    $instances.PSObject.Properties |
        Where-Object { $_.Name -notmatch '^PS' } |
        ForEach-Object { Write-Host "$($_.Name) -> $($_.Value)" }
} else {
    Write-Host "No 64-bit registered SQL Server instances found."
}

Write-Host ""
Write-Host "Next: follow LOCAL_QA_SETUP.md. If the engine is missing, install SQL Server Developer Edition from Microsoft's official download page."
