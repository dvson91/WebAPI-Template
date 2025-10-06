---
mode: agent
model: Claude Sonnet 4 (copilot)
---
Build a web application project template with the following specifications:

Project Name: WebAPI  
Technology Stack: ASP.NET Core API (.NET 9) integrated with Microsoft SQL Server

Technical Specifications and Best Practices

Application Architecture  
- Implement Clean Architecture following a Domain Driven Design approach (traditional) with CQRS using MediatR.  
- Utilize design patterns including Unit of Work, Repository, Services, and Dependency Injection.  
- Define a DependencyInjection class in both the Application and Infrastructure layers to register services in the Program class.

Coding Standards  
- Ensure adherence to best practices such as DRY (Don't Repeat Yourself), KISS (Keep It Simple, Stupid), and SOLID principles for simplicity, extensibility, maintainability, and flexibility.  
- Implement the Options pattern for configuration management settings.  
- Centralize message constants at a designated MessageConstants class.

Data Access and Performance  
- Use Entity Framework Core with a Code First approach for data access, employing LINQ for querying and configuring Entities with FluentAPI.  
- Apply performance optimization techniques including .AsNoTracking(), .AsSplitQuery(), IQueryable, and IEnumerable for efficient data processing.
- Using IEntity, IAuditable, IEventEmitter base class - Id use Guid (Globally Unique Identifier).
- CreatedAt, UpdatedAt, CreatedBy, UpdatedBy should be auto handle via Database Interceptors/Event Hooks - Persistence/Infrastructure.

CQRS Command Classes  
- Centralize transaction management in a DbTransactionBehavior class within the Contracts folder to ensure that any command class containing complex business logic related to the database is handled within a transaction, allowing for rollback in case of exceptions.

API Design  
- Implement OpenAPI specifications utilizing the Minimal API pattern.

Additional Notes  
- Logging implementation for infrastructure, application, and domain layers is not required.  
- Avoid using AutoMapper due to debugging challenges.  
- Include a .gitignore file for the project.  
- Add a README file to provide project details.  
- Generate copilot instructions to assist with project development.
- All DateTime with UTC.