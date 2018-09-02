using MicBeach.Develop.CQuery;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Sys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Sys.Repository
{
    /// <summary>
    /// 用户存储
    /// </summary>
    public interface IUserRepository: IRepository<User>
    {
    }
}
