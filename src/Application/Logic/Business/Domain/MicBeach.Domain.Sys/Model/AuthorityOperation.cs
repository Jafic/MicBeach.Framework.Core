using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Domain.Sys.Repository;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Sys;
using MicBeach.Util.Extension;
using System.Collections.Generic;
using System.Linq;
using MicBeach.Util.Data;
using MicBeach.Util;
using MicBeach.Application.Identity.Auth;
using MicBeach.Application.Identity;
using MicBeach.Util.Code;

namespace MicBeach.Domain.Sys.Model
{
    /// <summary>
    /// 授权操作
    /// </summary>
    public class AuthorityOperation : AggregationRoot<AuthorityOperation>
    {
        IAuthorityOperationRepository authorityOperationRepository = null;

        #region	字段

        /// <summary>
        /// 主键编号
        /// </summary>
        protected long _sysNo;

        /// <summary>
        /// 控制器
        /// </summary>
        protected string _controllerCode;

        /// <summary>
        /// 操作方法
        /// </summary>
        protected string _actionCode;

        /// <summary>
        /// 操作类型
        /// </summary>
        protected AuthorityOperationMethod _method;

        /// <summary>
        /// 名称
        /// </summary>
        protected string _name;

        /// <summary>
        /// 状态
        /// </summary>
        protected AuthorityOperationStatus _status;

        /// <summary>
        /// 排序
        /// </summary>
        protected int _sort;

        /// <summary>
        /// 操作分组
        /// </summary>
        protected LazyMember<AuthorityOperationGroup> _group;

        /// <summary>
        /// 授权类型
        /// </summary>
        protected AuthorityOperationAuthorizeType _authorizeType;

        /// <summary>
        /// 方法描述
        /// </summary>
        protected string _remark;

