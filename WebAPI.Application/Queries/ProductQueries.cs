using MediatR;
using WebAPI.Application.Common;
using WebAPI.Application.DTOs;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Interfaces;

namespace WebAPI.Application.Queries;

public class GetProductByIdQuery : IRequest<Result<ProductDto?>>
{
    public Guid Id { get; set; }

    public GetProductByIdQuery(Guid id)
    {
        Id = id;
    }
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto?>>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Category> _categoryRepository;

    public GetProductByIdQueryHandler(
        IRepository<Product> productRepository,
        IRepository<Category> categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<ProductDto?>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (product == null)
        {
            return Result<ProductDto?>.Failure(MessageConstants.ProductNotFound);
        }

        var category = await _categoryRepository.GetByIdAsync(product.CategoryId, cancellationToken);

        var productDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Amount = product.Price.Amount,
            Currency = product.Price.Currency,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            CategoryName = category?.Name ?? string.Empty,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };

        return Result<ProductDto?>.Success(productDto);
    }
}

public class GetAllProductsQuery : IRequest<Result<IEnumerable<ProductDto>>>
{
    public bool? IsActive { get; set; }
    public Guid? CategoryId { get; set; }
}

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<IEnumerable<ProductDto>>>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Category> _categoryRepository;

    public GetAllProductsQueryHandler(
        IRepository<Product> productRepository,
        IRepository<Category> categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        
        var categoryLookup = categories.ToDictionary(c => c.Id, c => c.Name);

        var filteredProducts = products.AsEnumerable();

        if (request.IsActive.HasValue)
        {
            filteredProducts = filteredProducts.Where(p => p.IsActive == request.IsActive.Value);
        }

        if (request.CategoryId.HasValue)
        {
            filteredProducts = filteredProducts.Where(p => p.CategoryId == request.CategoryId.Value);
        }

        var productDtos = filteredProducts.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Amount = product.Price.Amount,
            Currency = product.Price.Currency,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            CategoryName = categoryLookup.GetValueOrDefault(product.CategoryId, string.Empty),
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        });

        return Result<IEnumerable<ProductDto>>.Success(productDtos);
    }
}