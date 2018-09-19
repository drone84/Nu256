@echo off

del *.lst
64tass boot_test.asm --long-address --intel-hex -o kernel.hex --list kernel.lst
if errorlevel 1 goto end

copy kernel.hex ..\bin\debug\roms
copy kernel.lst ..\bin\debug\roms

:end
pause

