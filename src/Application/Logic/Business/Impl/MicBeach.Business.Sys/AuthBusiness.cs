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
using MicBeach.Domain.Sys.Service.Parameter;
using MicBeach.DTO.Sys.Query.Filter;
using MicBeach.Util.Response;
using MicBeach.Develop.UnitOfWork;
using MicBeach.Query.Sys;

namespace MicBeach.Business.Sys
{
    /// <summary>
    /// 权限业务
    /// </summary>
    public class AuthBusiness : IAuthBusiness
    {
        public AuthBusiness()
        {
        }

        #region 权限数据

        #region 保存权限

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="saveInfo">权限对象</param>
        /// <returns>执行结果</returns>
        public Result<AuthorityDto> SaveAuthority(SaveAuthorityCmdDto saveInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                if (saveInfo == null)
                {
                    return Result<AuthorityDto>.FailedResult("没有指定任何要保存的权限信息");
                }

                #region 保存权限数据

                var authSaveResult = AuthorityService.SaveAuthority(saveInfo.Authority.MapTo<Authority>());
                if (!authSaveResult.Success)
                {
                    return Result<AuthorityDto>.FailedResult(authSaveResult.Message);
                }

                #endregion

                var commitVal = businessWork.Commit();
                Result<AuthorityDto> result = null;
                if (commitVal.ExecutedSuccess)
                {
                    result = Result<AuthorityDto>.SuccessResult("保存成功");
                    result.Data = authSaveResult.Data.MapTo<AuthorityDto>();
                }
                else
                {
                    result = Result<AuthorityDto>.FailedResult("保存失败");
                }
                return result;
            }
        }

        #endregion

        #region 获取权限

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public AuthorityDto GetAuthority(AuthorityFilterDto filter)
        {
            var authority = AuthorityService.GetAuthority(CreateAuthorityQueryObject(filter));
            return authority.MapTo<AuthorityDto>();
        }

        #endregion

        #region 获取权限列表

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public List<AuthorityDto> GetAuthorityList(AuthorityFilterDto filter)
        {
            var authorityList = AuthorityService.GetAuthorityList(CreateAuthorityQueryObject(filter));
            return authorityList.Select(c => c.MapTo<AuthorityDto>()).ToList();
        }

        #endregion

        #region 获取权限分页

        /// <summary>
        /// 获取权限分页
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public IPaging<AuthorityDto> GetAuthorityPaging(AuthorityFilterDto filter)
        {
            var authorityPaging = AuthorityService.GetAuthorityPaging(CreateAuthorityQueryObject(filter));
            return authorityPaging.ConvertTo<AuthorityDto>();
        }

        #endregion

