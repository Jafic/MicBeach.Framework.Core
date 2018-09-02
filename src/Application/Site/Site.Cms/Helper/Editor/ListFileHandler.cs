using MicBeach.Util.Extension;
using MicBeach.Web.Utility;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Site.Cms.Util.Editor
{
    /// <summary>
    /// FileManager 的摘要说明
    /// </summary>
    public class ListFileManager : Handler
    {
        enum ResultState
        {
            Success,
            InvalidParam,
            AuthorizError,
            IOError,
            PathNotFound
        }

        private int Start;
        private int Size;
        private int Total;
        private ResultState State;
        private String PathToList;
        private String[] FileList;
        private String[] SearchExtensions;

        public ListFileManager(HttpContext context, string pathToList, string[] searchExtensions)
            : base(context)
        {
            this.SearchExtensions = searchExtensions.Select(x => x.ToLower()).ToArray();
            this.PathToList = pathToList;
        }

        public override void Process()
        {
            try
            {
                Start = String.IsNullOrEmpty(Request.GetValue("start")) ? 0 : Convert.ToInt32(Request.GetValue("start"));
                Size = String.IsNullOrEmpty(Request.GetValue("size")) ? EditorConfig.GetInt("imageManagerListSize") : Convert.ToInt32(Request.GetValue("size"));
            }
            catch (FormatException)
            {
                State = ResultState.InvalidParam;
                WriteResult();
                return;
            }
            var buildingList = new List<String>();
            try
            {
                var localPath = Path.Combine(Directory.GetCurrentDirectory(), PathToList);
                //buildingList.AddRange(Directory.GetFiles(localPath, "*", SearchOption.AllDirectories)
                //    .Where(x => SearchExtensions.Contains(Path.GetExtension(x).ToLower()))
                //    .Select(x => PathToList + x.Substring(localPath.Length).Replace("\\", "/")));
                string imgListUrl = "";//System.Configuration.ConfigurationManager.AppSettings["ImageListUrl"];
                Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"path",@"Editor\upload\image"},
                {"suffixFilter",".png,.jpg,.jpeg,.gif,.bmp"}
            };
                string imgPathString = string.Empty; //WebRequestHelper.GetValue(imgListUrl, parameters);
                string[] pathArray = imgPathString.LSplit(",");
                buildingList.AddRange(pathArray);
                Total = buildingList.Count;
                FileList = buildingList.OrderBy(x => x).Skip(Start).Take(Size).ToArray();
            }
            catch (UnauthorizedAccessException)
            {
                State = ResultState.AuthorizError;
            }
            catch (DirectoryNotFoundException)
            {
                State = ResultState.PathNotFound;
            }
            catch (IOException)
            {
                State = ResultState.IOError;
            }
            finally
            {
                WriteResult();
            }
        }

        private void WriteResult()
        {
            WriteJson(new
            {
                state = GetStateString(),
                list = FileList == null ? null : FileList.Select(x => new { url = x }),
                start = Start,
                size = Size,
                total = Total
            });
        }

        private string GetStateString()
        {
            switch (State)
            {
                case ResultState.Success:
                    return "SUCCESS";
                case ResultState.InvalidParam:
                    return "参数不正确";
                case ResultState.PathNotFound:
                    return "路径不存在";
                case ResultState.AuthorizError:
                    return "文件系统权限不足";
                case ResultState.IOError:
                    return "文件系统读取错误";
            }
            return "未知错误";
        }
    }
}