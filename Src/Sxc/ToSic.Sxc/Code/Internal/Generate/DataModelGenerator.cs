﻿using System.Text;
using ToSic.Eav.Apps;
using ToSic.Lib.Helpers;
using ToSic.Lib.Services;
using ToSic.Sxc.Internal;

namespace ToSic.Sxc.Code.Internal.Generate;

/// <summary>
/// Experimental
/// </summary>
public class DataModelGenerator() : ServiceBase(SxcLogging.SxcLogName + ".DMoGen")
{
    internal const int DepthNamespace = 0;
    internal const int DepthClass = 1;
    internal const int DepthProperty = 2;
    internal const int Indent = 4;
    internal const string NamespaceBody = "[NAMESPACE-BODY]";
    internal const string ClassBody = "[CLASS-BODY]";



    internal GenerateCodeHelper GenHelper = new();

    public string Generate(IAppState state)
    {
        // TODO:
        // - add comment with date/time stamp
        // - add comment with version of generator
        // - add attribute to mention what data-type it's for, in case the name doesn't match to help with errors
        // - consider modifying the ToString to better show what it's doing
        // - check Equals of the new objects
        var sb = new StringBuilder();
        // Generate usings
        sb.Append(GenerateUsings());
        // Generate namespace
        sb.Append(GenerateNamespace("ThisApp.Data"));

        // Generate classes for all types in scope Default
        var types = state.ContentTypes.OfScope(Scopes.Default);
        var classesSb = new StringBuilder();
        foreach (var type in types)
        {
            var classSb = GenerateClass(type.Name);
            var (hasProps, propsSb) = ClassProperties(type.Attributes.ToList());
            if (hasProps)
                classSb.Replace(ClassBody, propsSb.ToString());

            //var propsSb = new StringBuilder();
            //foreach (var attribute in type.Attributes) 
            //    propsSb.Append(GenerateProperty(attribute));
            //classSb.Replace(ClassBody, propsSb.ToString());

            classesSb.Append(classSb);
        }

        sb.Replace(NamespaceBody, classesSb.ToString());

        return sb.ToString();
    }

    public StringBuilder GenerateUsings()
    {
        var sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using System.Linq;");
        sb.AppendLine("using ThisApp.Data;");
        return sb;
    }

    public StringBuilder GenerateNamespace(string @namespace)
    {
        // TODO:
        // - configure initial namespace
        var sb = new StringBuilder();
        sb.AppendLine($"namespace {@namespace}");
        sb.AppendLine("{");
        sb.AppendLine(NamespaceBody);
        sb.AppendLine("}");
        return sb;
    }

    public StringBuilder GenerateClass(string className)
    {
        // TODO:
        // - base class
        // - additional base class when a property has the same name as the class
        var indent = GenHelper.Indentation(DepthClass);
        var sb = new StringBuilder();
        sb.AppendLine(indent + $"public partial class {className}");
        sb.AppendLine(indent + "{");
        // empty constructor with Xml Comment
        sb.Append(GenHelper.XmlComment(indent, summary: $"todo - empty constructor so As...<{className}>() works."));
        sb.AppendLine(GenHelper.Indentation(DepthProperty) + $"public {className}() {{ }}");

        // body
        sb.AppendLine(ClassBody);

        // close class
        sb.AppendLine(indent + "}");
        return sb;
    }

    public (bool, StringBuilder) ClassProperties(List<IContentTypeAttribute> attributes)
    {

        // Generate all properties with the helpers
        var propsSnippets = attributes
            .Select(a => new
            {
                Attribute = a,
                Generators = PropertyGenerators.Where(p => p.ForDataType == a.Type).ToList()
            })
            .Where(a => a.Generators.Any())
            .SelectMany(set =>
            {
                return set.Generators
                    .SelectMany(p => p.Generate(set.Attribute, DepthProperty));
            })
            .ToList();

        if (!propsSnippets.Any())
            return (false, null);

        // Detect duplicate names as this would fail
        // If we have duplicates, keep the first with a real priority
        var deduplicated = propsSnippets
            .GroupBy(ps => ps.NameId)
            .SelectMany(g => g.OrderBy(ps => ps.Priority ? 0 : 1).Take(1))
            .ToList();

        var sb = new StringBuilder();
        foreach (var genCode in deduplicated)
            sb.Append(genCode.Code);

        return (true, sb);
    }

    //public StringBuilder GenerateProperty(IContentTypeAttribute attribute)
    //{
    //    // TODO:
    //    // - figure out MethodName - eg. String(...)
    //    // - figure out fallback value
    //    // - possible multi-properties eg. Link, LinkUrl, Image / Images
    //    // - add XML comment

    //    // String builder with empty line
    //    var sb = new StringBuilder();
    //    sb.AppendLine();

    //    var indent = GenHelper.Indentation(DepthProperty);
    //    var type = ValueTypeHelpers.GetType(attribute.Type);
    //    if (type == null)
    //        return sb.AppendLine(indent + $"// Nothing generated for {attribute.Name} as type-specs missing");

    //    sb.Append(GenHelper.XmlComment(indent, summary: $"todo - {attribute.Name}"));
    //    return sb.AppendLine($"{indent}public {type.Name} {attribute.Name} => {nameof(ICanBeItem.Item)}.{attribute.Type}();");
    //}

    private List<GeneratePropertyBase> PropertyGenerators => _propGenerators.Get(() =>
    [
        new GeneratePropertyBool(),
        new GeneratePropertyString(),
        new GeneratePropertyEmpty(),
        new GeneratePropertyHyperlink(),
        // new GeneratePropertyNumber(),
        // new GeneratePropertyDateTime(),
    ]);
    private readonly GetOnce<List<GeneratePropertyBase>> _propGenerators = new();
}