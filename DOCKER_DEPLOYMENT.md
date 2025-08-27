# Docker Deployment Guide for SiglatCoreAPI

## Quick Start

### Prerequisites
- Docker and Docker Compose installed
- At least 2GB RAM available for containers
- Ports 5000, 5432, and 6379 available

### Running the Application

1. **Using the management script (recommended):**
   ```bash
   # Make the script executable
   chmod +x docker-manage.sh
   
   # Start all services
   ./docker-manage.sh up
   
   # Check service health
   ./docker-manage.sh health
   
   # View logs
   ./docker-manage.sh logs
   ```

2. **Using docker-compose directly:**
   ```bash
   # Start all services
   docker-compose up -d
   
   # Check status
   docker-compose ps
   
   # View logs
   docker-compose logs -f
   ```

### Available Services

Once running, the following services will be accessible:

- **SiglatCoreAPI**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **PostgreSQL Database**: localhost:5432
- **Redis Cache**: localhost:6379

## Management Commands

The `docker-manage.sh` script provides convenient commands:

```bash
./docker-manage.sh build     # Build the Docker image
./docker-manage.sh up        # Start all services
./docker-manage.sh down      # Stop all services
./docker-manage.sh restart   # Restart all services
./docker-manage.sh logs      # Show logs
./docker-manage.sh status    # Show service status
./docker-manage.sh clean     # Clean up containers and images
./docker-manage.sh db-only   # Start only database
./docker-manage.sh api-only  # Start only API
./docker-manage.sh health    # Check service health
./docker-manage.sh migrate   # Run database migrations
```

## Environment Configuration

### Default Configuration
The system uses sensible defaults defined in `docker-compose.yml`.

### Custom Configuration
Create a `.env.docker` file to override defaults:

```env
# Database Configuration
DB_PASS=YourSecurePassword
JWT_SECRET=YourSuperSecretJWTKey

# Port Configuration
API_PORT=5000
CITIZEN_UI_PORT=3000
BFP_UI_PORT=3001
PNP_UI_PORT=3002

# Environment
ASPNETCORE_ENVIRONMENT=Production
```

## Service Architecture

### Core Services
1. **siglat-core-api**: Main .NET 8 Web API
2. **postgres**: PostgreSQL 15 database
3. **redis**: Redis cache for sessions and caching

### Security Features
- Non-root user in API container
- Health checks for all services
- Secure environment variable handling
- Network isolation

### Monitoring and Health Checks
- Built-in health checks for all services
- Automatic restart on failure
- Comprehensive logging

## Database Management

### Automatic Migrations
Database migrations run automatically on startup via the `DatabaseInitializer` service.

### Manual Database Operations
```bash
# Access PostgreSQL directly
docker-compose exec postgres psql -U siglat_user -d siglat

# View database logs
docker-compose logs postgres

# Backup database
docker-compose exec postgres pg_dump -U siglat_user siglat > backup.sql
```

## Troubleshooting

### Common Issues

1. **Port conflicts**:
   ```bash
   # Check what's using the ports
   netstat -tulpn | grep :5000
   
   # Change ports in .env.docker
   API_PORT=5001
   ```

2. **Database connection issues**:
   ```bash
   # Check database health
   ./docker-manage.sh health
   
   # Restart database
   docker-compose restart postgres
   ```

3. **API not responding**:
   ```bash
   # Check API logs
   docker-compose logs siglat-core-api
   
   # Rebuild and restart
   ./docker-manage.sh build
   ./docker-manage.sh restart
   ```

### Logs and Debugging

```bash
# View all logs
docker-compose logs

# View specific service logs
docker-compose logs siglat-core-api
docker-compose logs postgres

# Follow logs in real-time
docker-compose logs -f --tail=100

# Debug inside container
docker-compose exec siglat-core-api bash
```

## Production Deployment

### Security Considerations
1. Change default passwords and JWT secrets
2. Use proper SSL certificates
3. Configure firewall rules
4. Enable audit logging
5. Regular security updates

### Performance Optimization
1. Adjust memory limits in docker-compose.yml
2. Configure PostgreSQL for production workloads
3. Set up Redis clustering if needed
4. Use reverse proxy (nginx) for SSL termination

### Backup Strategy
```bash
# Automated backup script
#!/bin/bash
DATE=$(date +%Y%m%d_%H%M%S)
docker-compose exec postgres pg_dump -U siglat_user siglat > backup_$DATE.sql
```

## Frontend Integration

The docker-compose.yml includes placeholders for frontend services:
- Citizen UI (port 3000)
- BFP UI (port 3001)  
- PNP UI (port 3002)
- Admin UI (port 3003)

Uncomment and configure the relevant sections when frontend containers are available.

## Scaling

For high-availability deployment:
1. Use Docker Swarm or Kubernetes
2. Set up database replication
3. Configure load balancing
4. Implement distributed caching
5. Set up monitoring and alerting