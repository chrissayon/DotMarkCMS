using DotMarkCMS.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DotMarkCMS.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddDotMarkCMS(this IServiceCollection services, CancellationToken cancellationToken = default)
    {
        services.Configure<DotMarkCMSOptions>(options =>
        {
            options.RootDirectory = "/";
        });
    }

    public static void AddDotMarkCMS(this IServiceCollection services, Action<DotMarkCMSOptions> options, CancellationToken cancellationToken = default)
    {
        services.Configure(options);
    }

    public static void UseDotMarkCMS(this WebApplication app)
    {
        var options = app.Services.GetRequiredService<IOptions<DotMarkCMSOptions>>().Value;

        // Slugs
        var directories = Directory.EnumerateDirectories(options.RootDirectory);
        foreach (string directory in directories)
        {
            var subfolder = directory.Split('\\').Last();
            app.MapGet($"{subfolder}", () =>
            {
                var slugs = Directory.EnumerateFiles(directory)
                                     .Select(f => FrontMatterHelper.ExtractFrontMatter(f).Url)
                                     .ToList();
                
                return slugs;
            })
            .WithName($"{subfolder}")
            .WithOpenApi();

            // Files
            var files = Directory.EnumerateFiles(directory, "*.md", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                var metaData = FrontMatterHelper.ExtractFrontMatter(file);
                app.MapGet($"{subfolder}/{metaData.Url}", () =>
                {
                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
                    var content = File.ReadAllTextAsync(filePath);
                    return content;
                })
                .WithName($"{file}")
                .WithOpenApi();
            }
        }

        // Root
        var rootFiles = Directory.EnumerateFiles(options.RootDirectory, "*.md");
        foreach (string file in rootFiles)
        {
            var metaData = FrontMatterHelper.ExtractFrontMatter(file);
            app.MapGet($"{metaData.Url}", () =>
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
