namespace DotMarkCMS.Helper;

public static class FrontMatterHelper
{
    public static FrontMatter ExtractFrontMatter(string file)
    {
        var lines = File.ReadAllLines(file);

        var frontMatter = new FrontMatter();
        bool isFrontMatter = false;

        foreach (var line in lines)
        {
            // Detect the start and end of the front matter block
            if (line.Trim() == "---")
            {
                isFrontMatter = !isFrontMatter;
                if (!isFrontMatter) break; // Break after closing front matter
                continue;
            }

            // If inside the front matter, extract key-value pairs
            if (isFrontMatter)
            {
                var parts = line.Split(':', 2);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim().ToLower();
                    var value = parts[1].Trim().Trim('\'', '"'); // Remove quotes if present

                    if (key.ToUpper() == "TITLE")
                    {
                        frontMatter.Title = value;
                    }
                    else if (key.ToUpper() == "SLUG")
                    {
                        frontMatter.Slug = value;
                    }
                    else if (key.ToUpper() == "SECTION")
                    {
                        frontMatter.Section = value;
                    }
                    else
                    {
                        frontMatter.MetaData[key] = value;
                    }
                }
            }
        }


        return frontMatter;
    }
}

public class FrontMatter
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public Dictionary<string, string> MetaData = [];
}