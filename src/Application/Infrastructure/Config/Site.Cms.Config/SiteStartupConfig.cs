using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Site.Cms.Config
{
    public class Startup : IStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Use(next =>
            {
                Console.WriteLine("A");
                return async (context) =>
                {
                    // 1. 对Request做一些处理
                    // TODO

                    // 2. 调用下一个中间件
                    Console.WriteLine("A-BeginNext");
                    await next(context);
                    Console.WriteLine("A-EndNext");

                    // 3. 生成 Response
                    //TODO
                };
            });

            app.Use(next =>
            {
                Console.WriteLine("B");
                return async (context) =>
                {
                    // 1. 对Request做一些处理
                    // TODO

                    // 2. 调用下一个中间件
                    Console.WriteLine("B-BeginNext");
                    await next(context);
                    Console.WriteLine("B-EndNext");

                    // 3. 生成 Response
                    //TODO
                };
            });
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //throw new NotImplementedException();
            return services.BuildServiceProvider();
        }
    }
}
