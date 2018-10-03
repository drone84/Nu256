@echo off

del *.lst
64tass kernel.asm --long-address --intel-hex -o kernel.hex --list kernel.lst
if errorlevel 1 goto end

if not exist bin\debug\roms md ..\bin\debug\roms
copy kernel.hex ..\bin\debug\roms
copy kernel.lst ..\bin\debug\roms

:end
pause

