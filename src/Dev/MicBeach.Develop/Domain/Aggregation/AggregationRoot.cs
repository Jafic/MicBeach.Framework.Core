using MicBeach.Develop.DataValidation;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Util.ExpressionUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Util;
using MicBeach.Util.CustomerException;

namespace MicBeach.Develop.Domain.Aggregation
{
    /// <summary>
    /// 聚合对象基类
    /// </summary>
    public abstract class AggregationRoot<T> : IAggregationRoot<T> where T : AggregationRoot<T>
    {
        /// <summary>
        /// 新创建的对象
        /// </summary>
        protected LifeStatus _lifeStatus = LifeStatus.New;
        /// <summary>
        /// 批量返回
        /// </summary>
        protected bool _batchReturn = false;

        //已经存储的值
        //private Dictionary<string, T> storeDataList = new Dictionary<string, T>();
        /// <summary>
        /// 启用延迟加载数据
        /// </summary>
        protected bool _loadLazyMember = true;
        //允许自动加载数据的属性
        protected Dictionary<string, bool> _allowLoadPropertys = new Dictionary<string, bool>();
        //static string typeFullName = typeof(T).FullName;

        #region 属性

        /// <summary>
        /// 能否保存
        /// </summary>
        public bool CanBeSave
        {
            get
            {
                return SaveValidation();
            }
        }

        /// <summary>
        /// 能否移除
        /// </summary>
        public bool CanBeRemove
        {
            get
            {
                return RemoveValidation();
            }
        }

        /// <summary>
        /// 是否是新的对象
        /// </summary>
        public LifeStatus LifeStatus
        {
            get
            {
                return _lifeStatus;
            }
            private set
            {
                _lifeStatus = value;
            }
        }

