﻿using System.IO;
using System.Text.RegularExpressions;
using ToSic.Eav.Internal.Environment;
using ToSic.Eav.Plumbing;
using ToSic.Lib.Services;
using ToSic.Sxc.Code.Internal.HotBuild;

namespace ToSic.Sxc.Code.Internal.SourceCode;

[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public class SourceAnalyzer : ServiceBase
{
    private readonly IServerPaths _serverPaths;

    public SourceAnalyzer(IServerPaths serverPaths) : base("Sxc.RzrSrc")
    {
        ConnectServices(
            _serverPaths = serverPaths
        );
    }

    public CodeFileInfo TypeOfVirtualPath(string virtualPath)
    {
        var l = Log.Fn<CodeFileInfo>($"{nameof(virtualPath)}: '{virtualPath}'");
        string sourceCode = default;
        try
        {
            
            sourceCode = GetFileContentsOfVirtualPath(virtualPath);
            return sourceCode == null
                ? l.ReturnAndLog(CodeFileInfo.CodeFileNotFound)
                : l.ReturnAndLog(AnalyzeContent(virtualPath, sourceCode));
        }
        catch
        {
            return l.ReturnAndLog(CodeFileInfo.CodeFileUnknown(sourceCode), "error trying to find type");
        }
    }

    private string GetFileContentsOfVirtualPath(string virtualPath)
    {
        var l = Log.Fn<string>($"{nameof(virtualPath)}: '{virtualPath}'");

        if (virtualPath.IsEmptyOrWs())
            return l.Return(null, "no path");

        var path = _serverPaths.FullContentPath(virtualPath);
        if (path == null || path.IsEmptyOrWs())
            return l.Return(null, "no path");

        if (!File.Exists(path))
            return l.Return(null, "file not found");

        var sourceCode = File.ReadAllText(path);
        return l.Return(sourceCode, $"found, {sourceCode.Length} bytes");
    }

    private CodeFileInfo AnalyzeContent(string path, string sourceCode)
    {
        var l = Log.Fn<CodeFileInfo>($"{nameof(path)}:{path}");
        if (sourceCode.Length < 10)
            return l.Return(CodeFileInfo.CodeFileUnknown(sourceCode), "file too short");

        var isCs = path.ToLowerInvariant().EndsWith(CodeCompiler.CsFileExtension, StringComparison.InvariantCultureIgnoreCase);
        l.A($"isCs: {isCs}");

        if (isCs)
        {
            var csHasThisAppCode = IsThisAppCodeUsedInCs(sourceCode);
            l.A($"cs, thisApp: {csHasThisAppCode}");

            var className = Path.GetFileNameWithoutExtension(path);
            l.A($"cs, className: {className}");

            var baseClass = ExtractBaseClass(sourceCode, className);
            l.A($"cs, baseClass: {baseClass}");

            if (baseClass.IsEmptyOrWs())
                return l.Return(csHasThisAppCode ? CodeFileInfo.CodeFileUnknownWithThisAppCode(sourceCode) : CodeFileInfo.CodeFileUnknown(sourceCode), "Ok, cs file without base class");

            var csBaseClassMatch = CodeFileInfo.CodeFileInfoTemplates
                .FirstOrDefault(cf => cf.Inherits.EqualsInsensitive(baseClass) && cf.ThisApp == csHasThisAppCode);

            return csBaseClassMatch != null
                ? l.ReturnAndLog(new(csBaseClassMatch, sourceCode: sourceCode))
                : l.Return(csHasThisAppCode ? CodeFileInfo.CodeFileOtherWithThisAppCode(sourceCode) : CodeFileInfo.CodeFileOther(sourceCode), "Ok, cs file with other base class");
        }

        // Cshtml part
        var inheritsMatch = Regex.Match(sourceCode, @"@inherits\s+(?<BaseName>[\w\.]+)", RegexOptions.Multiline);

        if (!inheritsMatch.Success)
            return l.Return(CodeFileInfo.CodeFileUnknown(sourceCode), "no inherits found");

        var ns = inheritsMatch.Groups["BaseName"].Value;
        if (ns.IsEmptyOrWs())
            return l.Return(CodeFileInfo.CodeFileUnknown(sourceCode));

        var cshtmlHasThisAppCode = IsThisAppCodeUsedInCshtml(sourceCode);

        var findMatch = CodeFileInfo.CodeFileInfoTemplates
            .FirstOrDefault(cf => cf.Inherits.EqualsInsensitive(ns) && cf.ThisApp == cshtmlHasThisAppCode);

        return findMatch != null
            ? l.ReturnAndLog(new(findMatch, sourceCode: sourceCode))
            : l.Return(CodeFileInfo.CodeFileOther(sourceCode), $"namespace '{ns}' can't be found");
    }

    private static bool IsThisAppCodeUsedInCshtml(string sourceCode)
    {
        // Pattern to match '@using ThisApp.Code' not commented out

        // TODO: stv, update code because this code is not robust enough
        // it does not handle all edge cases, event it does not work correctly in some cases

        const string pattern = @"
            # Ignore leading whitespaces
            (?<=^\s*)

            # Match the @using statement
            @using\s+ThisApp\.Code

            # Ensure that it's not part of a comment
            (?<!@(/\*)[\s\S]*?@using\s+ThisApp\.Code) # Not in Razor comment";

        var options = RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace;
        var thisAppMatch = Regex.Match(sourceCode, pattern, options);

        return thisAppMatch.Success;
    }

    private static bool IsThisAppCodeUsedInCs(string sourceCode)
    {
        // Pattern to match 'using ThisApp.Code;' not in single-line or multi-line comments

        // TODO: stv, update code because this code is not robust enough
        // it does not handle all edge cases, event it does not work correctly in some cases
        const string pattern = @"
            # Ignore leading whitespaces
            (?<=^\s*)

            # Match the 'using ThisApp.Code;' statement
            using\s+ThisApp\.Code\s*;

            # Ensure that it's not part of a single-line comment
            (?<!//.*using\s+ThisApp\.Code\s*;)

            # Ensure that it's not part of a multi-line comment
            (?<!/\*[\s\S]*?using\s+ThisApp\.Code\s*;)";

        var options = RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace;
        var thisAppMatch = Regex.Match(sourceCode, pattern, options);

        return thisAppMatch.Success;
    }


    /// <summary>
    /// Extract 'className' base class from source code
    /// </summary>
    /// <param name="sourceCode"></param>
    /// <param name="className"></param>
    /// <returns></returns>
    /// <remarks>
    /// Code Complexity: This regex won't work well if the class declaration spans multiple lines or if there are comments between the class name and its base class.
    /// Generic Classes: If the base class uses generics, the regex needs to be adjusted to handle such cases.
    /// Multiple Inheritance: C# doesn't support multiple inheritance for classes. However, if interfaces are involved, this regex will only capture the first inherited type (which is usually the base class).
    /// Formatting: The regex assumes standard formatting.If there are unusual spacings or line breaks, it might not work correctly.
    /// Nested Classes: If the class is nested within another class, the regex will not match it.
    /// Comments and Strings: If the class declaration is commented out or appears within a string, the regex will still match it, which might not be desired.
    /// More robust solution can be done with Roslyn source pars, but additional packages can be needed.
    /// </remarks>
    public static string ExtractBaseClass(string sourceCode, string className)
    {
        if (sourceCode.IsEmptyOrWs() || className.IsEmptyOrWs()) return null;
        var pattern = $@"class\s+{className}\s*:\s*([^\s{{,]+)";
        var match = Regex.Match(sourceCode, pattern, RegexOptions.IgnoreCase);
        return match.Success && match.Groups.Count > 1 
            ? match.Groups[1].Value 
            : null;
    }
}