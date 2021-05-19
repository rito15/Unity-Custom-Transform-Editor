:::::::::::::::::::::::::::::::::::::::::::::::::::::
::::::::: .gitignore 변경사항 적용 배치파일 :::::::::
:::::::::::::::::::::::::::::::::::::::::::::::::::::

@echo off

chcp 65001
cls

git rm -r --cached .
git add .

git commit -m "[%date%] Update gitignore"

git push origin main

echo.==========================
echo. gitignore 업데이트 완료!
echo.==========================

pause > nul
