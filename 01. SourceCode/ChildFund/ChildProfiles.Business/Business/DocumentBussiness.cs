using ChildProfiles.Business.Business;
using ChildProfiles.Model;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model.Document;
using ChildProfiles.Model.DocumentType;
using Ionic.Zip;
using NTS.Common;
using NTS.Common.Utils;
using NTS.Storage;
using NTS.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace ChildProfiles.Business
{
    public class DocumentBussiness
    {
        private ChildProfileEntities db = new ChildProfileEntities();

        public SearchResultObject<DocumentLibrarySearchResult> SearchDocument(DocumentLibrarySearchCondition searchCondition)
        {
            SearchResultObject<DocumentLibrarySearchResult> searchResult = new SearchResultObject<DocumentLibrarySearchResult>();
            try
            {
                var listmodel = (from a in db.DocumentLibraries.AsNoTracking()
                                 join b in db.Users.AsNoTracking() on a.UploadBy equals b.Id into ab
                                 from abv in ab.DefaultIfEmpty()
                                 select new DocumentLibrarySearchResult()
                                 {
                                     Id = a.Id,
                                     DocumentTyeid = a.DocumentTyeId,
                                     Name = a.Name,
                                     FileName = a.FileName,
                                     Path = a.Path,
                                     Size = a.Size,
                                     Extension = a.Extension,
                                     Description = a.Description,
                                     IsDisplay = a.IsDisplay,
                                     UploadBy = abv != null ? abv.Name : "",
                                     UploadDate = a.UploadDate,
                                     UpdateBy = a.UpdateBy,
                                     UpdateDate = a.UpdateDate
                                 }).AsQueryable();

                if (!string.IsNullOrEmpty(searchCondition.UploadDateFrom))
                {
                    var dateFrom = DateTimeUtils.ConvertDateFromStr(searchCondition.UploadDateFrom);
                    listmodel = listmodel.Where(r => r.UploadDate >= dateFrom);
                }
                if (!string.IsNullOrEmpty(searchCondition.UploadDateTo))
                {
                    var dateTo = DateTimeUtils.ConvertDateToStr(searchCondition.UploadDateTo);
                    listmodel = listmodel.Where(r => r.UploadDate <= dateTo);
                }

                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()));
                }

                if (searchCondition.IsDisplay != null)
                {
                    listmodel = listmodel.Where(r => r.IsDisplay == searchCondition.IsDisplay);
                }

                if (searchCondition.DocumentTyeid != null)
                {
                    listmodel = listmodel.Where(r => r.DocumentTyeid == searchCondition.DocumentTyeid);
                }

                if (searchCondition.Description != null)
                {
                    listmodel = listmodel.Where(r => r.Description.Contains(searchCondition.Description));
                }

                if (searchCondition.UploadBy != null)
                {
                    listmodel = listmodel.Where(r => r.UploadBy == searchCondition.UploadBy);
                }



                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();

                List<ComboboxResult> listDocumentType = GetListDocumentTye();
                foreach (var item in searchResult.ListResult)
                {
                    foreach (var itemDocumentTye in listDocumentType)
                    {
                        if (item.DocumentTyeid.Equals(itemDocumentTye.Id))
                        {
                            item.DocumentTypeidView = itemDocumentTye.Name;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("DocumentBussiness.SearchDocument", ex.Message, searchCondition);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }

        public List<ComboboxResult> GetListDocumentTye()
        {
            ComboboxDA comboboxBusiness = new ComboboxDA();
            List<ComboboxResult> listDocumentTye = comboboxBusiness.GetDocumentTyeCBB();
            return listDocumentTye;
        }

        public void DeleteDocumentLibrary(DocumentLibraryModel model)
        {
            using (
                    var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var data = db.DocumentLibraries.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (data != null)
                    {
                        db.DocumentLibraries.Remove(data);
                    }
                    else
                    {
                        throw new Exception("Tài liệu này đã bị xóa bởi người dùng khác");
                    }

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("DocumentBussiness.DeleteDocumentLibrary", ex.Message, model);
                    trans.Rollback();
                    throw new Exception(ErrorMessage.ERR001, ex.InnerException);
                }
            }
        }

        public void CreateDocumentLibrary(DocumentLibraryModel documentLibraryModel, HttpFileCollection httpFile)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    //bảng DocumentLibrary
                    var model = new DocumentLibrary();
                    model.Id = Guid.NewGuid().ToString();
                    model.Name = documentLibraryModel.Name;
                    model.DocumentTyeId = documentLibraryModel.DocumentTyeid;
                    model.Description = documentLibraryModel.Description;
                    //model.Path = documentLibraryModel.Path;
                    model.Size = documentLibraryModel.Size;
                    model.Extension = documentLibraryModel.Extension;
                    model.IsDisplay = documentLibraryModel.IsDisplay;

                    model.UploadBy = documentLibraryModel.UploadBy;
                    model.UploadDate = dateNow;
                    model.UpdateBy = documentLibraryModel.UpdateBy;
                    model.UpdateDate = dateNow;
                    //db.DocumentLibraries.Add(model);

                    //Upload file lên cloud
                    if (httpFile.Count > 0)
                    {
                        List<string> listFileKey = httpFile.AllKeys.ToList();

                        for (int i = 0; i < httpFile.Count; i++)
                        {
                            model.FileName = httpFile[i].FileName;
                            model.Path = Task.Run(async () =>
                            {
                                return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[i], httpFile[i].FileName, Constants.FolderReportProfile);
                            }).Result;
                            db.DocumentLibraries.Add(model);
                        }
                    }

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("DocumentBussiness.CreateDocumentLibrary", ex.Message, documentLibraryModel);
                    trans.Rollback();
                    throw new Exception(ErrorMessage.ERR001, ex.InnerException);
                }
            }
        }

        public DocumentLibraryModel GetDocumentLibById(string id)
        {
            var item = db.DocumentLibraries.Find(id);
            if (item == null)
            {
                throw new Exception("Tài liệu này đã bị xóa bởi người dùng khác");
            }
            try
            {
                DocumentLibraryModel result = new DocumentLibraryModel()
                {
                    Id = item.Id,
                    DocumentTyeid = item.DocumentTyeId,
                    Name = item.Name,
                    Path = item.Path,
                    Size = item.Size,
                    Extension = item.Extension,
                    Description = item.Description,
                    IsDisplay = item.IsDisplay,
                    UploadBy = item.UploadBy,
                    UploadDate = item.UploadDate,
                    UpdateBy = item.UpdateBy,
                    UpdateDate = item.UpdateDate,
                    FileName = item.FileName,
                };
                return result;
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("DocumentBussiness.GetDocumentLibById", ex.Message, id);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }

        public void UpdateDocument(DocumentLibraryModel documentLibraryModel, HttpFileCollection httpFile)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                string fileDelete = "";
                try
                {
                    var dateNow = DateTime.Now;
                    var model = db.DocumentLibraries.FirstOrDefault(u => u.Id.Equals(documentLibraryModel.Id));
                    if (model == null)
                    {
                        throw new Exception("Tài liệu này đã bị xóa bởi người dùng khác");
                    }
                    //Update GroupUser
                    model.Name = documentLibraryModel.Name;
                    model.DocumentTyeId = documentLibraryModel.DocumentTyeid;
                    model.Size = documentLibraryModel.Size;
                    model.Extension = documentLibraryModel.Extension;
                    model.Description = documentLibraryModel.Description;
                    model.IsDisplay = documentLibraryModel.IsDisplay;
                    model.UpdateDate = dateNow;
                    model.UpdateBy = documentLibraryModel.UpdateBy;

                    //Upload file lên cloud
                    if (httpFile.Count > 0)
                    {
                        List<string> listFileKey = httpFile.AllKeys.ToList();

                        fileDelete = model.Path;
                        for (int i = 0; i < httpFile.Count; i++)
                        {
                            model.FileName = httpFile[i].FileName;
                            model.Path = Task.Run(async () =>
                            {
                                return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[i], httpFile[i].FileName, Constants.FolderReportProfile);
                            }).Result;
                        }
                    }
                    db.SaveChanges();
                    trans.Commit();

                    if (!string.IsNullOrEmpty(fileDelete))
                    {
                        AzureStorageUploadFiles.GetInstance().DeleteFileAsync(model.Path);
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("DocumentBussiness.UpdateDocument", ex.Message, documentLibraryModel);
                    trans.Rollback();
                    throw new Exception(ErrorMessage.ERR001, ex.InnerException);
                }
            }
        }

        public string DownloadFile(string id)
        {
            var data = db.DocumentLibraries.FirstOrDefault(u => u.Id.Equals(id));
            if (data == null)
            {
                throw new Exception("File đã bị xóa bởi người dùng khác");
            }
            List<DocumentLibrarySearchResult> list = new List<DocumentLibrarySearchResult>();
            list.Add(new DocumentLibrarySearchResult { Name = data.FileName, Path = data.Path });
            string pathFile = "";
            try
            {
                string folder = "~/fileUpload/DocumentLibrary/" + DateTime.Now.ToString("dd-MM-yyyy/");
                string fileReturn = "~/fileUpload/DocumentLibrary/" + DateTime.Now.ToString("dd-MM-yyyy");
                if (DownLoadFileToServer(list, folder))
                {
                    pathFile = ZipFile(folder, fileReturn, "Images");
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("DocumentBussiness.DownloadFile", ex.Message, id);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return pathFile;
        }

        public bool DownLoadFileToServer(List<DocumentLibrarySearchResult> urls, string pathServer)
        {

            try
            {
                var path = HostingEnvironment.MapPath(pathServer);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                else
                {
                    foreach (FileInfo file in (new DirectoryInfo(path)).GetFiles())
                        file.Delete();
                    foreach (DirectoryInfo file in (new DirectoryInfo(path)).GetDirectories())
                        file.Delete(true);
                }
                var task = Parallel.ForEach(urls,
                             s =>
                             {
                                 using (WebClient wc = new WebClient())
                                 {
                                     wc.DownloadFile(new Uri(s.Path), path + s.Name);
                                 }
                             });
                return task.IsCompleted;
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("DocumentBussiness.DownLoadFileToServer", ex.Message, pathServer);
                throw ex;
            }
        }
        public string ZipFile(string folder, string pathZip, string forderName)
        {
            var tempError = string.Empty;
            var path = HostingEnvironment.MapPath(folder);
            if (!Directory.Exists(path))
            {
                throw new Exception("Thư mục không tồn tại hoặc đã bị xóa");
            }
            var files = Directory.GetFiles(path);
            var filezip = HostingEnvironment.MapPath(pathZip);
            if (files.Count() == 0)
            {
                throw new Exception("File đã bị xóa bởi người dùng khác");
            }
            try
            {
                using (var zip = new ZipFile())
                {
                    zip.AddFiles(files, forderName);
                    zip.Save(filezip + ".zip");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pathZip.Remove(0, 1) + ".zip";
        }


        //Bảng DocumentType
        public SearchResultObject<DocumentTypeSearchResult> SearchDocumentType(DocumentTypeSearchCondition searchCondition)
        {
            SearchResultObject<DocumentTypeSearchResult> searchResult = new SearchResultObject<DocumentTypeSearchResult>();
            try
            {
                var listmodel = (from a in db.DocumentTyes.AsNoTracking()
                                 orderby a.Name
                                 select new DocumentTypeSearchResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Description = a.Description,
                                     IsDisplay = a.IsDisplay,
                                     CreateBy = a.CreateBy,
                                     CreateDate = a.CreateDate,
                                     UpdateBy = a.UpdateBy,
                                     UpdateDate = a.UpdateDate
                                 }).AsQueryable();


                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();

            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("DocumentBussiness.SearchDocumentType", ex.Message, searchCondition);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }

        public void DeleteDocumentType(DocumentTypeModel model)
        {
            //Kiểm tra nhóm tài liệu có tài liệu bên trong không
            var documentLibrary = db.DocumentLibraries.FirstOrDefault(a => a.DocumentTyeId.Equals(model.Id));
            if (documentLibrary != null)
            {
                throw new Exception("Không được xóa nhóm tài liệu này!");
            }
            using (
                    var trans = db.Database.BeginTransaction())
            {

                try
                {
                    var data = db.DocumentTyes.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (data != null)
                    {
                        db.DocumentTyes.Remove(data);
                    }
                    else
                    {
                        throw new Exception("Nhóm tài liệu đã bị xóa bởi người dùng khác");
                    }

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("DocumentBussiness.DeleteDocumentType", ex.Message, model);
                    trans.Rollback();
                    throw new Exception(ErrorMessage.ERR001, ex.InnerException);
                }
            }
        }

        public void CreateDocumentType(DocumentTypeModel documentTypeModel)
        {
            var checkDocumnetType = db.DocumentTyes.FirstOrDefault(a => a.Name.Equals(documentTypeModel.Name));
            if (checkDocumnetType != null)
            {
                throw new Exception("Tên nhóm tài liệu này đã tồn tại!");
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    //bảng DocumentType
                    var model = new DocumentTye();
                    model.Id = Guid.NewGuid().ToString();
                    model.Name = documentTypeModel.Name;                  
                    model.Description = documentTypeModel.Description;                 
                    model.IsDisplay = documentTypeModel.IsDisplay;

                    model.CreateBy = documentTypeModel.CreateBy;
                    model.CreateDate = dateNow;
                    model.UpdateBy = documentTypeModel.UpdateBy;
                    model.UpdateDate = dateNow;

                    db.DocumentTyes.Add(model);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("DocumentBussiness.CreateDocumentType", ex.Message, documentTypeModel);
                    trans.Rollback();
                    throw new Exception(ErrorMessage.ERR001, ex.InnerException);
                }
            }
        }

        public void UpdateDocumentType(DocumentTypeModel documentTypeModel)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var model = db.DocumentTyes.FirstOrDefault(u => u.Id.Equals(documentTypeModel.Id));
                var checkDocumentType = db.DocumentTyes.FirstOrDefault(a => a.Name.Equals(documentTypeModel.Name));
                if (checkDocumentType != null)
                {
                    throw new Exception("Tên nhóm tài liệu này đã tồn tại!");
                }
                if (model == null)
                {
                    throw new Exception("Nhóm tài liệu này đã bị xóa bởi người dùng khác!");
                }
                
                try
                {
                    var dateNow = DateTime.Now;
                    //Update DocumentType
                    model.Name = documentTypeModel.Name;   
                    model.Description = documentTypeModel.Description;
                    model.IsDisplay = documentTypeModel.IsDisplay;
                    model.UpdateDate = dateNow;
                    model.UpdateBy = documentTypeModel.UpdateBy;

                    db.SaveChanges();
                    trans.Commit();
         
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("DocumentBussiness.UpdateDocumentType", ex.Message, documentTypeModel);
                    trans.Rollback();
                    throw new Exception(ErrorMessage.ERR001, ex.InnerException);
                }
            }
        }


    }
}
