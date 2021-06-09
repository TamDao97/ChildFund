using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using InformationHub.Business.Business;
using InformationHub.Model;
using InformationHub.Model.Repositories;
using NTS.Common;

namespace InformationHub.Business
{
    public class ComboboxBusiness
    {
        private InformationHubEntities db = new InformationHubEntities();
        public List<ComboboxResult> GetAllAbuseType()
        {
            try
            {
                string lang = "Vi";
                if (HttpContext.Current.Request.Cookies["culture"] != null)
                {
                    lang = HttpContext.Current.Request.Cookies["culture"].Value;
                }
                List<ComboboxResult> listAbuseType = db.AbuseTypes.OrderBy(u => u.OrderNumber).Select(r => new ComboboxResult()
                {
                    Id = r.Id,
                    Name = lang.Equals("Vi") ? r.Name : r.Note,
                    ThenNumber=r.ThenNumber

                }).ToList();
                return listAbuseType;
            }
            catch
            {
                return new List<ComboboxResult>();
            }
        }
        public List<ComboboxResult> GetProvinceCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.Provinces.AsNoTracking()
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> GetDistrictCBB(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.Districts.AsNoTracking()
                                where a.ProvinceId == id
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    PId = id
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> GetWardCBB(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.Wards.AsNoTracking()
                                where a.DistrictId == id
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    PId = id
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> GetUserAreaCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.AreaUsers.AsNoTracking()
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    PId = a.ProvinceId,
                                    Exten = a.ProvinceName
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> GetDistrictAreaCBB(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.AreaDistricts.AsNoTracking()
                                where a.ProvinceId.Equals(id)
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    PId = a.DistrictId
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> GetWardAreaCBB(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.AreaWards.AsNoTracking()
                                where a.DistrictId.Equals(id)
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    PId = a.WardId
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> GetGroupUserCBB(int type)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.GroupUsers.AsNoTracking()
                                where a.Type == type
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> GetDocumentTyeCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.DocumentTyes.AsNoTracking()
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> WardByUserId(string userId)
        {
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.Wards.AsNoTracking()
                                where a.Id.Equals(userInfo.WardId)
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> DistrictByWardId(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.Wards.AsNoTracking()
                                where a.Id.Equals(id)
                                join b in db.Districts.AsNoTracking() on a.DistrictId equals b.Id
                                select new ComboboxResult()
                                {
                                    Id = b.Id,
                                    Name = b.Name,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> ProvinceByWardId(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.Wards.AsNoTracking()
                                where a.Id.Equals(id)
                                join b in db.Districts.AsNoTracking() on a.DistrictId equals b.Id
                                join c in db.Provinces.AsNoTracking() on b.ProvinceId equals c.Id

                                select new ComboboxResult()
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> GetAddressByUser(LoginProfileModel userInfo)
        {

            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                if (userInfo.Type == Constants.LevelTeacher)
                {
                    searchResult = (from a in db.Wards.AsNoTracking()
                                    where a.Id.Equals(userInfo.WardId)
                                    orderby a.Name
                                    select new ComboboxResult()
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                    }
                             ).ToList();
                }
                else if (userInfo.Type == Constants.LevelArea)
                {
                    searchResult = (from a in db.Wards.AsNoTracking()
                                    where a.DistrictId.Equals(userInfo.DistrictId)
                                    orderby a.Name
                                    select new ComboboxResult()
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                    }
                             ).ToList();
                }
                else if (userInfo.Type == Constants.LevelOffice)
                {
                    searchResult = (from a in db.Districts.AsNoTracking()
                                    where a.ProvinceId.Equals(userInfo.ProvinceId)
                                    orderby a.Name
                                    select new ComboboxResult()
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                    }
                             ).ToList();
                }
                else
                {
                    searchResult = (from a in db.Provinces.AsNoTracking()
                                    orderby a.Name
                                    select new ComboboxResult()
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                    }
                             ).ToList();
                }

            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        //tìm kiếm theo dk truyền sang của ds sự vụ
        public List<ComboboxResult> GetWardByWardId(string wardId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var wards = db.Wards.FirstOrDefault(u => u.Id.Equals(wardId));
                searchResult = (from a in db.Wards.AsNoTracking()
                                where a.DistrictId.Equals(wards.DistrictId)
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    Exten = wardId,
                                    PId = wards.DistrictId
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> GetDistrictByWardId(string wardId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var districts = (from a in db.Wards.AsNoTracking()
                                 where a.Id.Equals(wardId)
                                 join b in db.Districts.AsNoTracking() on a.DistrictId equals b.Id
                                 select b
                                 ).FirstOrDefault();
                searchResult = (from a in db.Districts.AsNoTracking()
                                where a.ProvinceId.Equals(districts.ProvinceId)
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    Exten = districts.Id,
                                    PId = districts.ProvinceId
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> GetDistrictByDistrictId(string districtId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var provinceId = db.Districts.FirstOrDefault(u => u.Id.Equals(districtId)).ProvinceId;
                searchResult = (from a in db.Districts.AsNoTracking()
                                where a.ProvinceId.Equals(provinceId)
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    PId = provinceId,
                                    Exten = districtId
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }

        #region[lấy cho tống ke trang home]
        public List<ComboboxResult> GetProvinceByListId(List<string> lst)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.Provinces.AsNoTracking() where lst.Contains(a.Id)
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
            }
            return searchResult;
        }
        public List<ComboboxResult> GetDistrictByListId(List<string> lst,string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.Districts.AsNoTracking()
                                where a.ProvinceId.Equals(id) &&  lst.Contains(a.Id)
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
            }
            return searchResult;
        }
        public List<ComboboxResult> GetWardByListId(List<string> lst, string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in db.Wards.AsNoTracking()
                                where a.DistrictId.Equals(id) && lst.Contains(a.Id)
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
            }
            return searchResult;
        }
        #endregion
    }
}
