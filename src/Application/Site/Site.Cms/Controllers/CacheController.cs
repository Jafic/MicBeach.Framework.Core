//using MicBeach.Cache;
//using MicBeach.Develop.CQuery;
//using MicBeach.Util.Paging;
//using MicBeach.DTO.Cache.Cmd;
//using MicBeach.DTO.Cache.Query;
//using MicBeach.DTO.Cache.Query.Filter;
//using MicBeach.ServiceInterface.Cache;
//using MicBeach.Util;
//using MicBeach.Util.Extension;
//using MicBeach.Util.Serialize;
//using MicBeach.ViewModel.Cache;
//using MicBeach.ViewModel.Cache.Filter;
//using MicBeach.ViewModel.Common;
//using MicBeach.Web.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using TestWeb.Controllers.Base;
//using MicBeach.Util.Response;
//using MicBeach.Cache.Request;
//using Site.Cms.Controllers.Base;

//namespace Site.Cms.Controllers
//{
//    public class CacheController : WebBaseController
//    {
//        IServerService serverService = null;

//        public CacheController()
//        {
//            serverService = this.Instance<IServerService>();
//        }

//        #region 缓存服务器管理

//        #region 缓存服务器列表

//        public ActionResult ServerList()
//        {
//            return View();
//        }

//        public ActionResult SearchServer(ServerFilterViewModel filter)
//        {
//            var filterDto = filter.MapTo<ServerFilterDto>();
//            IPaging<ServerViewModel> serverPager = serverService.GetServerPaging(filterDto).ConvertTo<ServerViewModel>();
//            //string viewContent = this.RenderViewContent("Partial/_ServerList", serverPager, null, true);
//            object objResult = new
//            {
//                TotalCount = serverPager.TotalCount,
//                Datas = JsonSerialize.ObjectToJson(serverPager.ToList())
//            };
//            return Json(objResult, JsonRequestBehavior.AllowGet);
//        }
//        #endregion

//        #region 编辑/添加缓存服务器

//        public ActionResult EditServer(ServerViewModel server)
//        {
//            if (IsPost)
//            {
//                var result = serverService.SaveServer(new SaveServerCmdDto()
//                {
//                    Server = server.MapTo<ServerCmdDto>()
//                });
//                return Json(result);
//            }
//            else if (!(server.Id <= 0))
//            {
//                ServerFilterDto filter = new ServerFilterDto()
//                {
//                    Ids = new List<long>()
//                    {
//                        server.Id
//                    }
//                };
//                server = serverService.GetServer(filter).MapTo<ServerViewModel>();
//            }
//            return View(server);
//        }

//        #endregion

//        #region 删除缓存服务器

//        public ActionResult DeleteServer(string ids)
//        {
//            IEnumerable<long> idArray = ids.LSplit(",").Select(s => long.Parse(s));
//            Result result = serverService.DeleteServer(new DeleteServerCmdDto()
//            {
//                ServerIds = idArray
//            });
//            return Json(result);
//        }

//        #endregion

//        #region 服务器详情

//        public ActionResult ServerDetail(long id)
//        {
//            var filter = new ServerFilterDto()
//            {
//                Ids = new List<long>()
//                {
//                    id
//                }
//            };
//            var server = serverService.GetServer(filter).MapTo<ServerViewModel>();
//            if (server == null)
//            {
//                return Content("信息获取失败");
//            }
//            return View(server);
//        }

//        #endregion

//        #endregion

//        #region Redis

//        [HttpPost]
//        public ActionResult GetServerDataBase(ServerViewModel server)
//        {
//            CacheServer cacheServer = server.MapTo<CacheServer>();
//            var dataBases = CacheManager.GetAllDataBaseAsync(new GetAllDataBaseRequest()
//            {
//                Server = cacheServer
//            }).Result;
//            List<TreeNode> treeNodes = dataBases.DataBaseList.Select(c => new TreeNode()
//            {
//                Value = c.Index.ToString(),
//                IsParent = false,
//                Text = c.Name
//            }).ToList();
//            var jsonData = JsonSerialize.ObjectToJson(treeNodes);
//            var result = Result.SuccessResult("获取成功");
//            result.Data = jsonData;
//            return Json(result);
//        }

//        [HttpPost]
//        public ActionResult GetKeys(ServerViewModel server, KeyFilterViewModel keyFilter)
//        {
//            CacheServer cacheServer = server.MapTo<CacheServer>();
//            cacheServer.Db = keyFilter.DbIndex.ToString();
//            KeyQuery query = new KeyQuery()
//            {
//                Page = keyFilter.Page,
//                PageSize = keyFilter.PageSize,
//                MateKey = keyFilter.KeyMateKey
//            };
//            var response = CacheManager.GetKeysAsync(new GetKeysRequest()
//            {
//                Server = cacheServer,
//                Query = query
//            }).Result;
//            var keyList = response.Keys.Select(c => new KeyItemViewModel()
//            {
//                Key = c.Key,
//                Seconds = c.Seconds,
//                Type = c.Type,
//                Value = c.Value
//            }).ToList();
//            var keyPaging = new Paging<KeyItemViewModel>(response.Keys.Page, response.Keys.PageSize, response.Keys.TotalCount, keyList);
//            //string viewContent = this.RenderViewContent("Partial/_DbKeyList", keyPaging, null, true);
//            object objResult = new
//            {
//                TotalCount = keyPaging.TotalCount,
//                Datas = JsonSerialize.ObjectToJson(keyPaging.ToList())
//            };
//            return Json(objResult, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public ActionResult RemoveKeys(ServerViewModel server, RemoveCacheKeysViewModel removeInfo)
//        {
//            CacheServer cacheServer = server.MapTo<CacheServer>();
//            cacheServer.Db = removeInfo.DbIndex.ToString();
//            CacheManager.KeyDeleteAsync(new KeyDeleteRequest()
//            {
//                Server = cacheServer,
//                Keys = removeInfo.Keys
//            }).Wait();
//            var result = Result.SuccessResult("删除成功");
//            return Json(result);
//        }

