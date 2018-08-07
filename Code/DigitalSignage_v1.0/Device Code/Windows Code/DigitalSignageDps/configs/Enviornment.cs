using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DigitalSignageDps
{
    class Enviornment
    {
        public static readonly string FaceStoragePath = "C:/Signage/Faces";
        public static readonly string LogPath = "C:/Signage/Devicelogs";
        public static readonly string ConfigPath = "C:/Signage/Config";
        public static readonly string ImageStoragePath = "C:/Signage/Images";
        public static readonly string GlobalDeviceProvisioningEndPoint = "global.azure-devices-provisioning.net";
        public static readonly string DefaultImage = "welcome.jpg";
        public static  string DeviceProvisioningScopeId ;
        public static readonly string IotHubUri = ConfigurationManager.AppSettings["IotHubUri"];
       // public static readonly string StorageConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
        public static  string WebApiAddress ;
        public static void PrepareEnviornment()
        {
            Directory.CreateDirectory(ConfigPath);
            Directory.CreateDirectory(ImageStoragePath);
            Directory.CreateDirectory(FaceStoragePath);
            Directory.CreateDirectory(LogPath);
        
        }
    }
}
