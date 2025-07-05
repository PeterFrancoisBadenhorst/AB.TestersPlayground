#!/bin/bash
# Setup script for ZAP security tests

echo "Setting up OWASP ZAP Security Tests..."

# Check if Python is available
if ! command -v python &> /dev/null; then
    echo "Python is required but not installed. Please install Python 3.7+"
    exit 1
fi

# Install Python dependencies
echo "Installing Python dependencies..."
pip install -r requirements.txt

# Check if ZAP is available
if ! command -v zap.sh &> /dev/null && ! command -v zap.bat &> /dev/null; then
    echo "OWASP ZAP is not installed or not in PATH."
    echo "Please download and install ZAP from: https://www.zaproxy.org/download/"
    echo "Make sure zap.sh (Linux/Mac) or zap.bat (Windows) is in your PATH"
    exit 1
fi

echo "Setup complete!"
echo ""
echo "To run security tests:"
echo "1. Start OWASP ZAP (GUI or headless)"
echo "2. Start your application"
echo "3. Run: python run-security-tests.py"
echo ""
echo "For headless ZAP:"
echo "zap.sh -daemon -host 127.0.0.1 -port 8080 -config api.addrs.addr.name=.* -config api.addrs.addr.regex=true"