        /// <summary>
        /// 操作对应的权限
        /// </summary>
        protected LazyMember<List<Authority>> _authoritys;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化授权操作对象
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <param name="name">名称</param>
        /// <param name="controllerCode">控制编码</param>
        /// <param name="actionCode">方法编码</param>
        internal AuthorityOperation(long sysNo = 0, string name = "", string controllerCode = "", string actionCode = "")
        {
            _sysNo = sysNo;
            _name = name;
            _controllerCode = controllerCode?.ToUpper();
            _actionCode = actionCode?.ToUpper();
            _group = new LazyMember<AuthorityOperationGroup>(LoadAuthorityOperationGroup);
            _authoritys = new LazyMember<List<Authority>>(LoadAuthority);
            authorityOperationRepository = this.Instance<IAuthorityOperationRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 主键编号
        /// </summary>
        public long SysNo
        {
            get
            {
                return _sysNo;
            }
            set
            {
                _sysNo = value;
            }
        }

        /// <summary>
        /// 控制器
        /// </summary>
        public string ControllerCode
        {
            get
            {
                return _controllerCode;
            }
            set
            {
                _controllerCode = value?.ToUpper();
            }
        }

        /// <summary>
        /// 操作方法
        /// </summary>
        public string ActionCode
        {
            get
            {
                return _actionCode;
            }
            set
            {
                _actionCode = value?.ToUpper();
            }
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public AuthorityOperationMethod Method
        {
            get
            {
                return _method;
            }
            set
            {
                _method = value;
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
        /// 状态
        /// </summary>
        public AuthorityOperationStatus Status
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
        /// 操作分组
        /// </summary>
        public AuthorityOperationGroup Group
        {
            get
            {
                return _group.Value;
            }
            protected set
            {
                _group.SetValue(value, false);
            }
        }

        /// <summary>
        /// 授权类型
        /// </summary>
        public AuthorityOperationAuthorizeType AuthorizeType
        {
            get
            {
                return _authorizeType;
            }
            set
            {
                _authorizeType = value;
            }
        }

        /// <summary>
        /// 方法描述
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
        /// 操作对应的权限
        /// </summary>
        public List<Authority> Authoritys
        {
            get
            {
                return _authoritys.Value;
            }
            protected set
            {
                _authoritys.SetValue(value, false);
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
            authorityOperationRepository.Save(this);
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

        #region 设置操作分组

        /// <summary>
        /// 设置操作分组
        /// </summary>
        /// <param name="group">分组信息</param>
        /// <param name="init">是否初始化</param>
        public void SetGroup(AuthorityOperationGroup group, bool init = true)
        {
            _group.SetValue(group, init);
        }

        #endregion

        #region 添加新的权限

        /// <summary>
        /// 添加新的权限
        /// </summary>
        /// <param name="newAuthoritys">新的权限信息</param>
        public void AddAuthoritys(IEnumerable<Authority> newAuthoritys)
        {
            if (newAuthoritys.IsNullOrEmpty())
            {
                return;
            }
            List<Authority> nowAuthorityList = _authoritys.Value ?? new List<Authority>(0);
            foreach (var authority in newAuthoritys)
            {
                if (authority == null)
                {
                    continue;
                }
                var nowAuthority = nowAuthorityList.FirstOrDefault(c => c.Code == authority.Code);
                if (nowAuthority == null)
                {
                    var newAuthority = authority.MapTo<Authority>();//重新生成一个对象，纺织修改影响原对象的值
                    newAuthority.MarkNew();
                    nowAuthorityList.Add(newAuthority);
                    continue;
                }
                nowAuthority.MarkStored();
            }
            _authoritys.SetValue(nowAuthorityList, true);
        }

        #endregion

        #region 移除权限

        /// <summary>
        /// 移除权限
        /// </summary>
        /// <param name="removeAuthoritys">要移除的权限信息</param>
        public void RemoveAuthoritys(IEnumerable<Authority> removeAuthoritys)
        {
            if (removeAuthoritys.IsNullOrEmpty())
            {
                return;
            }
            List<Authority> nowAuthorityList = _authoritys.Value;
            if (nowAuthorityList.IsNullOrEmpty())
            {
                return;
            }
            foreach (var authority in removeAuthoritys)
            {
                if (authority == null)
                {
                    continue;
                }
                var nowAuthority = nowAuthorityList.FirstOrDefault(c => c.Code == authority.Code);
                if (nowAuthority != null)
                {
                    nowAuthority.MarkRemove();
                }
            }
        }

        #endregion

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitPrimaryValue()
        {
            base.InitPrimaryValue();
            _sysNo = GenerateAuthorityOperationId();
        }

        #endregion

        #endregion

        #region 内部方法

        #region 加载操作对应的分组

        /// <summary>
        /// 加载操作对应的分组
        /// </summary>
        /// <returns></returns>
        AuthorityOperationGroup LoadAuthorityOperationGroup()
        {
            if (!AllowLazyLoad(r => r.Group))
            {
                return _group.CurrentValue;
            }
            if (_group.CurrentValue == null || _group.CurrentValue.SysNo <= 0)
            {
                return _group.CurrentValue;
            }
            return this.Instance<IAuthorityOperationGroupRepository>().Get(QueryFactory.Create<AuthorityOperationGroupQuery>(r => r.SysNo == _group.CurrentValue.SysNo));
        }

        #endregion

        #region 保存数据验证

        /// <summary>
        /// 保存数据验证
        /// </summary>
        /// <returns></returns>
        protected override bool SaveValidation()
        {
            bool valResult = base.SaveValidation();
            if (!valResult)
            {
                return valResult;
            }
            if (_group.CurrentValue == null || _group.CurrentValue.SysNo <= 0)
            {
                throw new Exception("请设置操作所属分组");
            }
            IQuery groupQuery = QueryFactory.Create<AuthorityOperationGroupQuery>(c => c.SysNo == _group.CurrentValue.SysNo);
            if (!this.Instance<IAuthorityOperationGroupRepository>().Exist(groupQuery))
            {
                throw new Exception("请设置正确的分组");
            }
            return true;
        }

        #endregion

        #region 加载操作对应的权限信息

        /// <summary>
        /// 加载操作对应的权限信息
        /// </summary>
        /// <returns></returns>
        List<Authority> LoadAuthority()
        {
            if (!AllowLazyLoad(r => r.Authoritys))
            {
                return _authoritys.CurrentValue;
            }
            IQuery query = QueryFactory.Create();
            IQuery bindQuery = QueryFactory.Create(AuthorityBindOperationQuery.ObjectName);
            bindQuery.AddQueryFields<AuthorityBindOperationQuery>(a => a.AuthorityCode);
            bindQuery.And<AuthorityBindOperationQuery>(a => a.AuthorithOperation == _sysNo);
            query.And<AuthorityQuery>(a => a.Code, CriteriaOperator.In, bindQuery);
            return this.Instance<IAuthorityRepository>().GetList(query);
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

        #endregion

        #region 静态方法

        #region 生成一个操作编号

        /// <summary>
        /// 生成一个操作编号
        /// </summary>
        /// <returns></returns>
        public static long GenerateAuthorityOperationId()
        {
            return SerialNumber.GetSerialNumber(AppIdentityUtil.GetIdGroupCode(IdentityGroup.授权操作));
        }

        #endregion

        #region 创建授权操作

        /// <summary>
        /// 创建一个授权操作对象
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <param name="name">名称</param>
        /// <param name="controllerCode">控制编码</param>
        /// <param name="actionCode">方法编码</param>
        /// <returns></returns>
        public static AuthorityOperation CreateAuthorityOperation(long sysNo = 0, string name = "", string controllerCode = "", string actionCode = "")
        {
            sysNo = sysNo <= 0 ? GenerateAuthorityOperationId() : sysNo;
            return new AuthorityOperation(sysNo, name, controllerCode, actionCode);
        }

        #endregion

        #endregion

        #endregion
    }
}