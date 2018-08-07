using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SignageFaceRecognition
{
    class DownloadBlob
    {
        /// <summary>
        /// Download published Image and related details provided by scheduler service
        /// </summary>
        /// <param name="url"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public static void DownloadFromURL(CloudMessage cloudMessage)
        {
            try
            {
                string contentType = cloudMessage.ContentType.ToUpper();
                string url = cloudMessage.ContentUrl.Replace("https:", "http:").Trim();
                int index = url.LastIndexOf('/');
                string file = url.Substring(index + 1);
                string path = Path.GetFullPath(Enviornment.ImageStoragePath) + "\\" + file;
                if (contentType == "IMAGE-UPLOAD"
                    || contentType == "IMAGE-TEMPLATE"
                    || contentType == "PPT")
                {
                    using (var client = new WebClient())
                    {
                        if (!File.Exists(@path))
                        {
                            client.DownloadFile(url, @path);
                        }
                        FeedXML.UpdateXMLConfig(path, cloudMessage);
                    }
                }
                else if (contentType == "VIDEO" || contentType == "VIDEOURL")
                {
                    using (var client = new WebClient())
                    {
                        if (!File.Exists(@path))
                        {
                            client.DownloadFile(url, @path + ".mp4");
                        }
                        FeedXML.UpdateXMLConfig(path + ".mp4", cloudMessage);
                    }
                }
                else if (contentType == "WEBURL")
                {
                    string httpUrl = "";
                    httpUrl = url.Replace("https:", "http:");
                    httpUrl = System.Net.WebUtility.UrlEncode(httpUrl);
                    FeedXML.UpdateXMLConfig(httpUrl, cloudMessage);
                }
            }
            catch (Exception ex)
            {
                Logger.LogToConnector(ex.ToString());
            }
        }
    }
}
