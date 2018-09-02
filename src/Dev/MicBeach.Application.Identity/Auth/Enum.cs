using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Application.Identity.Auth
{
    #region 权限组

    /// <summary>
    /// 权限分组状态
    /// </summary>
    public enum AuthorityGroupStatus
    {
        启用 = 310,
        关闭 = 320
    }

    #endregion

    #region 权限

    /// <summary>
    /// 权限状态
    /// </summary>
    public enum AuthorityStatus
    {
        启用 = 310,
        关闭 = 320
    }

    /// <summary>
    /// 权限类型
    /// </summary>
    public enum AuthorityType
    {
        管理 = 410
    }

    #endregion

    #region 授权操作分组

    /// <summary>
    /// 授权操作分组状态
    /// </summary>
    public enum AuthorityOperationGroupStatus
    {
        启用 = 310,
        关闭 = 320
    }

    #endregion

    #region 授权操作

    /// <summary>
    /// 授权操作状态
    /// </summary>
    public enum AuthorityOperationStatus
    {
        启用 = 310,
        关闭 = 320
    }

    /// <summary>
    /// 授权操作请求方式
    /// </summary>
    public enum AuthorityOperationMethod
    {
        全部 = 410,
        GET = 420,
        POST = 430
    }

    /// <summary>
    /// 授权操作类型
    /// </summary>
    public enum AuthorityOperationAuthorizeType
    {
        无限制 = 510,
        权限授权 = 520
    }

    #endregion

    #region 授权

    /// <summary>
    /// 授权对象类型
    /// </summary>
    public enum AuthorizeType
    {
        权限组 = 410,
        权限 = 420
    }

    #endregion
}
