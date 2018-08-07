using IoTCoreDefaultApp.Message;
using IoTCoreDefaultApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace IoTCoreDefaultApp.Xml
{
    class XmlModifier
    {
        public static void AddToXMLConfig( CloudMessage cloudMessage)
        {
            Log.Write("Adding to xml");

           XmlDocument xmlConfig = new XmlDocument();
            string text = Windows.Storage.FileIO.ReadTextAsync(Config.Environment.ConfigFile).AsTask().Result;
            xmlConfig.LoadXml(text);

            Log.Write("Xml Loaded");
            IXmlNode xmlEl = xmlConfig.DocumentElement.SelectSingleNode("Display");
            XmlElement xmlElfile = xmlConfig.CreateElement("file");
            
            xmlElfile.SetAttribute("duration", cloudMessage.Duration);
            xmlElfile.SetAttribute("start-time", cloudMessage.Start);
            xmlElfile.SetAttribute("end-time", cloudMessage.End);
            xmlElfile.SetAttribute("content-type", cloudMessage.ContentType);
            if(cloudMessage.ContentType =="IMAGE-UPLOAD" || cloudMessage.ContentType =="IMAGE-TEMPLATE")
            {
                int index = cloudMessage.ContentUrl.LastIndexOf('/');
                string fileName = cloudMessage.ContentUrl.Substring(index + 1);
                xmlElfile.SetAttribute("path", fileName);
                StorageFile destinationFile = Config.Environment.ImagesFolder.GetFileAsync(fileName).AsTask().Result;
                if (!File.Exists(destinationFile.Path))
                {
                    Log.Write("Downloading Image");
                    HttpClient client = new HttpClient();
                    byte[] buffer =  client.GetByteArrayAsync(cloudMessage.ContentUrl).Result;
                    using (Stream fileStream =  destinationFile.OpenStreamForWriteAsync().Result)
                    {
                        fileStream.Write(buffer, 0, buffer.Length);
                        Log.Write("Image Downloaded");
                    }
                }
            }
            else
            {
                xmlElfile.SetAttribute("path", cloudMessage.ContentUrl.Replace("https","http"));
               
            }
            xmlElfile.SetAttribute("status", "on");
            xmlElfile.SetAttribute("frequency", cloudMessage.Frequency);

            if (cloudMessage.DaysOfWeek != null) xmlElfile.SetAttribute("daysOfWeek", cloudMessage.DaysOfWeek);

            if (cloudMessage.IconPosition != null) xmlElfile.SetAttribute("position", cloudMessage.IconPosition);

            xmlEl.AppendChild(xmlElfile);

            xmlConfig.SaveToFileAsync(Config.Environment.ConfigFile).AsTask().Wait();
            Log.Write("Saved Xml");
        }

        public static void DeleteFromConfig(string imagePath)
        {

            XmlDocument xmlConfig = new XmlDocument();
            string text = Windows.Storage.FileIO.ReadTextAsync(Config.Environment.ConfigFile).AsTask().Result;
            xmlConfig.LoadXml(text);
         


            IXmlNode node = xmlConfig.SelectSingleNode("/DigitalSignageConfig/Display/file[@path='" + imagePath + "']");
            if (node != null)
            {
                // get its parent node
                IXmlNode parent = node.ParentNode;
                // remove the child node
                parent.RemoveChild(node);

                // save to file or whatever....
                xmlConfig.SaveToFileAsync(Config.Environment.ConfigFile).AsTask().Wait();
            }
        }
    }
}
