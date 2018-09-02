using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.Domain.Aggregation
{
    /// <summary>
    /// 聚合根接口
    /// </summary>
    public interface IAggregationRoot<in T> where T : IAggregationRoot<T>
    {
        #region 属性

        /// <summary>
        /// 能否保存
        /// </summary>
        bool CanBeSave { get; }

        /// <summary>
        /// 能否移除
        /// </summary>
        bool CanBeRemove { get; }

        /// <summary>
        /// 保存时执行添加
        /// </summary>
        bool SaveByAdd { get; }

        /// <summary>
        /// 是否是新的对象
        /// </summary>
        LifeStatus LifeStatus { get; }

        #endregion

        #region 方法

        /// <summary>
        /// 标记为新的对象
        /// </summary>
        bool MarkNew();

        /// <summary>
        /// 标记为移除
        /// </summary>
        bool MarkRemove();

        /// <summary>
        /// 标记为修改
        /// </summary>
        bool MarkModify();

        /// <summary>
        /// 标记为保存
        /// </summary>
        /// <returns></returns>
        bool MarkStored();

        /// <summary>
        /// 保存方法
        /// </summary>
        void Save();

        /// <summary>
        /// 移除方法
        /// </summary>
        void Remove();

        #endregion
    }
}
