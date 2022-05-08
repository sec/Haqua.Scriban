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