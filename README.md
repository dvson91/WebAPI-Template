# WebAPI - Clean Architecture Template

A comprehensive ASP.NET Core 9 Web API project template built following Clean Architecture principles with Domain Driven Design (DDD), CQRS using MediatR, and Entity Framework Core.

## 🏗️ Architecture Overview

This project implements Clean Architecture with the following layers:

- **Domain Layer** (`WebAPI.Domain`): Contains business entities, value objects, domain events, and interfaces
- **Application Layer** (`WebAPI.Application`): Contains business logic, CQRS commands/queries, DTOs, and application services
- **Infrastructure Layer** (`WebAPI.Infrastructure`): Contains data access, repositories, and external service implementations
- **API Layer** (`WebAPI.API`): Contains controllers, minimal APIs, and presentation logic

## 🛠️ Technology Stack

- **.NET 9**: Latest .NET framework
- **ASP.NET Core**: Web API framework
- **Entity Framework Core 9**: ORM for data access
- **SQL Server**: Database provider
- **MediatR**: CQRS and mediator pattern implementation
- **FluentValidation**: Input validation
- **Swashbuckle**: OpenAPI/Swagger documentation
- **Minimal APIs**: Lightweight API endpoints

## 🎯 Key Features

### Architecture Patterns
- **Clean Architecture**: Separation of concerns with dependency inversion
- **Domain Driven Design (DDD)**: Rich domain models with business logic
- **CQRS**: Command Query Responsibility Segregation using MediatR
- **Repository Pattern**: Data access abstraction
- **Unit of Work Pattern**: Transaction management
- **Options Pattern**: Configuration management

### Code Quality
- **SOLID Principles**: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion
- **DRY (Don't Repeat Yourself)**: Eliminates code duplication
- **KISS (Keep It Simple, Stupid)**: Simple and maintainable code

### Performance Optimizations
- **AsNoTracking()**: Optimized read operations
- **AsSplitQuery()**: Efficient multi-table queries
- **IQueryable**: Deferred execution
- **Query Filters**: Global filters for soft deletes

### Database Features
- **Code First**: Entity Framework migrations
- **FluentAPI**: Entity configuration
- **Value Objects**: Complex types (Money)
- **Domain Events**: Event-driven architecture
- **Soft Delete**: Data preservation
- **Audit Fields**: Created/Updated tracking

## 📁 Project Structure

```
WebAPI/
├── WebAPI.Domain/
│   ├── Common/
│   │   ├── BaseEntity.cs
│   │   ├── ValueObject.cs
│   │   └── IDomainEvent.cs
│   ├── Entities/
│   │   ├── Product.cs
│   │   └── Category.cs
│   ├── ValueObjects/
│   │   └── Money.cs
│   ├── Events/
│   │   ├── ProductEvents.cs
│   │   └── CategoryEvents.cs
│   └── Interfaces/
│       ├── IRepository.cs
│       └── IUnitOfWork.cs
├── WebAPI.Application/
│   ├── Commands/
│   │   ├── CreateProductCommand.cs
│   │   └── UpdateProductCommand.cs
│   ├── Queries/
│   │   └── ProductQueries.cs
│   ├── DTOs/
│   │   ├── ProductDtos.cs
│   │   └── CategoryDtos.cs
│   ├── Behaviors/
│   │   └── ValidationBehavior.cs
│   ├── Contracts/
│   │   └── DbTransactionBehavior.cs
│   ├── Common/
│   │   ├── Result.cs
│   │   └── MessageConstants.cs
│   └── DependencyInjection.cs
├── WebAPI.Infrastructure/
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── Configurations/
│   │   ├── ProductConfiguration.cs
│   │   └── CategoryConfiguration.cs
│   ├── Repositories/
│   │   ├── Repository.cs
│   │   ├── ProductRepository.cs
│   │   └── CategoryRepository.cs
│   └── DependencyInjection.cs
└── WebAPI.API/
    ├── Extensions/
    │   ├── ProductEndpoints.cs
    │   └── CategoryEndpoints.cs
    ├── Configuration/
    │   └── AppSettings.cs
    └── Program.cs
```

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) or SQL Server
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd WebAPI
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Update connection string**
   
   Update the connection string in `appsettings.json` and `appsettings.Development.json` to match your SQL Server instance.

4. **Create and apply migrations**
   ```bash
   dotnet ef migrations add InitialCreate --project WebAPI.Infrastructure --startup-project WebAPI.API
   dotnet ef database update --project WebAPI.Infrastructure --startup-project WebAPI.API
   ```

5. **Build the solution**
   ```bash
   dotnet build
   ```

6. **Run the application**
   ```bash
   dotnet run --project WebAPI.API
   ```

7. **Access Swagger UI**
   
   Navigate to `https://localhost:5001` or `http://localhost:5000` to access the Swagger documentation.

## 📊 API Endpoints

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product

### Categories
- `GET /api/categories` - Get all categories
- `GET /api/categories/{id}` - Get category by ID
- `POST /api/categories` - Create new category
- `PUT /api/categories/{id}` - Update category

## 🔧 Configuration

The application uses the Options pattern for configuration management. Settings are defined in:

- `appsettings.json` - Production settings
- `appsettings.Development.json` - Development settings

### Available Settings

```json
{
  "ApiSettings": {
    "Title": "WebAPI",
    "Version": "v1.0.0",
    "Description": "A Clean Architecture ASP.NET Core Web API",
    "EnableSwagger": true,
    "EnableCors": true
  },
  "DatabaseSettings": {
    "CommandTimeout": 30,
    "EnableSensitiveDataLogging": false,
    "EnableDetailedErrors": false,
    "MaxRetryCount": 3
  }
}
```

## 🧪 Testing

The project structure supports easy testing with separate concerns:

- **Unit Tests**: Test domain logic and application services
- **Integration Tests**: Test API endpoints and database interactions
- **Repository Tests**: Test data access layer

## 📈 Performance Considerations

- Use `AsNoTracking()` for read-only queries
- Implement `AsSplitQuery()` for complex joins
- Leverage `IQueryable` for deferred execution
- Use query filters for global conditions
- Implement caching strategies as needed

## 🔒 Security Considerations

- Implement authentication and authorization
- Use HTTPS in production
- Validate all inputs
- Implement rate limiting
- Use parameterized queries (handled by EF Core)

## 🚢 Deployment

1. **Publish the application**
   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. **Update production settings**
   - Connection strings
   - CORS policies
   - Logging levels

3. **Deploy to your preferred hosting platform**
   - Azure App Service
   - AWS Elastic Beanstalk
   - Docker containers
   - IIS

## 🤝 Contributing

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Team

- Development Team - dev@company.com

## 📞 Support

For support and questions, please contact the development team or create an issue in the repository.