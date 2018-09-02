using MicBeach.Domain.Sys.Repository;
using MicBeach.Util.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Sys;
using MicBeach.Util;
using MicBeach.Domain.Sys.Model;
using MicBeach.Util.Paging;
using MicBeach.Domain.Sys.Service.Parameter;
using MicBeach.Application.Identity.Auth;
using MicBeach.Util.Response;

namespace MicBeach.Domain.Sys.Service
{
    /// <summary>
    /// 权限服务
    /// </summary>
    public static class AuthorityService
    {
        static IAuthorityRepository authRepository = ContainerManager.Resolve<IAuthorityRepository>();

        #region 修改权限状态

        /// <summary>
        /// 修改权限状态
        /// </summary>
        /// <param name="statusInfo">状态信息</param>
        public static Result ModifyAuthorityStatus(params ModifyAuthorityStatus[] statusInfos)
        {
            #region 参数判断

            if (statusInfos.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定要操作的权限信息");
            }

            #endregion

            List<string> authCodes = statusInfos.Select(c => c.Code).Distinct().ToList();
            var authorityList = GetAuthorityList(authCodes);
            if (authorityList.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定要操作的权限信息");
            }
            foreach (var auth in authorityList)
            {
                if (auth == null)
                {
                    continue;
                }
                var newStatus = statusInfos.FirstOrDefault(c => c.Code == auth.Code);
                if (newStatus == null)
                {
                    continue;
                }
                auth.Status = newStatus.Status;
                auth.Save();
            }
            return Result.SuccessResult("修改成功");
        }

        #endregion

        #region 删除权限

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="authorityCodes">要删除的权限编码</param>
        public static void DeleteAuthority(IEnumerable<string> authorityCodes)
        {
            if (authorityCodes.IsNullOrEmpty())
            {
                throw new Exception("应至少指定一个要删除的权限");
            }
            IQuery delQuery = QueryFactory.Create();
            delQuery.In<AuthorityQuery>(a => a.Code, authorityCodes);
            authRepository.Remove(delQuery);
        }

        #endregion

        #region 保存权限

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="authority">权限对象</param>
        /// <returns>执行结果</returns>
        public static Result<Authority> SaveAuthority(Authority authority)
        {
            if (authority == null)
            {
                return Result<Authority>.FailedResult("权限信息为空");
            }
            //权限分组
            if (authority.AuthGroup == null || authority.AuthGroup.SysNo <= 0)
            {
                return Result<Authority>.FailedResult("请设置正确的权限组");
            }
            if (!AuthorityGroupService.ExistAuthorityGroup(authority.AuthGroup.SysNo))
            {
                return Result<Authority>.FailedResult("请设置正确的权限组");
            }
            Authority nowAuthority = GetAuthority(authority.Code);
            if (nowAuthority == null)
            {
                nowAuthority = authority;
                nowAuthority.AuthType = AuthorityType.管理;
                nowAuthority.CreateDate = DateTime.Now;
                nowAuthority.Sort = 0;
            }
            else
            {
                //nowAuthority.Code = authority.Code;
                nowAuthority.Name = authority.Name;
                nowAuthority.Status = authority.Status;
                nowAuthority.Remark = authority.Remark;
            }
            nowAuthority.Save();
            var result = Result<Authority>.SuccessResult("保存成功");
            result.Data = nowAuthority;
            return result;
        }

        #endregion

        #region 获取权限

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        public static Authority GetAuthority(IQuery query)
        {
            var authority = authRepository.Get(query);
            return authority;
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="authCode">权限码</param>
        /// <returns></returns>
        public static Authority GetAuthority(string authCode)
        {
            if (authCode.IsNullOrEmpty())
            {
                return null;
            }
            IQuery authQuery = QueryFactory.Create<AuthorityQuery>(a => a.Code == authCode);
            return GetAuthority(authQuery);
        }

        #endregion

        #region 获取权限列表

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        public static List<Authority> GetAuthorityList(IQuery query)
        {
            var authorityList = authRepository.GetList(query);
            authorityList = LoadOtherObjectData(authorityList, query);
            return authorityList;
        }

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="codes">权限码</param>
        /// <returns></returns>
        public static List<Authority> GetAuthorityList(IEnumerable<string> codes)
        {
            if (codes.IsNullOrEmpty())
            {
                return new List<Authority>(0);
            }
            IQuery query = QueryFactory.Create<AuthorityQuery>(c => codes.Contains(c.Code));
            return GetAuthorityList(query);
        }

        #endregion

        #region 获取权限分页

        /// <summary>
        /// 获取权限分页
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        public static IPaging<Authority> GetAuthorityPaging(IQuery query)
        {
            var authorityPaging = authRepository.GetPaging(query);
            var authorityList = LoadOtherObjectData(authorityPaging, query);
            return new Paging<Authority>(authorityPaging.Page, authorityPaging.PageSize, authorityPaging.TotalCount, authorityList);
        }

        #endregion

        #region 检查权限编码是否存在

        /// <summary>
        /// 检查权限编码是否存在
        /// </summary>
        /// <param name="code">权限编码</param>
        /// <returns></returns>
        public static bool ExistAuthorityCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                return false;
            }
            IQuery query = QueryFactory.Create<AuthorityQuery>(c => c.Code == code);
            return authRepository.Exist(query);
        }

        #endregion

        #region 检查权限名称是否存在

        /// <summary>
        /// 检查权限名称是否存在
        /// </summary>
        /// <param name="name">权限名称</param>
        /// <param name="excludeCode">排除验证的权限编码</param>
        /// <returns></returns>
        public static bool ExistAuthorityName(string name, string excludeCode)
        {
            if (name.IsNullOrEmpty())
            {
                return false;
            }
            IQuery query = QueryFactory.Create<AuthorityQuery>(c => c.Name == name && c.Code != (excludeCode ?? string.Empty));
            return authRepository.Exist(query);
        }

        #endregion

        #region 加载其它数据

        /// <summary>
        /// 加载其它数据
        /// </summary>
        /// <param name="authoritys">权限数据</param>
        /// <param name="query">筛选条件</param>
        /// <returns></returns>
        static List<Authority> LoadOtherObjectData(IEnumerable<Authority> authoritys, IQuery query)
        {
            if (authoritys.IsNullOrEmpty())
            {
                return new List<Authority>(0);
            }
            if (query == null)
            {
                return authoritys.ToList();
            }

            #region 权限分组

            List<AuthorityGroup> groupList = null;
            if (query.AllowLoad<Authority>(c => c.AuthGroup))
            {
                var groupIds = authoritys.Select(c => c.AuthGroup?.SysNo ?? 0).Distinct().ToList();
                groupList = AuthorityGroupService.GetAuthorityGroupList(groupIds);
            }

            #endregion

            foreach (var auth in authoritys)
            {
                if (auth == null)
                {
                    continue;
                }
                if (!groupList.IsNullOrEmpty())
                {
                    auth.SetGroup(groupList.FirstOrDefault(c => c.SysNo == auth.AuthGroup?.SysNo));
                }
            }

            return authoritys.ToList();
        }

        #endregion
    }
}
