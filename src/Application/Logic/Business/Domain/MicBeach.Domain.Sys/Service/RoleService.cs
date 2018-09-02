using MicBeach.Domain.Sys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Sys;
using MicBeach.Domain.Sys.Repository;
using MicBeach.Util.IoC;
using MicBeach.Util;
using MicBeach.Util.Paging;
using MicBeach.Util.Response;

namespace MicBeach.Domain.Sys.Service
{
    /// <summary>
    /// 角色服务
    /// </summary>
    public static class RoleService
    {
        static IRoleRepository roleRepository = ContainerManager.Resolve<IRoleRepository>();

        #region 批量删除角色

        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="roleIds">角色编号</param>
        /// <returns></returns>
        public static void RemoveRole(IEnumerable<long> roleIds)
        {
            #region 参数判断

            if (roleIds.IsNullOrEmpty())
            {
                return;
            }

            #endregion

            //删除角色信息
            IQuery parentRemoveCondition = QueryFactory.Create();
            parentRemoveCondition.In<RoleQuery>(r => r.SysNo, roleIds);
            roleRepository.Remove(parentRemoveCondition);
            //删除子类
            IQuery childQuery = QueryFactory.Create();
            childQuery.In<RoleQuery>(r => r.Parent, roleIds);
            childQuery.AddQueryFields<RoleQuery>(r => r.SysNo);
            List<Role> roleList = roleRepository.GetList(childQuery);
            if (roleList.IsNullOrEmpty())
            {
                return;
            }
            RemoveRole(roleList.Select(r => r.SysNo));
        }

        #endregion

        #region 获取指定用户绑定的角色

        /// <summary>
        /// 获取指定用户绑定的角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>绑定的角色</returns>
        public static List<Role> GetUserBindRole(long userId)
        {
            if (userId <= 0)
            {
                return new List<Role>(0);
            }
            return roleRepository.GetUserBindRole(userId);
        }

        #endregion

        #region 保存角色信息

        /// <summary>
        /// 保存角色信息
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns></returns>
        public static Result<Role> SaveRole(Role role)
        {
            if (role == null)
            {
                return Result<Role>.FailedResult("没有指定要保存的");
            }
            if (role.SysNo <= 0)
            {
                return AddRole(role);
            }
            else
            {
                return EditRole(role);
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role">角色对象</param>
        /// <returns>执行结果</returns>
        static Result<Role> AddRole(Role role)
        {
            #region 参数判断

            if (role == null)
            {
                return Result<Role>.FailedResult("没有指定要添加的角色数据");
            }

            #endregion

            #region 上级

            long parentRoleId = role.Parent == null ? 0 : role.Parent.SysNo;
            Role parentRole = null;
            if (parentRoleId > 0)
            {
                IQuery parentQuery = QueryFactory.Create<RoleQuery>(c => c.SysNo == parentRoleId);
                parentRole = roleRepository.Get(parentQuery);
                if (parentRole == null)
                {
                    return Result<Role>.FailedResult("请选择正确的上级角色");
                }
            }
            role.SetParentRole(parentRole);

            #endregion

            role.CreateDate = DateTime.Now;
            role.Save();
            var result = Result<Role>.SuccessResult("添加成功");
            result.Data = role;
            return result;
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="newRole">角色对象</param>
        /// <returns>执行结果</returns>
        static Result<Role> EditRole(Role newRole)
        {
            #region 参数判断

            if (newRole == null)
            {
                return Result<Role>.FailedResult("没有指定要操作的角色信息");
            }

            #endregion

            Role role = roleRepository.Get(QueryFactory.Create<RoleQuery>(r => r.SysNo == newRole.SysNo));
            if (role == null)
            {
                return Result<Role>.FailedResult("没有指定要操作的角色信息");
            }
            //上级
            long newParentRoleId = newRole.Parent == null ? 0 : newRole.Parent.SysNo;
            long oldParentRoleId = role.Parent == null ? 0 : role.Parent.SysNo;
            //上级改变后 
            if (newParentRoleId != oldParentRoleId)
            {
                Role parentRole = null;
                if (newParentRoleId > 0)
                {
                    IQuery parentQuery = QueryFactory.Create<RoleQuery>(c => c.SysNo == newParentRoleId);
                    parentRole = roleRepository.Get(parentQuery);
                    if (parentRole == null)
                    {
                        return Result<Role>.FailedResult("请选择正确的上级角色");
                    }
                }
                role.SetParentRole(parentRole);
            }
            //修改信息
            role.Name = newRole.Name;
            role.Status = newRole.Status;
            role.Remark = newRole.Remark;
            role.Save();//保存角色信息
            var result = Result<Role>.SuccessResult("修改成功");
            result.Data = role;
            return result;
        }

        #endregion

        #region 获取角色

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        public static Role GetRole(IQuery query)
        {
            var role = roleRepository.Get(query);
            return role;
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="id">角色编号</param>
        /// <returns>角色信息</returns>
        public static Role GetRole(long id)
        {
            if (id <= 0)
            {
                return null;
            }
            IQuery query = QueryFactory.Create<RoleQuery>(c => c.SysNo == id);
            return GetRole(query);
        }

        #endregion

        #region 获取角色列表

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        public static List<Role> GetRoleList(IQuery query)
        {
            var roleList = roleRepository.GetList(query);
            return roleList;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="roleIds">角色编号</param>
        /// <returns></returns>
        public static List<Role> GetRoleList(IEnumerable<long> roleIds)
        {
            if (roleIds.IsNullOrEmpty())
            {
                return new List<Role>(0);
            }
            IQuery query = QueryFactory.Create<RoleQuery>(c => roleIds.Contains(c.SysNo));
            return GetRoleList(query);
        }

        #endregion

        #region 获取角色分页

        /// <summary>
        /// 获取Role分页
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        public static IPaging<Role> GetRolePaging(IQuery query)
        {
            var rolePaging = roleRepository.GetPaging(query);
            return rolePaging;
        }

        #endregion

        #region 修改角色排序

        /// <summary>
        /// 修改角色排序
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="newSort">新的排序</param>
        /// <returns></returns>
        public static Result ModifyRoleSortIndex(long roleId, int newSort)
        {
            #region 参数判断

            if (roleId <= 0)
            {
                return Result.FailedResult("没有指定要修改的角色");
            }

            #endregion

            Role role = GetRole(roleId);
            if (role == null)
            {
                return Result.FailedResult("没有指定要修改的角色");
            }
            role.ModifySortIndex(newSort);
            role.Save();
            return Result.SuccessResult("修改成功");
        }

        #endregion

        #region 检查角色名称是否存在

        /// <summary>
        /// 检查角色名称是否存在
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="excludeRoleId">除指定的角色之外</param>
        /// <returns></returns>
        public static bool ExistRoleName(string roleName, long excludeRoleId)
        {
            if (roleName.IsNullOrEmpty())
            {
                return false;
            }
            IQuery query = QueryFactory.Create<RoleQuery>(c => c.Name == roleName && c.SysNo != excludeRoleId);
            return roleRepository.Exist(query);
        }

        #endregion
    }
}
