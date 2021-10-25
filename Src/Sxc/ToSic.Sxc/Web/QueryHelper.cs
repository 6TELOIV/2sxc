﻿using System.Collections.Specialized;
using ToSic.Sxc.Web.Url;

namespace ToSic.Sxc.Web
{
    public class QueryHelper
    {
        public static string AddQueryString(string url, NameValueCollection newParams) // List<KeyValuePair<string, string>> newParams)
        {
            // check do we have any work to do
            if (newParams == null || newParams.Count == 0) return url;

            // Make sure we don't run into null-errors below
            url = url ?? string.Empty;

            // Problem ATM:
            // - if the url is something like "test.jpg?w=200" then running this function gets you something like test.jpg?w=200?w=1600
            // We should try to combine existing params - so if the url already has a "?..." we should merge these
            // Basically the logic we need is
            // - if the url already has some params we should take that and split it into it's pieces
            // ...ideally using some .net url processing API and not invent our own
            // Then we should check if our new params (the `queryString` dictionary would replace any of the params
            // ...if yes, they should be removed
            // then everything should be re-assembled to work

            // todo 
            // 1. first take away fragment
            var parts = new UrlParts(url);


            // 2. From what's left, check if we have a ?
            // 3. process that
            // don't use URI object

            // prepare temp absolute uri because it is required for ParseQueryString
            //var tempAbsoluteUri = GetTempAbsoluteUri(url);

            // if the url already has some params we should take that and split it into it's pieces
            var queryParams = UrlHelpers.ParseQueryString(parts.Query);
            // var queryString2 = HttpUtility.ParseQueryString(parts.Query);


            // new params would replace existing queryString params or append new param to queryString
            //newParams.ForEach(param => queryParams.Set(param.Key, param.Value));
            var finalParams = queryParams.Merge(newParams);

            // combine new query string in url
            return GetUrlWithUpdatedQueryString(parts, /* url, /*tempAbsoluteUri,*/ finalParams);
        }

        //private static UrlParts SplitUrlIntoParts(string url)
        //{
        //    var fragmentStart = url.IndexOf('#');
        //    var fragment = string.Empty;
        //    var urlWithoutFragment = url;
        //    if (fragmentStart >= 0)
        //    {
        //        fragment = url.Substring(fragmentStart + 1);
        //        urlWithoutFragment = url.Substring(0, fragmentStart);
        //    }

        //    var queryStart = url.IndexOf('?');
        //    var query = string.Empty;
        //    var urlWithoutAnything = urlWithoutFragment;
        //    if (queryStart >= 0)
        //    {
        //        query = urlWithoutFragment.Substring(queryStart + 1);
        //        urlWithoutAnything = urlWithoutFragment.Substring(0, queryStart);
        //    }

        //    return new UrlParts
        //    {
        //        Url = url,
        //        Query = query,
        //        Fragment = fragment,
        //        Path = urlWithoutAnything
        //    };
        //}

        //private static Uri GetTempAbsoluteUri(string url)
        //{
        //    var isAbsoluteWithoutProtocol = url.StartsWith(@"//");
        //    var isAbsoluteUrl = url.StartsWith(@"http") || isAbsoluteWithoutProtocol;

        //    // special handling for relative urls, because of ParseQueryString limitation (it is working only with absolute uris)
        //    var absoluteUri = isAbsoluteUrl
        //        ? new Uri(isAbsoluteWithoutProtocol ? "http:" + url : url, UriKind.Absolute)
        //        : new Uri(new Uri("http://unknown/", UriKind.Absolute), url); // generate temp/dummy absolute uri, just for use with ParseQueryString

        //    return absoluteUri;
        //}

        private static string GetUrlWithUpdatedQueryString(UrlParts parts, /* string url, /*Uri tempUri,*/ NameValueCollection queryString)
        {
            var newUrl = parts.ToLink(suffix: false);
            if (queryString.Count > 0)
                newUrl += UrlParts.QuerySeparator + UrlHelpers.NvcToString(queryString);

            if (!string.IsNullOrWhiteSpace(parts.Fragment))
                newUrl += UrlParts.FragmentSeparator + parts.Fragment;

            return newUrl;

            // check for old query string to replace
            //if (string.IsNullOrEmpty(parts.Query))
            //    // check for #fragment to handle
            //    if (string.IsNullOrEmpty(parts.Fragment))
            //        url += newQueryString;
            //    else // #fragment should be on the end or url
            //        url = url.Replace(parts.Fragment, newQueryString + parts.Fragment);
            //else // replace old query string with new one
            //    url = url.Replace(parts.Query, newQueryString);

            //return url;
        }


        // 2021-10-08 2dm - disabled, as it's not in use
        //public static string Combine(string url, string parameters)
        //{
        //    if (string.IsNullOrEmpty(parameters)) return url;

        //    var urlParts = new UrlParts(url);

        //    if (string.IsNullOrEmpty(urlParts.Query))
        //    {
        //        urlParts.Query = parameters;
        //    }
        //    else
        //    {
        //        // combine query strings
        //        var queryString = UrlHelpers.ParseQueryString(parameters);
        //        var currentRequestQueryString = UrlHelpers.ParseQueryString(urlParts.Query);
        //        foreach (var key in queryString.AllKeys)
        //        {
        //            currentRequestQueryString.Set(key, queryString.Get(key));
        //        }

        //        urlParts.Query = UrlHelpers.NvcToString(currentRequestQueryString); //.ToString();
        //    }

        //    return urlParts.BuildUrl();
        //}
    }
}
