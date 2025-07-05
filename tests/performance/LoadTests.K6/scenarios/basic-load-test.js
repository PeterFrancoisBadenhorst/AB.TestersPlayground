import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate } from 'k6/metrics';

// Custom metrics
const errorRate = new Rate('errors');

// Test configuration
export const options = {
  stages: [
    { duration: '2m', target: 10 }, // Ramp up to 10 users
    { duration: '5m', target: 10 }, // Stay at 10 users
    { duration: '2m', target: 20 }, // Ramp up to 20 users
    { duration: '5m', target: 20 }, // Stay at 20 users
    { duration: '2m', target: 0 },  // Ramp down to 0 users
  ],
  thresholds: {
    http_req_duration: ['p(95)<2000'], // 95% of requests should be below 2s
    http_req_failed: ['rate<0.1'],     // Error rate should be less than 10%
    errors: ['rate<0.1'],
  },
};

// Base URL - can be overridden with environment variable
const BASE_URL = __ENV.BASE_URL || 'https://localhost:7243';

export default function () {
  // Test the weather forecast endpoint
  const response = http.get(`${BASE_URL}/weatherforecast`);
  
  const result = check(response, {
    'status is 200': (r) => r.status === 200,
    'response time < 2000ms': (r) => r.timings.duration < 2000,
    'response has data': (r) => r.body.length > 0,
  });

  errorRate.add(!result);

  // Test the health check endpoint
  const healthResponse = http.get(`${BASE_URL}/health`);
  
  check(healthResponse, {
    'health check status is 200': (r) => r.status === 200,
    'health check response time < 500ms': (r) => r.timings.duration < 500,
  });

  // Simulate user think time
  sleep(1);
}

export function handleSummary(data) {
  return {
    'summary.json': JSON.stringify(data, null, 2),
    stdout: `
    ============================================
    Basic Load Test Summary
    ============================================
    Total Checks: ${data.metrics.checks.values.passes + data.metrics.checks.values.fails}
    Check Success Rate: ${(data.metrics.checks.values.rate * 100).toFixed(2)}%
    Average Response Time: ${data.metrics.http_req_duration.values.avg.toFixed(2)}ms
    95th Percentile: ${data.metrics.http_req_duration.values['p(95)'].toFixed(2)}ms
    Error Rate: ${(data.metrics.http_req_failed.values.rate * 100).toFixed(2)}%
    ============================================
    `,
  };
}
