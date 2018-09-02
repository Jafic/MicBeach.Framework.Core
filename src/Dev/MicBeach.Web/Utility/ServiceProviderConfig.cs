using MicBeach.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Web.Utility
{
    public static class ServiceProviderConfig
    {
        /// <summary>
        /// 服务解析提供程序
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// 服务容器
        /// </summary>
        public static IServiceCollection Services { get; private set; }

        /// <summary>
        /// 自定义注册服务方法
        /// </summary>
        public static Action RegisterServiceMethod { get; set; }

        /// <summary>
        /// 初始化服务注册
        /// </summary>
        /// <param name="oldServices"></param>
        public static IServiceProvider InitServiceProvider(IServiceCollection oldServices = null)
        {
            IServiceCollection newServices = new ServiceCollection();
            if (oldServices != null)
            {
                ServiceProvider = oldServices.BuildServiceProvider();
                foreach (var service in oldServices)
                {
                    newServices.Add(service);
                }
            }
            Services = newServices;
            RegisterServiceMethod?.Invoke();
            return ServiceProvider = Services.BuildServiceProvider();
        }
    }
}
