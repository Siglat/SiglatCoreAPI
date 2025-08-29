#!/bin/bash

# SIGLAT Core API Docker Management Script

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
PROJECT_NAME="siglat-core-api"
COMPOSE_FILE="docker-compose.yml"
ENV_FILE=".env.docker"

# Functions
print_usage() {
    echo -e "${BLUE}SIGLAT Core API Docker Management${NC}"
    echo "Usage: $0 [COMMAND]"
    echo ""
    echo "Commands:"
    echo "  build     - Build the Docker image"
    echo "  up        - Start all services"
    echo "  down      - Stop all services"
    echo "  restart   - Restart all services"
    echo "  logs      - Show logs"
    echo "  status    - Show service status"
    echo "  clean     - Clean up containers and images"
    echo "  db-only   - Start only database"
    echo "  api-only  - Start only API (requires database running)"
    echo "  health    - Check service health"
    echo "  migrate   - Run database migrations"
    echo ""
}

check_requirements() {
    if ! command -v docker &> /dev/null; then
        echo -e "${RED}Error: Docker is not installed${NC}"
        exit 1
    fi
    
    echo -e "${YELLOW}Note: Docker Compose not available in this environment${NC}"
    echo -e "${YELLOW}You can still build the Docker image directly:${NC}"
    echo -e "${BLUE}docker build -t siglat-core-api .${NC}"
}

load_env() {
    if [[ -f "$ENV_FILE" ]]; then
        echo -e "${YELLOW}Loading environment from $ENV_FILE${NC}"
        export $(cat $ENV_FILE | grep -v '^#' | xargs)
    else
        echo -e "${YELLOW}Warning: $ENV_FILE not found, using default values${NC}"
    fi
}

build_image() {
    echo -e "${BLUE}Building $PROJECT_NAME Docker image...${NC}"
    docker-compose build --no-cache siglat-core-api
    echo -e "${GREEN}Build completed!${NC}"
}

start_services() {
    echo -e "${BLUE}Starting SIGLAT Core API services...${NC}"
    docker-compose up -d
    echo -e "${GREEN}Services started!${NC}"
    echo -e "${YELLOW}API will be available at: http://localhost:${API_PORT:-5000}${NC}"
    echo -e "${YELLOW}Swagger UI: http://localhost:${API_PORT:-5000}/swagger${NC}"
}

stop_services() {
    echo -e "${BLUE}Stopping SIGLAT Core API services...${NC}"
    docker-compose down
    echo -e "${GREEN}Services stopped!${NC}"
}

restart_services() {
    echo -e "${BLUE}Restarting SIGLAT Core API services...${NC}"
    docker-compose restart
    echo -e "${GREEN}Services restarted!${NC}"
}

show_logs() {
    echo -e "${BLUE}Showing logs...${NC}"
    docker-compose logs -f --tail=100
}

show_status() {
    echo -e "${BLUE}Service Status:${NC}"
    docker-compose ps
}

cleanup() {
    echo -e "${BLUE}Cleaning up containers and images...${NC}"
    docker-compose down -v --remove-orphans
    docker system prune -f
    echo -e "${GREEN}Cleanup completed!${NC}"
}

start_db_only() {
    echo -e "${BLUE}Starting database only...${NC}"
    docker-compose up -d postgres redis
    echo -e "${GREEN}Database services started!${NC}"
}

start_api_only() {
    echo -e "${BLUE}Starting API only...${NC}"
    docker-compose up -d siglat-core-api
    echo -e "${GREEN}API service started!${NC}"
}

check_health() {
    echo -e "${BLUE}Checking service health...${NC}"
    
    # Check if containers are running
    if docker-compose ps | grep -q "Up"; then
        echo -e "${GREEN}✓ Containers are running${NC}"
    else
        echo -e "${RED}✗ Some containers are not running${NC}"
        docker-compose ps
        return 1
    fi
    
    # Check API health endpoint
    API_PORT=${API_PORT:-5000}
    if curl -f -s "http://localhost:$API_PORT/api/v1/public/health" > /dev/null; then
        echo -e "${GREEN}✓ API health check passed${NC}"
    else
        echo -e "${RED}✗ API health check failed${NC}"
        return 1
    fi
    
    # Check database connectivity
    if docker-compose exec -T postgres pg_isready -U siglat_user -d siglat > /dev/null; then
        echo -e "${GREEN}✓ Database is accessible${NC}"
    else
        echo -e "${RED}✗ Database is not accessible${NC}"
        return 1
    fi
    
    echo -e "${GREEN}All health checks passed!${NC}"
}

run_migrations() {
    echo -e "${BLUE}Running database migrations...${NC}"
    
    # Check if API container is running
    if ! docker-compose ps siglat-core-api | grep -q "Up"; then
        echo -e "${YELLOW}Starting API container for migration...${NC}"
        docker-compose up -d siglat-core-api
        sleep 10
    fi
    
    # Migrations are handled automatically by DatabaseInitializer
    echo -e "${GREEN}Migrations completed automatically on startup!${NC}"
}

# Main script logic
check_requirements
load_env

case "${1:-}" in
    build)
        build_image
        ;;
    up)
        start_services
        ;;
    down)
        stop_services
        ;;
    restart)
        restart_services
        ;;
    logs)
        show_logs
        ;;
    status)
        show_status
        ;;
    clean)
        cleanup
        ;;
    db-only)
        start_db_only
        ;;
    api-only)
        start_api_only
        ;;
    health)
        check_health
        ;;
    migrate)
        run_migrations
        ;;
    "")
        print_usage
        ;;
    *)
        echo -e "${RED}Error: Unknown command '$1'${NC}"
        print_usage
        exit 1
        ;;
esac