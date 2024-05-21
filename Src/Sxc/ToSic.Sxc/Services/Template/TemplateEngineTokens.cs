﻿using ToSic.Eav.LookUp;
using ToSic.Lib.Data;
using ToSic.Sxc.Services.Template;

namespace ToSic.Sxc.Services.Templates;

internal class TemplateEngineTokens(ILookUpEngine original): ITemplateEngine, IWrapper<ILookUpEngine>
{

    private const string TemplateKey = "Template";

    /// <summary>
    /// it's important to use depth 0 to prevent things such as query-string parameters providing new tokens
    /// otherwise you could have a [QueryString:Id] but the url being ?id=[page:id] or of course worse
    /// </summary>
    private const int MaxDepth = 0;

    IEnumerable<ILookUp> ITemplateEngine.GetSources(NoParamOrder protector = default, int depth = 0)
    {
        if (depth == 0)
            return original.Sources;

        // loop through depth to get all underlying sources
        var current = original;
        var sources = current.Sources;
        for (var i = 0; i < depth; i++)
        {
            if (current.Downstream == null) break;
            current = current.Downstream;
            sources = sources.Concat(current.Sources);
        }
        return sources.ToList();
    }

    string ITemplateEngine.Parse(string template)
        => ((ITemplateEngine)this).Parse(template, protector: default, sources: null);

    string ITemplateEngine.Parse(string template, NoParamOrder protector, IEnumerable<ILookUp> sources)
    {
        var dic = new Dictionary<string, string>
        {
            [TemplateKey] = template
        };
        var result = original.LookUp(dic, overrides: sources, depth: MaxDepth);
        return result[TemplateKey];
    }

    /// <summary>
    /// For now just on explicit implementation, for debug, without enlarging the public API
    /// </summary>
    /// <returns></returns>
    ILookUpEngine IWrapper<ILookUpEngine>.GetContents() => original;
}