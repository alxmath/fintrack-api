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

    public async Task<Result<object>> Send(object request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(
            requestType,
            GetResponseType(requestType));

        dynamic handler = _serviceProvider.GetRequiredService(handlerType);

        async Task<Result<object>> handlerDelegate()
        {
            var result = await handler.Handle((dynamic)request, cancellationToken);
            return ConvertResult(result);
        }

        return await _executor.Execute(
            (dynamic)request,
            (Func<Task<Result<object>>>)handlerDelegate,
            cancellationToken);
    }

    private static Type GetResponseType(Type requestType)
    {
        var handlerInterface = requestType.Assembly
            .GetTypes()
            .SelectMany(t => t.GetInterfaces())
            .First(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) &&
                i.GetGenericArguments()[0] == requestType);

        return handlerInterface.GetGenericArguments()[1];
    }

    private static Result<object> ConvertResult(object result)
    {
        dynamic r = result;

        if (r.IsSuccess)
            return Result<object>.Success(r.Value);

        return Result<object>.Failure(r.Errors, r.ErrorCode);
    }
}
