using MicBeach.Util.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using MicBeach.Web.Config;
using Microsoft.Extensions.Configuration;

namespace MicBeach.Web.Utility
{
    public class ServiceProviderContainer : IDIContainer
    {

        public bool IsRegister<T>()
        {
            return ServiceProviderConfig.Services.Contains(new ServiceDescriptor(typeof(T), default(T)));
        }

        public void RegisterType(Type fromType, Type toType)
        {
            ServiceProviderConfig.Services.AddSingleton(fromType, toType);
        }

        public void RegisterType(Type fromType, Type toType, IEnumerable<Type> behaviors)
        {
            ServiceProviderConfig.Services.AddSingleton(fromType, toType);
        }

        public T Resolve<T>()
        {
            return ServiceProviderConfig.ServiceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// 默认注册
        /// </summary>
        public void DefaultRegister()
        {
            try
            {
                RegisterProjectReference();//项目IoC解析
                ConfigRegister();//配置
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 配置信息注册
        /// </summary>
        void ConfigRegister()
        {
            var configuration = Resolve<IConfiguration>();
            //上传配置
            ServiceProviderConfig.Services.Configure<UploadConfig>(configuration.GetSection("UploadConfig"));
            //远程上传配置
            ServiceProviderConfig.Services.Configure<RemoteUploadConfig>(configuration.GetSection("RemoteUploadConfig"));
            //远程文件访问配置
            ServiceProviderConfig.Services.Configure<RemoteFilePathConfig>(configuration.GetSection("RemoteFilePathConfig"));
        }

        /// <summary>
        /// 项目默认接口实现注册
        /// </summary>
        void RegisterProjectReference()
        {
            string appPath = Directory.GetCurrentDirectory();
            IHostingEnvironment env = Resolve<IHostingEnvironment>();
            if (env == null)
            {
                return;
            }
            if (env.IsDevelopment())
            {
                appPath = Path.Combine(appPath, "bin");
                DebugCombineContentPath(ref appPath);
                ReleaseCombineContentPath(ref appPath);

#if NETCOREAPP2_0

                appPath = Path.Combine(appPath, "netcoreapp2.0");

#elif NETCOREAPP2_1
                appPath = Path.Combine(appPath, "netcoreapp2.1");
#endif
            }
            List<Type> types = new List<Type>();
            var files = new DirectoryInfo(appPath).GetFiles("*.dll").Where(c =>
c.Name.IndexOf("DataAccess") >= 0
|| c.Name.IndexOf("Business") >= 0
|| c.Name.IndexOf("Repository") >= 0
|| c.Name.IndexOf("Service") >= 0
|| c.Name.IndexOf("Domain") >= 0);
            foreach (var file in files)
            {
                types.AddRange(Assembly.LoadFrom(file.FullName).GetTypes());
            }

            foreach (Type type in types)
            {
                string typeName = type.Name;
                if (!typeName.StartsWith("I"))
                {
                    continue;
                }
                if (typeName.EndsWith("Service") || typeName.EndsWith("Business") || typeName.EndsWith("DbAccess") || typeName.EndsWith("Repository"))
                {
                    Type realType = types.FirstOrDefault(t => t.Name != type.Name && !t.IsInterface && type.IsAssignableFrom(t));
                    if (realType != null)
                    {
                        List<Type> behaviors = new List<Type>();
                        RegisterType(type, realType, behaviors);
                    }
                }
                if (typeName.EndsWith("DataAccess"))
                {
                    List<Type> relateTypes = types.Where(t => t.Name != type.Name && !t.IsInterface && type.IsAssignableFrom(t)).ToList();
                    if (relateTypes != null && relateTypes.Count > 0)
                    {
                        Type providerType = relateTypes.FirstOrDefault(c => c.Name.EndsWith("Cache"));
                        providerType = providerType ?? relateTypes.First();
                        RegisterType(type, providerType);
                    }
                }
            }
        }

        [Conditional("DEBUG")]
        void DebugCombineContentPath(ref string parentPath)
        {
            parentPath = Path.Combine(parentPath, "Debug");
        }

        [Conditional("RELEASE")]
        void ReleaseCombineContentPath(ref string parentPath)
        {
            parentPath = Path.Combine(parentPath, "Release");
        }
    }
}
