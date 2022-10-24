using OAuthLibrary.Middleware;
using System;

namespace Microsoft.AspNetCore.Builder
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class OAuthEndpointExtensions
    {
        public static IApplicationBuilder UseOAuthEndpoint(this IApplicationBuilder builder, string path, string baseApi, String[] pathsWithoutAuthoeizaion)
        {
            return builder.UseMiddleware<OAuthMiddleware>(path, baseApi, pathsWithoutAuthoeizaion);
        }
    }
}

