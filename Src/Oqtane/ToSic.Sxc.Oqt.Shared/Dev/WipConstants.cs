﻿using System;
using System.Collections.Generic;
using Oqtane.Models;
using ToSic.Eav.Run;

namespace ToSic.Sxc.Oqt.Shared.Dev
{
    /// <summary>
    /// Temporary constants which should be removed soon
    /// </summary>
    public class WipConstants
    {
        public static string DefaultLanguage => "en-us";
        public static string DefaultLanguageText = "Oqtane temp English";
        public static Dictionary<string,string> EmptyLanguages = new Dictionary<string, string>();
        public static List<TempTempCulture> EmptyCultureList = new List<TempTempCulture>();

        public static string ContentRoot = "/content-wip/";

        public static string AppRootPublicBase = "Tenants/1/Sites/1/";

        public static string HttpUrlRoot = "http://test-test/";

        // ADAM
        public const int MaxUploadSize = 25000;
        public const int ParentFolderNotFound = 0;
        public static void AdamNotImplementedYet() { }


        // User WIP
        public static Guid UserGuid = Guid.Empty;
        public static List<int> UserRoles = new List<int>();
        public static bool IsSuperUser = true;
        public static bool IsAdmin = true;
        public static bool IsDesigner = true;
        public static User NullUser = null;


        public static int NeedSiteFromHeader = 2;

        public static string WebApiPrefixFor1 = "1/";

        public static void DontDoAnythingImplementLater() { }
    }
}
