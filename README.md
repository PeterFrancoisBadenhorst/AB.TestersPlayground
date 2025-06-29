# Testers Playground

A playground project to help people improve their automation testing skills, featuring a bogus NFT shop. Includes:

- .NET 10 Web API backend
- React and Blazor frontends
- Nginx load balancer
- PostgreSQL database
- Testing tools: nBomber, k6, OWASP ZAP

## Getting Started

1. Install Docker and Docker Compose.
2. Clone this repository.
3. Build and run all services:
   ```sh
   docker-compose up --build
   ```
4. Access the services:
   - API: http://localhost:5000
   - React Frontend: http://localhost:3000
   - Blazor Frontend: http://localhost:5002
   - Nginx (Load Balancer): http://localhost

## Folder Structure
- `api/` - .NET 10 Web API backend
- `frontend-react/` - React frontend
- `frontend-blazor/` - Blazor frontend
- `nginx/` - Nginx config
- `tests/` - Testing tools (nBomber, k6, zap)

## Testing
- nBomber, k6, and zap are set up as Docker services for load, performance, and security testing.

## Requirements
- Docker
- Docker Compose
- .NET 10 SDK (for local development)

---

This project is for educational and testing purposes only. No real NFTs are bought or sold.
