using WebAPI.Domain.Common;
using WebAPI.Domain.Entities;

namespace WebAPI.Domain.Events;

public class CategoryCreatedEvent : IDomainEvent
{
    public Category Category { get; }
    public DateTime OccurredOn { get; }

    public CategoryCreatedEvent(Category category)
    {
        Category = category;
        OccurredOn = DateTime.UtcNow;
    }
}

public class CategoryUpdatedEvent : IDomainEvent
{
    public Category Category { get; }
    public DateTime OccurredOn { get; }

    public CategoryUpdatedEvent(Category category)
    {
        Category = category;
        OccurredOn = DateTime.UtcNow;
    }
}

public class CategoryDeactivatedEvent : IDomainEvent
{
    public Category Category { get; }
    public DateTime OccurredOn { get; }

    public CategoryDeactivatedEvent(Category category)
    {
        Category = category;
        OccurredOn = DateTime.UtcNow;
    }
}

public class CategoryActivatedEvent : IDomainEvent
{
    public Category Category { get; }
    public DateTime OccurredOn { get; }

    public CategoryActivatedEvent(Category category)
    {
        Category = category;
        OccurredOn = DateTime.UtcNow;
    }
}