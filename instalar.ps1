# Instalación de TaxiSoftWeb en la PC del cliente (requiere Docker Desktop).
# Levanta la aplicación + base de datos + backup automático.
$ErrorActionPreference = "Stop"

Write-Host "=== Instalación de TaxiSoft ===" -ForegroundColor Cyan

# 1. Verificar Docker
docker info *> $null
if ($LASTEXITCODE -ne 0) {
    Write-Host "Docker no está en ejecución. Inicie Docker Desktop y vuelva a ejecutar este script." -ForegroundColor Red
    exit 1
}
Write-Host "[OK] Docker en ejecución" -ForegroundColor Green

# 2. Verificar archivo .env
if (-not (Test-Path ".env")) {
    Copy-Item ".env.example" ".env"
    Write-Host "Se creó el archivo .env a partir de .env.example." -ForegroundColor Yellow
    Write-Host "Edítelo para cambiar POSTGRES_PASSWORD antes de continuar." -ForegroundColor Yellow
    notepad ".env"
    Read-Host "Presione Enter cuando haya configurado el .env" | Out-Null
}

# 3. Levantar los servicios
Write-Host "Construyendo y levantando los servicios (primera vez puede tardar varios minutos)..." -ForegroundColor Cyan
docker compose up -d --build
if ($LASTEXITCODE -ne 0) {
    Write-Host "Error al levantar los servicios." -ForegroundColor Red
    exit 1
}

# 4. Esperar a que la aplicación responda
Write-Host "Esperando que la aplicación esté disponible..." -ForegroundColor Cyan
$puerto = (Select-String -Path ".env" -Pattern "^PUERTO_APP=" | Select-Object -First 1).Line.Split("=")[1]
if (-not $puerto) { $puerto = "8080" }
$ok = $false
for ($i = 0; $i -lt 60; $i++) {
    try {
        $r = Invoke-WebRequest -Uri "http://localhost:$puerto/healthz" -UseBasicParsing -TimeoutSec 5
        if ($r.StatusCode -eq 200) { $ok = $true; break }
    } catch { Start-Sleep -Seconds 2 }
}
if (-not $ok) {
    Write-Host "La aplicación no respondió a tiempo. Revise los logs con: docker compose logs app" -ForegroundColor Red
    exit 1
}

Write-Host "[OK] Aplicación disponible" -ForegroundColor Green

# 5. Mostrar accesos
$ip = (Get-NetIPAddress -AddressFamily IPv4 -ErrorAction SilentlyContinue |
    Where-Object { $_.IPAddress -notlike "127.*" -and $_.IPAddress -notlike "169.254.*" } |
    Select-Object -First 1).IPAddress

Write-Host ""
Write-Host "=== Acceso a TaxiSoft ===" -ForegroundColor Green
Write-Host "  En esta PC : http://localhost:$puerto"
if ($ip) {
    Write-Host "  En la red  : http://$ip`:$puerto"
    Write-Host "  (otros equipos de la red local pueden abrir esa dirección)"
}
Write-Host ""
Write-Host "Los backups automáticos se guardan en .\backups (diario 02:00)."
Write-Host "Backup manual:  .\backup-manual.ps1"
Write-Host "Restaurar:      .\restore.ps1"
