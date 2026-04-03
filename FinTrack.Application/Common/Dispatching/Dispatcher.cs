using FinTrack.Application.Common.Execution;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrack.Application.Common.Dispatching;

public class Dispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HandlerExecutor _executor;

    public Dispatcher(IServiceProvider serviceProvider, HandlerExecutor executor)
    {
        _serviceProvider = serviceProvider;
        _executor = executor;
    }

    public async Task<Result<TResponse>> Send<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
    {
        var handler = _serviceProvider
            .GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        var validator = _serviceProvider
            .GetService<IValidator<TRequest>>();

        return await _executor.Execute(
            request,
            () => handler.Handle(request, cancellationToken),
            validator,
            cancellationToken);
    }
}
