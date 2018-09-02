using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using MicBeach.DTO.Sys.Cmd;
using MicBeach.DTO.Sys.Query;
using MicBeach.Domain.Sys.Repository;
using MicBeach.BusinessContract.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Domain.Sys.Model;
using MicBeach.Domain.Sys.Service;
using MicBeach.Util;
using MicBeach.DTO.Sys.Query.Filter;
using MicBeach.Util.Response;
using MicBeach.Develop.UnitOfWork;
using MicBeach.Query.Sys;

namespace MicBeach.Business.Sys
{
    /// <summary>
    /// 角色业务
    /// </summary>
    public class RoleBusiness : IRoleBusiness
    {
        public RoleBusiness()
        {
        }

        #region 保存角色

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns></returns>
        public Result<RoleDto> SaveRole(SaveRoleCmdDto saveInfo)
        {
            if (saveInfo == null)
            {
                return Result<RoleDto>.FailedResult("没有指定任何要保存的信息");
            }
            using (var businessWork = UnitOfWork.Create())
            {
                var roleResult = RoleService.SaveRole(saveInfo.Role.MapTo<Role>());
                if (!roleResult.Success)
                {
                    return Result<RoleDto>.FailedResult(roleResult.Message);
                }
                var commitResult = businessWork.Commit();
                Result<RoleDto> result = null;
                if (commitResult.ExecutedSuccess)
                {
                    result = Result<RoleDto>.SuccessResult("保存成功");
                    result.Data = roleResult.Data.MapTo<RoleDto>();
                }
                else
                {
                    result = Result<RoleDto>.FailedResult("保存失败");
                }
                return result;
            }
        }

        #endregion

        #region 获取角色

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public RoleDto GetRole(RoleFilterDto filter)
        {
            var role = RoleService.GetRole(CreateQueryObject(filter));
            return role.MapTo<RoleDto>();
        }

        #endregion

        #region 删除角色

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteRole(DeleteRoleCmdDto deleteInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                #region 参数判断

                if (deleteInfo == null || deleteInfo.RoleIds.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要删除的角色");
                }

                #endregion

                RoleService.RemoveRole(deleteInfo.RoleIds);
                var exectVal = businessWork.Commit();
                return exectVal.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
            }
        }

        #endregion

        #region 获取角色列表

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public List<RoleDto> GetRoleList(RoleFilterDto filter)
        {
            var roleList = RoleService.GetRoleList(CreateQueryObject(filter));
            return roleList.Select(c => c.MapTo<RoleDto>()).ToList();
        }

        #endregion

        #region 获取角色分页

        /// <summary>
        /// 获取Role分页
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public IPaging<RoleDto> GetRolePaging(RoleFilterDto filter)
        {
            var rolePaging = RoleService.GetRolePaging(CreateQueryObject(filter));
            return rolePaging.ConvertTo<RoleDto>();
        }

        #endregion

        #region 修改角色排序

        /// <summary>
        /// 修改角色排序
        /// </summary>
        /// <param name="sortIndexInfo">排序修改信息</param>
        /// <returns></returns>
        public Result ModifyRoleSortIndex(ModifyRoleSortCmdDto sortIndexInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                #region 参数判断

                if (sortIndexInfo == null || sortIndexInfo.RoleSysNo <= 0)
                {
                    return Result.FailedResult("没有指定要修改的角色");
                }

                #endregion

                var modifyResult = RoleService.ModifyRoleSortIndex(sortIndexInfo.RoleSysNo, sortIndexInfo.NewSortIndex);
                if (!modifyResult.Success)
                {
                    return modifyResult;
                }
                var executeVal = businessWork.Commit();
                return executeVal.ExecutedSuccess ? Result.SuccessResult("排序修改成功") : Result.FailedResult("排序修改失败");
            }
        }

        #endregion

        #region 验证角色名称是否存在

        /// <summary>
        /// 验证角色名称是否存在
        /// </summary>
        /// <param name="existInfo">验证信息</param>
        /// <returns></returns>
        public bool ExistRoleName(ExistRoleNameCmdDto existInfo)
        {
            if (existInfo == null)
            {
                return false;
            }
            return RoleService.ExistRoleName(existInfo.RoleName, existInfo.ExcludeRoleId);
        }

        #endregion

        #region 根据查询条件生成查询对象

        /// <summary>
        /// 根据查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IQuery CreateQueryObject(RoleFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = QueryFactory.Create<RoleQuery>(filter);
            if (!filter.SysNos.IsNullOrEmpty())
            {
                query.In<RoleQuery>(c => c.SysNo, filter.SysNos);
            }
            if (!filter.Name.IsNullOrEmpty())
            {
                query.Equal<RoleQuery>(c => c.Name, filter.Name);
            }
            if (filter.Level.HasValue)
            {
                query.Equal<RoleQuery>(c => c.Level, filter.Level.Value);
            }
            if (filter.Parent.HasValue)
            {
                query.Equal<RoleQuery>(c => c.Parent, filter.Parent.Value);
            }
            if (filter.SortIndex.HasValue)
            {
                query.Equal<RoleQuery>(c => c.SortIndex, filter.SortIndex.Value);
            }
            if (filter.Status.HasValue)
            {
                query.Equal<RoleQuery>(c => c.Status, filter.Status.Value);
            }
            if (filter.CreateDate.HasValue)
            {
                query.Equal<RoleQuery>(c => c.CreateDate, filter.CreateDate.Value);
            }
            //if (!filter.Application.IsNullOrEmpty())
            //{
            //    query.Equal<RoleQuery>(c => c.Application, filter.Application);
            //}
            if (!filter.Remark.IsNullOrEmpty())
            {
                query.Equal<RoleQuery>(c => c.Remark, filter.Remark);
            }
            return query;
        }

        #endregion
    }
}
