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

        // Validators
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
