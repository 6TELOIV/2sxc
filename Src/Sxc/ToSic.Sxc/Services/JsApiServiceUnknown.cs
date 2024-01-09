﻿using ToSic.Eav.Internal.Unknown;
using ToSic.Lib.Logging;
using ToSic.Lib.Services;
using ToSic.Sxc.Web.Internal.JsContext;

namespace ToSic.Sxc.Services;

[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
internal class JsApiServiceUnknown(WarnUseOfUnknown<JsApiServiceUnknown> _) : ServiceBase($"{LogScopes.NotImplemented}.JsApi"), IJsApiService, IIsUnknown
{
    public string GetJsApiJson(int? pageId, string siteRoot, string rvt) => null;
    public JsApi GetJsApi(int? pageId, string siteRoot, string rvt) => null;
}