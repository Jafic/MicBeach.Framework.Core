using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MicBeach.DTO.Sys;
using MicBeach.DTO.Sys.Cmd;
using MicBeach.Develop.CQuery;
using MicBeach.Util.Drawing;
using MicBeach.Web.Utility;
using MicBeach.Web.Mvc;
using MicBeach.ViewModel.Sys;
using Site.Cms.Controllers.Base;
using MicBeach.Util;
using Site.Cms.Helper;
using MicBeach.Util.Response;
using MicBeach.ServiceContract.Sys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Site.Cms.Controllers
{
    public class HomeController : WebBaseController
    {
        IUserService userService;
        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult Login(LoginViewModel loginInfo)

        {
            if (!IsPost)
            {
                UserHelper.LoginOut();
                return View("Login");
            }
            if (!ModelState.IsValid)
            {
                return Json(Result.FailedResult("登陆信息错误"));
            }
            return Json(UserHelper.Login(loginInfo));
        }

        [AllowAnonymous]
        public ActionResult VCode()
        {
            byte[] byteValues = VerificationCodeHelper.RefreshLoginCode();
            return File(byteValues, "image/jpeg");
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Authentication()
        {
            return View();
        }
    }
}