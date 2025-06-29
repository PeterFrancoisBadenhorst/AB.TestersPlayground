#!/bin/bash
set -e

# Wait for API to be available
echo "Waiting for API to be available..."
until $(curl --output /dev/null --silent --head --fail http://nginx/api/weatherforecast); do
  printf '.'
  sleep 5
done

echo "Starting ZAP scan..."

# Run ZAP baseline scan against API
zap-baseline.py -t http://nginx/api/ -r zap-report.html -I

echo "ZAP scan completed."
