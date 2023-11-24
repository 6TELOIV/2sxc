﻿using System.Collections.Generic;
using ToSic.Eav.Internal.Unknown;
using ToSic.Lib.Logging;

namespace ToSic.Sxc.DataSources
{
    internal class PagesDataSourceProviderUnknown: PagesDataSourceProvider
    {
        public PagesDataSourceProviderUnknown(WarnUseOfUnknown<PagesDataSourceProviderUnknown> _): base($"{Constants.SxcLogName}.{LogConstants.NameUnknown}")
        { }

        public override List<PageDataRaw> GetPagesInternal(string noParamOrder = Eav.Parameters.Protector,
            bool includeHidden = default, bool includeDeleted = default, bool includeAdmin = default,
            bool includeSystem = default, bool includeLinks = default, bool requireViewPermissions = true,
            bool requireEditPermissions = true) => new();
    }
}
