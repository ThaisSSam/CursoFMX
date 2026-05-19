#Requires -Version 5.1
<#
.SYNOPSIS
    Verifica se a maquina esta pronta para o curso (SDK .NET 8, Docker, build e testes).
.EXAMPLE
    .\scripts\verificar-ambiente.ps1
#>
$ErrorActionPreference = "Stop"
$raiz = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
Set-Location $raiz

Write-Host "=== Verificacao do ambiente - Projeto Treinamento (.NET 8) ===" -ForegroundColor Cyan
Write-Host "Pasta: $raiz`n"

$ok = $true

function Test-Command {
    param(
        [string]$Nome,
        [scriptblock]$ScriptBlock
    )
    try {
        & $ScriptBlock
        Write-Host "[OK] $Nome" -ForegroundColor Green
    }
    catch {
        Write-Host "[FALHA] $Nome - $($_.Exception.Message)" -ForegroundColor Red
        $script:ok = $false
    }
}

Test-Command -Nome "SDK .NET 8" -ScriptBlock {
    $sdk = dotnet --version
    if ($sdk -notmatch '^8\.') {
        throw "SDK atual: $sdk. Instale o .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0"
    }
    Write-Host "       dotnet --version = $sdk"
    if (Test-Path "$raiz\global.json") {
        $globalSdk = (Get-Content "$raiz\global.json" -Raw | ConvertFrom-Json).sdk.version
        Write-Host "       global.json referencia SDK $globalSdk (faixa 8.0.x)"
    }
}

Test-Command -Nome "Runtime .NET 8" -ScriptBlock {
    $runtimes = dotnet --list-runtimes | Out-String
    $temAspNet8 = $runtimes -match "Microsoft\.AspNetCore\.App\s+8\."
    $temNetCore8 = $runtimes -match "Microsoft\.NETCore\.App\s+8\."
    if (-not $temNetCore8) {
        throw "Runtime Microsoft.NETCore.App 8.x nao encontrado. Instale o .NET 8 SDK."
    }
    if (-not $temAspNet8) {
        Write-Host "       Aviso: Microsoft.AspNetCore.App 8.x nao listado; o SDK 8 costuma incluir o runtime." -ForegroundColor Yellow
    }
}

Test-Command -Nome "Docker" -ScriptBlock {
    docker version --format "{{.Server.Version}}" | Out-Null
}

Test-Command -Nome "dotnet restore" -ScriptBlock {
    if (-not (Test-Path "Treinamento.sln")) {
        throw "Arquivo Treinamento.sln nao encontrado na raiz do repositorio."
    }
    dotnet restore Treinamento.sln 2>&1 | Out-Null
}

Test-Command -Nome "dotnet build" -ScriptBlock {
    dotnet build Treinamento.sln --no-restore -v q
}

Test-Command -Nome "dotnet test" -ScriptBlock {
    dotnet test Treinamento.sln --no-build -v q
}

Write-Host ""
if ($ok) {
    Write-Host "Ambiente pronto para o curso." -ForegroundColor Green
    Write-Host "Proximos passos:"
    Write-Host "  docker compose up -d"
    Write-Host "  dotnet run --project src\Treinamento.API\Treinamento.API.csproj"
    exit 0
}

Write-Host "Corrija os itens acima antes de iniciar o Modulo 1." -ForegroundColor Yellow
exit 1
