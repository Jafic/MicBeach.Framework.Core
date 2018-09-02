using MicBeach.Domain.Sys.Model;
using MicBeach.Domain.Sys.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Application.Identity.User;

namespace MicBeach.Repository.Sys
{
    /// <summary>
    /// 管理用户存储
    /// </summary>
    public class AdminUserRepository: IAdminUserRepository
    {
        #region 加载管理账户信息

        /// <summary>
        /// 加载管理账户信息
        /// </summary>
        /// <param name="users">用户信息</param>
        public void LoadAdminUser(ref IEnumerable<User> users)
        {
            if (users.IsNullOrEmpty())
            {
                return;
            }
            List<User> newUserList = new List<User>();
            foreach (var user in users)
            {
                if (user == null)
                {
                    continue;
                }
                if (user.UserType != UserType.管理账户)
                {
                    newUserList.Add(user);
                }
                else
                {
                    newUserList.Add(user.MapTo<AdminUser>());
                }
            }
            users = newUserList;
        }

        #endregion
    }
}
