﻿using ToSic.Eav;
using ToSic.Eav.Logging;
using ToSic.Sxc.Code;
using ToSic.Sxc.Services;
using ToSic.Sxc.Web;

namespace ToSic.Sxc.Images
{
    public partial class ImageService: HasLog<ImageService>, IImageService, INeedsCodeRoot
    {
        #region Constructor and Inits

        public ImageService(ImgResizeLinker imgLinker) : base(Constants.SxcLogName + ".ImgSvc") => ImgLinker = imgLinker.Init(Log);
        internal ImgResizeLinker ImgLinker { get; }

        public void AddBlockContext(IDynamicCodeRoot codeRoot) => _codeRootOrNull = codeRoot;
        private IDynamicCodeRoot _codeRootOrNull;

        #endregion

        #region Settings Handling

        /// <summary>
        /// Use the given settings or try to use the default content-settings if available
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private object GetBestSettings(object settings) => settings ?? _codeRootOrNull?.Settings?.Images?.Content;

        #endregion



        public IResponsivePicture Picture(
            string url, 
            string noParamOrder = Parameters.Protector,
            object settings = null,
            object factor = null, 
            string srcSet = null, 
            string imgAlt = null, 
            string imgClass = null
        ) => new ResponsivePicture(this, url, GetBestSettings(settings), factor: factor, srcSet: srcSet);

        public IResponsiveImg Img(
            string url,
            string noParamOrder = Parameters.Protector,
            object settings = null,
            object factor = null,
            string srcSet = null,
            string imgAlt = null,
            string imgClass = null
        ) => new ResponsiveImg(this, url, GetBestSettings(settings), factor: factor, srcSet: srcSet, imgAlt: imgAlt, imgClass: imgClass);

        public IHybridHtmlString SrcSet(
            string url, 
            object settings = null,
            string noParamOrder = Parameters.Protector,
            object factor = null, 
            string srcSet = null
        ) => new HybridHtmlString(ImgLinker.Image(url, GetBestSettings(settings), factor: factor, srcSet: (srcSet as object) ?? true));

    }
}
