using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Site.Cms.Controllers.Base;
using Site.Cms.Util.Editor;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Site.Cms.Controllers
{
    public class EditorController : WebBaseController
    {
        public void Process()
        {
            var context = HttpContext;
            Handler action = null;
            switch (context.Request.Query["action"])
            {
                case "config":
                    action = new ConfigHandler(context);
                    break;
                case "uploadimage":
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = EditorConfig.GetStringList("imageAllowFiles"),
                        PathFormat = EditorConfig.GetString("imagePathFormat"),
                        SizeLimit = EditorConfig.GetInt("imageMaxSize"),
                        UploadFieldName = EditorConfig.GetString("imageFieldName")
                    });
                    break;
                case "uploadscrawl":
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = new string[] { ".png" },
                        PathFormat = EditorConfig.GetString("scrawlPathFormat"),
                        SizeLimit = EditorConfig.GetInt("scrawlMaxSize"),
                        UploadFieldName = EditorConfig.GetString("scrawlFieldName"),
                        Base64 = true,
                        Base64Filename = "scrawl.png"
                    });
                    break;
                case "uploadvideo":
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = EditorConfig.GetStringList("videoAllowFiles"),
                        PathFormat = EditorConfig.GetString("videoPathFormat"),
                        SizeLimit = EditorConfig.GetInt("videoMaxSize"),
                        UploadFieldName = EditorConfig.GetString("videoFieldName")
                    });
                    break;
                case "uploadfile":
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = EditorConfig.GetStringList("fileAllowFiles"),
                        PathFormat = EditorConfig.GetString("filePathFormat"),
                        SizeLimit = EditorConfig.GetInt("fileMaxSize"),
                        UploadFieldName = EditorConfig.GetString("fileFieldName")
                    });
                    break;
                case "listimage":
                    action = new ListFileManager(context, EditorConfig.GetString("imageManagerListPath"), EditorConfig.GetStringList("imageManagerAllowFiles"));
                    break;
                case "listfile":
                    action = new ListFileManager(context, EditorConfig.GetString("fileManagerListPath"), EditorConfig.GetStringList("fileManagerAllowFiles"));
                    break;
                case "catchimage":
                    action = new CrawlerHandler(context);
                    break;
                default:
                    action = new NotSupportedHandler(context);
                    break;
            }
            action.Process();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
