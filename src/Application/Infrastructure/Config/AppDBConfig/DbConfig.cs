using MicBeach.DB;
using MicBeach.DB.MySQL;
using MicBeach.DB.SQLServer;
using MicBeach.Develop.Command;
using MicBeach.Develop.CQuery;
using MicBeach.Entity.Sys;
using MicBeach.Query.Sys;
using MicBeach.Util.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DBConfig
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DbConfig
    {
        public static void Init()
        {
            DataBaseEngineConfig();//数据库执行器
            MicBeach.DB.DBConfig.GetDBServerMethod = GetServerInfo;//获取数据连接信息方法
            DataBaseNameConfig();//数据库名设置
            PrimaryKeyConfig();//主键信息配置
            CacheKeysConfig();//缓存信息配置
            CommandExecuteManager.ExectEngine = new DBCommandEngine();
            RefreshFieldsConfig();//刷新字段
        }

        /// <summary>
        /// 数据库执行器配置
        /// </summary>
        static void DataBaseEngineConfig()
        {
            MicBeach.DB.DBConfig.DbEngines.Add(ServerType.SQLServer, new SqlServerEngine());
            MicBeach.DB.DBConfig.DbEngines.Add(ServerType.MySQL, new MySqlEngine());
        }

        /// <summary>
        /// 获取数据库服务器
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        static List<ServerInfo> GetServerInfo(ICommand command)
        {
            List<ServerInfo> servers = new List<ServerInfo>();
            var objectNameArray = command.ObjectName.LSplit("_");
            string prefix = objectNameArray[0].ToLower();
            string suffix = objectNameArray[objectNameArray.Length - 1].ToLower();
            servers.Add(new ServerInfo()
            {
                ServerType = ServerType.SQLServer,
                ConnectionString = "Server=115.28.234.238;DataBase=SingleDefault;uid=sa;pwd=LDB123456ldb"
            });
            return servers;
        }

        /// <summary>
        /// 数据库名设置
        /// </summary>
        static void DataBaseNameConfig()
        {
            #region Sys

            QueryConfig.SetObjectName("Sys_User", typeof(UserEntity), typeof(UserQuery), typeof(AdminUserQuery));
            QueryConfig.SetObjectName("Sys_Role", typeof(RoleEntity), typeof(RoleQuery));
            QueryConfig.SetObjectName("Sys_UserRole", typeof(UserRoleEntity), typeof(UserRoleQuery));
            QueryConfig.SetObjectName("Sys_Authority", typeof(AuthorityEntity), typeof(AuthorityQuery));
            QueryConfig.SetObjectName("Sys_AuthorityBindOperation", typeof(AuthorityBindOperationEntity), typeof(AuthorityBindOperationQuery));
            QueryConfig.SetObjectName("Sys_AuthorityGroup", typeof(AuthorityGroupEntity), typeof(AuthorityGroupQuery));
            QueryConfig.SetObjectName("Sys_AuthorityOperation", typeof(AuthorityOperationEntity), typeof(AuthorityOperationQuery));
            QueryConfig.SetObjectName("Sys_AuthorityOperationGroup", typeof(AuthorityOperationGroupEntity), typeof(AuthorityOperationGroupQuery));
            QueryConfig.SetObjectName("Sys_UserAuthorize", typeof(UserAuthorizeEntity), typeof(UserAuthorizeQuery));
            QueryConfig.SetObjectName("Sys_RoleAuthorize", typeof(RoleAuthorizeEntity), typeof(RoleAuthorizeQuery));

            #endregion
        }

        /// <summary>
        /// 主键配置
        /// </summary>
        static void PrimaryKeyConfig()
        {
            #region Sys

            QueryConfig.SetPrimaryKey<UserEntity>(u => u.SysNo);
            QueryConfig.SetPrimaryKey<RoleEntity>(u => u.SysNo);
            QueryConfig.SetPrimaryKey<AuthorityEntity>(u => u.Code);
            QueryConfig.SetPrimaryKey<AuthorityGroupEntity>(u => u.SysNo);
            QueryConfig.SetPrimaryKey<AuthorityOperationEntity>(u => u.SysNo);
            QueryConfig.SetPrimaryKey<AuthorityOperationGroupEntity>(u => u.SysNo);
            QueryConfig.SetPrimaryKey<UserAuthorizeEntity>(u => u.User, u => u.Authority);
            QueryConfig.SetPrimaryKey<RoleAuthorizeEntity>(u => u.Role, u => u.Authority);

            #endregion
        }

        /// <summary>
        /// 缓存键值配置
        /// </summary>
        static void CacheKeysConfig()
        {
            QueryConfig.SetCacheKey<UserEntity>(u => u.UserName);
        }

        /// <summary>
        /// 刷新字段配置
        /// </summary>
        static void RefreshFieldsConfig()
        {
        }
    }
}
