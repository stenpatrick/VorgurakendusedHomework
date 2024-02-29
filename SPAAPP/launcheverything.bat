@echo off
cd /d "teamtrack-frontend"
start "" "teamtrackfrontend.bat"
cd /d "..\teamtrack-api\"
start "" "teamtrackapi.bat"
cd /d  "C:\Program Files\Mozilla Firefox\"
firefox.exe http://localhost:5173/
