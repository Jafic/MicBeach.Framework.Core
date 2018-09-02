using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Web.Config
{
    /// <summary>
    /// 远程配置选项
    /// </summary>
    public class RemoteUploadConfigOption
    {
        /// <summary>
        /// 上传文件操作路径
        /// </summary>
        public string UploadAction
        {
            get; set;
        } = "upfile";

        /// <summary>
        /// 上传文件路径
        /// </summary>
        public string FileListAction
        {
            get; set;
        } = "filelist";

        /// <summary>
        /// 上传服务器
        /// </summary>
        public string Server
        {
            get; set;
        }

        /// <summary>
        /// 获取上传路径
        /// </summary>
        /// <returns></returns>
        public string GetUploadUrl()
        {
            if (string.IsNullOrWhiteSpace(UploadAction))
            {
                return Server;
            }
            return string.Format("{0}/{1}", Server.Trim('/'), UploadAction);
        }

        /// <summary>
        /// 文件列表路径
        /// </summary>
        /// <returns></returns>
        public string GetFileList()
        {
            if (string.IsNullOrWhiteSpace(FileListAction))
            {
                return Server;
            }
            return string.Format("{0}/{1}", Server.Trim('/'), FileListAction);
        }
    }
}
