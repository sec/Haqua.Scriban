using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;

namespace Haqua.Scriban;

internal class IncludeFromDictionary : ITemplateLoader
{
    private readonly Dictionary<string, string> _templates;

    public IncludeFromDictionary(Dictionary<string, string> templates)
    {
        _templates = templates;
    }

    public string GetPath(TemplateContext context, SourceSpan callerSpan, string templateName)
    {
        return templateName;
    }

    public string Load(TemplateContext context, SourceSpan callerSpan, string templatePath)
    {
        return _templates[templatePath];
    }

    public ValueTask<string> LoadAsync(TemplateContext context, SourceSpan callerSpan, string templatePath)
    {
        return ValueTask.FromResult(_templates[templatePath]);
    }
}