﻿using System.Collections.Generic;
using ToSic.Lib.Documentation;
using ToSic.Lib.Helpers;
using ToSic.Sxc.Data;
using ToSic.Sxc.DataSources;
using static ToSic.Eav.Parameters;
using static ToSic.Eav.Configuration.ConfigurationConstants;

namespace ToSic.Sxc.Code
{
    public class TypedCode16Helper
    {
        public bool DefaultStrict = true;

        private readonly IDynamicCodeRoot _codeRoot;
        private readonly IDictionary<string, object> _myModelData;
        private readonly bool _isRazor;
        private readonly string _codeFileName;
        internal ContextData Data { get; }
        public TypedCode16Helper(IDynamicCodeRoot codeRoot, IContextData data, IDictionary<string, object> myModelData, bool isRazor, string codeFileName)
        {
            _codeRoot = codeRoot;
            _myModelData = myModelData;
            _isRazor = isRazor;
            _codeFileName = codeFileName;
            Data = data as ContextData;
        }

        public ITypedItem MyItem => _myItem.Get(() => _codeRoot.Cdf.AsItem(Data.MyItem, Protector, strict: DefaultStrict));
        private readonly GetOnce<ITypedItem> _myItem = new GetOnce<ITypedItem>();

        public IEnumerable<ITypedItem> MyItems => _myItems.Get(() => _codeRoot.Cdf.AsItems(Data.MyItem, Protector, strict: DefaultStrict));
        private readonly GetOnce<IEnumerable<ITypedItem>> _myItems = new GetOnce<IEnumerable<ITypedItem>>();

        public ITypedItem MyHeader => _myHeader.Get(() => _codeRoot.Cdf.AsItem(Data.MyHeader, Protector, strict: DefaultStrict));
        private readonly GetOnce<ITypedItem> _myHeader = new GetOnce<ITypedItem>();

        public ITypedModel MyModel => _myModel.Get(() => new TypedModel(_myModelData, _codeRoot, _isRazor, _codeFileName));
        private readonly GetOnce<ITypedModel> _myModel = new GetOnce<ITypedModel>();


        [PrivateApi] public ITypedStack AllResources => (_codeRoot as DynamicCodeRoot)?.AllResources;
        //_allRes.Get(() 
        //    => _codeRoot.Cdf.AsTypedStack(RootNameResources, (_codeRoot as DynamicCodeRoot)?.ResSrc));
        //private readonly GetOnce<ITypedStack> _allRes = new GetOnce<ITypedStack>();

        [PrivateApi]
        public ITypedStack AllSettings => (_codeRoot as DynamicCodeRoot)?.AllSettings;
        //    _allSettings.Get(() 
        //    => _codeRoot.Cdf.AsTypedStack(RootNameSettings, (_codeRoot as DynamicCodeRoot)?.SetSrc));
        //private readonly GetOnce<ITypedStack> _allSettings = new GetOnce<ITypedStack>();

    }
}
