using WebAPI.Domain.Common;
using WebAPI.Domain.Events;

namespace WebAPI.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    
    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    // Private constructor for EF Core
    private Category() { }

    public Category(string name, string description)
    {
        Name = name;
        Description = description;
        IsActive = true;

        AddDomainEvent(new CategoryCreatedEvent(this));
    }

    public void UpdateDetails(string name, string description)
    {
        Name = name;
        Description = description;

        AddDomainEvent(new CategoryUpdatedEvent(this));
    }

    public void Deactivate()
    {
        IsActive = false;

        AddDomainEvent(new CategoryDeactivatedEvent(this));
    }

    public void Activate()
    {
        IsActive = true;

        AddDomainEvent(new CategoryActivatedEvent(this));
    }
}