﻿using System;

namespace ToSic.Sxc.Data
{
    public partial class DynamicEntity
    {
        /// <inheritdoc />
        public int EntityId => Entity?.EntityId ?? 0;


        /// <inheritdoc />
        public Guid EntityGuid => Entity?.EntityGuid ?? Guid.Empty;


        /// <inheritdoc />
        public IDynamicField Field(string name) => new DynamicField(this, name);

        /// <inheritdoc />
        public string EntityType => Entity?.Type?.Name;

    }
}
