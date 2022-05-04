﻿using System.Collections.Generic;
using ToSic.Eav;
using ToSic.Eav.Documentation;
using ToSic.Eav.Logging;
using ToSic.Sxc.Data;
using ToSic.Sxc.Images;

namespace ToSic.Sxc.Services
{
    /// <summary>
    /// Service to help create responsive `img` and `picture` tags the best possible way.
    /// </summary>
    /// <remarks>
    /// History: Released 2sxc 13.10
    /// </remarks>
    [PublicApi("Still WIP")]
    public interface IImageService: ICanDebug
    {
        /// <summary>
        /// Get the format information for a specific extension.
        /// Mostly used internally, you will usually not need this. 
        /// </summary>
        /// <param name="path">Path or extension</param>
        /// <returns></returns>
        /// <remarks>Only works for the basic, known image types</remarks>
        [PrivateApi("Not sure if this is needed outside...")]
        IImageFormat GetFormat(string path);


        // TODO - SETTINGS CAN'T BE AN IResizeSettings, because that will throw an error in the merge call
        // We should reconsider this and make it possible to also rebuild a resize-settings
        // - Or a specially prepared <see cref="ToSic.Sxc.Images.IResizeSettings"/> object containing all settings.


        /// <summary>
        /// Construct custom Resize-Settings as needed, either based on existing settings or starting from scratch
        /// </summary>
        /// <param name="settings">
        /// - A standardized Image-Settings object like `Settings.Images.Content` used as a template - see http://r.2sxc.org/settings
        /// - The `string` name of a template settings , like "Content" or "Screen"
        /// - a `bool` true/false - if true, the normal "Content" configuration is used as a template, if false, no initial configuration is used
        /// - Or a dynamic object containing settings properties (this can also be a merged custom + standard settings)
        /// - Or a specially prepared <see cref="ToSic.Sxc.Images.IResizeSettings"/> object containing all settings.
        /// </param>
        /// <param name="noParamOrder">see [](xref:NetCode.Conventions.NamedParameters)</param>
        /// <param name="factor">A multiplier, usually used to create urls which resize to a part of the default content-size. Eg. 0.5. </param>
        /// <param name="width">Optional width parameter. Cannot be used if `factor` is set. Usually takes the default from the `settings`.</param>
        /// <param name="height">Optional height parameter. Can only be 0 if `factor` is set, no not specify a height. Usually takes the default from the `settings`.</param>
        /// <param name="quality">Optional quality parameter. Usually takes the default from the `settings`.</param>
        /// <param name="resizeMode">Optional resize-mode, like `crop` or `max`. Usually takes the default from the `settings`.</param>
        /// <param name="scaleMode">Optional scale-mode to allow up-scaling images like `up` or `both`. Usually takes the default from the `settings`.</param>
        /// <param name="format">Optional file format like `jpg` or `png`</param>
        /// <param name="aspectRatio">Aspect Ratio width/height, only relevant if a `factor` is supplied. Usually takes default from the `settings` or is ignored. </param>
        /// <param name="parameters">
        ///     - the parameters either as `id=47&amp;name=daniel` (Dnn also supports `/id/47/name/daniel`)
        ///     - it can also be an <see cref="Context.IParameters"/>
        /// </param>
        /// <param name="recipe">WIP - not ready yet</param>
        /// <returns>A settings object which has all the parameters as configured</returns>
        /// <remarks>
        /// History: Added in 2sxc 13.03
        /// </remarks>
        IResizeSettings Settings(
            object settings = default,
            string noParamOrder = Parameters.Protector,
            object factor = default,
            object width = default,
            object height = default,
            object quality = default,
            string resizeMode = default,
            string scaleMode = default,
            string format = default,
            object aspectRatio = default,
            string parameters = default,
            object recipe = default
        );

        Recipe Recipe(string variants);

        Recipe Recipe(
            Recipe recipe,
            string noParamOrder = Parameters.Protector,
            string name = default,
            int width = default,
            string variants = default,
            Dictionary<string, object> attributes = default,
            IEnumerable<Recipe> recipes = default,
            bool? setWidth = default,
            bool? setHeight = default,
            string forTag = default,
            string forFactor = default,
            string forCss = default
        );

