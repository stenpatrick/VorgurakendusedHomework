@echo off
docker rm my-db
docker run -d -p 5432:5432 --name my-db -e POSTGRES_PASSWORD=parool postgres
cd /d "teamtrack-frontend"
start "" "teamtrackfrontend.bat"
cd /d "..\teamtrack-api\"
start "" "teamtrackapi.bat"
cd /d  "C:\Program Files\Mozilla Firefox\"
firefox.exe http://localhost:5173/
firefox.exe http://localhost:5183/swagger/index.html