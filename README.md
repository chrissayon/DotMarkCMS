# DotMarkCMS

DotMarkCMS is a lightweight content management system (CMS) for .NET, designed to read and serve markdown files with metadata. It provides a simple and flexible way to build content-driven websites or applications using markdown as the source of truth, without relying on a traditional database.

## Features

- Markdown-based CMS: Use markdown files for content, metadata, and routing.
- Flexible Metadata Support: Define sections and slugs within front matter YAML.
- Automatic Routing: Auto-generates routes based on metadata.
- Directory Enumeration: Recursively scans directories for markdown content.
- Integration Ready: Easy to integrate into existing ASP.NET Core applications.

## Getting Started

1. Install the `DotMarkCMS` library:

```
dotnet add package DotMarkCMS
```

2. Add `DotMarkCMS` to your ASP.NET Core project:

```C#
var builder = WebApplication.CreateBuilder(args);

// Add DotMarkCMS services
builder.Services.AddDotMarkCMS(options =>
{
    options.RootDirectory = "Markdown";
    options.BaseUrl = "/api";
});

var app = builder.Build();

// Use DotMarkCMS
app.UseDotMarkCMS();

app.Run();
```

## Setting Up Your Markdown Files

DotMarkCMS expects markdown files with optional YAML front matter to generate routes and metadata. For example:

```yaml
---
title: "How to Use DotMarkCMS"
slug: "how-to-use"
section: "docs"
---
# How to Use DotMarkCMS

DotMarkCMS allows you to use markdown files as content sources.
```

- `slug`: Defines the URL slug for the markdown file.
- `section`: (Optional) Categorizes the content under a section (e.g., /docs/how-to-use).

## Options

DotMarkCMS provides several options to customize its behavior. Here’s the full set of options you can configure in AddDotMarkCMS:

```C#
public class DotMarkCMSOptions
{
    public string RootDirectory { get; set; } = string.Empty; // The directory containing markdown files
    public string BaseUrl { get; set; } = "/"; // Base URL for routing
}
```

## Example Usage

### Definiting Routes

DotMarkCMS automatically maps routes based on the directory structure and metadata in your markdown files.

For example, if you have the following directory structure:

```
Markdown/
    docs/
        how-to-use.md
    blog/
        first-post.md
```

And the how-to-use.md file contains:

```yaml
---
title: "How to Use DotMarkCMS"
slug: "how-to-use"
section: "docs"
---
```

The following route will be created:

```
/docs/how-to-use
```

DotMarkCMS will serve the content from how-to-use.md when the URL is accessed.
