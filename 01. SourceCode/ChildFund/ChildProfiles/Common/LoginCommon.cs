using ChildProfiles.Business.Business;
using ChildProfiles.Model;
using ChildProfiles.Model.UserModels;
using Newtonsoft.Json;
using NTS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ChildProfiles.Common
{
    public static class LoginCommon
    {
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
        public static string GenLeaningStatus(string paramValue, bool? bandicap)
        {
            string rs = "";
            switch (paramValue)
            {
                case Constants.LeaningChildhood:
                    rs = "Còn nhỏ/ Too young";
                    if (bandicap != null && bandicap == true)
                    {
                        rs += ", Khuyết tật/ Disability";
                    }
                    break;
                case Constants.LeaningDropout:
                    rs = "Bỏ học/ Drop-out";
                    if (bandicap != null && bandicap == true)
                    {
                        rs += ", Khuyết tật/ Disability";
                    }
                    break;
                case Constants.LeaningHandicapped:
                    rs = "Khuyết tật/ Disability";
                    break;
                case Constants.LeaningKindergarten:
                    rs = "Học mẫu giáo/ Attending kindergarten";
                    if (bandicap != null && bandicap == true)
                    {
                        rs += ", Khuyết tật/ Disability";
                    }
                    break;
                case Constants.LeaningPrimarySchool:
                    rs = "Học tiểu học (cấp 1)/ Primary school";
                    if (bandicap != null && bandicap == true)
                    {
                        rs += ", Khuyết tật/ Disability";
                    }
                    break;
                case Constants.LeaningHighSchool:
                    rs = "Học trung học cơ sở (cấp 2)/ Secondary school";
                    if (bandicap != null && bandicap == true)
                    {
                        rs += ", Khuyết tật/ Disability";
                    }
                    break;
                default:
                    rs = "Chưa xác định";
                    break;
            }
            return rs;
        }

        public static string GenInfoBase(ObjectBaseModel data, bool? isOther = null)
        {
            string result = "";
            try
            {
                if (data != null)
                {
                    data.ListObject = data.ListObject.Where(u => u.Check == true).ToList();
                    for (int i = 0; i < data.ListObject.Count; i++)
                    {
                        if (data.ListObject[i].Check)
                        {
                            if (i == 0)
                            {
                                result += data.ListObject[i].Name;
                            }
                            else
                            {
                                result += ", " + data.ListObject[i].Name;
                            }
                        }

                    }

                    //Thông tin khác
                    if (isOther != null && isOther == true && !string.IsNullOrEmpty(data.OtherValue))
                    {
                        result = (!string.IsNullOrEmpty(result) ? (result + ", " + data.OtherValue) : data.OtherValue);
                    }
                }

            }
            catch (Exception)
            { }
            return result;
        }

        /// <summary>
        /// Thông tin gia đình
        /// </summary>
        /// <param name="familyMember"></param>
        /// <returns></returns>
        public static string GetInfoFamily(FamilyMemberModel familyMember)
        {
            ComboboxDA comboboxDA = new ComboboxDA();
            string result = "";
            try
            {
                if (familyMember != null)
                {
                    result += familyMember.Name;

                    result += " | ";
                    if (familyMember.DateOfBirth.HasValue)
                    {
                        result += familyMember.DateOfBirth.Value.Year;
                    }

                    result += " | ";
                    result += familyMember.Gender == 1 ? "Nam/ Male" : "Nữ/ Female";

                    result += " | ";
                    if (!string.IsNullOrEmpty(familyMember.RelationshipId))
                    {
                        var itemRe = comboboxDA.GetRelationship(familyMember.RelationshipId);
                        result += itemRe.Name;
                    }

                    result += " | ";
                    if (!string.IsNullOrEmpty(familyMember.Job))
                    {
                        var itemRe = comboboxDA.GetJob(familyMember.Job);
                        result += itemRe.Name;
                    }

                    result += " | ";
                    result += familyMember.LiveWithChild == 1 ? "Có sống cùng trẻ" : "Không sống cùng trẻ";
                }

            }
            catch (Exception)
            { }
            return result;
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