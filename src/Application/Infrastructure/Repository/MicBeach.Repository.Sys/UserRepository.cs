using MicBeach.Domain.Sys.Model;
using MicBeach.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.DataAccessContract.Sys;
using MicBeach.Domain.Sys.Repository;
using MicBeach.Develop.Domain.Repository;

namespace MicBeach.Repository.Sys
{
    /// <summary>
    /// 用户存储操作
    /// </summary>
    public class UserRepository : DefaultRepository<User, UserEntity, IUserDataAccess>, IUserRepository
    {
        #region 事件绑定

        /// <summary>
        /// 时间绑定
        /// </summary>
        protected override void BindEvent()
        {
            base.BindEvent();

            #region 用户角色

            IUserRoleRepository userRoleRepository = this.Instance<IUserRoleRepository>();
            //SaveEvent += userRoleRepository.SaveUserRoleFromUser;//保存用户角色
            RemoveEvent += userRoleRepository.RemoveUserRoleByUser;//删除用户时删除绑定的角色信息

            #endregion

            #region 用户授权

            RemoveEvent += this.Instance<IUserAuthorizeRepository>().RemoveUserAuthorizeByUser;//移除用户时移除用户授权

            #endregion

            QueryEvent += this.Instance<IAdminUserRepository>().LoadAdminUser;//加载数据
        }

        #endregion
    }
}
