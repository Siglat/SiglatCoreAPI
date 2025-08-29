# SIGLAT Core API Documentation

## Overview
This API implementation provides comprehensive emergency response operations for the SIGLAT system, supporting multiple agencies including BFP (Bureau of Fire Protection) and PNP (Philippine National Police). The API includes role-based access control for different emergency response agencies.

## Base URL
```
http://localhost:5000/api/v1/public
```

## Supported Roles and Access Levels

### Public Access
- **Citizens**: Report incidents, view public information
- **Anonymous**: Health checks, basic system information

### Emergency Response Agencies
- **BFP (Bureau of Fire Protection)**: Fire incident management, unit dispatch, equipment tracking
- **PNP (Philippine National Police)**: Crime investigation, patrol management, warrant tracking
- **Ambulance Services**: Medical emergency response, patient transport
- **Admin**: System administration, cross-agency coordination

### Role-Based Endpoints

#### BFP (Fire Protection) - `/api/v1/bfp/`
- Fire incident management and response
- Fire unit tracking and dispatch
- Equipment and resource management
- Fire statistics and reporting

#### PNP (Police) - `/api/v1/pnp/`
- Police incident reporting and investigation
- Patrol unit management
- Warrant and evidence tracking
- Crime statistics and case management

## Pagination
All list endpoints support pagination with the following query parameters:

- `page` (int): Page number (default: 1, min: 1)
- `pageSize` (int): Items per page (default: 10, min: 1, max: 100)
- `search` (string): Search term for filtering results
- `sortBy` (string): Field to sort by
- `sortOrder` (string): Sort direction ("asc" or "desc", default: "asc")

### Pagination Response Format
```json
{
  "data": [...],
  "page": 1,
  "pageSize": 10,
  "totalCount": 25,
  "totalPages": 3,
  "hasNext": true,
  "hasPrevious": false,
  "nextPageUrl": "http://localhost:5000/api/v1/public/reports?page=2&pageSize=10",
  "previousPageUrl": null
}
```

## Endpoints

### Health Check
- **GET** `/health`
  - Returns basic API health status
- **GET** `/health/detailed`
  - Returns detailed system health information

### Authentication
- **GET** `/auth/health`
  - Returns auth service health status
- **POST** `/auth/register`
  - Register a new user
  - Body: `{ "firstName", "lastName", "email", "password", "phoneNumber?", "department?", "location?" }`
- **POST** `/auth/login`
  - Login with email and password
  - Body: `{ "email", "password" }`
  - Test credentials: `test@example.com` / `password123`
- **POST** `/auth/refresh`
  - Refresh authentication token
- **POST** `/auth/logout`
  - Logout (requires Authorization header)
- **POST** `/auth/forgot-password`
  - Request password reset
- **POST** `/auth/reset-password`
  - Reset password with token

### Reports
- **GET** `/reports`
  - Get paginated list of incident reports
  - Query params: `page`, `pageSize`, `search`, `sortBy` (timestamp, incidenttype), `sortOrder`
- **GET** `/reports/{id}`
  - Get specific report by ID
- **POST** `/reports`
  - Create new incident report
- **PUT** `/reports/{id}`
  - Update existing report
- **DELETE** `/reports/{id}`
  - Delete report

### Users
- **GET** `/users`
  - Get paginated list of users
  - Query params: `page`, `pageSize`, `search`, `sortBy` (firstname, lastname, email, department), `sortOrder`
- **GET** `/users/{email}`
  - Get specific user by email
- **POST** `/users`
  - Create new user
- **PUT** `/users/{email}`
  - Update existing user
- **DELETE** `/users/{email}`
  - Delete user

### BFP (Bureau of Fire Protection) Endpoints
- **GET** `/bfp/fireoperations/health`
  - BFP service health status
- **GET** `/bfp/fireoperations/incidents`
  - Get fire incidents with pagination and search
  - Query params: `page`, `pageSize`, `search`
- **POST** `/bfp/fireoperations/incidents/{incidentId}/respond`
  - Log BFP response to fire incident
- **GET** `/bfp/fireoperations/units`
  - Get fire units and their status
- **POST** `/bfp/fireoperations/units/{unitId}/dispatch`
  - Dispatch fire unit to incident
- **GET** `/bfp/fireoperations/equipment`
  - Get fire equipment status and availability
- **GET** `/bfp/fireoperations/statistics`
  - Get fire department statistics and metrics
- **POST** `/bfp/fireoperations/alerts`
  - Create fire emergency alert

### PNP (Philippine National Police) Endpoints
- **GET** `/pnp/policeoperations/health`
  - PNP service health status
- **GET** `/pnp/policeoperations/incidents`
  - Get police incidents with pagination and search
  - Query params: `page`, `pageSize`, `search`
- **POST** `/pnp/policeoperations/incidents/{incidentId}/respond`
  - Log PNP response to incident
- **GET** `/pnp/policeoperations/units`
  - Get police units and patrol status
