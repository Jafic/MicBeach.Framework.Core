//using MicBeach.CQuery;
//using MicBeach.CQuery.Paging;
//using MicBeach.Develop.Domain.Aggregation;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics.Contracts;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MicBeach.Develop.Domain.Repository
//{
//    [ContractClassFor(typeof(IRepository<>))]
//    public abstract class RepositoryContract<T> : IRepository<T> where T : IAggregationRoot<T>
//    {
//        #region 属性

//        /// <summary>
//        /// 数据移除事件
//        /// </summary>
//        public event DataChange<T> RemoveEvent;

//        /// <summary>
//        /// 数据保存事件
//        /// </summary>
//        public event DataChange<T> SaveEvent;

//        #endregion

//        #region 方法

//        /// <summary>
//        /// 保存对象集合
//        /// </summary>
//        /// <param name="objDatas">对象信息</param>
//        public void Save(params T[] objDatas)
//        {
//            Contract.Requires<ArgumentNullException>(objDatas != null && objDatas.Any(), "objDatas is null or empty");
//            Contract.Requires<ArgumentException>(Contract.ForAll(objDatas, (T t) => t.CanBeSave), "any object data cann't to be save");
//            Contract.Ensures(Contract.ForAll(objDatas, (T t) => t.MarkStored()));
//            //Contract.Ensures(ExecuteSaveEvent(objDatas));
//        }

//        /// <summary>
//        /// 移除对象集合
//        /// </summary>
//        /// <param name="objDatas">对象信息</param>
//        public void Remove(params T[] objDatas)
//        {
//            Contract.Requires<ArgumentNullException>(objDatas != null && objDatas.Any(), "objList is null or empty");
//            Contract.Requires<ArgumentException>(Contract.ForAll(objDatas, (T t) => t.CanBeRemove), "any object data cann't to be remove");
//            Contract.Ensures(Contract.ForAll(objDatas, (T t) => t.MarkRemove()));
//            Contract.Ensures(ExecuteRemoveEvent(objDatas));
//        }

//        /// <summary>
//        /// 根据条件移除对象
//        /// </summary>
//        /// <param name="query">查询对象</param>
//        public void Remove(IQuery query)
//        {

//        }

//        /// <summary>
//        /// 获取对象
//        /// </summary>
//        /// <param name="query">查询对象</param>
//        /// <returns>对象</returns>
//        public T Get(IQuery query)
//        {
//            return default(T);
//        }

//        /// <summary>
//        /// 获取对象列表
//        /// </summary>
//        /// <param name="query">查询对象</param>
//        /// <returns>对象列表</returns>
//        public List<T> GetList(IQuery query)
//        {
//            return new List<T>(0);
//        }

//        /// <summary>
//        /// 获取对象分页
//        /// </summary>
//        /// <param name="query">查询对象</param>
//        /// <returns>对象列表</returns>
//        public IPaging<T> GetPaging(IQuery query)
//        {
//            return Paging<T>.EmptyPaging();
//        }

//        /// <summary>
//        /// 是否存在指定的数据
//        /// </summary>
//        /// <param name="query">查询条件</param>
//        /// <returns></returns>
//        public bool Exist(IQuery query)
//        {
//            return false;
//        }

//        /// <summary>
//        /// 数据量
//        /// </summary>
//        /// <param name="query">查询条件</param>
//        /// <returns></returns>
//        public long Count(IQuery query)
//        {
//            return 0;
//        }

//        /// <summary>
//        /// 获取最大值
//        /// </summary>
//        /// <typeparam name="DT">返回数据类型</typeparam>
//        /// <param name="query">查询条件</param>
//        /// <returns>最大值</returns>
//        public DT Max<DT>(IQuery query)
//        {
//            return default(DT);
//        }

//        /// <summary>
//        /// 获取最小值
//        /// </summary>
//        /// <typeparam name="DT">返回数据类型</typeparam>
//        /// <param name="query"></param>
//        /// <returns>最小值</returns>
//        public DT Min<DT>(IQuery query)
//        {
//            return default(DT);
//        }

//        /// <summary>
//        /// 求和
//        /// </summary>
//        /// <typeparam name="DT">返回数据类型</typeparam>
//        /// <param name="query">查询条件</param>
//        /// <returns>总和</returns>
//        public DT Sum<DT>(IQuery query)
//        {
//            return default(DT);
//        }

//        /// <summary>
//        /// 平均值
//        /// </summary>
//        /// <typeparam name="DT">返回数据类型</typeparam>
//        /// <param name="query">查询条件</param>
//        /// <returns>平均值</returns>
//        public DT Avg<DT>(IQuery query)
//        {
//            return default(DT);
//        }

//        /// <summary>
//        /// 执行移除事件
//        /// </summary>
//        /// <param name="datas">数据</param>
//        /// <returns></returns>
//        public bool ExecuteRemoveEvent(params T[] datas)
//        {
//            RemoveEvent?.Invoke(datas);
//            return true;
//        }

//        /// <summary>
//        /// 执行保存事件
//        /// </summary>
//        /// <param name="datas">数据</param>
//        /// <returns></returns>
//        public bool ExecuteSaveEvent(params T[] datas)
//        {
//            SaveEvent?.Invoke(datas);
//            return true;
//        }

//        /// <summary>
//        /// 修改数据
//        /// </summary>
//        /// <param name="expression">修改表达式</param>
//        /// <param name="query">条件</param>
//        public void Modify(IModify expression, IQuery query)
//        {
//        }

//        #endregion
//    }
//}
