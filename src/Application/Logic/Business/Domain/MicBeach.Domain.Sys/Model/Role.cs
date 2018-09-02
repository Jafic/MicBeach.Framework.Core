using MicBeach.Develop.CQuery;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Domain.Sys.Repository;
using MicBeach.Query.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Util.Data;
using MicBeach.Util;
using MicBeach.Application.Identity.User;
using MicBeach.Application.Identity;
using MicBeach.Util.Code;
using MicBeach.Util.CustomerException;

namespace MicBeach.Domain.Sys.Model
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role : AggregationRoot<Role>
    {
        IRoleRepository roleRepository = null;

        #region	字段

        /// <summary>
        /// 角色编号
        /// </summary>
        protected long _sysNo;

        /// <summary>
        /// 名称
        /// </summary>
        protected string _name;

        /// <summary>
        /// 等级
        /// </summary>
        protected int _level;

        /// <summary>
        /// 上级
        /// </summary>
        protected LazyMember<Role> _parent;

        /// <summary>
        /// 排序
        /// </summary>
        protected int _sortIndex;

        /// <summary>
        /// 状态
        /// </summary>
        protected RoleStatus _status;

        /// <summary>
        /// 添加时间
        /// </summary>
        protected DateTime _createDate;

        /// <summary>
        /// 备注信息
        /// </summary>
        protected string _remark;

        #endregion

        #region 构造方法

        /// <summary>
        /// 初始化一个构造方法
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="name">角色名称</param>
        internal Role(long roleId = 0, string name = "")
        {
            _sysNo = roleId;
            _name = name;
            Initialization();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 角色编号
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
        /// 等级
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
        /// 上级
        /// </summary>
        public Role Parent
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
        public RoleStatus Status
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

        /// <summary>
        /// 备注信息
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

        #region 功能行为

        #region 保存角色

        /// <summary>
        /// 保存角色
        /// </summary>
        public override void Save()
        {
            roleRepository.Save(this);
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

        #region 设置上级角色

        /// <summary>
        /// 设置上级角色
        /// </summary>
        /// <param name="role">上级角色</param>
        public void SetParentRole(Role parentRole)
        {
            int parentLevel = 0;
            long parentSysNo = 0;
            if (parentRole != null)
            {
                parentLevel = parentRole.Level;
                parentSysNo = parentRole.SysNo;
            }
            if (parentSysNo == _sysNo&&!PrimaryValueIsNone())
            {
                throw new AppException("不能将角色本身设置为自己的上级角色");
            }
            //排序
            IQuery sortQuery = QueryFactory.Create<RoleQuery>(r => r.Parent == parentSysNo);
            sortQuery.AddQueryFields<RoleQuery>(c => c.SortIndex);
            int maxSortIndex = roleRepository.Max<int>(sortQuery);
            _sortIndex = maxSortIndex + 1;
            _parent.SetValue(parentRole, true);
            //等级
            int newLevel = parentLevel + 1;
            bool modifyChild = newLevel != _level;
            _level = newLevel;
            if (modifyChild)
            {
                //修改所有子集信息
                ModifyChildRole();
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
                throw new Exception("请填写正确的角色排序");
            }
            _sortIndex = newSortIndex;
            //其它角色顺延
            IQuery sortQuery = QueryFactory.Create<RoleQuery>(r => r.Parent == (_parent.CurrentValue == null ? 0 : _parent.CurrentValue.SysNo) && r.SortIndex >= newSortIndex);
            IModify modifyExpression = ModifyFactory.Create();
            modifyExpression.Add<RoleQuery>(r => r.SortIndex, 1);
            roleRepository.Modify(modifyExpression, sortQuery);
        }

        #endregion

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitPrimaryValue()
        {
            base.InitPrimaryValue();
            _sysNo = GenerateRoleId();
        }

        #endregion

        #endregion

        #region 内部行为

        #region 初始化对象

        /// <summary>
        /// 初始化对象
        /// </summary>
        void Initialization()
        {
            _createDate = DateTime.Now;
            _status = RoleStatus.正常;
            _parent = new LazyMember<Role>(LoadParentRole);
            roleRepository = this.Instance<IRoleRepository>();
        }

        #endregion

        #region 加载上级角色

        /// <summary>
        /// 加载上级角色
        /// </summary>
        /// <returns></returns>
        Role LoadParentRole()
        {
            if (!AllowLazyLoad(r => r.Parent))
            {
                return _parent.CurrentValue;
            }
            if (_level <= 1 || _parent.CurrentValue == null)
            {
                return null;
            }
            return roleRepository.Get(QueryFactory.Create<RoleQuery>(r => r.SysNo == _parent.CurrentValue.SysNo));
        }

        #endregion

        #region 修改子集信息

        /// <summary>
        /// 修改子集信息
        /// </summary>
        void ModifyChildRole()
        {
            if (IsNew)
            {
                return;
            }
            IQuery query = QueryFactory.Create<RoleQuery>(r => r.Parent == SysNo);
            List<Role> childRoleList = roleRepository.GetList(query);
            foreach (var role in childRoleList)
            {
                role.SetParentRole(this);
                role.Save();
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

        #region 生成角色编号

        /// <summary>
        /// 生成角色编号
        /// </summary>
        /// <returns></returns>
        public static long GenerateRoleId()
        {
            return SerialNumber.GetSerialNumber(AppIdentityUtil.GetIdGroupCode(IdentityGroup.角色));
        }

        #endregion

        #region 创建角色对象

        /// <summary>
        /// 创建一个角色对象
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="name">角色名</param>
        /// <returns>角色对象</returns>
        public static Role CreateRole(long roleId, string name = "")
        {
            return new Role(roleId, name);
        }

        #endregion

        #endregion

        #endregion
    }
}
