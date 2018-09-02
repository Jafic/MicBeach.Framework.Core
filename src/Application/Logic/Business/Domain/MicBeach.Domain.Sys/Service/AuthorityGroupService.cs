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
using MicBeach.Domain.Sys.Model;
using MicBeach.Util;
using MicBeach.Util.Paging;
using MicBeach.Util.Response;

namespace MicBeach.Domain.Sys.Service
{
    /// <summary>
    /// 权限分组服务
    /// </summary>
    public static class AuthorityGroupService
    {
        static IAuthorityGroupRepository authorityGroupRepository = ContainerManager.Resolve<IAuthorityGroupRepository>();

        #region 批量删除权限分组

        /// <summary>
        /// 批量删除权限分组
        /// </summary>
        /// <param name="groupIds">分组编号</param>
        /// <returns></returns>
        public static Result RemoveAuthorityGroup(IEnumerable<long> groupIds)
        {
            #region 参数判断

            if (groupIds.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定任何要删除的权限分组");
            }

            #endregion

            //删除分组信息
            IQuery parentRemoveCondition = QueryFactory.Create();
            parentRemoveCondition.In<AuthorityGroupQuery>(r => r.SysNo, groupIds);
            authorityGroupRepository.Remove(parentRemoveCondition);
            //删除下级
            IQuery childQuery = QueryFactory.Create();
            childQuery.In<AuthorityGroupQuery>(r => r.Parent, groupIds);
            childQuery.AddQueryFields<AuthorityGroupQuery>(r => r.SysNo);
            List<AuthorityGroup> authorityGroupList = authorityGroupRepository.GetList(childQuery);
            if (authorityGroupList.IsNullOrEmpty())
            {
                return Result.SuccessResult("没有需要删除的下级分组");
            }
            RemoveAuthorityGroup(authorityGroupList.Select(r => r.SysNo));
            return Result.SuccessResult("删除成功");
        }

        #endregion

        #region 验证权限分组是否存在

        /// <summary>
        /// 验证权限分组是否存在
        /// </summary>
        /// <param name="groupId">分组编号</param>
        /// <returns></returns>
        public static bool ExistAuthorityGroup(long groupId)
        {
            if (groupId <= 0)
            {
                return false;
            }
            IQuery query = QueryFactory.Create<AuthorityGroupQuery>(c => c.SysNo == groupId);
            return authorityGroupRepository.Exist(query);
        }

        #endregion

        #region 保存权限分组

        /// <summary>
        /// 保存权限分组
        /// </summary>
        /// <param name="authorityGroup">权限分组对象</param>
        /// <returns>执行结果</returns>
        public static Result<AuthorityGroup> SaveAuthorityGroup(AuthorityGroup authorityGroup)
        {
            if (authorityGroup == null)
            {
                return Result<AuthorityGroup>.FailedResult("没有指定任何要保存的分组信息");
            }
            if (authorityGroup.SysNo > 0)
            {
                return UpdateAuthorityGroup(authorityGroup);
            }
            return AddAuthorityGroup(authorityGroup);
        }

        /// <summary>
        /// 添加权限分组
        /// </summary>
        /// <param name="authorityGroup">权限信息</param>
        /// <returns>执行结果</returns>
        static Result<AuthorityGroup> AddAuthorityGroup(AuthorityGroup authorityGroup)
        {
            #region 上级

            long parentGroupId = authorityGroup.Parent == null ? 0 : authorityGroup.Parent.SysNo;
            AuthorityGroup parentGroup = null;
            if (parentGroupId > 0)
            {
                IQuery parentQuery = QueryFactory.Create<AuthorityGroupQuery>(c => c.SysNo == parentGroupId);
                parentGroup = authorityGroupRepository.Get(parentQuery);
                if (parentGroup == null)
                {
                    return Result<AuthorityGroup>.FailedResult("请选择正确的上级分组");
                }
            }
            authorityGroup.SetParentGroup(parentGroup);

            #endregion

            authorityGroup.Save();//保存
            var result = Result<AuthorityGroup>.SuccessResult("添加成功");
            result.Data = authorityGroup;
            return result;
        }

