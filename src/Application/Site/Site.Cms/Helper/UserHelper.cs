using MicBeach.DTO.Sys.Query;
using MicBeach.Util.IoC;
using MicBeach.ViewModel.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MicBeach.Util.Extension;
using MicBeach.Web.Utility;
using MicBeach.Develop.CQuery;
using System.Security.Claims;
using MicBeach.Web.Mvc;
using MicBeach.Util;
using MicBeach.DTO.Sys.Query.Filter;
using MicBeach.Application.Identity.User;
using MicBeach.DTO.Sys.Cmd;
using MicBeach.Util.Response;
using MicBeach.Util.Code;
using MicBeach.ServiceContract.Sys;
using MicBeach.Web.Security;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MicBeach.Web.Security.Authentication.Cookie;
using MicBeach.Web.Security.Authentication;
using MicBeach.Web.Security.Authorization;

namespace Site.Cms.Helper
{
    /// <summary>
    /// 用户操作
    /// </summary>
    public static class UserHelper
    {
        static UserHelper()
        {
            //CookieAuthenticationEventMethodProvider.OnValidatePrincipalAsync = ValidatePrincipalAsync;
        }

        static IUserService UserService
        {
            get
            {
                return ContainerManager.Container.Resolve<IUserService>();
            }
        }

        #region 登陆

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="loginInfo">登陆信息</param>
        /// <returns></returns>
        public static Result Login(LoginViewModel loginInfo)
        {
            if (loginInfo == null)
            {
                return Result.FailedResult("登陆信息不完整");
            }
            if (!VerificationCodeHelper.CheckLoginCode(loginInfo.VerificationCode))
            {
                return Result.FailedResult("验证码错误");
            }
            var result = UserService.Login(new UserDto()
            {
                UserName = loginInfo.LoginName,
                Pwd = loginInfo.Pwd
            });
            if (result == null || !result.Success || result.Data == null)
            {
                return Result.FailedResult("用户名或密码错误");
            }
            SaveLoginCredential(result.Object);
            return Result.SuccessResult("登陆成功");
        }

        /// <summary>
        /// 保存登陆信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        static void SaveLoginCredential(UserDto user)
        {
            if (null == user)
            {
                return;
            }
            AuthenticationUser<long> authUser = new AuthenticationUser<long>()
            {
                Id = user.SysNo,
                Name = user.UserName,
                RealName = user.RealName
            };
            HttpContextHelper.Current.SignInAsync(authUser, new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)

            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 登出
        /// </summary>
        public static void LoginOut()
        {
            try
            {
                HttpContextHelper.Current.SignOutAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
            }
        }

        static async Task<bool> ValidatePrincipalAsync(CookieValidatePrincipalContext context)
        {
            var authUser = AuthenticationUser<long>.GetUserFromPrincipal(context.Principal);
            if (authUser == null)
            {
                return await Task.FromResult(false).ConfigureAwait(false);
                //context.RejectPrincipal();
                //return Task.CompletedTask;
            }
            //context.ShouldRenew = true;
            //return Task.CompletedTask;
            return await Task.FromResult(true).ConfigureAwait(false);
        }

        public static AuthenticationUser<long> GetLoginUser()
        {
            return AuthenticationUser<long>.GetUserFromPrincipal(HttpContextHelper.Current.User);
        }

        #endregion

        #region 授权验证

        /// <summary>
        /// 授权验证
        /// </summary>
        /// <param name="operation">授权操作</param>
        /// <returns></returns>
        public static async Task<bool> AuthorizationAsync(AuthenticationUser<long> user, AuthorityOperationCmdDto operation)
        {
            if (operation == null || user == null)
            {
                return false;
            }
            AuthenticationCmdDto authInfo = new AuthenticationCmdDto()
            {
                Operation = operation,
                User = new AdminUserCmdDto()
                {
                    UserType = UserType.管理账户,
                    SysNo = user.Id
                }
            };
            return await Task.Run(() =>
            {
                return operation.Instance<IAuthService>().Authentication(authInfo);
            }).ConfigureAwait(false);
        }

        public static async Task<AuthorizeVerifyResult> AuthenticationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                return AuthorizeVerifyResult.ChallengeResult();
            }

            #region 操作信息

            string controllerName = context.RouteData.Values["controller"].ToString().ToUpper();
            string actionName = context.RouteData.Values["action"].ToString().ToUpper();
            string methodName = context.HttpContext.Request.Method;
            AuthorityOperationCmdDto operation = new AuthorityOperationCmdDto()
            {
                ControllerCode = controllerName,
                ActionCode = actionName
            };

            #endregion

            //登陆用户
            var loginUser = GetLoginUser();
            if (loginUser == null)
            {
                return AuthorizeVerifyResult.ChallengeResult();
            }
            var allowAccess = await AuthorizationAsync(loginUser, operation).ConfigureAwait(false);
            return allowAccess ? AuthorizeVerifyResult.SuccessResult() : AuthorizeVerifyResult.ForbidResult();
        }

        #endregion
    }
}