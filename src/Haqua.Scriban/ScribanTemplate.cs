using Scriban;
using Scriban.Runtime;
using WebMarkupMin.Core;

namespace Haqua.Scriban;

public class ScribanTemplate
{
    private readonly ITemplateLoader _templateLoader;
    private readonly Dictionary<string, string> _templates = new();

    public ScribanTemplate(ScribanTemplateOptions options)
    {
        LoadTemplate(options);
        _templateLoader = new IncludeFromDictionary(_templates);
    }

    public ValueTask<string> RenderAsync(string views, object? model = null)
    {
        var scriptObject = new ScriptObject { ["model"] = model };

        var context = new TemplateContext { TemplateLoader = _templateLoader };
        context.PushGlobal(scriptObject);

        var template = Template.Parse(_templates[views]);
        return template.RenderAsync(context);
    }

    private void LoadTemplate(ScribanTemplateOptions options)
    {
        _templates.Clear();

        var viewsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, options.Directory ?? "");
        var views = Directory.GetFiles(viewsDir, "*.html", SearchOption.AllDirectories);

        var htmlMinifier = new HtmlMinifier();

        foreach (var view in views)
        {
            var key = view
                .Replace(viewsDir + Path.DirectorySeparatorChar, "")
                .Replace("\\", "/");

            var viewFile = File.ReadAllText(view);

            if (options.MinifyTemplate)
            {
                var viewMinified = htmlMinifier.Minify(viewFile, false);
                _templates.Add(key, viewMinified.MinifiedContent);
            }
            else
            {
                _templates.Add(key, viewFile);
            }
        }
    }
}