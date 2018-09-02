using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Domain.Sys.Repository;
using MicBeach.Util.Extension;
using MicBeach.Domain.Sys.Service;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Sys;
using MicBeach.Util.Data;
using MicBeach.Util;
using System.Linq.Expressions;
using MicBeach.Util.ExpressionUtil;
using MicBeach.Application.Identity.User;

namespace MicBeach.Domain.Sys.Model
{
    /// <summary>
    /// 管理用户
    /// </summary>
    public class AdminUser : User
    {
        #region 字段

        /// <summary>
        /// 管理账户存储
        /// </summary>
        IUserRepository userRepository;

        /// <summary>
        /// 用户角色
        /// </summary>
        LazyMember<List<Role>> _roleList = null;

        #endregion

        #region 构造方法

        /// <summary>
        /// 初始化管理账户
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="roleList">用户角色</param>
        /// <param name="superUser">是否为超级管理员</param>

        internal AdminUser(long userId = 0, string userName = "", string pwd = "", string realName = "", List<Role> roleList = null, bool superUser = false) : base(userId, userName, pwd, realName)
        {
            _userType = UserType.管理账户;
            _superUser = superUser;
            _roleList = new LazyMember<List<Role>>(LoadRoles);
            AddRoles(roleList);
            userRepository = this.Instance<IUserRepository>();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 用户角色
        /// </summary>
        public List<Role> Roles
        {
            get
            {
                return _roleList.Value;
            }
            protected set
            {
                _roleList.SetValue(value, false);
            }
        }

        #endregion

        #region 方法

        #region 功能行为

        #region 添加角色

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleList">角色信息</param>
        public void AddRoles(IEnumerable<Role> roleList)
        {
            if (roleList.IsNullOrEmpty())
            {
                return;
            }
            if (_roleList.Value != null)
            {
                _roleList.Value.AddRange(roleList);
            }
            else
            {
                _roleList.SetValue(roleList.ToList(), true);
            }
        }

        #endregion

        #region 移除角色

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="roleList">角色信息</param>
        public void RemoveRoles(IEnumerable<Role> roleList)
        {
            if (roleList.IsNullOrEmpty())
            {
                return;
            }
            var nowRoleList = _roleList.Value;
            if (nowRoleList.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<long> removeRoleSysNos = roleList.Select(c => c.SysNo);
            nowRoleList.ForEach(c =>
            {
                if (removeRoleSysNos.Contains(c.SysNo))
                {
                    c.MarkRemove();
                }
            });
        }

        #endregion

        #region 设置角色

        /// <summary>
        /// 设置用户角色值
        /// </summary>
        /// <param name="roles">角色信息</param>
        /// <param name="init">是否初始化，设置为初始化后将不会再自动加载信息</param>
        public void SetRoles(IEnumerable<Role> roles, bool init = true)
        {
            _roleList.SetValue(roles == null ? null : roles.ToList(), init);
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存管理用户
        /// </summary>
        public override void Save()
        {
            userRepository.Save(this);
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除管理账户
        /// </summary>
        public override void Remove()
        {
            userRepository.Remove(this);
        }

        #endregion

        #region 根据给定的对象更新当前信息

        /// <summary>
        /// 根据给定的对象更新当前信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="excludePropertys">排除更新的属性</param>
        public override void ModifyFromOtherUser(User user, IEnumerable<string> excludePropertys = null)
        {
            base.ModifyFromOtherUser(user, excludePropertys);

            if (user == null || !(user is AdminUser))
            {
                return;
            }
            var adminUser = user as AdminUser;
            excludePropertys = excludePropertys ?? new List<string>(0);

            #region 修改值

            if (!excludePropertys.Contains("Roles"))
            {
                SetRoles(adminUser.Roles, true);
            }

            #endregion
        }

        #endregion

        #endregion

        #region 内部行为

        #region 加载当前用户角色

        /// <summary>
        /// 加载当前用户角色
        /// </summary>
        /// <returns></returns>
        List<Role> LoadRoles()
        {
            if (!AllowLazyLoad("Roles"))
            {
                return _roleList.CurrentValue ?? new List<Role>(0);
            }
            return RoleService.GetUserBindRole(_sysNo);
        }

        #endregion

        #endregion

        #region 静态方法

        #region 创建管理用户

        /// <summary>
        /// 创建一个管理账号对象
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="realName">真实名称</param>
        /// <param name="roleList">角色列表</param>
        /// <param name="superUser">是否为超级用户</param>
        /// <returns>管理账号对象</returns>
        public static AdminUser CreateNewAdminUser(long userId, string userName = "", string pwd = "", string realName = "", List<Role> roleList = null, bool superUser = false)
        {
            return new AdminUser(userId, userName, pwd, realName, roleList, superUser);
        }

        #endregion

        #endregion

        #endregion
    }
}
