using IoTCoreDefaultApp.Message;
using IoTCoreDefaultApp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace IoTCoreDefaultApp.Json
{
    class PlaylistModifier
    {
       public static void AddToPlaylist(CloudMessage cloudMessage)
        {
            string text = Windows.Storage.FileIO.ReadTextAsync(Config.Environment.ConfigFile).AsTask().Result;
            JsonPlaylist currentPlaylist = null;
            try
            {
                currentPlaylist = JsonConvert.DeserializeObject<JsonPlaylist>(text);
            }catch(Exception e) { Log.Write(e.ToString()); }
            cloudMessage.ContentUrl= cloudMessage.ContentUrl.Replace("https:", "http:").Trim();
            int index = cloudMessage.ContentUrl.LastIndexOf('/');
            string fileName = cloudMessage.ContentUrl.Substring(index+ 1);
            if(cloudMessage.ContentType.Contains("IMAGE"))
            {
                if (!File.Exists(Path.Combine(Config.Environment.ImagesFolder.Path,fileName)))
                {
                    StorageFile file = Config.Environment.ImagesFolder.CreateFileAsync(fileName).AsTask().Result;
                    HttpClient client = new HttpClient();
                    byte[] buffer = client.GetByteArrayAsync(cloudMessage.ContentUrl).Result;
                    using (Stream stream = file.OpenStreamForWriteAsync().Result)
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
                cloudMessage.ContentUrl = fileName;
            }
            else if (cloudMessage.ContentType.Contains("VIDEO"))
            {
                if (!File.Exists(Path.Combine(Config.Environment.ImagesFolder.Path, fileName+".mp4")))
                {
                    StorageFile file = Config.Environment.ImagesFolder.CreateFileAsync(fileName+".mp4").AsTask().Result;
                    HttpClient client = new HttpClient();
                    byte[] buffer = client.GetByteArrayAsync(cloudMessage.ContentUrl).Result;
                    using (Stream stream = file.OpenStreamForWriteAsync().Result)
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
                cloudMessage.ContentUrl = fileName+".mp4";
            }

            currentPlaylist.playlist.RemoveAll((message) => message.ContentUrl == cloudMessage.ContentUrl);
            currentPlaylist.playlist.Add(cloudMessage);
            FileIO.WriteTextAsync(Config.Environment.ConfigFile,JsonConvert.SerializeObject(currentPlaylist)).AsTask().Wait();
        }

        public static void RemoveFromPlayList(CloudMessage cloudMessage)
        {
            string text = Windows.Storage.FileIO.ReadTextAsync(Config.Environment.ConfigFile).AsTask().Result;
            JsonPlaylist currentPlaylist = JsonConvert.DeserializeObject<JsonPlaylist>(text);
            cloudMessage.ContentUrl = cloudMessage.ContentUrl.Trim().Replace("https:", "http:").Trim();
            if (cloudMessage.ContentType.Contains("IMAGE"))
            {
                int index = cloudMessage.ContentUrl.LastIndexOf('/');
                string fileName = cloudMessage.ContentUrl.Substring(index + 1);
                cloudMessage.ContentUrl = fileName;
                DeleteFile(Path.Combine(Config.Environment.ImagesFolder.Path,fileName));
            }
            else if (cloudMessage.ContentType.Contains("VIDEO"))
            {
                int index = cloudMessage.ContentUrl.LastIndexOf('/');
                string fileName = cloudMessage.ContentUrl.Substring(index + 1);
                cloudMessage.ContentUrl = fileName+".mp4";
                DeleteFile(Path.Combine(Config.Environment.ImagesFolder.Path, fileName+".mp4"));
            }
            currentPlaylist.playlist.RemoveAll((message)=>  message.ContentUrl==cloudMessage.ContentUrl);
            FileIO.WriteTextAsync(Config.Environment.ConfigFile, JsonConvert.SerializeObject(currentPlaylist)).AsTask().Wait();

        }
        public static void RemoveAllFromPlayList()
        {
            string text = Windows.Storage.FileIO.ReadTextAsync(Config.Environment.ConfigFile).AsTask().Result;
            JsonPlaylist currentPlaylist = JsonConvert.DeserializeObject<JsonPlaylist>(text);
            currentPlaylist.playlist.RemoveAll((message)=>true);
            FileIO.WriteTextAsync(Config.Environment.ConfigFile, JsonConvert.SerializeObject(currentPlaylist)).AsTask().Wait();

        }
        public static void DeleteFile(string filePath)
        {
            bool deleted = false;
            while(!deleted)
            {
                try
                {
                    File.Delete(filePath);
                    deleted = true;
                    Log.Write($"Deleted File {filePath}");
                }catch(Exception)
                {
                    Log.Write($"Unable to delete File {filePath}");
                    deleted = false;
                }
            }
        }

        public static List<string>GetAllExpiredEntries()
        {
            List<string> list = new List<string>();
            //Log.Write("loading file");
            StorageFolder folder = ApplicationData.Current.LocalFolder.GetFolderAsync("Signage").AsTask().Result;
            StorageFile config = folder.GetFileAsync("config.json").AsTask().Result;
            string text = FileIO.ReadTextAsync(config).AsTask().Result;
            //Log.Write("file contents");
            //Log.Write(text);

            JsonPlaylist currentPlaylist = JsonConvert.DeserializeObject<JsonPlaylist>(text);
            foreach (CloudMessage message in currentPlaylist.playlist)
            {
                message.ContentUrl = message.ContentUrl.Trim();
                if ( !Validations.ValidateDate(message.Start, message.End, message.Frequency, message.DaysOfWeek))
                {
                    list.Add(message.ContentUrl);
                }
            }
                    return list;
        }

        public static void RemoveAllExpiredEntries()
        {
            try
            {
                //Log.Write("loading file");
                StorageFolder folder = ApplicationData.Current.LocalFolder.GetFolderAsync("Signage").AsTask().Result;
                StorageFile config = folder.GetFileAsync("config.json").AsTask().Result;
                string text = FileIO.ReadTextAsync(config).AsTask().Result;
                //Log.Write("file contents");
                //Log.Write(text);
                Log.Write("Deleting expired entries");
                JsonPlaylist currentPlaylist = JsonConvert.DeserializeObject<JsonPlaylist>(text);
                foreach (CloudMessage message in currentPlaylist.playlist)
                {
                    DateTime endTimestamp = DateTime.ParseExact(message.End, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    DateTime todayDate = DateTime.Now;
                 
                    bool isEndDateNotValid = DateTime.Compare(endTimestamp,todayDate) <0;

                    if (isEndDateNotValid)
                    {
                        RemoveFromPlayList(message);
                    }
                }
                Log.Write("Deleted Expired Files");

            }catch(Exception e) { Log.Write(e.ToString()); }
            }
    }

 
}
