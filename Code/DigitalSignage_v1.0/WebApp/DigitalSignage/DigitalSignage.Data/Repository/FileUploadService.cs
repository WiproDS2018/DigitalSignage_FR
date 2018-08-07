
using DigitalSignage.Domain;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DigitalSignage.Data
{


    public class FileUploadService
    {
        public static string StroageConnectionString;
        public string StorageAccount;
        public string StorageKey;
        public string Signageimagecontainer;
        public string Signagehtmlcontainer;
        public string Signagetemplatecontainer;

        public FileUploadService()
        {
            StroageConnectionString = ConfigurationManager.AppSettings["StroageConnection"].ToString();
            Signageimagecontainer = ConfigurationManager.AppSettings["Signageimagecontainer"].ToString();
            Signagehtmlcontainer = ConfigurationManager.AppSettings["Signagehtmlcontainer"].ToString();
            Signagetemplatecontainer = ConfigurationManager.AppSettings["Signagetemplatecontainer"].ToString();
        }

        public string UploadImage(HttpPostedFileBase imageToUpload, string sceneName, string sceneType)
        {
            //public async Task<string> UploadImageAsync(HttpPostedFileBase imageToUpload)
            //StroageConnectionString = ConfigurationManager.ConnectionStrings["StorageConnection"].ToString();
            // string Path = "";

            string imageFullPath = null;
            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return null;
            }
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(StroageConnectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                //Signageimagecontainer = ConfigurationManager.AppSettings["Signageimagecontainer"].ToString();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(Signageimagecontainer);

                if (cloudBlobContainer.CreateIfNotExists())
                {
                    cloudBlobContainer.SetPermissions(
                       new BlobContainerPermissions
                       {
                           PublicAccess = BlobContainerPublicAccessType.Blob
                       }
                       );
                }

                string imageName = sceneName;
                imageName = imageName.Replace(" ", "");

                if (sceneType == SignageConstants.IMAGEUPLOAD)
                {
                    imageName = imageName + ".png";
                }

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                cloudBlockBlob.UploadFromStream(imageToUpload.InputStream);
              
                imageFullPath = " " + cloudBlockBlob.Uri.ToString() + " ";

              
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return imageFullPath;
        }

        public string UploadHTMLContent(SceneViewModel scene)
        {           
            string blobUri = null;

            if (scene.imgString == null || scene.imgString.Length == 0)
            {
                return null;
            }
            try
            {             

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(StroageConnectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(Signagehtmlcontainer);
                if (cloudBlobContainer.CreateIfNotExists())
                {
                    cloudBlobContainer.SetPermissions(
                       new BlobContainerPermissions
                       {
                           PublicAccess = BlobContainerPublicAccessType.Blob
                       }
                       );
                }
              
                string htmlName = scene.SceneName;
                htmlName= htmlName.Replace(" ","");
                htmlName = htmlName + ".png";

                CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(htmlName);
                blob.Properties.ContentType = "file";

                /////////////
                byte[] bytes = Convert.FromBase64String(scene.imgString);
                Stream stream = new MemoryStream(bytes);
                blob.UploadFromStream(stream);
                //////////////////////////             

                //blob.UploadText(scene.SceneContent);              

                blobUri = blob.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return blobUri;

        }

        public string DeleteImageFromBlob(SceneViewModel scene)
        {
            string message = "Success";
            string contianer = "";
            string blobImage = "";

            if (scene.SceneUrl == null || scene.SceneUrl.Trim() == "")
            {
                return null;
            }
            if (scene.SceneType.Trim() == "template")
            {
                contianer = Signagehtmlcontainer;
            }
            else
            {
                contianer = Signageimagecontainer;
            }

            string[] sceneName = scene.SceneUrl.Split('/');
            if (sceneName.Length>4)
            {
                blobImage = sceneName[4].Trim(); 
            }

            try
            {
               
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(StroageConnectionString);
                CloudBlobClient _blobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer _cloudBlobContainer = _blobClient.GetContainerReference(contianer);
                CloudBlockBlob _blockBlob = _cloudBlobContainer.GetBlockBlobReference(blobImage);
                //delete blob from container    
                _blockBlob.Delete();

            }
            catch (Exception ex)
            {
                message = "Error";
                throw ex;
            }
            return message;
        }

        public string UploadTemplateImage(HttpPostedFileBase imageToUpload, string ImageName)
        {
            //public async Task<string> UploadImageAsync(HttpPostedFileBase imageToUpload)
            //StroageConnectionString = ConfigurationManager.ConnectionStrings["StorageConnection"].ToString();
            // string Path = "";

            string imageFullPath = null;
            if (imageToUpload == null || imageToUpload.ContentLength == 0)
            {
                return null;
            }
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(StroageConnectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(Signagetemplatecontainer);

                if (cloudBlobContainer.CreateIfNotExists())
                {
                    cloudBlobContainer.SetPermissions(
                       new BlobContainerPermissions
                       {
                           PublicAccess = BlobContainerPublicAccessType.Blob
                       }
                       );
                }

                string imageName = ImageName;
                imageName = imageName.Replace(" ", "");
                imageName = imageName + ".png";

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
                cloudBlockBlob.UploadFromStream(imageToUpload.InputStream);

                imageFullPath = " " + cloudBlockBlob.Uri.ToString() + " ";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return imageFullPath;
        }

        public List<string> GetTemplateImages()
        {

            //var accnt = CloudStorageAccount.parse("");//ur accnt string
            //var client = accnt.CreateCloudBlobClient();
            //var blobs = client.GetContainerRefrence(""/*container name*/).ListBlobs();

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(StroageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(Signagetemplatecontainer).ListBlobs();

            var urls = new List<string>();
            foreach (var blob in cloudBlobContainer)
            {
                string url = blob.Uri.AbsoluteUri;
                urls.Add(url);
            }

            return urls;
        }

        //public Image LoadImage()
        //{
        //    signagetemplatecontainer
        //    //data:image/gif;base64,
        //    //this image is a single pixel (black)
        //    byte[] bytes = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw==");

        //    Image image;
        //    using (MemoryStream ms = new MemoryStream(bytes))
        //    {
        //        image = Image.FromStream(ms);
        //    }

        //    return image;
        //}


    }
}
