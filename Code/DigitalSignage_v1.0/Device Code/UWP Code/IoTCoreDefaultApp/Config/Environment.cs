using IoTCoreDefaultApp.Json;
using IoTCoreDefaultApp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Data.Xml;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace IoTCoreDefaultApp.Config
{
    class Environment
    {
        public static StorageFolder SignageFolder;
        public static StorageFolder ImagesFolder;
        public static string XmlFileName = "config.xml";
        public static string JsonFileName = "config.json";
        public static StorageFile ConfigFile;
        public static StorageFile SettingsFile;
        public static StorageFile ConfigBackup;
        public static string DeviceId;
        public static void ConfigureEnvironment()
        {
            try
            {
                SignageFolder = ApplicationData.Current.LocalFolder.
                    CreateFolderAsync("Signage", CreationCollisionOption.FailIfExists).AsTask().Result;

            }
            catch (Exception)
            {
                SignageFolder = ApplicationData.Current.LocalFolder.GetFolderAsync("Signage").AsTask().Result;

            }
            try
            {
                ImagesFolder = SignageFolder.
                    CreateFolderAsync("Images", CreationCollisionOption.FailIfExists).AsTask().Result;
            }
            catch (Exception)
            {
                ImagesFolder = SignageFolder.GetFolderAsync("Images").AsTask().Result;
            }
            string folderPath = SignageFolder.Path;
            string configFilePath = Path.Combine(folderPath, JsonFileName);
            string configBackUpPath = Path.Combine(folderPath, "config_backup.json");
            if (!File.Exists(configFilePath)) CreateJsonConfigFile();
            ConfigFile = SignageFolder.GetFileAsync("config.json").AsTask().Result;
            //if (!File.Exists(configBackUpPath)) CreateBackupFile();
            string settingFilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "settings.json");
            if (!File.Exists(settingFilePath))
            {
               
                EndPointSettings settings = new EndPointSettings();
                settings.DeviceId = GenerateRandomString();
                settings.EndPoint = "";
                string json = JsonConvert.SerializeObject(settings);

                // write string to a file
                var file = ApplicationData.Current.LocalFolder.CreateFileAsync("settings.json", CreationCollisionOption.ReplaceExisting).AsTask().Result;
                FileIO.WriteTextAsync(file, json).AsTask().Wait();
                SettingsFile = file;
            
            }
            else
            {
                SettingsFile = ApplicationData.Current.LocalFolder.GetFileAsync("settings.json").AsTask().Result;
                string settings = FileIO.ReadTextAsync(SettingsFile).AsTask().Result;
                EndPointSettings endPointSettings = JsonConvert.DeserializeObject<EndPointSettings>(settings);
                DeviceId = endPointSettings.DeviceId;
            }
            Log.Write("App settings configured");
        }
        public static void CreateXmlConfigFile()
        {
            XmlDocument doc = new XmlDocument();
            StorageFile configFile = SignageFolder.CreateFileAsync("config.xml").AsTask().Result;
            XmlElement xmlRoot = doc.CreateElement("DigitalSignageConfig");
            XmlElement xmlElem = doc.CreateElement("Display");
            xmlRoot.AppendChild(xmlElem);
            doc.AppendChild(xmlRoot);
            doc.SaveToFileAsync(configFile).AsTask().Wait();
            ConfigFile = configFile;
        }
        
        public static void  CreateJsonConfigFile()
        {
            StorageFile configFile = SignageFolder.CreateFileAsync("config.json").AsTask().Result;
            JsonPlaylist playlist = new JsonPlaylist();
            playlist.playlist = new List<Message.CloudMessage>();
            string playlistJson = JsonConvert.SerializeObject(playlist);
            FileIO.WriteTextAsync(configFile,playlistJson).AsTask().Wait();
            ConfigFile = configFile;
        }
        private static string GenerateRandomString()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch='a';
            for (int i = 0; i < 12; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            
                return builder.ToString().ToLower();
        }
    }
}
