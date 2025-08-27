# SIGLAT Core API

**Sistema Integrated Geographic Location Alert and Tracking - Core API**

A unified, role-based API designed to support multiple specialized frontend applications for comprehensive emergency response management.

## 🏗️ Architecture

This is a **single API, multi-frontend architecture** where:
- **One unified API** serves all frontend applications
- **Role-based access control** ensures proper authorization
- **Specialized frontends** provide optimized user experiences per role
- **Consistent data layer** maintains data integrity across all clients

```
┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐
│   Citizen App   │  │  Ambulance App  │  │   Admin App     │
│   (Public UI)   │  │  (Responder UI) │  │ (Management UI) │
└─────────┬───────┘  └─────────┬───────┘  └─────────┬───────┘
          │                    │                    │
          └────────────────────┼────────────────────┘
                               │
                    ┌─────────────────┐
                    │ SIGLAT Core API │
                    │ (This Project)  │
                    └─────────┬───────┘
                              │
                    ┌─────────────────┐
                    │   PostgreSQL    │
                    └─────────────────┘
```

## 🔐 Role-Based API Organization

### **Public Endpoints** (`/api/v1/public/`)
- User registration and authentication
- System status and health checks
- Emergency contact information
- No authentication required

### **Citizen Endpoints** (`/api/v1/citizen/`)
**Role Required**: `User`
- Emergency report submission
- Personal profile management
- Location sharing
- Report status tracking

### **Ambulance Endpoints** (`/api/v1/ambulance/`)
**Role Required**: `Ambulance`
- Emergency alert dashboard
- Response status updates
- Patient location tracking
- Navigation assistance

### **Admin Endpoints** (`/api/v1/admin/`)
**Role Required**: `Admin`
- User management and verification
- System analytics and monitoring
- Emergency coordination
- Multi-agency communication

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: PostgreSQL with Entity Framework Core 8.0.11
- **Authentication**: JWT Bearer tokens with role-based claims
- **ORM**: Entity Framework Core + Dapper (dual strategy)
- **Documentation**: Swagger/OpenAPI with role-based grouping
- **Containerization**: Docker with multi-stage builds

## 📁 Project Structure

```
SiglatCoreAPI/
├── Controllers/
│   ├── Public/             # No auth required
│   │   └── AuthController.cs
│   ├── Citizen/            # User role endpoints
│   │   ├── UserController.cs
│   │   ├── ChatController.cs
│   │   └── ReportController.cs
│   ├── Ambulance/          # Ambulance role endpoints
│   │   └── AmbulanceController.cs
│   └── Admin/              # Admin role endpoints
│       ├── AdminController.cs
│       ├── BFPController.cs
│       ├── PNPController.cs
│       ├── SiglatController.cs
│       ├── FloodController.cs
│       └── TyphoonController.cs
├── Models/                 # Shared DTOs
├── Services/               # Business logic
├── Data/                   # Database context
├── Middleware/             # Custom middleware
├── Migrations/             # EF Core migrations
└── Program.cs             # Application entry point
```

## 🚀 Quick Start

### Prerequisites

- .NET 8.0 SDK
- PostgreSQL 13+
- Docker (optional)

### Development Setup

```bash
# Clone the repository
git clone git@github.com:Siglat/SiglatCoreAPI.git
cd SiglatCoreAPI

# Restore dependencies
dotnet restore

# Set up environment variables
cp .env.example .env
# Edit .env with your configuration

# Run database migrations
dotnet ef database update

# Start development server
dotnet run

# API will be available at https://localhost:7045
# Swagger UI at https://localhost:7045/swagger
```

### Environment Configuration

Create `.env` file:

```env
# Database Configuration
DB_HOST=localhost
DB_PORT=5432
DB_USER=siglat_user
DB_PASS=your_password
DB_DB=siglat

# JWT Configuration
JWT_SECRET=your-super-secret-jwt-key-at-least-32-characters
JWT_ISSUER=siglat-core-api
JWT_AUDIENCE=siglat-clients

# CORS Origins (comma-separated)
ALLOWED_ORIGINS=http://localhost:3000,http://localhost:3001,http://localhost:3002
```

## 📊 Database Schema

### Core Entities

- **Identity**: User management with roles (Admin, User, Ambulance)
- **Verifications**: Identity verification workflow
- **Reports**: Emergency incident reporting
- **Alerts**: Real-time emergency alerts
- **Chat**: Inter-user communication
- **Coordinates**: Location tracking
- **Contact**: Emergency contact directory
- **LoginLogs**: Authentication audit trail

### Entity Relationships

```
Identity (Users) ←→ Reports (One-to-Many)
Identity (Users) ←→ Alerts (One-to-Many)
Identity (Users) ←→ Chat (One-to-Many)
Identity (Users) ←→ Coordinates (One-to-Many)
Identity (Users) ←→ Verifications (One-to-One)
```

## 🔒 Security Features

