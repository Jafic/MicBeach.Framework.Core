using MicBeach.Domain.Sys.Model;
using MicBeach.Domain.Sys.Repository;
using MicBeach.Util;
using MicBeach.Util.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Util.Response;

namespace MicBeach.Domain.Sys.Service
{
    /// <summary>
    /// 用户角色绑定服务
    /// </summary>
    public static class UserRoleService
    {
        static IUserRoleRepository userRoleRepository = ContainerManager.Container.Resolve<IUserRoleRepository>();

        #region 用户和角色的绑定

        /// <summary>
        /// 绑定用户角色
        /// </summary>
        /// <param name="userRoleBinds">用户角色绑定信息</param>
        /// <returns></returns>
        public static Result BindUserAndRole(params Tuple<User, Role>[] userRoleBinds)
        {
            if (userRoleBinds.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定任何要绑定的信息");
            }
            userRoleRepository.Save(userRoleBinds);
            return Result.SuccessResult("绑定成功");
        }

        #endregion

        #region 用户角色解绑

        /// <summary>
        /// 用户角色解绑
        /// </summary>
        /// <param name="userRoleBinds">用户角色绑定信息</param>
        /// <returns></returns>
        public static Result UnBindUserAndRole(params Tuple<User, Role>[] userRoleBinds)
        {
            if (userRoleBinds.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定要解绑任何信息");
            }
            userRoleRepository.Remove(userRoleBinds);
            return Result.SuccessResult("解绑成功");
        }

        #endregion
    }
}
