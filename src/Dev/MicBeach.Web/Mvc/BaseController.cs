using MicBeach.Web.Security.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Web.Mvc
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class BaseController : Controller
    {
        #region 属性

        /// <summary>
        /// 是否为Post请求
        /// </summary>
        public bool IsPost
        {
            get
            {
                return HttpMethods.IsPost(HttpContext.Request.Method);
            }
        }

        #endregion

        #region 方法

        public override JsonResult Json(object data, JsonSerializerSettings serializerSettings)
        {
            if (serializerSettings == null)
            {
                var jsonOptions = (HttpContext.RequestServices.GetService(typeof(IOptions<MvcJsonOptions>)) as IOptions<MvcJsonOptions>)?.Value;
                serializerSettings = jsonOptions?.SerializerSettings ?? new JsonSerializerSettings();
            }
            serializerSettings.Converters.Add(new BigNumberJsonConverter());
            serializerSettings.ContractResolver = new DefaultContractResolver();
            return new CustomJsonResult(data, serializerSettings);
        }

        public override JsonResult Json(object data)
        {
            return Json(data, null);
        }

        #endregion
    }

    /// <summary>
    /// 控制器基类
    /// </summary>
    public class BaseController<IK> : BaseController
    {
        /// <summary>
        /// 当前用户对象
        /// </summary>
        public new AuthenticationUser<IK> User
        {
            get
            {
                return AuthenticationUser<IK>.GetUserFromClaims(HttpContext.User.Claims);
            }
        }
    }
}
