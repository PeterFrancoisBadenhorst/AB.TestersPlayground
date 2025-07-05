# OWASP ZAP Security Testing

This directory contains automated security testing scripts using OWASP ZAP (Zed Attack Proxy).

## Prerequisites

1. **Python 3.7+** with pip
2. **OWASP ZAP** - Download from [zaproxy.org](https://www.zaproxy.org/download/)

## Setup

### Quick Setup
Run the setup script for your platform:
- **Windows**: `setup.bat`
- **Linux/Mac**: `./setup.sh`

### Manual Setup
1. Install Python dependencies:
   ```bash
   pip install -r requirements.txt
   ```

2. Ensure OWASP ZAP is installed and accessible via command line

## Configuration

Edit `zap-config.yml` to customize:
- Target URL
- Scan parameters
- Report formats
- Alert thresholds

## Running Security Tests

### Option 1: GUI Mode
1. Start ZAP GUI
2. Ensure API is enabled (default port 8080)
3. Run tests:
   ```bash
   python run-security-tests.py
   ```

### Option 2: Headless Mode
1. Start ZAP in daemon mode:
   ```bash
   # Windows
   zap.bat -daemon -host 127.0.0.1 -port 8080 -config api.addrs.addr.name=.* -config api.addrs.addr.regex=true
   
   # Linux/Mac
   zap.sh -daemon -host 127.0.0.1 -port 8080 -config api.addrs.addr.name=.* -config api.addrs.addr.regex=true
   ```

2. Run tests:
   ```bash
   python run-security-tests.py
   ```

## Test Process

The security test performs the following steps:

1. **Spider Scan** - Discovers all accessible URLs
2. **Passive Scan** - Analyzes responses for vulnerabilities
3. **Active Scan** - Sends attack payloads to test for vulnerabilities
4. **Report Generation** - Creates HTML, XML, and JSON reports
5. **Alert Analysis** - Checks results against configured thresholds

## Reports

Reports are generated in the `reports/` directory:
- `security-report.html` - Human-readable HTML report
- `security-report.xml` - XML format for CI/CD integration
- `security-report.json` - JSON format for programmatic analysis

## CI/CD Integration

The script returns appropriate exit codes:
- `0` - Success (alerts within thresholds)
- `1` - Failure (alerts exceed thresholds or errors occurred)

Configure alert thresholds in `zap-config.yml` to control when tests fail.

## Common Issues

### Connection Errors
- Ensure ZAP is running and API is accessible
- Check firewall settings
- Verify target application is running

### Authentication
- Configure authentication in ZAP GUI before running tests
- Update context configuration for authenticated testing

### Performance
- Adjust scan parameters in configuration file
- Use smaller scope for faster testing during development
