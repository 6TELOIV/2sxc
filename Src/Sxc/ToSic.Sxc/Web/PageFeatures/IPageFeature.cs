﻿using System.Collections.Generic;
using ToSic.Eav.Configuration;
using ToSic.Eav.Data;
using ToSic.Eav.Documentation;

namespace ToSic.Sxc.Web.PageFeatures
{
    [PrivateApi("Internal / not final - neither name, namespace or anything")]
    public interface IPageFeature : IHasIdentityNameId, IHasRequirements
    {
        /// <summary>
        /// Primary identifier to activate the feature
        /// </summary>
#pragma warning disable CS0108, CS0114
        string NameId { get; }
#pragma warning restore CS0108, CS0114

        /// <summary>
        /// Name of this feature. 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Nice description of the feature.
        /// </summary>
        string Description { get; }

        string Html { get; }

        /// <summary>
        /// List of other features required to run this feature.
        /// </summary>
        IEnumerable<string> Needs { get; }
    }
}