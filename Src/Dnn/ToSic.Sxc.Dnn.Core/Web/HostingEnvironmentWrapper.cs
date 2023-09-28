﻿using ToSic.Lib.Documentation;

namespace ToSic.Sxc.Web
{
    /// <summary>
    /// wrapper around HostingEnvironment
    /// </summary>
    /// <remarks>to mock the wrapper in unit tests</remarks>
    [InternalApi_DoNotUse_MayChangeWithoutNotice()]
    public class HostingEnvironmentWrapper : IHostingEnvironmentWrapper
    {
        /// <summary>Maps a virtual path to a physical path on the server.</summary>
        /// <param name="virtualPath">The virtual path (absolute or relative).</param>
        /// <returns>The physical path on the server specified by <paramref name="virtualPath" />.</returns>
        public string MapPath(string virtualPath) => System.Web.Hosting.HostingEnvironment.MapPath(virtualPath);
    }
}
