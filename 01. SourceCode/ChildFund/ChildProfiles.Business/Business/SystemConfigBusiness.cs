using ChildProfiles.Model;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model.Model.SystemConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Business.Business
{
    public class SystemConfigBusiness
    {
        private readonly ChildProfileEntities db = new ChildProfileEntities();

        public SearchResultObject<SchoolModel> GetListSchool(SchoolSearchCondition searchModel)
        {
            SearchResultObject<SchoolModel> result = new SearchResultObject<SchoolModel>();

            var query = (from s in db.Schools.AsNoTracking()
                         join w in db.Wards.AsNoTracking() on s.WardId equals w.Id
                         join d in db.Districts.AsNoTracking() on w.DistrictId equals d.Id
                         join p in db.Provinces.AsNoTracking() on d.ProvinceId equals p.Id
                         select new SchoolModel
                         {
                             Id = s.Id,
                             ProvinceName = p.Name,
                             DistrictName = d.Name,
                             WardName = w.Name,
                             SchoolName = s.SchoolName
                         }).OrderBy(r => new { r.ProvinceName, r.DistrictName, r.WardName })
                         .AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.SchoolName))
            {
                query = query.Where(r => r.SchoolName.ToUpper().Contains(searchModel.SchoolName));
            }

            result.TotalItem = query.Select(r => r.Id).Count();

            query = query
                .Skip((searchModel.PageNumber - 1) * searchModel.PageSize)
                .Take(searchModel.PageSize);

            result.ListResult = query.ToList();

            return result;
        }

        public bool Save(SchoolModel model)
        {
            try
            {
                bool isSuccess = false;
                School school = new School();

                if (string.IsNullOrEmpty(model.Id))
                {
                    school = new School
                    {
                        Id = Guid.NewGuid().ToString(),
                        WardId = model.WardId,
                        SchoolName = model.SchoolName
                    };

                    db.Schools.Add(school);
                }
                else
                {
                    school = db.Schools.Find(model.Id);
                    school.WardId = model.WardId;
                    school.SchoolName = model.SchoolName;
                }

                db.SaveChanges();
                isSuccess = true;
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SchoolModel GetDetailSchool(string id)
        {
            try
            {
                var model = (from s in db.Schools.AsNoTracking()
                             where s.Id.Equals(id)
                             join w in db.Wards.AsNoTracking() on s.WardId equals w.Id
                             join d in db.Districts.AsNoTracking() on w.DistrictId equals d.Id
                             join p in db.Provinces.AsNoTracking() on d.ProvinceId equals p.Id
                             select new SchoolModel
                             {
                                 Id = s.Id,
                                 WardId = w.Id,
                                 DistrictId = d.Id,
                                 ProvinceId = p.Id,
                                 SchoolName = s.SchoolName
                             }).FirstOrDefault();

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(string Id)
        {
            try
            {
                bool isSuccess = false;
                var model = db.Schools.Find(Id);

                if (model != null)
                {
                    db.Schools.Remove(model);
                    db.SaveChanges();
                    isSuccess = true;
                    return isSuccess;
                }

                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
