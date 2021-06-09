
using InformationHub.Model.Model.Function;
using InformationHub.Model.Model.GroupPermission;
using InformationHub.Model.Model.GroupUser;
using InformationHub.Model.Repositories;
using InformationHub.Model.SearchResults;
using NTS.Common.Utils;
using NTS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Business.Business
{
    public class GroupUserBussiness
    {
        private InformationHubEntities db = new InformationHubEntities();
        public SearchResultObject<GroupUserSearchResult> SearchGroupUser(GroupUserSearchCondition searchCondition)
        {
            SearchResultObject<GroupUserSearchResult> searchResult = new SearchResultObject<GroupUserSearchResult>();
            try
            {
                var listmodel = (from a in db.GroupUsers.AsNoTracking()
                                 join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id into ab
                                 from abv in ab.DefaultIfEmpty()
                                 select new GroupUserSearchResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     IsDisable = a.IsDisable,
                                     Type = a.Type,
                                     Description = a.Description,
                                     Code = a.Code,
                                     CreateBy = abv != null ? abv.FullName : "",
                                     CreateDate = a.CreateDate,
                                     UpdateBy = a.UpdateBy,
                                     UpdateDate = a.UpdateDate
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()));
                }

                if (searchCondition.IsDisable != null)
                {
                    listmodel = listmodel.Where(r => r.IsDisable == searchCondition.IsDisable);
                }

                if (searchCondition.Type.HasValue)
                {
                    listmodel = listmodel.Where(r => r.Type == searchCondition.Type);
                }
                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                foreach (var item in searchResult.ListResult)
                {
                    item.TypeView = GenType(item.Type);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }

        public string GenType(int? stt)
        {
            string rs = "";
            try
            {
                switch (stt)
                {
                    case 0:
                        rs = "Cấp quản lý"; break;
                    case 1:
                        rs = "Cấp tỉnh/thành phố"; break;
                    case 2:
                        rs = "Cấp quận/huyện"; break;
                    case 3:
                        rs = "Cấp xã/phường"; break;
                    default:
                        rs = ""; break; ;
                }
            }
            catch (Exception)
            { }
            return rs;
        }

        public void LockGroup(string id)
        {
            var user = db.GroupUsers.FirstOrDefault(u => u.Id.Equals(id));
            if (user == null || user.IsDisable == true)
            {
                throw new Exception(Resource.Resource.GroupUser_Locked);
            }

            try
            {
                user.IsDisable = true;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
        }

        public void UnLockGroup(string id)
        {
            var user = db.GroupUsers.FirstOrDefault(u => u.Id.Equals(id));
            if (user == null || user.IsDisable == false)
            {
                throw new Exception(Resource.Resource.GroupUser_Locked);
            }

            try
            {
                user.IsDisable = false;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
        }

        public List<FunctionModel> GetAllFunction(string type)
        {
            int intType = 0;
            try
            {
                if (!string.IsNullOrEmpty(type))
                {
                    intType = Int32.Parse(type);
                }
                List<FunctionModel> listFunction = new List<FunctionModel>();
                FunctionModel modelAdd;
                var listFuntion = (from b in db.Permissions.AsNoTracking().Where(r => (intType == 0 && r.TypeLevel1) || (intType == 1 && r.TypeLevel2)
                                  || (intType == 2 && r.TypeLevel3) || (intType == 3 && r.TypeLevel4))
                                   select b).ToList();
                var listGroupId = listFuntion.Select(u => u.GroupFunctionId).ToList();
                var listGroup = db.GroupFunctions.Where(u => listGroupId.Contains(u.Id)).ToList();
                List<Permission> listFuntiontemp;
                int index = 0;
                foreach (var item in listGroup)
                {
                    modelAdd = new FunctionModel();
                    modelAdd.Index = "0";
                    modelAdd.Name = item.Name;
                    modelAdd.Code = string.Empty;
                    modelAdd.GroupFunctionId = string.Empty;
                    listFunction.Add(modelAdd);
                    listFuntiontemp = listFuntion.Where(u => u.GroupFunctionId.Equals(item.Id)).ToList();
                    foreach (var itemSub in listFuntiontemp)
                    {
                        index++;
                        modelAdd = new FunctionModel();
                        modelAdd.Index = index.ToString();
                        modelAdd.Name = itemSub.Name;
                        modelAdd.Code = itemSub.Code;
                        modelAdd.PermissionId = itemSub.Id;
                        modelAdd.GroupFunctionId = item.Id;
                        listFunction.Add(modelAdd);
                    }
                }
                return listFunction;
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
        }

        public void CreateGroupUser(GroupUserModel groupUserModel)
        {
            var checkCode = db.GroupUsers.FirstOrDefault(a => a.Code.Equals(groupUserModel.Code));
            var checkName = db.GroupUsers.FirstOrDefault(b => b.Name.Equals(groupUserModel.Name));
            if (checkCode != null)
            {
                throw new Exception(Resource.Resource.GroupUser_Duplicate_Code);
            }
            if (checkName != null)
            {
                throw new Exception(Resource.Resource.GroupUser_Duplicate_Name);
            }
            if (groupUserModel.ListPermission == null || groupUserModel.ListPermission.Count() == 0)
            {
                throw new Exception(Resource.Resource.GroupUser_UnCheck_Permission);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    //bảng GroupUser
                    var model = new GroupUser();
                    model.Id = Guid.NewGuid().ToString();
                    model.Name = groupUserModel.Name;
                    model.Type = groupUserModel.Type;
                    model.Description = groupUserModel.Description;
                    model.Code = groupUserModel.Code;
                    model.IsDisable = groupUserModel.IsDisable;
                    model.CreateBy = groupUserModel.CreateBy;
                    model.UpdateBy = groupUserModel.CreateBy;
                    model.CreateDate = dateNow;
                    model.UpdateDate = dateNow;
                    db.GroupUsers.Add(model);

                    //bảng GroupPermission
                    GroupPermission itemGroupPermission;
                    if (groupUserModel.ListPermission.Count() > 0)
                    {
                        foreach (var item in groupUserModel.ListPermission)
                        {
                            itemGroupPermission = new GroupPermission();
                            itemGroupPermission.Id = Guid.NewGuid().ToString();
                            itemGroupPermission.GroupUserId = model.Id;
                            itemGroupPermission.PermissionId = item.ToString();
                            db.GroupPermissions.Add(itemGroupPermission);
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

        public GroupUserModel GetGroupUserById(string id)
        {
            var item = db.GroupUsers.Find(id);
            if (item == null)
            {
                throw new Exception(Resource.Resource.GroupUser_Deleted);
            }
            try
            {
                GroupUserModel result = new GroupUserModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Type = Int32.Parse(item.Type.ToString()),
                    Code = item.Code,
                    IsDisable = item.IsDisable,
                    Description = item.Description,
                    CreateBy = item.CreateBy,
                    CreateDate = item.CreateDate,
                    UpdateBy = item.UpdateBy,
                    UpdateDate = item.UpdateDate,
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
        }

        //public List<FunctionModel> GetListPermissionUpdate(string groupUserId)
        //{
        //    List<FunctionModel> result = new List<FunctionModel>();
        //    try
        //    {
        //        var listPermisson = db.GroupPermissions.Where(u => u.GroupUserId.Equals(groupUserId)).Select(u => u.PermissionId).ToList();

        //        result = (from a in db.GroupPermissions.AsNoTracking()
        //                  join b in db.Permissions.AsNoTracking() on a.PermissionId equals b.Id
        //                  join c in db.GroupFunctions.AsNoTracking() on a.GroupUserId equals c.Id
        //                  select new FunctionModel()
        //                  {                         
        //                      PermissionId = a.PermissionId,
        //                      ItemChecked = listPermisson.Contains(a.PermissionId) ? "checked" : ""
        //                  }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ErrorMessage.ERR001, ex.InnerException);
        //    }
        //    return result;
        //}

        public List<FunctionModel> GetListPermissionUpdate(string groupUserId, string type)
        {
            int intType = 0;
            try
            {
                if (!string.IsNullOrEmpty(type))
                {
                    intType = Int32.Parse(type);
                }
                var listPermisson = db.GroupPermissions.Where(u => u.GroupUserId.Equals(groupUserId)).Select(u => u.PermissionId).ToList();
                List<FunctionModel> listFunction = new List<FunctionModel>();
                FunctionModel modelAdd;
                var listFuntion = (from b in db.Permissions.AsNoTracking().Where(r => (intType == 0 && r.TypeLevel1) || (intType == 1 && r.TypeLevel2)
                                  || (intType == 2 && r.TypeLevel3) || (intType == 3 && r.TypeLevel4))
                                   select b).ToList();
                var listGroupId = listFuntion.Select(u => u.GroupFunctionId).ToList();
                var listGroup = db.GroupFunctions.Where(u => listGroupId.Contains(u.Id)).ToList();
                List<Permission> listFuntiontemp;
                int index = 0;
                foreach (var item in listGroup)
                {
                    modelAdd = new FunctionModel();
                    modelAdd.Index = "0";
                    modelAdd.Name = item.Name;
                    modelAdd.Code = string.Empty;
                    modelAdd.GroupFunctionId = string.Empty;
                    listFunction.Add(modelAdd);
                    listFuntiontemp = listFuntion.Where(u => u.GroupFunctionId.Equals(item.Id)).ToList();
                    foreach (var itemSub in listFuntiontemp)
                    {
                        index++;
                        modelAdd = new FunctionModel();
                        modelAdd.Index = index.ToString();
                        modelAdd.Name = itemSub.Name;
                        modelAdd.Code = itemSub.Code;
                        modelAdd.PermissionId = itemSub.Id;
                        modelAdd.GroupFunctionId = item.Id;
                        modelAdd.ItemChecked = listPermisson.Contains(itemSub.Id) ? "checked" : "";
                        listFunction.Add(modelAdd);
                    }
                }
                return listFunction;
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
        }

        public void UpdateGroupUser(GroupUserModel groupUserModel)
        {
            var model = db.GroupUsers.FirstOrDefault(u => u.Id.Equals(groupUserModel.Id));
            if (model == null)
            {
                throw new Exception(Resource.Resource.GroupUser_Deleted);
            }
            var checkCode = db.GroupUsers.FirstOrDefault(a => a.Code.Equals(groupUserModel.Code));
            var checkName = db.GroupUsers.FirstOrDefault(b => b.Name.Equals(groupUserModel.Name));
            if (checkCode != null && !checkCode.Id.Equals(model.Id))
            {
                throw new Exception(Resource.Resource.GroupUser_Duplicate_Code);
            }
            if (checkName != null && !checkName.Id.Equals(model.Id))
            {
                throw new Exception(Resource.Resource.GroupUser_Duplicate_Name);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;

                    //Update GroupUser
                    model.Name = groupUserModel.Name;
                    model.Type = groupUserModel.Type;
                    model.IsDisable = groupUserModel.IsDisable;
                    model.Code = groupUserModel.Code;
                    model.Description = groupUserModel.Description;
                    model.UpdateDate = dateNow;
                    model.UpdateBy = groupUserModel.UpdateBy;

                    var oldPermission = db.GroupPermissions.Where(u => u.GroupUserId.Equals(model.Id)).ToList();
                    if (oldPermission.Count > 0)
                    {
                        db.GroupPermissions.RemoveRange(oldPermission);
                    }

                    GroupPermission itemGroupPermission;
                    if (groupUserModel.ListPermission.Count() > 0 && groupUserModel.ListPermission != null)
                    {
                        foreach (var item in groupUserModel.ListPermission)
                        {
                            itemGroupPermission = new GroupPermission();
                            itemGroupPermission.Id = Guid.NewGuid().ToString();
                            itemGroupPermission.GroupUserId = model.Id;
                            itemGroupPermission.PermissionId = item.ToString();
                            db.GroupPermissions.Add(itemGroupPermission);
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
    }
}
