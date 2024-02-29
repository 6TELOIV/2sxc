﻿namespace ToSic.Sxc.Code.Internal.Generate;

internal class GeneratePropertyString: GeneratePropertyBase
{
    public override ValueTypes ForDataType => ValueTypes.String;

    public override List<CodeFragment> Generate(CodeGenSpecs specs, IContentTypeAttribute attribute, int tabs)
    {
        var name = attribute.Name;

        // If we're generating a "Title" for a property that is already the title, don't generate it again
        // Don't do this. Reason is that it will confuse people why it's missing, and serialization might also miss it.
        // if (name == Attributes.TitleNiceName && attribute.IsTitle) return [];

        return
        [
            GenPropSnip(tabs: tabs, returnType: "string", name: name, method: $"{specs.ItemAccessor}.String",
                parameters: "fallback: \"\"",
                summary:
                [
                    $"{name} as string. <br/>",
                    $"For advanced manipulation like scrubHtml, use .String(\"{name}\", scrubHtml: true) etc."
                ]),
        ];
    }
}