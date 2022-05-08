using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Haqua.Scriban;

public static class ScribanTemplateServiceCollectionExtensions
{
    public static IServiceCollection AddScribanTemplate(
        this IServiceCollection services,
        ScribanTemplateOptions options)
    {
        services.TryAddSingleton(_ => new ScribanTemplate(options));
        return services;
    }
}