        /// <summary>
        /// 更新权限分组
        /// </summary>
        /// <param name="authorityGroup">权限信息</param>
        /// <returns>执行结果</returns>
        static Result<AuthorityGroup> UpdateAuthorityGroup(AuthorityGroup newAuthorityGroup)
        {
            AuthorityGroup authorityGroup = authorityGroupRepository.Get(QueryFactory.Create<AuthorityGroupQuery>(r => r.SysNo == newAuthorityGroup.SysNo));
            if (authorityGroup == null)
            {
                return Result<AuthorityGroup>.FailedResult("没有指定要操作的分组信息");
            }
            //上级
            long newParentGroupId = newAuthorityGroup.Parent == null ? 0 : newAuthorityGroup.Parent.SysNo;
            long oldParentGroupId = authorityGroup.Parent == null ? 0 : authorityGroup.Parent.SysNo;
            //上级改变后 
            if (newParentGroupId != oldParentGroupId)
            {
                AuthorityGroup parentGroup = null;
                if (newParentGroupId > 0)
                {
                    IQuery parentQuery = QueryFactory.Create<AuthorityGroupQuery>(c => c.SysNo == newParentGroupId);
                    parentGroup = authorityGroupRepository.Get(parentQuery);
                    if (parentGroup == null)
                    {
                        return Result<AuthorityGroup>.FailedResult("请选择正确的上级分组");
                    }
                }
                authorityGroup.SetParentGroup(parentGroup);
            }
            //修改信息
            authorityGroup.Name = newAuthorityGroup.Name;
            authorityGroup.Status = newAuthorityGroup.Status;
            authorityGroup.Remark = newAuthorityGroup.Remark;
            authorityGroup.Save();//保存
            var result = Result<AuthorityGroup>.SuccessResult("更新成功");
            result.Data = authorityGroup;
            return result;
        }

        #endregion

        #region 获取权限分组

        /// <summary>
        /// 获取权限分组
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        public static AuthorityGroup GetAuthorityGroup(IQuery query)
        {
            var authorityGroup = authorityGroupRepository.Get(query);
            return authorityGroup;
        }

        /// <summary>
        /// 获取权限分组
        /// </summary>
        /// <param name="groupId">分组编号</param>
        /// <returns></returns>
        public static AuthorityGroup GetAuthorityGroup(long groupId)
        {
            if (groupId <= 0)
            {
                return null;
            }
            IQuery query = QueryFactory.Create<AuthorityGroupQuery>(c => c.SysNo == groupId);
            return GetAuthorityGroup(query);
        }

        #endregion

        #region 获取权限分组列表

        /// <summary>
        /// 获取权限分组列表
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        public static List<AuthorityGroup> GetAuthorityGroupList(IQuery query)
        {
            var authorityGroupList = authorityGroupRepository.GetList(query);
            return authorityGroupList;
        }

        /// <summary>
        /// 获取权限分组列表
        /// </summary>
        /// <param name="groupIds">分组编号</param>
        /// <returns></returns>
        public static List<AuthorityGroup> GetAuthorityGroupList(IEnumerable<long> groupIds)
        {
            if(groupIds.IsNullOrEmpty())
            {
                return new List<AuthorityGroup>(0);
            }
            IQuery query = QueryFactory.Create<AuthorityGroupQuery>(c => groupIds.Contains(c.SysNo));
            return GetAuthorityGroupList(query);
        }

        #endregion

        #region 获取权限分组分页

        /// <summary>
        /// 获取权限分组分页
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        public static IPaging<AuthorityGroup> GetAuthorityGroupPaging(IQuery query)
        {
            var authorityGroupPaging = authorityGroupRepository.GetPaging(query);
            return authorityGroupPaging;
        }

        #endregion

        #region 修改分组排序

        /// <summary>
        /// 修改分组排序
        /// </summary>
        /// <param name="sortIndexInfo">排序修改信息</param>
        /// <returns></returns>
        public static Result ModifyAuthorityGroupSort(long groupId, int newSort)
        {
            #region 参数判断

            if (groupId <= 0)
            {
                return Result.FailedResult("没有指定要修改的分组");
            }

            #endregion

            AuthorityGroup group = GetAuthorityGroup(groupId);
            if (group == null)
            {
                return Result.FailedResult("没有指定要修改的分组");
            }
            group.ModifySortIndex(newSort);
            group.Save();
            return Result.SuccessResult("修改成功");
        }

        #endregion

        #region 验证权限分组名是否存在

        /// <summary>
        /// 验证权限分组名是否存在
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <param name="excludeGroupId">排除验证的分组编号</param>
        /// <returns></returns>
        public static bool ExistGroupName(string groupName, long excludeGroupId)
        {
            if (groupName.IsNullOrEmpty())
            {
                return false;
            }
            IQuery query = QueryFactory.Create<AuthorityGroupQuery>(c => c.Name == groupName && c.SysNo != excludeGroupId);
            return authorityGroupRepository.Exist(query);
        }

        #endregion
    }
}
