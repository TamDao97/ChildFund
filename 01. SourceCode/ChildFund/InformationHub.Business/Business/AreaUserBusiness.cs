﻿
using NTS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Utils;
using InformationHub.Model.AreaUser;
using InformationHub.Model.Repositories;
using InformationHub.Model;
using InformationHub.Model.SearchResults;

namespace InformationHub.Business
{
    public class AreaUserBusiness
    {
        private InformationHubEntities db = new InformationHubEntities();
        public SearchResultObject<AreaUserSearchResult> SearchAreaUser(AreaUserSearchCondition searchCondition)
        {
            SearchResultObject<AreaUserSearchResult> searchResult = new SearchResultObject<AreaUserSearchResult>();
            try
            {
                var listmodel = (from a in db.AreaUsers.AsNoTracking()
                                 select new AreaUserSearchResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     IsActivate = a.IsActivate,
                                     Manager = a.Manager,
                                     ProvinceName = a.ProvinceName,
                                     CountDistrict = db.AreaDistricts.Where(u => u.AreaUserId.Equals(a.Id)).Select(u => u.Id).Count(),
                                     CountWard = (from c in db.AreaDistricts where c.AreaUserId.Equals(a.Id) join d in db.AreaWards on c.Id equals d.AreaDistrictId select d.Id).Count()
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()));
                }

                if (searchCondition.IsActivate != null)
                {
                    listmodel = listmodel.Where(r => r.IsActivate == searchCondition.IsActivate);
                }
                if (!string.IsNullOrEmpty(searchCondition.Manager))
                {
                    listmodel = listmodel.Where(r => r.Manager.ToLower().Contains(searchCondition.Manager.ToLower()));
                }
                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
            return searchResult;
        }

