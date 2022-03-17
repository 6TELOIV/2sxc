﻿using ToSic.Eav;
using ToSic.Eav.Logging;
using ToSic.Sxc.Code;
using ToSic.Sxc.Services;
using ToSic.Sxc.Web;

namespace ToSic.Sxc.Images
{
    public partial class ImageService: HasLog<ImageService>, IImageService, INeedsDynamicCodeRoot
    {
        #region Constructor and Inits

        public ImageService(ImgResizeLinker imgLinker, IFeaturesService features) : base(Constants.SxcLogName + ".ImgSvc")
        {
            _features = features;
            ImgLinker = imgLinker.Init(Log);
        }

        internal ImgResizeLinker ImgLinker { get; }
        private readonly IFeaturesService _features;

        public void ConnectToRoot(IDynamicCodeRoot codeRoot) => _codeRootOrNull = codeRoot;
        private IDynamicCodeRoot _codeRootOrNull;

        public ImageServiceSettings Settings { get; } = new ImageServiceSettings();

        #endregion

        #region Settings Handling

        /// <summary>
        /// Use the given settings or try to use the default content-settings if available
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private object GetBestSettings(object settings)
        {
            return settings == null || settings is bool boolSettings && boolSettings
                ? _codeRootOrNull?.Settings?.Images?.Content
                : settings;
        }

        #endregion



        public IResponsivePicture Picture(
            string url, 
            string noParamOrder = Parameters.Protector,
            object settings = default,
            object factor = default,
            string imgAlt = default,
            string imgClass = default,
            object rules = default
        ) => new ResponsivePicture(this, _features, url, GetBestSettings(settings), factor: factor, rules: rules, imgAlt: imgAlt, imgClass: imgClass);

        public IResponsiveImage Img(
            string url,
            string noParamOrder = Parameters.Protector,
            object settings = default,
            object factor = default,
            string imgAlt = default,
            string imgClass = default,
            object rules = default
        ) => new ResponsiveImage(this, url, GetBestSettings(settings), factor: factor, rules: rules, imgAlt: imgAlt, imgClass: imgClass);

        public IHybridHtmlString SrcSet(
            string url, 
            object settings = null,
            string noParamOrder = Parameters.Protector,
            object factor = null, 
            string srcset = null
        ) => new HybridHtmlString(ImgLinker.SrcSet(url, MergeSettings(settings, factor: factor, srcset: srcset), SrcSetType.ImgSrcSet));

        private ResizeSettings MergeSettings(
            object settings = null,
            string noParamOrder = Parameters.Protector,
            object factor = null, 
            string srcset = null
        ) => ImgLinker.ResizeParamMerger.BuildResizeSettings((GetBestSettings(settings), factor: factor, srcset: srcset as object ?? true));
    }
}
