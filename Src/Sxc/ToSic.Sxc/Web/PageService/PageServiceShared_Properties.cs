﻿using System.Collections.Generic;
using System.Linq;
using ToSic.Eav.Logging;

namespace ToSic.Sxc.Web.PageService
{
    public partial class PageServiceShared
    {
        internal IList<PagePropertyChange> PropertyChanges { get; } = new List<PagePropertyChange>();

        public IList<PagePropertyChange> GetPropertyChangesAndFlush(ILog log)
        {
            var wrapLog = log.Call<IList<PagePropertyChange>>();
            var changes = PropertyChanges.ToArray().ToList();
            PropertyChanges.Clear();
            return wrapLog($"{changes.Count}", changes);
        }

        /// <summary>
        /// Add something to the queue for setting a page property
        /// </summary>
        internal void Queue(PageProperties property, string value, PageChangeModes change, string token)
        {
            PropertyChanges.Add(new PagePropertyChange
            {
                ChangeMode = GetMode(change),
                Property = property,
                Value = value,
                ReplacementIdentifier = token,
            });

        }

    }
}
