
using NTS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Utils;
using NTS.Caching;
using System.Configuration;
using SwipeSafe.Model.Model.CacheModel;

namespace SwipeSafe.Business
{
    public class HomesBusiness
    {
        public List<NotifyModel> GetNotify(string userId)
        {
            RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
            List<NotifyModel> lst = new List<NotifyModel>();
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                lst = redisService.GetContains(cacheNotify + userId + ":*");
            }
            catch (Exception)
            { }
            return lst;
        }

        public void DeleteNotify(string id)
        {
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var key = cacheNotify + System.Web.HttpContext.Current.User.Identity.Name + ":" + id;
                redisService.Remove(key);
            }
            catch (Exception)
            {
                throw new Exception("Xảy ra lỗi vui lòng thử lại");
            }
        }
        public void TickNotify(string id)
        {
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var key = cacheNotify + System.Web.HttpContext.Current.User.Identity.Name + ":" + id;
                NotifyModel notify = redisService.Get<NotifyModel>(key);
                notify.Status = Constants.ViewNotification;
                redisService.Replace(key, notify);
            }
            catch (Exception)
            {
                throw new Exception("Xảy ra lỗi vui lòng thử lại");
            }
        }
    }
}
