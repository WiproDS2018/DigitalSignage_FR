using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SignageFaceRecognition.Face
{
    class XmlHandler
    {

        private static string xmlPath = System.Configuration.ConfigurationManager.AppSettings["XMLPath"];
        public static int TurnOffNormalEntries()
        {
            DateTime b = DateTime.Now;
            XmlDocument doc = LoadXml();
            if (doc == null) return -1;
            int count = 0;
            XmlNodeList list = doc.SelectNodes("DigitalSignageConfig/Display/file");
            foreach (XmlElement elm in list)
            {
                elm.SetAttribute("status", "off");
                count++;
            }
            SaveXml(doc);
            DateTime end = DateTime.Now;
            // Console.WriteLine("Time Taken for XML TURN OFF " + (end - b).Milliseconds);
            //LogWriter.AddToLog("Time Taken for XML TURN OFF " + (end - b).Milliseconds);
            return count;
        }
        private static XmlDocument LoadXml()
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlPath);
                return doc;
            }
            catch (Exception )
            {
               // LogHelper.WriteDebugLog("Failed to Load XML");
            }
            return null;
        }
        private static int SaveXml(XmlDocument doc)
        {
            try
            {
                doc.Save(xmlPath);
                return 1;
            }
            catch (Exception )
            {
                return -1;
            }
        }
        public static int TurnOnScheduledEntries()
        {
            DateTime b = DateTime.Now;

            XmlDocument doc = LoadXml();
            if (doc == null) return -1;
            int count = 0;
            XmlNodeList list = doc.SelectNodes("DigitalSignageConfig/Display/file");
            foreach (XmlElement elm in list)
            {
                if (elm.GetAttribute("temp") != "yes")
                {
                    // Console.WriteLine(elm.GetAttribute("path") + " is set on");
                    //    LogWriter.AddToLog(elm.GetAttribute("path") + " is set on\n");
                    elm.SetAttribute("status", "on");
                    count++;
                }
                else elm.SetAttribute("status", "off");
            }
            SaveXml(doc);
            DateTime end = DateTime.Now;
            // Console.WriteLine("Time Taken for XML TURN ON " + (end - b).Milliseconds);
            //    LogWriter.AddToLog("Time Taken for XML TURN ON " + (end - b).Milliseconds);
            return count;
        }
        public static int DeleteTempEntries()
        {
            DateTime b = DateTime.Now;

            int count = 0;
            XmlDocument doc = LoadXml();
            if (doc == null) return -1;
            XmlNodeList nodes = doc.SelectNodes("/DigitalSignageConfig/Display/file");
            foreach (XmlElement node in nodes)
            {
                if (node.GetAttribute("temp") == "yes")
                {
                    // get its parent node
                    XmlNode parent = node.ParentNode;
                    // remove the child node
                    parent.RemoveChild(node);

                    // verify the new XML structure
                    string newXML = doc.OuterXml;

                    // save to file or whatever....
                    count++;
                }
            }
            SaveXml(doc);


            Console.WriteLine(count + " entries in xml deleted");
           // LogHelper.WriteDebugLog(count + " entries in xml deleted");
            //   LogWriter.AddToLog(count + " entries in xml deleted\n");
            DateTime end = DateTime.Now;
            // Console.WriteLine("Time Taken for XML DELETE " + (end - b).Milliseconds);
            //    LogWriter.AddToLog("Time Taken for XML DELETE " + (end - b).Milliseconds);
            return count;
        }
        public static void AddToXml(List<string> urls)
        {
            DateTime b = DateTime.Now;

            XmlDocument doc = LoadXml();
            if (doc == null) return;
            XmlElement displayElement = doc.DocumentElement["Display"];
            foreach (string file in urls)
            {
                XmlNodeList nodes = doc.SelectNodes("/DigitalSignageConfig/Display/file[@path='" + file + "']");

                XmlElement newFile = doc.CreateElement("file");
                newFile.SetAttribute("path", file);
                newFile.SetAttribute("duration", "4000");
                DateTime now = DateTime.Now;
                DateTime plusOneMinute = now.AddMinutes(10);
                newFile.SetAttribute("start-time", now.ToString("dd-MM-yyyy HH:mm:ss"));
                newFile.SetAttribute("end-time", plusOneMinute.ToString("dd-MM-yyyy HH:mm:ss"));
                newFile.SetAttribute("status", "on");
                newFile.SetAttribute("content-type", "IMAGE-UPLOAD");
                newFile.SetAttribute("temp", "yes");
                displayElement.AppendChild(newFile);

            }
            SaveXml(doc);
            DateTime end = DateTime.Now;
            Console.WriteLine(urls.Count + " Entries Added to XML ");
            //LogHelper.WriteDebugLog(urls.Count + " Entries Added to XML");
            //  LogWriter.AddToLog("Time Taken for XML ADDITION " + (end - b).Milliseconds);
        }
    }
}