        /// <summary>
        /// 是否为新对象
        /// </summary>
        public bool IsNew
        {
            get
            {
                return _lifeStatus == LifeStatus.New;
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        public bool IsRemove
        {
            get
            {
                return _lifeStatus == LifeStatus.Remove;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        public bool IsModify
        {
            get
            {
                return _lifeStatus == LifeStatus.Modify;
            }
        }

        /// <summary>
        /// 是否批量返回
        /// </summary>
        protected bool BatchReturn
        {

            get
            {
                return _batchReturn;
            }
            private set
            {
                _batchReturn = value;
                _loadLazyMember = !value;
            }
        }

        /// <summary>
        /// 是否允许加载延迟对象
        /// </summary>
        protected bool LoadLazyMember
        {

            get
            {
                return _loadLazyMember;
            }
        }

        /// <summary>
        /// 允许自动加载的属性
        /// </summary>
        protected Dictionary<string, bool> LoadPropertys
        {
            get
            {
                return _allowLoadPropertys;
            }
            private set
            {
                _allowLoadPropertys = value;
            }
        }

        /// <summary>
        /// 已经保存的值
        /// </summary>
        private T StoredData { get; set; } = default(T);

        /// <summary>
        /// 使用新增方法保存
        /// </summary>
        public bool SaveByAdd
        {
            get
            {
                return IsNew || IsRemove;
            }
        }

        ///// <summary>
        ///// 已保存的值
        ///// </summary>
        //internal Dictionary<string, T> StoreDatas
        //{
        //    get
        //    {
        //        return storeDataList;
        //    }
        //}

        #endregion

        #region 方法

        /// <summary>
        /// 保存数据验证
        /// </summary>
        /// <returns></returns>
        protected virtual bool SaveValidation()
        {
            if (this == null)
            {
                return false;
            }
            //主要标识信息
            if (PrimaryValueIsNone())
            {
                if (SaveByAdd)
                {
                    InitPrimaryValue();
                }
                else
                {
                    throw new AppException("未指定要保存对象的标识值");
                }
            }
            var verifyResults = ValidationManager.Validate(this);
            string[] errorMessages = verifyResults.GetErrorMessage();
            if (errorMessages != null && errorMessages.Length > 0)
            {
                throw new AppException(string.Join("\n", errorMessages));
            }
            return true;
        }

        /// <summary>
        /// 移除数据验证
        /// </summary>
        /// <returns></returns>
        protected virtual bool RemoveValidation()
        {
            return this != null;
        }

        /// <summary>
        /// 标记为新的状态
        /// </summary>
        protected void MarkLifeStatus(LifeStatus status)
        {
            _lifeStatus = status;
            switch (status)
            {
                case LifeStatus.Stored:
                    StoredData = (T)MemberwiseClone();
                    break;
                case LifeStatus.New:
                case LifeStatus.Remove:
                    StoredData = default(T);
                    break;
            }
        }

        /// <summary>
        /// 标记为新的对象
        /// </summary>
        public virtual bool MarkNew()
        {
            MarkLifeStatus(LifeStatus.New);
            return true;
        }

        /// <summary>
        /// 标记为移除
        /// </summary>
        public virtual bool MarkRemove()
        {
            MarkLifeStatus(LifeStatus.Remove);
            return true;
        }

        /// <summary>
        /// 标记为修改
        /// </summary>
        public virtual bool MarkModify()
        {
            MarkLifeStatus(LifeStatus.Modify);
            return true;
        }

        /// <summary>
        /// 标记为保存
        /// </summary>
        /// <returns></returns>
        public virtual bool MarkStored()
        {
            MarkLifeStatus(LifeStatus.Stored);
            return true;
        }

        /// <summary>
        /// 设置自动加载的属性
        /// </summary>
        /// <param name="loadPropertys">加载属性数据</param>
        /// <returns></returns>
        public virtual void SetLoadPropertys(Dictionary<string, bool> loadPropertys)
        {
            if (loadPropertys == null)
            {
                return;
            }
            _allowLoadPropertys = _allowLoadPropertys ?? new Dictionary<string, bool>();
            foreach (var property in loadPropertys)
            {
                if (_allowLoadPropertys.ContainsKey(property.Key))
                {
                    _allowLoadPropertys[property.Key] = property.Value;
                }
                else
                {
                    _allowLoadPropertys.Add(property.Key, property.Value);
                }
            }
        }

        /// <summary>
        /// 设置自动加载的属性
        /// </summary>
        /// <param name="property">加载属性</param>
        /// <param name="allowLoad">是否允许加载</param>
        public virtual void SetLoadPropertys(Expression<Func<T, dynamic>> property, bool allowLoad = true)
        {
            if (property == null)
            {
                return;
            }
            Dictionary<string, bool> propertyDic = new Dictionary<string, bool>()
            {
                { ExpressionHelper.GetExpressionPropertyName(property.Body),allowLoad}
            };
            SetLoadPropertys(propertyDic);
        }

        /// <summary>
        /// 关闭延迟加载
        /// </summary>
        public virtual void CloseLazyMemberLoad()
        {
            _loadLazyMember = false;
        }

        /// <summary>
        /// 打开延迟数据加载
        /// </summary>
        public virtual void OpenLazyMemberLoad()
        {
            _loadLazyMember = true;
        }

        /// <summary>
        /// 检查是否允许加载指定属性
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns>是否允许加载属性</returns>
        protected virtual bool AllowLazyLoad(string property)
        {
            if (!_loadLazyMember || _allowLoadPropertys == null || !_allowLoadPropertys.ContainsKey(property))
            {
                return false;
            }
            return _allowLoadPropertys[property];
        }

        /// <summary>
        /// 检查是否允许加载指定属性
        /// </summary>
        /// <param name="property">属性</param>
        /// <returns>是否允许加载属性</returns>
        protected virtual bool AllowLazyLoad(Expression<Func<T, dynamic>> property)
        {
            if (property == null)
            {
                return false;
            }
            return AllowLazyLoad(ExpressionHelper.GetExpressionPropertyName(property.Body));
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// 移除方法
        /// </summary>
        public abstract void Remove();

        ///// <summary>
        ///// 保存存储数据
        ///// </summary>
        ///// <param name="modelTypeFullName">类型全称</param>
        ///// <param name="data">要保存的数据</param>
        //void SetStoreData(string modelTypeFullName, T data)
        //{
        //    if (modelTypeFullName.IsNullOrEmpty() || data == null)
        //    {
        //        return;
        //    }
        //    if (storeDataList.ContainsKey(modelTypeFullName))
        //    {
        //        storeDataList[modelTypeFullName] = data;
        //    }
        //    else
        //    {
        //        storeDataList.Add(modelTypeFullName, data);
        //    }
        //}

        ///// <summary>
        ///// 保存存储数据
        ///// </summary>
        ///// <typeparam name="DT">数据类型，必须继承自聚合根类型</typeparam>
        ///// <param name="data">数据</param>
        //public void SetStoreData<DT>(DT data) where DT : T
        //{
        //    string typeFullName = typeof(DT).FullName;
        //    SetStoreData(typeFullName, data);
        //}

        ///// <summary>
        ///// 获取保存数据
        ///// </summary>
        ///// <param name="modelTypeFullName">类型全名</param>
        //T GetStoreData(string modelTypeFullName)
        //{
        //    if (modelTypeFullName.IsNullOrEmpty() || !storeDataList.ContainsKey(modelTypeFullName))
        //    {
        //        return default(T);
        //    }
        //    return storeDataList[modelTypeFullName];
        //}

        ///// <summary>
        ///// 获取保存数据
        ///// </summary>
        ///// <typeparam name="DT">数据类型，必须继承自聚合根类型</typeparam>
        //public DT GetStoreData<DT>() where DT : class, T
        //{
        //    var typeName = typeof(DT).FullName;
        //    T data = GetStoreData(typeName);
        //    if (data == null || !(data is DT))
        //    {
        //        return default(DT);
        //    }
        //    return data as DT;
        //}

        ///// <summary>
        ///// 合并对象存储数据
        ///// </summary>
        ///// <param name="otherObjects">源数据对象</param>
        //internal void MergeStoreData<DT>(params DT[] otherObjects) where DT : AggregationRoot<T>
        //{
        //    if (otherObjects == null || otherObjects.Length <= 0)
        //    {
        //        return;
        //    }
        //    foreach (var sourceObject in otherObjects)
        //    {
        //        if (sourceObject == null)
        //        {
        //            continue;
        //        }
        //        foreach (var dataItem in sourceObject.StoreDatas)
        //        {
        //            SetStoreData(dataItem.Key, dataItem.Value);
        //        }
        //    }
        //}

        /// <summary>
        /// 根据其他相似对象初始化当前对象
        /// </summary>
        /// <typeparam name="DT">数据类型</typeparam>
        /// <param name="similarObject">相似对象</param>
        /// <returns></returns>
        public virtual void InitFromSimilarObject<DT>(DT similarObject) where DT : AggregationRoot<T>, T
        {
            if (similarObject == null)
            {
                return;
            }
            MarkLifeStatus(similarObject.LifeStatus);
            CopyDataFromSimilarObject(similarObject);//复制数据
            //合并存储数据
            if (similarObject.StoredData != null)
            {
                if (StoredData == null)
                {
                    StoredData = similarObject.StoredData;
                }
                else
                {
                    StoredData.CopyDataFromSimilarObject(similarObject.StoredData);
                }
            }
        }

        /// <summary>
        /// 从相似的对象拷贝数据
        /// </summary>
        /// <typeparam name="DT">数据类型</typeparam>
        /// <param name="similarObject">相识对象</param>
        /// <param name="excludePropertys">排除不复制的属性</param>
        protected virtual void CopyDataFromSimilarObject<DT>(DT similarObject, IEnumerable<string> excludePropertys = null) where DT : T
        {
        }

        /// <summary>
        /// 初始化对象唯一标识值
        /// </summary>
        public virtual void InitPrimaryValue()
        {

        }

        /// <summary>
        /// 验证对象主要标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        public abstract bool PrimaryValueIsNone();

        /// <summary>
        /// 验证值是否发生了改变
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">属性</param>
        /// <returns></returns>
        protected virtual bool ValueIsChanged(Func<T, dynamic> property)
        {
            if (property == null || StoredData == null)
            {
                return true;
            }
            var newValue = property((T)this);
            var oldValue = property(StoredData);
            if (newValue == null && oldValue == null)
            {
                return false;
            }
            if (newValue == null || oldValue == null)
            {
                return true;
            }
            return !newValue.Equals(oldValue);
        }

        #endregion
    }
}
