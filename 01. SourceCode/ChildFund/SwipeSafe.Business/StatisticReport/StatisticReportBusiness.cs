using NTS.Common.Utils;
using SwipeSafe.Model.ProfileReport;
using SwipeSafe.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Business.StatisticReport
{
    public class StatisticReportBusiness
    {
        private ReportAppEntities db = new ReportAppEntities();
        //báo cáo thống kê cho mobile
        public List<ReportByCountWardModel> ReportByCountWard(ReportByCountWardSearch model)
        {
            List<ReportByCountWardModel> lst = new List<ReportByCountWardModel>();
            try
            {
                var lstReport = (from a in db.ProfileChilds.AsNoTracking()
                                 where a.WardId.Equals(model.WardId)
                                 && a.CreateDate.Value.Year <= model.YearTo
                                 && a.CreateDate.Value.Year >= model.YearFrom
                                 select new
                                 {
                                     Year = a.CreateDate.Value.Year,
                                     Count = 1
                                 }).GroupBy(u => u.Year).Select(gr => new ReportByCountWardModel { Count = gr.Count(), Year = gr.FirstOrDefault().Year }).OrderBy(u => u.Year).ToList();

                ReportByCountWardModel reportByCountWardModel;
                for (int year = model.YearFrom; year <= model.YearTo; year++)
                {
                    var item = lstReport.Where(r => r.Year == year).FirstOrDefault();
                    reportByCountWardModel = new ReportByCountWardModel()
                    {
                        Year = year,
                        Count = item != null ? item.Count : 0
                    };
                    lst.Add(reportByCountWardModel);
                }
            }
            catch (Exception ex)
            { }
            return lst;
        }

        public List<ReportByAbuseWardModel> ReportByAbuseWard(ReportByAbuseWardSearch model)
        {
            List<ReportByAbuseWardModel> lst = new List<ReportByAbuseWardModel>();
            try
            {
                var dateFrom = DateTimeUtils.ConvertDateFrom(model.DateFrom);
                var dateTo = DateTimeUtils.ConvertDateTo(model.DateTo);
                lst = (from a in db.ProfileChilds.AsNoTracking()
                       where a.WardId.Equals(model.WardId)
                       && a.CreateDate <= dateTo
                       && a.CreateDate >= dateFrom
                       join b in db.ProfileChildAbuses.AsNoTracking() on a.Id equals b.ProfileChildId
                       select new
                       {
                           AbuseName = b.AbuseName,
                           AbuseId = b.AbuseId,
                           Count = 1
                       }).GroupBy(u => u.AbuseId).Select(gr => new ReportByAbuseWardModel { Count = gr.Count(), AbuseName = gr.FirstOrDefault().AbuseName }).OrderBy(u => u.Count).ToList();
            }
            catch (Exception ex)
            { }
            return lst;
        }

        //cap huyen

        public List<ReportByAreaDistrictModel> ReportByAreaDistrict(ReportByAreaDistrictSearch model)
        {
            List<ReportByAreaDistrictModel> lst = new List<ReportByAreaDistrictModel>();
            try
            {
                var dateFrom = DateTimeUtils.ConvertDateFrom(model.DateFrom);
                var dateTo = DateTimeUtils.ConvertDateTo(model.DateTo);
                lst = (from a in db.ProfileChilds.AsNoTracking()
                       where a.DistrictId.Equals(model.DistrictId)
                       && a.CreateDate <= dateTo
                       && a.CreateDate >= dateFrom
                       && (string.IsNullOrEmpty(model.WardId) || a.WardId.Equals(model.WardId))
                       join b in db.Wards.AsNoTracking() on a.WardId equals b.Id
                       select new ReportByAreaDistrictModel()
                       {
                           Count = 1,
                           WardId = b.Id,
                           WardName = b.Name
                       }).GroupBy(u => u.WardId).Select(gr => new ReportByAreaDistrictModel { Count = gr.Count(), WardName = gr.FirstOrDefault().WardName }).OrderBy(u => u.Count).ToList();
            }
            catch (Exception ex)
            { }
            return lst;
        }

        public List<ReportByAbuseAndTypeDistrictModel> ReportByAbuseAndTypeDistrict(ReportByAbuseAndTypeDistrictSearch model)
        {
            List<ReportByAbuseAndTypeDistrictModel> lst = new List<ReportByAbuseAndTypeDistrictModel>();
            try
            {
                var dateFrom = DateTimeUtils.ConvertDateFrom(model.DateFrom);
                var dateTo = DateTimeUtils.ConvertDateTo(model.DateTo);
                lst = (from a in db.ProfileChilds.AsNoTracking()
                       where a.DistrictId.Equals(model.DistrictId)
                       && a.CreateDate <= dateTo
                       && a.CreateDate >= dateFrom
                       && (string.IsNullOrEmpty(model.WardId) || a.WardId.Equals(model.WardId))
                       join b in db.ProfileChildAbuses.AsNoTracking() on a.Id equals b.ProfileChildId
                       select new ReportByAbuseAndTypeDistrictModel()
                       {
                           Count = 1,
                           AbuseName = b.AbuseName,
                           AbuseId = b.AbuseId,
                           Type = a.SeverityLevel.Value,
                       }).GroupBy(u => u.AbuseId).Select(gr => new ReportByAbuseAndTypeDistrictModel { Count = gr.Count(), AbuseName = gr.FirstOrDefault().AbuseName, ListType = gr.Select(u => u.Type).ToList() }).ToList();
            }
            catch (Exception ex)
            { }
            return lst;
        }

        public List<ReportByAreaAndTypeDistrictModel> ReportByAreaAndTypeDistrict(ReportByAreaAndTypeDistrictSearch model)
        {
            List<ReportByAreaAndTypeDistrictModel> lst = new List<ReportByAreaAndTypeDistrictModel>();
            try
            {
                var dateFrom = DateTimeUtils.ConvertDateFrom(model.DateFrom);
                var dateTo = DateTimeUtils.ConvertDateTo(model.DateTo);
                lst = (from a in db.ProfileChilds.AsNoTracking()
                       where a.DistrictId.Equals(model.DistrictId)
                       && a.CreateDate <= dateTo
                       && a.CreateDate >= dateFrom
                       && (string.IsNullOrEmpty(model.WardId) || a.WardId.Equals(model.WardId))
                       join b in db.Wards.AsNoTracking() on a.WardId equals b.Id
                       select new ReportByAreaAndTypeDistrictModel()
                       {
                           Count = 1,
                           WardName = b.Name,
                           WardId = b.Id,
                           Type = a.SeverityLevel.Value,
                       }).GroupBy(u => u.WardId).Select(gr => new ReportByAreaAndTypeDistrictModel { Count = gr.Count(), WardName = gr.FirstOrDefault().WardName, ListType = gr.Select(u => u.Type).ToList() }).ToList();
            }
            catch (Exception ex)
            { }
            return lst;
        }

        public List<ReportByAbuseAndAgeDistrictModel> ReportByAreaAndAgeDistrict(ReportByAbuseAndAgeDistrictSearch model)

        {
            List<ReportByAbuseAndAgeDistrictModel> lst = new List<ReportByAbuseAndAgeDistrictModel>();
            try
            {
                var dateFrom = DateTimeUtils.ConvertDateFrom(model.DateFrom);
                var dateTo = DateTimeUtils.ConvertDateTo(model.DateTo);
                lst = (from a in db.ProfileChilds.AsNoTracking()
                       where a.DistrictId.Equals(model.DistrictId)
                       && a.CreateDate <= dateTo
                       && a.CreateDate >= dateFrom
                       && (string.IsNullOrEmpty(model.WardId) || a.WardId.Equals(model.WardId))
                       join b in db.ProfileChildAbuses.AsNoTracking() on a.Id equals b.ProfileChildId
                       select new ReportByAbuseAndAgeDistrictModel()
                       {
                           Count = 1,
                           AbuseName = b.AbuseName,
                           AbuseId = b.AbuseId,
                           Age = a.Age,
                       }).GroupBy(u => u.AbuseId).Select(gr => new ReportByAbuseAndAgeDistrictModel { Count = gr.Count(), AbuseName = gr.FirstOrDefault().AbuseName, ListType = gr.Select(u => u.Age).ToList() }).ToList();
            }
            catch (Exception ex)
            { }
            return lst;
        }

        public List<ReportByAbuseAndGenderDistrictModel> ReportByAreaAndGenderDistrict(ReportByAbuseAndGenderDistrictSearch model)

        {
            List<ReportByAbuseAndGenderDistrictModel> lst = new List<ReportByAbuseAndGenderDistrictModel>();
            try
            {
                var dateFrom = DateTimeUtils.ConvertDateFrom(model.DateFrom);
                var dateTo = DateTimeUtils.ConvertDateTo(model.DateTo);
                lst = (from a in db.ProfileChilds.AsNoTracking()
                       where a.DistrictId.Equals(model.DistrictId)
                       && a.CreateDate <= dateTo
                       && a.CreateDate >= dateFrom
                       && (string.IsNullOrEmpty(model.WardId) || a.WardId.Equals(model.WardId))
                       join b in db.ProfileChildAbuses.AsNoTracking() on a.Id equals b.ProfileChildId
                       select new ReportByAbuseAndGenderDistrictModel()
                       {
                           Count = 1,
                           AbuseName = b.AbuseName,
                           AbuseId = b.AbuseId,
                           Gender = a.Gender,
                       }).GroupBy(u => u.AbuseId).Select(gr => new ReportByAbuseAndGenderDistrictModel { Count = gr.Count(), AbuseName = gr.FirstOrDefault().AbuseName, ListType = gr.Select(u => u.Gender).ToList() }).ToList();
            }
            catch (Exception ex)
            { }
            return lst;
        }

        //cap tinh

        public List<ReportByAreaProvinceModel> ReportByAreaProvince(ReportByAreaProvinceSearch model)
        {
            List<ReportByAreaProvinceModel> lst = new List<ReportByAreaProvinceModel>();
            try
            {
                var dateFrom = DateTimeUtils.ConvertDateFrom(model.DateFrom);
                var dateTo = DateTimeUtils.ConvertDateTo(model.DateTo);
                lst = (from a in db.ProfileChilds.AsNoTracking()
                       where a.ProvinceId.Equals(model.ProvinceId)
                       && a.CreateDate <= dateTo
                       && a.CreateDate >= dateFrom
                       && (string.IsNullOrEmpty(model.DistrictId) || a.WardId.Equals(model.DistrictId))
                       join b in db.Districts.AsNoTracking() on a.DistrictId equals b.Id
                       select new ReportByAreaProvinceModel()
                       {
                           Count = 1,
                           DistrictId = b.Id,
                           DistrictName = b.Name
                       }).GroupBy(u => u.DistrictId).Select(gr => new ReportByAreaProvinceModel { Count = gr.Count(), DistrictName = gr.FirstOrDefault().DistrictName }).OrderBy(u => u.Count).ToList();
            }
            catch (Exception ex)
            { }
            return lst;
        }

        public List<ReportByAbuseAndTypeProvinceModel> ReportByAbuseAndTypeProvince(ReportByAbuseAndTypeProvinceSearch model)
        {
            List<ReportByAbuseAndTypeProvinceModel> lst = new List<ReportByAbuseAndTypeProvinceModel>();
            try
            {
                var dateFrom = DateTimeUtils.ConvertDateFrom(model.DateFrom);
                var dateTo = DateTimeUtils.ConvertDateTo(model.DateTo);
                lst = (from a in db.ProfileChilds.AsNoTracking()
                       where a.ProvinceId.Equals(model.ProvinceId)
                       && a.CreateDate <= dateTo
                       && a.CreateDate >= dateFrom
                       && (string.IsNullOrEmpty(model.DistrictId) || a.WardId.Equals(model.DistrictId))
                       join b in db.ProfileChildAbuses.AsNoTracking() on a.Id equals b.ProfileChildId
                       select new ReportByAbuseAndTypeProvinceModel()
                       {
                           Count = 1,
                           AbuseName = b.AbuseName,
                           AbuseId = b.AbuseId,
                           Type = a.SeverityLevel.Value,
                       }).GroupBy(u => u.AbuseId).Select(gr => new ReportByAbuseAndTypeProvinceModel { Count = gr.Count(), AbuseName = gr.FirstOrDefault().AbuseName, ListType = gr.Select(u => u.Type).ToList() }).ToList();
            }
            catch (Exception ex)
            { }
            return lst;
        }

        public List<ReportByAreaAndTypeProvinceModel> ReportByAreaAndTypeProvince(ReportByAreaAndTypeProvinceSearch model)
        {
            List<ReportByAreaAndTypeProvinceModel> lst = new List<ReportByAreaAndTypeProvinceModel>();
            try
            {
                var dateFrom = DateTimeUtils.ConvertDateFrom(model.DateFrom);
                var dateTo = DateTimeUtils.ConvertDateTo(model.DateTo);
                lst = (from a in db.ProfileChilds.AsNoTracking()
                       where a.ProvinceId.Equals(model.ProvinceId)
                       && a.CreateDate <= dateTo
                       && a.CreateDate >= dateFrom
                       && (string.IsNullOrEmpty(model.DistrictId) || a.WardId.Equals(model.DistrictId))
                       join b in db.Wards.AsNoTracking() on a.WardId equals b.Id
                       select new ReportByAreaAndTypeProvinceModel()
                       {
                           Count = 1,
                           WardName = b.Name,
                           WardId = b.Id,
                           Type = a.SeverityLevel.Value,
                       }).GroupBy(u => u.WardId).Select(gr => new ReportByAreaAndTypeProvinceModel { Count = gr.Count(), WardName = gr.FirstOrDefault().WardName, ListType = gr.Select(u => u.Type).ToList() }).ToList();
            }
            catch (Exception ex)
            { }
            return lst;
        }

        public List<ReportByAbuseAndAgeProvinceModel> ReportByAreaAndAgeProvince(ReportByAbuseAndAgeProvinceSearch model)

        {
            List<ReportByAbuseAndAgeProvinceModel> lst = new List<ReportByAbuseAndAgeProvinceModel>();
            try
            {
                var dateFrom = DateTimeUtils.ConvertDateFrom(model.DateFrom);
                var dateTo = DateTimeUtils.ConvertDateTo(model.DateTo);
                lst = (from a in db.ProfileChilds.AsNoTracking()
                       where a.ProvinceId.Equals(model.ProvinceId)
                       && a.CreateDate <= dateTo
                       && a.CreateDate >= dateFrom
                       && (string.IsNullOrEmpty(model.DistrictId) || a.WardId.Equals(model.DistrictId))
                       join b in db.ProfileChildAbuses.AsNoTracking() on a.Id equals b.ProfileChildId
                       select new ReportByAbuseAndAgeProvinceModel()
                       {
                           Count = 1,
                           AbuseName = b.AbuseName,
                           AbuseId = b.AbuseId,
                           Age = a.Age,
                       }).GroupBy(u => u.AbuseId).Select(gr => new ReportByAbuseAndAgeProvinceModel { Count = gr.Count(), AbuseName = gr.FirstOrDefault().AbuseName, ListType = gr.Select(u => u.Age).ToList() }).ToList();
            }
            catch (Exception ex)
            { }
            return lst;
        }

        public List<ReportByAbuseAndGenderProvinceModel> ReportByAreaAndGenderProvince(ReportByAbuseAndGenderProvinceSearch model)
        {
            List<ReportByAbuseAndGenderProvinceModel> lst = new List<ReportByAbuseAndGenderProvinceModel>();
            try
            {
                var dateFrom = DateTimeUtils.ConvertDateFrom(model.DateFrom);
                var dateTo = DateTimeUtils.ConvertDateTo(model.DateTo);
                lst = (from a in db.ProfileChilds.AsNoTracking()
                       where a.ProvinceId.Equals(model.ProvinceId)
                       && a.CreateDate <= dateTo
                       && a.CreateDate >= dateFrom
                       && (string.IsNullOrEmpty(model.DistrictId) || a.WardId.Equals(model.DistrictId))
                       join b in db.ProfileChildAbuses.AsNoTracking() on a.Id equals b.ProfileChildId
                       select new ReportByAbuseAndGenderProvinceModel()
                       {
                           Count = 1,
                           AbuseName = b.AbuseName,
                           AbuseId = b.AbuseId,
                           Gender = a.Gender,
                       }).GroupBy(u => u.AbuseId).Select(gr => new ReportByAbuseAndGenderProvinceModel { Count = gr.Count(), AbuseName = gr.FirstOrDefault().AbuseName, ListType = gr.Select(u => u.Gender).ToList() }).ToList();
            }
            catch (Exception ex)
            { }
            return lst;
        }
    }
}
