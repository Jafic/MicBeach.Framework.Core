using MicBeach.Util.Serialize;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Formatters.Json.Internal;

namespace MicBeach.Web.Mvc
{
    /// <summary>
    /// 自定义Json对象
    /// </summary>
    public class CustomJsonResult:JsonResult
    {
        public CustomJsonResult(object value, JsonSerializerSettings serializerSettings):base(value,serializerSettings)
        {
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var services = context.HttpContext.RequestServices;
            var executor = services.GetRequiredService<JsonResultExecutor>();
            return executor.ExecuteAsync(context, this);
        }
    }
}
