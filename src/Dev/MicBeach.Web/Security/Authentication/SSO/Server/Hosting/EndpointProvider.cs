﻿using MicBeach.Web.Security.Authentication.SSO.Server.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Web.Security.Authentication.SSO.Server.Hosting
{
    internal static class EndpointProvider
    {
        internal static Dictionary<string, Endpoint> _endpoints = new Dictionary<string, Endpoint>();

        public static void AddDefaultsEndpoints(this IServiceCollection services)
        {
            services.AddTransient<CredentialVerifyEndpoint>();
            _endpoints.Add(Constants.RoutePaths.CredentialVerify, new Endpoint(Constants.EndpointNames.CredentialVerify, Constants.RoutePaths.CredentialVerify, typeof(CredentialVerifyEndpoint)));
        }

        public static ISSOAuthenticationEndpointHandler FindEndpoint(HttpContext context)
        {
            var path = context.Request.Path.Value.Trim('/');
            if (_endpoints.ContainsKey(path))
            {
                var endpoint = _endpoints[path];
                var handler = context.RequestServices.GetService(endpoint.Handler) as ISSOAuthenticationEndpointHandler;
                return handler;
            }
            return null;
        }
    }
}
