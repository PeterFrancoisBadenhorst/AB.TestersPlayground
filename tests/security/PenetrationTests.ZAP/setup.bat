@echo off
REM Setup script for ZAP security tests on Windows

echo Setting up OWASP ZAP Security Tests...

REM Check if Python is available
python --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Python is required but not installed. Please install Python 3.7+
    exit /b 1
)

REM Install Python dependencies
echo Installing Python dependencies...
pip install -r requirements.txt

REM Check if ZAP is available
where zap.bat >nul 2>&1
if %errorlevel% neq 0 (
    echo OWASP ZAP is not installed or not in PATH.
    echo Please download and install ZAP from: https://www.zaproxy.org/download/
    echo Make sure zap.bat is in your PATH
    exit /b 1
)

echo Setup complete!
echo.
echo To run security tests:
echo 1. Start OWASP ZAP (GUI or headless)
echo 2. Start your application
echo 3. Run: python run-security-tests.py
echo.
echo For headless ZAP:
echo zap.bat -daemon -host 127.0.0.1 -port 8080 -config api.addrs.addr.name=.* -config api.addrs.addr.regex=true
