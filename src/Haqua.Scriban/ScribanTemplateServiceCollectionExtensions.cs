using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Haqua.Scriban;

public static class ScribanTemplateServiceCollectionExtensions
{
    public static IServiceCollection AddScribanTemplate(
        this IServiceCollection services,
        ScribanTemplateOptions options)
    {
        services.TryAddSingleton(_ =>
        {
            var scribanTemplate = new ScribanTemplate(options);
            scribanTemplate.LoadTemplate();

            return scribanTemplate;
        });

        return services;
    }
}