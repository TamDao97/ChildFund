using InformationHub.Business.Business;
using InformationHub.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NTS.Utils;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace InformationHub.Controllers
{
    public class AuthorizeController : BaseController
    {
        private AuthorizeBusiness authorizeBusiness = new AuthorizeBusiness();
        // GET: Authorize
        public ActionResult Login()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut();
            authorizeBusiness.RemoveCache();
            return View();
        }

        // GET: Authorize
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            try
            {
                var securityKey = Guid.NewGuid().ToString();
                loginModel.SecurityKey = securityKey;
                LoginProfileModel loginProfileModel = authorizeBusiness.Login(loginModel);
                var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, loginProfileModel.Id),
                new Claim(ClaimTypes.Uri,string.IsNullOrEmpty(loginProfileModel.ImagePath)?"":loginProfileModel.ImagePath),
                new Claim(ClaimTypes.Version, securityKey)}, "ApplicationCookie");
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identity);
                int AdminTimeOut = 3;
                try
                {
                    AdminTimeOut = int.Parse(ConfigurationManager.AppSettings["AdminTimeOut"]);
                }
                catch (Exception)
                { }
                Session["AdminTimeOut"] = loginModel.UserName;
                Session.Timeout = AdminTimeOut;
              
                return Json(new { Ok = true, type = loginProfileModel.Type }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ErrorMessage.ConvertMessage(ex) }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Logout()
        {
            //var ctx = Request.GetOwinContext();
            //var authManager = ctx.Authentication;
            //authManager.SignOut();
            authorizeBusiness.RemoveCache();
            return Redirect("/Authorize/Login");
        }

        /// <summary>
        /// get thông tin hồ sơ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProfileUser()
        {
            try
            {
                ProfileUserModel loginProfileModel = authorizeBusiness.GetProfileUser(System.Web.HttpContext.Current.User.Identity.Name);
                return Json(new { Ok = true, Data = loginProfileModel }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ErrorMessage.ConvertMessage(ex) }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Cập nhật hồ sơ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize]
        public ActionResult UpdateProfileUser()
        {
            try
            {
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                var modelJson = Request.Form["Model"];
                ProfileUserModel model = JsonConvert.DeserializeObject<ProfileUserModel>(modelJson, dateTimeConverter);
                model.UpdateBy = model.Id = System.Web.HttpContext.Current.User.Identity.Name;
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;

                var result = authorizeBusiness.UpdateProfileUser(model, httpFile);
                return Json(new { Ok = true, Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ErrorMessage.ConvertMessage(ex) }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize]
        public ActionResult ChangePasswordUser(ChangePasswordUserModel model)
        {
            try
            {
                model.Id = System.Web.HttpContext.Current.User.Identity.Name;
                bool result = authorizeBusiness.ChangePasswordUser(model);
                return Json(new { Ok = true, Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ErrorMessage.ConvertMessage(ex) }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Authorize
        public ActionResult ForwardPassword()
        {
            return View();
        }

        // GET: Authorize
        public ActionResult ConfirmForwardPassword()
        {
            return View();
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ForwardPassword(ForwardPasswordModel model)
        {
            try
            {
                string userId = authorizeBusiness.ForwardPassword(model);

                if (!string.IsNullOrEmpty(userId))
                {
                    var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, userId)}, "ApplicationCookie");

                    var ctx = Request.GetOwinContext();
                    var authManager = ctx.Authentication;
                    authManager.SignIn(identity);
                }
                return Json(new { Ok = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ErrorMessage.ConvertMessage(ex) }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConfirmForwardPassword(ForwardPasswordModel model)
        {
            try
            {
                model.Id = System.Web.HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(model.Id))
                {
                    return Redirect("/Authorize/ForwardPassword");
                }
                bool result = authorizeBusiness.ConfirmForwardPassword(model);
                return Json(new { Ok = true, Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ErrorMessage.ConvertMessage(ex) }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CheckTimeOut()
        {
            if (Session["AdminTimeOut"] != null)
            {
                return Json(new { Ok = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Ok = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}