using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Domain.Sys.Repository;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Sys;
using MicBeach.Util.Extension;
using System.Collections.Generic;
using MicBeach.Util.Data;
using MicBeach.Util;
using MicBeach.Application.Identity.Auth;
using MicBeach.Application.Identity;
using MicBeach.Util.Code;

namespace MicBeach.Domain.Sys.Model
{
    /// <summary>
    /// 授权操作组
    /// </summary>
    public class AuthorityOperationGroup : AggregationRoot<AuthorityOperationGroup>
    {
        IAuthorityOperationGroupRepository authorityOperationGroupRepository = null;

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
        protected int _sort;

        /// <summary>
        /// 上级
        /// </summary>
        protected LazyMember<AuthorityOperationGroup> _parent;

        /// <summary>
        /// 根组
        /// </summary>
        protected LazyMember<AuthorityOperationGroup> _root;

        /// <summary>
        /// 等级
        /// </summary>
        protected int _level;

        /// <summary>
        /// 状态
        /// </summary>
        protected AuthorityOperationGroupStatus _status;

        /// <summary>
        /// 说明
        /// </summary>
        protected string _remark;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化一个操作分组对象
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <param name="name">名称</param>
        internal AuthorityOperationGroup(long sysNo = 0, string name = "")
        {
            _sysNo = sysNo;
            _name = name;
            _parent = new LazyMember<AuthorityOperationGroup>(LoadParentGroup);
            _root = new LazyMember<AuthorityOperationGroup>(LoadRootGroup);
            authorityOperationGroupRepository = this.Instance<IAuthorityOperationGroupRepository>();
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
            set
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
        /// 上级
        /// </summary>
        public AuthorityOperationGroup Parent
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
        /// 根组
        /// </summary>
        public AuthorityOperationGroup Root
        {
            get
            {
                return _root.Value;
            }
            protected set
            {
                _root.SetValue(value, false);
            }
        }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public AuthorityOperationGroupStatus Status
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
            //根节点
            ValidateRootGroup();
            authorityOperationGroupRepository.Save(this);
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
        public void SetParentGroup(AuthorityOperationGroup parentGroup)
        {
            int parentLevel = 0;
            long parentSysNo = 0;
            if (parentGroup != null)
            {
                if (_sysNo == parentGroup.SysNo&&!PrimaryValueIsNone())
                {
                    throw new Exception("不能将分组数据设置为自己的上级分组");
                }
                if (parentGroup.Root != null && parentGroup.Root.SysNo == _sysNo)
                {
                    throw new Exception("不能将当前分组的下级设置为上级分组");
                }
                parentLevel = parentGroup.Level;
                parentSysNo = parentGroup.SysNo;
                _root.SetValue(parentGroup.Root, false);
            }
            else
            {
                _root.SetValue(null, false);
            }
            //排序
            IQuery sortQuery = QueryFactory.Create<AuthorityOperationGroupQuery>(r => r.Parent == parentSysNo);
            sortQuery.AddQueryFields<AuthorityOperationGroupQuery>(c => c.Sort);
            int maxSortIndex = authorityOperationGroupRepository.Max<int>(sortQuery);
            _sort = maxSortIndex + 1;
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

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitPrimaryValue()
        {
            base.InitPrimaryValue();
            _sysNo = GenerateAuthorityOperationGroupId();
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
            IQuery query = QueryFactory.Create<AuthorityOperationGroupQuery>(r => r.Parent == SysNo);
            List<AuthorityOperationGroup> childGroupList = authorityOperationGroupRepository.GetList(query);
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
        AuthorityOperationGroup LoadParentGroup()
        {
            if (!AllowLazyLoad(r => r.Parent))
            {
                return _parent.CurrentValue;
            }
            if (_level <= 1 || _parent.CurrentValue == null)
            {
                return _parent.CurrentValue;
            }
            IQuery parentQuery = QueryFactory.Create<AuthorityOperationGroupQuery>(r => r.SysNo == _parent.CurrentValue.SysNo);
            return authorityOperationGroupRepository.Get(parentQuery);
        }

        #endregion

        #region 加载根级分组

        /// <summary>
        /// 加载根级分组
        /// </summary>
        AuthorityOperationGroup LoadRootGroup()
        {
            if (!AllowLazyLoad(r => r.Root))
            {
                return _root.CurrentValue;
            }
            if (_root.CurrentValue == null || _root.CurrentValue.SysNo <= 0)
            {
                return null;
            }
            IQuery rootQuery = QueryFactory.Create<AuthorityOperationGroupQuery>(r => r.SysNo == _root.CurrentValue.SysNo);
            return authorityOperationGroupRepository.Get(rootQuery);
        }

        #endregion

        #region 验证根组

        /// <summary>
        /// 验证根组
        /// </summary>
        void ValidateRootGroup()
        {
            if (_root.CurrentValue == null || _root.CurrentValue.SysNo <= 0)
            {
                if (PrimaryValueIsNone())
                {
                    InitPrimaryValue();
                }
                var newRootGroup = CreateAuthorityOperationGroup(_sysNo);
                _root.SetValue(newRootGroup, false);
            }
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

        #region 生成分组对象编号

        /// <summary>
        /// 生成分组对象编号
        /// </summary>
        /// <returns></returns>
        public static long GenerateAuthorityOperationGroupId()
        {
            return SerialNumber.GetSerialNumber(AppIdentityUtil.GetIdGroupCode(IdentityGroup.授权操作分组));
        }

        #endregion

        #region 创建新的操作分组对象

        /// <summary>
        /// 创建操作分组对象
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public static AuthorityOperationGroup CreateAuthorityOperationGroup(long sysNo, string name = "")
        {
            return new AuthorityOperationGroup(sysNo, name);
        }

        #endregion

        #endregion

        #endregion
    }
}