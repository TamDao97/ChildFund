using ChildProfiles.Business.Business;
using ChildProfiles.Model;
using ChildProfiles.Model.ChildProfileModels;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model.Model.CacheModel;
using ChildProfiles.Model.Model.ChildProfileModels;
using ChildProfiles.Model.Model.FliesLibrary;
using Newtonsoft.Json;
using NTS.Caching;
using NTS.Common;
using NTS.Common.Utils;
using NTS.Storage;
using NTS.Utils;
using NTSFramework.Common.Utils;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;


namespace ChildProfiles.Business
{
    public class ChildProfileBusiness
    {
        private ChildProfileEntities db = new ChildProfileEntities();
        private RedisService<ComboboxResult> redisService = RedisService<ComboboxResult>.GetInstance();
        public void AddChildProfile(ChildProfileModel model, HttpFileCollection httpFile)
        {
            if (db.ChildProfiles.AsNoTracking().Where(o => model.ChildCode.ToUpper().Equals(o.ChildCode.ToUpper())).Count() > 0)
            {
                throw new Exception("Trùng mã số trẻ");
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    string WardName = "";
                    string DistrictName = "";
                    string ProvinceName = "";
                    string VillageName = "";
                    try
                    {
                        WardName = db.AreaWards.FirstOrDefault(u => u.WardId.Equals(model.WardId)).Name;
                        DistrictName = db.AreaDistricts.FirstOrDefault(u => u.DistrictId.Equals(model.DistrictId)).Name;
                        ProvinceName = db.AreaUsers.FirstOrDefault(u => u.ProvinceId.Equals(model.ProvinceId)).ProvinceName;
                        VillageName = db.Villages.FirstOrDefault(u => u.Id.Equals(model.Address)).Name;
                    }
                    catch (Exception)
                    {
                    }
                    model.ConvertObjectModelToJson();

                    ChildProfile childProfile = new ChildProfile
                    {
                        Id = Guid.NewGuid().ToString(),
                        InfoDate = model.InfoDate,
                        EmployeeName = model.EmployeeName,
                        EmployeeTitle = model.EmployeeTitle,
                        ProgramCode = model.ProgramCode,
                        ProvinceId = model.ProvinceId,
                        DistrictId = model.DistrictId,
                        WardId = model.WardId,
                        Address = model.Address,
                        FullAddress = VillageName + " - " + WardName + " - " + DistrictName + " - " + ProvinceName,
                        ChildCode = model.ChildCode,
                        SchoolId = model.SchoolId,
                        SchoolOtherName = model.SchoolOtherName,
                        EthnicId = model.EthnicId,
                        ReligionId = model.ReligionId,
                        Name = model.Name,
                        NickName = model.NickName,
                        Gender = model.Gender,
                        DateOfBirth = model.DateOfBirth,
                        LeaningStatus = model.LeaningStatus,
                        ClassInfo = model.ClassInfo,
                        FavouriteSubject = model.FavouriteSubject,
                        LearningCapacity = model.LearningCapacity,
                        Housework = model.Housework,
                        Health = model.Health,
                        Personality = model.Personality,
                        Hobby = model.Hobby,
                        Dream = model.Dream,
                        FamilyMember = model.FamilyMember,
                        LivingWithParent = model.LivingWithParent,
                        NotLivingWithParent = model.NotLivingWithParent,
                        LivingWithOther = model.LivingWithOther,
                        LetterWrite = model.LetterWrite,
                        HouseType = model.HouseType,
                        HouseRoof = model.HouseRoof,
                        HouseWall = model.HouseWall,
                        HouseFloor = model.HouseFloor,
                        UseElectricity = model.UseElectricity,
                        SchoolDistance = model.SchoolDistance,
                        ClinicDistance = model.ClinicDistance,
                        WaterSourceDistance = model.WaterSourceDistance,
                        WaterSourceUse = model.WaterSourceUse,
                        RoadCondition = model.RoadCondition,
                        IncomeFamily = model.IncomeFamily,
                        HarvestOutput = model.HarvestOutput,
                        NumberPet = model.NumberPet,
                        FamilyType = model.FamilyType,
                        TotalIncome = model.TotalIncome,
                        IncomeSources = model.IncomeSources,
                        IncomeOther = model.IncomeOther,
                        ProcessStatus = Constants.CreateNew,
                        IsDelete = Constants.IsUse,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                        Handicap = model.Handicap,

                        ConsentName = model.ConsentName,
                        ConsentRelationship = model.ConsentRelationship,
                        ConsentVillage = model.ConsentVillage,
                        ConsentWard = model.ConsentWard,
                        SiblingsJoiningChildFund = model.SiblingsJoiningChildFund,
                        Malformation = model.Malformation,
                        Orphan = model.Orphan
                    };


                    if (httpFile.Count > 0)
                    {
                        int imageSize = 0;
                        for (int index = 0; index < httpFile.Count; index++)
                        {
                            if (httpFile.Keys[index].Equals("Avatar"))
                            {
                                imageSize += (httpFile[index].ContentLength * 2);

                                NTS.Storage.ImageResult imageResult = Task.Run(async () =>
                                {
                                    return await AzureStorageUploadFiles.GetInstance().UploadImageAsync(httpFile[index], Constants.FolderImageChildProfile);
                                }).Result;

                                if (imageResult != null)
                                {
                                    childProfile.ImagePath = imageResult.ImageOrigin;
                                    childProfile.ImageThumbnailPath = imageResult.ImageThumbnail;
                                    childProfile.ImageSize = imageSize;

                                    ImageChildHistory imageChildHistory = new ImageChildHistory
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ImagePath = imageResult.ImageOrigin,
                                        ImageThumbnailPath = imageResult.ImageThumbnail,
                                        UploadDate = DateTime.Now,
                                        UploadBy = model.CreateBy,
                                        ChildProfileId = childProfile.Id
                                    };
                                    db.ImageChildHistories.Add(imageChildHistory);
                                }
                            }
                            //Lưu ảnh chữ ký
                            else if (httpFile.Keys[index].Equals("ImageSignature"))
                            {
                                NTS.Storage.ImageResult imageResult = Task.Run(async () =>
                                {
                                    return await AzureStorageUploadFiles.GetInstance().UploadImageAsync(httpFile[index], Constants.FolderImageChildProfile);
                                }).Result;

                                if (imageResult != null)
                                {
                                    childProfile.ImageSignaturePath = imageResult.ImageOrigin;
                                    childProfile.ImageSignatureThumbnailPath = imageResult.ImageThumbnail;
                                }
                            }
                        }
                    }

                    db.ChildProfiles.Add(childProfile);
                    db.SaveChanges();
                    trans.Commit();
                    #region[lưu cache notify]
                    if (model.UserLever.Equals(Constants.LevelTeacher))
                    {
                        RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                        var addressModel = (from a in db.Provinces.AsNoTracking()
                                            join b in db.Districts.AsNoTracking() on a.Id equals b.ProvinceId
                                            join c in db.Wards.AsNoTracking() on b.Id equals c.DistrictId
                                            where c.Id.Equals(childProfile.WardId)
                                            select new
                                            {
                                                ProvinceName = a.Name,
                                                DistrictName = b.Name,
                                                WardName = c.Name
                                            }).FirstOrDefault();
                        var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(model.CreateBy);
                        //địa phương duyệt- lấy tk trung ương
                        List<User> userNotify = new List<User>();
                        try
                        {
                            userNotify = (from a in db.Users.AsNoTracking()
                                          join b in db.AreaUsers.AsNoTracking() on a.AreaUserId equals b.Id
                                          where b.ProvinceId.Equals(model.ProvinceId)
                                          join c in db.AreaDistricts.AsNoTracking() on a.AreaDistrictId equals c.Id into ac
                                          from ac1 in ac.DefaultIfEmpty()
                                          where (string.IsNullOrEmpty(a.AreaDistrictId) || (ac1 != null && ac1.DistrictId.Equals(model.DistrictId)))
                                          select a).ToList();
                        }
                        catch (Exception)
                        { }
                        NotifyModel notifyModel;
                        var dateNow = DateTime.Now;
                        string address = "";
                        if (addressModel != null)
                        {
                            address = addressModel.WardName + ", " + addressModel.DistrictName + ", " + addressModel.ProvinceName;
                        }

                        string isSendEmail = ConfigurationManager.AppSettings["IsSendEmail"];
                        if (isSendEmail.ToLower().Equals("true"))
                        {
                            TimeSpan ts = new TimeSpan(24 * 30, 0, 0);
                            string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                            foreach (var item in userNotify)
                            {
                                notifyModel = new NotifyModel();
                                notifyModel.Image = childProfile.ImageThumbnailPath;
                                notifyModel.Id = Guid.NewGuid().ToString();
                                notifyModel.Addres = address;
                                notifyModel.CreateDate = DateTime.Now;
                                notifyModel.Status = Constants.NotViewNotification;
                                notifyModel.Title = "Hồ sơ thêm mới: <b>" + childProfile.ChildCode + "-" + childProfile.Name + "</b> từ cán bộ <b>" + userInfo.Name + "</b>";
                                notifyModel.Link = "/ProfileNew/DetailProfile/" + childProfile.Id + "";
                                redisService.Add(cacheNotify + item.Id + ":" + notifyModel.Id, notifyModel, ts);
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("ChildProfileBusiness.AddChildProfile", ex.Message, model);
                    trans.Rollback();
                    throw ex;
                }
            }
        }
        public string UpdateChildProfile(ChildProfileModel model, HttpFileCollection httpFile)
        {
            var childProfile = db.ChildProfiles.FirstOrDefault(e => e.Id.Equals(model.Id));

            if (childProfile == null)
            {
                throw new Exception("Không tồn tại hồ sơ");
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    string WardName = "";
                    string DistrictName = "";
                    string ProvinceName = "";
                    string VillageName = "";
                    try
                    {
                        if (string.IsNullOrEmpty(model.Address))
                        {
                            model.Address = string.Empty;
                        }
                        WardName = db.AreaWards.FirstOrDefault(u => u.WardId.Equals(model.WardId)).Name;
                        DistrictName = db.AreaDistricts.FirstOrDefault(u => u.DistrictId.Equals(model.DistrictId)).Name;
                        ProvinceName = db.AreaUsers.FirstOrDefault(u => u.ProvinceId.Equals(model.ProvinceId)).ProvinceName;
                        VillageName = db.Villages.FirstOrDefault(u => u.Id.Equals(model.Address))?.Name;
                    }
                    catch (Exception)
                    {
                    }
                    model.ConvertObjectModelToJson();

                    // Nếu hồ sơ là mới tạo thì cho phép update luôn vào hồ sơ
                    if (childProfile.ProcessStatus.Equals(Constants.CreateNew))
                    {
                        childProfile.InfoDate = model.InfoDate;
                        childProfile.EmployeeName = model.EmployeeName;
                        childProfile.EmployeeTitle = model.EmployeeTitle;
                        childProfile.ProgramCode = model.ProgramCode;
                        childProfile.ProvinceId = model.ProvinceId;
                        childProfile.DistrictId = model.DistrictId;
                        childProfile.WardId = model.WardId;
                        childProfile.Address = model.Address;
                        childProfile.FullAddress = VillageName + " - " + WardName + " - " + DistrictName + " - " + ProvinceName;
                        childProfile.ChildCode = model.ChildCode;
                        childProfile.SchoolId = model.SchoolId;
                        childProfile.SchoolOtherName = model.SchoolOtherName;
                        childProfile.EthnicId = model.EthnicId;
                        childProfile.ReligionId = model.ReligionId;
                        childProfile.Name = model.Name;
                        childProfile.NickName = model.NickName;
                        childProfile.Gender = model.Gender;
                        childProfile.DateOfBirth = model.DateOfBirth;
                        childProfile.LeaningStatus = model.LeaningStatus;
                        childProfile.ClassInfo = model.ClassInfo;
                        childProfile.FavouriteSubject = model.FavouriteSubject;
                        childProfile.LearningCapacity = model.LearningCapacity;
                        childProfile.Housework = model.Housework;
                        childProfile.Health = model.Health;
                        childProfile.Personality = model.Personality;
                        childProfile.Hobby = model.Hobby;
                        childProfile.Dream = model.Dream;
                        childProfile.FamilyMember = model.FamilyMember;
                        childProfile.LivingWithParent = model.LivingWithParent;
                        childProfile.NotLivingWithParent = model.NotLivingWithParent;
                        childProfile.LivingWithOther = model.LivingWithOther;
                        childProfile.LetterWrite = model.LetterWrite;
                        childProfile.HouseType = model.HouseType;
                        childProfile.HouseRoof = model.HouseRoof;
                        childProfile.HouseWall = model.HouseWall;
                        childProfile.HouseFloor = model.HouseFloor;
                        childProfile.UseElectricity = model.UseElectricity;
                        childProfile.SchoolDistance = model.SchoolDistance;
                        childProfile.ClinicDistance = model.ClinicDistance;
                        childProfile.WaterSourceDistance = model.WaterSourceDistance;
                        childProfile.WaterSourceUse = model.WaterSourceUse;
                        childProfile.RoadCondition = model.RoadCondition;
                        childProfile.IncomeFamily = model.IncomeFamily;
                        childProfile.HarvestOutput = model.HarvestOutput;
                        childProfile.NumberPet = model.NumberPet;
                        childProfile.FamilyType = model.FamilyType;
                        childProfile.TotalIncome = model.TotalIncome;
                        childProfile.IncomeSources = model.IncomeSources;
                        childProfile.IncomeOther = model.IncomeOther;
                        childProfile.UpdateBy = model.UpdateBy;
                        childProfile.UpdateDate = DateTime.Now;

                        childProfile.ConsentName = model.ConsentName;
                        childProfile.ConsentRelationship = model.ConsentRelationship;
                        childProfile.ConsentVillage = model.ConsentVillage;
                        childProfile.ConsentWard = model.ConsentWard;
                        childProfile.SiblingsJoiningChildFund = model.SiblingsJoiningChildFund;
                        childProfile.Malformation = model.Malformation;
                        childProfile.Orphan = model.Orphan;
                        childProfile.Handicap = model.Handicap;

                        if (httpFile.Count > 0)
                        {
                            int imageSize = 0;

                            for (int index = 0; index < httpFile.Count; index++)
                            {
                                if (httpFile.Keys[index].Equals("Avatar"))
                                {
                                    ImageChildHistory imageChildHistoryUpdate = db.ImageChildHistories.FirstOrDefault(e => e.ChildProfileId == model.Id);
                                    ImageResult imageResult = Task.Run(async () =>
                                    {
                                        return await AzureStorageUploadFiles.GetInstance().UploadImageAsync(httpFile[index], Constants.FolderImageChildProfile);
                                    }).Result;

                                    if (imageResult != null)
                                    {
                                        imageSize += (httpFile[index].ContentLength * 2);
                                        childProfile.ImageSize = imageSize;

                                        if (imageChildHistoryUpdate == null)
                                        {
                                            childProfile.ImagePath = imageResult.ImageOrigin;
                                            childProfile.ImageThumbnailPath = imageResult.ImageThumbnail;

                                            ImageChildHistory imageChildHistory = new ImageChildHistory
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                ImagePath = imageResult.ImageOrigin,
                                                ImageThumbnailPath = imageResult.ImageThumbnail,
                                                UploadDate = DateTime.Now,
                                                UploadBy = model.CreateBy,
                                                ChildProfileId = childProfile.Id
                                            };
                                            db.ImageChildHistories.Add(imageChildHistory);
                                        }
                                        else
                                        {
                                            imageChildHistoryUpdate.ImagePath = imageResult.ImageOrigin;
                                            imageChildHistoryUpdate.ImageThumbnailPath = imageResult.ImageThumbnail;

                                            childProfile.ImagePath = imageResult.ImageOrigin;
                                            childProfile.ImageThumbnailPath = imageResult.ImageThumbnail;
                                        }
                                    }
                                }
                                //Lưu ảnh chữ ký
                                else if (httpFile.Keys[index].Equals("ImageSignature"))
                                {
                                    NTS.Storage.ImageResult imageResult = Task.Run(async () =>
                                    {
                                        return await AzureStorageUploadFiles.GetInstance().UploadImageAsync(httpFile[index], Constants.FolderImageChildProfile);
                                    }).Result;

                                    if (imageResult != null)
                                    {
                                        childProfile.ImageSignaturePath = imageResult.ImageOrigin;
                                        childProfile.ImageSignatureThumbnailPath = imageResult.ImageThumbnail;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //nếu ko có file up mới và có lệnh xóa ảnh
                            if (model.IsRemoveImage == true)
                            {
                                ImageChildHistory imageChildHistoryDelete = db.ImageChildHistories.FirstOrDefault(e => e.ChildProfileId == model.Id);
                                if (imageChildHistoryDelete != null)
                                {
                                    Task.Run(async () =>
                                    {
                                        await AzureStorageUploadFiles.GetInstance().DeleteFileAsync(childProfile.ImagePath);
                                        await AzureStorageUploadFiles.GetInstance().DeleteFileAsync(childProfile.ImageThumbnailPath);
                                    });

                                    imageChildHistoryDelete.ImagePath = "";
                                    imageChildHistoryDelete.ImageThumbnailPath = "";

                                    childProfile.ImagePath = "";
                                    childProfile.ImageThumbnailPath = "";
                                    childProfile.ImageSize = 0;
                                }
                            }

                        }
                    }
                    //Trường hợp đã được cấp VP Hà Nội duyệt
                    else if (childProfile.ProcessStatus.Equals(Constants.ApproveOffice))
                    {
                        string imgNotify = "";
                        var childUpdateId = string.Empty;
                        var childProfileUpdate = db.ChildProfileUpdates.FirstOrDefault(e => e.ChildProfileId == childProfile.Id);
                        if (childProfileUpdate == null)
                        {
                            childProfileUpdate = new ChildProfileUpdate()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ChildProfileId = childProfile.Id,
                                InfoDate = model.InfoDate,
                                EmployeeName = model.EmployeeName,
                                EmployeeTitle = model.EmployeeTitle,
                                ProgramCode = model.ProgramCode,
                                ProvinceId = model.ProvinceId,
                                DistrictId = model.DistrictId,
                                WardId = model.WardId,
                                Address = model.Address,
                                FullAddress = VillageName + " - " + WardName + " - " + DistrictName + " - " + ProvinceName,
                                ChildCode = model.ChildCode,
                                SchoolId = model.SchoolId,
                                SchoolOtherName = model.SchoolOtherName,
                                EthnicId = model.EthnicId,
                                ReligionId = model.ReligionId,
                                Name = model.Name,
                                NickName = model.NickName,
                                Gender = model.Gender,
                                DateOfBirth = model.DateOfBirth,
                                LeaningStatus = model.LeaningStatus,
                                ClassInfo = model.ClassInfo,
                                FavouriteSubject = model.FavouriteSubject,
                                LearningCapacity = model.LearningCapacity,
                                Housework = model.Housework,
                                Health = model.Health,
                                Personality = model.Personality,
                                Hobby = model.Hobby,
                                Dream = model.Dream,
                                FamilyMember = model.FamilyMember,
                                LivingWithParent = model.LivingWithParent,
                                NotLivingWithParent = model.NotLivingWithParent,
                                LivingWithOther = model.LivingWithOther,
                                LetterWrite = model.LetterWrite,
                                HouseType = model.HouseType,
                                HouseRoof = model.HouseRoof,
                                HouseWall = model.HouseWall,
                                HouseFloor = model.HouseFloor,
                                UseElectricity = model.UseElectricity,
                                SchoolDistance = model.SchoolDistance,
                                ClinicDistance = model.ClinicDistance,
                                WaterSourceDistance = model.WaterSourceDistance,
                                WaterSourceUse = model.WaterSourceUse,
                                RoadCondition = model.RoadCondition,
                                IncomeFamily = model.IncomeFamily,
                                HarvestOutput = model.HarvestOutput,
                                NumberPet = model.NumberPet,
                                FamilyType = model.FamilyType,
                                TotalIncome = model.TotalIncome,
                                IncomeSources = model.IncomeSources,
                                IncomeOther = model.IncomeOther,
                                ProcessStatus = Constants.CreateNew,
                                UpdateBy = model.UpdateBy,
                                UpdateDate = DateTime.Now,

                                ConsentName = model.ConsentName,
                                ConsentRelationship = model.ConsentRelationship,
                                ConsentVillage = model.ConsentVillage,
                                ConsentWard = model.ConsentWard,
                                SiblingsJoiningChildFund = model.SiblingsJoiningChildFund,
                                Malformation = model.Malformation,
                                Orphan = model.Orphan,
                                Handicap = model.Handicap,
                                SaleforceId = model.SaleforceID,
                            };
                            if (httpFile.Count > 0)
                            {
                                int imageSize = 0;

                                for (int index = 0; index < httpFile.Count; index++)
                                {
                                    if (httpFile.Keys[index].Equals("Avatar"))
                                    {
                                        ImageResult imageResult = Task.Run(async () =>
                                        {
                                            return await AzureStorageUploadFiles.GetInstance().UploadImageAsync(httpFile[index], Constants.FolderImageChildProfile);
                                        }).Result;

                                        if (imageResult != null)
                                        {
                                            imageSize += (httpFile[index].ContentLength * 2);
                                            childProfile.ImageSize = imageSize;
                                            childProfileUpdate.ImagePath = imageResult.ImageOrigin;
                                            childProfileUpdate.ImageThumbnailPath = imageResult.ImageThumbnail;
                                            imgNotify = imageResult.ImageThumbnail;
                                        }
                                        ImageChildHistory imageChildHistory = new ImageChildHistory
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ImagePath = imageResult.ImageOrigin,
                                            ImageThumbnailPath = imageResult.ImageThumbnail,
                                            UploadDate = DateTime.Now,
                                            UploadBy = model.CreateBy,
                                            ChildProfileId = childProfile.Id
                                        };
                                        db.ImageChildHistories.Add(imageChildHistory);

                                    }
                                    //Lưu ảnh chữ ký
                                    else if (httpFile.Keys[index].Equals("ImageSignature"))
                                    {
                                        NTS.Storage.ImageResult imageResult = Task.Run(async () =>
                                        {
                                            return await AzureStorageUploadFiles.GetInstance().UploadImageAsync(httpFile[index], Constants.FolderImageChildProfile);
                                        }).Result;

                                        if (imageResult != null)
                                        {
                                            childProfileUpdate.ImageSignaturePath = imageResult.ImageOrigin;
                                            childProfileUpdate.ImageSignatureThumbnailPath = imageResult.ImageThumbnail;
                                        }
                                    }
                                }

                            }
                            childUpdateId = childProfileUpdate.Id;
                            db.ChildProfileUpdates.Add(childProfileUpdate);
                        }
                        else
                        {
                            childUpdateId = childProfileUpdate.Id;
                            childProfileUpdate.InfoDate = model.InfoDate;
                            childProfileUpdate.EmployeeName = model.EmployeeName;
                            childProfileUpdate.EmployeeTitle = model.EmployeeTitle;
                            childProfileUpdate.ProgramCode = model.ProgramCode;
                            childProfileUpdate.ProvinceId = model.ProvinceId;
                            childProfileUpdate.DistrictId = model.DistrictId;
                            childProfileUpdate.WardId = model.WardId;
                            childProfileUpdate.Address = model.Address;
                            childProfileUpdate.FullAddress = VillageName + " - " + WardName + " - " + DistrictName + " - " + ProvinceName;
                            childProfileUpdate.ChildCode = model.ChildCode;
                            childProfileUpdate.SchoolId = model.SchoolId;
                            childProfileUpdate.SchoolOtherName = model.SchoolOtherName;
                            childProfileUpdate.EthnicId = model.EthnicId;
                            childProfileUpdate.ReligionId = model.ReligionId;
                            childProfileUpdate.Name = model.Name;
                            childProfileUpdate.NickName = model.NickName;
                            childProfileUpdate.Gender = model.Gender;
                            childProfileUpdate.DateOfBirth = model.DateOfBirth;
                            childProfileUpdate.LeaningStatus = model.LeaningStatus;
                            childProfileUpdate.ClassInfo = model.ClassInfo;
                            childProfileUpdate.FavouriteSubject = model.FavouriteSubject;
                            childProfileUpdate.LearningCapacity = model.LearningCapacity;
                            childProfileUpdate.Housework = model.Housework;
                            childProfileUpdate.Health = model.Health;
                            childProfileUpdate.Personality = model.Personality;
                            childProfileUpdate.Hobby = model.Hobby;
                            childProfileUpdate.Dream = model.Dream;
                            childProfileUpdate.FamilyMember = model.FamilyMember;
                            childProfileUpdate.LivingWithParent = model.LivingWithParent;
                            childProfileUpdate.NotLivingWithParent = model.NotLivingWithParent;
                            childProfileUpdate.LivingWithOther = model.LivingWithOther;
                            childProfileUpdate.LetterWrite = model.LetterWrite;
                            childProfileUpdate.HouseType = model.HouseType;
                            childProfileUpdate.HouseRoof = model.HouseRoof;
                            childProfileUpdate.HouseWall = model.HouseWall;
                            childProfileUpdate.HouseFloor = model.HouseFloor;
                            childProfileUpdate.UseElectricity = model.UseElectricity;
                            childProfileUpdate.SchoolDistance = model.SchoolDistance;
                            childProfileUpdate.ClinicDistance = model.ClinicDistance;
                            childProfileUpdate.WaterSourceDistance = model.WaterSourceDistance;
                            childProfileUpdate.WaterSourceUse = model.WaterSourceUse;
                            childProfileUpdate.RoadCondition = model.RoadCondition;
                            childProfileUpdate.IncomeFamily = model.IncomeFamily;
                            childProfileUpdate.HarvestOutput = model.HarvestOutput;
                            childProfileUpdate.NumberPet = model.NumberPet;
                            childProfileUpdate.FamilyType = model.FamilyType;
                            childProfileUpdate.TotalIncome = model.TotalIncome;
                            childProfileUpdate.IncomeSources = model.IncomeSources;
                            childProfileUpdate.IncomeOther = model.IncomeOther;
                            childProfileUpdate.UpdateBy = model.UpdateBy;
                            childProfileUpdate.UpdateDate = DateTime.Now;

                            childProfileUpdate.ConsentName = model.ConsentName;
                            childProfileUpdate.ConsentRelationship = model.ConsentRelationship;
                            childProfileUpdate.ConsentVillage = model.ConsentVillage;
                            childProfileUpdate.ConsentWard = model.ConsentWard;
                            childProfileUpdate.SiblingsJoiningChildFund = model.SiblingsJoiningChildFund;
                            childProfileUpdate.Malformation = model.Malformation;
                            childProfileUpdate.Orphan = model.Orphan;
                            childProfileUpdate.Handicap = model.Handicap;

                            if (httpFile.Count > 0)
                            {
                                int imageSize = 0;

                                for (int index = 0; index < httpFile.Count; index++)
                                {
                                    if (httpFile.Keys[index].Equals("Avatar"))
                                    {
                                        ImageResult imageResult = Task.Run(async () =>
                                {
                                    return await AzureStorageUploadFiles.GetInstance().UploadImageAsync(httpFile[index], Constants.FolderImageChildProfile);
                                }).Result;
                                        if (imageResult != null)
                                        {
                                            imageSize += (httpFile[index].ContentLength * 2);
                                            childProfileUpdate.ImageSize = imageSize;
                                            childProfileUpdate.ImagePath = imageResult.ImageOrigin;
                                            childProfileUpdate.ImageThumbnailPath = imageResult.ImageThumbnail;

                                            imgNotify = imageResult.ImageThumbnail;
                                        }
                                        ImageChildHistory imageChildHistory = new ImageChildHistory
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            ImagePath = imageResult.ImageOrigin,
                                            ImageThumbnailPath = imageResult.ImageThumbnail,
                                            UploadDate = DateTime.Now,
                                            UploadBy = model.CreateBy,
                                            ChildProfileId = childProfile.Id
                                        };
                                        db.ImageChildHistories.Add(imageChildHistory);
                                    }
                                    //Lưu ảnh chữ ký
                                    else if (httpFile.Keys[index].Equals("ImageSignature"))
                                    {
                                        NTS.Storage.ImageResult imageResult = Task.Run(async () =>
                                        {
                                            return await AzureStorageUploadFiles.GetInstance().UploadImageAsync(httpFile[index], Constants.FolderImageChildProfile);
                                        }).Result;

                                        if (imageResult != null)
                                        {
                                            childProfileUpdate.ImageSignaturePath = imageResult.ImageOrigin;
                                            childProfileUpdate.ImageSignatureThumbnailPath = imageResult.ImageThumbnail;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (model.IsRemoveImage)
                                {
                                    Task.Run(async () =>
                                    {
                                        await AzureStorageUploadFiles.GetInstance().DeleteFileAsync(childProfileUpdate.ImagePath);
                                        await AzureStorageUploadFiles.GetInstance().DeleteFileAsync(childProfileUpdate.ImageThumbnailPath);
                                    });
                                    childProfileUpdate.ImagePath = "";
                                    childProfileUpdate.ImageThumbnailPath = "";
                                    childProfileUpdate.ImageSize = 0;
                                }
                            }
                        }

                        #region[lưu cache notify]
                        if (model.UserLever.Equals(Constants.LevelTeacher))
                        {
                            RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                            var addressModel = (from a in db.Provinces.AsNoTracking()
                                                join b in db.Districts.AsNoTracking() on a.Id equals b.ProvinceId
                                                join c in db.Wards.AsNoTracking() on b.Id equals c.DistrictId
                                                where c.Id.Equals(model.WardId)
                                                select new
                                                {
                                                    ProvinceName = a.Name,
                                                    DistrictName = b.Name,
                                                    WardName = c.Name
                                                }).FirstOrDefault();
                            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(model.UpdateBy);
                            //địa phương duyệt- lấy tk trung ương
                            List<User> userNotify = new List<User>();
                            try
                            {
                                userNotify = (from a in db.Users.AsNoTracking()
                                              join b in db.AreaUsers.AsNoTracking() on a.AreaUserId equals b.Id
                                              where b.ProvinceId.Equals(model.ProvinceId)
                                              join c in db.AreaDistricts.AsNoTracking() on a.AreaDistrictId equals c.Id into ac
                                              from ac1 in ac.DefaultIfEmpty()
                                              where (string.IsNullOrEmpty(a.AreaDistrictId) || (ac1 != null && ac1.DistrictId.Equals(model.DistrictId)))
                                              select a).ToList();
                            }
                            catch (Exception)
                            { }

                            NotifyModel notifyModel;
                            var dateNow = DateTime.Now;
                            string address = "";
                            if (addressModel != null)
                            {
                                address = addressModel.WardName + ", " + addressModel.DistrictName + ", " + addressModel.ProvinceName;
                            }

                            string isSendEmail = ConfigurationManager.AppSettings["IsSendEmail"];
                            if (isSendEmail.ToLower().Equals("true"))
                            {
                                TimeSpan ts = new TimeSpan(24 * 30, 0, 0);
                                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                                foreach (var item in userNotify)
                                {
                                    notifyModel = new NotifyModel();
                                    notifyModel.Image = imgNotify;
                                    notifyModel.Id = Guid.NewGuid().ToString();
                                    notifyModel.Addres = address;
                                    notifyModel.CreateDate = DateTime.Now;
                                    notifyModel.Status = Constants.NotViewNotification;
                                    notifyModel.Title = "Hồ sơ cập nhật: <b>" + model.ChildCode + "-" + model.Name + "</b> từ cán bộ <b>" + userInfo.Name + "</b>";
                                    notifyModel.Link = "/ProfilesUpdate/CompareProfile/" + childUpdateId + "";
                                    redisService.Add(cacheNotify + item.Id + ":" + notifyModel.Id, notifyModel, ts);
                                }
                            }
                        }
                        #endregion
                    }

                    db.SaveChanges();
                    trans.Commit();

                    if (model.IsExportWord)
                    {
                        return ExportWordForm2(model.Id);
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("ChildProfileBusiness.UpdateChildProfile", ex.Message, model);
                    trans.Rollback();
                    throw ex;
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// Lấy thông tin hồ sơ trẻ theo id
        /// </summary>
        /// <param name="id">Id hồ sơ trẻ</param>
        /// <returns></returns>
        public ChildProfileModel GetInfoChildProfileApproved(string id)
        {
            ChildProfileModel childProfileModel = new ChildProfileModel();

            childProfileModel = (db.ChildProfiles.AsNoTracking()
                .Where(r => r.Id.Equals(id))
                .Select(s => new ChildProfileModel
                {
                    Id = s.Id,
                    InfoDate = s.InfoDate,
                    EmployeeName = s.EmployeeName,
                    EmployeeTitle = s.EmployeeTitle,
                    ProgramCode = s.ProgramCode,
                    ProvinceId = s.ProvinceId,
                    DistrictId = s.DistrictId,
                    WardId = s.WardId,
                    Address = s.Address,
                    ChildCode = s.ChildCode,
                    SchoolId = s.SchoolId,
                    SchoolOtherName = s.SchoolOtherName,
                    EthnicId = s.EthnicId,
                    ReligionId = s.ReligionId,
                    Name = s.Name,
                    NickName = s.NickName,
                    Gender = s.Gender,
                    DateOfBirth = s.DateOfBirth,
                    LeaningStatus = s.LeaningStatus,
                    ClassInfo = s.ClassInfo,
                    FavouriteSubject = s.FavouriteSubject,
                    LearningCapacity = s.LearningCapacity,
                    Housework = s.Housework,
                    Health = s.Health,
                    Personality = s.Personality,
                    Hobby = s.Hobby,
                    Dream = s.Dream,
                    FamilyMember = s.FamilyMember,
                    LivingWithParent = s.LivingWithParent,
                    NotLivingWithParent = s.NotLivingWithParent,
                    LivingWithOther = s.LivingWithOther,
                    LetterWrite = s.LetterWrite,
                    HouseType = s.HouseType,
                    HouseRoof = s.HouseRoof,
                    HouseWall = s.HouseWall,
                    HouseFloor = s.HouseFloor,
                    UseElectricity = s.UseElectricity,
                    SchoolDistance = s.SchoolDistance,
                    ClinicDistance = s.ClinicDistance,
                    WaterSourceDistance = s.WaterSourceDistance,
                    WaterSourceUse = s.WaterSourceUse,
                    RoadCondition = s.RoadCondition,
                    IncomeFamily = s.IncomeFamily,
                    HarvestOutput = s.HarvestOutput,
                    NumberPet = s.NumberPet,
                    FamilyType = s.FamilyType,
                    TotalIncome = s.TotalIncome,
                    IncomeSources = s.IncomeSources,
                    IncomeOther = s.IncomeOther,
                    ImagePath = s.ImagePath,
                    ImageThumbnailPath = s.ImageThumbnailPath,
                    ConsentName = s.ConsentName,
                    ConsentRelationship = s.ConsentRelationship,
                    ConsentVillage = s.ConsentVillage,
                    ConsentWard = s.ConsentWard,
                    SiblingsJoiningChildFund = s.SiblingsJoiningChildFund,
                    Malformation = s.Malformation,
                    Orphan = s.Orphan,
                    ImageSignaturePath = s.ImageSignaturePath,
                    ImageSignatureThumbnailPath = s.ImageSignatureThumbnailPath,
                    Handicap = s.Handicap,
                    SaleforceID = s.SaleforceId,
                })).FirstOrDefault();

            //Convert string json to model
            if (childProfileModel == null)
            {
                childProfileModel = new ChildProfileModel();
            }

            childProfileModel.ConvertObjectJsonToModel();
            return childProfileModel;
        }

        public ChildProfileModel GetInfoChildProfileUpdate(string id)
        {
            ChildProfileModel childProfileModel = null;

            if (!string.IsNullOrEmpty(id))
            {
                var updateProfiles = db.ChildProfileUpdates.AsNoTracking().Where(r => r.ChildProfileId.Equals(id)).ToList();

                if (updateProfiles.Count > 0)
                {
                    childProfileModel = new ChildProfileModel
                    {
                        Id = updateProfiles.FirstOrDefault().ChildProfileId,
                        InfoDate = updateProfiles.FirstOrDefault().InfoDate,
                        EmployeeName = updateProfiles.FirstOrDefault().EmployeeName,
                        EmployeeTitle = updateProfiles.FirstOrDefault().EmployeeTitle,
                        ProgramCode = updateProfiles.FirstOrDefault().ProgramCode,
                        ProvinceId = updateProfiles.FirstOrDefault().ProvinceId,
                        DistrictId = updateProfiles.FirstOrDefault().DistrictId,
                        WardId = updateProfiles.FirstOrDefault().WardId,
                        Address = updateProfiles.FirstOrDefault().Address,
                        ChildCode = updateProfiles.FirstOrDefault().ChildCode,
                        SchoolId = updateProfiles.FirstOrDefault().SchoolId,
                        SchoolOtherName = updateProfiles.FirstOrDefault().SchoolOtherName,
                        EthnicId = updateProfiles.FirstOrDefault().EthnicId,
                        ReligionId = updateProfiles.FirstOrDefault().ReligionId,
                        Name = updateProfiles.FirstOrDefault().Name,
                        NickName = updateProfiles.FirstOrDefault().NickName,
                        Gender = updateProfiles.FirstOrDefault().Gender,
                        DateOfBirth = updateProfiles.FirstOrDefault().DateOfBirth,
                        LeaningStatus = updateProfiles.FirstOrDefault().LeaningStatus,
                        ClassInfo = updateProfiles.FirstOrDefault().ClassInfo,
                        FavouriteSubject = updateProfiles.FirstOrDefault().FavouriteSubject,
                        LearningCapacity = updateProfiles.FirstOrDefault().LearningCapacity,
                        Housework = updateProfiles.FirstOrDefault().Housework,
                        Health = updateProfiles.FirstOrDefault().Health,
                        Personality = updateProfiles.FirstOrDefault().Personality,
                        Hobby = updateProfiles.FirstOrDefault().Hobby,
                        Dream = updateProfiles.FirstOrDefault().Dream,
                        FamilyMember = updateProfiles.FirstOrDefault().FamilyMember,
                        LivingWithParent = updateProfiles.FirstOrDefault().LivingWithParent,
                        NotLivingWithParent = updateProfiles.FirstOrDefault().NotLivingWithParent,
                        LivingWithOther = updateProfiles.FirstOrDefault().LivingWithOther,
                        LetterWrite = updateProfiles.FirstOrDefault().LetterWrite,
                        HouseType = updateProfiles.FirstOrDefault().HouseType,
                        HouseRoof = updateProfiles.FirstOrDefault().HouseRoof,
                        HouseWall = updateProfiles.FirstOrDefault().HouseWall,
                        HouseFloor = updateProfiles.FirstOrDefault().HouseFloor,
                        UseElectricity = updateProfiles.FirstOrDefault().UseElectricity,
                        SchoolDistance = updateProfiles.FirstOrDefault().SchoolDistance,
                        ClinicDistance = updateProfiles.FirstOrDefault().ClinicDistance,
                        WaterSourceDistance = updateProfiles.FirstOrDefault().WaterSourceDistance,
                        WaterSourceUse = updateProfiles.FirstOrDefault().WaterSourceUse,
                        RoadCondition = updateProfiles.FirstOrDefault().RoadCondition,
                        IncomeFamily = updateProfiles.FirstOrDefault().IncomeFamily,
                        HarvestOutput = updateProfiles.FirstOrDefault().HarvestOutput,
                        NumberPet = updateProfiles.FirstOrDefault().NumberPet,
                        FamilyType = updateProfiles.FirstOrDefault().FamilyType,
                        TotalIncome = updateProfiles.FirstOrDefault().TotalIncome,
                        IncomeSources = updateProfiles.FirstOrDefault().IncomeSources,
                        IncomeOther = updateProfiles.FirstOrDefault().IncomeOther,
                        ImagePath = updateProfiles.FirstOrDefault().ImagePath,
                        ImageThumbnailPath = updateProfiles.FirstOrDefault().ImageThumbnailPath,
                        ConsentName = updateProfiles.FirstOrDefault().ConsentName,
                        ConsentRelationship = updateProfiles.FirstOrDefault().ConsentRelationship,
                        ConsentVillage = updateProfiles.FirstOrDefault().ConsentVillage,
                        ConsentWard = updateProfiles.FirstOrDefault().ConsentWard,
                        SiblingsJoiningChildFund = updateProfiles.FirstOrDefault().SiblingsJoiningChildFund,
                        Malformation = updateProfiles.FirstOrDefault().Malformation,
                        Orphan = updateProfiles.FirstOrDefault().Orphan,
                        ImageSignaturePath = updateProfiles.FirstOrDefault().ImageSignaturePath,
                        ImageSignatureThumbnailPath = updateProfiles.FirstOrDefault().ImageSignatureThumbnailPath,
                        Handicap = updateProfiles.FirstOrDefault().Handicap,
                        SaleforceID = updateProfiles.FirstOrDefault().SaleforceId,
                    };
                }

                if (childProfileModel == null)
                {
                    throw new Exception("Hồ sơ trẻ đã bị xóa bởi người dùng khác!");
                }
            }
            else
            {
                childProfileModel = new ChildProfileModel();
            }

            //Convert string json to model
            childProfileModel.ConvertObjectJsonToModel();

            return childProfileModel;
        }

        public ChildProfileModel GetInfoChildProfile(string id)
        {
            ChildProfileModel childProfileModel = null;

            if (!string.IsNullOrEmpty(id))
            {
                var updateProfiles = db.ChildProfileUpdates.AsNoTracking().Where(r => r.ChildProfileId.Equals(id)).ToList();

                if (updateProfiles.Count > 0)
                {
                    childProfileModel = GetInfoChildProfileUpdate(id);
                }
                else
                {
                    childProfileModel = GetInfoChildProfileApproved(id);
                }

                if (childProfileModel == null)
                {
                    throw new Exception("Hồ sơ trẻ đã bị xóa bởi người dùng khác!");
                }
            }
            else
            {
                childProfileModel = new ChildProfileModel();
                childProfileModel.ConvertObjectJsonToModel();
            }

            //Convert string json to model
            //childProfileModel.ConvertObjectJsonToModel();

            return childProfileModel;
        }
        public SearchResultObject<ChildProfileSearchResult> SearchChildProfileProvince(ChildProfileSearchCondition searchCondition)
        {
            SearchResultObject<ChildProfileSearchResult> searchResult = new SearchResultObject<ChildProfileSearchResult>();
            try
            {
                var listmodel = (from a in db.ChildProfiles.AsNoTracking()
                                 where a.ProcessStatus.Equals(searchCondition.Status)
                                 && a.IsDelete == Constants.IsUse
                                 join c in db.Religions.AsNoTracking() on a.ReligionId equals c.Id into ac
                                 from ac1 in ac.DefaultIfEmpty()
                                     //join d in db.Provinces.AsNoTracking() on a.ProvinceId equals d.Id
                                     //join e in db.Districts.AsNoTracking() on a.DistrictId equals e.Id
                                     //join f in db.Wards.AsNoTracking() on a.WardId equals f.Id
                                 join g in db.Ethnics.AsNoTracking() on a.EthnicId equals g.Id
                                 join s in db.Schools.AsNoTracking() on a.SchoolId equals s.Id into asc
                                 from asc1 in asc.DefaultIfEmpty()
                                 join h in db.Users.AsNoTracking() on a.CreateBy equals h.Id
                                 join i in db.Users.AsNoTracking() on a.OfficeApproveBy equals i.Id into ai
                                 from ai1 in ai.DefaultIfEmpty()
                                 select new ChildProfileSearchResult()
                                 {
                                     Avata = a.ImageThumbnailPath,
                                     Id = a.Id,
                                     Name = a.Name,
                                     ReligionName = ac1 != null ? ac1.Name : "",
                                     ProgramCode = a.ProgramCode,
                                     School = asc1 != null ? asc1.SchoolName : a.SchoolOtherName,
                                     NationName = g.Name,
                                     Status = a.ProcessStatus,
                                     Handicap = a.Handicap.HasValue ? (bool)a.Handicap : false,
                                     ChildCode = a.ChildCode,
                                     ProvinceId = a.ProvinceId,
                                     DistrictId = a.DistrictId,
                                     WardId = a.WardId,
                                     Address = a.FullAddress,
                                     DateOfBirth = a.DateOfBirth,
                                     Gender = a.Gender == Constants.Male ? "Nam" : "Nữ",
                                     ApproveDate = a.AreaApproverDate,
                                     OfficeApproveDate = a.OfficeApproveDate,
                                     ApproverName = ai1 != null ? ai1.Name : "",
                                     CreateDate = a.CreateDate,
                                     CreateBy = h.Name,
                                     SalesforceID = a.SaleforceId,
                                     HealthHandicap = (a.Health.Contains("\"Id\":\"04\",\"Check\":true") || a.Health.Contains("\"Check\":true,\"Id\":\"04\"")) ? true : false,

                                 }).AsQueryable();

                if (!string.IsNullOrEmpty(searchCondition.ProgramCode))
                {
                    listmodel = listmodel.Where(r => r.ProgramCode.ToLower().Contains(searchCondition.ProgramCode));
                }
                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()) || r.ChildCode.ToLower().Contains(searchCondition.Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.CreateBy))
                {
                    listmodel = listmodel.Where(r => r.CreateBy.ToLower().Contains(searchCondition.CreateBy.ToLower()));
                }

                if (!string.IsNullOrEmpty(searchCondition.ProvinceId))
                {
                    listmodel = listmodel.Where(r => r.ProvinceId.Equals(searchCondition.ProvinceId));
                }
                if (!string.IsNullOrEmpty(searchCondition.DistrictId))
                {
                    listmodel = listmodel.Where(r => r.DistrictId.Equals(searchCondition.DistrictId));
                }
                if (!string.IsNullOrEmpty(searchCondition.WardId))
                {
                    listmodel = listmodel.Where(r => r.WardId.Equals(searchCondition.WardId));
                }
                if (!string.IsNullOrEmpty(searchCondition.DateFrom))
                {
                    try
                    {
                        var dateFrom = DateTimeUtils.ConvertDateFromStr(searchCondition.DateFrom);
                        listmodel = listmodel.Where(r => r.CreateDate >= dateFrom);
                    }
                    catch (Exception)
                    { }

                }
                if (!string.IsNullOrEmpty(searchCondition.DateTo))
                {
                    try
                    {
                        var dateTo = DateTimeUtils.ConvertDateToStr(searchCondition.DateTo);
                        listmodel = listmodel.Where(r => r.CreateDate <= dateTo);
                    }
                    catch (Exception)
                    { }

                }
                if (!string.IsNullOrEmpty(searchCondition.DateFromByADO))
                {
                    try
                    {
                        var dateFromByADO = DateTimeUtils.ConvertDateFromStr(searchCondition.DateFromByADO);
                        listmodel = listmodel.Where(r => r.ApproveDate >= dateFromByADO);
                    }
                    catch (Exception)
                    { }

                }
                if (!string.IsNullOrEmpty(searchCondition.DateToByADO))
                {
                    try
                    {
                        var dateToByADO = DateTimeUtils.ConvertDateToStr(searchCondition.DateToByADO);
                        listmodel = listmodel.Where(r => r.ApproveDate <= dateToByADO);
                    }
                    catch (Exception)
                    { }

                }
                if (!string.IsNullOrEmpty(searchCondition.DateFromByHNO))
                {
                    try
                    {
                        var dateFromByHNO = DateTimeUtils.ConvertDateFromStr(searchCondition.DateFromByHNO);
                        listmodel = listmodel.Where(r => r.OfficeApproveDate >= dateFromByHNO);
                    }
                    catch (Exception)
                    { }

                }
                if (!string.IsNullOrEmpty(searchCondition.DateToByHNO))
                {
                    try
                    {
                        var dateToByHNO = DateTimeUtils.ConvertDateToStr(searchCondition.DateToByHNO);
                        listmodel = listmodel.Where(r => r.OfficeApproveDate <= dateToByHNO);
                    }
                    catch (Exception)
                    { }

                }
                searchResult.ListId = listmodel.Select(u => u.Id).ToList();
                searchResult.TotalItem = searchResult.ListId.Count();

                // Trường hợp tìm danh sách đã duyệt thì thực hiện Order theo ngày duyệt
                if (searchCondition.Status == "2")
                {
                    searchResult.ListResult = SQLHelpper.OrderBy(listmodel, "OfficeApproveDate", searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                }
                else
                {
                    searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                }

                searchResult.PathFile = "";
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileBusiness.SearchChildProfileProvince", ex.Message, searchCondition);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }
        public SearchResultObject<ChildProfileSearchResult> SearchChildProfileWard(ChildProfileSearchCondition searchCondition)
        {
            SearchResultObject<ChildProfileSearchResult> searchResult = new SearchResultObject<ChildProfileSearchResult>();
            try
            {
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(searchCondition.UserId);
                var listmodel = (from a in db.ChildProfiles.AsNoTracking()
                                 where // a.Status.Equals(Constants.ProfilesNew) &&
                                 a.IsDelete == Constants.IsUse
                                 join c in db.Religions.AsNoTracking() on a.ReligionId equals c.Id into ac
                                 from ac1 in ac.DefaultIfEmpty()
                                 join g in db.Ethnics.AsNoTracking() on a.EthnicId equals g.Id
                                 join s in db.Schools.AsNoTracking() on a.SchoolId equals s.Id into asc
                                 from asc1 in asc.DefaultIfEmpty()
                                 join h in db.Users.AsNoTracking() on a.CreateBy equals h.Id
                                 join i in db.Users.AsNoTracking() on a.AreaApproverId equals i.Id into ai
                                 from ai1 in ai.DefaultIfEmpty()
                                 select new ChildProfileSearchResult()
                                 {
                                     Avata = a.ImageThumbnailPath,
                                     Id = a.Id,
                                     Name = a.Name,
                                     ReligionName = ac1 != null ? ac1.Name : "",
                                     ProgramCode = a.ProgramCode,
                                     School = asc1 != null ? asc1.SchoolName : a.SchoolOtherName,
                                     NationName = g.Name,
                                     Status = a.ProcessStatus,
                                     ChildCode = a.ChildCode,
                                     ProvinceId = a.ProvinceId,
                                     DistrictId = a.DistrictId,
                                     WardId = a.WardId,
                                     Address = a.FullAddress,
                                     DateOfBirth = a.DateOfBirth,
                                     Gender = a.Gender == Constants.Male ? "Nam" : "Nữ",
                                     CreateDate = a.CreateDate,
                                     UserId = a.CreateBy,
                                     CreateBy = h.Name,
                                     ApproveDate = a.AreaApproverDate,
                                     ApproverName = ai1 != null ? ai1.Name : "",
                                     SalesforceID = a.SaleforceId,
                                     Handicap = a.Handicap.HasValue ? (bool)a.Handicap : false,
                                     HealthHandicap = (a.Health.Contains("\"Id\":\"04\",\"Check\":true") || a.Health.Contains("\"Check\":true,\"Id\":\"04\"")) ? true : false,

                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(searchCondition.ProgramCode))
                {
                    listmodel = listmodel.Where(r => r.ProgramCode.ToLower().Contains(searchCondition.ProgramCode));
                }
                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()) || r.ChildCode.ToLower().Contains(searchCondition.Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.CreateBy))
                {
                    listmodel = listmodel.Where(r => r.CreateBy.ToLower().Contains(searchCondition.CreateBy.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.ProvinceId))
                {
                    listmodel = listmodel.Where(r => r.ProvinceId.Equals(searchCondition.ProvinceId));
                }
                if (!string.IsNullOrEmpty(searchCondition.DistrictId))
                {
                    listmodel = listmodel.Where(r => r.DistrictId.Equals(searchCondition.DistrictId));
                }
                else
                {
                    List<string> lstDistrictId = new List<string>();
                    if (!string.IsNullOrEmpty(searchCondition.UserId))
                    {
                        lstDistrictId = (from a in db.AreaUsers.AsNoTracking()
                                         where a.Id.Equals(userInfo.AreaUserId)
                                         join c in db.AreaDistricts.AsNoTracking() on a.Id equals c.AreaUserId
                                         where (string.IsNullOrEmpty(userInfo.DistrictId) || c.DistrictId.Equals(userInfo.DistrictId))
                                         select c.DistrictId).ToList();
                    }
                    listmodel = listmodel.Where(r => lstDistrictId.Contains(r.DistrictId));
                }
                if (!string.IsNullOrEmpty(searchCondition.WardId))
                {
                    listmodel = listmodel.Where(r => r.WardId.Equals(searchCondition.WardId));
                }
                else
                {
                    List<string> lstWardId = new List<string>();
                    if (!string.IsNullOrEmpty(searchCondition.UserId))
                    {
                        lstWardId = (from a in db.AreaUsers.AsNoTracking()
                                     where a.Id.Equals(userInfo.AreaUserId)
                                     join c in db.AreaDistricts.AsNoTracking() on a.Id equals c.AreaUserId
                                     join d in db.AreaWards.AsNoTracking() on c.Id equals d.AreaDistrictId
                                     select d.WardId).ToList();
                    }
                    listmodel = listmodel.Where(r => lstWardId.Contains(r.WardId));
                }
                if (!string.IsNullOrEmpty(searchCondition.DateFrom))
                {
                    try
                    {
                        var dateFrom = DateTimeUtils.ConvertDateFromStr(searchCondition.DateFrom);
                        listmodel = listmodel.Where(r => r.CreateDate >= dateFrom);
                    }
                    catch (Exception)
                    { }

                }
                if (!string.IsNullOrEmpty(searchCondition.DateTo))
                {
                    try
                    {
                        var dateTo = DateTimeUtils.ConvertDateToStr(searchCondition.DateTo);
                        listmodel = listmodel.Where(r => r.CreateDate <= dateTo);
                    }
                    catch (Exception)
                    { }
                }
                //nếu là tk giáo viên chỉ xem dc nó tạo
                if (userInfo.UserLever.Equals(Constants.LevelTeacher))
                {
                    listmodel = listmodel.Where(r => r.UserId.Equals(searchCondition.UserId));
                }
                searchResult.ListId = listmodel.Select(u => u.Id).ToList();
                searchResult.TotalItem = searchResult.ListId.Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                searchResult.PathFile = "";
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileBusiness.SearchChildProfileWard", ex.Message, searchCondition);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }
        public SearchResultObject<ChildProfileMobileSearchResult> SearchChildProfileMobiles(ChildProfileSearchCondition searchCondition)
        {
            SearchResultObject<ChildProfileMobileSearchResult> searchResult = new SearchResultObject<ChildProfileMobileSearchResult>();
            try
            {
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(searchCondition.UserId);

                var listmodel = (from a in db.ChildProfiles.AsNoTracking()
                                 where a.IsDelete == Constants.IsUse
                                 && (a.ProcessStatus.Equals(Constants.CreateNew) || a.ProcessStatus.Equals(Constants.ApproveOffice))
                                 && a.ProvinceId.Equals(userInfo.ProvinceId) && a.DistrictId.Equals(userInfo.DistrictId)
                                 && a.WardId.Equals(userInfo.WardId)
                                 join d in db.Schools.AsNoTracking() on a.SchoolId equals d.Id into ad
                                 from adv in ad.DefaultIfEmpty()
                                 join u in db.ChildProfileUpdates.AsNoTracking() on a.Id equals u.ChildProfileId into au
                                 from auv in au.DefaultIfEmpty()
                                 join s in db.Schools.AsNoTracking() on auv.SchoolId equals s.Id into aus
                                 from ausv in aus.DefaultIfEmpty()
                                 join f in db.Villages.AsNoTracking() on a.Address equals f.Id into af
                                 from afv in af.DefaultIfEmpty()
                                 select new ChildProfileMobileSearchResult()
                                 {
                                     Id = a.Id,
                                     InfoDate = a.InfoDate,
                                     EmployeeName = a.EmployeeName,
                                     ProgramCode = a.ProgramCode,
                                     ProvinceId = a.ProvinceId,
                                     DistrictId = a.DistrictId,
                                     WardId = a.WardId,
                                     Address = a.Address,
                                     FullAddress = a.FullAddress,
                                     ChildCode = auv != null ? auv.ChildCode : a.ChildCode,
                                     SchoolId = a.SchoolId,
                                     SchoolOtherName = a.SchoolOtherName,
                                     EthnicId = a.EthnicId,
                                     ReligionId = a.ReligionId,
                                     Name = auv != null ? auv.Name : a.Name,
                                     NickName = a.NickName,
                                     Gender = auv != null ? auv.Gender : a.Gender,
                                     DateOfBirth = auv != null ? auv.DateOfBirth : a.DateOfBirth,
                                     LeaningStatus = a.LeaningStatus,
                                     ClassInfo = a.ClassInfo,
                                     FavouriteSubject = a.FavouriteSubject,
                                     LearningCapacity = a.LearningCapacity,
                                     Housework = a.Housework,
                                     Health = a.Health,
                                     Personality = a.Personality,
                                     Hobby = a.Hobby,
                                     Dream = a.Dream,
                                     FamilyMember = a.FamilyMember,
                                     LivingWithParent = a.LivingWithParent,
                                     NotLivingWithParent = a.NotLivingWithParent,
                                     LivingWithOther = a.LivingWithOther,
                                     LetterWrite = a.LetterWrite,
                                     HouseType = a.HouseType,
                                     HouseRoof = a.HouseRoof,
                                     HouseWall = a.HouseWall,
                                     HouseFloor = a.HouseFloor,
                                     UseElectricity = a.UseElectricity,
                                     SchoolDistance = a.SchoolDistance,
                                     ClinicDistance = a.ClinicDistance,
                                     WaterSourceDistance = a.WaterSourceDistance,
                                     WaterSourceUse = a.WaterSourceUse,
                                     RoadCondition = a.RoadCondition,
                                     IncomeFamily = a.IncomeFamily,
                                     HarvestOutput = a.HarvestOutput,
                                     NumberPet = a.NumberPet,
                                     FamilyType = a.FamilyType,
                                     TotalIncome = a.TotalIncome,
                                     IncomeSources = a.IncomeSources,
                                     IncomeOther = a.IncomeOther,
                                     StoryContent = a.StoryContent,
                                     ImagePath = a.ImagePath,
                                     ImageThumbnailPath = a.ImageThumbnailPath,
                                     AreaApproverId = a.AreaApproverId,
                                     AreaApproverDate = a.AreaApproverDate,
                                     OfficeApproveBy = a.OfficeApproveBy,
                                     OfficeApproveDate = a.OfficeApproveDate,
                                     ProcessStatus = a.ProcessStatus,
                                     IsDelete = a.IsDelete,
                                     CreateBy = a.CreateBy,
                                     CreateDate = auv != null ? auv.UpdateDate : a.UpdateDate,
                                     UpdateBy = a.UpdateBy,
                                     UpdateDate = a.UpdateDate,
                                     ConsentName = a.ConsentName,
                                     ConsentRelationship = a.ConsentRelationship,
                                     ConsentVillage = a.ConsentVillage,
                                     ConsentWard = a.ConsentWard,
                                     SiblingsJoiningChildFund = a.SiblingsJoiningChildFund,
                                     Malformation = a.Malformation,
                                     Orphan = a.Orphan,
                                     EmployeeTitle = a.EmployeeTitle,
                                     ImageSignaturePath = a.ImageSignaturePath,
                                     ImageSignatureThumbnailPath = a.ImageSignatureThumbnailPath,
                                     SaleforceId = a.SaleforceId,
                                     Handicap = a.Handicap,
                                     ImageSize = a.ImageSize,
                                     Avata = a.ImageThumbnailPath,
                                     Status = auv != null ? "0" : a.ProcessStatus,
                                     School = ausv != null ? (ausv.SchoolName + (afv != null ? " | Xóm/Village: " + afv.Name : "")) : ((adv != null ? adv.SchoolName : a.SchoolOtherName) + (afv != null ? " | Xóm/Village: " + afv.Name : "")),

                                     //Id = a.Id,
                                     //Name = a.Name,
                                     //DateOfBirth = a.DateOfBirth,
                                     //Gender = a.Gender,
                                     //ProgramCode = a.ProgramCode,
                                     //ChildCode = a.ChildCode,
                                     //ProvinceId = a.ProvinceId,
                                     //DistrictId = a.DistrictId,
                                     //WardId = a.WardId,
                                     //CreateDate = a.UpdateDate,
                                 }).AsQueryable();

                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()));
                }

                if (!string.IsNullOrEmpty(searchCondition.ChildCode))
                {
                    listmodel = listmodel.Where(r => r.ChildCode.ToLower().Contains(searchCondition.ChildCode.ToLower()));
                }

                if (!string.IsNullOrEmpty(searchCondition.Address))
                {
                    listmodel = listmodel.Where(r => r.School.ToLower().Contains(searchCondition.Address.ToLower()));
                }

                searchResult.ListId = listmodel.Select(u => u.Id).ToList();
                searchResult.TotalItem = searchResult.ListId.Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();

                //Tính kích thước của một hồ sơ
                string jsonString = string.Empty;
                byte[] byteArray;

                foreach (var item in searchResult.ListResult)
                {
                    jsonString = JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    byteArray = Encoding.UTF8.GetBytes(jsonString);
                    item.TotalSize = (((item.ImageSize ?? 0) + byteArray.Length) / 1024f) / 1024f;
                    item.TotalSize = Math.Round(item.TotalSize, 2);

                    var updateChildProfile = db.ChildProfileUpdates.AsNoTracking().Where(a => a.ChildProfileId.Equals(item.Id)).FirstOrDefault();
                    if (updateChildProfile != null)
                    {
                        item.isHasUpdate = true;
                    }
                    else
                    {
                        item.isHasUpdate = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileBusiness.SearchChildProfileMobiles   ", ex.Message, searchCondition);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }

        public void DeleteChildProfile(ChildProfileModel model)
        {
            var checkChild = db.ChildProfiles.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (checkChild == null)
            {
                throw new Exception("Hồ sơ đã bị xóa bởi người dùng khác");
            }
            try
            {
                checkChild.IsDelete = Constants.IsDelete;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileBusiness.DeleteChildProfile", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }
        public void ConfimProfile(ChildProfileModel model)
        {
            if (!model.Id.Equals("-1"))
            {//nếu là duyệt 1 thì đưa id vào list
                model.SelectId = new List<string>();
                model.SelectId.Add(model.Id);
            }
            var chilSelect = db.ChildProfiles.Where(u => model.SelectId.Contains(u.Id)).ToList();

            var userNotify = db.Users.Where(u => u.UserLever.Equals(Constants.LevelOffice)).ToList();
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(model.CreateBy);

            try
            {
                string address = string.Empty;
                foreach (var checkChild in chilSelect)
                {
                    // Trường hợp hồ sơ mới thêm
                    if (checkChild.ProcessStatus.Equals(Constants.CreateNew))
                    {
                        checkChild.ProcessStatus = Constants.ApproverArea;
                        checkChild.AreaApproverDate = DateTime.Now;
                        checkChild.AreaApproverId = model.CreateBy;
                        #region[lưu cache notify]
                        RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();

                        NotifyModel notifyModel;
                        var dateNow = DateTime.Now;
                        address = checkChild.FullAddress;// addressModel.WardName + ", " + addressModel.DistrictName + ", " + addressModel.ProvinceName;
                        string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                        string isSendEmail = ConfigurationManager.AppSettings["IsSendEmail"];
                        if (isSendEmail.ToLower().Equals("true"))
                        {
                            TimeSpan ts = new TimeSpan(24 * 30, 0, 0);
                            foreach (var item in userNotify)
                            {
                                notifyModel = new NotifyModel();
                                notifyModel.Image = checkChild.ImageThumbnailPath;
                                notifyModel.Id = Guid.NewGuid().ToString();
                                notifyModel.Addres = address;
                                notifyModel.CreateDate = dateNow;
                                notifyModel.Status = Constants.NotViewNotification;
                                notifyModel.Title = "Hồ sơ thêm mới: <b>" + checkChild.ChildCode + "-" + checkChild.Name + "</b> từ cán bộ <b>" + userInfo.Name + "</b>";
                                notifyModel.Link = "/ProfileNew/DetailProfile/" + checkChild.Id + "";
                                redisService.Add(cacheNotify + item.Id + ":" + notifyModel.Id, notifyModel, ts);
                            }

                        }
                        #endregion
                    }
                    // Trường hợp đã được văn phòng vùng duyệt
                    else if (checkChild.ProcessStatus.Equals(Constants.ApproverArea) && (userInfo.UserLever.Equals(Constants.LevelOffice) || userInfo.UserLever.Equals(Constants.LevelAdmin)))
                    {
                        checkChild.ProcessStatus = Constants.ApproveOffice;
                        checkChild.OfficeApproveDate = DateTime.Now;
                        checkChild.OfficeApproveBy = model.CreateBy;
                        //xử lý sinh câu chuyện
                        checkChild.StoryContent = GenStory(checkChild.Id, true);
                    }
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileBusiness.ConfimProfile", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }
        public string ResetStory(ChildProfileModel model)
        {
            var checkChild = db.ChildProfiles.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (checkChild == null)
            {
                throw new Exception("Hồ sơ đã bị xóa bởi người dùng khác");
            }
            try
            {
                //xử lý sinh câu chuyện
                var content = GenStory(model.Id, true);
                checkChild.StoryContent = content;
                db.SaveChanges();
                return content;
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileBusiness.ResetStory", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }
        public string GenStory(string id, bool isOfficial)
        {
            string content = "";
            ChildProfileModel childProfileModel = new ChildProfileModel();

            if (isOfficial)
            {
                childProfileModel = GetInfoChildProfileApproved(id);
            }
            else
            {
                childProfileModel = GetInfoChildProfileUpdate(id);
            }

            string typeStory = "3";//còn lại

            if (childProfileModel.LeaningStatus.Equals(Constants.LeaningChildhood))
            {
                typeStory = "1";//còn nhỏ
            }
            else if (childProfileModel.LeaningStatus.Equals(Constants.LeaningDropout))
            {
                typeStory = "2";//bỏ học
            }
            else if (childProfileModel.LeaningStatus.Equals(Constants.LeaningHandicapped))
            {
                typeStory = "3";//khuyết tật
            }
            else if (childProfileModel.LeaningStatus.Equals(Constants.LeaningKindergarten))
            {
                typeStory = "4";//mẫu giáo
            }
            else if (childProfileModel.LeaningStatus.Equals(Constants.LeaningPrimarySchool) || childProfileModel.LeaningStatus.Equals(Constants.LeaningHighSchool))
            {
                typeStory = "5";//tiểu học - trung học
            }
            var lstStory = db.StoryTemplates.Where(u => u.IsDelete == Constants.IsUse && u.Type.Equals(typeStory)).ToList();
            if (lstStory.Count > 0)
            {
                Random rd = new Random();
                var index = rd.Next(0, lstStory.Count);

                content = new StoryBusiness(childProfileModel).GenerateStory(lstStory[index].StoryContent);
                childProfileModel.StoryContent = content;
            }

            return content;
        }
        public string ExportProfileSelect(ChildProfileExport model)
        {
            if (model.ListCheck == null)
            {
                model.ListCheck = new List<string>();
            }
            string result = "";
            try
            {
                var listmodel = (from a in db.ChildProfiles.AsNoTracking()
                                 where model.ListCheck.Contains(a.Id)
                                 && a.IsDelete == Constants.IsUse
                                 join c in db.Religions.AsNoTracking() on a.ReligionId equals c.Id
                                 //join d in db.Provinces.AsNoTracking() on a.ProvinceId equals d.Id
                                 //join e in db.Districts.AsNoTracking() on a.DistrictId equals e.Id
                                 //join f in db.Wards.AsNoTracking() on a.WardId equals f.Id
                                 join g in db.Ethnics.AsNoTracking() on a.EthnicId equals g.Id
                                 join h in db.Users.AsNoTracking() on a.CreateBy equals h.Id
                                 join i in db.Users.AsNoTracking() on a.AreaApproverId equals i.Id into ai
                                 from ai1 in ai.DefaultIfEmpty()
                                 join j in db.Users.AsNoTracking() on a.OfficeApproveBy equals j.Id into aj
                                 from aj1 in aj.DefaultIfEmpty()
                                 join x in db.Schools.AsNoTracking() on a.SchoolId equals x.Id into xx
                                 from xxs in xx.DefaultIfEmpty()
                                 orderby a.CreateDate descending
                                 select new ChildProfileSearchResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     ReligionName = c.Name,
                                     ProgramCode = a.ProgramCode,
                                     NationName = g.Name,
                                     Status = a.ProcessStatus,
                                     ChildCode = a.ChildCode,
                                     Address = a.FullAddress,
                                     DateOfBirth = a.DateOfBirth,
                                     Gender = a.Gender == Constants.Male ? "Nam" : "Nữ",
                                     ApproveDate = a.AreaApproverDate,
                                     ApproverName = ai1 != null ? ai1.Name : "",
                                     OfficeApproveDate = a.OfficeApproveDate,
                                     OfficeApproveBy = aj1 != null ? aj1.Name : "",
                                     CreateDate = a.CreateDate,
                                     CreateBy = h.Name,
                                     StoryContent = a.StoryContent,
                                     SchoolName = xxs.SchoolName,
                                     SalesforceID = a.SaleforceId
                                 }).ToList();
                result = ExportProfile(listmodel, model.IsProvince);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileBusiness.ExportProfileSelect", ex.Message, model);
            }
            return result;
        }
        public string ExportProfile(List<ChildProfileSearchResult> list, bool IsProvince)
        {
            string pathTemplate = "/Template/ProfileNewProvince.xlsx";
            string pathExport = "/Template/Export/Danh-Sach-Ho-So.xlsx";

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(HttpContext.Current.Server.MapPath(pathTemplate));
            IWorksheet sheet = workbook.Worksheets[0];
            IRange rangeValue = sheet.FindFirst("<Title>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            rangeValue.Text = rangeValue.Text.Replace("<Title>", "");
            int total = list.Count;
            if (total == 0)
            {
                IRange rangeValueDaTa = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValueDaTa.Text = rangeValueDaTa.Text.Replace("<Data>", (string.Empty));
            }
            if (total > 0)
            {
                int index = 1;
                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (total > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, total - 1, ExcelInsertOptions.FormatAsBefore);
                }
                var listExport = (from a in list
                                  select new
                                  {
                                      Index = index++,
                                      a.Name,
                                      a.Gender,
                                      d1 = a.DateOfBirth.ToString("dd/MM/yyyy"),
                                      a.NationName,
                                      a.Address,
                                      a.ProgramCode,
                                      a.ChildCode,
                                      a.StoryContent,
                                      d2 = a.SchoolName,
                                      d3 = GenStatus(a.Status, IsProvince),
                                      d4 = IsProvince == true ? a.OfficeApproveBy : a.ApproverName,
                                      d5 = IsProvince == true ? (a.OfficeApproveDate != null ? a.OfficeApproveDate.Value.ToString("dd/MM/yyyy") : "") : (a.ApproveDate != null ? a.ApproveDate.Value.ToString("dd/MM/yyyy") : ""),
                                      a.SalesforceID,
                                  }).ToList();
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders.Color = ExcelKnownColors.Black;
                sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 14].CellStyle.WrapText = true;

            }
            workbook.SaveAs(HttpContext.Current.Server.MapPath(pathExport));
            return pathExport;
        }
        public string GenStatus(string Status, bool IsProvince)
        {
            string rs = string.Empty;
            if (IsProvince)
            {
                rs = Status.Equals(Constants.ApproveOffice) ? "Đã duyệt" : "Chưa duyệt";
            }
            else
            {
                rs = Status.Equals(Constants.ApproverArea) ? "Đã duyệt" : "Chưa duyệt";
            }
            return rs;
        }
        public string ExportStorySelect(ChildProfileExport model)
        {
            if (model.ListCheck == null)
            {
                model.ListCheck = new List<string>();
            }
            string result = "";
            try
            {
                var listmodel = (from a in db.ChildProfiles.AsNoTracking()
                                 where model.ListCheck.Contains(a.Id)
                                 && a.IsDelete == Constants.IsUse
                                 join c in db.Provinces.AsNoTracking() on a.ProvinceId equals c.Id
                                 join d in db.Districts.AsNoTracking() on a.DistrictId equals d.Id
                                 orderby a.CreateDate descending
                                 select new ChildProfileExportResult()
                                 {
                                     Content = a.StoryContent,
                                     ProgramCode = a.ProgramCode,
                                     ChildCode = a.ChildCode,
                                     SaleforceID = a.SaleforceId,
                                     Name = a.Name,
                                     ProvinceName = c.Name,
                                     //  DistrictName = d.Name,
                                     CreateDate = a.UpdateDate,
                                     Image = a.ImagePath,
                                     ImageSignaturePath = a.ImageSignaturePath
                                 }).ToList();
                if (listmodel.Count == 0)
                {
                    throw new Exception("Không có câu chuyện của trẻ!");
                }
                result = ExportStory(listmodel);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileBusiness.ExportStorySelect", ex.Message, model);
                throw new Exception("Không có câu chuyện của trẻ!");
            }
            return result;
        }
        public string ExportStory(List<ChildProfileExportResult> listAll)
        {
            var list = listAll.Where(u => u.ProgramCode.StartsWith(Constants.ChildCode199)).ToList();
            var list213 = listAll.Where(u => u.ProgramCode.StartsWith(Constants.ChildCode213)).ToList();
            List<AttachmentImageModel> lstFile = new List<AttachmentImageModel>();
            AttachmentImageModel itemFile;
            string pathExport = "";
            string result = "";
            try
            {
                var dateView = DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + "-Story";
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Template/Export/" + dateView));
                if (list.Count > 0)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Template/Export/" + dateView + "/199/Narrative"));
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Template/Export/" + dateView + "/199/Photo"));
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Template/Export/" + dateView + "/199/Consent"));
                }
                if (list213.Count > 0)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Template/Export/" + dateView + "/213/Photo"));
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Template/Export/" + dateView + "/213/Consent"));
                }
                foreach (var item in list)
                {
                    pathExport = HttpContext.Current.Server.MapPath("/Template/Export/" + dateView + "/199/Narrative/" + Common.ConvertNameToTag(item.ChildCode.Trim().Replace("\n","")) + ".txt");
                    using (StreamWriter sw = new StreamWriter(pathExport))
                    {
                        sw.WriteLine(item.Content);
                    }
                    itemFile = new AttachmentImageModel();
                    itemFile.ImagePath = pathExport;
                    itemFile.Name = item.ChildCode + item.Name;
                    lstFile.Add(itemFile);
                }
                //tai anh
                if (list.Count > 0)
                {
                    var urls = list.Select(e => new AttachmentImageModel
                    {
                        ImagePath = e.Image,
                        Name = e.Name,
                        Code = e.ChildCode
                    }).ToList();
                    new ImageLibraryDA().DownLoadImgProfileToServer(urls, "/Template/Export/" + dateView + "/199/Photo/", false, false);
                    var urlImageSignature = list.Select(a => new AttachmentImageModel
                    {
                        ImagePath = a.ImageSignaturePath,
                        Name = a.ChildCode,
                        Code = a.ChildCode
                    }).ToList();
                    if (urlImageSignature.Count > 0)
                    {
                        new ImageLibraryDA().DownLoadImgProfileToServer(urlImageSignature, "/Template/Export/" + dateView + "/199/Consent/", false, true);
                    }
                }
                if (list213.Count > 0)
                {
                    itemFile = new AttachmentImageModel();
                    itemFile.ImagePath = ExportCSV(list213, dateView);
                    itemFile.Name = "CSV213";
                    lstFile.Add(itemFile);
                    var urls213 = list213.Select(e => new AttachmentImageModel
                    {
                        ImagePath = e.Image,
                        Code = e.ChildCode,
                        Name = e.Name
                    }).ToList();
                    new ImageLibraryDA().DownLoadImgProfileToServer(urls213, "/Template/Export/" + dateView + "/213/Photo/", true, false);
                    var urlImageSignature = list213.Select(a => new AttachmentImageModel
                    {
                        ImagePath = a.ImageSignaturePath,
                        Name = a.ChildCode,
                        Code = a.ChildCode
                    }).ToList();
                    if (urlImageSignature.Count > 0)
                    {
                        new ImageLibraryDA().DownLoadImgProfileToServer(urlImageSignature, "/Template/Export/" + dateView + "/213/Consent/", false, true);
                    }
                }
                string folder = "~/Template/Export/" + dateView;
                string fileReturn = "~/Template/Export/" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + "/";
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(fileReturn));
                result = new ImageLibraryDA().ZipFileForder(folder, fileReturn, "Ho-So");
            }
            catch (Exception ex)
            { LogUtils.ExceptionLog("ChildProfileBusiness.ExportStory", ex.Message, ""); }
            return result;
        }
        public ChildProfileModel DetailProfile(string id)
        {
            var listVillage = db.Villages.AsNoTracking().ToList();

            ChildProfileModel childProfiles = new ChildProfileModel();
            try
            {
                childProfiles = (from a in db.ChildProfiles.AsNoTracking()
                                 where a.Id.Equals(id)
                                 join b in db.Ethnics.AsNoTracking() on a.EthnicId equals b.Id
                                 join c in db.Religions.AsNoTracking() on a.ReligionId equals c.Id into ac
                                 from ac1 in ac.DefaultIfEmpty()
                                 join d in db.Provinces.AsNoTracking() on a.ProvinceId equals d.Id
                                 join e in db.Districts.AsNoTracking() on a.DistrictId equals e.Id
                                 join f in db.Wards.AsNoTracking() on a.WardId equals f.Id
                                 join g in db.Schools.AsNoTracking() on a.SchoolId equals g.Id into ag
                                 from ag1 in ag.DefaultIfEmpty()
                                 select new ChildProfileModel
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     ReligionId = ac1 != null ? ac1.Name : "",
                                     EthnicId = b.Name,
                                     ProcessStatus = a.ProcessStatus,
                                     ProgramCode = a.ProgramCode,
                                     SchoolId = ag1 != null ? ag1.SchoolName : a.SchoolOtherName,
                                     ChildCode = a.ChildCode,
                                     ProvinceId = d.Name,
                                     DistrictId = e.Name,
                                     WardId = f.Name,
                                     Address = a.Address,
                                     EmployeeName = a.EmployeeName,
                                     EmployeeTitle = a.EmployeeTitle,
                                     InfoDate = a.InfoDate,
                                     DateOfBirth = a.DateOfBirth,
                                     Gender = a.Gender,
                                     NickName = a.NickName,
                                     LeaningStatus = a.LeaningStatus,
                                     ClassInfo = a.ClassInfo,
                                     FavouriteSubject = a.FavouriteSubject,
                                     LearningCapacity = a.LearningCapacity,
                                     Housework = a.Housework,
                                     Health = a.Health,
                                     Personality = a.Personality,
                                     Hobby = a.Hobby,
                                     Dream = a.Dream,
                                     FamilyMember = a.FamilyMember,
                                     LivingWithParent = a.LivingWithParent,
                                     NotLivingWithParent = a.NotLivingWithParent,
                                     LivingWithOther = a.LivingWithOther,
                                     LetterWrite = a.LetterWrite,
                                     HouseType = a.HouseType,
                                     HouseRoof = a.HouseRoof,
                                     HouseWall = a.HouseWall,
                                     HouseFloor = a.HouseFloor,
                                     UseElectricity = a.UseElectricity,
                                     SchoolDistance = a.SchoolDistance,
                                     ClinicDistance = a.ClinicDistance,
                                     WaterSourceDistance = a.WaterSourceDistance,
                                     WaterSourceUse = a.WaterSourceUse,
                                     RoadCondition = a.RoadCondition,
                                     IncomeFamily = a.IncomeFamily,
                                     HarvestOutput = a.HarvestOutput,
                                     NumberPet = a.NumberPet,
                                     FamilyType = a.FamilyType,
                                     TotalIncome = a.TotalIncome,
                                     IncomeSources = a.IncomeSources,
                                     IncomeOther = a.IncomeOther,
                                     ImagePath = a.ImagePath,
                                     ImageThumbnailPath = a.ImageThumbnailPath,
                                     ImageSignaturePath = a.ImageSignaturePath,
                                     StoryContent = a.StoryContent,
                                     Handicap = a.Handicap,
                                     ConsentName = a.ConsentName,
                                     ConsentRelationship = a.ConsentRelationship,
                                     ConsentVillage = a.ConsentVillage,
                                     ConsentWard = a.ConsentWard,
                                     SiblingsJoiningChildFund = a.SiblingsJoiningChildFund,
                                     Malformation = a.Malformation,
                                     Orphan = a.Orphan,
                                 }).FirstOrDefault();
                if (childProfiles == null)
                {
                    throw new Exception("Hồ sơ đã bị xóa bởi người dùng khách");
                }

                var village = listVillage.Where(r => r.Id.Equals(childProfiles.Address)).FirstOrDefault();
                if (village != null)
                {
                    childProfiles.Address = village.Name;
                }

                childProfiles.ListImage = (from img in db.ImageChildHistories.AsNoTracking()
                                           where img.ChildProfileId.Equals(id)
                                           select new ImageHistory
                                           {
                                               Image = img.ImagePath,
                                               CreateDate = img.UploadDate
                                           }).OrderByDescending(u => u.CreateDate).Take(3).ToList();
                childProfiles.ListImage = childProfiles.ListImage.OrderBy(u => u.CreateDate).ToList();
                childProfiles.ConvertObjectJsonToModel();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileBusiness.DetailProfile", ex.Message, id);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return childProfiles;
        }
        public void UpdateStory(ChildProfileModel model)
        {
            var data = db.ChildProfiles.FirstOrDefault(u => u.Id.Equals(model.Id));
            try
            {
                if (data != null)
                {
                    data.StoryContent = model.StoryContent;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileBusiness.UpdateStory", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }
        public void ConverFile(string filePathSource, string filePathResult)
        {
            #region[xuất file]
            //Initialize ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Initialize Application
                IApplication application = excelEngine.Excel;
                //Set default version for application
                application.DefaultVersion = ExcelVersion.Excel2013;
                //Open a workbook to be export as CSV
                IWorkbook workbook = application.Workbooks.Open(HttpContext.Current.Server.MapPath(filePathSource));
                //Accessing first worksheet in the workbook
                IWorksheet worksheet = workbook.Worksheets[0];
                //Save the workbook to csv format
                worksheet.SaveAs(HttpContext.Current.Server.MapPath(filePathResult), ",");
            }
            #endregion
        }
        public string ExportCSV(List<ChildProfileExportResult> list, string dateView)
        {
            //string filePathResult = "/Template/Export/" + dateView + "/Story213/Story/" + NTS.Common.Utils.Common.ConvertNameToTag(list[0].DistrictName) + ".csv";
            string filePathResult = "/Template/Export/" + dateView + "/213/Ho-So.csv";
            try
            {
                #region[xuất file]
                string filePath = "/Template/Export/Data" + dateView + ".xlsx";
                // Khỏi tạo bảng excel
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(HttpContext.Current.Server.MapPath("/Template/TemplateCSV.xlsx"));
                IWorksheet sheet = workbook.Worksheets[0];
                int total = list.Count;
                var listExport = (from a in list
                                  select new
                                  {
                                      a1 = a.SaleforceID,
                                      a2 = a.Content,
                                      a3 = a.CreateDate.ToString("dd/MM/yyyy")
                                  }).ToList();
                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (total == 0)
                {
                    iRangeData.Text = iRangeData.Text.Replace("<Data>", "");
                }
                else
                {
                    sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                }
                workbook.SaveAs(HttpContext.Current.Server.MapPath(filePath));
                ConverFile(filePath, filePathResult);
                #endregion
            }
            catch (Exception)
            { }
            return HttpContext.Current.Server.MapPath(filePathResult);
        }

        private void FindReplaceContent(WordDocument wordDocument, string textFind, string textReplace)
        {
            TextSelection textSelections = wordDocument.Find(textFind, false, true);
            if (textSelections != null)
            {
                WTextRange textRange = textSelections.GetAsOneRange();
                textRange.Text = textReplace;
            }
        }

        public string ExportWordForm2(string profileId)
        {
            ChildProfileModel childProfileModel = GetInfoChildProfileApproved(profileId);

            try
            {
                string templatePath = "/Template/Mau1.docx";
                string outPath = $"/Template/Export/Mau1_{DateTime.Now.ToString("ddMMyyyyHHmmssfff")}.docx";
                WordDocument wordDocument = new WordDocument(HttpContext.Current.Server.MapPath(templatePath), FormatType.Docx);
                wordDocument.Open(HttpContext.Current.Server.MapPath(templatePath));

                FindReplaceContent(wordDocument, "<infoDate>", !string.IsNullOrEmpty(DateTimeUtils.ConvertDateToDDMMYYYY(childProfileModel.InfoDate)) ? DateTimeUtils.ConvertDateToDDMMYYYY(childProfileModel.InfoDate).ToLower() : "");
                FindReplaceContent(wordDocument, "<employeeName>", childProfileModel.EmployeeName != null ? childProfileModel.EmployeeName : "");
                FindReplaceContent(wordDocument, "<programCode>", childProfileModel.ProgramCode != null ? childProfileModel.ProgramCode : "");

                var school = db.Schools.Find(childProfileModel.SchoolId);
                FindReplaceContent(wordDocument, "<school>", school != null ? school.SchoolName : "");

                string villageName = null;
                if (!string.IsNullOrEmpty(childProfileModel.Address))
                {
                    villageName = db.Villages.FirstOrDefault(u => u.Id.Equals(childProfileModel.Address)).Name;
                }

                FindReplaceContent(wordDocument, "<address>", villageName != null ? villageName : "");


                FindReplaceContent(wordDocument, "<childCode>", childProfileModel.ChildCode != null ? childProfileModel.ChildCode : "");
                FindReplaceContent(wordDocument, "<nameChild>", childProfileModel.Name != null ? childProfileModel.Name : "");
                FindReplaceContent(wordDocument, "<nickName>", childProfileModel.NickName != null ? childProfileModel.NickName : "");
                FindReplaceContent(wordDocument, "<totalIncome>", childProfileModel.TotalIncome != null ? childProfileModel.TotalIncome : "");
                FindReplaceContent(wordDocument, "<DateOfBirth>", !string.IsNullOrEmpty(DateTimeUtils.ConvertDateText(childProfileModel.DateOfBirth)) ? DateTimeUtils.ConvertDateText(childProfileModel.DateOfBirth).ToLower() : "");


                string WardName = db.AreaWards.FirstOrDefault(u => u.WardId.Equals(childProfileModel.WardId)).Name;
                string DistrictName = db.AreaDistricts.FirstOrDefault(u => u.DistrictId.Equals(childProfileModel.DistrictId)).Name;
                string ProvinceName = db.AreaUsers.FirstOrDefault(u => u.ProvinceId.Equals(childProfileModel.ProvinceId)).ProvinceName;
                string ReligionName = db.Religions.FirstOrDefault(u => u.Id.Equals(childProfileModel.ReligionId)).Name;
                string EthnicName = db.Ethnics.FirstOrDefault(u => u.Id.Equals(childProfileModel.EthnicId)).Name;
                FindReplaceContent(wordDocument, "<districtId>", DistrictName != null ? DistrictName : "");
                FindReplaceContent(wordDocument, "<wardId>", WardName != null ? WardName : "");
                FindReplaceContent(wordDocument, "<religionId>", ReligionName != null ? ReligionName : "");
                FindReplaceContent(wordDocument, "<ethnicId>", EthnicName != null ? EthnicName : "");

                if (childProfileModel.Gender.ToString().Equals("0"))
                {
                    FindReplaceContent(wordDocument, "<Female>", "☑");
                    FindReplaceContent(wordDocument, "<Male>", "☐");
                }
                else
                {
                    FindReplaceContent(wordDocument, "<Female>", "☐");
                    FindReplaceContent(wordDocument, "<Male>", "☑");
                }

                FindReplaceContent(wordDocument, "<Tooyoung>", childProfileModel.LeaningStatus.ToString().Equals("11") ? "☑" : "☐");
                FindReplaceContent(wordDocument, "<Dropout>", childProfileModel.LeaningStatus.ToString().Equals("12") ? "☑" : "☐");
                FindReplaceContent(wordDocument, "<Disability>", (childProfileModel.Handicap.HasValue && (bool)childProfileModel.Handicap )? "☑" : "☐");
                FindReplaceContent(wordDocument, "<Attendingkindergarten>", childProfileModel.LeaningStatus.ToString().Equals("14") ? "☑" : "☐");
                FindReplaceContent(wordDocument, "<Grade1>", childProfileModel.LeaningStatus.ToString().Equals("15") ? "☑" : "☐");
                FindReplaceContent(wordDocument, "<Grade2>", childProfileModel.LeaningStatus.ToString().Equals("16") ? "☑" : "☐");
               // FindReplaceContent(wordDocument, "<Grade3>", childProfileModel.LeaningStatus.ToString().Equals("17") ? "☑" : "☐");

                FindReplaceContent(wordDocument, "<classInfo>", childProfileModel.LeaningStatus.ToString().Equals("15") ? childProfileModel.ClassInfo : "");
                FindReplaceContent(wordDocument, "<classInfo2>", childProfileModel.LeaningStatus.ToString().Equals("16") ? childProfileModel.ClassInfo : "");

                FindReplaceContent(wordDocument, "<learningCapacityOther>", childProfileModel.LearningCapacityModel.OtherValue != null ? childProfileModel.LearningCapacityModel.OtherValue : "");

                var learningCapacity04 = childProfileModel.HealthModel.ListObject.FirstOrDefault(e => e.Id.Equals("04"));
                FindReplaceContent(wordDocument, "<healthItemOther_04>", learningCapacity04.OtherValue != null ? learningCapacity04.OtherValue : "");

                var learningCapacity05 = childProfileModel.HealthModel.ListObject.FirstOrDefault(e => e.Id.Equals("05"));
                FindReplaceContent(wordDocument, "<healthItemOther_05>", learningCapacity05.OtherValue != null ? learningCapacity04.OtherValue : "");

                FindReplaceContent(wordDocument, "<PersonalitOther>", childProfileModel.PersonalityModel.OtherValue != null ? childProfileModel.PersonalityModel.OtherValue : "");
                FindReplaceContent(wordDocument, "<HobbyOther>", childProfileModel.HobbyModel.OtherValue != null ? childProfileModel.HobbyModel.OtherValue : "");
                FindReplaceContent(wordDocument, "<DreamOther>", childProfileModel.DreamModel.OtherValue != null ? childProfileModel.DreamModel.OtherValue : "");
                FindReplaceContent(wordDocument, "<livingWithOther>", childProfileModel.LivingWithOtherModel.OtherValue != null ? childProfileModel.LivingWithOtherModel.OtherValue : "");
                FindReplaceContent(wordDocument, "<notLivingWithParentOther>", childProfileModel.NotLivingWithParentModel.OtherValue != null ? childProfileModel.NotLivingWithParentModel.OtherValue : "");
                FindReplaceContent(wordDocument, "<letterWriteOther>", childProfileModel.LetterWriteModel.OtherValue != null ? childProfileModel.LetterWriteModel.OtherValue : "");
                FindReplaceContent(wordDocument, "<houseTypeOther>", childProfileModel.HouseTypeModel.OtherValue != null ? childProfileModel.HouseTypeModel.OtherValue : "");
                FindReplaceContent(wordDocument, "<houseRoofOther>", childProfileModel.HouseRoofModel.OtherValue != null ? childProfileModel.HouseRoofModel.OtherValue : "");
                FindReplaceContent(wordDocument, "<houseWallOther>", childProfileModel.HouseWallModel.OtherValue != null ? childProfileModel.HouseWallModel.OtherValue : "");
                FindReplaceContent(wordDocument, "<houseFloorOther>", childProfileModel.HouseFloorModel.OtherValue != null ? childProfileModel.HouseFloorModel.OtherValue : "");
                FindReplaceContent(wordDocument, "<waterSourceUseOther>", childProfileModel.WaterSourceUseModel.OtherValue != null ? childProfileModel.WaterSourceUseModel.OtherValue : "");


                var incomFamily02 = childProfileModel.IncomeFamilyModel.ListObject.FirstOrDefault(e => e.Id.Equals("02"));
                FindReplaceContent(wordDocument, "<incomeFamilyModel_02>", incomFamily02.Value != null ? incomFamily02.Value : "");

                var harvestOutputModel01 = childProfileModel.HarvestOutputModel.ListObject.FirstOrDefault(e => e.Id.Equals("01"));
                var harvestOutputModel02 = childProfileModel.HarvestOutputModel.ListObject.FirstOrDefault(e => e.Id.Equals("02"));
                FindReplaceContent(wordDocument, "<harvestOutputModel_01>", harvestOutputModel01.Value != null ? harvestOutputModel01.Value : "");
                FindReplaceContent(wordDocument, "<harvestOutputModel_02>", harvestOutputModel02.Value != null ? harvestOutputModel02.Value : "");

                FindReplaceContent(wordDocument, "<numberPet>", childProfileModel.NumberPet != null ? childProfileModel.NumberPet : "");
                FindReplaceContent(wordDocument, "<familyType>", childProfileModel.FamilyType != null ? childProfileModel.FamilyType : "");

                var yearDead01 = childProfileModel.NotLivingWithParentModel.ListObject.FirstOrDefault(e => e.Id.Equals("01"));
                FindReplaceContent(wordDocument, "<Year1>", yearDead01.OtherValue != null ? yearDead01.OtherValue : "");

                var yearDead02 = childProfileModel.NotLivingWithParentModel.ListObject.FirstOrDefault(e => e.Id.Equals("02"));
                FindReplaceContent(wordDocument, "<Year2>", yearDead02.OtherValue != null ? yearDead02.OtherValue : "");

                var yearDead03 = childProfileModel.NotLivingWithParentModel.ListObject.FirstOrDefault(e => e.Id.Equals("03"));
                FindReplaceContent(wordDocument, "<Year3>", yearDead03.OtherValue != null ? yearDead03.OtherValue : "");


                for (int i = 0; i < childProfileModel.FavouriteSubjectModel.ListObject.Count; i++)
                {
                    if (childProfileModel.FavouriteSubjectModel.ListObject[i].Check == true)
                    {
                        FindReplaceContent(wordDocument, "<FS" + i + ">", "☑");
                    }
                    else
                    {
                        FindReplaceContent(wordDocument, "<FS" + i + ">", "☐");
                    }
                }

                for (int i = 0; i < childProfileModel.LearningCapacityModel.ListObject.Count; i++)
                {
                    if (childProfileModel.LearningCapacityModel.ListObject[i].Check == true)
                    {
                        FindReplaceContent(wordDocument, "<LC>", "☑");
                    }
                    else
                    {
                        FindReplaceContent(wordDocument, "<LC>", "☐");
                    }
                }

                for (int i = 0; i < childProfileModel.PersonalityModel.ListObject.Count; i++)
                {
                    if (childProfileModel.PersonalityModel.ListObject[i].Check == true)
                    {
                        FindReplaceContent(wordDocument, "<P>", "☑");
                    }
                    else
                    {
                        FindReplaceContent(wordDocument, "<P>", "☐");
                    }
                }

                for (int i = 0; i < childProfileModel.HobbyModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<HB>", childProfileModel.HobbyModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.DreamModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<DR>", childProfileModel.DreamModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.LivingWithParentModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<WP>", childProfileModel.LivingWithParentModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.NotLivingWithParentModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<NWP" + i + ">", childProfileModel.NotLivingWithParentModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.LivingWithOtherModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<WGP>", childProfileModel.LivingWithOtherModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.LetterWriteModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<Letter>", childProfileModel.LetterWriteModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.HouseTypeModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<HT>", childProfileModel.HouseTypeModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.HouseRoofModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<HR>", childProfileModel.HouseRoofModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.HouseWallModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<HW>", childProfileModel.HouseWallModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.HouseFloorModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<HF>", childProfileModel.HouseFloorModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.SchoolDistanceModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<toSchool>", childProfileModel.SchoolDistanceModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.ClinicDistanceModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<toHealthCenter>", childProfileModel.ClinicDistanceModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.WaterSourceDistanceModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<toWater>", childProfileModel.WaterSourceDistanceModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.WaterSourceDistanceModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<toWater>", childProfileModel.WaterSourceDistanceModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.WaterSourceUseModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<sourceWater>", childProfileModel.WaterSourceUseModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.RoadConditionModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<roadCondition>", childProfileModel.RoadConditionModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.IncomeOtherModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<incomOther>", childProfileModel.IncomeOtherModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.HouseworkModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<DHW" + i + ">", childProfileModel.HouseworkModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                for (int i = 0; i < childProfileModel.HealthModel.ListObject.Count; i++)
                {
                    FindReplaceContent(wordDocument, "<health>", childProfileModel.HealthModel.ListObject[i].Check == true ? "☑" : "☐");
                }

                var joinChild = childProfileModel.SiblingsJoiningChildFundModel.ListObject.FirstOrDefault(e => e.Id.Equals("01"));

                if (joinChild.Check == true)
                {
                    FindReplaceContent(wordDocument, "<joinYes>", "☑");
                    FindReplaceContent(wordDocument, "<joinYes1>", "☑");
                    FindReplaceContent(wordDocument, "<joinNo>", "☐");
                    FindReplaceContent(wordDocument, "<joinNo1>", "☐");
                    FindReplaceContent(wordDocument, "<joinYear>", joinChild.OtherValue);
                }
                else
                {
                    FindReplaceContent(wordDocument, "<joinYes>", "☐");
                    FindReplaceContent(wordDocument, "<joinYes1>", "☐");
                    FindReplaceContent(wordDocument, "<joinNo>", "☑");
                    FindReplaceContent(wordDocument, "<joinNo1>", "☑");
                    FindReplaceContent(wordDocument, "<joinYear>", "");
                }

                var malformation = childProfileModel.MalformationModel.ListObject.FirstOrDefault(e => e.Id.Equals("01"));

                if (malformation.Check == true)
                {
                    FindReplaceContent(wordDocument, "<malformationYes>", "☑");
                    FindReplaceContent(wordDocument, "<malformationYes1>", "☑");
                    FindReplaceContent(wordDocument, "<malformationNo>", "☐");
                    FindReplaceContent(wordDocument, "<malformationNo1>", "☐");

                }
                else
                {
                    FindReplaceContent(wordDocument, "<malformationYes>", "☐");
                    FindReplaceContent(wordDocument, "<malformationYes1>", "☐");
                    FindReplaceContent(wordDocument, "<malformationNo>", "☑");
                    FindReplaceContent(wordDocument, "<malformationNo1>", "☑");

                }

                var orphan = childProfileModel.OrphanModel.ListObject.FirstOrDefault(e => e.Id.Equals("01"));
                if (malformation.Check == true)
                {
                    FindReplaceContent(wordDocument, "<orphanYes>", "☑");
                    FindReplaceContent(wordDocument, "<orphanYes1>", "☑");
                    FindReplaceContent(wordDocument, "<orphanNo>", "☐");
                    FindReplaceContent(wordDocument, "<orphanNo1>", "☐");

                }
                else
                {
                    FindReplaceContent(wordDocument, "<orphanYes>", "☐");
                    FindReplaceContent(wordDocument, "<orphanYes1>", "☐");
                    FindReplaceContent(wordDocument, "<orphanNo>", "☑");
                    FindReplaceContent(wordDocument, "<orphanNo1>", "☑");

                }

                FindReplaceContent(wordDocument, "<consentName>", childProfileModel.ConsentName != null ? childProfileModel.ConsentName : "");
                FindReplaceContent(wordDocument, "<consentName1>", childProfileModel.ConsentName != null ? childProfileModel.ConsentName : "");
                FindReplaceContent(wordDocument, "<nameChild1>", childProfileModel.Name != null ? childProfileModel.Name : "");
                FindReplaceContent(wordDocument, "<nameChild2>", childProfileModel.Name != null ? childProfileModel.Name : "");
                FindReplaceContent(wordDocument, "<consentVillage>", childProfileModel.ConsentVillage != null ? childProfileModel.ConsentVillage : "");
                FindReplaceContent(wordDocument, "<consentVillage1>", childProfileModel.ConsentVillage != null ? childProfileModel.ConsentVillage : "");
                FindReplaceContent(wordDocument, "<consentRelationship>", childProfileModel.ConsentRelationship != null ? childProfileModel.ConsentRelationship : "");
                FindReplaceContent(wordDocument, "<consentWards>", childProfileModel.ConsentWard != null ? childProfileModel.ConsentWard : "");
                FindReplaceContent(wordDocument, "<consentWards1>", childProfileModel.ConsentWard != null ? childProfileModel.ConsentWard : "");


                WTable table = wordDocument.GetTableByFindText("<listFamily>");
                wordDocument.NTSReplaceFirst("<listFamily>", "");
                WTableRow templateRow;
                WTableRow row;
                int index = 1;
                templateRow = table.Rows[1].Clone();
                List<FamilyMemberModel> listFamily = new List<FamilyMemberModel>();
                listFamily = childProfileModel.ListFamilyMember.ToList();
                int totalBrother = 0;
                int totalSister = 0;
                foreach (var e in listFamily)
                {
                    //     outPath = string.Empty;
                    if (index > 1)
                    {
                        table.Rows.Insert(index, templateRow.Clone());
                    }
                    row = table.Rows[index];
                    row.Cells[0].Paragraphs[0].Text = (index).ToString();
                    row.Cells[1].Paragraphs[0].Text = e.Name != null ? e.Name : "";
                    row.Cells[2].Paragraphs[0].Text = !string.IsNullOrEmpty(DateTimeUtils.ConvertDateToDDMMYYYY(e.DateOfBirth).ToLower()) ? DateTimeUtils.ConvertDateToDDMMYYYY(e.DateOfBirth).ToLower().Substring(6, 4) : "";
                    var relationship = db.Relationships.FirstOrDefault(a => a.Id.Equals(e.RelationshipId)).Name;
                    row.Cells[3].Paragraphs[0].Text = relationship != null ? relationship : "";
                    row.Cells[4].Paragraphs[0].Text = e.Gender == 0 ? "Nữ" : "Nam";
                    var job = db.Jobs.FirstOrDefault(b => b.Id.Equals(e.Job)).Name;
                    row.Cells[5].Paragraphs[0].Text = job != null ? job : "";
                    row.Cells[6].Paragraphs[0].Text = e.LiveWithChild == 0 ? "Không" : "Có";
                    if (e.RelationshipId.ToString().Equals("R0009") || e.RelationshipId.ToString().Equals("R0010"))
                    {
                        totalBrother++;
                    }
                    if (e.RelationshipId.ToString().Equals("R0006") || e.RelationshipId.ToString().Equals("R0008"))
                    {
                        totalSister++;
                    }
                    index++;
                }

                FindReplaceContent(wordDocument, "<totalBrother>", totalBrother.ToString());
                FindReplaceContent(wordDocument, "<totalSister>", totalSister.ToString());


                wordDocument.Save(HttpContext.Current.Server.MapPath(outPath));

                return outPath;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý", ex.InnerException);
            }
        }

        public void AddImageChilByYear(ImageChildByYear model, HttpFileCollection httpFile)
        {
            var dateNow = DateTime.Now;
            if (httpFile.Count > 0)
            {
                var file = httpFile[0];
                NTS.Storage.ImageResult imageResult = Task.Run(async () =>
                {
                    return await AzureStorageUploadFiles.GetInstance().UploadImageAsync(file, dateNow.Year + "");
                }).Result;

                if (imageResult != null)
                {
                    var child = db.ChildProfiles.FirstOrDefault(u => u.Id.Equals(model.ChildProfileId));
                    var imgChild = db.ImageChildByYears.Where(u => u.ChildProfileId.Equals(model.ChildProfileId)).ToList();
                    var imgNow = imgChild.FirstOrDefault(u => u.Year == dateNow.Year);
                    if (imgNow == null)
                    {
                        //năm nay chưa có ảnh thì thêm mới
                        ImageChildByYear imageChildByYear = new ImageChildByYear();
                        imageChildByYear.Id = Guid.NewGuid().ToString();
                        imageChildByYear.ChildProfileId = model.ChildProfileId;
                        imageChildByYear.CreateDate = DateTime.Now;
                        imageChildByYear.Year = DateTime.Now.Year;
                        imageChildByYear.ImageUrl = imageResult.ImageOrigin;
                        imageChildByYear.ImageThumbnail = imageResult.ImageThumbnail;

                        imageChildByYear.ProvinceId = child.ProvinceId;
                        imageChildByYear.DistrictId = child.DistrictId;
                        imageChildByYear.WardId = child.WardId;
                        db.ImageChildByYears.Add(imageChildByYear);
                        db.SaveChanges();
                    }
                    else
                    {
                        imgNow.ProvinceId = child.ProvinceId;
                        imgNow.DistrictId = child.DistrictId;
                        imgNow.WardId = child.WardId;
                        imgNow.ImageThumbnail = imageResult.ImageThumbnail;
                        imgNow.ImageUrl = imageResult.ImageOrigin;
                        imgNow.CreateDate = DateTime.Now;
                        imgNow.Year = DateTime.Now.Year;
                        db.SaveChanges();
                    }
                }
            }
        }
        public List<ImageChildByYear> GetImageChild(string userId)
        {
            List<ImageChildByYear> listImage = new List<ImageChildByYear>();
            try
            {
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                if (userInfo.UserLever.Equals(Constants.LevelArea) || userInfo.UserLever.Equals(Constants.LevelTeacher))
                {
                    var lstWardId = (from a in db.AreaUsers.AsNoTracking()
                                     where a.Id.Equals(userInfo.AreaUserId)
                                     join c in db.AreaDistricts.AsNoTracking() on a.Id equals c.AreaUserId
                                     join d in db.AreaWards.AsNoTracking() on c.Id equals d.AreaDistrictId
                                     select d.WardId).ToList();
                    listImage = db.ImageChildByYears.Where(u => lstWardId.Contains(u.WardId)).ToList();
                }
                //else if (userInfo.UserLever.Equals(Constants.LevelOffice))
                //{
                //    var lstDistrictId = (from a in db.AreaUsers.AsNoTracking()
                //                         where a.Id.Equals(userInfo.AreaUserId)
                //                         join c in db.AreaDistricts.AsNoTracking() on a.Id equals c.AreaUserId
                //                         select c.DistrictId).ToList();
                //    listImage = db.ImageChildByYears.Where(u => lstDistrictId.Contains(u.DistrictId)).ToList();
                //}
                else
                {//hn thì xem hết
                    listImage = db.ImageChildByYears.ToList();
                }

            }
            catch (Exception)
            { }
            return listImage;
        }
        public List<ImageChildByYearView> GetImageChildByYear(string userId, int? year, string wardId)
        {
            List<ImageChildByYearView> list = new List<ImageChildByYearView>();
            List<ImageChildByYear> listImage = new List<ImageChildByYear>();
            try
            {
                if (string.IsNullOrEmpty(wardId))
                {
                    var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                    if (userInfo.UserLever.Equals(Constants.LevelArea) || userInfo.UserLever.Equals(Constants.LevelTeacher))
                    {
                        var lstWardId = (from a in db.AreaUsers.AsNoTracking()
                                         where a.Id.Equals(userInfo.AreaUserId)
                                         join c in db.AreaDistricts.AsNoTracking() on a.Id equals c.AreaUserId
                                         join d in db.AreaWards.AsNoTracking() on c.Id equals d.AreaDistrictId
                                         select d.WardId).ToList();
                        listImage = db.ImageChildByYears.AsNoTracking().Where(u => lstWardId.Contains(u.WardId) && (year == null || year == u.Year)).ToList();
                    }

                    else
                    {
                        listImage = db.ImageChildByYears.AsNoTracking().Where(u => (year == null || year == u.Year)).ToList();
                    }
                }
                else
                {
                    listImage = db.ImageChildByYears.AsNoTracking().Where(u => wardId.Equals(u.WardId) && (year == null || year == u.Year)).ToList();
                }

                List<string> childProfileIds = listImage.Select(r => r.ChildProfileId).ToList();
                list = (from b in db.ChildProfiles.AsNoTracking().Where(r => childProfileIds.Contains(r.Id))
                        select new ImageChildByYearView
                        {
                            ChildCode = b.ChildCode,
                            ChildName = b.Name,
                            Id = b.Id,
                            ProvinceId = b.ProvinceId,
                            DistrictId = b.DistrictId,
                            WardId = b.WardId,
                            ChildProfileId = b.Id,
                        }).OrderByDescending(u => u.ChildCode).ToList();

                foreach (var item in list)
                {
                    item.ImageThumbnail = listImage.Where(r => r.ChildProfileId.Equals(item.ChildProfileId)).First().ImageThumbnail;
                    item.ImageUrl = listImage.Where(r => r.ChildProfileId.Equals(item.ChildProfileId)).First().ImageUrl;
                    item.CreateDate = listImage.Where(r => r.ChildProfileId.Equals(item.ChildProfileId)).First().CreateDate;
                    item.Id = listImage.Where(r => r.ChildProfileId.Equals(item.ChildProfileId)).First().Id;
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        public List<ImageChildByYear> GetImageByChildId(string id)
        {
            List<ImageChildByYear> listImage = new List<ImageChildByYear>();
            try
            {
                listImage = db.ImageChildByYears.OrderByDescending(u => u.CreateDate).Where(u => u.ChildProfileId.Equals(id)).ToList();
                return listImage;
            }
            catch (Exception)
            { }
            return listImage;
        }
        public void DeleteImageChild(ImageActionModel model)
        {
            var listIdDel = model.ImageId;
            try
            {
                var data = db.ImageChildByYears.Where(u => listIdDel.Contains(u.Id)).ToList();
                if (data.Count > 0)
                {
                    db.ImageChildByYears.RemoveRange(data);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileBusiness.DeleteImageChild", ex.Message, model);
                throw new Exception("Có lỗi phát sinh, vui lòng thử lại");
            }
        }

        public List<ImageChildByYearView> ViewImageChildLevelByYear(int year, string type, string areaId)
        {
            List<ImageChildByYearView> list = new List<ImageChildByYearView>();
            List<ImageChildByYear> listImage = new List<ImageChildByYear>();
            try
            {
                if (type.Equals("province"))
                {
                    var lstDistrictId = (from a in db.Districts.AsNoTracking()
                                         where a.ProvinceId.Equals(areaId)
                                         select a.Id).ToList();
                    listImage = db.ImageChildByYears.Where(u => lstDistrictId.Contains(u.DistrictId) && year == u.Year).ToList();
                }
                else
                {
                    var lstWardId = (from a in db.Wards.AsNoTracking()
                                     where a.DistrictId.Equals(areaId)
                                     select a.Id).ToList();
                    listImage = db.ImageChildByYears.Where(u => lstWardId.Contains(u.WardId) && year == u.Year).ToList();
                }
                List<string> childProfileIds = listImage.Select(r => r.ChildProfileId).ToList();
                list = (from b in db.ChildProfiles.AsNoTracking()
                        where childProfileIds.Contains(b.Id)
                        select new ImageChildByYearView
                        {
                            ChildCode = b.ChildCode,
                            ChildName = b.Name,
                            ProvinceId = b.ProvinceId,
                            DistrictId = b.DistrictId,
                            WardId = b.WardId
                        }).OrderByDescending(u => u.ChildCode).ToList();

                foreach (var item in list)
                {
                    item.ImageThumbnail = listImage.Where(r => r.ChildProfileId.Equals(item.ChildProfileId)).First().ImageThumbnail;
                    item.ImageUrl = listImage.Where(r => r.ChildProfileId.Equals(item.ChildProfileId)).First().ImageUrl;
                    item.CreateDate = listImage.Where(r => r.ChildProfileId.Equals(item.ChildProfileId)).First().CreateDate;
                    item.Id = listImage.Where(r => r.ChildProfileId.Equals(item.ChildProfileId)).First().Id;
                }
            }
            catch (Exception)
            { }
            return list;
        }

        public List<ImageChildByYear> MenuImageChild(string userId)
        {
            List<ImageChildByYear> listImage = new List<ImageChildByYear>();
            try
            {
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                if (userInfo.UserLever.Equals(Constants.LevelArea) || userInfo.UserLever.Equals(Constants.LevelTeacher))
                {
                    var lstWardId = (from a in db.AreaUsers.AsNoTracking()
                                     where a.Id.Equals(userInfo.AreaUserId)
                                     join c in db.AreaDistricts.AsNoTracking() on a.Id equals c.AreaUserId
                                     join d in db.AreaWards.AsNoTracking() on c.Id equals d.AreaDistrictId
                                     select d.WardId).ToList();
                    listImage = db.ImageChildByYears.Where(u => lstWardId.Contains(u.WardId)).ToList();
                }
                else
                {//hn thì xem hết
                    listImage = db.ImageChildByYears.ToList();
                }

            }
            catch (Exception)
            { }
            return listImage;
        }
        public void ImportProfile(string createBy, HttpPostedFile file)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                string nameCreate = string.Empty;
                nameCreate = db.Users.FirstOrDefault(u => u.Id.Equals(createBy))?.Name;
                var fileArray = file.FileName.ToString().Split('.');
                string fileName = string.Empty;
                fileName = Guid.NewGuid().ToString() + "." + fileArray[fileArray.Length - 1];
                string pathFolder = "Template/Upload/";
                string pathFolderServer = HostingEnvironment.MapPath("~/" + pathFolder);
                string fileResult = string.Empty;
                try
                {
                    #region[tải file lên để đọc]
                    // Kiểm tra folder là tên của ProjectId đã tồn tại chưa.
                    if (!Directory.Exists(pathFolderServer))
                    {
                        Directory.CreateDirectory(pathFolderServer);
                    }
                    // kiểm tra size file > 0
                    if (file.ContentLength > 0)
                    {
                        file.SaveAs(pathFolderServer + fileName);
                    }
                    #endregion
                }
                catch (Exception)
                { throw new Exception("Xử lý file excel lỗi, vui lòng thử lại"); }

                string keyAllProvince = ConfigurationManager.AppSettings["cacheNotify"] + "AllProvince:";
                List<ComboboxResult> lProvince = redisService.Get<List<ComboboxResult>>(keyAllProvince + "lProvince");
                if (lProvince == null)
                {
                    lProvince = (from a in db.Provinces.AsNoTracking()
                                 select new ComboboxResult
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).ToList();
                    try
                    {
                        foreach (var item in lProvince)
                        {
                            item.UnsignName = VietnameseUtils.RemoveSign4VietnameseString(item.Name);
                        }

                        redisService.Add(keyAllProvince + "lProvince", lProvince);
                    }
                    catch (Exception)
                    { }
                }
                List<ComboboxResult> lDistrict = redisService.Get<List<ComboboxResult>>(keyAllProvince + "lDistrict");
                if (lDistrict == null)
                {
                    lDistrict = (from a in db.Districts.AsNoTracking()
                                 select new ComboboxResult
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     PId = a.ProvinceId,
                                 }).ToList();
                    try
                    {
                        foreach (var item in lDistrict)
                        {
                            item.UnsignName = VietnameseUtils.RemoveSign4VietnameseString(item.Name);
                        }
                        redisService.Add(keyAllProvince + "lDistrict", lDistrict);
                    }
                    catch (Exception)
                    { }
                }
                List<ComboboxResult> lWard = redisService.Get<List<ComboboxResult>>(keyAllProvince + "lWard");
                if (lWard == null)
                {
                    lWard = (from a in db.Wards.AsNoTracking()
                             select new ComboboxResult
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 PId = a.DistrictId,
                             }).ToList();
                    try
                    {
                        foreach (var item in lWard)
                        {
                            item.UnsignName = VietnameseUtils.RemoveSign4VietnameseString(item.Name);
                        }
                        redisService.Add(keyAllProvince + "lWard", lWard);
                    }
                    catch (Exception)
                    { }
                }

                var lVillage = (from a in db.Villages.AsNoTracking()
                                select new ComboboxResult
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    PId = a.WardId,
                                }).ToList();

                foreach (var item in lVillage)
                {
                    item.UnsignName = VietnameseUtils.RemoveSign4VietnameseString(item.Name);
                }

                var lSchools = (from a in db.Schools.AsNoTracking()
                                select new ComboboxResult
                                { Id = a.Id, Name = a.SchoolName, PId = a.WardId }).ToList();
                Village itemVillages;
                School itemSchool;
                var lReligion = (from a in db.Religions.AsNoTracking()
                                 select new ComboboxResult
                                 { Id = a.Id, Name = a.Name }).ToList();
                var lEthnic = (from a in db.Ethnics.AsNoTracking()
                               select new ComboboxResult
                               { Id = a.Id, Name = a.Name }).ToList();
                var lPeofile = db.ChildProfiles.ToList();
                ChildProfile childProfileItem;
                #region[đọc file excel]
                ExcelEngine excelEngine = new ExcelEngine();
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open(pathFolderServer + fileName);
                string provinceName = string.Empty;
                string districtName = string.Empty;
                string wardName = string.Empty;
                string villageName = string.Empty;

                string father = string.Empty;
                string mother = string.Empty;

                string programCode = string.Empty;
                string childCode = string.Empty;
                string childName = string.Empty;
                string schoolName = string.Empty;
                string className = string.Empty;
                string religionName = string.Empty;
                string ethnicName = string.Empty;
                string gender = string.Empty;
                string dateOfBirth = string.Empty;

                var saleforceID = string.Empty;

                string familyMember = string.Empty;

                List<FamilyMemberModel> listFamilyMemberModel = new List<FamilyMemberModel>();
                FamilyMemberModel itemFamilyMemberModel;
                ComboboxResult comboboxResult;
                IWorksheet sheet = workbook.Worksheets[0];

                int countRow = sheet.Rows.Count(); int startRow = 3;
                bool isInsert = false;
                string provinceUnsignName = string.Empty;
                string districtUnsignName = string.Empty;
                string wardUnsignName = string.Empty;
                string villageUnsignName = string.Empty;

                for (int indexRow = startRow; indexRow <= countRow; indexRow++)
                {
                    listFamilyMemberModel = new List<FamilyMemberModel>();
                    familyMember = string.Empty;

                    provinceName = sheet.Range[indexRow, 1].DisplayText.Trim();
                    districtName = sheet.Range[indexRow, 2].DisplayText.Trim();
                    wardName = sheet.Range[indexRow, 3].DisplayText.Trim();
                    villageName = sheet.Range[indexRow, 4].DisplayText.Trim();
                    father = sheet.Range[indexRow, 5].DisplayText.Trim();
                    mother = sheet.Range[indexRow, 6].DisplayText.Trim();
                    programCode = sheet.Range[indexRow, 7].DisplayText.Trim();
                    childCode = sheet.Range[indexRow, 8].DisplayText.Trim().Replace("\n", "");
                    childName = sheet.Range[indexRow, 9].DisplayText.Trim();
                    schoolName = sheet.Range[indexRow, 10].DisplayText.Split(new char[] { '-' }).FirstOrDefault().Trim();
                    className = sheet.Range[indexRow, 10].DisplayText.Split(new char[] { '-' }).LastOrDefault().Trim();
                    religionName = sheet.Range[indexRow, 11].DisplayText.Trim();
                    ethnicName = sheet.Range[indexRow, 12].DisplayText.Trim();
                    gender = sheet.Range[indexRow, 13].DisplayText.Trim();
                    dateOfBirth = sheet.Range[indexRow, 14].DisplayText.Trim();
                    saleforceID = sheet.Range[indexRow, 15].DisplayText.Trim();

                    if (!string.IsNullOrEmpty(father))
                    {
                        itemFamilyMemberModel = new FamilyMemberModel();
                        itemFamilyMemberModel.Name = father;
                        itemFamilyMemberModel.Gender = 1;
                        itemFamilyMemberModel.LiveWithChild = 1;
                        itemFamilyMemberModel.Job = "1";//khác
                        itemFamilyMemberModel.RelationshipId = "R0001";
                        listFamilyMemberModel.Add(itemFamilyMemberModel);
                    }
                    if (!string.IsNullOrEmpty(mother))
                    {
                        itemFamilyMemberModel = new FamilyMemberModel();
                        itemFamilyMemberModel.Name = mother;
                        itemFamilyMemberModel.Gender = 0;
                        itemFamilyMemberModel.LiveWithChild = 1;
                        itemFamilyMemberModel.Job = "1";//khác
                        itemFamilyMemberModel.RelationshipId = "R0007";
                        listFamilyMemberModel.Add(itemFamilyMemberModel);
                    }

                    familyMember = JsonConvert.SerializeObject(listFamilyMemberModel);

                    childProfileItem = lPeofile.FirstOrDefault(u => u.ChildCode.ToLower().Equals(childCode.ToLower()));

                    if (childProfileItem != null)
                    {
                        isInsert = false;
                    }
                    else
                    {
                        isInsert = true;
                        childProfileItem = new ChildProfile();
                        childProfileItem.Id = Guid.NewGuid().ToString();
                        childProfileItem.InfoDate = DateTime.Now;
                        childProfileItem.CreateDate = DateTime.Now;
                        childProfileItem.UpdateDate = childProfileItem.CreateDate;
                        childProfileItem.CreateBy = createBy;
                        childProfileItem.UpdateBy = createBy;
                        childProfileItem.IsDelete = false;
                    }

                    childProfileItem.EmployeeName = nameCreate;
                    childProfileItem.ProgramCode = programCode;
                    childProfileItem.Gender = gender.ToLower().Equals("nam") ? 1 : 0;
                    childProfileItem.ChildCode = childCode;
                    childProfileItem.Name = childName;
                    childProfileItem.FamilyMember = familyMember;
                    childProfileItem.ProcessStatus = Constants.ApproveOffice;
                    childProfileItem.SaleforceId = saleforceID;

                    // Xử lý ngày sinh của trẻ
                    if (string.IsNullOrEmpty(dateOfBirth))
                    {
                        childProfileItem.DateOfBirth = new DateTime();
                    }
                    else
                    {
                        try
                        {
                            childProfileItem.DateOfBirth = sheet.Range[indexRow, 14].DateTime;
                            if (childProfileItem.DateOfBirth.Year == 1)
                            {
                                childProfileItem.DateOfBirth = NTS.Common.Utils.DateTimeUtils.ConvertDateFromStr(dateOfBirth);
                            }
                        }
                        catch (Exception ex)
                        {
                            childProfileItem.DateOfBirth = NTS.Common.Utils.DateTimeUtils.ConvertDateFromStr(dateOfBirth);
                        }
                    }


                    //tỉnh
                    if (!string.IsNullOrEmpty(provinceName))
                    {
                        provinceUnsignName = VietnameseUtils.RemoveSign4VietnameseString(provinceName.Trim());
                        comboboxResult = lProvince.FirstOrDefault(u => u.UnsignName.ToLower().Trim().Equals(provinceUnsignName.ToLower().Trim()));
                        if (comboboxResult == null)
                        {
                            throw new Exception("Tỉnh có tên sau không tồn tại: " + provinceName);
                        }
                        childProfileItem.ProvinceId = comboboxResult.Id;
                    }

                    //huyện
                    if (!string.IsNullOrEmpty(districtName))
                    {
                        districtUnsignName = VietnameseUtils.RemoveSign4VietnameseString(districtName.Trim());
                        comboboxResult = lDistrict.FirstOrDefault(u => u.PId.Equals(childProfileItem.ProvinceId) && u.UnsignName.ToLower().Trim().Equals(districtUnsignName.ToLower().Trim()));
                        if (comboboxResult == null)
                        {
                            comboboxResult = lDistrict.FirstOrDefault(u => u.UnsignName.ToLower().Trim().Equals(districtUnsignName.ToLower().Trim()));

                            if (comboboxResult == null)
                            {
                                throw new Exception("Huyện có tên sau không tồn tại: " + districtName);
                            }
                        }

                        childProfileItem.DistrictId = comboboxResult.Id;
                    }
                    //xã
                    if (!string.IsNullOrEmpty(wardName))
                    {// chi xu ly khi chua có
                        wardUnsignName = VietnameseUtils.RemoveSign4VietnameseString(wardName.Trim());

                        comboboxResult = lWard.FirstOrDefault(u => u.PId.Equals(childProfileItem.DistrictId) && u.UnsignName.ToLower().Trim().Equals(wardUnsignName.ToLower().Trim()));
                        if (comboboxResult == null)
                        {
                            comboboxResult = lWard.FirstOrDefault(u => u.UnsignName.ToLower().Trim().Equals(wardUnsignName.ToLower().Trim()));

                            if (comboboxResult == null)
                            {
                                throw new Exception("Xã có tên sau không tồn tại: " + wardName);
                            }
                        }
                        childProfileItem.WardId = comboboxResult.Id;
                    }
                    //làng
                    if (childProfileItem.WardId != null)
                    {
                        villageUnsignName = VietnameseUtils.RemoveSign4VietnameseString(villageName);

                        comboboxResult = lVillage.FirstOrDefault(u => u.PId.Equals(childProfileItem.WardId) && u.UnsignName.ToLower().Trim().Equals(villageUnsignName.ToLower().Trim()));
                        if (comboboxResult == null)
                        {
                            itemVillages = new Village();
                            itemVillages.Id = Guid.NewGuid().ToString();
                            itemVillages.Name = villageName;
                            itemVillages.NameEN = VietnameseUtils.RemoveSign4VietnameseString(villageName);
                            itemVillages.Type = "Xóm";
                            itemVillages.WardId = childProfileItem.WardId;
                            db.Villages.Add(itemVillages);
                            lVillage.Add(new ComboboxResult { Id = itemVillages.Id, Name = itemVillages.Name, PId = childProfileItem.WardId, UnsignName = villageUnsignName });
                            childProfileItem.Address = itemVillages.Id;
                        }
                        else
                        {
                            childProfileItem.Address = comboboxResult.Id;
                        }

                        childProfileItem.FullAddress = villageName + " - " + wardName + " - " + districtName + " - " + provinceName;
                    }

                    //truong học
                    if (!string.IsNullOrEmpty(schoolName))
                    {
                        string schoolStatus = VietnameseUtils.RemoveSign4VietnameseString(schoolName).ToLower();

                        switch (schoolStatus)
                        {
                            case Constants.Scholl_Connho:
                                childProfileItem.LeaningStatus = "11";//còn nhỏ
                                break;
                            case Constants.Scholl_Bohoc:
                                childProfileItem.LeaningStatus = "12";//bỏ học
                                break;
                            case Constants.Scholl_Khuyettat:
                                childProfileItem.LeaningStatus = "13";//Khuyết tật
                                break;
                            case Constants.Scholl_Maugiao:
                                childProfileItem.LeaningStatus = "14";//mau giao
                                break;
                            default:
                                comboboxResult = lSchools.FirstOrDefault(u => u.PId.Equals(childProfileItem.WardId) && u.Name.ToLower().Trim().Equals(schoolName.ToLower().Trim()));
                                if (comboboxResult == null)
                                {
                                    itemSchool = new School();
                                    itemSchool.Id = Guid.NewGuid().ToString();
                                    itemSchool.SchoolName = schoolName;
                                    itemSchool.WardId = childProfileItem.WardId;
                                    db.Schools.Add(itemSchool);
                                    lSchools.Add(new ComboboxResult { Id = itemSchool.Id, Name = itemSchool.SchoolName, PId = childProfileItem.WardId });
                                    childProfileItem.SchoolId = itemSchool.Id;
                                }
                                else
                                {
                                    childProfileItem.SchoolId = comboboxResult.Id;
                                }
                                if (className.StartsWith("1") || className.StartsWith("2") || className.StartsWith("3") || className.StartsWith("4") || className.StartsWith("5"))
                                {
                                    childProfileItem.LeaningStatus = "15";
                                    childProfileItem.ClassInfo = className;
                                }
                                else if (className.StartsWith("6") || className.StartsWith("7") || className.StartsWith("8") || className.StartsWith("9"))
                                {
                                    childProfileItem.LeaningStatus = "16";
                                    childProfileItem.ClassInfo = className;
                                }
                                else if (className.StartsWith("10") || className.StartsWith("11") || className.StartsWith("12"))
                                {
                                    childProfileItem.LeaningStatus = "17";
                                    childProfileItem.ClassInfo = className;
                                }
                                else
                                {
                                    childProfileItem.LeaningStatus = "14";//mau giao
                                }

                                break;
                        }
                    }
                    else
                    {
                        childProfileItem.LeaningStatus = "11";
                    }
                    //dân tộc
                    comboboxResult = lEthnic.FirstOrDefault(u => u.Name.ToLower().Equals(ethnicName.ToLower()));
                    if (comboboxResult != null)
                    {
                        childProfileItem.EthnicId = comboboxResult.Id;
                    }
                    else
                    {
                        childProfileItem.EthnicId = Constants.Ethnic_Kinh;
                    }
                    //tôn giáo

                    comboboxResult = lReligion.FirstOrDefault(u => u.Name.ToLower().Contains(religionName.ToLower()));
                    if (comboboxResult != null)
                    {
                        childProfileItem.ReligionId = comboboxResult.Id;
                    }
                    else
                    {
                        childProfileItem.ReligionId = Constants.Religion_None;
                    }
                    if (isInsert && !string.IsNullOrEmpty(childProfileItem.ChildCode))
                    {//thêm mới hs
                        db.ChildProfiles.Add(childProfileItem);
                    }
                }
                #endregion
                db.SaveChanges();
                trans.Commit();
            }

        }

        public SearchResultObject<ChildProfileMobileSearchResult> GetChildProfiles(ChildProfileDownloadSearch childProfileDownloadSearch)
        {
            try
            {
                SearchResultObject<ChildProfileMobileSearchResult> searchResult = new SearchResultObject<ChildProfileMobileSearchResult>();
                ChildProfileMobileSearchResult childProfileMobileSearchResult;
                List<ChildProfileMobileSearchResult> listData = new List<ChildProfileMobileSearchResult>();
                if (childProfileDownloadSearch.ListChildProfileId.Count > 0)
                {
                    foreach (var item in childProfileDownloadSearch.ListChildProfileId)
                    {
                        //var updateProfiles = db.ChildProfileUpdates.AsNoTracking().Where(r => r.ChildProfileId.Equals(item)).ToList();
                        var update = (from a in db.ChildProfileUpdates.AsNoTracking()
                                          //join b in db.ChildProfiles.AsNoTracking() on a.ChildProfileId equals b.Id into abm
                                          //from ab in abm.DefaultIfEmpty()
                                      where item.Equals(a.ChildProfileId)
                                      join s in db.Schools.AsNoTracking() on a.SchoolId equals s.Id into ctm
                                      from ct in ctm.DefaultIfEmpty()
                                      join v in db.Villages.AsNoTracking() on a.Address equals v.Id into avm
                                      from afv in avm.DefaultIfEmpty()
                                      select new ChildProfileMobileSearchResult
                                      {
                                          Id = a.ChildProfileId,
                                          InfoDate = a.InfoDate,
                                          EmployeeName = a.EmployeeName,
                                          ProgramCode = a.ProgramCode,
                                          ProvinceId = a.ProvinceId,
                                          DistrictId = a.DistrictId,
                                          WardId = a.WardId,
                                          Address = a.Address,
                                          FullAddress = a.FullAddress,
                                          ChildCode = a.ChildCode,
                                          SchoolId = a.SchoolId,
                                          SchoolOtherName = a.SchoolOtherName,
                                          EthnicId = a.EthnicId,
                                          ReligionId = a.ReligionId,
                                          Name = a.Name,
                                          NickName = a.NickName,
                                          Gender = a.Gender,
                                          DateOfBirth = a.DateOfBirth,
                                          LeaningStatus = a.LeaningStatus,
                                          ClassInfo = a.ClassInfo,
                                          FavouriteSubject = a.FavouriteSubject,
                                          LearningCapacity = a.LearningCapacity,
                                          Housework = a.Housework,
                                          Health = a.Health,
                                          Personality = a.Personality,
                                          Hobby = a.Hobby,
                                          Dream = a.Dream,
                                          FamilyMember = a.FamilyMember,
                                          LivingWithParent = a.LivingWithParent,
                                          NotLivingWithParent = a.NotLivingWithParent,
                                          LivingWithOther = a.LivingWithOther,
                                          LetterWrite = a.LetterWrite,
                                          HouseType = a.HouseType,
                                          HouseRoof = a.HouseRoof,
                                          HouseWall = a.HouseWall,
                                          HouseFloor = a.HouseFloor,
                                          UseElectricity = a.UseElectricity,
                                          SchoolDistance = a.SchoolDistance,
                                          ClinicDistance = a.ClinicDistance,
                                          WaterSourceDistance = a.WaterSourceDistance,
                                          WaterSourceUse = a.WaterSourceUse,
                                          RoadCondition = a.RoadCondition,
                                          IncomeFamily = a.IncomeFamily,
                                          HarvestOutput = a.HarvestOutput,
                                          NumberPet = a.NumberPet,
                                          FamilyType = a.FamilyType,
                                          TotalIncome = a.TotalIncome,
                                          IncomeSources = a.IncomeSources,
                                          IncomeOther = a.IncomeOther,
                                          //StoryContent = ab.StoryContent,
                                          ImagePath = a.ImagePath,
                                          ImageThumbnailPath = a.ImageThumbnailPath,
                                          //AreaApproverId = ab.AreaApproverId,
                                          //AreaApproverDate = ab.AreaApproverDate,
                                          //OfficeApproveBy = ab.OfficeApproveBy,
                                          //OfficeApproveDate = ab.OfficeApproveDate,
                                          ProcessStatus = a.ProcessStatus,
                                          //IsDelete = ab.IsDelete,
                                          CreateBy = a.UpdateBy,
                                          CreateDate = a.UpdateDate,
                                          UpdateBy = a.UpdateBy,
                                          UpdateDate = a.UpdateDate,
                                          ConsentName = a.ConsentName,
                                          ConsentRelationship = a.ConsentRelationship,
                                          ConsentVillage = a.ConsentVillage,
                                          ConsentWard = a.ConsentWard,
                                          SiblingsJoiningChildFund = a.SiblingsJoiningChildFund,
                                          Malformation = a.Malformation,
                                          Orphan = a.Orphan,
                                          EmployeeTitle = a.EmployeeTitle,
                                          ImageSignaturePath = a.ImageSignaturePath,
                                          ImageSignatureThumbnailPath = a.ImageSignatureThumbnailPath,
                                          SaleforceId = a.SaleforceId,
                                          Handicap = a.Handicap,
                                          ImageSize = a.ImageSize,
                                          Avata = a.ImageThumbnailPath,
                                          Status = a.ProcessStatus,
                                          School = (ct != null ? ct.SchoolName : a.SchoolOtherName) + (afv != null ? " | Xóm/Village: " + afv.Name : ""),

                                      }).FirstOrDefault();

                        if (update != null)
                        {
                            listData.Add(update);
                        }
                        else
                        {
                            var childProfile = (from a in db.ChildProfiles.AsNoTracking()
                                                where item.Equals(a.Id) && a.IsDelete.Equals(Constants.IsUse)
                                                join s in db.Schools.AsNoTracking() on a.SchoolId equals s.Id into ctm
                                                from ct in ctm.DefaultIfEmpty()
                                                join v in db.Villages.AsNoTracking() on a.Address equals v.Id into avm
                                                from afv in avm.DefaultIfEmpty()
                                                select new ChildProfileMobileSearchResult()
                                                {
                                                    Id = a.Id,
                                                    InfoDate = a.InfoDate,
                                                    EmployeeName = a.EmployeeName,
                                                    ProgramCode = a.ProgramCode,
                                                    ProvinceId = a.ProvinceId,
                                                    DistrictId = a.DistrictId,
                                                    WardId = a.WardId,
                                                    Address = a.Address,
                                                    FullAddress = a.FullAddress,
                                                    ChildCode = a.ChildCode,
                                                    SchoolId = a.SchoolId,
                                                    SchoolOtherName = a.SchoolOtherName,
                                                    EthnicId = a.EthnicId,
                                                    ReligionId = a.ReligionId,
                                                    Name = a.Name,
                                                    NickName = a.NickName,
                                                    Gender = a.Gender,
                                                    DateOfBirth = a.DateOfBirth,
                                                    LeaningStatus = a.LeaningStatus,
                                                    ClassInfo = a.ClassInfo,
                                                    FavouriteSubject = a.FavouriteSubject,
                                                    LearningCapacity = a.LearningCapacity,
                                                    Housework = a.Housework,
                                                    Health = a.Health,
                                                    Personality = a.Personality,
                                                    Hobby = a.Hobby,
                                                    Dream = a.Dream,
                                                    FamilyMember = a.FamilyMember,
                                                    LivingWithParent = a.LivingWithParent,
                                                    NotLivingWithParent = a.NotLivingWithParent,
                                                    LivingWithOther = a.LivingWithOther,
                                                    LetterWrite = a.LetterWrite,
                                                    HouseType = a.HouseType,
                                                    HouseRoof = a.HouseRoof,
                                                    HouseWall = a.HouseWall,
                                                    HouseFloor = a.HouseFloor,
                                                    UseElectricity = a.UseElectricity,
                                                    SchoolDistance = a.SchoolDistance,
                                                    ClinicDistance = a.ClinicDistance,
                                                    WaterSourceDistance = a.WaterSourceDistance,
                                                    WaterSourceUse = a.WaterSourceUse,
                                                    RoadCondition = a.RoadCondition,
                                                    IncomeFamily = a.IncomeFamily,
                                                    HarvestOutput = a.HarvestOutput,
                                                    NumberPet = a.NumberPet,
                                                    FamilyType = a.FamilyType,
                                                    TotalIncome = a.TotalIncome,
                                                    IncomeSources = a.IncomeSources,
                                                    IncomeOther = a.IncomeOther,
                                                    StoryContent = a.StoryContent,
                                                    ImagePath = a.ImagePath,
                                                    ImageThumbnailPath = a.ImageThumbnailPath,
                                                    AreaApproverId = a.AreaApproverId,
                                                    AreaApproverDate = a.AreaApproverDate,
                                                    OfficeApproveBy = a.OfficeApproveBy,
                                                    OfficeApproveDate = a.OfficeApproveDate,
                                                    ProcessStatus = a.ProcessStatus,
                                                    IsDelete = a.IsDelete,
                                                    CreateBy = a.CreateBy,
                                                    CreateDate = a.CreateDate,
                                                    UpdateBy = a.UpdateBy,
                                                    UpdateDate = a.UpdateDate,
                                                    ConsentName = a.ConsentName,
                                                    ConsentRelationship = a.ConsentRelationship,
                                                    ConsentVillage = a.ConsentVillage,
                                                    ConsentWard = a.ConsentWard,
                                                    SiblingsJoiningChildFund = a.SiblingsJoiningChildFund,
                                                    Malformation = a.Malformation,
                                                    Orphan = a.Orphan,
                                                    EmployeeTitle = a.EmployeeTitle,
                                                    ImageSignaturePath = a.ImageSignaturePath,
                                                    ImageSignatureThumbnailPath = a.ImageSignatureThumbnailPath,
                                                    SaleforceId = a.SaleforceId,
                                                    Handicap = a.Handicap,
                                                    ImageSize = a.ImageSize,
                                                    Avata = a.ImageThumbnailPath,
                                                    Status = a.ProcessStatus,
                                                    School = (ct != null ? ct.SchoolName : a.SchoolOtherName) + (afv != null ? " | Xóm/Village: " + afv.Name : ""),
                                                }).FirstOrDefault();
                            if (childProfile != null)
                            {
                                listData.Add(childProfile);
                            }
                        }
                    }
                }
                #region List danh sach
                //List<ChildProfileMobileSearchResult> listData = (from a in db.ChildProfiles.AsNoTracking()
                //                                                 where childProfileDownloadSearch.ListChildProfileId.Contains(a.Id) && a.IsDelete.Equals(Constants.IsUse)
                //                                                 join s in db.Schools.AsNoTracking() on a.SchoolId equals s.Id into ctm
                //                                                 from ct in ctm.DefaultIfEmpty()
                //                                                 join v in db.Villages.AsNoTracking() on a.Address equals v.Id into avm
                //                                                 from afv in avm.DefaultIfEmpty()
                //                                                 select new ChildProfileMobileSearchResult()
                //                                                 {
                //                                                     Id = a.Id,
                //                                                     InfoDate = a.InfoDate,
                //                                                     EmployeeName = a.EmployeeName,
                //                                                     ProgramCode = a.ProgramCode,
                //                                                     ProvinceId = a.ProvinceId,
                //                                                     DistrictId = a.DistrictId,
                //                                                     WardId = a.WardId,
                //                                                     Address = a.Address,
                //                                                     FullAddress = a.FullAddress,
                //                                                     ChildCode = a.ChildCode,
                //                                                     SchoolId = a.SchoolId,
                //                                                     SchoolOtherName = a.SchoolOtherName,
                //                                                     EthnicId = a.EthnicId,
                //                                                     ReligionId = a.ReligionId,
                //                                                     Name = a.Name,
                //                                                     NickName = a.NickName,
                //                                                     Gender = a.Gender,
                //                                                     DateOfBirth = a.DateOfBirth,
                //                                                     LeaningStatus = a.LeaningStatus,
                //                                                     ClassInfo = a.ClassInfo,
                //                                                     FavouriteSubject = a.FavouriteSubject,
                //                                                     LearningCapacity = a.LearningCapacity,
                //                                                     Housework = a.Housework,
                //                                                     Health = a.Health,
                //                                                     Personality = a.Personality,
                //                                                     Hobby = a.Hobby,
                //                                                     Dream = a.Dream,
                //                                                     FamilyMember = a.FamilyMember,
                //                                                     LivingWithParent = a.LivingWithParent,
                //                                                     NotLivingWithParent = a.NotLivingWithParent,
                //                                                     LivingWithOther = a.LivingWithOther,
                //                                                     LetterWrite = a.LetterWrite,
                //                                                     HouseType = a.HouseType,
                //                                                     HouseRoof = a.HouseRoof,
                //                                                     HouseWall = a.HouseWall,
                //                                                     HouseFloor = a.HouseFloor,
                //                                                     UseElectricity = a.UseElectricity,
                //                                                     SchoolDistance = a.SchoolDistance,
                //                                                     ClinicDistance = a.ClinicDistance,
                //                                                     WaterSourceDistance = a.WaterSourceDistance,
                //                                                     WaterSourceUse = a.WaterSourceUse,
                //                                                     RoadCondition = a.RoadCondition,
                //                                                     IncomeFamily = a.IncomeFamily,
                //                                                     HarvestOutput = a.HarvestOutput,
                //                                                     NumberPet = a.NumberPet,
                //                                                     FamilyType = a.FamilyType,
                //                                                     TotalIncome = a.TotalIncome,
                //                                                     IncomeSources = a.IncomeSources,
                //                                                     IncomeOther = a.IncomeOther,
                //                                                     StoryContent = a.StoryContent,
                //                                                     ImagePath = a.ImagePath,
                //                                                     ImageThumbnailPath = a.ImageThumbnailPath,
                //                                                     AreaApproverId = a.AreaApproverId,
                //                                                     AreaApproverDate = a.AreaApproverDate,
                //                                                     OfficeApproveBy = a.OfficeApproveBy,
                //                                                     OfficeApproveDate = a.OfficeApproveDate,
                //                                                     ProcessStatus = a.ProcessStatus,
                //                                                     IsDelete = a.IsDelete,
                //                                                     CreateBy = a.CreateBy,
                //                                                     CreateDate = a.CreateDate,
                //                                                     UpdateBy = a.UpdateBy,
                //                                                     UpdateDate = a.UpdateDate,
                //                                                     ConsentName = a.ConsentName,
                //                                                     ConsentRelationship = a.ConsentRelationship,
                //                                                     ConsentVillage = a.ConsentVillage,
                //                                                     ConsentWard = a.ConsentWard,
                //                                                     SiblingsJoiningChildFund = a.SiblingsJoiningChildFund,
                //                                                     Malformation = a.Malformation,
                //                                                     Orphan = a.Orphan,
                //                                                     EmployeeTitle = a.EmployeeTitle,
                //                                                     ImageSignaturePath = a.ImageSignaturePath,
                //                                                     ImageSignatureThumbnailPath = a.ImageSignatureThumbnailPath,
                //                                                     SaleforceId = a.SaleforceId,
                //                                                     Handicap = a.Handicap,
                //                                                     ImageSize = a.ImageSize,
                //                                                     Avata = a.ImageThumbnailPath,
                //                                                     Status = a.ProcessStatus,
                //                                                     School = (ct != null ? ct.SchoolName : a.SchoolOtherName) + (afv != null ? " | Xóm/Village: " + afv.Name : ""),
                //                                                 }).ToList();
                #endregion
                searchResult.ListResult = listData;
                return searchResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SaveChangeCode(string Id, string programCode)
        {
            string userId = HttpContext.Current.User.Identity.Name;
            var childProfile = db.ChildProfiles.Find(Id);

            if (childProfile != null)
            {
                childProfile.ProgramCode = programCode;
                childProfile.UpdateBy = userId;
                childProfile.UpdateDate = DateTime.Now;
                db.SaveChanges();

                return childProfile.Id;
            }

            return string.Empty;
        }
    }
}
