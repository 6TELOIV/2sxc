﻿using System;
using System.Collections.Generic;
using System.IO;
using ToSic.Eav.Data;
using ToSic.Eav.DataSource;
using ToSic.Eav.LookUp;
using ToSic.Lib.Documentation;
using ToSic.Sxc.Code;
using ToSic.Sxc.Code.DevTools;
using ToSic.Sxc.Context;
using ToSic.Sxc.Data;
using ToSic.Sxc.Services;
using DynamicJacket = ToSic.Sxc.Data.DynamicJacket;
using IApp = ToSic.Sxc.Apps.IApp;
using IEntity = ToSic.Eav.Data.IEntity;
using IFolder = ToSic.Sxc.Adam.IFolder;

// ReSharper disable once CheckNamespace
namespace Custom.Hybrid
{

    public abstract partial class Api12
    {
        [PrivateApi] public int CompatibilityLevel => _DynCodeRoot.CompatibilityLevel;

        /// <inheritdoc />
        public IApp App => _DynCodeRoot.App;

        /// <inheritdoc />
        public IContextData Data => _DynCodeRoot.Data;

        #region File

        

        #endregion



        #region AsDynamic implementations
        /// <inheritdoc/>
        public dynamic AsDynamic(string json, string fallback = default) => _DynCodeRoot.AsC.AsDynamicFromJson(json, fallback);

        /// <inheritdoc />
        public dynamic AsDynamic(IEntity entity) => _DynCodeRoot.AsC.AsDynamic(entity);

        /// <inheritdoc />
        public dynamic AsDynamic(object dynamicEntity) =>  _DynCodeRoot.AsDynamic(dynamicEntity);

        /// <inheritdoc />
        [PublicApi("Careful - still Experimental in 12.02")]
        public dynamic AsDynamic(params object[] entities) => _DynCodeRoot.AsC.MergeDynamic(entities);

        /// <inheritdoc />
        public IEntity AsEntity(object dynamicEntity) =>  _DynCodeRoot.AsC.AsEntity(dynamicEntity);

        #endregion

        #region AsList

        /// <inheritdoc />
        public IEnumerable<dynamic> AsList(object list) => _DynCodeRoot?.AsC.AsDynamicList(list);

        #endregion

        #region Convert-Service

        /// <inheritdoc />
        public IConvertService Convert => _DynCodeRoot.Convert;

        #endregion



        #region CreateSource implementations

        /// <inheritdoc />
        public T CreateSource<T>(IDataSource inSource = null, ILookUpEngine configurationProvider = default) where T : IDataSource
            => _DynCodeRoot.CreateSource<T>(inSource, configurationProvider);

        /// <inheritdoc />
	    public T CreateSource<T>(IDataStream source) where T : IDataSource 
            => _DynCodeRoot.CreateSource<T>(source);

        #endregion

        #region Content, Presentation & List

        /// <inheritdoc />
        public dynamic Content => _DynCodeRoot.Content;

        /// <inheritdoc />
        public dynamic Header => _DynCodeRoot.Header;


        #endregion


        #region Adam

        /// <inheritdoc />
        public IFolder AsAdam(ICanBeEntity item, string fieldName) => _DynCodeRoot.AsAdam(item, fieldName);

        /// <inheritdoc />
        public new ToSic.Sxc.Adam.IFile SaveInAdam(string noParamOrder = ToSic.Eav.Parameters.Protector,
            Stream stream = null,
            string fileName = null,
            string contentType = null,
            Guid? guid = null,
            string field = null,
            string subFolder = "")
            => base.SaveInAdam(noParamOrder, stream, fileName, contentType, guid, field, subFolder);

        #endregion

        #region Link & Edit - added to API in 2sxc 10.01

        /// <inheritdoc />
        public ILinkService Link => _DynCodeRoot?.Link;

        /// <inheritdoc />
        public IEditService Edit => _DynCodeRoot?.Edit;

        #endregion

        #region v12 Properties like CmsContext, Resources, Settings

        /// <inheritdoc />
        public ICmsContext CmsContext => _DynCodeRoot?.CmsContext;

        /// <inheritdoc />
        public dynamic Resources => _DynCodeRoot.Resources;

        /// <inheritdoc />
        public dynamic Settings => _DynCodeRoot.Settings;

        [PrivateApi("Not yet ready")]
        public IDevTools DevTools => _DynCodeRoot.DevTools;

        #endregion
    }
}
