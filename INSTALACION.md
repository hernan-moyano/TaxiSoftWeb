# TaxiSoft — Instalación en la PC del cliente

Aplicación interna para red local. Se distribuye con Docker: **la PC del cliente solo necesita tener instalado Docker Desktop** (Windows) o Docker Engine. No requiere instalar PostgreSQL ni .NET.

> Para documentación de desarrollo (estructura, puesta en marcha, arquitectura) ver [`README.md`](README.md).

## Requisitos

- Windows 10/11 (o Linux) con **Docker Desktop** instalado y en ejecución.
- Docker con contenedores **Linux** (opción por defecto).

## Instalación (primera vez)

1. Copiar la carpeta del proyecto a la PC del cliente (o copiar el repositorio).
2. Doble clic en **`instalar.ps1`** (o ejecutarlo desde PowerShell):
   - Verifica que Docker esté corriendo.
   - Si no existe `.env`, lo crea a partir de `.env.example` y abre el editor para configurar la contraseña de la base de datos.
   - Construye y levanta los 3 servicios (base de datos, aplicación y backup).
   - Espera a que la app responda y muestra la dirección de acceso.

> Si Windows bloquea el script, ejecutar una vez: `Set-ExecutionPolicy -Scope Process Bypass` y volver a ejecutar.

## Acceso

- **En la misma PC:** `http://localhost:8080`
- **Desde otros equipos de la red:** `http://IP-DE-LA-PC:8080` (la IP se muestra al final de la instalación).

Si el puerto 8080 está ocupado, cambiarlo en `.env` → `PUERTO_APP`.

## Servicios que se levantan

| Servicio | Función |
|---|---|
| `app` | Aplicación web (puerto 8080) |
| `db` | PostgreSQL 17 con volumen persistente en `.\data\postgres` |
| `backup` | Copia de seguridad automática de la BD |

La base de datos se crea y migra automáticamente al primer arranque (tablas + datos iniciales). No hace falta ningún paso manual.

## Backups

- **Automáticos:** todos los días a las 02:00 en `.\backups` (configurable en `.env` → `BACKUP_SCHEDULE`). Se conservan 7 diarios y 4 semanales.
- **Manual:** ejecutar `.\backup-manual.ps1`.
- **Restaurar:** `.\restore.ps1` (restaura el último backup, o `.\restore.ps1 -Archivo backups\manual_XXXX.dump`). **Atención: reemplaza los datos actuales.**

Para copiar los datos a otro disco, alcanza con copiar la carpeta `.\backups`.

## Actualización a una nueva versión

Reemplazar los archivos de la aplicación (sin tocar `.\data` ni `.\backups`) y ejecutar **`.\actualizar.ps1`**. Los datos se conservan.

## Solución de problemas

| Problema | Solución |
|---|---|
| La app no abre | `docker compose logs app` para ver el error |
| Verificar si la app responde | `Invoke-WebRequest http://localhost:8080/healthz` (debe devolver 200) |
| "No se pudo aplicar la migración" | Esperar a que `db` esté sano: `docker compose ps` |
| Quiero borrar todo y empezar de cero | `docker compose down -v` (borra también los datos) |
| Cambié `.env` | `docker compose up -d` para aplicar los cambios |

## Notas de seguridad

- La app se sirve por **HTTP** dentro de la red local. No exponer el puerto 8080 hacia Internet.
- El puerto de la base de datos (5433) solo es accesible desde la propia PC, para administración.
- La contraseña de la base está en el archivo `.env` (no se sube a git). Mantenerlo fuera de copias públicas.
