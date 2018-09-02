using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using MicBeach.Application.Identity.User;
using MicBeach.Domain.Sys.Model;
using MicBeach.Domain.Sys.Service.Parameter;
using MicBeach.DTO.Sys.Cmd;
using MicBeach.DTO.Sys.Query;
using MicBeach.DTO.Sys.Query.Filter;
using MicBeach.Entity.Sys;
using MicBeach.Util.Data;
using MicBeach.Util.Extension;
using MicBeach.Util.ObjectMap;
using MicBeach.Util.Serialize;
using MicBeach.ViewModel.Sys;
using MicBeach.ViewModel.Sys.Filter;

namespace App.Mapper
{
    public class AutoMapMapper : IObjectMap
    {
        /// <summary>
        /// 转换对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="sourceObj">源对象类型</param>
        /// <returns>目标对象类型</returns>
        public T MapTo<T>(object sourceObj)
        {
            return AutoMapper.Mapper.Map<T>(sourceObj);
        }

        /// <summary>
        /// /// <summary>
        /// 注册对象映射
        /// </summary>
        /// </summary>
        public void Register()
        {
            var cfg = new MapperConfigurationExpression();
            cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly || p.GetMethod.IsPrivate || p.GetMethod.IsFamilyAndAssembly || p.GetMethod.IsFamily || p.GetMethod.IsFamilyOrAssembly;

            #region Sys

            #region User

            cfg.CreateMap<UserEntity, User>().ForMember(u => u.Contact, u => u.ResolveUsing(ue => new Contact(mobile: ue.Mobile, email: ue.Email, qq: ue.QQ)));
            cfg.CreateMap<User, UserEntity>().ForMember(u => u.Email, u => u.ResolveUsing(ue => ue.Contact.Email)).ForMember(u => u.Mobile, u => u.ResolveUsing(ue => ue.Contact.Mobile)).ForMember(u => u.QQ, u => u.ResolveUsing(ue => ue.Contact.QQ));
            cfg.CreateMap<User, AdminUser>();
            //admin user
            cfg.CreateMap<AdminUser, AdminUserDto>();
            cfg.CreateMap<AdminUserDto, AdminUserViewModel>().ForMember(u => u.Email, u => u.ResolveUsing(ue => ue.Contact.Email)).ForMember(u => u.Mobile, u => u.ResolveUsing(ue => ue.Contact.Mobile)).ForMember(u => u.QQ, u => u.ResolveUsing(ue => ue.Contact.QQ));
            cfg.CreateMap<AdminUserViewModel, AdminUserCmdDto>().ForMember(u => u.Contact, u => u.ResolveUsing(ue => new Contact(mobile: ue.Mobile, email: ue.Email, qq: ue.QQ)));
            cfg.CreateMap<AdminUserCmdDto, AdminUser>();
            cfg.CreateMap<UserViewModel, AdminUserViewModel>();

            cfg.CreateMap<User, UserDto>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                UserDto user = null;
                switch (c.UserType)
                {
                    case UserType.管理账户:
                        user = ((AdminUser)c).MapTo<AdminUserDto>();
                        break;
                }
                return user;
            });