        // 2022-03-19 2dm - not ready yet
        ///// <summary>
        ///// Generate a `srcset` attribute for an image, containing various sizes as specified by the image itself
        ///// </summary>
        ///// <param name="url">The image url</param>
        ///// <param name="settings">
        /////     - A standardized Image-Settings object like Settings.Images.Content - see http://r.2sxc.org/settings
        /////     - Or a dynamic object containing settings properties (this can also be a merged custom + standard settings)
        /////     - Or a specially prepared <see cref="IResizeSettings"/> object containing all settings.
        /////     Note: If you need to construct very custom settings, use <see cref="ResizeSettings">ResizeSettings</see> to create them
        ///// </param>
        ///// <param name="noParamOrder">see [](xref:NetCode.Conventions.NamedParameters)</param>
        ///// <param name="factor">A multiplier, usually used to create urls which resize to a part of the default content-size. Eg. 0.5. </param>
        ///// <param name="recipe">Optional string to configure what `srcset`s to generate - see [](xref:NetCode.Images.SrcSet) (note it's `srcset`, not `srcSet`)</param>
        ///// <returns></returns>
        ///// <remarks>
        ///// History: Added in 2sxc 13.03
        ///// </remarks>
        //IHybridHtmlString SrcSet(
        //    string url,
        //    object settings = default,
        //    string noParamOrder = Eav.Parameters.Protector,
        //    object factor = default,
        //    object recipe = default
        //);

        /// <summary>
        /// Get a Responsive Picture object which you can then either just show, or use to construct a more customized output as you need it.
        /// 
        /// The resulting object can just be added to the html, like `@pic` or you can work with sub-properties as specified in the <see cref="IResponsivePicture"/>.
        ///
        /// **Important:** This call only allows you to set the most common parameters `factor` and `width`.
        /// For other parameters like `height`, `aspectRatio`, `quality` etc. create Settings <see cref="Settings"/> and pass them in.
        /// </summary>
        /// <param name="link">
        /// What should be in this, can be:
        /// 
        /// - a string url, in which case it would be used if `url` is not specified
        /// - a <see cref="IDynamicField"/> in which case it would be used if `field` is not specified
        /// </param>
        /// <param name="noParamOrder">see [](xref:NetCode.Conventions.NamedParameters)</param>
        /// <param name="settings">
        /// - A standardized Image-Settings object like Settings.Images.Content - see http://r.2sxc.org/settings
        /// - The name of a settings configuration, like "Content" or "Screen"
        /// - Or a dynamic object containing settings properties (this can also be a merged custom + standard settings)
        /// - Or a specially prepared <see cref="IResizeSettings"/> object containing all settings.
        /// 
        /// Note: If you need to construct very custom settings, use <see cref="Settings">ResizeSettings</see> to create them
        /// </param>
        /// <param name="factor">A multiplier, usually used to create urls which resize to a part of the default content-size. Eg. 0.5. </param>
        /// <param name="imgAlt">`alt` attribute on the created `img` tag for SEO etc.</param>
        /// <param name="imgClass">`class` attribute on the created `img` tag</param>
        /// <param name="recipe">
        /// Optional recipe = instructions how to create the various variants of this link.
        /// Can be any one of these:
        /// 
        /// - string containing variants
        /// - Rule object
        /// 
        /// TODO: DOCS not quite ready
        /// </param>
        /// <returns>A ResponsivePicture object which can be rendered directly. See [](xref:NetCode.Images.Index)</returns>
        /// <remarks>
        /// History: Added in 2sxc 13.03
        /// </remarks>
        IResponsivePicture Picture(
            object link = null,
            object settings = default,
            string noParamOrder = Parameters.Protector,
            object factor = default,
            object width = default,
            string imgAlt = default,
            string imgClass = default,
            object recipe = default
        );

