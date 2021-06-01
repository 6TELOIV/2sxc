﻿namespace ToSic.Sxc.Web.PageFeatures
{
    public class BuiltInFeatures
    {
        public static PageFeature PageContext = new PageFeature("2sxc.PageContext", "the $2sxc headers in the page so everything works");

        public static PageFeature Core = new PageFeature("2sxc.Core", "2sxc core js APIs", requires: new[]
        {
            PageContext.Key
        });

        public static PageFeature EditApi = new PageFeature("2sxc.EditApi", "2sxc inpage editing APIs", requires: new[]
        {
            Core.Key
        });

        public static PageFeature EditUi =
            new PageFeature("2sxc.EditUi", "2sxc inpage editing UIs / Toolbar", requires: new[]
            {
                Core.Key, 
                EditApi.Key
            });

        public static PageFeature TurnOn = new PageFeature("turnOn", "turnOn JS library");

    }
}
