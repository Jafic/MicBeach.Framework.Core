using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Domain.Sys.Repository;
using MicBeach.Util.Extension;
using MicBeach.Util.Data;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Sys;

namespace MicBeach.Domain.Sys.Model
{
    /// <summary>
    /// 用户授权
    /// </summary>
    public class UserAuthorize : AggregationRoot<UserAuthorize>
    {
        IUserAuthorizeRepository userAuthorityRepository = null;

        #region	字段

        /// <summary>
        /// 用户
        /// </summary>
        protected LazyMember<User> _user;

        /// <summary>
        /// 权限
        /// </summary>
        protected LazyMember<Authority> _authority;

        /// <summary>
        /// 禁用
        /// </summary>
        protected bool _disable;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化用户授权对象
        /// </summary>
        internal UserAuthorize()
        {
            _user = new LazyMember<User>(LoadUser);
            _authority = new LazyMember<Authority>(LoadAuthority);
            userAuthorityRepository = this.Instance<IUserAuthorizeRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 用户
        /// </summary>
        public User User
        {
            get
            {
                return _user.Value;
            }
            protected set
            {
                _user.SetValue(value, false);
            }
        }

        /// <summary>
        /// 权限
        /// </summary>
        public Authority Authority
        {
            get
            {
                return _authority.Value;
            }
            protected set
            {
                _authority.SetValue(value, false);
            }
        }

        /// <summary>
        /// 禁用
        /// </summary>
        public bool Disable
        {
            get
            {
                return _disable;
            }
            protected set
            {
                _disable = value;
            }
        }

        #endregion

        #region 方法

        #region 功能方法

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        public override void Save()
        {
            userAuthorityRepository.Save(this);
        }

        #endregion

        #region	移除

        /// <summary>
        /// 移除
        /// </summary>
        public override void Remove()
        {
            userAuthorityRepository.Remove(this);
        }

        #endregion

        #endregion

        #region 内部方法

        #region 加载用户

        /// <summary>
        /// 加载用户
        /// </summary>
        /// <returns></returns>
        User LoadUser()
        {
            if (!AllowLazyLoad(r => r.User))
            {
                return _user.CurrentValue;
            }
            if (_user.CurrentValue == null)
            {
                return null;
            }
            return this.Instance<IUserRepository>().Get(QueryFactory.Create<UserQuery>(r => r.SysNo == _user.CurrentValue.SysNo));
        }

        #endregion

        #region 加载权限

        /// <summary>
        /// 加载权限
        /// </summary>
        /// <returns></returns>
        Authority LoadAuthority()
        {
            if (!AllowLazyLoad(r => r.Authority))
            {
                return _authority.CurrentValue;
            }
            if (_authority.CurrentValue == null)
            {
                return null;
            }
            return this.Instance<IAuthorityRepository>().Get(QueryFactory.Create<AuthorityQuery>(r => r.Code == _authority.CurrentValue.Code));
        }

        #endregion

        #region 主标识值是否为空

        /// <summary>
        /// 主标识值是否为空
        /// </summary>
        /// <returns></returns>
        public override bool PrimaryValueIsNone()
        {
            return _authority.Value == null || _user.Value == null;
        }

        #endregion

        #endregion

        #region 静态方法

        #region 创建用户授权

        /// <summary>
        /// 创建一个用户授权对象
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="authority">权限</param>
        /// <param name="disable">禁用权限</param>
        /// <returns></returns>
        public static UserAuthorize CreateUserAuthority(User user, Authority authority, bool disable = false)
        {
            return new UserAuthorize()
            {
                User = user,
                Authority = authority,
                Disable = disable
            };
        }

        #endregion

        #endregion

        #endregion
    }
}