using DotMarkCMS.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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

        // Enumerate all markdown files within the directory
        var files = Directory.EnumerateFiles(options.RootDirectory, "*.md", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            // Extract metadata
            var frontMatter = FrontMatterHelper.ExtractFrontMatter(file);
            string url = string.IsNullOrEmpty(frontMatter.Section) ? string.Empty : frontMatter.Section; 
            url += $"/{frontMatter.Slug}";

            // Construct the route based on metadata
            app.MapGet(url, async () =>
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
                var content = await File.ReadAllTextAsync(filePath);
                return content;
            })
            .WithTags(frontMatter.Section)
            .WithName(url)
            .WithOpenApi();
        }
    }
}
