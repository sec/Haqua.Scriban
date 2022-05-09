using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;

namespace Haqua.Scriban;

public static class ScribanTemplateServiceCollectionExtensions
{
    public static IServiceCollection AddScribanTemplate(
        this IServiceCollection services,
        ScribanTemplateOptions? options = null)
    {
        services.TryAddSingleton(provider =>
        {
            if (options?.FileProvider == null)
            {
                var webHostEnv = provider.GetService<IWebHostEnvironment>();
                var fileProvider = new PhysicalFileProvider(Path.Combine(webHostEnv!.ContentRootPath, "views"));

                if (options == null)
                    return new ScribanTemplate(new ScribanTemplateOptions { FileProvider = fileProvider });

                options.FileProvider = fileProvider;
            }

            return new ScribanTemplate(options);
        });

        return services;
    }
}