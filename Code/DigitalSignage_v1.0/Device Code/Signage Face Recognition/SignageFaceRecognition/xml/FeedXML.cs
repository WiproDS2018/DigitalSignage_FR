using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SignageFaceRecognition
{
    public class FeedXML
    {
        public static void UpdateXMLConfig(string imagePath, CloudMessage cloudMessage)
        {
            string playerConfigPath = Enviornment.ConfigPath + "/config.xml";
            XmlDocument doc = new XmlDocument();

            if (!File.Exists(playerConfigPath))
            {
                XmlElement xmlRoot = doc.CreateElement("DigitalSignageConfig");
                XmlElement xmlElem = doc.CreateElement("Display");
                xmlRoot.AppendChild(xmlElem);
                doc.AppendChild(xmlRoot);
                doc.Save(playerConfigPath);
            }
            doc.Load(playerConfigPath);
            XmlElement xmlEl = doc.DocumentElement["Display"];
            XmlElement xmlElfile = doc.CreateElement("file");
            xmlElfile.SetAttribute("path", imagePath);
            xmlElfile.SetAttribute("duration", cloudMessage.Duration);
            xmlElfile.SetAttribute("start-time", cloudMessage.Start);
            xmlElfile.SetAttribute("end-time", cloudMessage.End);
            xmlElfile.SetAttribute("content-type", cloudMessage.ContentType);
            xmlElfile.SetAttribute("status", "on");
            xmlElfile.SetAttribute("frequency", cloudMessage.Frequency);

            if (cloudMessage.DaysOfWeek != null) xmlElfile.SetAttribute("daysOfWeek", cloudMessage.DaysOfWeek);

            if (cloudMessage.IconPosition != null) xmlElfile.SetAttribute("position", cloudMessage.IconPosition);
            xmlEl.AppendChild(xmlElfile);
            doc.Save(playerConfigPath);
        }
        public static void DeleteFromConfig(string imagePath)
        {
            string playerConfigPath = Enviornment.ConfigPath + "/config.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(playerConfigPath);
            XmlNode node = doc.SelectSingleNode("/DigitalSignageConfig/Display/file[@path='" + imagePath + "']");
            if (node != null)
            {
                // get its parent node
                XmlNode parent = node.ParentNode;
                // remove the child node
                parent.RemoveChild(node);

                // verify the new XML structure
                string newXML = doc.OuterXml;

                // save to file or whatever....
                doc.Save(playerConfigPath);
            }
        }

    }

}
