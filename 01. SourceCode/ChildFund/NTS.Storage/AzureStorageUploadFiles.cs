using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NTS.Storage
{
    public class AzureStorageUploadFiles
    {

        private static AzureStorageUploadFiles singletonObject;

        private static CloudStorageAccount storageAccount;
        private static string _container;
        private static string _urlHosting;

        public static AzureStorageUploadFiles GetInstance()
        {
            if (singletonObject == null)
            {
                singletonObject = new AzureStorageUploadFiles();
                storageAccount = AzureStorageUtils.StorageAccount;
                _container = ConfigurationManager.AppSettings["StorageContainer"];
                _urlHosting = ConfigurationManager.AppSettings["UrlHostImage"];
            }

            return singletonObject;
        }

        public StorageResult UploadFile(byte[] fileBinary, string contentType, string fileName)
        {
            StorageResult resultObject = new StorageResult();
            if (fileBinary == null || fileBinary.Length == 0)
            {
                return null;
            }

            try
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer container = blobClient.GetContainerReference(_container);

                if (container.CreateIfNotExists())
                {
                    // Enable public access on the newly created "images" container
                    container.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess =
                                BlobContainerPublicAccessType.Blob
                        });
                }

                using (var stream = new MemoryStream(fileBinary, writable: false))
                {
                    // Upload image to Blob Storage
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
                    blockBlob.Properties.ContentType = contentType;

                    blockBlob.UploadFromStream(stream);

                    resultObject.FileName = fileName;
                    resultObject.Folder = _container;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return resultObject;
        }

        public async Task<string> UploadFileStreamAsync(string contentType, Stream stream, string fileName, string folder)
        {
            if (stream == null || stream.Length == 0)
            {
                return null;
            }

            string fullPath = null;

            try
            {
                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_container);

                // Create a unique name for the images we are about to upload
                string imageName = folder + "/" + String.Format("{0}{1}",
                    Guid.NewGuid().ToString(),
                    Path.GetExtension(fileName));

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = contentType;
                await blockBlob.UploadFromStreamAsync(stream);
                // Convert to be HTTP based URI (default storage path is HTTPS)
                fullPath = _urlHosting + _container + "/" + imageName;
            }
            catch (Exception ex)
            {
            }

            return fullPath;
        }

        public async Task<string> UploadFileAsync(HttpPostedFile fileToUpload, string fileName, string folder)
        {
            if (fileToUpload == null || fileToUpload.ContentLength == 0)
            {
                return null;
            }

            string fullPath = null;

            try
            {
                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_container);

                // Create a unique name for the images we are about to upload
                string imageName = folder + "/" + fileName;

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = fileToUpload.ContentType;
                await blockBlob.UploadFromStreamAsync(fileToUpload.InputStream);

                // Convert to be HTTP based URI (default storage path is HTTPS)
                fullPath = _urlHosting + _container + "/" + imageName;
            }
            catch (Exception ex)
            {
            }

            return fullPath;
        }

        public async Task<ImageResult> UploadImageAsync(HttpPostedFile imageUpload, string folder)
        {
            if (imageUpload == null || imageUpload.ContentLength == 0)
            {
                return null;
            }

            ImageResult imageResult = new ImageResult();

            try
            {
                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_container);

                // Create a unique name for the images we are about to upload
                string imageOrigin = folder + "/" + String.Format("{0}{1}",
                    Guid.NewGuid().ToString(),
                    Path.GetExtension(imageUpload.FileName));

                // Upload image origin Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageOrigin);
                blockBlob.Properties.ContentType = imageUpload.ContentType;
                await blockBlob.UploadFromStreamAsync(imageUpload.InputStream);
                imageResult.ImageOrigin = _urlHosting + _container + "/" + imageOrigin;

                // Upload thumbnail origin Blob Storage
                string imageThumbnail = folder + "/" + String.Format("{0}{1}",
                    Guid.NewGuid().ToString(),
                    "-thumbnail.jpg");
                using (Stream streamThumbnail = new MemoryStream())
                {
                    Bitmap bitmap = ImageUtil.CreateImageThumbnail(imageUpload);
                    if (bitmap != null)
                    {
                        bitmap.Save(streamThumbnail, System.Drawing.Imaging.ImageFormat.Jpeg);
                        streamThumbnail.Position = 0;

                        CloudBlockBlob blockBlobThumbnail = container.GetBlockBlobReference(imageThumbnail);
                        blockBlobThumbnail.Properties.ContentType = imageUpload.ContentType;
                        await blockBlobThumbnail.UploadFromStreamAsync(streamThumbnail);
                        imageResult.ImageThumbnail = _urlHosting + _container + "/" + imageThumbnail;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return imageResult;
        }

        public void UploadThumbnailAsync(Bitmap bitmap, string folder, string fileName)
        {
            if (bitmap == null)
            {
                return;
            }

            try
            {
                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_container);

                using (Stream streamThumbnail = new MemoryStream())
                {
                    bitmap.Save(streamThumbnail, System.Drawing.Imaging.ImageFormat.Jpeg);
                    streamThumbnail.Position = 0;

                    CloudBlockBlob blockBlobThumbnail = container.GetBlockBlobReference(fileName);
                     blockBlobThumbnail.Properties.ContentType = "image/jpeg";
                    blockBlobThumbnail.UploadFromStreamAsync(streamThumbnail).Wait();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<string> UploadFileStreamAsync(Stream file, string folder, string extension)
        {
            if (file == null || file.Length == 0)
            {
                return string.Empty;
            }

            string path = string.Empty;
            string folderSub = DateTime.Now.ToString("ddMMyyyy");
            try
            {
                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_container);

                // Create a unique name for the images we are about to upload
                string imageName = folder + "/" + folderSub + "/" + String.Format("{0}.{1}",
                    Guid.NewGuid().ToString(),
                    extension);

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = String.Format("image/{0}",
                    Guid.NewGuid().ToString(),
                    extension);
                await blockBlob.UploadFromStreamAsync(file);

                // Convert to be HTTP based URI (default storage path is HTTPS)
                path = imageName;
            }
            catch (Exception ex)
            {
            }

            return path;
        }

        public async Task DownloadFileAsync(string fileBlobPath, string fileClientPath)
        {

            try
            {
                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_container);

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileBlobPath);
                Stream temp = new MemoryStream();
                blockBlob.DownloadToStream(temp, null, null, null);
            }
            catch (Exception ex)
            {
            }
        }

        public Stream ReadFileToStream(string filePath)
        {

            try
            {
                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_container);

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filePath);
                return blockBlob.OpenRead();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteFileAsync(string filePath)
        {
            try
            {
                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_container);

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filePath);
                await blockBlob.DeleteIfExistsAsync();

            }
            catch (Exception ex)
            {
            }
        }


        public List<string> GetFileList()
        {
            List<string> fileUris = new List<string>();
            try
            {
                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_container);

                BlobResultSegment resultSegment = container.ListBlobsSegmentedAsync(currentToken: null).Result;

                var directory = container.GetDirectoryReference(@"ImageChild");
                var folders = directory.ListBlobs().ToList();
                foreach (var folder in folders)
                {
                    fileUris.Add(folder.StorageUri.PrimaryUri.ToString());
                }
            }
            catch (Exception ex)
            {
            }

            return fileUris;
        }

        /// <summary>
        /// Check tồn tại file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool ExitFileAsync(string filePath)
        {
            try
            {
                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_container);

                // Upload image to Blob Storage
                CloudBlob blob = container.GetBlobReference(filePath);
                bool tem = blob.Exists();
                return blob.Exists();

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
