﻿using System.Collections.Immutable;
using ToSic.Lib.Documentation;
using static ToSic.Eav.Parameters;

namespace ToSic.Sxc.Edit.Toolbar
{
    /// <summary>
    /// Experimental new API in v15.07 to improve how to configure the Ui of a button.
    ///
    /// > [!TIP]
    /// > Read more about this in [](xref:ToSic.Sxc.Services.ToolbarBuilder.TweakButtons)
    /// </summary>
    /// <remarks>
    /// Added in v15.07
    /// </remarks>
    [PublicApi]
    public interface ITweakButton
    {
        /// <summary>
        /// List of changes to apply to the UI parameter
        /// </summary>
        [PrivateApi] IImmutableList<object> UiMerge { get; }
        [PrivateApi] IImmutableList<object> ParamsMerge { get; }

        #region UI

        /// <summary>
        /// Add a floating note to the button.
        /// </summary>
        /// <param name="note">The note/message</param>
        /// <param name="noParamOrder">see [](xref:NetCode.Conventions.NamedParameters)</param>
        /// <param name="type">Optional type, like `info` (default), `warning`, `help`</param>
        /// <param name="background">Background color.</param>
        /// <returns></returns>
        /// <remarks>
        /// * New feature in v15.07
        /// </remarks>
        ITweakButton Note(
            string note = default,
            string noParamOrder = Protector,
            string type = default,
            string background = default
        );

        /// <summary>
        /// Set the show of this button.
        /// </summary>
        /// <param name="show">Optional show value, default is `true`</param>
        /// <returns></returns>
        ITweakButton Show(bool show = true);

        /// <summary>
        /// Set the color of this button.
        /// A color can be `red`, `green` or `#FFCC66` as well as transparent colors such as `#FFCC6699`
        /// </summary>
        /// <param name="color">The main color parameter. Can contain two values, comma separated.</param>
        /// <param name="noParamOrder"></param>
        /// <param name="background">Background color - will only take affect if the `color` was not set.</param>
        /// <param name="foreground">Foreground color - will only take affect if the `color` was not set.</param>
        /// <returns></returns>
        ITweakButton Color(
            string color = default,
            string noParamOrder = Protector,
            string background = default,
            string foreground = default
        );

        /// <summary>
        /// Set the title / Tooltip of the button.
        /// </summary>
        /// <param name="value">The title/tooltip to show</param>
        /// <returns></returns>
        ITweakButton Tooltip(string value);

        /// <summary>
        /// Set what group the button is in.
        /// This is rarely used.
        /// </summary>
        /// <param name="value">the group name</param>
        /// <returns></returns>
        ITweakButton Group(string value);

        /// <summary>
        /// Set the icon for this button.
        /// </summary>
        /// <param name="value">One of a few predefined names, or a SVG string.</param>
        /// <returns></returns>
        ITweakButton Icon(string value);

        /// <summary>
        /// Set one or more classes on the button.
        /// </summary>
        /// <param name="value">a string containing one or more CSS class names</param>
        /// <returns></returns>
        ITweakButton Classes(string value);

        /// <summary>
        /// Add a general ui-rule for things which are not in the standard API.
        /// </summary>
        /// <param name="value">a string such as `this=that` or an object which will be parsed/serialized such as `new { this = 27 }`</param>
        /// <returns></returns>
        ITweakButton Ui(object value);

        /// <summary>
        /// Add a general UI rule for things which are not in the standard API.
        /// </summary>
        /// <param name="name">The name/key which comes before the `=`</param>
        /// <param name="value">The value which comes after the `=`</param>
        /// <returns></returns>
        ITweakButton Ui(string name, object value);

        #endregion

        #region Params

        /// <summary>
        /// Add parameters to the button - which are usually used when executing the command.
        /// </summary>
        /// <param name="value">A string such as `name=value` or an anonymous object such as `new { name = 27 }`</param>
        /// <returns></returns>
        ITweakButton Parameters(object value);

        /// <summary>
        /// Add parameters to the button - which are usually used when executing the command.
        /// </summary>
        /// <param name="name">The name/key which comes before the `=`</param>
        /// <param name="value">The value which comes after the `=`</param>
        /// <returns></returns>
        ITweakButton Parameters(string name, object value);

        /// <summary>
        /// Add prefill information to the button, usually for creating new Entities.
        /// </summary>
        /// <param name="value">A string such as `name=value` or an anonymous object such as `new { name = 27 }`</param>
        /// <returns></returns>
        ITweakButton Prefill(object value);

        /// <summary>
        /// Add prefill information to the button, usually for creating new Entities.
        /// </summary>
        /// <param name="name">The name/key which comes before the `=`</param>
        /// <param name="value">The value which comes after the `=`</param>
        /// <returns></returns>
        ITweakButton Prefill(string name, object value);

        /// <summary>
        /// Add filter information to the button - usually when opening Data dialogs.
        /// </summary>
        /// <param name="value">A string such as `name=value` or an anonymous object such as `new { name = 27 }`</param>
        /// <returns></returns>
        ITweakButton Filter(object value);

        /// <summary>
        /// Add filter information to the button - usually when opening Data dialogs.
        /// </summary>
        /// <param name="name">The name/key which comes before the `=`</param>
        /// <param name="value">The value which comes after the `=`</param>
        /// <returns></returns>
        ITweakButton Filter(string name, object value);

        #endregion
    }
}
