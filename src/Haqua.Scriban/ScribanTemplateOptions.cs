using Microsoft.Extensions.FileProviders;

namespace Haqua.Scriban;

public class ScribanTemplateOptions
{
    public PhysicalFileProvider? FileProvider { get; set; }
    public bool MinifyTemplate { get; set; } = true;
    public bool WatchChanged { get; set; } = true;
}