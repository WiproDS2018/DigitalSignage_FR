using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SignageFaceRecognition
{
    class DeviceProvision
    {
        //ProvisioningTransportHandlerMqtt provisioningTransport =
        //  new ProvisioningTransportHandlerMqtt();


        ProvisioningTransportHandlerHttp provisioningTransport =
            new ProvisioningTransportHandlerHttp();


        TpmGenerator tpmGenerator = TpmGenerator.GetGenerator();

        public ProvisionResult ProvisionDetails = null;
        public DeviceClient IotClient = null;
        public async Task<ProvisionResult> EnrollDevice(TextBlock message, ProgressBar progress)
        {

            message.Dispatcher.Invoke(() => {
                message.Text = "creating device please wait...";
            });
            string details = await WebConnector.GetDeviceRegistrationDetails(tpmGenerator.RegistrationId, tpmGenerator.EndorsementKey);
            ProvisionResult result = null;
            try
            {
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<ProvisionResult>(details);
                ProvisionDetails = result;
                progress.Dispatcher.Invoke(() => { progress.Visibility = System.Windows.Visibility.Hidden; });
                Logger.LogToConnector($"registrationId : {result.registrationId}");
                Logger.LogToConnector($"endorsementKey : {result.attestation.tpm.endorsementKey}");

                message.Dispatcher.Invoke(() => {
                    message.Visibility = System.Windows.Visibility.Visible;
                    message.Text = "Registration Successfull";
                });
                return result;
            }
            catch (Exception e)
            {

                message.Dispatcher.Invoke(() => {
                    message.Text = "Please wait while application restarts ";
                    message.Visibility = System.Windows.Visibility.Visible;
                });
                progress.Dispatcher.Invoke(() => { progress.Visibility = System.Windows.Visibility.Hidden; });
                Logger.LogToConnector(e.ToString());
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);


                Application.Current.Shutdown();
            }
            return result;
        }

        public void CreateDeviceClient(TextBlock message, ProgressBar progress)
        {


            message.Dispatcher.Invoke(() => {
                message.Text = "Connecting To IotHub...";
            });

            progress.Dispatcher.Invoke(() => {
                progress.Visibility = System.Windows.Visibility.Visible;
            });

            try
            {
                DeviceRegistrationResult result = GetDeviceRegistrationResult();

                Logger.LogToConnector("Device Registered With IotHub");
                Logger.LogToConnector($"DeviceId : {result.DeviceId}");
                Logger.LogToConnector($"IotHub : {result.AssignedHub}");
                var auth = new DeviceAuthenticationWithTpm(result.DeviceId, tpmGenerator.SecurityProvider);
                IotClient = DeviceClient.Create(result.AssignedHub, auth, Microsoft.Azure.Devices.Client.TransportType.Http1);
                Logger.LogToConnector("Device Connected to Hub");
                message.Dispatcher.Invoke(() => {
                    message.Text = "Double click on device id to goto player\nRight Click on deviceId to copy to clipboard";
                });

                progress.Dispatcher.Invoke(() => {
                    progress.Visibility = System.Windows.Visibility.Hidden;
                });



            }
            catch (Exception e)
            {
                Logger.LogToConnector(e.ToString());

                message.Dispatcher.Invoke(() => {
                    message.Foreground = Brushes.Red;
                    message.Text = $"Failed Registering with IotHub {e.Message}";
                });

                progress.Dispatcher.Invoke(() => {
                    progress.Visibility = System.Windows.Visibility.Hidden;
                });
            }
        }
        public DeviceClient GetDeviceClient()
        {
            ProvisioningDeviceClient provClient =
                  ProvisioningDeviceClient.Create(Enviornment.GlobalDeviceProvisioningEndPoint,
                  Enviornment.DeviceProvisioningScopeId, tpmGenerator.SecurityProvider, provisioningTransport);
            DeviceRegistrationResult result = provClient.RegisterAsync().Result;
            Logger.LogToConnector("Device Registered With IotHub");
            Logger.LogToConnector($"DeviceId : {result.DeviceId}");
            Logger.LogToConnector($"IotHub : {result.AssignedHub}");
            var auth = new DeviceAuthenticationWithTpm(result.DeviceId, tpmGenerator.SecurityProvider);
            return DeviceClient.Create(result.AssignedHub, auth, Microsoft.Azure.Devices.Client.TransportType.Http1);

        }
        public DeviceRegistrationResult GetDeviceRegistrationResult()
        {
            try
            {

                var cl = ProvisioningDeviceClient.Create(Enviornment.GlobalDeviceProvisioningEndPoint,
                    Enviornment.DeviceProvisioningScopeId, tpmGenerator.SecurityProvider, new ProvisioningTransportHandlerHttp());
                DeviceRegistrationResult result = cl.RegisterAsync().Result;
                return result;
            }
            catch (Exception)
            {
                var cl = ProvisioningDeviceClient.Create(Enviornment.GlobalDeviceProvisioningEndPoint,
                    Enviornment.DeviceProvisioningScopeId, tpmGenerator.SecurityProvider, new ProvisioningTransportHandlerAmqp());
                DeviceRegistrationResult result = cl.RegisterAsync().Result;
                return result;
            }
        }

        public DeviceClient CreateDeviceClient()
        {

            string settings = WebConnector.CreateDevice();
            DeviceDetails deviceDetails = JsonConvert.DeserializeObject<DeviceDetails>(settings);
            string connectionString = $"HostName={deviceDetails.IotHub};DeviceId={TpmGenerator.GetGenerator().RegistrationId};SharedAccessKey={deviceDetails.DeviceKey}";
            Logger.LogToConnector(connectionString);
            return DeviceClient.CreateFromConnectionString(connectionString, TransportType.Http1);
        }
    }
}