### Authentication & Authorization
- **JWT Bearer Tokens**: Secure stateless authentication
- **Role-based Claims**: Granular access control per endpoint
- **Token Expiration**: Configurable token lifetime
- **Password Security**: BCrypt hashing with salt

### API Security
- **CORS Configuration**: Controlled cross-origin access
- **Input Validation**: Data annotation validation
- **SQL Injection Protection**: Parameterized queries via EF Core
- **Error Handling**: Secure error responses without sensitive data

## 📱 Frontend Integration

This API is designed to support multiple specialized frontend applications:

### **Citizen App** (Port 3000)
- **Target**: General public
- **Features**: Emergency reporting, status checking
- **API Access**: Public + Citizen endpoints

### **Ambulance App** (Port 3001)
- **Target**: Medical responders
- **Features**: Alert dashboard, navigation, response tracking
- **API Access**: Public + Ambulance endpoints

### **Admin App** (Port 3002)
- **Target**: System administrators
- **Features**: User management, analytics, system control
- **API Access**: Public + Admin endpoints (full access)

## 🐳 Docker Deployment

### Single Container

```bash
# Build image
docker build -t siglat-core-api .

# Run container
docker run -d \
  --name siglat-core-api \
  -p 5000:8080 \
  -e DB_HOST=your-db-host \
  -e JWT_SECRET=your-secret-key \
  siglat-core-api
```

### Docker Compose (Recommended)

```yaml
version: '3.8'

services:
  postgres:
    image: postgres:15
    environment:
      POSTGRES_DB: siglat
      POSTGRES_USER: siglat_user
      POSTGRES_PASSWORD: ${DB_PASS}
    volumes:
      - postgres_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"

  siglat-core-api:
    build: .
    depends_on:
      - postgres
    environment:
      - DB_HOST=postgres
      - DB_PORT=5432
      - DB_USER=siglat_user
      - DB_PASS=${DB_PASS}
      - DB_DB=siglat
      - JWT_SECRET=${JWT_SECRET}
    ports:
      - "5000:8080"

volumes:
  postgres_data:
```

## 📊 API Documentation

### Swagger/OpenAPI
- **Development**: https://localhost:7045/swagger
- **Production**: https://your-domain/swagger
- **OpenAPI Spec**: https://your-domain/swagger/v1/swagger.json

### Endpoint Examples

```bash
# Public: User Registration
POST /api/v1/public/auth/register
Content-Type: application/json
{
  "email": "user@example.com",
  "password": "securepassword",
  "firstName": "John",
  "lastName": "Doe"
}

# Public: User Login
POST /api/v1/public/auth/login
Content-Type: application/json
{
  "email": "user@example.com",
  "password": "securepassword"
}

# Citizen: Submit Emergency Report
POST /api/v1/citizen/report
Authorization: Bearer <jwt-token>
Content-Type: application/json
{
  "type": "Medical",
  "description": "Medical emergency at location",
  "latitude": 14.5995,
  "longitude": 120.9842
}

# Ambulance: Get Current Alerts
GET /api/v1/ambulance/ambulance/all-alert
Authorization: Bearer <jwt-token>

# Admin: Get All Users
GET /api/v1/admin/admin/userlist
Authorization: Bearer <jwt-token>
```

## 🔧 Development

### Adding New Endpoints

1. **Determine Role**: Decide which role(s) should access the endpoint
2. **Choose Controller**: Add to appropriate role-based controller
3. **Set Authorization**: Use `[Authorize(Roles = "RoleName")]`
4. **Update Documentation**: Add XML documentation comments

### Role-based Controller Example

```csharp
namespace SiglatCoreAPI.Controllers.Citizen
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "User")]
    [Route("api/v{version:apiVersion}/citizen/[controller]")]
    public class ExampleController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            // Only users with "User" role can access this
            return Ok("Citizen data");
        }
    }
}
```

## 🚀 Migration from Previous Version

This project replaces the monolithic SIGLAT-API with a role-based architecture:

### What's New
- ✅ **Role-based routing**: Organized by user role
- ✅ **Enhanced security**: Granular access control
- ✅ **Better organization**: Clear separation of concerns
- ✅ **Multi-frontend ready**: Designed for specialized UIs

### Migration Path
1. **Controllers**: Moved from flat structure to role-based folders
2. **Routes**: Updated to include role prefix (e.g., `/citizen/`, `/admin/`)
3. **Authorization**: Enhanced with role-specific access control
4. **Documentation**: Improved with role-based grouping

## 📈 Monitoring & Health Checks

### Health Check Endpoint
```bash
GET /health
```

### Application Metrics
```bash
GET /metrics
```

### Database Health
```bash
GET /api/v1/public/status
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Follow role-based organization principles
4. Add proper authorization attributes
5. Update documentation
6. Submit a pull request

## 📄 License

This project is licensed under the ISC License.

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/Siglat/SiglatCoreAPI/issues)
- **Documentation**: [API Documentation](https://your-domain/swagger)
- **Email**: support@siglat.org

---

**SIGLAT Core API** - Powering the next generation of emergency response systems with role-based architecture and multi-frontend support.