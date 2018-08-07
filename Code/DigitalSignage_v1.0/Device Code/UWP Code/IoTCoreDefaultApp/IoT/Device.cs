using IoTCoreDefaultApp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;

namespace IoTCoreDefaultApp.IoT
{
    class Device
    {
        public string DeviceKey { get; set; }
        public string IotHub { get; set; }
        public static Device GetDeviceDetails()
        {
            Device deviceDetails = null;

            if (File.Exists(Path.Combine(ApplicationData.Current.LocalFolder.Path, "DeviceDetails.json"))) 
            {
                StorageFile deviceDetailsFile = ApplicationData.Current.LocalFolder.GetFileAsync("DeviceDetails.json").AsTask().Result;
                Log.Write("using Details from file");
                string deviceSetting = Windows.Storage.FileIO.ReadTextAsync(deviceDetailsFile).AsTask().Result;
                Log.Write(deviceSetting);
                deviceDetails = JsonConvert.DeserializeObject<Device>(deviceSetting);
               
            }
            else
            {
                // Fetching from Web And Saving to File
                Log.Write("Fetching Details From Web");
                HttpClient httpClient = new HttpClient();
                var deviceInformation = new EasClientDeviceInformation();
                var base64Guid = Config.Environment.DeviceId;

                //var base64Guid = deviceInformation.Id.ToString();
                // Replace URL unfriendly characters with better ones
                try
                {
                    var values = new Dictionary<string, string>
                {
                        { "id" , base64Guid },
                        { "password", "cre@teDev!ce" }
                    };

                    var formContent = new FormUrlEncodedContent(values);
                    var httpResponse = httpClient.PostAsync(MainPage.WebApiEndPoint+"/device/deviceDetails", formContent).Result;

                    string details=httpResponse.Content.ReadAsStringAsync().Result;
                    while(details==null || details.Contains("internal server error"))
                    {
                       var newResponse = httpClient.PostAsync(MainPage.WebApiEndPoint + "/device/deviceDetails", formContent).Result;
                        details = newResponse.Content.ReadAsStringAsync().Result;
                    }
                    var file = ApplicationData.Current.LocalFolder.CreateFileAsync("DeviceDetails.json", CreationCollisionOption.ReplaceExisting).AsTask().Result;
                    FileIO.WriteTextAsync(file, details).AsTask().Wait();
                    Log.Write(details);
                    deviceDetails = JsonConvert.DeserializeObject<Device>(details);
                }
                catch (Exception) {
                    Log.Write("Unable to fetch device Details");
                }
            }
            return deviceDetails;
        }
    }
}
