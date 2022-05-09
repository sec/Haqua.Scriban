using Haqua.Scriban;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScribanTemplate(new ScribanTemplateOptions
{
    ViewDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "views"
});

var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/", () => Results.Extensions.ScribanView(
    "pages/home.html",
    new { Name = "Scriban Template" }
));

app.Run();