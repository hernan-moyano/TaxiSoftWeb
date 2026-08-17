# Backup manual de la base de datos TaxiSoft
# Genera un archivo .dump (formato custom de pg_dump) en la carpeta .\backups
$ErrorActionPreference = "Stop"

function Get-EnvValue {
    param([string]$Key)
    $line = Select-String -Path ".env" -Pattern "^$Key=" -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($line) { return $line.Line.Substring($Key.Length + 1).Trim() }
    return ""
}

$dbName   = Get-EnvValue "POSTGRES_DB"
$dbUser   = Get-EnvValue "POSTGRES_USER"

if (-not $dbName -or -not $dbUser) {
    Write-Host "No se encontró el archivo .env o le faltan POSTGRES_DB/POSTGRES_USER." -ForegroundColor Red
    exit 1
}

New-Item -ItemType Directory -Force -Path "backups" | Out-Null
$fecha = Get-Date -Format "yyyyMMdd_HHmmss"
$nombre = "manual_$fecha.dump"

Write-Host "Generando backup de la base '$dbName'..." -ForegroundColor Cyan
# El redireccionamiento por cmd mantiene el binario intacto
cmd /c "docker compose exec -T db pg_dump --format=custom --compress=9 -U `"$dbUser`" -d `"$dbName`" > `"backups\$nombre`""

if ($LASTEXITCODE -eq 0) {
    $tamano = (Get-Item "backups\$nombre").Length / 1MB
    Write-Host "Backup creado: backups\$nombre ($([math]::Round($tamano,2)) MB)" -ForegroundColor Green
} else {
    Write-Host "Error al generar el backup." -ForegroundColor Red
    exit 1
}
