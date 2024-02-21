﻿using ToSic.Sxc.Services;

namespace ToSic.Sxc.Data.Internal;

partial class CodeDataFactory
{
    /// <summary>
    /// EXPERIMENTAL
    /// </summary>
    public T AsCustom<T>(ICanBeEntity source, ServiceKit16 kit, NoParamOrder protector, bool nullIfNull)
        where T : class, ITypedItemWrapper16, ITypedItem, new()
    {
        if (nullIfNull && source == null) return null;
        if (source is T alreadyT) return alreadyT;

        var item = source as ITypedItem ?? AsItem(source);
        var wrapper = new T();
        wrapper.Setup(item, kit);
        return wrapper;
    }

    /// <summary>
    /// EXPERIMENTAL
    /// </summary>
    [PrivateApi("WIP, don't publish yet")]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public IEnumerable<T> AsCustomList<T>(IEnumerable<ICanBeEntity> source, ServiceKit16 kit, NoParamOrder protector, bool nullIfNull)
        where T : class, ITypedItemWrapper16, ITypedItem, new()
    {
        if (nullIfNull && source == null) return null;
        if (source is IEnumerable<T> alreadyListT) return alreadyListT;

        var items = SafeItems().Select(i =>
        {
            var wrapper = new T();
            wrapper.Setup(i, kit);
            return wrapper;
        });
        return items;

        IEnumerable<ITypedItem> SafeItems()
        {
            if (source == null || !source.Any()) return [];
            if (source is IEnumerable<ITypedItem> alreadyOk) return alreadyOk;
            return _CodeApiSvc._Cdf.AsItems(source);
        }
    }

}