        /// <summary>
        /// Get a Responsive Image object which you can then either just show, or use to construct a more customized output as you need it.
        /// 
        /// The resulting object can just be added to the html, like `@img` or you can work with sub-properties as specified in the <see cref="IResponsiveImage"/>
        /// </summary>
        /// <param name="link">
        ///     What should be in this, can be:
        /// 
        ///     - a string url, in which case it would be used if `url` is not specified
        ///     - a <see cref="IDynamicField"/> in which case it would be used if `field` is not specified
        /// </param>
        /// <param name="noParamOrder">see [](xref:NetCode.Conventions.NamedParameters)</param>
        /// <param name="settings">
        ///     - A standardized Image-Settings object like Settings.Images.Content - see http://r.2sxc.org/settings
        ///     - The name of a settings configuration, like "Content" or "Screen"
        ///     - Or a dynamic object containing settings properties (this can also be a merged custom + standard settings)
        ///     - Or a specially prepared <see cref="IResizeSettings"/> object containing all settings.
        /// 
        ///     Note: If you need to construct very custom settings, use <see cref="Settings">ResizeSettings</see> to create them
        /// </param>
        /// <param name="factor">A multiplier, usually used to create urls which resize to a part of the default content-size. Eg. 0.5. </param>
        /// <param name="imgAlt">`alt` attribute on the created `img` tag for SEO etc.</param>
        /// <param name="imgClass">`class` attribute on the created `img` tag</param>
        /// <param name="recipe">
        ///     Optional recipe = instructions how to create the various variants of this link.
        ///     Can be any one of these:
        /// 
        ///     - string containing variants
        ///     - Rule object
        /// 
        ///     TODO: DOCS not quite ready
        /// </param>
        /// <returns>A ResponsiveImage object which can be rendered directly. See [](xref:NetCode.Images.Index)</returns>
        /// <remarks>
        /// History: Added in 2sxc 13.03
        /// </remarks>
        IResponsiveImage Img(
            object link = null,
            object settings = default,
            string noParamOrder = Parameters.Protector,
            object factor = default,
            object width = default,
            string imgAlt = default,
            string imgClass = default,
            object recipe = default
        );

        /// <summary>
        /// Determine the best scenario (has multiple formats or not) and then return either an `img` (if it is not resizable and/or not multiple formats) or a `picture`
        /// 
        /// The resulting object can just be added to the html, like `@img` or you can work with sub-properties as specified in the <see cref="IResponsiveImage"/>
        /// </summary>
        /// <param name="link">
        ///     What should be in this, can be:
        /// 
        ///     - a string url, in which case it would be used if `url` is not specified
        ///     - a <see cref="IDynamicField"/> in which case it would be used if `field` is not specified
        /// </param>
        /// <param name="noParamOrder">see [](xref:NetCode.Conventions.NamedParameters)</param>
        /// <param name="settings">
        ///     - A standardized Image-Settings object like Settings.Images.Content - see http://r.2sxc.org/settings
        ///     - The name of a settings configuration, like "Content" or "Screen"
        ///     - Or a dynamic object containing settings properties (this can also be a merged custom + standard settings)
        ///     - Or a specially prepared <see cref="IResizeSettings"/> object containing all settings.
        /// 
        ///     Note: If you need to construct very custom settings, use <see cref="Settings">ResizeSettings</see> to create them
        /// </param>
        /// <param name="factor">A multiplier, usually used to create urls which resize to a part of the default content-size. Eg. 0.5. </param>
        /// <param name="imgAlt">`alt` attribute on the created `img` tag for SEO etc.</param>
        /// <param name="imgClass">`class` attribute on the created `img` tag</param>
        /// <param name="recipe">
        ///     Optional recipe = instructions how to create the various variants of this link.
        ///     Can be any one of these:
        /// 
        ///     - string containing variants
        ///     - Rule object
        /// 
        ///     TODO: DOCS not quite ready
        /// </param>
        /// <returns>A ResponsiveImage object which can be rendered directly. See [](xref:NetCode.Images.Index)</returns>
        /// <remarks>
        /// History: Added in 2sxc 13.03
        /// </remarks>
        IResponsiveImage ImgOrPic(
            object link = null,
            object settings = null,
            string noParamOrder = Parameters.Protector,
            object factor = null,
            object width = default,
            string imgAlt = null,
            string imgClass = null,
            object recipe = null);

    }
}
