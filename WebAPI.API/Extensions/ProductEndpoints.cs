using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.Commands;
using WebAPI.Application.DTOs;
using WebAPI.Application.Queries;

namespace WebAPI.API.Extensions;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/products")
                      .WithTags("Products")
                      .WithOpenApi();

        // GET /api/products
        group.MapGet("/", async (IMediator mediator, Guid? categoryId, bool? isActive) =>
        {
            var query = new GetAllProductsQuery { CategoryId = categoryId, IsActive = isActive };
            var result = await mediator.Send(query);

            return result.IsSuccess 
                ? Results.Ok(result) 
                : Results.BadRequest(result);
        })
        .WithName("GetAllProducts")
        .WithSummary("Get all products")
        .WithDescription("Retrieve all products with optional filtering by category and active status")
        .Produces<WebAPI.Application.Common.Result<IEnumerable<ProductDto>>>(200)
        .Produces<WebAPI.Application.Common.Result<IEnumerable<ProductDto>>>(400);

        // GET /api/products/{id}
        group.MapGet("/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var query = new GetProductByIdQuery(id);
            var result = await mediator.Send(query);

            return result.IsSuccess 
                ? Results.Ok(result) 
                : Results.NotFound(result);
        })
        .WithName("GetProductById")
        .WithSummary("Get product by ID")
        .WithDescription("Retrieve a specific product by its ID")
        .Produces<WebAPI.Application.Common.Result<ProductDto>>(200)
        .Produces<WebAPI.Application.Common.Result<ProductDto>>(404);

        // POST /api/products
        group.MapPost("/", async (IMediator mediator, [FromBody] CreateProductDto request) =>
        {
            var command = new CreateProductCommand
            {
                Name = request.Name,
                Description = request.Description,
                Amount = request.Amount,
                Currency = request.Currency,
                Stock = request.Stock,
                CategoryId = request.CategoryId
            };

            var result = await mediator.Send(command);

            return result.IsSuccess 
                ? Results.Created($"/api/products/{result.Data?.Id}", result)
                : Results.BadRequest(result);
        })
        .WithName("CreateProduct")
        .WithSummary("Create a new product")
        .WithDescription("Create a new product with the provided details")
        .Accepts<CreateProductDto>("application/json")
        .Produces<WebAPI.Application.Common.Result<ProductDto>>(201)
        .Produces<WebAPI.Application.Common.Result<ProductDto>>(400);

        // PUT /api/products/{id}
        group.MapPut("/{id:guid}", async (IMediator mediator, Guid id, [FromBody] UpdateProductDto request) =>
        {
            var command = new UpdateProductCommand
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                Amount = request.Amount,
                Currency = request.Currency
            };

            var result = await mediator.Send(command);

            return result.IsSuccess 
                ? Results.Ok(result) 
                : Results.BadRequest(result);
        })
        .WithName("UpdateProduct")
        .WithSummary("Update an existing product")
        .WithDescription("Update an existing product with the provided details")
        .Accepts<UpdateProductDto>("application/json")
        .Produces<WebAPI.Application.Common.Result<ProductDto>>(200)
        .Produces<WebAPI.Application.Common.Result<ProductDto>>(400)
        .Produces<WebAPI.Application.Common.Result<ProductDto>>(404);
    }
}