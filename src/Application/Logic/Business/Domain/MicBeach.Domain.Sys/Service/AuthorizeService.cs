using MicBeach.Domain.Sys.Model;
using MicBeach.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Domain.Sys.Service.Parameter;
using MicBeach.Domain.Sys.Repository;
using MicBeach.Util.IoC;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Sys;
using MicBeach.Application.Identity.Auth;
using MicBeach.Util.Response;

namespace MicBeach.Domain.Sys.Service
{
    /// <summary>
    /// 授权服务
    /// </summary>
    public static class AuthorizeService
    {
        static IRoleAuthorizeRepository roleAuthRepository = ContainerManager.Container.Resolve<IRoleAuthorizeRepository>();
        static IUserAuthorizeRepository userAuthRepository = ContainerManager.Container.Resolve<IUserAuthorizeRepository>();
        static IAuthorityRepository authorityRepository = ContainerManager.Container.Resolve<IAuthorityRepository>();

        #region 角色授权

        #region 修改角色授权信息

        /// <summary>
        /// 修改角色授权信息
        /// </summary>
        /// <param name="roleAuthorizes">角色授权信息</param>
        /// <returns></returns>
        public static Result ModifyRoleAuthorize(ModifyRoleAuthorize roleAuthorizes)
        {
            if (roleAuthorizes == null || (roleAuthorizes.Binds.IsNullOrEmpty() && roleAuthorizes.UnBinds.IsNullOrEmpty()))
            {
                return Result.FailedResult("没有指定任何要修改的绑定信息");
            }
            //解绑
            if (!roleAuthorizes.UnBinds.IsNullOrEmpty())
            {
                roleAuthRepository.Remove(roleAuthorizes.UnBinds);
            }
            //绑定
            if (!roleAuthorizes.Binds.IsNullOrEmpty())
            {
                roleAuthRepository.Save(roleAuthorizes.Binds);
            }
            return Result.SuccessResult("修改成功");
        }

        #endregion

        #endregion

        #region 用户授权

        #region 修改用户授权

        /// <summary>
        /// 修改用户授权
        /// </summary>
        /// <param name="userAuthorizes">用户授权信息</param>
        /// <returns></returns>
        public static Result ModifyUserAuthorize(IEnumerable<UserAuthorize> userAuthorizes)
        {
            if (userAuthorizes.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定任何要修改的用户授权信息");
            }

            #region 角色授权

            List<long> userIds = userAuthorizes.Select(c => c.User?.SysNo ?? 0).Distinct().ToList();
            IQuery userRoleBindQuery = QueryFactory.Create<UserRoleQuery>(c => userIds.Contains(c.UserSysNo));
            userRoleBindQuery.AddQueryFields<UserRoleQuery>(c => c.RoleSysNo);
            IQuery roleAuthBindQuery = QueryFactory.Create<RoleAuthorizeQuery>();
            roleAuthBindQuery.And<RoleAuthorizeQuery>(c => c.Role, CriteriaOperator.In, userRoleBindQuery);
            roleAuthBindQuery.AddQueryFields<RoleAuthorizeQuery>(c => c.Authority);
            IQuery authQuery = QueryFactory.Create<AuthorityQuery>();
            authQuery.And<AuthorityQuery>(c => c.Code, CriteriaOperator.In, roleAuthBindQuery);
            authQuery.AddQueryFields<AuthorityQuery>(c => c.Code);
            IEnumerable<Authority> roleAuthoritys = AuthorityService.GetAuthorityList(authQuery);
            List<string> roleAuthorityCodes = roleAuthoritys.Select(c => c.Code).ToList();

            #endregion

            List<UserAuthorize> saveUserAuthorizes = new List<UserAuthorize>();
            userAuthRepository.Remove(userAuthorizes.ToArray());//移除授权数据
            List<UserAuthorize> disableAuthorizes = userAuthorizes.Where(c => c.Disable && roleAuthorityCodes.Contains(c.Authority?.Code)).ToList();//角色拥有但是用户显示禁用掉的授权
            if (!disableAuthorizes.IsNullOrEmpty())
            {
                saveUserAuthorizes.AddRange(disableAuthorizes);
            }
            List<UserAuthorize> enableAuthorizes = userAuthorizes.Where(c => !c.Disable && !roleAuthorityCodes.Contains(c.Authority?.Code)).ToList();//用户单独授权的权限
            if (!enableAuthorizes.IsNullOrEmpty())
            {
                saveUserAuthorizes.AddRange(enableAuthorizes);
            }
            if (!saveUserAuthorizes.IsNullOrEmpty())
            {
                userAuthRepository.Save(saveUserAuthorizes.ToArray());
            }
            return Result.SuccessResult("修改成功");
        }

