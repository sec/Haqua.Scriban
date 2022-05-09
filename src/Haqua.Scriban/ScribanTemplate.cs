using Scriban;
using Scriban.Runtime;
using WebMarkupMin.Core;

namespace Haqua.Scriban;

public class ScribanTemplate : IDisposable
{
    private readonly FileSystemWatcher _fileSystemWatcher;
    private readonly HtmlMinifier _htmlMinifier;
    private readonly ScribanTemplateOptions _options;
    private readonly Dictionary<string, string> _templates = new();

    private ITemplateLoader? _templateLoader;

    public ScribanTemplate(ScribanTemplateOptions options)
    {
        _options = options;

        _htmlMinifier = new HtmlMinifier();
        _fileSystemWatcher = new FileSystemWatcher();

        if (_options.WatchChanged) WatchViewDirectoryChange();
    }

    public void Dispose()
    {
        _fileSystemWatcher.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<string> RenderAsync(string viewPath, object? model = null)
    {
        if (_templates.Count == 0) await LoadTemplateFromDirectory();

        var scriptObject = new ScriptObject { ["model"] = model };

        var context = new TemplateContext { TemplateLoader = _templateLoader };
        context.PushGlobal(scriptObject);

        var template = Template.Parse(_templates[viewPath]);
        var result = await template.RenderAsync(context);

        return result;
    }

    private async Task LoadTemplateFromDirectory()
    {
        if (_options.ViewDirectory == null) throw new ArgumentNullException();

        _templates.Clear();

        var viewFiles = Directory.GetFiles(_options.ViewDirectory, "*.html", SearchOption.AllDirectories);
        foreach (var viewPath in viewFiles) await LoadTemplate(viewPath);

        _templateLoader = new IncludeFromDictionary(_templates);
    }

    private async Task LoadTemplate(string viewPath)
    {
        var fileReady = false;
        while (!fileReady)
            try
            {
                var view = await File.ReadAllTextAsync(viewPath);
                fileReady = true;

                var viewName = viewPath
                    .Replace(_options.ViewDirectory + Path.DirectorySeparatorChar, "")
                    .Replace("\\", "/");

                if (_options.MinifyTemplate)
                {
                    var viewMinified = _htmlMinifier.Minify(view, false);
                    _templates[viewName] = viewMinified.MinifiedContent;
                }
                else
                {
                    _templates[viewName] = view;
                }
            }
            catch (IOException _)
            {
                await Task.Delay(100);
            }
    }

    private void WatchViewDirectoryChange()
    {
        if (_options.ViewDirectory == null) throw new ArgumentNullException();

        _fileSystemWatcher.Path = _options.ViewDirectory;

        _fileSystemWatcher.Changed += OnViewChanged;
        _fileSystemWatcher.Renamed += OnViewChanged;

        _fileSystemWatcher.IncludeSubdirectories = true;
        _fileSystemWatcher.EnableRaisingEvents = true;
    }

    private void OnViewChanged(object s, FileSystemEventArgs e)
    {
        var viewPath = e.FullPath.TrimEnd('~');
        LoadTemplate(viewPath);
    }
}