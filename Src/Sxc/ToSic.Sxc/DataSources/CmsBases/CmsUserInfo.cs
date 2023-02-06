﻿using System;
using System.Collections.Generic;
using ToSic.Eav.Data;
using ToSic.Eav.Data.Raw;
using ToSic.Lib.Data;
using ToSic.Lib.Documentation;
using Attributes = ToSic.Eav.Data.Attributes;

namespace ToSic.Sxc.DataSources
{
    /// <summary>
    /// Internal class to hold all the information about the user,
    /// until it's converted to an IEntity in the <see cref="Users"/> DataSource.
    ///
    /// For detailed documentation, check the docs of the underlying objects:
    ///
    /// * TODO:
    /// * TODO:
    /// Important: this is an internal object.
    /// We're just including in in the docs to better understand where the properties come from.
    /// We'll probably move it to another namespace some day.
    /// </summary>
    /// <remarks>
    /// Make sure the property names never change, as they are critical for the created Entity.
    /// </remarks>
    [InternalApi_DoNotUse_MayChangeWithoutNotice]
    public class CmsUserInfo: IRawEntity, IHasIdentityNameId // : IUser - not inheriting for the moment, to not include deprecated properties IsAdmin, IsSuperUser, IsDesigner...
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string NameId { get; set; }
        public List<int> RoleIds { get; set; }
        public bool IsSystemAdmin { get; set; }
        public bool IsSiteAdmin { get; set; }
        public bool IsContentAdmin { get; set; }
        //public bool IsDesigner { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Username { get; set; }
        public string Email { get; set; } // aka PreferredEmail
        public string Name { get; set; } // aka DisplayName

        /// <summary>
        /// Data but without Id, Guid, Created, Modified
        /// </summary>
        [PrivateApi]
        public Dictionary<string, object> RawProperties => new Dictionary<string, object>
        {
            { Attributes.TitleNiceName, Name },
            { nameof(Name), Name },
            { nameof(NameId), NameId },
            { nameof(IsSystemAdmin), IsSystemAdmin },
            { nameof(IsSiteAdmin), IsSiteAdmin },
            { nameof(IsContentAdmin), IsContentAdmin },
            { nameof(IsAnonymous), IsAnonymous },
            { nameof(Username), Username },
            { nameof(Email), Email },
            { nameof(RoleIds), RoleIds }
        };
    }
}