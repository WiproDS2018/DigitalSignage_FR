using IoTCoreDefaultApp.Message;
using IoTCoreDefaultApp.Utils;
using IoTCoreDefaultApp.Xml;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;

namespace IoTCoreDefaultApp.IoT
{
    class DeviceMessenger
    {

        public static Task messageReceiver;
        public static DeviceClient client = null;
        public static async Task StartMessagingService()
        {
            Log.Write("Entered in Messaging Service");
            if (client != null) return;
            while (!IsInternet()) await Task.Delay(2000);
            try
            {
                Device device = Device.GetDeviceDetails();
                var deviceInformation = new EasClientDeviceInformation();
                var base64Guid = Config.Environment.DeviceId;
                //var base64Guid = deviceInformation.Id.ToString();
                // Replace URL unfriendly characters with better ones
                //base64Guid = base64Guid.Replace('+', '-').Replace('/', '_');

                //// Remove the trailing ==
                //base64Guid = base64Guid.Substring(0, base64Guid.Length - 12).ToLower();
                //base64Guid = base64Guid.Substring(0, 4) + "-" + base64Guid.Substring(4, 4) + "-" + base64Guid.Substring(8, 4);

                string connectionString = $"HostName={device.IotHub};DeviceId={base64Guid};SharedAccessKey={device.DeviceKey}";
                client = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Http1);
                 await MessengeSender();
                Log.Write("Connected To Hub");
            }
            catch (Exception e) { Log.Write(e.ToString()); }
          
        }
        private static async Task<bool> MessengeSender()
        {
            Log.Write("Network Is Connected");
             
             messageReceiver = Task.Factory.StartNew(() =>
             {
                 while (true)
                 {
                     try
                     {
                         Microsoft.Azure.Devices.Client.Message receivedMessage = client.ReceiveAsync().Result;
                         if (receivedMessage == null) continue;

                         string msg = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                         Log.Write($"Received Message : {msg}");
                         CloudMessage message = JsonConvert.DeserializeObject<CloudMessage>(msg);
                         Log.Write("Message Deserialize");
                         
                         if (message.show=="True" ||message.show=="true")
                             Json.PlaylistModifier.AddToPlaylist(message);
                         else
                         {
                             Json.PlaylistModifier.RemoveFromPlayList(message);
                         }
                         client.CompleteAsync(receivedMessage).Wait();
                         Log.Write("Acknowledgement sent");
                     }
                     catch (Exception e)
                     {
                         Log.Write(e.ToString());
                     }
                 }
             });
               
                return false;
            }
        public  static bool IsInternet()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            return connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            
        }
        public static async Task SendMessageToCloud(string fileName)
        {
            
            if (client == null) {
              
                return;
            }
            var message = new
            {
                status = "Connected",
                NowPlaying = fileName
            };
            await client.SendEventAsync(new
                Microsoft.Azure.Devices.Client.Message(
                Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(message))));
          
        }
    }
}
