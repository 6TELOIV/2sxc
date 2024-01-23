﻿namespace ToSic.Sxc.Backend.Cms;

[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public abstract class LoadSettingsProviderBase(string logName) : ServiceBase(logName)
{
    protected Dictionary<string, object> SettingsByKeys(PropertyStack appSettings, List<string> keys) => Log.Func(l =>
    {
        // Try to find each setting
        var settings = keys.ToDictionary(
            key => key,
            key => appSettings.InternalGetPath(key).Result
        );

        return (settings, $"{settings.Count}");
    });

}