using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebAPI.Domain.Common;

namespace WebAPI.Infrastructure.Interceptors;

/// <summary>
/// Interceptor to automatically handle audit fields for entities implementing IAuditable
/// </summary>
public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;

    public AuditInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditFields(DbContext? context)
    {
        if (context == null) return;

        var currentUser = _currentUserService.GetCurrentUser();
        var utcNow = DateTime.UtcNow;

        var entries = context.ChangeTracker.Entries<IAuditable>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = utcNow;
                    entry.Entity.CreatedBy = currentUser;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = utcNow;
                    entry.Entity.UpdatedBy = currentUser;
                    break;
            }
        }
    }
}

/// <summary>
/// Service to get the current user (to be implemented based on authentication system)
/// </summary>
public interface ICurrentUserService
{
    string GetCurrentUser();
}

/// <summary>
/// Default implementation of current user service
/// This should be replaced with actual user context from authentication
/// </summary>
public class DefaultCurrentUserService : ICurrentUserService
{
    public string GetCurrentUser()
    {
        // TODO: Replace with actual user context from authentication
        // For now, return system user
        return "System";
    }
}