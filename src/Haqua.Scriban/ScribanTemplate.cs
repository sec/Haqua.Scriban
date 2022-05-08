using Scriban;
using Scriban.Runtime;
using WebMarkupMin.Core;

namespace Haqua.Scriban;

public class ScribanTemplate
{
    private readonly ScribanTemplateOptions _options;
    private readonly Dictionary<string, string> _templates = new();
    private ITemplateLoader? _templateLoader;

    public ScribanTemplate(ScribanTemplateOptions options)
    {
        _options = options;

        LoadTemplate();
    }

    public ValueTask<string> RenderAsync(string views, object? model = null)
    {
        var scriptObject = new ScriptObject { ["model"] = model };

        var context = new TemplateContext { TemplateLoader = _templateLoader };
        context.PushGlobal(scriptObject);

        var template = Template.Parse(_templates[views]);
        return template.RenderAsync(context);
    }

    private void LoadTemplate()
    {
        _templates.Clear();

        var viewsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _options.Directory ?? "");
        var views = Directory.GetFiles(viewsDir, "*.html", SearchOption.AllDirectories);

        var htmlMinifier = new HtmlMinifier();

        foreach (var view in views)
        {
            var key = view
                .Replace(viewsDir + Path.DirectorySeparatorChar, "")
                .Replace("\\", "/");

            var viewFile = File.ReadAllText(view);

            if (_options.MinifyTemplate)
            {
                var viewMinified = htmlMinifier.Minify(viewFile, false);
                _templates.Add(key, viewMinified.MinifiedContent);
            }
            else
            {
                _templates.Add(key, viewFile);
            }
        }

        _templateLoader = new IncludeFromDictionary(_templates);
    }
}