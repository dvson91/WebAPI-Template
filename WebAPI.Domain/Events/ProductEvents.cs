using WebAPI.Domain.Common;
using WebAPI.Domain.Entities;

namespace WebAPI.Domain.Events;

public class ProductUpdatedEvent : IDomainEvent
{
    public Product Product { get; }
    public DateTime OccurredOn { get; }

    public ProductUpdatedEvent(Product product)
    {
        Product = product;
        OccurredOn = DateTime.UtcNow;
    }
}

public class ProductStockUpdatedEvent : IDomainEvent
{
    public Product Product { get; }
    public int OldStock { get; }
    public int NewStock { get; }
    public DateTime OccurredOn { get; }

    public ProductStockUpdatedEvent(Product product, int oldStock, int newStock)
    {
        Product = product;
        OldStock = oldStock;
        NewStock = newStock;
        OccurredOn = DateTime.UtcNow;
    }
}

public class ProductDeactivatedEvent : IDomainEvent
{
    public Product Product { get; }
    public DateTime OccurredOn { get; }

    public ProductDeactivatedEvent(Product product)
    {
        Product = product;
        OccurredOn = DateTime.UtcNow;
    }
}

public class ProductActivatedEvent : IDomainEvent
{
    public Product Product { get; }
    public DateTime OccurredOn { get; }

    public ProductActivatedEvent(Product product)
    {
        Product = product;
        OccurredOn = DateTime.UtcNow;
    }
}