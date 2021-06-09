
using NTS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Caching;
using System.Configuration;
using InformationHub.Model.Repositories;
using InformationHub.Model.CacheModel;
using NTS.Common.Utils;

namespace InformationHub.Business
{
    public class HomesBusiness
    {
        private InformationHubEntities db = new InformationHubEntities();

        public List<NotifyModel> GetNotify(string userId)
        {
            RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();

            List<NotifyModel> lst = new List<NotifyModel>();
            NotifyModel notify = new NotifyModel();

            try
            {
                DateTime expiredTime = DateTime.Now.AddDays(- int.Parse(ConfigurationManager.AppSettings["CacheExpiredTime"]));

                var notifies = (from r in db.Notifies.AsNoTracking()
                                where r.UserId.Equals(userId) && r.CreateDate > expiredTime
                                select r).ToList();

                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";

                foreach (var item in notifies)
                {
                    notify = new NotifyModel();
                    notify = redisService.Get<NotifyModel>(cacheNotify + item.NotifyKey);
                    if (notify != null)
                    {
                        lst.Add(notify);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("HomesBusiness.GetNotify", ex.Message, string.Empty);
            }

            return lst;
        }

        public void DeleteNotify(string id)
        {
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var key = cacheNotify + id;
                redisService.Remove(key);
            }
            catch (Exception)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title);
            }
        }
        public void TickNotify(string id)
        {
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var key = cacheNotify + id;
                NotifyModel notify = redisService.Get<NotifyModel>(key);
                notify.Status = Constants.ViewNotification;
                redisService.Replace(key, notify);
            }
            catch (Exception)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title);
            }
        }
    }
}
