﻿using ToSic.Eav.Code.Infos;
using static ToSic.Eav.Code.Infos.CodeInfoWarning;

namespace ToSic.Sxc.Code
{
    public class DynamicCode16Warnings
    {
        public static ICodeInfo AvoidSettingsResources = Warn("no-settings-resources-on-code16",
            message: "Don't use Settings or Resources - use App.Settings/App.Resources or SettingsStack / ResourcesStack");

        public static ICodeInfo NoDataMyContent = Warn("no-data-my-content-code16",
            message: "Don't use Data.MyContent - use new MyItem or MyItems");
        public static ICodeInfo NoDataMyHeader = Warn("no-data-my-header-code16",
            message: "Don't use Data.MyHeader - use new MyHeader");
        public static ICodeInfo NoDataMyData = Warn("no-data-my-data-code16",
            message: "Don't use Data.MyData - use new TODO!");
    }
}
