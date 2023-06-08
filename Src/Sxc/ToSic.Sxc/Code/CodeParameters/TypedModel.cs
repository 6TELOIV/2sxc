﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ToSic.Eav.Data;
using ToSic.Eav.Generics;
using ToSic.Eav.Plumbing;
using ToSic.Sxc.Adam;
using ToSic.Sxc.Data;
using static ToSic.Eav.Parameters;

namespace ToSic.Sxc.Code
{
    public class TypedModel : ITypedModel
    {
        private readonly IDynamicCodeRoot _codeRoot;
        private readonly string _razorFileName;
        private readonly IDictionary<string, object> _paramsDictionary;

        public TypedModel(IDictionary<string, object> paramsDictionary, IDynamicCodeRoot codeRoot, string razorFileName)
        {
            _codeRoot = codeRoot;
            _razorFileName = razorFileName;
            _paramsDictionary = paramsDictionary?.ToInvariant() ?? new Dictionary<string, object>();
        }

        #region Check if parameters were supplied

        public bool HasAll(params string[] names)
        {
            if (names == null || names.Length == 0) return true;
            return names.All(n => _paramsDictionary.ContainsKey(n));
        }

        public bool HasAny(params string[] names)
        {
            if (names == null || names.Length == 0) return true;
            return names.Any(n => _paramsDictionary.ContainsKey(n));
        }

        public string RequireAny(params string[] names)
        {
            if (HasAny(names)) return null;
            throw new ArgumentException(RequireMsg("one or more", "none", names));
        }
        public string RequireAll(params string[] names)
        {
            if (HasAll(names)) return null;
            throw new ArgumentException(RequireMsg("all", "not all", names));
        }

        private string RequireMsg(string requires, string but, string[] names) =>
            $"Partial Razor '{_razorFileName}' requires {requires} of the following parameters, but {but} were provided: " +
            string.Join(", ", (names ?? Array.Empty<string>()).Select(s => $"'{s}'"));

        #endregion

        #region Get and GetInternal

        public object Get(string name, string noParamOrder = Protector, bool? required = default) 
            => GetInternal(name, noParamOrder, required);

        public T Get<T>(string name, string noParamOrder = Protector, T fallback = default, bool? required = default) 
            => GetInternal(name, noParamOrder, fallback, required);

        private T GetInternal<T>(string name, string noParamOrder, T fallback, bool? required, [CallerMemberName] string cName = default)
        {
            // If we have a clear fallback, don't make it required
            if (fallback != null && !EqualityComparer<T>.Default.Equals(fallback, default)) required = false;

            var found = GetInternal(name, noParamOrder, required, cName: cName);
            
            if (found == null) return fallback;

            // Already matching type OR Interface (because ConvertOrFallback can't handle interface)
            if (found is T typed) return typed;

            return typeof(T).IsInterface ? fallback : found.ConvertOrFallback(fallback);
        }

        private object GetInternal(string name, string noParamOrder = Protector, bool? required = default, [CallerMemberName] string cName = default)
        {
            Protect(noParamOrder, "required, fallback", cName);

            if (_paramsDictionary.TryGetValue(name, out var result))
                return result;
            if (required == false)
                return null;

            var call = $"{nameof(TypedModel)}.{nameof(cName)}(\"{name}\")";
            throw new ArgumentException($@"Tried to get parameter with {call} but parameter '{name}' not provided. 
Either change the calling Html.Partial(...) or use {call.Replace(")", ", required: false)")} to make it optional.", nameof(name));
        }

        public (T typed, object untyped, bool ok) GetInternalForInterface<T>(string name, string noParamOrder, T fallback, bool? required = default,
            [CallerMemberName] string cName = default) where T : class
        {
            var maybe = GetInternal(name, noParamOrder, required, cName);
            if (maybe == null) return (fallback, null, true);
            if (maybe is T typed) return (typed, maybe, true);

            return (null, maybe, false);
        }


        #endregion

