using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Util.Extension;
using MicBeach.Domain.Sys.Repository;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Sys;
using MicBeach.Util.Data;
using MicBeach.Application.Identity.Auth;

namespace MicBeach.Domain.Sys.Model
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Authority : AggregationRoot<Authority>
    {
        IAuthorityRepository authorityRepository = null;

        #region	字段

        /// <summary>
        /// 权限编码
        /// </summary>
        protected string _code;

        /// <summary>
        /// 名称
        /// </summary>
        protected string _name;

        /// <summary>
        /// 权限类型
        /// </summary>
        protected AuthorityType _authType;

        /// <summary>
        /// 状态
        /// </summary>
        protected AuthorityStatus _status;

        /// <summary>
        /// 排序
        /// </summary>
        protected int _sort;

        /// <summary>
        /// 权限分组
        /// </summary>
        protected LazyMember<AuthorityGroup> _authGroup;

        /// <summary>
        /// 说明
        /// </summary>
        protected string _remark;

        /// <summary>
        /// 添加时间
        /// </summary>
        protected DateTime _createDate;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个权限对象
        /// </summary>
        /// <param name="code">权限码</param>
        /// <param name="name">权限名称</param>
        public Authority(string code = "", string name = "")
        {
            _code = code;
            _name = name;
            _status = AuthorityStatus.启用;
            _authGroup = new LazyMember<AuthorityGroup>(LoadAuthorityGroup);
            _authType = AuthorityType.管理;
            _createDate = DateTime.Now;
            _sort = 0;
            authorityRepository = this.Instance<IAuthorityRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// 权限类型
        /// </summary>
        public AuthorityType AuthType
        {
            get
            {
                return _authType;
            }
            set
            {
                _authType = value;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public AuthorityStatus Status
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
        /// 排序
        /// </summary>
        public int Sort
        {
            get
            {
                return _sort;
            }
            set
            {
                _sort = value;
            }
        }

        /// <summary>
        /// 权限分组
        /// </summary>
        public AuthorityGroup AuthGroup
        {
            get
            {
                return _authGroup.Value;
            }
            protected set
            {
                _authGroup.SetValue(value, false);
            }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                _remark = value;
            }
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
            set
            {
                _createDate = value;
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
            authorityRepository.Save(this);
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除
        /// </summary>
        public override void Remove()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 设置分组

        /// <summary>
        /// 设置分组
        /// </summary>
        /// <param name="group">权限分组</param>
        /// <param name="init">是否初始化</param>
        public void SetGroup(AuthorityGroup group, bool init = true)
        {
            _authGroup.SetValue(group, init);
        }

        #endregion

        #endregion

        #region 内部方法

        #region 加载权限分组

        /// <summary>
        /// 加载权限分组
        /// </summary>
        /// <returns></returns>
        AuthorityGroup LoadAuthorityGroup()
        {
            if (!AllowLazyLoad(r => r.AuthGroup))
            {
                return _authGroup.CurrentValue;
            }
            if (_authGroup.CurrentValue == null || _authGroup.CurrentValue.SysNo <= 0)
            {
                return _authGroup.CurrentValue;
            }
            return this.Instance<IAuthorityGroupRepository>().Get(QueryFactory.Create<AuthorityGroupQuery>(r => r.SysNo == _authGroup.CurrentValue.SysNo));
        }

        #endregion

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        public override bool PrimaryValueIsNone()
        {
            return _code.IsNullOrEmpty();
        }

        #endregion

        #endregion

        #region 静态方法

        /// <summary>
        /// 创建一个权限对象
        /// </summary>
        /// <param name="code">授权码</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static Authority CreateAuthority(string code = "", string name = "")
        {
            return new Authority(code, name);
        }

        #endregion

        #endregion
    }
}