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
    /// 用户数据访问接口
    /// </summary>
    public interface IUserDataAccess : IDataAccess<UserEntity>
    {
    }

    /// <summary>
    /// 数据库访问
    /// </summary>
    public interface IUserDbAccess : IUserDataAccess
    {

    }
}
