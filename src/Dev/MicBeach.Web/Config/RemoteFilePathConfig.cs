using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Web.Config
{
    /// <summary>
    /// 远程文件访问
    /// </summary>
    public class RemoteFilePathConfig
    {
        /// <summary>
        /// 远程文件访问路径配置
        /// </summary>
        public Dictionary<string, List<string>> Urls
        {
            get; set;
        } = new Dictionary<string, List<string>>();

        /// <summary>
        /// 获取远程文件访问配置信息
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public List<string> GetPaths(string key)
        {
            if (string.IsNullOrWhiteSpace(key) || Urls == null || !Urls.ContainsKey(key))
            {
                return new List<string>(0);
            }
            return Urls[key];
        }

        /// <summary>
        /// 添加新的路径，若当前已存在指定键值的配置，则合并
        /// </summary>
        /// <param name="key">配置键值</param>
        /// <param name="paths">新的路径信息</param>
        /// <returns>返回最新的路径信息</returns>
        public List<string> AddPaths(string key, params string[] paths)
        {
            if (string.IsNullOrWhiteSpace(key) || paths == null || paths.Length <= 0)
            {
                return new List<string>(0);
            }
            if (Urls == null)
            {
                Urls = new Dictionary<string, List<string>>();
            }
            List<string> newPaths = null;
            if (Urls.ContainsKey(key))
            {
                newPaths = Urls[key];
            }
            else
            {
                newPaths = new List<string>();
                Urls.Add(key, newPaths);
            }
            newPaths = newPaths ?? new List<string>();
            newPaths.AddRange(paths);
            return newPaths;
        }
    }
}
