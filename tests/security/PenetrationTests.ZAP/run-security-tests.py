#!/usr/bin/env python3
"""
OWASP ZAP Automation Script for Testers Playground API
Performs automated security testing including spidering, active scanning, and reporting
"""

import time
import sys
import yaml
import json
from zapv2 import ZAPv2
import requests
from pathlib import Path

def load_config():
    """Load configuration from YAML file"""
    config_path = Path(__file__).parent / "zap-config.yml"
    with open(config_path, 'r') as file:
        return yaml.safe_load(file)

def wait_for_zap(zap_proxy, timeout=60):
    """Wait for ZAP to be ready"""
    print("Waiting for ZAP to start...")
    start_time = time.time()
    
    while time.time() - start_time < timeout:
        try:
            response = requests.get(f"{zap_proxy}/JSON/core/view/version/")
            if response.status_code == 200:
                print("ZAP is ready!")
                return True
        except requests.exceptions.ConnectionError:
            pass
        time.sleep(2)
    
    print("Timeout waiting for ZAP to start")
    return False

def setup_context(zap, config):
    """Setup ZAP context with include/exclude patterns"""
    context_name = config['context']['name']
    
    # Create context
    context_id = zap.context.new_context(context_name)
    
    # Add include patterns
    for pattern in config['context']['include_regex']:
        zap.context.include_in_context(context_name, pattern)
    
    # Add exclude patterns  
    for pattern in config['context']['exclude_regex']:
        zap.context.exclude_from_context(context_name, pattern)
        
    return context_id

def run_spider(zap, target_url, config):
    """Run ZAP spider against target"""
    print(f"Starting spider scan of {target_url}")
    
    spider_config = config['spider']
    scan_id = zap.spider.scan(
        target_url,
        maxchildren=spider_config['max_children'],
        recurse=True,
        contextname=config['context']['name']
    )
    
    # Wait for spider to complete
    while int(zap.spider.status(scan_id)) < 100:
        progress = zap.spider.status(scan_id)
        print(f"Spider progress: {progress}%")
        time.sleep(5)
    
    print("Spider scan completed")
    return zap.spider.results(scan_id)

def run_active_scan(zap, target_url, config):
    """Run ZAP active scan against target"""
    print(f"Starting active scan of {target_url}")
    
    scan_id = zap.ascan.scan(
        target_url,
        recurse=True,
        contextid=config['context']['name']
    )
    
    # Wait for active scan to complete
    while int(zap.ascan.status(scan_id)) < 100:
        progress = zap.ascan.status(scan_id)
        print(f"Active scan progress: {progress}%")
        time.sleep(10)
    
    print("Active scan completed")

def generate_reports(zap, config):
    """Generate security test reports"""
    output_dir = Path(config['reports']['output_dir'])
    output_dir.mkdir(parents=True, exist_ok=True)
    
    formats = config['reports']['formats']
    
    if 'HTML' in formats:
        html_report = zap.core.htmlreport()
        with open(output_dir / 'security-report.html', 'w') as f:
            f.write(html_report)
        print(f"HTML report saved to {output_dir}/security-report.html")
    
    if 'XML' in formats:
        xml_report = zap.core.xmlreport()
        with open(output_dir / 'security-report.xml', 'w') as f:
            f.write(xml_report)
        print(f"XML report saved to {output_dir}/security-report.xml")
    
    if 'JSON' in formats:
        json_report = zap.core.jsonreport()
        with open(output_dir / 'security-report.json', 'w') as f:
            f.write(json_report)
        print(f"JSON report saved to {output_dir}/security-report.json")

def check_alerts(zap, config):
    """Check alerts against thresholds and return exit code"""
    alerts = zap.core.alerts()
    
    risk_counts = {'High': 0, 'Medium': 0, 'Low': 0, 'Informational': 0}
    
    for alert in alerts:
        risk = alert['risk']
        risk_counts[risk] = risk_counts.get(risk, 0) + 1
    
    print("\nSecurity Alert Summary:")
    for risk, count in risk_counts.items():
        print(f"{risk}: {count}")
    
    # Check against thresholds
    thresholds = config['alert_thresholds']
    failed = False
    
    if risk_counts['High'] > thresholds['high']:
        print(f"FAIL: High risk alerts ({risk_counts['High']}) exceed threshold ({thresholds['high']})")
        failed = True
    
    if risk_counts['Medium'] > thresholds['medium']:
        print(f"FAIL: Medium risk alerts ({risk_counts['Medium']}) exceed threshold ({thresholds['medium']})")
        failed = True
    
    if risk_counts['Low'] > thresholds['low']:
        print(f"FAIL: Low risk alerts ({risk_counts['Low']}) exceed threshold ({thresholds['low']})")
        failed = True
    
    return 1 if failed else 0

def main():
    """Main security testing workflow"""
    try:
        # Load configuration
        config = load_config()
        
        # Initialize ZAP client
        zap = ZAPv2(proxies={'http': config['zap_proxy'], 'https': config['zap_proxy']})
        
        # Wait for ZAP to be ready
        if not wait_for_zap(config['zap_proxy']):
            print("Failed to connect to ZAP")
            return 1
        
        target_url = config['target_url']
        print(f"Starting security test of {target_url}")
        
        # Setup context
        setup_context(zap, config)
        
        # Run spider scan
        spider_results = run_spider(zap, target_url, config)
        print(f"Spider found {len(spider_results)} URLs")
        
        # Run passive scan (automatically enabled)
        print("Passive scan running in background...")
        
        # Run active scan
        run_active_scan(zap, target_url, config)
        
        # Wait a bit for passive scan to complete
        time.sleep(10)
        
        # Generate reports
        generate_reports(zap, config)
        
        # Check alerts and determine exit code
        exit_code = check_alerts(zap, config)
        
        print("\nSecurity testing completed!")
        return exit_code
        
    except Exception as e:
        print(f"Error during security testing: {str(e)}")
        return 1

if __name__ == "__main__":
    exit_code = main()
    sys.exit(exit_code)
