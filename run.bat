@echo off
echo Limpiando el proyecto...
dotnet clean

echo ==========================
echo Compilando el proyecto...
dotnet build

echo ==========================
echo Ejecutando el proyecto...
dotnet run

pause