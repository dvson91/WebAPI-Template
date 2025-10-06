using MediatR;
using WebAPI.Domain.Interfaces;

namespace WebAPI.Application.Contracts;

/// <summary>
/// Attribute to mark commands that require database transaction management
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class RequiresTransactionAttribute : Attribute
{
}

/// <summary>
/// Interface marker for commands that require transaction handling
/// </summary>
public interface ITransactionalCommand
{
}

/// <summary>
/// Pipeline behavior that handles database transactions for commands marked with RequiresTransactionAttribute
/// or implementing ITransactionalCommand interface
/// </summary>
/// <typeparam name="TRequest">The request type</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
public class DbTransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;

    public DbTransactionBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Check if the request requires transaction management
        var requiresTransaction = request.GetType().GetCustomAttributes(typeof(RequiresTransactionAttribute), false).Any() ||
                                 request is ITransactionalCommand;

        if (!requiresTransaction)
        {
            return await next();
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var response = await next();

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return response;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}