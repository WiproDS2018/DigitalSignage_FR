using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SignageFaceRecognition
{
    class DeviceMessenger
    {

        private DeviceClient deviceClient;
        private ProvisionResult details;
        private string deviceId;
        private DeviceProvision provision;
        public DeviceMessenger(DeviceClient deviceClient)
        {
            this.deviceClient = deviceClient;
            if (provision != null)
                this.details = provision.ProvisionDetails;
           // this.provision = provision;

        }
        public void StartMessenger()
        {

            try
            {
                deviceId = TpmGenerator.GetGenerator().RegistrationId;

                Device device = new Device();
                device.DeviceId = TpmGenerator.GetGenerator().RegistrationId;
                Thread sender = new Thread(() =>
                {
                    SendMessages(device);
                });
                sender.IsBackground = true;
                sender.Start();
                Thread receiver = new Thread(() => {
                    ReceiveMessages(device);
                });
                receiver.IsBackground = true;
                receiver.Start();

            }
            catch (Exception ex)
            {
                Logger.LogToConnector(ex.ToString());
            }

        }

        private void SendMessages(Device device)
        {

            while (true)
            {
                try
                {
                    device.Status = "connected";
                    var messageString = JsonConvert.SerializeObject(device);
                    var message = new Message(Encoding.ASCII.GetBytes(messageString));

                    deviceClient.SendEventAsync(message).Wait();
                    Logger.LogToConnector(" Sending message:  " + messageString);
                    Thread.Sleep(300000);
                }
                catch (Exception ex)
                {
                    if (deviceClient == null) continue;
                    Logger.LogToConnector(ex.ToString());
                    continue;
                }
            }

        }

        private void ReceiveMessages(Device device)
        {

            Logger.LogToConnector("Receiving cloud to device messages from service");

            while (true)
            {
                try
                {
                    Message receivedMessage = deviceClient.ReceiveAsync().Result;
                    if (receivedMessage == null) continue;
                    string msg = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Logger.LogToConnector($"Message Received {msg}");
                    CloudMessage message = JsonConvert.DeserializeObject<CloudMessage>(msg);

                    if (message.show == "True" || message.show == "true")
                    {
                        DownloadBlob.DownloadFromURL(message);
                    }
                    else
                    {
                        DeleteFile.Delete(message.ContentUrl.Trim());
                    }

                    deviceClient.CompleteAsync(receivedMessage).Wait();
                }
                catch (Exception e)
                {
                    if (deviceClient == null) continue;
                    Logger.LogToConnector(e.ToString());

                    continue;
                }
            }
        }

    }
}
