using MicBeach.Serialize.Json.JsonNet;
using MicBeach.Util.Drawing;
using MicBeach.Util.IoC;
using MicBeach.Util.Serialize;
using MicBeach.VerificationCode.SkiaSharp;
using MicBeach.Web.Mvc;
using MicBeach.Web.Utility;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.IoC
{
    public static class ContainerFactory
    {
        public static IServiceProvider GetServiceProvider(IServiceCollection services)
        {
            ServiceProviderConfig.RegisterServiceMethod = RegisterTypes;
            return ServiceProviderConfig.InitServiceProvider(services);
        }

        static void RegisterTypes()
        {
            var container = new ServiceProviderContainer();
            container.DefaultRegister();
            container.RegisterType(typeof(IJsonSerializer), typeof(JsonNetSerializer));
            container.RegisterType(typeof(VerificationCodeBase),typeof(SkiaSharpVerificationCode));
            ContainerManager.Container = container;
        }
    }
}
