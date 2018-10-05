@echo off

md ..\bin
md ..\bin\debug
md ..\bin\debug\roms

:start
del *.lst
64tass kernel.asm --long-address --intel-hex -o kernel.hex --list kernel.lst
if errorlevel 1 goto fail

copy kernel.hex ..\bin\debug\roms
copy kernel.lst ..\bin\debug\roms

:fail
choice /m "Try again?"
if errorlevel 2 goto end
goto start

:end
echo END OF LINE
