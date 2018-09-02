using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Web.Security.Authorization
{
    /// <summary>
    /// 授权验证结果
    /// </summary>
    public class AuthorizeVerifyResult
    {
        /// <summary>
        /// 验证值
        /// </summary>
        public AuthorizeVerifyValue VerifyValue
        {
            get; set;
        }

        public bool AllowAccess
        {
            get
            {
                return VerifyValue == AuthorizeVerifyValue.Success;
            }
        }

        /// <summary>
        /// 未登陆拒绝访问
        /// </summary>
        /// <returns></returns>
        public static AuthorizeVerifyResult ChallengeResult()
        {
            return GetAuthorizeVerifyResult(AuthorizeVerifyValue.Challenge);
        }

        /// <summary>
        /// 未授权拒绝访问
        /// </summary>
        /// <returns></returns>
        public static AuthorizeVerifyResult ForbidResult()
        {
            return GetAuthorizeVerifyResult(AuthorizeVerifyValue.Forbid);
        }

        /// <summary>
        /// 验证通过
        /// </summary>
        /// <returns></returns>
        public static AuthorizeVerifyResult SuccessResult()
        {
            return GetAuthorizeVerifyResult(AuthorizeVerifyValue.Success);
        }

        /// <summary>
        /// 获取授权验证结果
        /// </summary>
        /// <param name="value">验证结果类型</param>
        /// <returns></returns>
        public static AuthorizeVerifyResult GetAuthorizeVerifyResult(AuthorizeVerifyValue value = AuthorizeVerifyValue.Forbid)
        {
            return new AuthorizeVerifyResult()
            {
                VerifyValue = value
            };
        }
    }

    public enum AuthorizeVerifyValue
    {
        Challenge = 110, //未登陆
        Forbid = 120, //未授权
        Success = 130 //成功
    }
}
