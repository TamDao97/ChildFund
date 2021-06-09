using ChildProfiles.Model;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model.Model.Statistic;
using Newtonsoft.Json;
using NTS.Common.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Business.Business
{
    public class ChildDataByWardBusiness
    {
        private ChildProfileEntities db = new ChildProfileEntities();

        /// <summary>
        /// Thống kê số lượng trẻ trong xã
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultObject<ChildDataByWardModel> StatisticChildDataByWard(ChildDataByWardSearchModel modelSearch)
        {
            SearchResultObject<ChildDataByWardModel> result = new SearchResultObject<ChildDataByWardModel>();
            var query = db.ChildProfiles.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.ProvinceId))
                query = query.Where(r => r.ProvinceId.Equals(modelSearch.ProvinceId));
            if (!string.IsNullOrEmpty(modelSearch.DistrictId))
                query = query.Where(r => r.DistrictId.Equals(modelSearch.DistrictId));
            if (!string.IsNullOrEmpty(modelSearch.WardId))
                query = query.Where(r => r.WardId.Equals(modelSearch.WardId));

            var listData = (from c in query
                            group c by c.WardId into g
                            join w in db.Wards.AsNoTracking() on g.Key equals w.Id
                            select new ChildDataByWardModel
                            {
                                WardName = w.Name,
                                ChildData = g.ToList(),
                                ChildTotal = g.Select(r => r.ChildCode).Count(),
                                Size = g.Select(r => r.ImageSize ?? 0).Sum()
                            }).ToList();

            //string jsonString = string.Empty;
            int sizeDataMax = 100000;
            //int size = 0;

            foreach (var item in listData)
            {
                //sizeDataMax = 0;

                //foreach (ChildProfile obj in item.ChildData)
                //{
                //    jsonString = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                //    size = Encoding.UTF8.GetByteCount(jsonString);
                //    sizeDataMax = sizeDataMax > size ? sizeDataMax : size;
                //}

                item.Size = (item.Size + ((sizeDataMax * 15 / 10) * item.ChildTotal));
                item.SizeString = ConvertSize(Convert.ToDouble(item.Size));
            }

            result.TotalItem = listData.Select(r => r.WardName).Count();
            result.ListResult = listData.OrderByDescending(r => r.Size).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();

            return result;
        }

        /// <summary>
        /// Quy đổi ra các đại lượng
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private String ConvertSize(double size)
        {
            String[] units = new String[] { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            double mod = 1024.0;
            int i = 0;

            while (size >= mod)
            {
                size /= mod;
                i++;
            }

            return Math.Round(size, 2) + units[i];
        }
    }
}
