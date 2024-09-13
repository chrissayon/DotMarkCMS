using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DotMarkCMS.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddDotMarkCMS(this IServiceCollection services, CancellationToken cancellationToken = default)
    {

    }

    public static void AddDotMarkCMS(this IServiceCollection services, Action<DotMarkCMSOptions> options, CancellationToken cancellationToken = default)
    {
        services.Configure(options);
    }

    public static void UseDotMarkCMS(this WebApplication app)
    {
        var options = app.Services.GetRequiredService<IOptions<DotMarkCMSOptions>>().Value;

        var files = Directory.EnumerateFiles(options.RootDirectory);

        foreach (string file in files)
        {
            app.MapGet($"/{file.Split("\\").Last()}", () =>
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
                var content = File.ReadAllTextAsync(filePath);
                return content;
            })
            .WithName($"{file}")
            .WithOpenApi();
        }
    }
}
