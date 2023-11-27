﻿using System;
using ToSic.Lib.Coding;
using static ToSic.Eav.Parameters;

namespace ToSic.Sxc.Edit.Toolbar;

public partial interface IToolbarBuilder
{
    /// <summary>
    /// Create Button to **change the view/layout** of the data shown on the page.
    /// Can also be used to remove the same button on a toolbar which would have it by default.
    /// </summary>
    /// <param name="target">_not used ATM_ just here for API consistency</param>
    /// <param name="noParamOrder">see [](xref:NetCode.Conventions.NamedParameters)</param>
    /// <param name="tweak">Functional [Tweak API](xref:ToSic.Sxc.Services.ToolbarBuilder.TweakButtons) to modify UI and parameters (new v15.07)</param>
    /// <param name="ui">_optional_ configuration how to show, see [ui guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Ui)</param>
    /// <param name="parameters">_optional_ parameters for the command, see [parameters guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Parameters)</param>
    /// <param name="operation">_optional_ change [what should happen](xref:ToSic.Sxc.Services.ToolbarBuilder.Operation)</param>
    /// <returns>a _new_ toolbar builder - see [guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Index)</returns>
    IToolbarBuilder Layout(
        object target = null,
        NoParamOrder noParamOrder = default,
        Func<ITweakButton, ITweakButton> tweak = default,
        object ui = null,
        object parameters = null,
        string operation = null
    );

    /// <summary>
    /// Create Button to **run JS code**.
    /// Can also be used to remove the same button on a toolbar which would have it by default. 
    /// </summary>
    /// <param name="target">Name of the function to call, without parameters. </param>
    /// <param name="noParamOrder">see [](xref:NetCode.Conventions.NamedParameters)</param>
    /// <param name="tweak">Functional [Tweak API](xref:ToSic.Sxc.Services.ToolbarBuilder.TweakButtons) to modify UI and parameters (new v15.07)</param>
    /// <param name="ui">_optional_ configuration how to show, see [ui guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Ui)</param>
    /// <param name="parameters">_optional_ parameters for the command, see [parameters guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Parameters)</param>
    /// <param name="operation">_optional_ change [what should happen](xref:ToSic.Sxc.Services.ToolbarBuilder.Operation)</param>
    /// <returns>a _new_ toolbar builder - see [guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Index)</returns>
    IToolbarBuilder Code(
        object target,
        NoParamOrder noParamOrder = default,
        Func<ITweakButton, ITweakButton> tweak = default,
        object ui = null,
        object parameters = null,
        string operation = null
    );

    /// <summary>
    /// Create Button to open a dialog to **manage the fields/attributes** of the content type.
    /// Can also be used to remove the same button on a toolbar which would have it by default.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="noParamOrder">see [](xref:NetCode.Conventions.NamedParameters)</param>
    /// <param name="tweak">Functional [Tweak API](xref:ToSic.Sxc.Services.ToolbarBuilder.TweakButtons) to modify UI and parameters (new v15.07)</param>
    /// <param name="ui">_optional_ configuration how to show, see [ui guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Ui)</param>
    /// <param name="parameters">_optional_ parameters for the command, see [parameters guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Parameters)</param>
    /// <param name="operation">_optional_ change [what should happen](xref:ToSic.Sxc.Services.ToolbarBuilder.Operation)</param>
    /// <returns>a _new_ toolbar builder - see [guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Index)</returns>
    IToolbarBuilder Fields(
        object target = null,
        NoParamOrder noParamOrder = default,
        Func<ITweakButton, ITweakButton> tweak = default,
        object ui = null,
        object parameters = null,
        string operation = null
    );

    /// <summary>
    /// Create Button to **open the edit-template** (source-code) dialog.
    /// Can also be used to remove the same button on a toolbar which would have it by default.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="noParamOrder">see [](xref:NetCode.Conventions.NamedParameters)</param>
    /// <param name="tweak">Functional [Tweak API](xref:ToSic.Sxc.Services.ToolbarBuilder.TweakButtons) to modify UI and parameters (new v15.07)</param>
    /// <param name="ui">_optional_ configuration how to show, see [ui guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Ui)</param>
    /// <param name="parameters">_optional_ parameters for the command, see [parameters guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Parameters)</param>
    /// <param name="operation">_optional_ change [what should happen](xref:ToSic.Sxc.Services.ToolbarBuilder.Operation)</param>
    /// <returns>a _new_ toolbar builder - see [guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Index)</returns>
    IToolbarBuilder Template(
        object target = null,
        NoParamOrder noParamOrder = default,
        Func<ITweakButton, ITweakButton> tweak = default,
        object ui = null,
        object parameters = null,
        string operation = null
    );

    /// <summary>
    /// Create Button to **open the design/edit query** dialog.
    /// Can also be used to remove the same button on a toolbar which would have it by default.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="noParamOrder">see [](xref:NetCode.Conventions.NamedParameters)</param>
    /// <param name="tweak">Functional [Tweak API](xref:ToSic.Sxc.Services.ToolbarBuilder.TweakButtons) to modify UI and parameters (new v15.07)</param>
    /// <param name="ui">_optional_ configuration how to show, see [ui guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Ui)</param>
    /// <param name="parameters">_optional_ parameters for the command, see [parameters guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Parameters)</param>
    /// <param name="operation">_optional_ change [what should happen](xref:ToSic.Sxc.Services.ToolbarBuilder.Operation)</param>
    /// <returns>a _new_ toolbar builder - see [guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Index)</returns>
    IToolbarBuilder Query(
        object target = null,
        NoParamOrder noParamOrder = default,
        Func<ITweakButton, ITweakButton> tweak = default,
        object ui = null,
        object parameters = null,
        string operation = null
    );

    /// <summary>
    /// Create Button to open the **edit view settings** dialog.
    /// Can also be used to remove the same button on a toolbar which would have it by default.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="noParamOrder">see [](xref:NetCode.Conventions.NamedParameters)</param>
    /// <param name="tweak">Functional [Tweak API](xref:ToSic.Sxc.Services.ToolbarBuilder.TweakButtons) to modify UI and parameters (new v15.07)</param>
    /// <param name="ui">_optional_ configuration how to show, see [ui guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Ui)</param>
    /// <param name="parameters">_optional_ parameters for the command, see [parameters guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Parameters)</param>
    /// <param name="operation">_optional_ change [what should happen](xref:ToSic.Sxc.Services.ToolbarBuilder.Operation)</param>
    /// <returns>a _new_ toolbar builder - see [guide](xref:ToSic.Sxc.Services.ToolbarBuilder.Index)</returns>
    IToolbarBuilder View(
        object target = null,
        NoParamOrder noParamOrder = default,
        Func<ITweakButton, ITweakButton> tweak = default,
        object ui = null,
        object parameters = null,
        string operation = null
    );

}