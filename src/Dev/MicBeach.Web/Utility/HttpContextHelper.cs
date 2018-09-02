using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Web.Utility
{
    public static class HttpContextHelper
    {
        /// <summary>
        /// current context
        /// </summary>
        public static HttpContext Current
        {
            get
            {
                object factory = ServiceProviderConfig.ServiceProvider.GetService(typeof(IHttpContextAccessor));
                HttpContext context = ((HttpContextAccessor)factory).HttpContext;
                return context;
            }
        }
    }
}
