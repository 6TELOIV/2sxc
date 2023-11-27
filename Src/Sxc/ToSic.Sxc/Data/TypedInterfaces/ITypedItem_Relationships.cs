﻿using System.Collections.Generic;
using ToSic.Lib.Coding;

namespace ToSic.Sxc.Data;

public partial interface ITypedItem
{
    #region parents / children

    /// <inheritdoc cref="ITypedRelationships.Child"/>
    ITypedItem Child(string name, NoParamOrder noParamOrder = default, bool? required = default);

    /// <inheritdoc cref="ITypedRelationships.Children"/>
    IEnumerable<ITypedItem> Children(string field = default, NoParamOrder noParamOrder = default, string type = default, bool? required = default);

    /// <inheritdoc cref="ITypedRelationships.Parent"/>
    ITypedItem Parent(NoParamOrder noParamOrder = default, bool? current = default, string type = default, string field = default);

    /// <inheritdoc cref="ITypedRelationships.Parents"/>
    IEnumerable<ITypedItem> Parents(NoParamOrder noParamOrder = default, string type = default, string field = default);

    #endregion 
}