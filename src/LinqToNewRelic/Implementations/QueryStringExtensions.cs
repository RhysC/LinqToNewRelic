﻿using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace LinqToNewRelic.Implementations
{
    static class QueryStringExtensions
    {
        public static string ToQueryString(this NameValueCollection source, bool removeEmptyEntries)
        {
            return source != null ? String.Join("&", source.AllKeys
                                                           .Where(key => !removeEmptyEntries || source.GetValues(key).Any(value => !String.IsNullOrEmpty(value)))
                                                           .SelectMany(key => source.GetValues(key)
                                                           .Where(value => !removeEmptyEntries || !String.IsNullOrEmpty(value))
                                                           .Select(value => String.Format("{0}={1}", HttpUtility.UrlEncode(key), value != null ? HttpUtility.UrlEncode(value) : string.Empty)))
                                                           .ToArray())
                       : string.Empty;
        }
    }
}