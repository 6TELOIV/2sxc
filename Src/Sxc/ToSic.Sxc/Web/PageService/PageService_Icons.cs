﻿using System.Collections.Generic;
using ToSic.Razor.Html5;
using ToSic.Razor.Internals.Page;

namespace ToSic.Sxc.Web.PageService
{
    public partial class PageService
    {
        /// <inheritdoc />
        public string AddIcon(string path,
            string doNotRelyOnParameterOrder = "Rule: All params must be named (https://r.2sxc.org/named-params)",
            string rel = "",
            int size = 0, string type = null)
        {
            Eav.Parameters.ProtectAgainstMissingParameterNames(doNotRelyOnParameterOrder, nameof(AddIcon), $"{nameof(path)}, {nameof(rel)}, {nameof(size)}, {nameof(type)}");
            AddToHead(new Icon(path, rel, size, type));
            return "";
        }

        /// <inheritdoc />
        public string AddIconSet(string path,
            string doNotRelyOnParameterOrder = "Rule: All params must be named (https://r.2sxc.org/named-params)",
            object favicon = null, IEnumerable<string> rels = null, IEnumerable<int> sizes = null)
        {
            Eav.Parameters.ProtectAgainstMissingParameterNames(doNotRelyOnParameterOrder, nameof(AddIcon), $"{nameof(path)}, {nameof(favicon)}, {nameof(rels)}, {nameof(sizes)}");
            foreach (var s in IconSet.GenerateIconSet(path, favicon, rels, sizes))
                AddToHead(s);
            return "";
        }

    }
}
