#Requires -Version 5.1
<#
.SYNOPSIS
    Encerra instancias da Treinamento.API que bloqueiam o build (MSB3026).
.EXAMPLE
    .\scripts\parar-api.ps1
#>
$processos = Get-Process -Name "Treinamento.API" -ErrorAction SilentlyContinue
if (-not $processos) {
    $processos = Get-CimInstance Win32_Process -Filter "Name = 'Treinamento.API.exe'" -ErrorAction SilentlyContinue
}

if (-not $processos) {
    Write-Host "Nenhum processo Treinamento.API em execucao." -ForegroundColor Green
    exit 0
}

foreach ($p in $processos) {
    Write-Host "Encerrando Treinamento.API (PID $($p.ProcessId))..."
    Stop-Process -Id $p.ProcessId -Force -ErrorAction SilentlyContinue
}

Start-Sleep -Seconds 1
Write-Host "Processos encerrados. Voce pode compilar novamente: dotnet build Treinamento.sln" -ForegroundColor Green
