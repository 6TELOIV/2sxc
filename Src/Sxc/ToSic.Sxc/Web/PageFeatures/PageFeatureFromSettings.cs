﻿namespace ToSic.Sxc.Web.PageFeatures
{
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class PageFeatureFromSettings: PageFeature
    {
        public PageFeatureFromSettings(string key, bool autoOptimize = default, string[] needs = null, string html = null) : base(key, "", "", needs, html)
        {
            AutoOptimize = autoOptimize;
        }

        public bool AlreadyProcessed { get; set; }

        public bool AutoOptimize { get; }

    }
}
