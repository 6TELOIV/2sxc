﻿using ToSic.Eav.Metadata;
using ToSic.Eav.Plumbing;
using ToSic.Sxc.Data;
using ToSic.Sxc.Data.Internal;
using ToSic.Sxc.Images.Internal;

namespace ToSic.Sxc.Images;

/// <summary>
/// Helper class to handle all kinds of parameters passed to a responsive tag
/// </summary>
[PrivateApi]
internal class ResponsiveParams(ResponsiveParams.PreparedResponsiveParams prepared)
{
    /// <summary>
    /// The only reliable object which knows about the url - can never be null
    /// </summary>
    public IHasLink Link => prepared.HasLinkOrNull;

    /// <summary>
    /// The field used for this responsive output - can be null!
    /// </summary>
    public IField Field => prepared.FieldOrNull;

    public IHasMetadata HasMetadataOrNull => prepared.HasMdOrNull;

    public ImageDecorator ImageDecoratorOrNull => prepared.ImgDecoratorOrNull;

    public ImageDecorator InputImageDecoratorOrNull => prepared.InputImgDecoratorOrNull;

    public IResizeSettings Settings { get; init; }
    public string ImgAlt { get; init; }
    public string ImgAltFallback { get; init; }
    public string ImgClass { get; init; }
    public IDictionary<string, object> ImgAttributes { get; init; }
    public IDictionary<string, object> PictureAttributes { get; init; }

    public string PictureClass { get; init; }

    public object Toolbar { get; init; }

    internal static PreparedResponsiveParams Prepare(object target)
    {
        switch (target)
        {
            case null: return new(null, null, null, null, null);
            case PreparedResponsiveParams already: return already;
        }

        var field = target as IField ?? (target as IFromField)?.Field;
        var link = target as IHasLink ?? new HasLink(target as string);
        var mdProvider = target as IHasMetadata ?? field;
        var imgDecorator = field?.ImageDecoratorOrNull ?? ImageDecorator.GetOrNull(mdProvider, []);

        var inputImgDecorator = ImageDecorator.GetOrNull(field?.Parent.Type[field.Name], []);

        return new(field, mdProvider, imgDecorator, link, inputImgDecorator);
    }

    internal record PreparedResponsiveParams(
        IField FieldOrNull,
        IHasMetadata HasMdOrNull,
        ImageDecorator ImgDecoratorOrNull,
        IHasLink HasLinkOrNull,
        ImageDecorator InputImgDecoratorOrNull
    )
    {
        internal string ResizeSettingsOrNull => ImgDecoratorOrNull?.ResizeSettings?.NullIfNoValue()
                                               ?? InputImgDecoratorOrNull?.ResizeSettings?.NullIfNoValue();
    }
}