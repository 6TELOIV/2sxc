﻿using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ToSic.Eav.Plumbing;
using ToSic.Lib.Logging;
using ToSic.Lib.Services;
using System.Web.Hosting;
using ToSic.Sxc.Code.Help;
using System;

namespace ToSic.Sxc.Engines
{
    [Serializable]
    public class DnnRazorSourceAnalyzer: ServiceBase
    {
        public DnnRazorSourceAnalyzer() : base("Dnn.RzrSrc") { }

        public CodeFileInfo TypeOfVirtualPath(string virtualPath)
        {
            var l = Log.Fn<CodeFileInfo>($"{nameof(virtualPath)}: '{virtualPath}'");
            try
            {
                var contents = GetFileContentsOfVirtualPath(virtualPath);
                return contents == null
                    ? l.ReturnAndLog(CodeFileInfo.CodeFileNotFound)
                    : l.ReturnAndLog(AnalyzeContent(contents));
            }
            catch
            {
                return l.ReturnAndLog(CodeFileInfo.CodeFileUnknown, "error trying to find type");
            }
        }

        private string GetFileContentsOfVirtualPath(string virtualPath)
        {
            var l = Log.Fn<string>($"{nameof(virtualPath)}: '{virtualPath}'");

            if (virtualPath.IsEmptyOrWs())
                return l.Return(null, "no path");

            var path = HostingEnvironment.MapPath(virtualPath);
            if (path == null || path.IsEmptyOrWs())
                return l.Return(null, "no path");

            if (!File.Exists(path))
                return l.Return(null, "file not found");

            var contents = File.ReadAllText(path);
            return l.Return(contents, $"found, {contents?.Length} bytes");
        }

        public CodeFileInfo AnalyzeContent(string contents)
        {
            var l = Log.Fn<CodeFileInfo>();
            if (contents.Length < 10)
                return l.Return(CodeFileInfo.CodeFileUnknown, "file too short");

            var inheritsMatch = Regex.Match(contents, @"@inherits\s+(?<BaseName>[\w\.]+)", RegexOptions.Multiline);

            if (!inheritsMatch.Success)
                return l.Return(CodeFileInfo.CodeFileUnknown, "no namespace found");

            var ns = inheritsMatch.Groups["BaseName"].Value;
            if (ns.IsEmptyOrWs())
                return l.Return(CodeFileInfo.CodeFileUnknown);

            var findMatch = CodeFileInfo.CodeFileList
                .FirstOrDefault(cf => cf.Inherits.EqualsInsensitive(ns));

            return findMatch != null
                ? l.ReturnAndLog(findMatch)
                : l.Return(CodeFileInfo.CodeFileOther, $"namespace '{ns}' can't be found");
        }
    }
}
