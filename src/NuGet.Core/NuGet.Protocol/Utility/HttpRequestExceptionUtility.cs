// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;

namespace NuGet.Protocol.Core.Types
{
    internal static class HttpRequestExceptionUtility
    {
        internal static HttpStatusCode? GetHttpStatusCode(HttpRequestException ex)
        {
            HttpStatusCode? statusCode = null;

#if NETCOREAPP5_0
            statusCode = ex.StatusCode;
#else
            // All places which might raise an HttpRequestException need to put StatusCode in exception object.
            if (ex.Data.Contains("StatusCode"))
            {
                statusCode = (HttpStatusCode)ex.Data["StatusCode"];
            }
#endif

            return statusCode;
        }

        internal static void ThrowFatalProtocolExceptionIfCritical(HttpRequestException ex, string url)
        {
            HttpStatusCode? statusCode = GetHttpStatusCode(ex);
            string message = null;

            // For these status codes, we throw exception.
            // Ideally, we add more status codes to this switch statement as we run into other codes that
            // will benifit with a better error experience.
            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    message = string.Format(CultureInfo.CurrentCulture, Strings.Http_CredentialsForUnauthorized, url);
                    break;
                case HttpStatusCode.Forbidden:
                    message = string.Format(CultureInfo.CurrentCulture, Strings.Http_CredentialsForForbidden, url);
                    break;
                case HttpStatusCode.NotFound:
                    message = string.Format(CultureInfo.CurrentCulture, Strings.Http_CredentialsForNotFound, url);
                    break;
                case HttpStatusCode.ProxyAuthenticationRequired:
                    message = string.Format(CultureInfo.CurrentCulture, Strings.Http_CredentialsForProxy, url);
                    break;
            }

            if (message != null)
            {
                throw new FatalProtocolException(message, ex, statusCode.Value);
            }
        }
    }
}
