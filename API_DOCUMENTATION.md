# SIGLAT Minimal API Documentation

## Overview
This minimal API implementation provides basic CRUD operations with pagination for the SIGLAT emergency response system. All endpoints are publicly accessible for demonstration purposes.

## Base URL
```
http://localhost:5000/api/v1/public
```

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

## Example Requests

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

1. **Complete CRUD Operations**: All endpoints support Create, Read, Update, Delete operations
2. **Advanced Pagination**: Includes page navigation, search, sorting, and metadata
3. **Input Validation**: Model validation with proper error responses
4. **Mock Data**: Realistic sample data for testing
5. **Consistent API Design**: RESTful endpoints with standard HTTP methods
6. **Error Handling**: Proper HTTP status codes and error messages
7. **Health Monitoring**: Basic and detailed health check endpoints
8. **Authentication Flow**: Complete auth endpoints (mock implementation)

## Mock Data
The API includes realistic mock data for:
- 3 sample incident reports (fire, medical, traffic)
- 5 sample users from different departments
- Proper timestamps and relationships

## Testing
You can test the API using:
- Swagger UI (when running): `http://localhost:5000/swagger`
- curl commands (examples above)
- Postman or similar API testing tools

## Notes
- All endpoints are currently set to `[AllowAnonymous]` for testing
- JWT tokens are mock implementations
- Database operations are simulated with in-memory data
- In production, implement proper authentication, database persistence, and security measures