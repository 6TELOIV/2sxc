﻿using System.Linq;
using ToSic.Eav.Apps;
using ToSic.Eav.Plumbing;
using ToSic.Lib.Services;
using ToSic.Sxc.Internal;

namespace ToSic.Sxc.Edit.Internal.Toolbar;

[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
internal class ToolbarButtonDecoratorHelper(IAppStates appStates) : ServiceBase($"{SxcLogging.SxcLogName}.TbdHlp")
{
    public IAppIdentity MainAppIdentity { get; set; }

    internal ToolbarButtonDecorator GetDecorator(IAppIdentity appIdentity, string typeName, string command)
    {
        // If no special context was given, use the main one from the current context
        appIdentity ??= MainAppIdentity;

        if (appIdentity == null || !typeName.HasValue() || !command.HasValue()) return null;

        var appState = appStates.GetReader(appIdentity);

        var type = appState?.GetContentType(typeName);
        if (type == null) return null;

        var md = type.Metadata
            .OfType(ToolbarButtonDecorator.TypeName)
            .ToList();

        return md
            .Select(m => new ToolbarButtonDecorator(m))
            .FirstOrDefault(d => d.Command.EqualsInsensitive(command));

    }
}