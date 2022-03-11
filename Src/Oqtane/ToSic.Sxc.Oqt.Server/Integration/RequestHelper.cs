﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using ToSic.Sxc.Context;
using ToSic.Sxc.Oqt.Server.WebApi;

namespace ToSic.Sxc.Oqt.Server.Integration
{
    /// <summary>
    /// Helper to get header, query string and route information from current request.
    /// Used as input to build current context.
    /// </summary>
    public class RequestHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public T GetTypedHeader<T>(string headerName, T fallback)
        {
            var valueString = _httpContextAccessor.HttpContext?.Request.Headers[headerName] ?? StringValues.Empty;
            return ReturnTypedResultOrFallback(valueString, fallback);
        }

        public T GetQueryString<T>(string key, T fallback)
        {
            var valueString = _httpContextAccessor.HttpContext?.Request.Query[key] ?? StringValues.Empty;
            return ReturnTypedResultOrFallback(valueString, fallback);
        }

        public T GetRouteValuesString<T>(string key, T fallback)
        {
            // TODO: stv - this looks wrong, don't think valueString is of this type
            var valueString = $"{_httpContextAccessor.HttpContext?.Request.RouteValues[key]}";
            return ReturnTypedResultOrFallback(valueString, fallback);
        }

        // TODO: REVIEW IF we should call ObjectExtensions.ChangeTypeOrFallback(...) instead; functionality may be a tiny bit different
        private static T ReturnTypedResultOrFallback<T>(StringValues valueString, T fallback)
        {
            if (valueString == StringValues.Empty) return fallback;
            try
            {
                return (T)Convert.ChangeType(valueString.ToString(), typeof(T));
            }
            catch
            {
                return fallback;
            }
        }

        public int TryGetPageId() =>
            GetTypedHeader(ContextConstants.PageIdKey,
                GetQueryString(WebApiConstants.PageId,
                    GetRouteValuesString(WebApiConstants.PageId, Eav.Constants.NullId)));

        public int TryGetModuleId() =>
            GetTypedHeader(Sxc.WebApi.WebApiConstants.HeaderInstanceId,
                GetQueryString(WebApiConstants.ModuleId,
                    GetRouteValuesString(WebApiConstants.ModuleId, Eav.Constants.NullId)));
    }
}
