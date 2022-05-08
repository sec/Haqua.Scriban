# Haqua.Scriban

Integrate [Scriban](https://github.com/scriban/scriban) Template Engine to ASP.NET Core Minimal API.

[Scriban](https://github.com/scriban/scriban) is a fast, powerful, safe and lightweight scripting language and engine for .NET, which was primarily developed for text templating with a compatibility mode for parsing liquid templates.

## Example
```csharp
using Haqua.Scriban;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScribanTemplate(new ScribanTemplateOptions { Directory = "views" });

var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/", () => Results.Extensions.ScribanView(
    "pages/home.html",
    new
    {
        Title = "Home",
        Name = "Scriban Template Engine"
    }
));

app.Run();
```

## License
This software is released under the [BSD-Clause 2 license](https://opensource.org/licenses/BSD-2-Clause).
