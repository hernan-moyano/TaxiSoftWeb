# Restaura un backup de la base de datos TaxiSoft.
# Uso: .\restore.ps1                 (restaura el backup más reciente de .\backups)
# Uso: .\restore.ps1 -Archivo backups\manual_20260101_120000.dump
param(
    [string]$Archivo = ""
)

$ErrorActionPreference = "Stop"

function Get-EnvValue {
    param([string]$Key)
    $line = Select-String -Path ".env" -Pattern "^$Key=" -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($line) { return $line.Line.Substring($Key.Length + 1).Trim() }
    return ""
}

$dbName = Get-EnvValue "POSTGRES_DB"
$dbUser = Get-EnvValue "POSTGRES_USER"

if (-not $dbName -or -not $dbUser) {
    Write-Host "No se encontró el archivo .env o le faltan POSTGRES_DB/POSTGRES_USER." -ForegroundColor Red
    exit 1
}

if (-not $Archivo) {
    $Archivo = Get-ChildItem "backups" -Filter "*.dump" -ErrorAction SilentlyContinue |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1 | ForEach-Object { "backups\$($_.Name)" }
}

if (-not $Archivo -or -not (Test-Path $Archivo)) {
    Write-Host "No se encontró ningún archivo .dump en .\backups. Especifique uno con -Archivo." -ForegroundColor Red
    exit 1
}

$nombreContenedor = Split-Path $Archivo -Leaf
Write-Host "Se restaurará '$Archivo' en la base '$dbName'." -ForegroundColor Yellow
Write-Host "ATENCIÓN: esto reemplaza los datos actuales. Presione Enter para continuar o Ctrl+C para cancelar."
Read-Host | Out-Null

cmd /c "docker compose exec -T db pg_restore --clean --if-exists -U `"$dbUser`" -d `"$dbName`" `/backups/$nombreContenedor`"

if ($LASTEXITCODE -eq 0) {
    Write-Host "Restauración completada." -ForegroundColor Green
} else {
    Write-Host "La restauración finalizó con advertencias o errores (código $LASTEXITCODE). Revise el mensaje anterior." -ForegroundColor Red
}
