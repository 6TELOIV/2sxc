﻿using System.Collections.Generic;
using ToSic.Eav;
using ToSic.Eav.Data;
using ToSic.Lib.Documentation;
using ToSic.Lib.Helpers;
using ToSic.Sxc.Apps;
using ToSic.Sxc.Code;
using ToSic.Sxc.Code.DevTools;
using ToSic.Sxc.Context;
using ToSic.Sxc.Data;
using ToSic.Sxc.Services;
using static ToSic.Eav.Parameters;
using CodeInfoService = ToSic.Eav.Code.InfoSystem.CodeInfoService;
using Constants = ToSic.Sxc.Constants;

// ReSharper disable ConvertToNullCoalescingCompoundAssignment

// ReSharper disable once CheckNamespace
namespace Custom.Hybrid
{
    /// <summary>
    /// Base class for v16 Dynamic Code files.
    /// 
    /// Will provide the <see cref="ServiceKit14"/> on property `Kit`.
    /// This contains all the popular services used in v14/16, so that your code can be lighter. 
    /// </summary>
    [WorkInProgressApi("WIP 16.02")]
    public abstract class CodePro : DynamicCodeBase, IHasCodeLog, IDynamicCode16
    {

        #region Constructor / Setup

        /// <summary>
        /// Main constructor. May never have parameters, otherwise inheriting code will run into problems. 
        /// </summary>
        protected CodePro() : base("Sxc.Code14") { }

        // <inheritdoc />
        public new ICodeLog Log => SysHlp.CodeLog;

        /// <inheritdoc cref="IDynamicCode12.GetService{TService}" />
        public TService GetService<TService>() => _DynCodeRoot.GetService<TService>();

        private TypedCode16Helper CodeHelper => _codeHelper ?? (_codeHelper = CreateCodeHelper());
        private TypedCode16Helper _codeHelper;

        [PrivateApi] public override int CompatibilityLevel => Constants.CompatibilityLevel16;

        #endregion

        public ServiceKit14 Kit => _kit.Get(() => _DynCodeRoot.GetKit<ServiceKit14>());
        private readonly GetOnce<ServiceKit14> _kit = new GetOnce<ServiceKit14>();

        #region Stuff added by Code12

        [PrivateApi("Not yet ready")]
        public IDevTools DevTools => _DynCodeRoot.DevTools;

        #endregion


        #region Link and Edit
        /// <inheritdoc cref="IDynamicCode.Link" />
        public ILinkService Link => _DynCodeRoot?.Link;

        #endregion


        #region SharedCode - must also map previous path to use here

        /// <inheritdoc />
        [PrivateApi]
        public string CreateInstancePath { get; set; }

        /// <inheritdoc cref="IDynamicCode.CreateInstance" />
        public dynamic CreateInstance(string virtualPath, string noParamOrder = Protector, string name = null, string relativePath = null, bool throwOnError = true) =>
            SysHlp.CreateInstance(virtualPath, noParamOrder, name, relativePath, throwOnError);

        #endregion


        private CodeInfoService CcS => _ccs.Get(GetService<CodeInfoService>);
        private readonly GetOnce<CodeInfoService> _ccs = new GetOnce<CodeInfoService>();


        #region New App, Settings, Resources

        /// <inheritdoc />
        public IAppTyped App => (IAppTyped)_DynCodeRoot?.App;

        /// <inheritdoc cref="IDynamicCode16.AllResources" />
        public ITypedStack AllResources => _DynCodeRoot.Resources;

        /// <inheritdoc cref="IDynamicCode16.AllSettings" />
        public ITypedStack AllSettings => _DynCodeRoot.Settings;


        public IMyData MyData => _DynCodeRoot.Data as IMyData;

        #endregion

        #region My... Stuff


        private TypedCode16Helper CreateCodeHelper()
        {
            return new TypedCode16Helper(_DynCodeRoot, MyData, null, false, "c# code file");
        }

        public ITypedItem MyItem => CodeHelper.MyItem;

        public IEnumerable<ITypedItem> MyItems => CodeHelper.MyItems;

        public ITypedItem MyHeader => CodeHelper.MyHeader;

        #endregion


        #region As Conversions

        /// <inheritdoc cref="IDynamicCode16.AsItem" />
        public ITypedItem AsItem(object target, string noParamOrder = Parameters.Protector)
            => _DynCodeRoot.AsC.AsItem(target);

        /// <inheritdoc cref="IDynamicCode16.AsItems" />
        public IEnumerable<ITypedItem> AsItems(object list, string noParamOrder = Parameters.Protector)
            => _DynCodeRoot.AsC.AsItems(list);

        /// <inheritdoc cref="IDynamicCode16.AsEntity" />
        public IEntity AsEntity(ICanBeEntity thing) => _DynCodeRoot.AsC.AsEntity(thing);

        /// <inheritdoc cref="IDynamicCode16.AsTyped" />
        public ITyped AsTyped(object original) => _DynCodeRoot.AsC.AsTypedPure(original);

        /// <inheritdoc cref="IDynamicCode16.AsTypedList" />
        public IEnumerable<ITyped> AsTypedList(object list) => _DynCodeRoot.AsC.AsTypedListPure(list);

        /// <inheritdoc cref="IDynamicCode16.AsStack" />
        public ITypedStack AsStack(params object[] items) => _DynCodeRoot.AsC.AsStack(items);

        #endregion

        public ITypedModel MyModel => CodeHelper.MyModel;

        /// <inheritdoc />
        public ICmsContext MyContext => _DynCodeRoot.CmsContext;

        /// <inheritdoc />
        public ICmsUser MyUser => _DynCodeRoot.CmsContext.User;

        /// <inheritdoc />
        public ICmsPage MyPage => _DynCodeRoot.CmsContext.Page;

        /// <inheritdoc />
        public ICmsView MyView => _DynCodeRoot.CmsContext.View;
    }
}
