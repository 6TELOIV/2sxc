﻿using System;
using System.Collections.Generic;
using System.Linq;
using ToSic.Eav.Data;
using ToSic.Eav.WebApi.Dto;

namespace ToSic.Sxc.WebApi.Cms
{
    public partial class EditLoadBackend
    {

        private Dictionary<string, LinkInfoDto> PrefetchLinks(int appId, EditDto editData)
        {
            try
            {
                // Step 1: try to find hyperlink fields
                var bundlesHavingLinks = BundleWithLinkFields(editData);

                var links = bundlesHavingLinks
                    .SelectMany(set => set.HyperlinkFields
                        .SelectMany(h => h.Value?.Select(linkAttrib => new
                        {
                            Link = linkAttrib.Value,
                            Converted = TryToConvertOrErrorText(appId, set.ContentTypeName, h.Key, linkAttrib.Value, set.Guid),
                        })))
                    .Where(set => set != null)
                    // Step 2: Check which ones have a link reference
                    .Where(set => ValueConverterBase.CouldBeReference(set.Link))
                    // Make distinct by Link
                    .GroupBy(l => l.Link)
                    .Select(g => g.First())
                    .ToList();


                // Step 3: Look them up
                // Step 4: return dictionary with these
                return links.ToDictionary(
                    s => s.Link,
                    s => s.Converted);
            }
            catch (Exception ex)
            {
                return new Dictionary<string, LinkInfoDto>
                {
                    {"Error", new LinkInfoDto { Value = "An error occurred pre-fetching the links " + ex.Message } }
                };
            }
        }

        private LinkInfoDto TryToConvertOrErrorText(int appId, string contentType, string field, string value, Guid entityGuid)
        {
            try
            {
                var hlnkBackend = _hyperlinkBackend ??
                                  (_hyperlinkBackend = GetService<HyperlinkBackend<int, int>>().Init(Log));
                var result = hlnkBackend.LookupHyperlink(appId, value, contentType, entityGuid, field);
                return result;
            }
            catch
            {
                return new LinkInfoDto  { Value = "error" };
            }
        }
        private HyperlinkBackend<int, int> _hyperlinkBackend;


        private static List<BundleWithLinkField> BundleWithLinkFields(EditDto editData, bool includeStringFields = false)
        {
            var bundlesHavingLinks = editData.Items
                // Only these with hyperlinks
                .Where(b =>
                {
                    var hasLinks = b.Entity?.Attributes?.Hyperlink?.Any() ?? false;
                    var hasString = includeStringFields && (b.Entity?.Attributes?.String?.Any() ?? false);
                    return hasLinks || hasString;
                })
                .Select(b => new BundleWithLinkField
                {
                    Guid = b.Entity.Guid,
                    HyperlinkFields = b.Entity.Attributes.Hyperlink,
                    StringFields = b.Entity.Attributes.String,
                    ContentTypeName = b.Entity.Type.Name,
                });
            return bundlesHavingLinks.ToList();
        }

        private class BundleWithLinkField
        {
            //public int AppId;
            public Guid Guid;
            public Dictionary<string, Dictionary<string, string>> HyperlinkFields;
            public Dictionary<string, Dictionary<string, string>> StringFields;
            public string ContentTypeName;
        }

    }
    
    
    

}
