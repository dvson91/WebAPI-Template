# GitHub Copilot Instructions for WebAPI Project

## Project Overview
You are working on a **Clean Architecture ASP.NET Core 9 Web API** project that follows Domain Driven Design (DDD) principles with CQRS implementation using MediatR. The project uses **GUID primary keys** and **automatic audit trail handling** via database interceptors.

## Key Architecture Patterns

### 🔑 GUID-Based Entity Design
All entities use GUID primary keys and implement interface contracts:
```csharp
public abstract class BaseEntity : IEntity, IAuditable, IEventEmitter, ISoftDelete
{
    public Guid Id { get; set; } = Guid.NewGuid();  // Auto-generated GUIDs
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}
```

### 🕐 Automatic Audit Handling
The `AuditInterceptor` automatically sets audit fields in UTC:
```csharp
// In WebAPI.Infrastructure/Interceptors/AuditInterceptor.cs
// Automatically handles CreatedAt, UpdatedAt, CreatedBy, UpdatedBy
// All timestamps are stored in UTC
// Triggered on EF Core SaveChanges operations
```

**Critical**: When creating entities, only set business properties - audit fields are handled automatically.

### 📊 CQRS with MediatR Patterns

**Commands for mutations** (use GUID parameters):
```csharp
public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    string Currency,
    int Stock,
    Guid CategoryId  // Always GUID for foreign keys
) : IRequest<Result<ProductDto>>;
```

**Queries for reads** (use GUID parameters):
```csharp
public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto?>>;
```

## Critical Development Workflows

### 🗃️ Database Migrations
**Always run from API project** (where DbContext is configured):
```bash
# From WebAPI.API directory:
dotnet ef migrations add MigrationName --project ..\WebAPI.Infrastructure
dotnet ef database update --project ..\WebAPI.Infrastructure
```

### 🚀 Running the Application
```bash
# From WebAPI.API directory:
dotnet run
# API: http://localhost:5050
# Swagger: http://localhost:5050/swagger
```

### 🏗️ Building the Solution
```bash
# From solution root:
dotnet build
# OR use the configured task from VS Code
```

## Layer-Specific Patterns

### Domain Layer (`WebAPI.Domain`)
- **Entities**: Inherit from `BaseEntity`, use GUID IDs
- **Interfaces**: `IEntity`, `IAuditable`, `IEventEmitter`, `ISoftDelete`
- **Value Objects**: Use for complex types like `Money`
- **No dependencies** on other layers

### Application Layer (`WebAPI.Application`) 
- **Commands/Queries**: Always use GUID for entity references
- **DTOs**: Mirror entity structure with GUID IDs
- **Validation**: Use FluentValidation with GUID validation
- **Result Pattern**: Consistent `Result<T>` responses

### Infrastructure Layer (`WebAPI.Infrastructure`)
- **AuditInterceptor**: Automatically handles audit fields with UTC
- **Repositories**: Generic `Repository<T>` with GUID-based queries
- **EF Configurations**: Fluent API setup for GUID primary keys
- **DI Registration**: Register interceptor with DbContext

### API Layer (`WebAPI.API`)
- **Minimal APIs**: Use `{id:guid}` route constraints
- **Endpoints**: Accept and return GUIDs in all operations
- **Options Pattern**: Configuration management via `ApiSettings`

## Project-Specific Conventions

### 🔧 Dependency Injection Setup
```csharp
// In Program.cs - layer registration order matters:
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
```

### 📝 Endpoint Patterns
```csharp
// Minimal API with GUID constraint
group.MapGet("/{id:guid}", async (IMediator mediator, Guid id) =>
{
    var result = await mediator.Send(new GetProductByIdQuery(id));
    return result.IsSuccess ? Results.Ok(result) : Results.NotFound(result);
});
```

### 🎯 Repository Patterns
```csharp
// Generic repository with GUID
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}
```

## Critical Integration Points

### 🔄 Domain Events Flow
1. Entity raises domain event via `AddDomainEvent()`
2. `DbTransactionBehavior` publishes events after successful transaction
3. Event handlers process business logic asynchronously

### 🛡️ Validation Pipeline
1. Commands validated via `FluentValidation` 
2. `ValidationBehavior` handles validation errors
3. Returns consistent `Result<T>` with error details

### 📦 Transaction Management
- Use `[RequiresTransaction]` attribute on commands
- Implement `ITransactionalCommand` for complex operations
- `DbTransactionBehavior` manages automatic transactions

## Quick Development Guide

### Adding a New Entity
1. **Domain**: Create entity inheriting `BaseEntity` with GUID ID
2. **Application**: Create DTOs, commands/queries with GUID parameters  
3. **Infrastructure**: Add EF configuration with GUID primary key
4. **API**: Create endpoints with `{id:guid}` constraints

### Testing the Audit Interceptor
Create any entity - `CreatedAt` and `CreatedBy` will be automatically set in UTC. Update operations automatically set `UpdatedAt` and `UpdatedBy`.

Remember: This codebase emphasizes **GUID-based distributed design** and **zero-touch audit handling** through database interceptors.

## Architecture Guidelines

### 1. Domain Layer (`WebAPI.Domain`)
- **Purpose**: Contains business entities, value objects, domain events, and core business rules
- **Key Principles**:
  - No dependencies on other layers
  - Rich domain models with encapsulated business logic
  - Use value objects for complex types (e.g., Money)
  - Implement domain events for cross-aggregate communication
  - Follow aggregate root patterns

**When working with entities:**
- Always use private constructors for Entity Framework
- Implement domain events in business methods
- Use descriptive method names that reflect business operations
- Encapsulate business rules within the entity
- **All entities use GUID IDs and inherit from BaseEntity**

