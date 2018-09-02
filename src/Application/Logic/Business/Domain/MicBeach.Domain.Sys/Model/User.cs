using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Domain.Sys.Repository;
using MicBeach.Util.Extension;
using MicBeach.Develop.CQuery;
using MicBeach.Util.Data;
using MicBeach.Util;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using MicBeach.Util.ExpressionUtil;
using MicBeach.Application.Identity.User;
using MicBeach.Application.Identity;
using MicBeach.Util.Code;

namespace MicBeach.Domain.Sys.Model
{
    /// <summary>
    /// 管理用户
    /// </summary>
    public class User : AggregationRoot<User>
    {
        #region	字段

        /// <summary>
        /// 用户编号
        /// </summary>
        protected long _sysNo;

        /// <summary>
        /// 用户名
        /// </summary>
        protected string _userName;

        /// <summary>
        /// 真实名称
        /// </summary>
        protected string _realName;

        /// <summary>
        /// 密码
        /// </summary>
        protected string _pwd;

        /// <summary>
        /// 类型
        /// </summary>
        protected UserType _userType;

        /// <summary>
        /// 超级用户
        /// </summary>
        protected bool _superUser = false;

        /// <summary>
        /// 状态
        /// </summary>
        protected UserStatus _status;

        /// <summary>
        /// 联系方式
        /// </summary>
        protected Contact _contact;

        /// <summary>
        /// 添加时间
        /// </summary>
        protected DateTime _createDate;

        /// <summary>
        /// 最近登录时间
        /// </summary>
        protected DateTime _lastLoginDate;

        /// <summary>
        /// 用户存储
        /// </summary>
        IUserRepository userRepository = null;

        #endregion

        #region 构造方法

