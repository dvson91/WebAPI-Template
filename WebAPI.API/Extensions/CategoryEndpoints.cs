using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.DTOs;
using WebAPI.Application.Queries;

namespace WebAPI.API.Extensions;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/categories")
                      .WithTags("Categories")
                      .WithOpenApi();

        // GET /api/categories
        group.MapGet("/", async (IMediator mediator) =>
        {
            // For now, return a simple success message
            // You can implement GetAllCategoriesQuery similar to products
            return Results.Ok(new { message = "Categories endpoint - not yet implemented" });
        })
        .WithName("GetAllCategories")
        .WithSummary("Get all categories")
        .WithDescription("Retrieve all categories")
        .Produces<object>(200);

        // GET /api/categories/{id}
        group.MapGet("/{id:guid}", (IMediator mediator, Guid id) =>
        {
            // For now, return a simple success message
            // You can implement GetCategoryByIdQuery similar to products
            return Results.Ok(new { message = $"Category {id} endpoint - not yet implemented" });
        })
        .WithName("GetCategoryById")
        .WithSummary("Get category by ID")
        .WithDescription("Retrieve a specific category by its ID")
        .Produces<object>(200)
        .Produces<object>(404);

        // POST /api/categories
        group.MapPost("/", (IMediator mediator, [FromBody] CreateCategoryDto request) =>
        {
            // For now, return a simple success message
            // You can implement CreateCategoryCommand similar to products
            return Results.Ok(new { message = "Create category endpoint - not yet implemented", data = request });
        })
        .WithName("CreateCategory")
        .WithSummary("Create a new category")
        .WithDescription("Create a new category with the provided details")
        .Accepts<CreateCategoryDto>("application/json")
        .Produces<object>(201)
        .Produces<object>(400);

        // PUT /api/categories/{id}
        group.MapPut("/{id:guid}", (IMediator mediator, Guid id, [FromBody] UpdateCategoryDto request) =>
        {
            // For now, return a simple success message
            // You can implement UpdateCategoryCommand similar to products
            return Results.Ok(new { message = $"Update category {id} endpoint - not yet implemented", data = request });
        })
        .WithName("UpdateCategory")
        .WithSummary("Update an existing category")
        .WithDescription("Update an existing category with the provided details")
        .Accepts<UpdateCategoryDto>("application/json")
        .Produces<object>(200)
        .Produces<object>(400)
        .Produces<object>(404);
    }
}