        #region 删除权限

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteAuthority(DeleteAuthorityCmdDto deleteInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                #region 参数判断

                if (deleteInfo == null || deleteInfo.AuthorityCodes.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要删除的权限");
                }

                #endregion

                AuthorityService.DeleteAuthority(deleteInfo.AuthorityCodes);
                var exectVal = businessWork.Commit();
                return exectVal.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
            }
        }

        #endregion

        #region 修改权限状态

        /// <summary>
        /// 修改权限状态
        /// </summary>
        /// <param name="statusInfo">状态信息</param>
        /// <returns>执行结果</returns>
        public Result ModifyAuthorityStatus(ModifyAuthorityStatusCmdDto statusInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                if (statusInfo == null || statusInfo.AuthorityStatusInfo == null)
                {
                    return Result.FailedResult("没有指定任何要修改的权限信息");
                }
                List<ModifyAuthorityStatus> modifyStatusList = new List<ModifyAuthorityStatus>();
                foreach (var statusItem in statusInfo.AuthorityStatusInfo)
                {
                    modifyStatusList.Add(new ModifyAuthorityStatus()
                    {
                        Code = statusItem.Key,
                        Status = statusItem.Value
                    });
                }
                var modifyResult = AuthorityService.ModifyAuthorityStatus(modifyStatusList.ToArray());
                if (!modifyResult.Success)
                {
                    return modifyResult;
                }
                var commitVal = businessWork.Commit();
                return commitVal.ExecutedSuccess ? Result.SuccessResult("修改成功") : Result.FailedResult("修改失败");
            }
        }

        #endregion

        #region 检查权限编码是否存在

        /// <summary>
        /// 检查权限编码是否存在
        /// </summary>
        /// <param name="codeInfo">权限编码</param>
        /// <returns></returns>
        public bool ExistAuthorityCode(ExistAuthorityCodeCmdDto codeInfo)
        {
            if (codeInfo==null)
            {
                return false;
            }
            return AuthorityService.ExistAuthorityCode(codeInfo.AuthCode);
        }

        #endregion

        #region 检查权限名称是否存在

        /// <summary>
        /// 检查权限名称是否存在
        /// </summary>
        /// <param name="nameInfo">权限名信息</param>
        /// <returns></returns>
        public bool ExistAuthorityName(ExistAuthorityNameCmdDto nameInfo)
        {
            if (nameInfo==null)
            {
                return false;
            }
            return AuthorityService.ExistAuthorityName(nameInfo.Name, nameInfo.ExcludeCode);
        }

        #endregion

        #endregion

        #region 权限分组

        #region 保存权限分组

        /// <summary>
        /// 保存权限分组
        /// </summary>
        /// <param name="authorityGroup">权限分组对象</param>
        /// <returns>执行结果</returns>
        public Result<AuthorityGroupDto> SaveAuthorityGroup(SaveAuthorityGroupCmdDto saveInfo)
        {
            if (saveInfo == null || saveInfo.AuthorityGroup == null)
            {
                return Result<AuthorityGroupDto>.FailedResult("分组信息不完整");
            }
            using (var businessWork = UnitOfWork.Create())
            {
                var saveResult = AuthorityGroupService.SaveAuthorityGroup(saveInfo.AuthorityGroup.MapTo<AuthorityGroup>());
                if (!saveResult.Success)
                {
                    return Result<AuthorityGroupDto>.FailedResult(saveResult.Message);
                }
                var commitResult = businessWork.Commit();
                Result<AuthorityGroupDto> result = null;
                if (commitResult.ExecutedSuccess)
                {
                    result = Result<AuthorityGroupDto>.SuccessResult("保存成功");
                    result.Data = saveResult.Data.MapTo<AuthorityGroupDto>();
                }
                else
                {
                    result = Result<AuthorityGroupDto>.FailedResult("保存失败");
                }
                return result;
            }
        }

        #endregion

        #region 获取权限分组

        /// <summary>
        /// 获取权限分组
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public AuthorityGroupDto GetAuthorityGroup(AuthorityGroupFilterDto filter)
        {
            var authorityGroup = AuthorityGroupService.GetAuthorityGroup(CreateAuthorityGroupQueryObject(filter));
            return authorityGroup.MapTo<AuthorityGroupDto>();
        }

        #endregion

        #region 获取权限分组列表

        /// <summary>
        /// 获取权限分组列表
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public List<AuthorityGroupDto> GetAuthorityGroupList(AuthorityGroupFilterDto filter)
        {
            var authorityGroupList = AuthorityGroupService.GetAuthorityGroupList(CreateAuthorityGroupQueryObject(filter));
            return authorityGroupList.Select(c => c.MapTo<AuthorityGroupDto>()).ToList();
        }

        #endregion

        #region 获取权限分组分页

        /// <summary>
        /// 获取权限分组分页
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public IPaging<AuthorityGroupDto> GetAuthorityGroupPaging(AuthorityGroupFilterDto filter)
        {
            var authorityGroupPaging = AuthorityGroupService.GetAuthorityGroupPaging(CreateAuthorityGroupQueryObject(filter));
            return authorityGroupPaging.ConvertTo<AuthorityGroupDto>();
        }

        #endregion

        #region 删除权限分组

        /// <summary>
        /// 删除权限分组
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteAuthorityGroup(DeleteAuthorityGroupCmdDto deleteInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                #region 参数判断

                if (deleteInfo == null || deleteInfo.AuthorityGroupIds.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要删除的权限分组");
                }

                #endregion

                var result=AuthorityGroupService.RemoveAuthorityGroup(deleteInfo.AuthorityGroupIds);
                if (!result.Success)
                {
                    return result;
                }
                var exectVal = businessWork.Commit();
                return exectVal.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
            }
        }

        #endregion

        #region 修改分组排序

        /// <summary>
        /// 修改分组排序
        /// </summary>
        /// <param name="sortIndexInfo">排序修改信息</param>
        /// <returns></returns>
        public Result ModifyAuthorityGroupSort(ModifyAuthorityGroupSortCmdDto sortIndexInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                #region 参数判断

                if (sortIndexInfo == null || sortIndexInfo.AuthorityGroupSysNo <= 0)
                {
                    return Result.FailedResult("没有指定要修改的分组");
                }

                #endregion

                #region 修改分组状态信息

                var modifyResult = AuthorityGroupService.ModifyAuthorityGroupSort(sortIndexInfo.AuthorityGroupSysNo, sortIndexInfo.NewSortIndex);
                if (!modifyResult.Success)
                {
                    return modifyResult;
                }

                #endregion

                var executeVal = businessWork.Commit();
                return executeVal.ExecutedSuccess ? Result.SuccessResult("排序修改成功") : Result.FailedResult("排序修改失败");
            }
        }

        #endregion

        #region 验证权限分组名称是否存在

        /// <summary>
        /// 验证权限分组名称是否存在
        /// </summary>
        /// <param name="existInfo">验证信息</param>
        /// <returns></returns>
        public bool ExistAuthorityGroupName(ExistAuthorityGroupNameCmdDto existInfo)
        {
            if (existInfo == null)
            {
                return false;
            }
            return AuthorityGroupService.ExistGroupName(existInfo.GroupName, existInfo.ExcludeGroupId);
        }

        #endregion

        #endregion

        #region 操作分组

        #region 保存授权操作组

        /// <summary>
        /// 保存授权操作组
        /// </summary>
        /// <param name="saveInfo">授权操作组对象</param>
        /// <returns>执行结果</returns>
        public Result<AuthorityOperationGroupDto> SaveAuthorityOperationGroup(SaveAuthorityOperationGroupCmdDto saveInfo)
        {
            if (saveInfo == null)
            {
                return Result<AuthorityOperationGroupDto>.FailedResult("操作分组信息不完整");
            }
            using (var businessWork = UnitOfWork.Create())
            {
                var saveResult = AuthorityOperationGroupService.SaveAuthorityOperationGroup(saveInfo.AuthorityOperationGroup.MapTo<AuthorityOperationGroup>());
                if (!saveResult.Success)
                {
                    return Result<AuthorityOperationGroupDto>.FailedResult(saveResult.Message);
                }

                var commitResult = businessWork.Commit();
                Result<AuthorityOperationGroupDto> result = null;
                if (commitResult.ExecutedSuccess)
                {
                    result = Result<AuthorityOperationGroupDto>.SuccessResult("保存成功");
                    result.Data = saveResult.Data.MapTo<AuthorityOperationGroupDto>();
                }
                else
                {
                    result = Result<AuthorityOperationGroupDto>.FailedResult("保存失败");
                }
                return result;
            }
        }

        #endregion

        #region 获取授权操作组

        /// <summary>
        /// 获取授权操作组
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public AuthorityOperationGroupDto GetAuthorityOperationGroup(AuthorityOperationGroupFilterDto filter)
        {
            var authorityOperationGroup = AuthorityOperationGroupService.GetAuthorityOperationGroup(CreateAuthorityOperationGroupQueryObject(filter));
            return authorityOperationGroup.MapTo<AuthorityOperationGroupDto>();
        }

        #endregion

        #region 获取授权操作组列表

        /// <summary>
        /// 获取授权操作组列表
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public List<AuthorityOperationGroupDto> GetAuthorityOperationGroupList(AuthorityOperationGroupFilterDto filter)
        {
            var authorityOperationGroupList = AuthorityOperationGroupService.GetAuthorityOperationGroupList(CreateAuthorityOperationGroupQueryObject(filter));
            return authorityOperationGroupList.Select(c => c.MapTo<AuthorityOperationGroupDto>()).ToList();
        }

        #endregion

        #region 获取授权操作组分页

        /// <summary>
        /// 获取授权操作组分页
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public IPaging<AuthorityOperationGroupDto> GetAuthorityOperationGroupPaging(AuthorityOperationGroupFilterDto filter)
        {
            var authorityOperationGroupPaging = AuthorityOperationGroupService.GetAuthorityOperationGroupPaging(CreateAuthorityOperationGroupQueryObject(filter));
            return authorityOperationGroupPaging.ConvertTo<AuthorityOperationGroupDto>();
        }

        #endregion

        #region 删除授权操作组

        /// <summary>
        /// 删除授权操作组
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteAuthorityOperationGroup(DeleteAuthorityOperationGroupCmdDto deleteInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                #region 参数判断

                if (deleteInfo == null || deleteInfo.AuthorityOperationGroupIds.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要删除的授权操作组");
                }

                #endregion

                var deleteResult = AuthorityOperationGroupService.DeleteAuthorityOperationGroup(deleteInfo.AuthorityOperationGroupIds);
                if (!deleteResult.Success)
                {
                    return deleteResult;
                }
                var exectVal = businessWork.Commit();
                return exectVal.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
            }
        }

        #endregion

        #region 修改授权操作组排序

        /// <summary>
        /// 修改分组排序
        /// </summary>
        /// <param name="sortInfo">排序信息</param>
        /// <returns>执行结果</returns>
        public Result ModifySortIndex(ModifyAuthorityOperationGroupSortCmdDto sortInfo)
        {
            if (sortInfo == null)
            {
                return Result.FailedResult("没有指定任何要修改的信息");
            }
            using (var businessWork = UnitOfWork.Create())
            {
                var modifyResult = AuthorityOperationGroupService.ModifySortIndex(sortInfo.AuthorityOperationGroupSysNo, sortInfo.NewSort);
                if (!modifyResult.Success)
                {
                    return modifyResult;
                }
                var commitResult = businessWork.Commit();
                return commitResult.ExecutedSuccess ? Result.SuccessResult("修改成功") : Result.FailedResult("修改失败");
            }
        }

        #endregion

        #region 检查操作分组名称是否可用

        /// <summary>
        /// 检查操作分组名称是否可用
        /// </summary>
        /// <param name="nameInfo">分组名称信息</param>
        /// <returns></returns>
        public bool ExistAuthorityOperationGroupName(ExistAuthorityOperationGroupNameCmdDto nameInfo)
        {
            if (nameInfo == null)
            {
                return false;
            }
            return AuthorityOperationGroupService.ExistGroupName(nameInfo.GroupName, nameInfo.ExcludeGroupId);
        }

        #endregion

        #endregion

        #region 授权操作

        #region 保存授权操作

        /// <summary>
        /// 保存授权操作
        /// </summary>
        /// <param name="saveInfo">授权操作对象</param>
        /// <returns>执行结果</returns>
        public Result<AuthorityOperationDto> SaveAuthorityOperation(SaveAuthorityOperationCmdDto saveInfo)
        {
            if (saveInfo == null)
            {
                return Result<AuthorityOperationDto>.FailedResult("授权操作信息不完整");
            }
            using (var businessWork = UnitOfWork.Create())
            {
                var saveResult = AuthorityOperationService.SaveAuthorityOperation(saveInfo.AuthorityOperation.MapTo<AuthorityOperation>());
                if (!saveResult.Success)
                {
                    return Result<AuthorityOperationDto>.FailedResult(saveResult.Message);
                }
                var commitResult = businessWork.Commit();
                Result<AuthorityOperationDto> result = null;
                if (commitResult.ExecutedSuccess)
                {
                    result = Result<AuthorityOperationDto>.SuccessResult("保存成功");
                    result.Data = saveResult.Data.MapTo<AuthorityOperationDto>();
                }
                else
                {
                    result = Result<AuthorityOperationDto>.SuccessResult("保存失败");
                }
                return result;
            }
        }

        #endregion

        #region 获取授权操作

        /// <summary>
        /// 获取授权操作
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public AuthorityOperationDto GetAuthorityOperation(AuthorityOperationFilterDto filter)
        {
            var authorityOperation = AuthorityOperationService.GetAuthorityOperation(CreateAuthorityOperationQueryObject(filter));
            return authorityOperation.MapTo<AuthorityOperationDto>();
        }

        #endregion

        #region 获取授权操作列表

        /// <summary>
        /// 获取授权操作列表
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public List<AuthorityOperationDto> GetAuthorityOperationList(AuthorityOperationFilterDto filter)
        {
            var authorityOperationList = AuthorityOperationService.GetAuthorityOperationList(CreateAuthorityOperationQueryObject(filter));
            return authorityOperationList.Select(c => c.MapTo<AuthorityOperationDto>()).ToList();
        }

        #endregion

        #region 获取授权操作分页

        /// <summary>
        /// 获取授权操作分页
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public IPaging<AuthorityOperationDto> GetAuthorityOperationPaging(AuthorityOperationFilterDto filter)
        {
            var authorityOperationPaging = AuthorityOperationService.GetAuthorityOperationPaging(CreateAuthorityOperationQueryObject(filter));
            return authorityOperationPaging.ConvertTo<AuthorityOperationDto>();
        }

        #endregion

        #region 删除授权操作

        /// <summary>
        /// 删除授权操作
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteAuthorityOperation(DeleteAuthorityOperationCmdDto deleteInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                #region 参数判断

                if (deleteInfo == null || deleteInfo.AuthorityOperationIds.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要删除的授权操作");
                }

                #endregion

                AuthorityOperationService.DeleteAuthorityOperation(deleteInfo.AuthorityOperationIds);
                var exectVal = businessWork.Commit();
                return exectVal.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
            }
        }

        #endregion

        #region 修改操作状态

        /// <summary>
        /// 修改授权操作状态
        /// </summary>
        /// <param name="statusInfo">状态信息</param>
        /// <returns>执行结果</returns>
        public Result ModifyAuthorityOperationStatus(ModifyAuthorityOperationStatusCmdDto statusInfo)
        {
            if (statusInfo == null || statusInfo.StatusInfo == null || statusInfo.StatusInfo.Count <= 0)
            {
                return Result.FailedResult("没有指定任何要修改的状态信息");
            }
            using (var businessWork = UnitOfWork.Create())
            {
                #region 修改状态信息

                List<ModifyAuthorityOperationStatus> newStatusList = new List<ModifyAuthorityOperationStatus>();
                foreach (var statusItem in statusInfo.StatusInfo)
                {
                    newStatusList.Add(new ModifyAuthorityOperationStatus()
                    {
                        OperationId = statusItem.Key,
                        Status = statusItem.Value
                    });
                }
                var modifyResult = AuthorityOperationService.ModifyStatus(newStatusList.ToArray());
                if (!modifyResult.Success)
                {
                    return modifyResult;
                }

                #endregion

                var commitVal = businessWork.Commit();
                return commitVal.ExecutedSuccess ? Result.SuccessResult("修改成功") : Result.FailedResult("修改失败");
            }
        }

        #endregion

        #region 验证授权操作名是否存在

        /// <summary>
        /// 验证授权操作名是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="excludeId">排除指定的授权操作</param>
        /// <returns></returns>
        public bool ExistAuthorityOperationName(string name, long excludeId)
        {
            return AuthorityOperationService.ExistOperationName(name, excludeId);
        }

        #endregion

        #endregion

        #region 权限&操作绑定

        /// <summary>
        /// 修改权限及权限操作绑定
        /// </summary>
        /// <param name="bindInfo">绑定信息</param>
        /// <returns></returns>
        public Result ModifyAuthorityOperationBindAuthority(ModifyAuthorityBindAuthorityOperationCmdDto bindInfo)
        {
            if (bindInfo == null)
            {
                return Result.FailedResult("没有指定任何要修改的信息");
            }
            using (var businessWork = UnitOfWork.Create())
            {
                var modifyResult = AuthorityBindAuthorityOperationService.ModifyAuthorityAndAuthorityOperationBind(bindInfo.MapTo<ModifyAuthorityAndAuthorityOperationBind>());
                if (!modifyResult.Success)
                {
                    return modifyResult;
                }
                var commitResult = businessWork.Commit();
                return commitResult.ExecutedSuccess ? Result.SuccessResult("修改成功") : Result.FailedResult("修改失败");
            }
        }

        #endregion

        #region 授权

        #region 修改角色授权

        /// <summary>
        /// 修改角色授权
        /// </summary>
        /// <param name="authInfo">授权信息</param>
        /// <returns></returns>
        public Result ModifyRoleAuthorize(ModifyRoleAuthorizeCmdDto authInfo)
        {
            if (authInfo == null)
            {
                return Result.FailedResult("没有指定任何要修改的角色授权信息");
            }
            using (var businessWork = UnitOfWork.Create())
            {
                var modifyResult = AuthorizeService.ModifyRoleAuthorize(authInfo.MapTo<ModifyRoleAuthorize>());
                if (!modifyResult.Success)
                {
                    return modifyResult;
                }
                var commitResult = businessWork.Commit();
                return commitResult.ExecutedSuccess ? Result.SuccessResult("修改成功") : Result.FailedResult("修改失败");
            }
        }

        #endregion

        #region 修改用户授权

        /// <summary>
        /// 修改用户授权
        /// </summary>
        /// <param name="authorizeInfo">用户授权信息</param>
        /// <returns></returns>
        public Result ModifyUserAuthorize(ModifyUserAuthorizeCmdDto authorizeInfo)
        {
            if (authorizeInfo == null || authorizeInfo.UserAuthorizes.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定任何要修改的用户授权信息");
            }
            using (var businessWork = UnitOfWork.Create())
            {
                var modifyResult = AuthorizeService.ModifyUserAuthorize(authorizeInfo.UserAuthorizes.Select(c => c.MapTo<UserAuthorize>()));
                if (!modifyResult.Success)
                {
                    return modifyResult;
                }
                var commitResult = businessWork.Commit();
                return commitResult.ExecutedSuccess ? Result.SuccessResult("修改成功") : Result.FailedResult("修改失败");
            }
        }

        #endregion

        #region 授权验证

        /// <summary>
        /// 授权验证
        /// </summary>
        /// <param name="auth">授权验证信息</param>
        /// <returns></returns>
        public bool Authentication(AuthenticationCmdDto auth)
        {
            if (auth == null)
            {
                return false;
            }
            return AuthorizeService.Authentication(auth.MapTo<Authentication>());
        }

        #endregion

        #endregion

        #region 根据权限查询条件生成查询对象

        /// <summary>
        /// 根据权限查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IQuery CreateAuthorityQueryObject(AuthorityFilterDto filter, bool useBaseFilter = false)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = null;

            if (useBaseFilter)
            {
                query = QueryFactory.Create<AuthorityQuery>(filter);

                #region 数据筛选

                if (!filter.Codes.IsNullOrEmpty())
                {
                    query.In<AuthorityQuery>(c => c.Code, filter.Codes);
                }
                if (!filter.Name.IsNullOrEmpty())
                {
                    query.Equal<AuthorityQuery>(c => c.Name, filter.Name);
                }
                if (!filter.NameCodeMateKey.IsNullOrEmpty())
                {
                    query.And<AuthorityQuery>(QueryOperator.OR, CriteriaOperator.Like, filter.NameCodeMateKey,null, a => a.Code, a => a.Name);
                }
                if (filter.AuthType.HasValue)
                {
                    query.Equal<AuthorityQuery>(c => c.AuthType, filter.AuthType.Value);
                }
                if (filter.Status.HasValue)
                {
                    query.Equal<AuthorityQuery>(c => c.Status, filter.Status.Value);
                }
                if (filter.Sort.HasValue)
                {
                    query.Equal<AuthorityQuery>(c => c.Sort, filter.Sort.Value);
                }
                if (filter.AuthGroup.HasValue)
                {
                    query.Equal<AuthorityQuery>(c => c.AuthGroup, filter.AuthGroup.Value);
                }
                if (!filter.Remark.IsNullOrEmpty())
                {
                    query.Equal<AuthorityQuery>(c => c.Remark, filter.Remark);
                }
                //if (!filter.Application.IsNullOrEmpty())
                //{
                //    query.Equal<AuthorityQuery>(c => c.Application, filter.Application);
                //}
                if (filter.CreateDate.HasValue)
                {
                    query.Equal<AuthorityQuery>(c => c.CreateDate, filter.CreateDate.Value);
                }

                #endregion

                #region 数据加载

                if (filter.LoadGroup)
                {
                    query.SetLoadPropertys<Authority>(true, c => c.AuthGroup);
                }

                #endregion
            }
            else
            {
                if (filter is AuthorityOperationBindAuthorityFilterDto)
                {
                    query = CreateAuthorityQueryObject(filter as AuthorityOperationBindAuthorityFilterDto);//操作授权
                }
                else if (filter is RoleAuthorityFilterDto)
                {
                    query = CreateAuthorityQueryObject(filter as RoleAuthorityFilterDto);//角色授权
                }
                else if (filter is UserAuthorityFilterDto)
                {
                    query = CreateAuthorityQueryObject(filter as UserAuthorityFilterDto);//用户授权
                }
                else
                {
                    query = CreateAuthorityQueryObject(filter, true);
                }
            }

            return query;
        }

        /// <summary>
        /// 授权操作绑定权限筛选
        /// </summary>
        /// <param name="filter">筛选对象</param>
        /// <returns></returns>
        IQuery CreateAuthorityQueryObject(AuthorityOperationBindAuthorityFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = CreateAuthorityQueryObject(filter, true) ?? QueryFactory.Create<AuthorityQuery>();

            #region 授权操作筛选

            if (filter.AuthorityOperationFilter != null)
            {
                IQuery operationQuery = CreateAuthorityOperationQueryObject(filter.AuthorityOperationFilter);
                if (operationQuery != null)
                {
                    operationQuery.AddQueryFields<AuthorityOperationQuery>(c => c.SysNo);
                    IQuery authBindOperationQuery = QueryFactory.Create<AuthorityBindOperationQuery>();
                    authBindOperationQuery.And<AuthorityBindOperationQuery>(c => c.AuthorithOperation, CriteriaOperator.In, operationQuery);
                    authBindOperationQuery.AddQueryFields<AuthorityBindOperationQuery>(c => c.AuthorityCode);
                    query.And<AuthorityQuery>(c => c.Code, CriteriaOperator.In, authBindOperationQuery);
                }
            }

            #endregion

            return query;
        }

        /// <summary>
        /// 角色权限筛选
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        IQuery CreateAuthorityQueryObject(RoleAuthorityFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = CreateAuthorityQueryObject(filter, true) ?? QueryFactory.Create<AuthorityQuery>();

            #region 角色授权筛选

            if (filter.RoleFilter != null)
            {
                IQuery roleFilter = this.Instance<IRoleBusiness>().CreateQueryObject(filter.RoleFilter);
                if (roleFilter != null)
                {
                    roleFilter.AddQueryFields<RoleQuery>(c => c.SysNo);
                    IQuery roleBindAuthQuery = QueryFactory.Create<RoleAuthorizeQuery>();
                    roleBindAuthQuery.And<RoleAuthorizeQuery>(c => c.Role, CriteriaOperator.In, roleFilter);
                    roleBindAuthQuery.AddQueryFields<RoleAuthorizeQuery>(c => c.Authority);
                    query.And<AuthorityQuery>(c => c.Code, CriteriaOperator.In, roleBindAuthQuery);
                }
            }

            #endregion

            return query;
        }

        /// <summary>
        /// 用户授权筛选
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        IQuery CreateAuthorityQueryObject(UserAuthorityFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = CreateAuthorityQueryObject(filter, true) ?? QueryFactory.Create<AuthorityQuery>();

            #region 用户授权筛选

            if (filter.UserFilter != null)
            {
                IQuery userQuery = this.Instance<IUserBusiness>().CreateQueryObject(filter.UserFilter);
                if (userQuery != null)
                {
                    userQuery.AddQueryFields<UserQuery>(c => c.SysNo);

                    #region 用户或者角色绑定的授权

                    IQuery userOrRoleBindAuthQuery = QueryFactory.Create();

                    //用户授权
                    IQuery userAuthBindQuery = QueryFactory.Create<UserAuthorizeQuery>(c => c.Disable == false);
                    userAuthBindQuery.And<UserAuthorizeQuery>(c => c.User, CriteriaOperator.In, userQuery);
                    userAuthBindQuery.AddQueryFields<UserAuthorizeQuery>(c => c.Authority);
                    IQuery userAuthQuery = QueryFactory.Create<AuthorityQuery>();
                    userAuthQuery.And<AuthorityQuery>(c => c.Code, CriteriaOperator.In, userAuthBindQuery);
                    userOrRoleBindAuthQuery.And(userAuthQuery);
                    //角色授权
                    IQuery userRoleBindQuery = QueryFactory.Create<UserRoleQuery>();
                    userRoleBindQuery.And<UserRoleQuery>(c => c.UserSysNo, CriteriaOperator.In, userQuery);
                    userRoleBindQuery.AddQueryFields<UserRoleQuery>(c => c.RoleSysNo);
                    IQuery roleAuthBindQuery = QueryFactory.Create<RoleAuthorizeQuery>();
                    roleAuthBindQuery.And<RoleAuthorizeQuery>(c => c.Role, CriteriaOperator.In, userRoleBindQuery);
                    roleAuthBindQuery.AddQueryFields<RoleAuthorizeQuery>(c => c.Authority);
                    IQuery roleAuthQuery = QueryFactory.Create<AuthorityQuery>();
                    roleAuthQuery.And<AuthorityQuery>(c => c.Code, CriteriaOperator.In, roleAuthBindQuery);
                    userOrRoleBindAuthQuery.Or(roleAuthQuery);

                    query.And(userOrRoleBindAuthQuery);

                    #endregion

                    //排除用户禁用的授权
                    IQuery userDisableAuthQuery = QueryFactory.Create<UserAuthorizeQuery>(c => c.Disable == true);
                    userDisableAuthQuery.And<UserAuthorizeQuery>(c => c.User, CriteriaOperator.In, userQuery);
                    userDisableAuthQuery.AddQueryFields<UserAuthorizeQuery>(c => c.Authority);
                    query.And<AuthorityQuery>(c => c.Code, CriteriaOperator.NotIn, userDisableAuthQuery);
                }
            }

            #endregion

            return query;
        }

        #endregion

        #region 根据权限分组查询条件生成查询对象

        /// <summary>
        /// 根据权限分组查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IQuery CreateAuthorityGroupQueryObject(AuthorityGroupFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = QueryFactory.Create<AuthorityGroupQuery>(filter);
            if (!filter.SysNos.IsNullOrEmpty())
            {
                query.In<AuthorityGroupQuery>(c => c.SysNo, filter.SysNos);
            }
            if (!filter.Name.IsNullOrEmpty())
            {
                query.Equal<AuthorityGroupQuery>(c => c.Name, filter.Name);
            }
            if (filter.SortIndex.HasValue)
            {
                query.Equal<AuthorityGroupQuery>(c => c.SortIndex, filter.SortIndex.Value);
            }
            if (filter.Status.HasValue)
            {
                query.Equal<AuthorityGroupQuery>(c => c.Status, filter.Status.Value);
            }
            if (filter.Parent.HasValue)
            {
                query.Equal<AuthorityGroupQuery>(c => c.Parent, filter.Parent.Value);
            }
            if (filter.Level.HasValue)
            {
                query.Equal<AuthorityGroupQuery>(c => c.Level, filter.Level.Value);
            }
            //if (!filter.Application.IsNullOrEmpty())
            //{
            //    query.Equal<AuthorityGroupQuery>(c => c.Application, filter.Application);
            //}
            if (!filter.Remark.IsNullOrEmpty())
            {
                query.Equal<AuthorityGroupQuery>(c => c.Remark, filter.Remark);
            }
            return query;
        }

        #endregion

        #region 根据授权操作分组查询条件生成查询对象

        /// <summary>
        /// 根据授权操作分组查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IQuery CreateAuthorityOperationGroupQueryObject(AuthorityOperationGroupFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = QueryFactory.Create<AuthorityOperationGroupQuery>(filter);
            if (!filter.SysNos.IsNullOrEmpty())
            {
                query.In<AuthorityOperationGroupQuery>(c => c.SysNo, filter.SysNos);
            }
            if (!filter.Name.IsNullOrEmpty())
            {
                query.Equal<AuthorityOperationGroupQuery>(c => c.Name, filter.Name);
            }
            if (filter.Sort.HasValue)
            {
                query.Equal<AuthorityOperationGroupQuery>(c => c.Sort, filter.Sort.Value);
            }
            if (filter.Parent.HasValue)
            {
                query.Equal<AuthorityOperationGroupQuery>(c => c.Parent, filter.Parent.Value);
            }
            if (filter.Root.HasValue)
            {
                query.Equal<AuthorityOperationGroupQuery>(c => c.Root, filter.Root.Value);
            }
            if (filter.Level.HasValue)
            {
                query.Equal<AuthorityOperationGroupQuery>(c => c.Level, filter.Level.Value);
            }
            if (filter.Status.HasValue)
            {
                query.Equal<AuthorityOperationGroupQuery>(c => c.Status, filter.Status.Value);
            }
            //if (!filter.Application.IsNullOrEmpty())
            //{
            //    query.Equal<AuthorityOperationGroupQuery>(c => c.Application, filter.Application);
            //}
            if (!filter.Remark.IsNullOrEmpty())
            {
                query.Equal<AuthorityOperationGroupQuery>(c => c.Remark, filter.Remark);
            }
            return query;
        }

        #endregion

        #region 根据授权操作查询条件生成查询对象

        /// <summary>
        /// 根据授权操作查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IQuery CreateAuthorityOperationQueryObject(AuthorityOperationFilterDto filter, bool useBaseFilter = false)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = null;

            if (useBaseFilter)
            {
                query = QueryFactory.Create<AuthorityOperationQuery>(filter);

                #region 数据筛选

                if (!filter.SysNos.IsNullOrEmpty())
                {
                    query.In<AuthorityOperationQuery>(c => c.SysNo, filter.SysNos);
                }
                if (!filter.OperationMateKey.IsNullOrEmpty())
                {
                    query.And<AuthorityOperationQuery>(QueryOperator.OR, CriteriaOperator.Like, filter.OperationMateKey,null, u => u.Name, u => u.ControllerCode, u => u.ActionCode);
                }
                if (!filter.ControllerCode.IsNullOrEmpty())
                {
                    query.Equal<AuthorityOperationQuery>(c => c.ControllerCode, filter.ControllerCode);
                }
                if (!filter.ActionCode.IsNullOrEmpty())
                {
                    query.Equal<AuthorityOperationQuery>(c => c.ActionCode, filter.ActionCode);
                }
                if (filter.Method.HasValue)
                {
                    query.Equal<AuthorityOperationQuery>(c => c.Method, filter.Method.Value);
                }
                if (!filter.Name.IsNullOrEmpty())
                {
                    query.Equal<AuthorityOperationQuery>(c => c.Name, filter.Name);
                }
                if (filter.Status.HasValue)
                {
                    query.Equal<AuthorityOperationQuery>(c => c.Status, filter.Status.Value);
                }
                if (filter.Sort.HasValue)
                {
                    query.Equal<AuthorityOperationQuery>(c => c.Sort, filter.Sort.Value);
                }
                if (filter.Group.HasValue)
                {
                    query.Equal<AuthorityOperationQuery>(c => c.Group, filter.Group.Value);
                }
                //if (filter.AuthorizeType.HasValue)
                //{
                //    query.Equal<AuthorityOperationQuery>(c => c.AuthorizeType, filter.AuthorizeType.Value);
                //}
                //if (!filter.Application.IsNullOrEmpty())
                //{
                //    query.Equal<AuthorityOperationQuery>(c => c.Application, filter.Application);
                //}
                if (!filter.Remark.IsNullOrEmpty())
                {
                    query.Equal<AuthorityOperationQuery>(c => c.Remark, filter.Remark);
                }

                #endregion

                #region 数据加载

                if (filter.LoadGroup)
                {
                    query.SetLoadPropertys<AuthorityOperation>(true, c => c.Group);
                }

                #endregion
            }
            else
            {
                if (filter is AuthorityBindOperationFilterDto)
                {
                    query = CreateAuthorityOperationQueryObject(filter as AuthorityBindOperationFilterDto);
                }
                else
                {
                    query = CreateAuthorityOperationQueryObject(filter, true);
                }
            }
            return query;
        }

        /// <summary>
        /// 权限绑定操作筛选
        /// </summary>
        /// <param name="filter">筛选信息</param>
        /// <returns></returns>
        IQuery CreateAuthorityOperationQueryObject(AuthorityBindOperationFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = CreateAuthorityOperationQueryObject(filter, true) ?? QueryFactory.Create<AuthorityOperationQuery>();

            #region 权限筛选

            if (filter.AuthorityFilter != null)
            {
                IQuery authorityQuery = CreateAuthorityQueryObject(filter.AuthorityFilter);
                if (authorityQuery != null && authorityQuery.Criterias.Count > 0)
                {
                    authorityQuery.AddQueryFields<AuthorityQuery>(c => c.Code);
                    IQuery authBindOperationQuery = QueryFactory.Create<AuthorityBindOperationQuery>();
                    authBindOperationQuery.And<AuthorityBindOperationQuery>(c => c.AuthorityCode, CriteriaOperator.In, authorityQuery);
                    authBindOperationQuery.AddQueryFields<AuthorityBindOperationQuery>(c => c.AuthorithOperation);
                    query.And<AuthorityOperationQuery>(c => c.SysNo, CriteriaOperator.In, authBindOperationQuery);
                }
            }

            #endregion

            return query;
        }

        #endregion
    }
}