        protected User(long userId = 0, string userName = "", string pwd = "", string realName = "")
        {
            _sysNo = userId;
            _userName = userName;
            _realName = realName;
            SetPassword(pwd);
            Initialization();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 用户编号
        /// </summary>
        public long SysNo
        {
            get
            {
                return _sysNo;
            }
            protected set
            {
                _sysNo = value;
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        /// <summary>
        /// 真实名称
        /// </summary>
        public string RealName
        {
            get
            {
                return _realName;
            }
            set
            {
                _realName = value;
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            get
            {
                return _pwd;
            }
            protected set
            {
                _pwd = value;
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public UserType UserType
        {
            get
            {
                return _userType;
            }
            protected set
            {
                _userType = value;
            }
        }

        /// <summary>
        /// 超级用户
        /// </summary>
        public bool SuperUser
        {
            get
            {
                return _superUser;
            }
            protected set
            {
                _superUser = value;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public UserStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        /// <summary>
        /// 联系方式
        /// </summary>
        public Contact Contact
        {
            get { return _contact; }
            set { _contact = value; }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateDate
        {
            get
            {
                return _createDate;
            }
            protected set
            {
                _createDate = value;
            }
        }

        /// <summary>
        /// 最近登录时间
        /// </summary>
        public DateTime LastLoginDate
        {
            get
            {
                return _lastLoginDate;
            }
            protected set
            {
                _lastLoginDate = value;
            }
        }

        #endregion

        #region 方法

        #region 功能行为

        #region 修改密码

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="newPassword">信息的密码</param>
        public void ModifyPassword(string newPassword)
        {
            SetPassword(newPassword);
        }

        #endregion

        #region 保存用户

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="user">用户信息</param>
        public override void Save()
        {
            userRepository.Save(this);
        }

        #endregion

        #region 移除用户

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="user">用户信息</param>
        public override void Remove()
        {
            userRepository.Remove(this);
        }

        #endregion

        #region 执行用户密码加密

        /// <summary>
        /// 执行用户密码加密
        /// </summary>
        /// <param name="pwdValue">密码值</param>
        /// <returns></returns>
        public static string PasswordEncryption(string pwdValue)
        {
            return pwdValue.MD5();
        }

        #endregion

        #region 当前账号是否允许登陆

        /// <summary>
        /// 当前账号是否允许登陆
        /// </summary>
        /// <returns></returns>
        public bool AllowLogin()
        {
            return _status == UserStatus.正常;
        }

        #endregion

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitPrimaryValue()
        {
            base.InitPrimaryValue();
            _sysNo = GenerateUserId();
        }

        #endregion

        #region 根据给定的对象更新当前信息

        /// <summary>
        /// 根据给定的对象更新当前信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="excludePropertys">排除更新的属性</param>
        public virtual void ModifyFromOtherUser(User user, IEnumerable<string> excludePropertys = null)
        {
            if (user == null)
            {
                return;
            }
            CopyDataFromSimilarObject(user, excludePropertys);
        }

        #endregion

        #endregion

        #region 内部行为

        #region 设置用户密码

        /// <summary>
        /// 设置用户密码
        /// </summary>
        /// <param name="newPwd">新的密码</param>
        void SetPassword(string newPwd)
        {
            _pwd = PasswordEncryption(newPwd);
        }

        #endregion

        #region 创建初始化信息

        /// <summary>
        /// 创建初始化信息
        /// </summary>
        protected void Initialization()
        {
            _createDate = DateTime.Now;
            _lastLoginDate = DateTime.Now;
            _status = UserStatus.正常;
            userRepository = this.Instance<IUserRepository>();
        }

        #endregion

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        public override bool PrimaryValueIsNone()
        {
            return _sysNo <= 0;
        }

        #endregion

        #region 从指定对象复制值

        /// <summary>
        /// 从指定对象复制值
        /// </summary>
        /// <typeparam name="DT">数据类型</typeparam>
        /// <param name="similarObject">数据对象</param>
        /// <param name="excludePropertys">排除不复制的属性</param>
        protected override void CopyDataFromSimilarObject<DT>(DT similarObject, IEnumerable<string> excludePropertys = null)
        {
            base.CopyDataFromSimilarObject<DT>(similarObject, excludePropertys);
            if (similarObject == null)
            {
                return;
            }
            excludePropertys = excludePropertys ?? new List<string>(0);
            #region 复制值

            if (!excludePropertys.Contains("SysNo"))
            {
                SysNo = similarObject.SysNo;
            }
            if (!excludePropertys.Contains("UserName"))
            {
                UserName = similarObject.UserName;
            }
            if (!excludePropertys.Contains("RealName"))
            {
                RealName = similarObject.RealName;
            }
            if (!excludePropertys.Contains("Pwd"))
            {
                Pwd = similarObject.Pwd;
            }
            if (!excludePropertys.Contains("UserType"))
            {
                UserType = similarObject.UserType;
            }
            if (!excludePropertys.Contains("SuperUser"))
            {
                SuperUser = similarObject.SuperUser;
            }
            if (!excludePropertys.Contains("Status"))
            {
                Status = similarObject.Status;
            }
            if (!excludePropertys.Contains("Contact"))
            {
                Contact = similarObject.Contact;
            }
            if (!excludePropertys.Contains("CreateDate"))
            {
                CreateDate = similarObject.CreateDate;
            }
            if (!excludePropertys.Contains("LastLoginDate"))
            {
                LastLoginDate = similarObject.LastLoginDate;
            }

            #endregion
        }

        #endregion

        #endregion

        #region 静态方法

        #region 生成用户编号

        /// <summary>
        /// 生成用户编号
        /// </summary>
        /// <returns></returns>
        public static long GenerateUserId()
        {
            return SerialNumber.GetSerialNumber(AppIdentityUtil.GetIdGroupCode(IdentityGroup.用户));
        }

        #endregion

        #region 创建用户对象

        /// <summary>
        /// 生成用户对象
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="userType">用户类型</param>
        /// <returns></returns>
        public static User CreateUser(long userId, UserType userType = UserType.管理账户)
        {
            User user = null;
            switch (userType)
            {
                case UserType.管理账户:
                    user = new AdminUser(userId);
                    break;
            }
            return user;
        }

        #endregion

        #endregion

        #endregion
    }
}