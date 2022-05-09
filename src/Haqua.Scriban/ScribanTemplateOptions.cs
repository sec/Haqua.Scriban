namespace Haqua.Scriban;

public class ScribanTemplateOptions
{
    public string? ViewDirectory { get; set; }
    public bool MinifyTemplate { get; set; } = true;
    public bool WatchChanged { get; set; } = true;
}