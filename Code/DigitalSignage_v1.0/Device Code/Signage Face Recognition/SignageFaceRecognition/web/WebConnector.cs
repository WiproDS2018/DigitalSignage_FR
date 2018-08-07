﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SignageFaceRecognition
{
    class WebConnector
    {
        private static HttpClient client = new HttpClient();

        public static async Task<string> GetDeviceRegistrationDetails(string registrationId, string endorsementKey)
        {
            client.BaseAddress = new Uri(Enviornment.WebApiAddress);
            HttpResponseMessage response = await client.GetAsync($"/device/?id={registrationId}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                string result = await content.ReadAsStringAsync();
                Logger.LogToConnector($"From GET method {result}");
                return result;
            }
            var values = new Dictionary<string, string>
                {
                    { "id", registrationId },
                    { "key", endorsementKey }
                 };

            var formContent = new FormUrlEncodedContent(values);
            var postResponse = await client.PostAsync("/device", formContent);

            var responseString = await postResponse.Content.ReadAsStringAsync();
            Logger.LogToConnector($"From POST method {responseString}");
            return responseString;
        }
        public static string CreateDevice()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var values = new Dictionary<string, string>
                {
                    { "id", TpmGenerator.GetGenerator().RegistrationId },
                    { "password", "cre@teDev!ce" }
                 };
                var formContent = new FormUrlEncodedContent(values);
                var postResponse = httpClient.PostAsync($"{Enviornment.WebApiAddress}/device/deviceDetails", formContent).Result;
               
                var responseString = postResponse.Content.ReadAsStringAsync().Result;
                while (responseString == null || responseString.Contains("internal server error"))
                {
                    var newResponse = httpClient.PostAsync($"{Enviornment.WebApiAddress}/device/deviceDetails", formContent).Result;
                    responseString= newResponse .Content.ReadAsStringAsync().Result;
                }
                Logger.LogToConnector($"From POST method {responseString}");
                Logger.LogToConnector("Device Created");
                return responseString;
            }
            catch (Exception) { return null;}
            
            // /deviceDetails
        }
    }
}
