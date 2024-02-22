@echo off
docker start a41cfa0e660a
cd /d "teamtrack-frontend"
start "" "teamtrackfrontend.bat"
cd /d "..\teamtrack-api\"
start "" "teamtrackapi.bat"