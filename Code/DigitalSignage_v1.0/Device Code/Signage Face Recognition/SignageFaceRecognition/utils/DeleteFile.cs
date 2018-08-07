using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignageFaceRecognition
{
    class DeleteFile
    {
        public static void Delete(string url)
        {
            string httpUrl = url.Replace("https:", "http:");
            httpUrl = System.Net.WebUtility.UrlEncode(httpUrl);
            FeedXML.DeleteFromConfig(httpUrl);
            int index = url.LastIndexOf('/');
            string file = url.Substring(index + 1);
            string path = Path.Combine(@"C:\Signage\Images", file);


            FeedXML.DeleteFromConfig(@url);
            FeedXML.DeleteFromConfig(@path);
            while (System.IO.File.Exists(@path))
            {
                try
                {
                    File.Delete(@path);
                }
                catch (Exception) { continue; }

            }
            path = path + ".mp4";
            FeedXML.DeleteFromConfig(@path);
            while (System.IO.File.Exists(@path))
            {
                try
                {
                    File.Delete(@path);
                }
                catch (Exception) { continue; }

            }
        }
    }
}
