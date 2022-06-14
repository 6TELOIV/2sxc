﻿using System;
using ToSic.Eav.Documentation;
using ToSic.Eav.Logging;
using ToSic.Sxc.Blocks;
using ToSic.Sxc.Code;
using ToSic.Sxc.Oqt.Server.Plumbing;
using ToSic.Sxc.Oqt.Shared;
using ToSic.Sxc.Services;

namespace ToSic.Sxc.Oqt.Server.Blocks
{
    [PrivateApi]
    public class OqtaneDynamicCodeRoot : DynamicCodeRoot<object, Kit>
    {
        private readonly Lazy<SiteStateInitializer> _siteStateInitializerLazy;
        public OqtaneDynamicCodeRoot(Dependencies dependencies, Lazy<SiteStateInitializer> siteStateInitializerLazy) : base(dependencies, OqtConstants.OqtLogPrefix)
        {
            _siteStateInitializerLazy = siteStateInitializerLazy;
        }

        public override IDynamicCodeRoot InitDynCodeRoot(IBlock block, ILog parentLog, int compatibility = Constants.CompatibilityLevel12)
        {
            _siteStateInitializerLazy.Value.InitIfEmpty(block?.Context?.Site?.Id);
            return base.InitDynCodeRoot(block, parentLog, compatibility);
        }
    }
}