        #endregion

        #endregion

        #region 授权验证

        /// <summary>
        /// 用户授权验证
        /// </summary>
        /// <param name="auth">授权验证信息</param>
        /// <returns></returns>
        public static bool Authentication(Authentication auth)
        {
            if (auth == null || auth.User == null || auth.Operation == null)
            {
                return false;
            }

            #region 用户信息验证

            User nowUser = UserService.GetUser(auth.User.SysNo);//当前用户
            if (nowUser == null)
            {
                return false;
            }
            if (nowUser.SuperUser)
            {
                return true;//超级用户不受权限控制
            }

            #endregion

            #region 授权操作信息验证

            AuthorityOperation nowOperation = AuthorityOperationService.GetAuthorityOperation(auth.Operation.ControllerCode, auth.Operation.ActionCode);//授权操作信息
            if (nowOperation == null || nowOperation.Status == AuthorityOperationStatus.关闭)
            {
                return false;
            }
            if (nowOperation.AuthorizeType == AuthorityOperationAuthorizeType.无限制)
            {
                return true;
            }

            #endregion

            #region 授权验证

            //权限
            IQuery authorityQuery = QueryFactory.Create<AuthorityQuery>(a => a.Status == AuthorityStatus.启用);
            authorityQuery.AddQueryFields<AuthorityQuery>(a => a.Code);
            //操作绑定权限
            IQuery operationBindQuery = QueryFactory.Create<AuthorityBindOperationQuery>(a => a.AuthorithOperation == nowOperation.SysNo);
            operationBindQuery.AddQueryFields<AuthorityBindOperationQuery>(a => a.AuthorityCode);
            authorityQuery.And<AuthorityQuery>(a => a.Code, CriteriaOperator.In, operationBindQuery);
            //当前用户可以使用
            IQuery userAuthorizeQuery = QueryFactory.Create<UserAuthorizeQuery>(a => a.User == auth.User.SysNo && a.Disable == false);
            userAuthorizeQuery.AddQueryFields<UserAuthorizeQuery>(a => a.Authority);
            //用户角色
            IQuery userRoleQuery = QueryFactory.Create<UserRoleQuery>(a => a.UserSysNo == auth.User.SysNo);
            userRoleQuery.AddQueryFields<UserRoleQuery>(r => r.RoleSysNo);
            //角色权限
            IQuery roleAuthorizeQuery = QueryFactory.Create<RoleAuthorizeQuery>();
            roleAuthorizeQuery.AddQueryFields<RoleAuthorizeQuery>(a => a.Authority);
            roleAuthorizeQuery.And<RoleAuthorizeQuery>(a => a.Role, CriteriaOperator.In, userRoleQuery);
            //用户或用户角色拥有权限
            IQuery userAndRoleAuthorityQuery = QueryFactory.Create();
            userAndRoleAuthorityQuery.And<AuthorityQuery>(a => a.Code, CriteriaOperator.In, userAuthorizeQuery);//用户拥有权限
            userAndRoleAuthorityQuery.Or<AuthorityQuery>(a => a.Code, CriteriaOperator.In, roleAuthorizeQuery);//或者角色拥有权限
            authorityQuery.And(userAndRoleAuthorityQuery);
            //去除用户禁用的
            IQuery userDisableAuthorizeQuery = QueryFactory.Create<UserAuthorizeQuery>(a => a.User == auth.User.SysNo && a.Disable == true);
            userDisableAuthorizeQuery.AddQueryFields<UserAuthorizeQuery>(a => a.Authority);
            authorityQuery.And<AuthorityQuery>(a => a.Code, CriteriaOperator.NotIn, userDisableAuthorizeQuery);
            return authorityRepository.Exist(authorityQuery);

            #endregion
        }

        #endregion
    }
}
