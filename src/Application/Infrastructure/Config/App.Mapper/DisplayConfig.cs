using MicBeach.DataValidation.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Mapper
{
    /// <summary>
    /// 显示配置
    /// </summary>
    public static class DisplayConfig
    {
        public static void Init()
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data/Config/Display");
            if (Directory.Exists(folderPath))
            {
                var files = Directory.GetFiles(folderPath).Where(c => Path.GetExtension(c).Trim('.').ToLower() == "disconfig").ToArray();
                MicBeach.DataValidation.Config.DisplayConfig.InitFromFiles(files);
            }
        }
    }
}
