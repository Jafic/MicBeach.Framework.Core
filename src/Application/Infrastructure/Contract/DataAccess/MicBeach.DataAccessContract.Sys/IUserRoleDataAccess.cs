using MicBeach.Develop.DataAccess;
using MicBeach.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DataAccessContract.Sys
{
    /// <summary>
    /// 用户角色数据访问
    /// </summary>
    public interface IUserRoleDataAccess : IDataAccess<UserRoleEntity>
    {
        #region 获取指定用户绑定的角色

        /// <summary>
        /// 获取指定用户绑定的角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        List<UserRoleEntity> GetUserBindRoles(long userId);

        #endregion
    }

    public interface IUserRoleDbAccess : IUserRoleDataAccess
    { }
}
