@echo off

del *.lst
tass\64tass kernel.asm --long-address --intel-hex -o kernel.hex --list kernel.lst
if errorlevel 1 goto end

copy kernel.hex "..\..\Nu64 Simulator\ROMs"

:end
pause
