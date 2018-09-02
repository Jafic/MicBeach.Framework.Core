using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.Domain.Aggregation
{
    /// <summary>
    /// 数据状态
    /// </summary>
    public enum LifeStatus
    {
        New,
        Modify,
        Remove,
        Stored
    }
}
