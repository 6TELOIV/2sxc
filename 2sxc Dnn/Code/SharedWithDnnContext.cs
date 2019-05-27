﻿using ToSic.SexyContent.Razor.Helpers;
using ToSic.Sxc.Dnn.Interfaces;
using ToSic.Sxc.Interfaces;

// ReSharper disable once CheckNamespace
namespace ToSic.Sxc.Code
{
    public abstract class SharedWithDnnContext: SharedWithContext, IHasDnnContext
    {
        public DnnHelper Dnn { get; private set; }

        internal override void InitShared(IAppOutputGenerators parent)
        {
            if (parent is IHasDnnContext withDnn) Dnn = withDnn.Dnn;

            base.InitShared(parent);
        }
    }
}
