using FinTrack.Application.Common.Results;

namespace FinTrack.Application.Common.Interfaces;

public interface IRequestHandler<TRequest, TResponse>
{
    Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}
