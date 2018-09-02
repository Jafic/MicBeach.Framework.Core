using IdentityModel;
using MicBeach.Util.Data;
using MicBeach.Util.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace MicBeach.Web.Security.Authentication
{
    /// <summary>
    /// 授权用户
    /// </summary>
    public class AuthenticationUser<TK> : IIdentity
    {
        #region 属性

        /// <summary>
        /// 用户编号
        /// </summary>
        public TK Id
        {
            get; set;
        }

        /// <summary>
        /// 授权类型
        /// </summary>
        public string AuthenticationType
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// 真实名称
        /// </summary>
        public string RealName
        {
            get; set;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 根据凭据信息获取一个用户对象
        /// </summary>
        /// <param name="principal">认证凭据</param>
        /// <returns></returns>
        public static AuthenticationUser<TK> GetUserFromPrincipal(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return null;
            }
            return GetUserFromClaims(principal.Claims);
        }

        /// <summary>
        /// 根据认证信息获取一个用户对象
        /// </summary>
        /// <param name="claims">认证信息</param>
        /// <returns></returns>
        public static AuthenticationUser<TK> GetUserFromClaims(IEnumerable<Claim> claims)
        {
            if (claims.IsNullOrEmpty())
            {
                return null;
            }
            var idClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (idClaim == null)
            {
                idClaim=claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject);
            }
            var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (nameClaim == null)
            {
                nameClaim=claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name);
            }
            var realNameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            if (realNameClaim == null)
            {
                realNameClaim=realNameClaim = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.NickName);
            }
            if (idClaim == null)
            {
                return null;
            }
            return new AuthenticationUser<TK>()
            {
                Id = DataConverter.ConvertToSimpleType<TK>(idClaim.Value),
                Name = nameClaim?.Value,
                RealName = realNameClaim?.Value
            };
        }

        /// <summary>
        /// 获取用户对应的身份认证信息
        /// </summary>
        /// <returns></returns>
        public virtual List<Claim> GetClaims()
        {
            return new List<Claim>()
            {
                new Claim(JwtClaimTypes.Subject,Id.ToString()),
                new Claim(JwtClaimTypes.Name,Name),
                new Claim(JwtClaimTypes.NickName,RealName)
            };
        }

        #endregion
    }
}
