import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
  stages: [
    { duration: '30s', target: 10 }, // Ramp up to 10 users over 30 seconds
    { duration: '1m', target: 10 },  // Stay at 10 users for 1 minute
    { duration: '30s', target: 0 },  // Ramp down to 0 users over 30 seconds
  ],
  thresholds: {
    http_req_duration: ['p(95)<500'], // 95% of requests must complete below 500ms
    http_req_failed: ['rate<0.1'],    // Error rate must be below 10%
  },
};

export default function () {
  // Test the API endpoints through nginx proxy
  let apiResponse = http.get('http://nginx/api/test');
  check(apiResponse, {
    'API status is 200': (r) => r.status === 200,
    'API response time < 500ms': (r) => r.timings.duration < 500,
  });

  // Test the API info endpoint
  let infoResponse = http.get('http://nginx/api/info');
  check(infoResponse, {
    'API info status is 200': (r) => r.status === 200,
    'API info response time < 500ms': (r) => r.timings.duration < 500,
  });

  // Test the Blazor frontend through nginx proxy
  let blazorResponse = http.get('http://nginx/blazor/');
  check(blazorResponse, {
    'Blazor frontend status is 200': (r) => r.status === 200,
    'Blazor response time < 2000ms': (r) => r.timings.duration < 2000,
  });

  // Test the React frontend through nginx proxy
  let reactResponse = http.get('http://nginx/');
  check(reactResponse, {
    'React frontend status is 200': (r) => r.status === 200,
    'React response time < 2000ms': (r) => r.timings.duration < 2000,
  });

  sleep(1);
}
