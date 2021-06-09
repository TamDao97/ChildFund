using ChildProfiles.Model;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model.Model.ChildStory;
using NTS.Common.Utils;
using NTS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Business.Business
{
    public class SampleStoryDA
    {
        private ChildProfileEntities db = new ChildProfileEntities();

        public SearchResultObject<SampleStoryModel> GetListSampleStory(SampleStorySearchModel modelSearch)
        {
            SearchResultObject<SampleStoryModel> model = new SearchResultObject<SampleStoryModel>();
            try
            {
                var listmodel = (from a in db.StoryTemplates.AsNoTracking()
                                 where !a.IsDelete
                                 select new SampleStoryModel
                                 {
                                     Id = a.Id,
                                     Title = a.Title,
                                     Content = a.StoryContent,
                                     CreateBy = a.CreateBy,
                                     UpdateBy = a.UpdateBy,
                                     UpdateDate = a.UpdateDate,
                                     CreateDate = a.CreateDate,
                                     Type = a.Type,
                                     Status = a.UerStatus
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.Title))
                {
                    listmodel = listmodel.Where(r => r.Title.ToLower().Contains(modelSearch.Title.ToLower()));
                }
                if (!string.IsNullOrEmpty(modelSearch.Type))
                {
                    listmodel = listmodel.Where(r => modelSearch.Type.Equals(r.Type));
                }
                model.TotalItem = listmodel.Select(u => u.Id).Count();
                model.ListResult = SQLHelpper.OrderBy(listmodel, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("SampleStoryDA.GetListSampleStory", ex.Message, modelSearch);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return model;
        }

        public SampleStoryModel GetInfoTemplate(string id)
        {
            SampleStoryModel model = new SampleStoryModel();
            try
            {
                var tem = db.StoryTemplates.FirstOrDefault(e => e.Id == id);
                if (tem != null)
                {
                    model.Id = tem.Id;
                    model.Title = tem.Title;
                    model.Content = tem.StoryContent;
                    model.Type = tem.Type;
                    model.Status = tem.UerStatus;
                    model.UpdateDate = tem.UpdateDate;
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("SampleStoryDA.GetInfoTemplate", ex.Message, id);
                throw ex;
            }
            return model;
        }

        public List<CategoryModel> GetCategoryTemplate()
        {
            return new List<CategoryModel>
            {
                    new CategoryModel {Name="Họ và tên",TextReplace="[fullName]"},
                    new CategoryModel {Name="Tên",TextReplace="[strName]"},
                    new CategoryModel {Name="Tình trạng sức khỏe",TextReplace ="[strHealth]"},
                    new CategoryModel {Name="Hoàn cảnh gia đình",TextReplace = "[strSpecialSituation]"},
                    new CategoryModel {Name="Thành viên gia đình",TextReplace = "[strFamilyMember]"},
                    new CategoryModel {Name="Nghề nghiệp gia đình",TextReplace ="[strParentsJob]"},
                    new CategoryModel {Name="Tình trạng nhà cửa",TextReplace ="[strHouseCondition]"},
                    new CategoryModel {Name="Nguồn nước",TextReplace ="[strWaterSource]"},
                    new CategoryModel {Name="Tính cách", TextReplace="[strCharacteristic]"},
                    new CategoryModel {Name="Ước mơ", TextReplace= "[strDream]"},
                    new CategoryModel {Name="Sở thích",TextReplace="[strHobby]"},
                    new CategoryModel {Name="Môn học",TextReplace ="[strSubject]"},
                    new CategoryModel {Name="Việc nhà",TextReplace="[strHouseWork]"},
                    new CategoryModel {Name="Đại từ nhân xưng(she, he)",TextReplace="[str_child_sex1]"},
                    new CategoryModel {Name="Đại từ sở hữu(her, his)",TextReplace="[str_child_sex2]"},
                    new CategoryModel {Name="Tân ngữ(her, him)",TextReplace="[str_child_sex3]"},
                    new CategoryModel {Name="Giới tính(girl, boy)",TextReplace="[str_child_sex4]"},
                    new CategoryModel {Name="Đại từ nhân xưng(She, He)",TextReplace="[str_child_sex5]"},

            };
        }

        public void UpdateTemplate(SampleStoryModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var tem = db.StoryTemplates.FirstOrDefault(e => e.Id == model.Id);
                    if (tem != null)
                    {
                        tem.Title = model.Title;
                        tem.StoryContent = model.Content;
                        tem.Type = model.Type;
                        tem.UpdateBy = model.UpdateBy;
                        tem.UpdateDate = DateTime.Now;
                        db.SaveChanges();
                        trans.Commit();
                    }
                    else
                    {
                        throw new Exception("Mẫu không tồn tại");
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("SampleStoryDA.UpdateTemplate", ex.Message, model);
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public void AddTemplate(SampleStoryModel model)
        {
            var tem = db.StoryTemplates.FirstOrDefault(e => e.Title.ToUpper().Equals(model.Title.ToUpper()));
            if (tem != null)
            {
                throw new Exception("Trùng tiêu đề mẫu câu chuyện");
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    StoryTemplate template = new StoryTemplate
                    {
                        Id = Guid.NewGuid().ToString(),
                        StoryContent = model.Content,
                        Title = model.Title,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UerStatus = model.Status,
                        Type = model.Type,

                    };
                    db.StoryTemplates.Add(template);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("SampleStoryDA.AddTemplate", ex.Message, model);
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public void UpdateStatus(string id, string status)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var tem = db.StoryTemplates.FirstOrDefault(e => e.Id == id);
                    if (tem != null)
                    {
                        tem.UerStatus = status;
                        db.SaveChanges();
                        trans.Commit();
                    }
                    else
                    {
                        throw new Exception("Mẫu không tồn tại");
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("SampleStoryDA.UpdateStatus", ex.Message, id);
                    trans.Rollback();
                    throw ex;
                }
            }
        }
        public void DeleteTemplate(string id)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var tem = db.StoryTemplates.FirstOrDefault(e => e.Id == id);
                    if (tem != null)
                    {
                        tem.IsDelete = true;
                        db.SaveChanges();
                        trans.Commit();
                    }
                    else
                    {
                        throw new Exception("Mẫu không tồn tại");
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("SampleStoryDA.DeleteTemplate", ex.Message, id);
                    trans.Rollback();
                    throw ex;
                }
            }
        }
    }
}