        public dynamic Dynamic(string name, string noParamOrder = Protector, object fallback = default, bool? required = default) 
            => GetInternal(name, noParamOrder, fallback, required);

        public string String(string name, string noParamOrder = Protector, string fallback = default, bool? required = default) 
            => GetInternal(name, noParamOrder, fallback, required);

        #region Numbers

        public int Int(string name, string noParamOrder = Protector, int fallback = default, bool? required = default) 
            => GetInternal(name, noParamOrder, fallback, required);

        public float Float(string name, string noParamOrder = Protector, float fallback = default, bool? required = default) 
            => GetInternal(name, noParamOrder, fallback, required);

        public double Double(string name, string noParamOrder = Protector, double fallback = default, bool? required = default) 
            => GetInternal(name, noParamOrder, fallback, required);

        public decimal Decimal(string name, string noParamOrder = Protector, decimal fallback = default, bool? required = default) 
            => GetInternal(name, noParamOrder, fallback, required);

        #endregion

        public Guid Guid(string name, string noParamOrder = Protector, Guid fallback = default, bool? required = default) 
            => GetInternal(name, noParamOrder, fallback, required);

        public bool Bool(string name, string noParamOrder = Protector, bool fallback = default, bool? required = default) 
            => GetInternal(name, noParamOrder, fallback, required);

        public DateTime DateTime(string name, string noParamOrder = Protector, DateTime fallback = default, bool? required = default) 
            => GetInternal(name, noParamOrder, fallback, required);

        public IEntity Entity(string name, string noParamOrder = Protector, IEntity fallback = default, bool? required = default)
        {
            var (typed, untyped, ok) = GetInternalForInterface(name, noParamOrder, fallback, required);
            // Try to convert, in case it's an IEntity or something; could also result in error
            return ok ? typed : _codeRoot.AsEntity(untyped);
        }

        #region Adam

        public IFile File(string name, string noParamOrder = Protector, IFile fallback = default, bool? required = default)
        {
            var (typed, untyped, ok) = GetInternalForInterface(name, noParamOrder, fallback, required);
            if (ok) return typed;

            // Flatten list if necessary
            return untyped is IEnumerable<IFile> list ? list.First() : fallback;
        }

        public IEnumerable<IFile> Files(string name, string noParamOrder = Protector, IEnumerable<IFile> fallback = default, bool? required = default)
        {
            var (typed, untyped, ok) = GetInternalForInterface(name, noParamOrder, fallback, required);
            if (ok) return typed;

            // Wrap into list if necessary
            return untyped is IFile item ? new List<IFile> { item } : fallback;
        }

        // todo: @2dm incomplete!
        public IFolder Folder(string name, string noParamOrder = Protector, IFolder fallback = null, bool? required = default) 
            => GetInternal(name, noParamOrder, fallback, required);

        public IEnumerable<IFolder> Folders(string name, string noParamOrder = Protector, IEnumerable<IFolder> fallback = null, bool? required = default)
        {
            var (typed, untyped, ok) = GetInternalForInterface(name, noParamOrder, fallback, required);
            if (ok) return typed;

            // Wrap into list if necessary
            return untyped is IFolder item ? new List<IFolder> { item } : fallback;
        }

        #endregion


        public ITypedItem Item(string name, string noParamOrder = Protector, ITypedItem fallback = default, bool? required = default)
        {
            var (typed, untyped, ok) = GetInternalForInterface(name, noParamOrder, fallback, required);
            // Try to convert, in case it's an IEntity or something; could also result in error
            return ok ? typed : _codeRoot.AsTyped(untyped);
        }

        public IEnumerable<ITypedItem> Items(string name, string noParamOrder = Protector, IEnumerable<ITypedItem> fallback = default, bool? required = default)
        {
            var (typed, untyped, ok) = GetInternalForInterface(name, noParamOrder, fallback, required);
            // Try to convert, in case it's an IEntity or something; could also result in error
            return ok ? typed : _codeRoot.AsTypedList(untyped);
        }
    }
}
