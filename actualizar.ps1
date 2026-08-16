# Actualización de TaxiSoftWeb a una nueva versión.
# Reconstruye la imagen y vuelve a levantar los servicios sin perder los datos.
$ErrorActionPreference = "Stop"

Write-Host "=== Actualización de TaxiSoft ===" -ForegroundColor Cyan

docker info *> $null
if ($LASTEXITCODE -ne 0) {
    Write-Host "Docker no está en ejecución. Inicie Docker Desktop." -ForegroundColor Red
    exit 1
}

Write-Host "Reconstruyendo la imagen..." -ForegroundColor Cyan
docker compose build
if ($LASTEXITCODE -ne 0) { Write-Host "Error al construir." -ForegroundColor Red; exit 1 }

Write-Host "Aplicando la actualización..." -ForegroundColor Cyan
docker compose up -d

Write-Host "Actualización completada. La base de datos no se modifica (los datos se conservan)." -ForegroundColor Green
