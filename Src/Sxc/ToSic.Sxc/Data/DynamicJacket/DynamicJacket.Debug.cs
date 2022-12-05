﻿using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using ToSic.Eav.Data.Debug;
using ToSic.Eav.Data.PropertyLookup;
using ToSic.Eav.Documentation;

namespace ToSic.Sxc.Data
{
    public partial class DynamicJacket
    {
        private const string DumpSourceName = "Dynamic";
        [PrivateApi("internal")]
        public override List<PropertyDumpItem> _Dump(PropReqSpecs specs, string path)
        {
            if (_contents == null || !_contents.Any()) return new List<PropertyDumpItem>();

            if (string.IsNullOrEmpty(path)) path = DumpSourceName;

            var allProperties = _contents.ToList();

            var simpleProps = allProperties.Where(p => !(p.Value is JsonObject));
            var resultDynChildren = simpleProps.Select(p => new PropertyDumpItem
                {
                    Path = path + PropertyDumpItem.Separator + p.Key,
                    Property = FindPropertyInternal(specs.ForOtherField(p.Key),
                        new PropertyLookupPath().Add("DynJacket", p.Key)),
                    SourceName = DumpSourceName
                })
                .ToList();

            var objectProps = allProperties
                .Where(p => p.Value is JsonObject)
                .SelectMany(p =>
                {
                    var jacket = new DynamicJacket(p.Value.AsObject());
                    return jacket._Dump(specs, path + PropertyDumpItem.Separator + p.Key);
                })
                .Where(p => !(p is null));

            resultDynChildren.AddRange(objectProps);

            // TODO: JArrays

            return resultDynChildren.OrderBy(p => p.Path).ToList();
        }

    }
}
