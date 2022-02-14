﻿namespace ToSic.Sxc.Oqt.Shared
{
    /// <summary>
    /// This should only contain constants which should really be final, no WIP or similar
    /// </summary>
    public static class OqtConstants
    {
        public const string SiteKeyForZoneId = "EavZone";

        public const string AppRoot = "2sxc";

        public const string AppRootPublicBase = AppRoot + "\\{0}";

        public const string SharedAppFolder =  "Shared";

        public const string ContentSubfolder = "Content";

        public const string ContentRootPublicBase = ContentSubfolder + "\\Tenants\\{0}\\Sites\\{1}";

        public const string ApiAppLinkPart = "api/sxc/app";

        public const string DownloadLinkTemplate = "/{0}/api/file/download/{1}";

        public const string UserTokenPrefix = "oqt";

        // #uncertain: maybe should incorporate the virtual path of the application?
        public const string UiRoot = "/Modules/ToSic.Sxc";
        // #uncertain: maybe should be more dynamic
        public const string SiteRoot = "/";

        // Logging constants
        public const string OqtLogPrefix = "Oqt";
        
        // Special Oqtane constants missing in Oqtane
        public const string EntityIdParam = "entityid";

        public const int Unknown = -1;
    }
}
