:::::::::::::::::::::::::::::::::::::::::::::::::::
:::::::::    깃헙 푸시 배치파일           :::::::::
:::::::::::::::::::::::::::::::::::::::::::::::::::

@echo off

chcp 65001
cls

set /p memo="업로드 내용 > "

cd .

git pull origin main

git add .

git commit -m "[%date%] %memo%"

git push origin main

echo.======================
echo. 깃헙 업로드 완료 !
echo.======================

pause > nul
