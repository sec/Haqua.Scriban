using Haqua.Scriban;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScribanTemplate();

var app = builder.Build();

app.UseStaticFiles();

app.MapGet("/", () => new ScribanView("pages/home.html", new { Name = "Scriban Template" }));

app.Run();