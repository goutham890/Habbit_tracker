@echo off
set "APP_HTML=%~dp0index.html"
set "USER_DATA=%LOCALAPPDATA%\HabitTrackerProfile"
set "EDGE_EXE=C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"

if not exist "%EDGE_EXE%" (
    set "EDGE_EXE=C:\Program Files\Microsoft\Edge\Application\msedge.exe"
)

start "" "%EDGE_EXE%" ^
  --app="file:///%APP_HTML%" ^
  --window-size=1520,950 ^
  --user-data-dir="%USER_DATA%" ^
  --no-first-run ^
  --no-default-browser-check ^
  --disable-extensions ^
  --disable-background-networking ^
  --disable-component-update ^
  --disable-sync ^
  --enable-gpu-rasterization ^
  --enable-zero-copy ^
  --disable-features=Translate,OptimizationHints,MediaRouter
