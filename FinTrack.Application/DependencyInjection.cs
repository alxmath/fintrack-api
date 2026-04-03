using FinTrack.Application.Common.Dispatching;
using FinTrack.Application.Common.Execution;
using FinTrack.Application.Common.Interfaces;
using FinTrack.Application.Common.Results;
using FinTrack.Application.Features.Categories.Create;
using FinTrack.Application.Features.Categories.Get;
using FinTrack.Application.Features.Transactions.Create;
using FinTrack.Application.Features.Transactions.Get;
using FinTrack.Application.Features.Transactions.GetById;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrack.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Handlers
        services.AddScoped<CreateTransactionHandler>();
        services.AddScoped<GetTransactionsHandler>();
        services.AddScoped<GetTransactionByIdHandler>();

        services.AddScoped<CreateCategoryHandler>();
        services.AddScoped<GetCategoriesHandler>();

        services.AddScoped<Dispatcher>();
        services.AddScoped<HandlerExecutor>();

        services.AddScoped<IRequestHandler<CreateTransactionCommand, CreateTransactionResponse>, CreateTransactionHandler>();
        services.AddScoped<IRequestHandler<GetTransactionsQuery, PagedResult<GetTransactionsResponse>>, GetTransactionsHandler>();
        services.AddScoped<IRequestHandler<GetTransactionByIdQuery, GetTransactionByIdResponse>, GetTransactionByIdHandler>();

        services.AddScoped<IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>, CreateCategoryHandler>();
        services.AddScoped<IRequestHandler<GetCategoriesQuery, List<GetCategoriesResponse>>, GetCategoriesHandler>();


        // Validators
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
