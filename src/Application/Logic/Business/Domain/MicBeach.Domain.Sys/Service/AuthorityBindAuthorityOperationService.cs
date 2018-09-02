using MicBeach.Domain.Sys.Repository;
using MicBeach.Domain.Sys.Service.Parameter;
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
    /// 权限&授权操作绑定操作
    /// </summary>
    public static class AuthorityBindAuthorityOperationService
    {
        static IAuthorityBindOperationRepository bindRepository = ContainerManager.Container.Resolve<IAuthorityBindOperationRepository>();

        #region 修改权限&操作绑定

        /// <summary>
        /// 修改权限&操作绑定
        /// </summary>
        /// <param name="bindInfo">绑定信息</param>
        /// <returns></returns>
        public static Result ModifyAuthorityAndAuthorityOperationBind(ModifyAuthorityAndAuthorityOperationBind bindInfo)
        {
            if (bindInfo == null || (bindInfo.Binds.IsNullOrEmpty() && bindInfo.UnBinds.IsNullOrEmpty()))
            {
                return Result.FailedResult("没有指定任何要修改的信息");
            }
            //解绑
            if (!bindInfo.UnBinds.IsNullOrEmpty())
            {
                bindRepository.UnBind(bindInfo.UnBinds);
            }
            //绑定
            if (!bindInfo.Binds.IsNullOrEmpty())
            {
                bindRepository.Bind(bindInfo.Binds);
            }
            return Result.SuccessResult("修改成功");
        }

        #endregion
    }
}