            cfg.CreateMap<UserCmdDto, User>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                User user = null;
                switch (c.UserType)
                {
                    case UserType.管理账户:
                        user = ((AdminUserCmdDto)c).MapTo<AdminUser>();
                        break;
                }
                return user;
            });

            cfg.CreateMap<UserDto, UserViewModel>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                UserViewModel user = null;
                switch (c.UserType)
                {
                    case UserType.管理账户:
                        user = ((AdminUserDto)c).MapTo<AdminUserViewModel>();
                        break;
                }
                return user;
            });

            cfg.CreateMap<UserViewModel, UserCmdDto>().ConvertUsing(c =>
            {
                if (c == null)
                {
                    return null;
                }
                UserCmdDto user = null;
                switch (c.UserType)
                {
                    case UserType.管理账户:
                        user = ((AdminUserViewModel)c).MapTo<AdminUserCmdDto>();
                        break;
                }
                return user;
            });

            //cfg.CreateMap<User, UserDto>();
            //cfg.CreateMap<UserEntity, AdminUser>().ForMember(u => u.Contact, u => u.ResolveUsing(ue => new Contact(mobile: ue.Mobile, email: ue.Email, qq: ue.QQ)));
            //cfg.CreateMap<AdminUser, UserEntity>().ForMember(u => u.Email, u => u.ResolveUsing(ue => ue.Contact.Email)).ForMember(u => u.Mobile, u => u.ResolveUsing(ue => ue.Contact.Mobile)).ForMember(u => u.QQ, u => u.ResolveUsing(ue => ue.Contact.QQ)); ;
            //cfg.CreateMap<AdminUser, UserDto>();
            //cfg.CreateMap<AdminUser, AdminUserDto>().ForMember(a => a.User, a => a.MapFrom(c => c));
            //cfg.CreateMap<UserDto, AdminUserViewModel>().ForMember(u => u.Email, u => u.ResolveUsing(ue => ue.Contact.Email)).ForMember(u => u.Mobile, u => u.ResolveUsing(ue => ue.Contact.Mobile)).ForMember(u => u.QQ, u => u.ResolveUsing(ue => ue.Contact.QQ));
            //cfg.CreateMap<AdminUserDto, AdminUserViewModel>().ConvertUsing(r =>
            //{
            //    var viewModel = r.User.MapTo<AdminUserViewModel>();
            //    viewModel.Roles = (r.Roles ?? new List<RoleDto>(0)).Select(c => c.MapTo<RoleViewModel>()).ToList();
            //    return viewModel;
            //});
            //cfg.CreateMap<ModifyPasswordViewModel, ModifyPasswordCmdDto>();
            //cfg.CreateMap<AdminUserViewModel, UserCmdDto>().ForMember(u => u.Contact, u => u.ResolveUsing(ue => new Contact(mobile: ue.Mobile, email: ue.Email, qq: ue.QQ)));
            //cfg.CreateMap<UserCmdDto, AdminUser>();

            cfg.CreateMap<UserFilterViewModel, UserFilterDto>();
            cfg.CreateMap<AdminUserFilterViewModel, AdminUserFilterDto>();

            cfg.CreateMap<ModifyPasswordViewModel, ModifyPasswordCmdDto>();
            cfg.CreateMap<ModifyPasswordCmdDto, ModifyUserPassword>();

            #endregion

            #region Role

            cfg.CreateMap<RoleEntity, Role>().ForMember(r => r.Parent, r => r.ResolveUsing(re => { return Role.CreateRole(re.Parent); }));
            cfg.CreateMap<Role, RoleEntity>().ForMember(re => re.Parent, r => r.MapFrom(rs => rs.Parent.SysNo));
            cfg.CreateMap<Role, RoleDto>();
            cfg.CreateMap<RoleDto, Role>();
            cfg.CreateMap<RoleCmdDto, Role>();
            cfg.CreateMap<RoleViewModel, RoleCmdDto>();
            cfg.CreateMap<RoleDto, RoleViewModel>();
            cfg.CreateMap<RoleFilterViewModel, RoleFilterDto>();
            cfg.CreateMap<ModifyRoleAuthorizeCmdDto, ModifyRoleAuthorize>()
                .ForMember(c => c.Binds, ce => ce.MapFrom(cs => cs.Binds.Select(cm => new Tuple<Role, Authority>(cm.Item1.MapTo<Role>(), cm.Item2.MapTo<Authority>()))))
                .ForMember(c => c.UnBinds, ce => ce.MapFrom(cs => cs.UnBinds.Select(cm => new Tuple<Role, Authority>(cm.Item1.MapTo<Role>(), cm.Item2.MapTo<Authority>()))));

            #endregion

            #region AuthorityGroup

            cfg.CreateMap<AuthorityGroup, AuthorityGroupEntity>().ForMember(r => r.Parent, re => re.MapFrom(rs => rs.Parent.SysNo));
            cfg.CreateMap<AuthorityGroupEntity, AuthorityGroup>().ForMember(re => re.Parent, r => r.ResolveUsing(re => { return AuthorityGroup.CreateAuthorityGroup(re.Parent); }));
            cfg.CreateMap<AuthorityGroup, AuthorityGroupDto>();
            cfg.CreateMap<AuthorityGroupCmdDto, AuthorityGroup>();
            cfg.CreateMap<AuthorityGroupDto, AuthorityGroupViewModel>();
            cfg.CreateMap<AuthorityGroupViewModel, AuthorityGroupCmdDto>();
            cfg.CreateMap<AuthorityGroupViewModel, SaveAuthorityGroupCmdDto>().ForMember(a => a.AuthorityGroup, a => a.MapFrom(c => c));

            #endregion

            #region Authority

            cfg.CreateMap<Authority, AuthorityEntity>().ForMember(c => c.AuthGroup, re => re.MapFrom(rs => rs.AuthGroup.SysNo));
            cfg.CreateMap<AuthorityEntity, Authority>().ForMember(c => c.AuthGroup, re => re.ResolveUsing(rs => { return AuthorityGroup.CreateAuthorityGroup(rs.AuthGroup); }));
            cfg.CreateMap<Authority, AuthorityDto>();
            cfg.CreateMap<AuthorityCmdDto, Authority>();
            cfg.CreateMap<AuthorityDto, AuthorityViewModel>();
            cfg.CreateMap<AuthorityViewModel, AuthorityCmdDto>();
            cfg.CreateMap<AuthorityFilterViewModel, AuthorityFilterDto>();
            cfg.CreateMap<AuthorityDto, AuthorizationAuthorityViewModel>();

            #endregion

            #region AuthorityOperationGroup

            cfg.CreateMap<AuthorityOperationGroup, AuthorityOperationGroupEntity>().ForMember(r => r.Parent, re => re.MapFrom(rs => rs.Parent.SysNo)).ForMember(r => r.Root, re => re.MapFrom(rs => rs.Root.SysNo));
            cfg.CreateMap<AuthorityOperationGroupEntity, AuthorityOperationGroup>().ForMember(re => re.Parent, r => r.ResolveUsing(re => { return AuthorityOperationGroup.CreateAuthorityOperationGroup(re.Parent); })).ForMember(re => re.Root, r => r.ResolveUsing(re => { return AuthorityOperationGroup.CreateAuthorityOperationGroup(re.Root); }));
            cfg.CreateMap<AuthorityOperationGroup, AuthorityOperationGroupDto>();
            cfg.CreateMap<AuthorityOperationGroupCmdDto, AuthorityOperationGroup>();
            cfg.CreateMap<AuthorityOperationGroupDto, AuthorityOperationGroupViewModel>();
            cfg.CreateMap<AuthorityOperationGroupViewModel, AuthorityOperationGroupCmdDto>();

            #endregion

            #region AuthorityOperation

            cfg.CreateMap<AuthorityOperation, AuthorityOperationEntity>().ForMember(c => c.Group, re => re.MapFrom(rs => rs.Group.SysNo));
            cfg.CreateMap<AuthorityOperationEntity, AuthorityOperation>().ForMember(c => c.Group, re => re.ResolveUsing(rs => { return AuthorityOperationGroup.CreateAuthorityOperationGroup(rs.Group); }));
            cfg.CreateMap<AuthorityOperation, AuthorityOperationDto>();
            cfg.CreateMap<AuthorityOperationCmdDto, AuthorityOperation>();
            cfg.CreateMap<AuthorityOperationDto, AuthorityOperationViewModel>();
            cfg.CreateMap<AuthorityOperationViewModel, AuthorityOperationCmdDto>();
            cfg.CreateMap<AuthorityOperationFilterViewModel, AuthorityOperationFilterDto>();

            #endregion

            #region AuthorityBinOperation

            cfg.CreateMap<ModifyAuthorityBindAuthorityOperationCmdDto, ModifyAuthorityAndAuthorityOperationBind>()
                .ForMember(c => c.Binds, ce => ce.MapFrom(cs => cs.Binds.Select(cm => new Tuple<Authority, AuthorityOperation>(cm.Item1.MapTo<Authority>(), cm.Item2.MapTo<AuthorityOperation>()))))
                .ForMember(c => c.UnBinds, ce => ce.MapFrom(cs => cs.UnBinds.Select(cm => new Tuple<Authority, AuthorityOperation>(cm.Item1.MapTo<Authority>(), cm.Item2.MapTo<AuthorityOperation>()))));
            cfg.CreateMap<AuthorityBindOperationFilterViewModel, AuthorityBindOperationFilterDto>();
            cfg.CreateMap<AuthorityOperationBindAuthorityFilterViewModel, AuthorityOperationBindAuthorityFilterDto>();

            #endregion

            #region UserAuthorize

            cfg.CreateMap<UserAuthorizeCmdDto, UserAuthorize>();
            cfg.CreateMap<UserAuthorize, UserAuthorizeEntity>().ForMember(c => c.User, ce => ce.MapFrom(cs => cs.User.SysNo)).ForMember(c => c.Authority, ce => ce.MapFrom(cs => cs.Authority.Code));
            cfg.CreateMap<UserAuthorizeEntity, UserAuthorize>().ForMember(c => c.User, ce => ce.ResolveUsing(cs => { return User.CreateUser(cs.User); })).ForMember(c => c.Authority, ce => ce.ResolveUsing(cs => Authority.CreateAuthority(cs.Authority)));

            #endregion

            #region Authentication

            cfg.CreateMap<AuthenticationCmdDto, Authentication>();

            #endregion

            #endregion

            AutoMapper.Mapper.Initialize(cfg);
        }
    }
}
