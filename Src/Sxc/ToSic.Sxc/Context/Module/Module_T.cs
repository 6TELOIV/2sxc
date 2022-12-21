﻿using ToSic.Eav.Apps.Run;
using ToSic.Eav.Data;
using ToSic.Lib.Documentation;
using ToSic.Lib.Logging;
using ToSic.Lib.Services;

namespace ToSic.Sxc.Context
{
    /// <summary>
    /// A base implementation of the block information wrapping the CMS specific object along with it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [PrivateApi("this is just fyi")]
    public abstract class Module<T>: ServiceBase, IModule, IWrapper<T> where T: class
    {
        #region Constructors and DI

        /// <inheritdoc />
        public T UnwrappedContents => _contents;

        public T GetContents() => _contents;
        [PrivateApi] private T _contents;

        protected Module(string logName) : base(logName) { }

        public IModule Init(T item, ILog parentLog)
        {
            this.Init(parentLog);
            _contents = item;
            return this;
        }

        public abstract IModule Init(int id, ILog parentLog);
        #endregion

        /// <inheritdoc />
        public abstract int Id { get; }

        /// <inheritdoc />
        public abstract bool IsContent { get; }

        /// <inheritdoc />
        public abstract IBlockIdentifier BlockIdentifier { get; }
    }
}
