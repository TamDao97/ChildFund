using InformationHub.Business.Business;
using InformationHub.Model;
using InformationHub.Model.UserModels;
using Newtonsoft.Json;
using NTS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using UserModel = InformationHub.Model.UserModels.UserModel;

namespace InformationHub.Common
{
    public static class LoginCommon
    {
        public static bool CheckPermission(string code)
        {
            var rs = false;
            try
            {
                var UserId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(UserId);
                if (userInfo!=null)
                {
                    rs = userInfo.ListRoles.Contains(code);
                }
            }
            catch (Exception)
            { }
            return rs;
        }
        public static string GetUserName()
        {
            string rs = "";
            try
            {
                var UserId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(UserId);
                rs = userInfo.Name;
            }
            catch (Exception)
            { }
            return rs;
        }
        public static UserModel GetCookies()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("admins");
            if (cookie != null)
            {
                UserModel modelInfo = new UserModel();
                modelInfo = JsonConvert.DeserializeObject<UserModel>(cookie.Value);
                return modelInfo;
            }
            else
            {
                return null;
            }
        }
    }
    public static class CommonProcess
    {
        public static string GenLeaningStatus(string paramValue)
        {
            string rs = "";
            switch (paramValue)
            {
                case Constants.LeaningChildhood:
                    rs = "Còn nhỏ";
                    break;
                case Constants.LeaningDropout:
                    rs = "Bỏ học";
                    break;
                case Constants.LeaningHandicapped:
                    rs = "Khuyết tật";
                    break;
                case Constants.LeaningKindergarten:
                    rs = "Mẫu giáo";
                    break;
                case Constants.LeaningPrimarySchool:
                    rs = "Tiểu học";
                    break;
                case Constants.LeaningHighSchool:
                    rs = "Trung học";
                    break;
                default:
                    rs = "Chưa xác định";
                    break;
            }
            return rs;
        }

        #region Name To Tag
        public static string NameToTag(string strName)
        {
            string strReturn = "";
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            strReturn = Regex.Replace(strName, "[^\\w\\s]", string.Empty).Replace(" ", "-").ToLower();
            string strFormD = strReturn.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, string.Empty).Replace("đ", "d");
        }
        #endregion
    }

}