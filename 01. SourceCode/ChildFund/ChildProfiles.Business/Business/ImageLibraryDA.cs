using ChildProfiles.Model;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model.Model.FliesLibrary;
using Ionic.Zip;
using NTS.Common;
using NTS.Common.Utils;
using NTS.Storage;
using NTS.Utils;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace ChildProfiles.Business.Business
{
    public class ImageLibraryDA
    {
        private ChildProfileEntities db = new ChildProfileEntities();

        public SearchResultObject<ShareImageModel> GetListUpload(UploadImageSearchModel modelSearch)
        {
            SearchResultObject<ShareImageModel> model = new SearchResultObject<ShareImageModel>();
            try
            {
                var listmodel = (from a in db.ShareImages.AsNoTracking()
                                 where !a.IsDelete
                                 join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                                 select new ShareImageModel
                                 {
                                     Id = a.Id,
                                     UserId = a.UserId,
                                     UploadDate = a.UploadDate,
                                     CreateDate = a.CreateDate,
                                     Content = a.Content,
                                     CreateBy = b.Name,
                                     ImageNumber = a.AttachmentImages.Count()
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.Description))
                {
                    listmodel = listmodel.Where(r => r.Content.ToLower().Contains(modelSearch.Description.ToLower()));
                }
                if (!string.IsNullOrEmpty(modelSearch.CreateBy))
                {
                    listmodel = listmodel.Where(r => r.CreateBy.ToLower().Contains(modelSearch.CreateBy.ToLower()));
                }
                if (modelSearch.DateFrom != null)
                {
                    modelSearch.DateFrom = DateTimeUtils.ConvertDateFrom(modelSearch.DateFrom);
                    listmodel = listmodel.Where(r => r.CreateDate >= modelSearch.DateFrom);
                }
                if (modelSearch.DateTo != null)
                {
                    modelSearch.DateTo = DateTimeUtils.ConvertDateTo(modelSearch.DateTo);
                    listmodel = listmodel.Where(r => r.CreateDate <= modelSearch.DateTo);
                }
                model.TotalItem = listmodel.Select(u => u.Id).Count();
                model.ListResult = listmodel.OrderByDescending(x => x.UploadDate).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ImageLibraryDA.GetListUpload", ex.Message, modelSearch);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return model;
        }

        public void UploadImage(HttpFileCollection httpFile, ShareImageModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    ShareImage upload = new ShareImage
                    {
                        Id = Guid.NewGuid().ToString(),
                        Content = model.Content,
                        CreateBy = model.UserId,
                        CreateDate = DateTime.Now,
                        UploadDate = DateTime.Now,
                        UserId = model.UserId
                    };

                    if (httpFile.Count > 0)
                    {
                        for (int i = 0; i < httpFile.Count; i++)
                        {
                            var item = httpFile[i];
                            ImageResult imageResult = Task.Run(async () =>
                            {
                                return await AzureStorageUploadFiles.GetInstance().UploadImageAsync(item, Constants.FolderImageChildProfile);
                            }).Result;
                            var file = new AttachmentImage
                            {
                                ImagePath = imageResult.ImageOrigin,
                                ImageThumbnailPath = imageResult.ImageThumbnail,
                                Name = item.FileName,
                                Id = Guid.NewGuid().ToString(),
                                SizeBase = item.ContentLength,
                                UploadDate = DateTime.Now,
                                UploadBy = model.UserId,
                                FileType = item.ContentType.Contains("image") ? Constants.ImageType : (item.ContentType.Contains("application") ? Constants.DocumentType : Constants.OthorFileType)
                            };

                            upload.AttachmentImages.Add(file);
                        }
                    }

                    db.ShareImages.Add(upload);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("ImageLibraryDA.UploadImage", ex.Message, model);
                    trans.Rollback();
                    throw ex;
                }
            }
        }


        private Stream ConvertThumb(Stream photoToUpload)
        {
            Stream Output = new MemoryStream();
            try
            {
                Image image = Image.FromStream(photoToUpload);
                int imgHeight = 192;
                int imgWidth = 272;
                if (image.Width < image.Height)
                {
                    //portrait image  
                    imgHeight = 100;
                    var imgRatio = (float)imgHeight / (float)image.Height;
                    imgWidth = Convert.ToInt32(image.Height * imgRatio);
                }
                else if (image.Height < image.Width)
                {
                    //landscape image  
                    imgWidth = 100;
                    var imgRatio = (float)imgWidth / (float)image.Width;
                    imgHeight = Convert.ToInt32(image.Height * imgRatio);
                }
                Image thumb = image.GetThumbnailImage(imgWidth, imgHeight, () => false, IntPtr.Zero);

                //thumb.Save(HostingEnvironment.MapPath("~/")+ "thumb.jpg");

                thumb.Save(Output, ImageFormat.Jpeg);


                //using (FileStream fileStream = new FileStream(HostingEnvironment.MapPath("~/") + "thumb.jpg", FileMode.Create, FileAccess.Write))
                //{
                //    Output.Seek(0, SeekOrigin.Begin);
                //    Output.CopyTo(fileStream);
                //}


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Output;
        }


        public ShareImageModel GetInfoFileAttachImage(string id)
        {
            ShareImageModel model = new ShareImageModel();
            try
            {
                var folderImage = db.ShareImages.FirstOrDefault(e => e.Id.Equals(id) && !e.IsDelete);
                if (folderImage != null)
                {
                    model.Content = folderImage.Content;
                    model.Id = folderImage.Id;
                    model.UpdateDate = folderImage.UpdateDate;
                    model.Files = folderImage.AttachmentImages.Where(u => u.IsDelete == Constants.IsUse).Select(x => new AttachmentImageModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ImagePath = x.ImagePath,
                        ImageThumbnailPath = x.ImageThumbnailPath
                    }).ToList();
                }
                else
                {
                    throw new Exception("Mục file không tồn tại");
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ImageLibraryDA.GetInfoFileAttachImage", ex.Message, id);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return model;
        }

        public void DeleteImage(ImageActionModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var tem = db.ShareImages.FirstOrDefault(e => e.Id == model.UserUploadId);
                    if (tem != null)
                    {
                        foreach (var item in tem.AttachmentImages.Where(e => model.ImageId.Contains(e.Id)))
                        {
                            item.IsDelete = true;
                        }
                        db.SaveChanges();
                        trans.Commit();
                    }
                    else
                    {
                        throw new Exception("Mục file tồn tại");
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("ImageLibraryDA.DeleteImage", ex.Message, model);
                    trans.Rollback();
                    throw ex;
                }
            }
        }
        public void DeleteItemUpload(string id)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var tem = db.ShareImages.FirstOrDefault(e => e.Id == id);
                    if (tem != null)
                    {
                        tem.IsDelete = true;
                        foreach (var item in tem.AttachmentImages)
                        {
                            item.IsDelete = true;
                        }
                        db.SaveChanges();
                        trans.Commit();
                    }
                    else
                    {
                        throw new Exception("Mục file tồn tại");
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("ImageLibraryDA.DeleteItemUpload", ex.Message, id);
                    trans.Rollback();
                    throw ex;
                }
            }
        }


        public string DownloadAllImage(string id)
        {
            string path = string.Empty;
            try
            {
                var tem = db.ShareImages.FirstOrDefault(e => e.Id == id);
                if (tem != null)
                {
                    var urls = tem.AttachmentImages.Select(e => new AttachmentImageModel
                    {
                        ImagePath = e.ImagePath,
                        Name = e.Name
                    }).ToList();
                    string folder = "~/fileUpload/FileUser/" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss/");
                    string fileReturn = "~/fileUpload/FileUser/" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
                    if (DownLoadFileToServer(urls, folder, true))
                    {
                        path = ZipFile(folder, fileReturn, "Images");
                    }
                }
                else
                {
                    throw new Exception("Mục file tồn tại");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return path;
        }

        public string DownloadImage(string id, List<string> ids)
        {
            string path = string.Empty;
            try
            {
                var tem = db.ShareImages.FirstOrDefault(e => e.Id == id);
                if (tem != null)
                {
                    var urls = tem.AttachmentImages.Where(u => ids.Contains(u.Id)).Select(e => new AttachmentImageModel
                    {
                        ImagePath = e.ImagePath,
                        Name = e.Name
                    }).ToList();
                    string folder = "~/fileUpload/FileUser/" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss/");
                    string fileReturn = "~/fileUpload/FileUser/" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
                    if (DownLoadFileToServer(urls, folder, true))
                    {
                        path = ZipFile(folder, fileReturn, "Images");
                    }
                }
                else
                {
                    throw new Exception("Mục file tồn tại");
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ImageLibraryDA.DownloadImage", ex.Message, id);
                throw ex;
            }
            return path;
        }

        public bool DownLoadImgProfileToServer(List<AttachmentImageModel> urls, string pathServer, bool is213, bool isImageSignature)
        {
            urls = urls.Where(u => !string.IsNullOrEmpty(u.ImagePath)).ToList();
            for (int i = 0; i < urls.Count; i++)
            {
                if (is213)
                {
                    urls[i].Name = "213" + urls[i].Code.Replace("\n","") + ".jpg";
                }
                else
                {
                    if (!isImageSignature)
                    {
                        urls[i].Name = "199-" + urls[i].Code.Replace("\n", "") + "-" + urls[i].Name.Trim().Replace(" ", "-") + ".jpg";
                    }
                    else
                    {
                        urls[i].Name = urls[i].Code.Replace("\n", "") + ".jpg";
                    }

                }

            }
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
                                     wc.DownloadFile(new Uri(s.ImagePath), path + s.Name);
                                 }
                             });
                return task.IsCompleted;
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ImageLibraryDA.DownLoadImgProfileToServer", ex.Message, pathServer);
                throw ex;
            }
        }
        public bool DownLoadFileToServer(List<AttachmentImageModel> urls, string pathServer, bool isIndex)
        {
            for (int i = 0; i < urls.Count; i++)
            {
                if (isIndex)
                {
                    urls[i].Name = (i + 1) + "," + urls[i].Name.Replace("\n", "");
                }
                else
                {
                    urls[i].Name = urls[i].Name.Replace("\n", "");
                }
            }
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
                                       wc.DownloadFile(new Uri(s.ImagePath), path + s.Name);
                                   }
                               });
                return task.IsCompleted;
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ImageLibraryDA.DownLoadFileToServer", ex.Message, pathServer);
                throw ex;
            }
        }

        public bool DownLoadSingleFileToServer(AttachmentImageModel urls, string pathServer)
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

                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(new Uri(urls.ImagePath), path + urls.Name);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ImageLibraryDA.DownLoadSingleFileToServer", ex.Message, pathServer);
                throw ex;
            }
        }

        public bool DownLoadFileTemplateToServer(string filename, string pathServer)
        {
            try
            {
                var path = HttpContext.Current.Server.MapPath(pathServer);
                WebClient wc = new WebClient();
                wc.DownloadFile(path, filename);
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ImageLibraryDA.DownLoadFileTemplateToServer", ex.Message, pathServer);
                throw ex;
            }
        }

        /// <summary>
        /// Zip tất cả các file
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// 
        public string ZipFileForder(string folder, string pathZip, string forderName)
        {
            var tempError = string.Empty;
            var path = HostingEnvironment.MapPath(folder);
            if (!Directory.Exists(path))
            {
                throw new Exception("Thư mục không tồn tại hoặc đã bị xóa");
            }
            try
            {
                using (var zip = new ZipFile())
                {
                    zip.AddDirectory(HostingEnvironment.MapPath(folder));
                    var rspath = HostingEnvironment.MapPath(pathZip + forderName) + ".zip";
                    zip.Save(rspath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return pathZip.Remove(0, 1) + forderName + ".zip";
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
                throw new Exception("File không tồn tại hoặc bị xóa");
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

        public string DownImageChild(List<string> ids)
        {
            string path = string.Empty;
            try
            {
                var listFile = (from a in db.ImageChildByYears.AsNoTracking()
                                join b in db.ChildProfiles.AsNoTracking()
                                on a.ChildProfileId equals b.Id
                                where ids.Contains(a.Id)
                                select new AttachmentImageModel
                                {
                                    ImagePath = a.ImageUrl,
                                    Code = b.ChildCode.ToUpper(),
                                    Name = b.Name.ToUpper(),
                                    CreateDate = a.CreateDate
                                }).ToList();
                if (listFile.Count > 0)
                {
                    foreach (var item in listFile)
                    {
                        item.Name = item.Code + "_" + Common.ConvertNameToTag(item.Name) + ".jpg";
                    }
                    string folder = "~/fileUpload/FileUser/" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss/");
                    string fileReturn = "~/fileUpload/FileUser/" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
                    if (DownLoadFileToServer(listFile, folder, false))
                    {
                        path = ZipFile(folder, fileReturn, "Images_" + listFile[0].CreateDate.Value.Year);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return path;
        }

        public List<Province> GetListP(List<string> lstId)
        {
            try
            {
                var list = db.Provinces.Where(u => lstId.Contains(u.Id)).ToList();
                return list;
            }
            catch (Exception)
            {

                return new List<Province>();
            }
        }
        public List<District> GetListD(List<string> lstId)
        {
            try
            {
                var list = db.Districts.Where(u => lstId.Contains(u.Id)).ToList();
                return list;
            }
            catch (Exception)
            {

                return new List<District>();
            }
        }
        public List<Ward> GetListW(List<string> lstId)
        {
            try
            {
                var list = db.Wards.Where(u => lstId.Contains(u.Id)).ToList();
                return list;
            }
            catch (Exception)
            {

                return new List<Ward>();
            }
        }

        public string DownTemplate()
        {
            string pathTemplate = "/Template/ImportChildProfile.xlsx";
            string path = "/Template/Export/Data/ImportChildProfile.xlsx";
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(HttpContext.Current.Server.MapPath(pathTemplate));
            workbook.SaveAs(HttpContext.Current.Server.MapPath(path));
            return path;
        }
    }
}
