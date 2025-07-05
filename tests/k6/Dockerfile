FROM grafana/k6:latest

# Copy k6 test scripts
COPY . /tests

# Set working directory
WORKDIR /tests

# Default command to run load tests
CMD ["run", "load-test.js"]