**Example entity method:**
```csharp
public void UpdateStock(int newStock)
{
    var oldStock = Stock;
    Stock = newStock;
    // UpdatedAt handled automatically by AuditInterceptor
    
    AddDomainEvent(new ProductStockUpdatedEvent(this, oldStock, newStock));
}
```

### 2. Application Layer (`WebAPI.Application`)
- **Purpose**: Contains application logic, CQRS implementation, DTOs, and coordinated workflows
- **Key Principles**:
  - Use CQRS pattern with MediatR
  - Implement command and query handlers
  - Use FluentValidation for input validation
  - Return Result<T> pattern for consistent responses
  - Use DTOs for data transfer
  - **Always use GUID for entity references**

**Command Structure:**
```csharp
[RequiresTransaction] // For complex business operations
public class CreateProductCommand : IRequest<Result<ProductDto>>, ITransactionalCommand
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int Stock { get; set; }
    public Guid CategoryId { get; set; }  // GUID for foreign key
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    // Implementation with repository patterns
}
```

**Query Structure:**
```csharp
public class GetProductByIdQuery : IRequest<Result<ProductDto?>>
{
    public Guid Id { get; set; }  // GUID parameter
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto?>>
{
    // Use AsNoTracking() for read operations
}
```

### 3. Infrastructure Layer (`WebAPI.Infrastructure`)
- **Purpose**: Contains data access, repository implementations, and external service integrations
- **Key Principles**:
  - Implement repository interfaces from Domain layer
  - Use Entity Framework Core with Fluent API configurations
  - Implement Unit of Work pattern
  - Use performance optimizations (AsNoTracking, AsSplitQuery)
  - Handle database transactions
  - **Register AuditInterceptor for automatic audit handling**

**Repository Implementation:**
```csharp
public class ProductRepository : Repository<Product>, IProductRepository
{
    public async Task<Product?> GetWithCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
```

**Entity Configuration:**
```csharp
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // GUID primary key configuration
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
        
        // Use Fluent API for entity configuration
        builder.OwnsOne(p => p.Price, money => 
        {
            money.Property(m => m.Amount).HasColumnName("Price");
            money.Property(m => m.Currency).HasColumnName("Currency");
        });
        
        // Foreign key with GUID
        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId);
    }
}
```

### 4. API Layer (`WebAPI.API`)
- **Purpose**: Contains web API endpoints, middleware, and presentation logic
- **Key Principles**:
  - Use Minimal APIs for lightweight endpoints
  - Implement proper OpenAPI documentation
  - Use Options pattern for configuration
  - Handle cross-cutting concerns (CORS, validation, logging)
  - **Use GUID route constraints for all entity endpoints**

**Minimal API Endpoint:**
```csharp
group.MapPost("/", async (IMediator mediator, [FromBody] CreateProductDto request) =>
{
    var command = new CreateProductCommand 
    { 
        Name = request.Name,
        Description = request.Description,
        Price = request.Price,
        Currency = request.Currency,
        Stock = request.Stock,
        CategoryId = request.CategoryId  // GUID
    };
    var result = await mediator.Send(command);
    
    return result.IsSuccess 
        ? Results.Created($"/api/products/{result.Data?.Id}", result)
        : Results.BadRequest(result);
})
.WithName("CreateProduct")
.WithSummary("Create a new product")
.Produces<Result<ProductDto>>(201)
.Produces<Result<ProductDto>>(400);
```

## Coding Standards and Best Practices

### SOLID Principles
1. **Single Responsibility**: Each class has one reason to change
2. **Open/Closed**: Open for extension, closed for modification
3. **Liskov Substitution**: Derived classes must be substitutable for base classes
4. **Interface Segregation**: Clients shouldn't depend on interfaces they don't use
5. **Dependency Inversion**: Depend on abstractions, not concretions

### Performance Guidelines
- Use `AsNoTracking()` for read-only queries
- Implement `AsSplitQuery()` for complex joins
- Leverage `IQueryable` for deferred execution
- Use `CancellationToken` in all async methods
- Implement query filters for global conditions (soft deletes)

### Error Handling
- Use Result<T> pattern for consistent error handling
- Centralize error messages in MessageConstants
- Implement validation using FluentValidation
- Use domain exceptions for business rule violations

### Testing Guidelines
- Write unit tests for domain logic
- Use integration tests for API endpoints
- Mock external dependencies
- Test both success and failure scenarios

## Common Patterns and Examples

### Adding a New Feature (e.g., Category management)

1. **Domain Layer**:
   ```csharp
   // Add Category entity inheriting BaseEntity with GUID ID
   // Add category-related domain events
   // Add ICategoryRepository interface
   ```

2. **Application Layer**:
   ```csharp
   // Add CategoryDto classes with GUID properties
   // Add CreateCategoryCommand and handler with GUID parameters
   // Add GetCategoryByIdQuery and handler with GUID parameters
   // Add validation rules with GUID validation
   ```

3. **Infrastructure Layer**:
   ```csharp
   // Add CategoryConfiguration for EF Core with GUID setup
   // Implement CategoryRepository with GUID-based queries
   // Register in DependencyInjection
   ```

4. **API Layer**:
   ```csharp
   // Add CategoryEndpoints with {id:guid} constraints
   // Map endpoints in Program.cs
   ```

### Message Constants Usage
Always use MessageConstants for user-facing messages:
```csharp
return Result<ProductDto>.Success(productDto, MessageConstants.ProductCreated);
return Result<ProductDto>.Failure(MessageConstants.ProductNotFound);
```

### Configuration Management
Use Options pattern for all configuration:
```csharp
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection(ApiSettings.SectionName));
```