//        [HttpPost]
//        public ActionResult ClearData(ServerViewModel server, IEnumerable<int> dbs)
//        {
//            CacheServer cacheServer = server.MapTo<CacheServer>();
//            CacheManager.ClearDataAsync(new ClearDataRequest()
//            {
//                Server = cacheServer,
//                DataBaseList = dbs.Select(c => new CacheDb()
//                {
//                    Index = c
//                }).ToList()
//            }).Wait();
//            var result = Result.SuccessResult("清除成功");
//            return Json(result);
//        }

//        [HttpPost]
//        public ActionResult GetCacheServerConfig(ServerViewModel server)
//        {
//            var cacheServer = server.MapTo<CacheServer>();
//            var serverConfig = CacheManager.GetServerConfigAsync(new GetServerConfigRequest()
//            {
//                Server = cacheServer
//            }).Result.MapTo<CacheServerConfigViewModel>();
//            ViewBag.ServerId = server.Id;
//            string viewContent = this.RenderViewContent("Partial/_CacheServerConfig", serverConfig, null, true);
//            var result = Result.SuccessResult("获取成功");
//            result.Data = viewContent;
//            return Json(result);
//        }

//        [HttpPost]
//        public ActionResult ApplyCacheServerConfig(long serverId, CacheServerConfigViewModel config)
//        {
//            var server = serverService.GetServer(new ServerFilterDto()
//            {
//                Ids = new List<long>()
//                {
//                    serverId
//                }
//            }).MapTo<ServerViewModel>();
//            var cacheServer = server.MapTo<CacheServer>();
//            var configInfo = config.MapTo<CacheServerConfig>();
//            CacheManager.SaveServerConfigAsync(new SaveServerConfigRequest()
//            {
//                Server = cacheServer,
//                ServerConfig = configInfo
//            }).Wait();
//            var result = Result.SuccessResult("修改成功");
//            return Json(result);
//        }

//        [HttpPost]
//        public ActionResult ServerTestConnection(ServerViewModel server)
//        {
//            return null;
//        }

//        public ActionResult KeyItemDetail(long serverId, int dbIndex, string key)
//        {
//            var server = serverService.GetServer(new ServerFilterDto()
//            {
//                Ids = new List<long>()
//                {
//                    serverId
//                }
//            }).MapTo<ServerViewModel>();
//            var cacheServer = server.MapTo<CacheServer>();
//            cacheServer.Db = dbIndex.ToString();
//            var keyItem = CacheManager.GetKeyDetailAsync(new GetKeyDetailRequest()
//            {
//                Server = cacheServer,
//                Key = key
//            }).Result;
//            if (keyItem == null)
//            {
//                return Content("信息获取失败");
//            }
//            KeyItemViewModel keyModel = new KeyItemViewModel()
//            {
//                Key = keyItem.KeyDetail.Key,
//                Value = keyItem.KeyDetail.Value,
//                Type = keyItem.KeyDetail.Type
//            };
//            return View(keyModel);
//        }

//        public ActionResult AddItem(long serverId, int dbIndex, KeyItemViewModel item)
//        {
//            if (IsPost)
//            {
//                switch (item.Type)
//                {
//                    case CacheKeyType.List:
//                    case CacheKeyType.SortedSet:
//                    case CacheKeyType.Set:
//                        break;
//                    case CacheKeyType.Hash:
//                        break;
//                    case CacheKeyType.String:
//                        break;
//                }
//            }
//            ViewBag.ServerId = serverId;
//            ViewBag.DbIndex = dbIndex;
//            return View(item);
//        }

//        string InitStringValue()
//        {
//            return Request["string_value"];
//        }

//        List<string> InitListValue()
//        {
//            List<string> valueKeys = Request.Form.AllKeys.Where(c => c.StartsWith("list-val")).ToList();
//            List<string> values = new List<string>();
//            foreach (var key in valueKeys)
//            {
//                string nowVal = Request[key];
//                if (nowVal.IsNullOrEmpty())
//                {
//                    continue;
//                }
//                values.Add(nowVal);
//            }
//            return values;
//        }

//        Dictionary<string, string> InitHashValue()
//        {
//            List<string> nameKeys = Request.Form.AllKeys.Where(c => c.StartsWith("hash-name_")).ToList();
//            Dictionary<string, string> values = new Dictionary<string, string>();
//            foreach (var key in nameKeys)
//            {
//                string name = Request[key];
//                if (name.IsNullOrEmpty())
//                {
//                    continue;
//                }
//                string[] keyArray = key.LSplit("_");
//                if (keyArray.Length < 2)
//                {
//                    continue;
//                }
//                int inputIndex = 0;
//                if (!int.TryParse(keyArray[1], out inputIndex) || inputIndex <= 0)
//                {
//                    continue;
//                }
//                string value = Request[string.Format("hash-val_{0}", inputIndex)];
//                if (value.IsNullOrEmpty())
//                {
//                    continue;
//                }
//                if (values.ContainsKey(name))
//                {
//                    values[name] = value;
//                }
//                else
//                {
//                    values.Add(name, value);
//                }
//            }
//            return values;
//        }

//        #endregion
//    }
//}