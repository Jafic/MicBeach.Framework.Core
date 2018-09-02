using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Util.Extension;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Sys;
using MicBeach.Domain.Sys.Repository;
using System.Collections.Generic;
using MicBeach.Util.Data;
using MicBeach.Util;
using MicBeach.Application.Identity.Auth;
using MicBeach.Application.Identity;
using MicBeach.Util.Code;
using MicBeach.Util.CustomerException;

namespace MicBeach.Domain.Sys.Model
{
    /// <summary>
    /// 权限分组
    /// </summary>
    public class AuthorityGroup : AggregationRoot<AuthorityGroup>
    {
        IAuthorityGroupRepository authorityGroupRepository = null;

        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        protected long _sysNo;

        /// <summary>
        /// 名称
        /// </summary>
        protected string _name;

        /// <summary>
        /// 排序
        /// </summary>
        protected int _sortIndex;

        /// <summary>
        /// 状态
        /// </summary>
        protected AuthorityGroupStatus _status;

        /// <summary>
        /// 上级分组
        /// </summary>
        protected LazyMember<AuthorityGroup> _parent;

        /// <summary>
        /// 分组等级
        /// </summary>
        protected int _level;

        /// <summary>
        /// 说明
        /// </summary>
        protected string _remark;

        #endregion

        #region 构造方法

        /// <summary>
        /// 初始化一个构造方法
        /// </summary>
        /// <param name="roleId">编号</param>
        /// <param name="name">名称</param>
        internal AuthorityGroup(long roleId = 0, string name = "")
        {
            _sysNo = roleId;
            _name = name;
            _parent = new LazyMember<AuthorityGroup>(LoadParentGroup);
            authorityGroupRepository = this.Instance<IAuthorityGroupRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 编号
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
        /// 排序
        /// </summary>
        public int SortIndex
        {
            get
            {
                return _sortIndex;
            }
            protected set
            {
                _sortIndex = value;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public AuthorityGroupStatus Status
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
        /// 上级分组
        /// </summary>
        public AuthorityGroup Parent
        {
            get
            {
                return _parent.Value;
            }
            protected set
            {
                _parent.SetValue(value, false);
            }
        }

        /// <summary>
        /// 分组等级
        /// </summary>
        public int Level
        {
            get
            {
                return _level;
            }
            protected set
            {
                _level = value;
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

        #endregion

        #region 方法

        #region 功能方法

        #region 保存分组

        /// <summary>
        /// 保存分组
        /// </summary>
        public override void Save()
        {
            authorityGroupRepository.Save(this);
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

        #region 设置上级分组

        /// <summary>
        /// 设置上级分组
        /// </summary>
        /// <param name="parentGroup">上级分组</param>
        public void SetParentGroup(AuthorityGroup parentGroup)
        {
            int parentLevel = 0;
            long parentSysNo = 0;
            if (parentGroup != null)
            {
                parentLevel = parentGroup.Level;
                parentSysNo = parentGroup.SysNo;
            }
            if (parentSysNo == _sysNo && !PrimaryValueIsNone())
            {
                throw new AppException("不能将分组本身设置为上级分组");
            }
            //排序
            IQuery sortQuery = QueryFactory.Create<AuthorityGroupQuery>(r => r.Parent == parentSysNo);
            sortQuery.AddQueryFields<AuthorityGroupQuery>(c => c.SortIndex);
            int maxSortIndex = authorityGroupRepository.Max<int>(sortQuery);
            _sortIndex = maxSortIndex + 1;
            _parent.SetValue(parentGroup, true);
            //等级
            int newLevel = parentLevel + 1;
            bool modifyChild = newLevel != _level;
            _level = newLevel;
            if (modifyChild)
            {
                //修改所有子集信息
                ModifyChildAuthorityGroupParentGroup();
            }
        }

        #endregion

        #region 修改排序

        /// <summary>
        /// 修改排序
        /// </summary>
        /// <param name="newSortIndex">新排序,排序编号必须大于0</param>
        public void ModifySortIndex(int newSortIndex)
        {
            if (newSortIndex <= 0)
            {
                throw new Exception("请填写正确的排序编号");
            }
            _sortIndex = newSortIndex;
            //其它角色顺延
            IQuery sortQuery = QueryFactory.Create<AuthorityGroupQuery>(r => r.Parent == (_parent.CurrentValue == null ? 0 : _parent.CurrentValue.SysNo) && r.SortIndex >= newSortIndex);
            IModify modifyExpression = ModifyFactory.Create();
            modifyExpression.Add<RoleQuery>(r => r.SortIndex, 1);
            authorityGroupRepository.Modify(modifyExpression, sortQuery);
        }

        #endregion

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitPrimaryValue()
        {
            base.InitPrimaryValue();
            _sysNo = GenerateAuthorityGroupId();
        }

        #endregion

        #endregion

        #region 内部方法

        #region 修改下级分组

        /// <summary>
        /// 修改下级分组
        /// </summary>
        void ModifyChildAuthorityGroupParentGroup()
        {
            if (IsNew)
            {
                return;
            }
            IQuery query = QueryFactory.Create<AuthorityGroupQuery>(r => r.Parent == SysNo);
            List<AuthorityGroup> childGroupList = authorityGroupRepository.GetList(query);
            foreach (var group in childGroupList)
            {
                group.SetParentGroup(this);
                group.Save();
            }
        }

        #endregion

        #region 加载上级分组

        /// <summary>
        /// 加载上级分组
        /// </summary>
        AuthorityGroup LoadParentGroup()
        {
            if (!AllowLazyLoad(r => r.Parent))
            {
                return _parent.CurrentValue;
            }
            if (_level <= 1 || _parent.CurrentValue == null)
            {
                return null;
            }
            return authorityGroupRepository.Get(QueryFactory.Create<AuthorityGroupQuery>(r => r.SysNo == _parent.CurrentValue.SysNo));
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

        #region 生成分组编号

        /// <summary>
        /// 生成角色编号
        /// </summary>
        /// <returns></returns>
        public static long GenerateAuthorityGroupId()
        {
            return SerialNumber.GetSerialNumber(AppIdentityUtil.GetIdGroupCode(IdentityGroup.权限分组));
        }

        #endregion

        #region 创建分组对象

        /// <summary>
        /// 创建分组对象
        /// </summary>
        /// <param name="sysNo">分组编号</param>
        /// <param name="name">分组名称</param>
        /// <returns></returns>
        public static AuthorityGroup CreateAuthorityGroup(long sysNo, string name = "")
        {
            return new AuthorityGroup(sysNo, name);
        }

        #endregion

        #endregion

        #endregion
    }
}