using MediatR;

namespace WebAPI.Domain.Common;

/// <summary>
/// Interface for domain events
/// </summary>
public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}

/// <summary>
/// Base interface for all entities
/// </summary>
public interface IEntity
{
    Guid Id { get; set; }
}

/// <summary>
/// Interface for entities that support audit tracking
/// </summary>
public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    string CreatedBy { get; set; }
    string? UpdatedBy { get; set; }
}

/// <summary>
/// Interface for entities that emit domain events
/// </summary>
public interface IEventEmitter
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent domainEvent);
    void RemoveDomainEvent(IDomainEvent domainEvent);
    void ClearDomainEvents();
}

/// <summary>
/// Interface for soft delete functionality
/// </summary>
public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}