        public void DeleteAreaUser(AreaUserModel model)
        {
            var checkUser = db.Users.FirstOrDefault(u => u.AreaUserId.Equals(model.Id));
            if (checkUser != null)
            {
                throw new Exception(Resource.Resource.AreaDelete_Used);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var data = db.AreaUsers.FirstOrDefault(u => u.Id.Equals(model.Id));
                    var delAreaDistricts = db.AreaDistricts.Where(u => u.AreaUserId.Equals(model.Id)).ToList();
                    if (delAreaDistricts.Count > 0)
                    {
                        var lstId = delAreaDistricts.Select(u => u.Id).ToList();
                        var delAreaWards = db.AreaWards.Where(u => lstId.Contains(u.AreaDistrictId));
                        if (delAreaWards.Count() > 0)
                        {
                            db.AreaWards.RemoveRange(delAreaWards);
                        }
                        db.AreaDistricts.RemoveRange(delAreaDistricts);
                    }
                    db.AreaUsers.Remove(data);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
                }
            }
        }
        public void CreateAreaUser(AreaUserModel areaUserModel)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    var listDistrict = db.Districts.Where(u => areaUserModel.ListDistrict.Contains(u.Id)).ToList();
                    var listWard = db.Wards.Where(u => areaUserModel.ListWard.Contains(u.Id)).ToList();
                    //bảng địa bàn
                    var model = new AreaUser();
                    model.Id = Guid.NewGuid().ToString();
                    model.Name = areaUserModel.Name;
                    model.Manager = areaUserModel.Manager;
                    model.Description = areaUserModel.Description;
                    model.ProvinceId = areaUserModel.ProvinceId;
                    model.ProvinceName = db.Provinces.FirstOrDefault(u => u.Id.Equals(areaUserModel.ProvinceId)).Name;
                    model.IsActivate = areaUserModel.IsActivate;
                    model.CreateBy = areaUserModel.CreateBy;
                    model.UpdateBy = areaUserModel.CreateBy;
                    model.CreateDate = dateNow;
                    model.UpdateDate = dateNow;
                    db.AreaUsers.Add(model);

                    //bảng xa phường
                    AreaDistrict itemAreaDistrict;
                    AreaWard itemAreaWard;
                    foreach (var item in listDistrict)
                    {
                        itemAreaDistrict = new AreaDistrict();
                        itemAreaDistrict.Id = Guid.NewGuid().ToString();
                        itemAreaDistrict.DistrictId = item.Id;
                        itemAreaDistrict.Name = item.Name;
                        itemAreaDistrict.ProvinceId = item.ProvinceId;
                        itemAreaDistrict.AreaUserId = model.Id;
                        db.AreaDistricts.Add(itemAreaDistrict);
                        foreach (var item2 in listWard.Where(u => u.DistrictId.Equals(item.Id)))
                        {
                            itemAreaWard = new AreaWard();
                            itemAreaWard.Id = Guid.NewGuid().ToString();
                            itemAreaWard.WardId = item2.Id;
                            itemAreaWard.Name = item2.Name;
                            itemAreaWard.DistrictId = item2.DistrictId;
                            itemAreaWard.AreaDistrictId = itemAreaDistrict.Id;
                            db.AreaWards.Add(itemAreaWard);
                        }
                    }
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
                }
            }
        }
        public void UpdateAreaUser(AreaUserModel areaUserModel)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    var model = db.AreaUsers.FirstOrDefault(u => u.Id.Equals(areaUserModel.Id));
                    if (model == null)
                    {
                        throw new Exception(Resource.Resource.AreaDelete_Deleted);
                    }
                    var listDistrict = db.Districts.Where(u => areaUserModel.ListDistrict.Contains(u.Id)).ToList();
                    var listWard = db.Wards.Where(u => areaUserModel.ListWard.Contains(u.Id)).ToList();
                    //bảng địa bàn
                    model.Name = areaUserModel.Name;
                    model.Manager = areaUserModel.Manager;
                    model.Description = areaUserModel.Description;
                    model.ProvinceId = areaUserModel.ProvinceId;
                    model.ProvinceName = db.Provinces.FirstOrDefault(u => u.Id.Equals(areaUserModel.ProvinceId)).Name;
                    model.IsActivate = areaUserModel.IsActivate;
                    model.UpdateDate = dateNow;
                    model.UpdateBy = areaUserModel.CreateBy;
                    #region[xóa phường xã cũ]
                    var oldAreaDistrict = db.AreaDistricts.Where(u => u.AreaUserId.Equals(model.Id)).ToList();
                    if (oldAreaDistrict.Count > 0)
                    {
                        var oldAreaWardId = oldAreaDistrict.Select(u => u.Id).ToList();
                        var oldAreaWard = db.AreaWards.Where(u => oldAreaWardId.Contains(u.AreaDistrictId)).ToList();
                        if (oldAreaWard.Count > 0)
                        {
                            db.AreaWards.RemoveRange(oldAreaWard);
                        }
                        db.AreaDistricts.RemoveRange(oldAreaDistrict);
                    }
                    #endregion
                    //bảng xa phường
                    AreaDistrict itemAreaDistrict;
                    AreaWard itemAreaWard;
                    foreach (var item in listDistrict)
                    {
                        itemAreaDistrict = new AreaDistrict();
                        itemAreaDistrict.Id = Guid.NewGuid().ToString();
                        itemAreaDistrict.DistrictId = item.Id;
                        itemAreaDistrict.Name = item.Name;
                        itemAreaDistrict.ProvinceId = item.ProvinceId;
                        itemAreaDistrict.AreaUserId = model.Id;
                        db.AreaDistricts.Add(itemAreaDistrict);
                        foreach (var item2 in listWard.Where(u => u.DistrictId.Equals(item.Id)))
                        {
                            itemAreaWard = new AreaWard();
                            itemAreaWard.Id = Guid.NewGuid().ToString();
                            itemAreaWard.WardId = item2.Id;
                            itemAreaWard.Name = item2.Name;
                            itemAreaWard.DistrictId = item2.DistrictId;
                            itemAreaWard.AreaDistrictId = itemAreaDistrict.Id;
                            db.AreaWards.Add(itemAreaWard);
                        }
                    }
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
                }
            }
        }

        public AreaUserModel GetInfo(string id)
        {
            try
            {
                AreaUserModel result = new AreaUserModel();
                var model = db.AreaUsers.FirstOrDefault(u => u.Id.Equals(id));
                if (model == null)
                {
                    throw new Exception(Resource.Resource.AreaDelete_Deleted);
                }
                result.Id = id;
                result.Name = model.Name;
                result.Manager = model.Manager;
                result.IsActivate = model.IsActivate;
                result.ProvinceId = model.ProvinceId;
                result.Description = model.Description;

                var lstAreaDistricts = db.AreaDistricts.Where(u => u.AreaUserId.Equals(model.Id)).OrderBy(u => u.Name).ToList();
                result.ListDistrict = lstAreaDistricts.Select(u => u.DistrictId).ToList();
                var lstIdlstAreaDistricts = lstAreaDistricts.Select(u => u.Id).ToList();
                result.ListWard = db.AreaWards.Where(u => lstIdlstAreaDistricts.Contains(u.AreaDistrictId)).OrderBy(u => u.Name).Select(u => u.WardId).ToList(); ;
                return result;
            }
            catch (Exception ex)
            { throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException); }
        }
    }
}
