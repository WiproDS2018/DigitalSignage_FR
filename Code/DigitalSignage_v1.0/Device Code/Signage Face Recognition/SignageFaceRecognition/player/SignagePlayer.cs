using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace SignageFaceRecognition
{
    class SignagePlayer
    {
        private string xmlPath = Enviornment.ConfigPath + "/config.xml";
        private string defaultImage = AppDomain.CurrentDomain.BaseDirectory + "\\" + Enviornment.DefaultImage;
        private MediaElement player;
        private WebBrowser webBrowser;
        private Grid weather;
        private MediaElement faceRecognitionPlayer;
        public SignagePlayer(MainWindow mainWindow)
        {
            this.player = mainWindow.mePlayer;
            this.webBrowser = mainWindow.meBrowser;
            this.weather =mainWindow.weather;
            this.faceRecognitionPlayer = mainWindow.faceRecognitionPlayer;
        }

        public bool Pause { get; set; }

        public void DisplayImageXML()
        {
            Logger.LogToPlayer($"Default Image Path is {defaultImage}");
            Logger.LogToPlayer($"Images Path is {Path.GetFullPath(Enviornment.ImageStoragePath)}");
            try
            {
                string xmlPath = @"C:\Signage\Config\config.xml";


                Pause = false;
                while (true)
                {
                    if (Pause) continue;
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        doc.Load(xmlPath);
                    }
                    catch (Exception e)
                    {
                        Logger.LogToPlayer(e.ToString());
                        continue;
                    }


                    XmlNodeList list = doc.SelectNodes("DigitalSignageConfig/Display/file");
                    string filename;
                    string type;
                    string status;
                    string frequency;
                    string daysOfWeek;
                    int delay = 2000;
                    bool switchoff = false;
                    string position;
                    if (list.Count > 0)
                    {

                        foreach (XmlElement elm in list)
                        {
                            if (Pause) continue;
                            filename = elm.GetAttribute("path").ToString();
                            filename = filename.Trim();
                            delay = Convert.ToInt32(elm.GetAttribute("duration"));
                            type = elm.GetAttribute("content-type").ToString();
                            status = elm.GetAttribute("status").ToString();
                            position = elm.GetAttribute("position");
                            frequency = elm.GetAttribute("frequency");
                            daysOfWeek = elm.GetAttribute("daysOfWeek");
                            string startString = elm.GetAttribute("start-time");
                            string endString = elm.GetAttribute("end-time");

                            if (Validation.ValidateDate(startString, endString, frequency, daysOfWeek) && status == "on")
                            {
                                Logger.LogToPlayer("In Date Check:StrtDate:" + startString + "- EndtDate:" + endString + "- CurDate:"
                                    + DateTime.Now.ToString("dd-MM-yyyy HH:mm:yyyy") + "- File:" + filename);

                                if ((type == "IMAGE-UPLOAD" || type == "IMAGE-TEMPLATE"))
                                {
                                    webBrowser.Dispatcher.Invoke(() => { webBrowser.Visibility = System.Windows.Visibility.Hidden; });
                                    player.Dispatcher.Invoke(() =>
                                    {
                                        player.Visibility = System.Windows.Visibility.Visible;
                                        player.Source = new Uri(filename);

                                    });
                                    displayWeather(position, weather);
                                    Thread.Sleep(delay);
                                    switchoff = true;
                                }
                                else if (type == "VIDEO" || type == "VIDEOURL")
                                {
                                    player.Dispatcher.Invoke(() =>
                                    {
                                        player.Visibility = System.Windows.Visibility.Visible;
                                        player.Source = new Uri(filename);

                                    });
                                    weather.Dispatcher.Invoke(() => { weather.Visibility = Visibility.Hidden; });
                                    webBrowser.Dispatcher.Invoke(() => { webBrowser.Visibility = System.Windows.Visibility.Hidden; });
                                    int milliSecs = delay;
                                    //player.Dispatcher.Invoke(() =>
                                    //{
                                    //    milliSecs = (int)player.NaturalDuration.TimeSpan.TotalMilliseconds;
                                    //});
                                    switchoff = true;
                                    Thread.Sleep(milliSecs);
                                }
                                else if (type == "WEBURL")
                                {
                                    //weather.Dispatcher.Invoke(() => { weather.Visibility = Visibility.Hidden; });
                                    player.Dispatcher.Invoke(() => player.Visibility = System.Windows.Visibility.Hidden);
                                    webBrowser.Dispatcher.Invoke(() =>
                                    {

                                        webBrowser.Source = new Uri(System.Net.WebUtility.UrlDecode(filename));
                                        webBrowser.Visibility = System.Windows.Visibility.Visible;
                                    });
                                    switchoff = true;
                                    System.Threading.Thread.Sleep(delay);
                                }
                            }
                        }
                        if (!switchoff)
                        {
                            Logger.LogToPlayer("No Files Scheduled");
                            player.Dispatcher.Invoke(() =>
                            {

                                player.Visibility = System.Windows.Visibility.Visible;
                                player.Source = new Uri(defaultImage);
                                webBrowser.Visibility = System.Windows.Visibility.Hidden;
                            });
                            Thread.Sleep(2000);
                        }
                    }
                    else
                    {
                        Logger.LogToPlayer("No Files in Config. Epmty XML");
                        player.Dispatcher.Invoke(() =>
                        {
                            player.Source = new Uri(defaultImage);
                            player.Visibility = System.Windows.Visibility.Visible;
                            webBrowser.Visibility = System.Windows.Visibility.Hidden;
                        });
                        weather.Dispatcher.Invoke(() =>
                        {
                            weather.Visibility = Visibility.Visible;
                            weather.HorizontalAlignment = HorizontalAlignment.Right;
                            weather.VerticalAlignment = VerticalAlignment.Bottom;
                        });
                        Thread.Sleep(2000);
                    }
                    // System.GC.Collect();
                }

            }

            catch (Exception ex)
            {

                Logger.LogToPlayer(ex.ToString());
            }
        }
        private void DisplayImage(string imagePath, bool isPlayer)
        {
            if (isPlayer)
                player.Dispatcher.Invoke(() =>
                {
                    player.Visibility = Visibility.Visible;
                    player.Source = new Uri(imagePath);
                });
            else
                webBrowser.Dispatcher.Invoke(() =>
                {
                    webBrowser.Visibility = Visibility.Visible;
                    webBrowser.Source = new Uri(imagePath);
                });
        }
        public static void CreateConfigXML()
        {
            string playerConfigPath = Path.GetFullPath(Enviornment.ConfigPath);


            XmlDocument doc = new XmlDocument();

            if (!File.Exists(playerConfigPath + "/config.xml"))
            {

                XmlElement xmlRoot = doc.CreateElement("DigitalSignageConfig");
                XmlElement xmlElem = doc.CreateElement("Display");
                xmlRoot.AppendChild(xmlElem);
                doc.AppendChild(xmlRoot);
                doc.Save(playerConfigPath + "/config.xml");
            }

        }
        private XmlDocument LoadXml()
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlPath);
                return doc;
            }
            catch (Exception e)
            {
                Logger.LogToPlayer(e.ToString());
                return null;
            }
        }
        private void displayWeather(string position, Grid weather)
        {
            if (position == null || position == "") weather.Dispatcher.Invoke(() => { weather.Visibility = Visibility.Hidden; });
            if (position == "TopRight") weather.Dispatcher.Invoke(() =>
            {
                weather.Visibility = Visibility.Visible;
                weather.HorizontalAlignment = HorizontalAlignment.Right;
                weather.VerticalAlignment = VerticalAlignment.Top;
            });
            if (position == "TopLeft") weather.Dispatcher.Invoke(() =>
            {
                weather.Visibility = Visibility.Visible;
                weather.HorizontalAlignment = HorizontalAlignment.Left;
                weather.VerticalAlignment = VerticalAlignment.Top;
            });
            if (position == "BottomRight") weather.Dispatcher.Invoke(() =>
            {
                weather.Visibility = Visibility.Visible;
                weather.HorizontalAlignment = HorizontalAlignment.Right;
                weather.VerticalAlignment = VerticalAlignment.Bottom;
            });
            if (position == "BottomLeft") weather.Dispatcher.Invoke(() =>
            {
                weather.Visibility = Visibility.Visible;
                weather.HorizontalAlignment = HorizontalAlignment.Left;
                weather.VerticalAlignment = VerticalAlignment.Bottom;
            });
        }
    }
}
