using ChildProfiles.Model;
using ChildProfiles.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChildProfiles.Business.Business
{
    public class ComboboxDA
    {
        private static ChildProfileEntities _dbCbb = new ChildProfileEntities();

        /// <summary>
        /// dân tộc
        /// </summary>
        /// <returns></returns>
        public List<ComboboxResult> GetNationCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Ethnics.AsNoTracking()
                                orderby a.OrderNumber
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                // throw ex;
            }
            return searchResult;
        }

        public object GetAreaUserCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.AreaUsers.AsNoTracking()
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

        public object GetAllAreaUser()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.AreaUsers.AsNoTracking()
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

        public object GetAreaDistrict(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.AreaDistricts.AsNoTracking()
                                where a.AreaUserId == id
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.DistrictId,
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

        public object GetAreaWard(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.AreaWards.AsNoTracking()
                                where a.DistrictId == id
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.WardId,
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

        public object GetAllDepartment()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                //TODO
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }

        /// <summary>
        /// tôn giáo
        /// </summary>
        /// <returns></returns>
        public List<ComboboxResult> GetGeligionCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Religions.AsNoTracking()
                                orderby a.OrderNumber
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
        public List<ComboboxResult> GetJobCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Jobs.AsNoTracking()
                                orderby a.OrderNumber
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    PId = a.NameEn
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> ImageCreateByCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Users.AsNoTracking()
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
        public List<ComboboxResult> GetProvinceCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Provinces.AsNoTracking()
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
                searchResult = (from a in _dbCbb.Districts.AsNoTracking()
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
                searchResult = (from a in _dbCbb.Wards.AsNoTracking()
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

        public List<ComboboxResult> RelationshipCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Relationships.AsNoTracking()
                                orderby a.OrderNumber
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    PId = a.Gender != null ? a.Gender.ToString() : "1"
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> GetProvinceByUser(string areaUserId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.AreaUsers.AsNoTracking()
                                where a.Id.Equals(areaUserId)
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.ProvinceId,
                                    Name = a.ProvinceName,
                                }
                               ).ToList();
            }
            catch (Exception)
            { }
            return searchResult;
        }
        public List<ComboboxResult> GetDistrictByUser(string id, string DistrictId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.AreaDistricts.AsNoTracking()
                                where a.ProvinceId.Equals(id)
                                && (string.IsNullOrEmpty(DistrictId) || a.DistrictId.Equals(DistrictId))
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.DistrictId,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception)
            { }
            return searchResult;
        }
        public List<ComboboxResult> GetWardByUser(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.AreaWards.AsNoTracking()
                                where a.DistrictId.Equals(id)
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.WardId,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception)
            { }
            return searchResult;
        }

        /// <summary>
        /// Lấy danh sách tỉnh thuộc id vùng
        /// </summary>
        /// <param name="areaUserId">Id vùng tỉnh</param>
        /// <returns></returns>
        public List<ComboboxResult> GetProvinceByArea(string areaUserId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.AreaUsers.AsNoTracking()
                                where a.Id.Equals(areaUserId)
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.ProvinceId,
                                    Name = a.ProvinceName,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }

        /// <summary>
        ///  Lấy danh sách huyện thuộc id vùng
        /// </summary>
        /// <param name="areaDistrictId">Id vùng huyen</param>
        /// <returns></returns>
        public List<ComboboxResult> GetDistrictByArea(string areaDistrictId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.AreaDistricts.AsNoTracking()
                                where a.DistrictId.Equals(areaDistrictId)
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.DistrictId,
                                    Name = a.Name,
                                    PId = a.ProvinceId
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }

        /// <summary>
        ///  Lấy danh sách xã thuộc id vùng
        /// </summary>
        /// <param name="areaDistrictId">Id vùng huyện</param>
        /// <returns></returns>
        public List<ComboboxResult> GetWardByArea(string areaWardId)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.AreaWards.AsNoTracking()
                                where a.WardId.Equals(areaWardId)
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.WardId,
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

        //địa bàn trang chủ
        public object GetAllAreaHome()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.AreaUsers.AsNoTracking()
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.ProvinceId,
                                    Name = a.ProvinceName,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }

        public object GetAreaDistrictHome(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.AreaDistricts.AsNoTracking()
                                where a.ProvinceId.Equals(id)
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.DistrictId,
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

        //danh sách survey
        public object GetAllSurvey()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Surveys.AsNoTracking()
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id Xa</param>
        public List<ComboboxResult> GetVillageByWrad(string id)
        {
            List<ComboboxResult> listVillage = new List<ComboboxResult>();
            try
            {
                listVillage = _dbCbb.Villages.Where(r => r.WardId.Equals(id))
                    .Select(r => new ComboboxResult()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        PId = r.WardId
                    }).ToList();
            }
            catch (Exception ex)
            {
            }

            return listVillage;
        }

        public List<ComboboxResult> GetReportContent()
        {
            List<ComboboxResult> listReportContent = new List<ComboboxResult>();
            try
            {
                listReportContent = _dbCbb.ReportContents
                    .OrderBy(r => r.Index)
                    .Select(r => new ComboboxResult()
                    {
                        Id = r.Id,
                        Name = r.Content
                    }).ToList();
            }
            catch (Exception ex)
            {
            }

            return listReportContent;
        }

        public List<ComboboxResult> GetDocumentTyeCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.DocumentTyes.AsNoTracking()
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

        public List<ComboboxResult> GetSchool()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Schools.AsNoTracking()
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.SchoolName,
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

        public List<ComboboxResult> GetSchoolWard(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Schools.AsNoTracking()
                                where a.WardId.Equals(id)
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.SchoolName,
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
        public Province GetProvinceByDistrictId(string id)
        {
            try
            {
                var pro = (from p in _dbCbb.Provinces.AsNoTracking()
                           join d in _dbCbb.Districts.AsNoTracking()
                          on p.Id equals d.ProvinceId
                           where d.Id.Equals(id)
                           select p).FirstOrDefault();
                return pro;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Province GetProvinceById(string id)
        {
            try
            {
                var province = _dbCbb.Provinces.FirstOrDefault(u => u.Id.Equals(id));
                return province;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public District GetDistrictById(string id)
        {
            try
            {
                var dis = _dbCbb.Districts.FirstOrDefault(u => u.Id.Equals(id));
                return dis;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public District GetDistrictByWardId(string id)
        {
            try
            {
                var pro = (from p in _dbCbb.Districts.AsNoTracking()
                           join d in _dbCbb.Wards.AsNoTracking()
                          on p.Id equals d.DistrictId
                           where d.Id.Equals(id)
                           select p).FirstOrDefault();
                return pro;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Ward GetWardById(string id)
        {
            try
            {
                var wards = _dbCbb.Wards.FirstOrDefault(u => u.Id.Equals(id));
                return wards;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ComboboxResult GetRelationship(string id)
        {
            ComboboxResult searchResult = new ComboboxResult();
            try
            {
                searchResult = (from a in _dbCbb.Relationships.AsNoTracking()
                                where a.Id.Equals(id)
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name
                                }
                               ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ComboboxResult GetJob(string id)
        {
            ComboboxResult searchResult = new ComboboxResult();
            try
            {
                searchResult = (from a in _dbCbb.Jobs.AsNoTracking()
                                where a.Id.Equals(id)
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    UnsignName = a.NameEn
                                }
                               ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListProvinceById()
        {
            var result = _dbCbb.Provinces.AsNoTracking().Select(r => new ComboboxResult
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

            return result;
        }

        public List<ComboboxResult> GetListDistrictByProviceId(string provinceId)
        {
            var result = (from p in _dbCbb.Provinces.AsNoTracking()
                          where (!string.IsNullOrEmpty(provinceId) && p.Id.Equals(provinceId))
                          join d in _dbCbb.Districts.AsNoTracking() on p.Id equals d.ProvinceId
                          select new ComboboxResult
                          {
                              Id = d.Id,
                              Name = d.Name
                          }).ToList();

            return result;
        }

        public List<ComboboxResult> GetListWardByDistrictId(string districtId)
        {
            var result = (from d in _dbCbb.Districts.AsNoTracking()
                          where (!string.IsNullOrEmpty(districtId) && d.Id.Equals(districtId))
                          join w in _dbCbb.Wards.AsNoTracking() on d.Id equals w.DistrictId
                          select new ComboboxResult
                          {
                              Id = w.Id,
                              Name = w.Name
                          }).ToList();

            return result;
        }
    }
}