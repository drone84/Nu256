@echo off

del *.lst
64tass kernel.asm --long-address --intel-hex -o kernel.hex --list kernel.lst
if errorlevel 1 goto end

:end
pause

