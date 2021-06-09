using InformationHub.Business.Business;
using InformationHub.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace InformationHub
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        private AuthorizeBusiness authorizeBusiness = new AuthorizeBusiness();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool check = false;
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var id = httpContext.User.Identity.Name;
                check = CheckVersionLogin(id);
                if (check)
                {
                    var roles = this.Roles;
                    if (!string.IsNullOrEmpty(roles))
                    {
                        var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(id);
                        if (userInfo!=null)
                        {
                            check = userInfo.ListRoles.Contains(roles);
                        }
                    }
                }
            }
            if (check == false) httpContext.Response.Redirect("~/Authorize/Login", true);
            return check;
        }

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    // If they are authorized, handle accordingly
        //    if (this.AuthorizeCore(filterContext.HttpContext))
        //    {
        //        base.OnAuthorization(filterContext);
        //    }
        //    else
        //    {
        //        // Otherwise redirect to your specific authorized area
        //        filterContext.Result = new RedirectResult("~/Authorize/Login");
        //    }
        //}

        /// <summary>
        /// Danh sách quyền người dùng
        /// </summary>
        /// <param name="id">Id người dùng</param>
        /// <returns></returns>
        private List<string> GetRolesForUser(string id)
        {
            LoginProfileModel loginProfileModel = authorizeBusiness.GetCacheLoginProfile(id);
            if (loginProfileModel != null && string.IsNullOrEmpty(loginProfileModel.Id))
            {
                return null;// loginProfileModel.ListRoles;
            }
            return new List<string>();
        }

        /// <summary>
        /// Check quyền
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="rolesUser"></param>
        /// <returns></returns>
        private bool CheckRoles(List<string> roles, List<string> rolesUser)
        {
            foreach (var item in roles)
            {
                if (rolesUser.Where(r => r.Equals(item)).Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckUsers(string[] users, string userName)
        {
            var countUser = users.Where(r => r.Equals(userName));
            if (countUser.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckVersionLogin(string id)
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            AuthorizeBusiness authorBU = new AuthorizeBusiness();
            var loginProfile = authorBU.GetCacheLoginProfile(id);
            if (identity != null)
            {
                var claimVersion = identity.Claims.Where(t => t.Type.Contains("version")).FirstOrDefault();
                if (claimVersion != null)
                {
                    var version = claimVersion.Value;
                    if (loginProfile != null && !string.IsNullOrEmpty(loginProfile.SecurityKey))
                    {
                        if (loginProfile.SecurityKey.Equals(version)) return true;
                    }

                }
            }
            return false;
        }
    }
}