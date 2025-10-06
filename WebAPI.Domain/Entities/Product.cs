using WebAPI.Domain.Common;
using WebAPI.Domain.ValueObjects;
using WebAPI.Domain.Events;

namespace WebAPI.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Money Price { get; private set; } = null!;
    public int Stock { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; set; } = null!;

    // Private constructor for EF Core
    private Product() { }

    public Product(string name, string description, Money price, int stock, Guid categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
        IsActive = true;

        AddDomainEvent(new ProductCreatedEvent(this));
    }

    public void UpdateDetails(string name, string description, Money price)
    {
        Name = name;
        Description = description;
        Price = price;

        AddDomainEvent(new ProductUpdatedEvent(this));
    }

    public void UpdateStock(int newStock)
    {
        var oldStock = Stock;
        Stock = newStock;

        AddDomainEvent(new ProductStockUpdatedEvent(this, oldStock, newStock));
    }

    public void Deactivate()
    {
        IsActive = false;

        AddDomainEvent(new ProductDeactivatedEvent(this));
    }

    public void Activate()
    {
        IsActive = true;

        AddDomainEvent(new ProductActivatedEvent(this));
    }
}