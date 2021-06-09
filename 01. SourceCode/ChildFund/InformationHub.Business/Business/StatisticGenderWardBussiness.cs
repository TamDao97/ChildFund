using InformationHub.Model;
using InformationHub.Model.Repositories;
using InformationHub.Model.StatisticModels;
using NTS.Common.Utils;
using NTS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Business.Business
{
    public class StatisticGenderWardBussiness
    {
        private InformationHubEntities db = new InformationHubEntities();
        public List<StatisticGenderWardModel> SearchStatisticByAge(StatisticSearchCondition modelSearch)
        {
            List<StatisticGenderWardModel> searchResult = new List<StatisticGenderWardModel>();
            try
            {
                var listmodel = (from a in db.ReportProfileAbuseTypes.AsNoTracking()
                                 join b in db.ReportProfiles.AsNoTracking() on a.ReportProfileId equals b.Id 
                                 select new StatisticGenderWardModel()
                                 {
                                     Type = a.AbuseTypeId,
                                     CreateDate = b.CreateDate,
                                     Gender = b.Gender,
                                     WardId = b.WardId,
                                     DistrictId = b.DistrictId,
                                     ProvinceId = b.ProvinceId
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.DateFrom))
                {
                    var dateFrom = DateTimeUtils.ConvertDateFromStr(modelSearch.DateFrom);
                    listmodel = listmodel.Where(r => r.CreateDate >= dateFrom);
                }
                if (!string.IsNullOrEmpty(modelSearch.DateTo))
                {
                    var dateTo = DateTimeUtils.ConvertDateToStr(modelSearch.DateTo);
                    listmodel = listmodel.Where(r => r.CreateDate <= dateTo);
                }
                if (!string.IsNullOrEmpty(modelSearch.WardId))
                {
                    listmodel = listmodel.Where(r => r.WardId.ToLower().Contains(modelSearch.WardId.ToLower()));
                }

                searchResult = listmodel.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }

            return searchResult;
        }
    }
}
