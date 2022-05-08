using Scriban;
using Scriban.Runtime;

namespace Haqua.Scriban;

public class ScribanTemplate
{
    private readonly ScribanTemplateOptions _options;

    private readonly Dictionary<string, string> _templates = new();

    public ScribanTemplate(ScribanTemplateOptions options)
    {
        _options = options;
    }

    public void LoadTemplate()
    {
        _templates.Clear();

        var viewsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _options.Directory ?? "");
        var views = Directory.GetFiles(viewsDir, "*.html", SearchOption.AllDirectories);

        foreach (var view in views)
        {
            var key = view
                .Replace(viewsDir + Path.DirectorySeparatorChar, "")
                .Replace("\\", "/");

            var value = File.ReadAllText(view);

            _templates.Add(key, value);
        }
    }

    public ValueTask<string> RenderAsync(string views, object? model = null)
    {
        var scriptObject = new ScriptObject();
        scriptObject["model"] = model;

        var context = new TemplateContext();
        context.TemplateLoader = new IncludeFromDictionary(_templates);
        context.PushGlobal(scriptObject);

        var template = Template.Parse(_templates[views]);
        return template.RenderAsync(context);
    }
}