- **POST** `/pnp/policeoperations/units/{unitId}/dispatch`
  - Dispatch police unit to incident
- **GET** `/pnp/policeoperations/patrols`
  - Get active patrol information
- **GET** `/pnp/policeoperations/statistics`
  - Get police department statistics and metrics
- **POST** `/pnp/policeoperations/alerts`
  - Create police emergency alert
- **GET** `/pnp/policeoperations/warrants`
  - Get active warrants with pagination
- **POST** `/pnp/policeoperations/incidents/{incidentId}/evidence`
  - Submit evidence for incident

## Example Requests

### BFP Fire Operations
```bash
# Get fire incidents
curl -H "Authorization: Bearer {bfp_token}" \
  "http://localhost:5000/api/v1/bfp/fireoperations/incidents?search=structure"

# Dispatch fire unit
curl -X POST -H "Authorization: Bearer {bfp_token}" \
  -H "Content-Type: application/json" \
  "http://localhost:5000/api/v1/bfp/fireoperations/units/1/dispatch" \
  -d '{"incidentId": "123e4567-e89b-12d3-a456-426614174000", "priority": "high"}'
```

### PNP Police Operations
```bash
# Get police incidents
curl -H "Authorization: Bearer {pnp_token}" \
  "http://localhost:5000/api/v1/pnp/policeoperations/incidents?search=robbery"

# Get active warrants
curl -H "Authorization: Bearer {pnp_token}" \
  "http://localhost:5000/api/v1/pnp/policeoperations/warrants?page=1&pageSize=10"
```

### Get Reports with Pagination
```bash
curl "http://localhost:5000/api/v1/public/reports?page=1&pageSize=5&search=fire&sortBy=timestamp&sortOrder=desc"
```

### Create Report
```bash
curl -X POST "http://localhost:5000/api/v1/public/reports" \
  -H "Content-Type: application/json" \
  -d '{
    "incidentType": "Fire Emergency",
    "description": "Kitchen fire at 123 Main St",
    "whoReportedId": "123e4567-e89b-12d3-a456-426614174000",
    "involvedAgencies": "Fire Department",
    "notes": "No injuries reported"
  }'
```

### Login
```bash
curl -X POST "http://localhost:5000/api/v1/public/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "password123"
  }'
```

## Features Implemented

1. **Role-Based Access Control**: Separate endpoints for BFP, PNP, Ambulance, Admin, and Citizen roles
2. **Emergency Agency Operations**: Specialized controllers for fire protection and police operations
3. **Complete CRUD Operations**: All endpoints support Create, Read, Update, Delete operations
4. **Advanced Pagination**: Includes page navigation, search, sorting, and metadata
5. **Input Validation**: Model validation with proper error responses
6. **Mock Data**: Realistic sample data for testing different emergency scenarios
7. **Consistent API Design**: RESTful endpoints with standard HTTP methods
8. **Error Handling**: Proper HTTP status codes and error messages
9. **Health Monitoring**: Basic and detailed health check endpoints
10. **Authentication Flow**: JWT-based authentication with role-based authorization
11. **Multi-Agency Coordination**: Cross-agency access for Admin and emergency response

## Mock Data
The API includes realistic mock data for:
- **Fire Incidents**: Structure fires, vehicle fires, forest fires with BFP response data
- **Police Incidents**: Traffic accidents, robberies, drug operations with PNP case data
- **Emergency Units**: Fire trucks, police patrols, ambulances with real-time status
- **Personnel**: Officers, firefighters, paramedics with role assignments
- **Equipment**: Fire equipment, police vehicles, medical supplies with availability status

## Agency-Specific Features

### BFP (Bureau of Fire Protection)
- Fire incident classification and tracking
- Fire unit dispatch and resource allocation
- Equipment inventory and maintenance tracking
- Fire damage assessment and statistics
- Multi-alarm fire coordination

### PNP (Philippine National Police)
- Crime incident reporting and investigation
- Patrol unit management and dispatch
- Warrant tracking and enforcement
- Evidence collection and chain of custody
- Criminal case management
- Traffic enforcement and accident response

## Testing
You can test the API using:
- Swagger UI (when running): `http://localhost:5000/swagger`
- curl commands (examples above)
- Postman or similar API testing tools

## Notes
- Role-based endpoints require proper JWT authentication with assigned roles
- BFP endpoints are restricted to BFP personnel and authorized users
- PNP endpoints are restricted to PNP personnel and authorized users  
- Admin endpoints allow cross-agency access for system coordination
- Public endpoints remain accessible for citizen reporting and information
- JWT tokens are currently mock implementations for development
- Database operations are simulated with in-memory data for testing
- In production, implement proper authentication, database persistence, and security measures

## Authorization Roles
- **BFP**: Bureau of Fire Protection personnel
- **PNP**: Philippine National Police personnel  
- **Ambulance**: Emergency medical services personnel
- **Admin**: System administrators with cross-agency access
- **Citizen**: General public users for reporting incidents
- **Anonymous**: Unauthenticated users with limited access