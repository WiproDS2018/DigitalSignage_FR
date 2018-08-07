using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Windows.Data.Xml;
using Windows.Data.Xml.Dom;
using IoTCoreDefaultApp.Utils;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;
using IoTCoreDefaultApp.Config;
using System.IO;
using Windows.UI.Xaml;
using System.Xml.Linq;
using IoTCoreDefaultApp.IoT;
using IoTCoreDefaultApp.Json;
using Newtonsoft.Json;
using IoTCoreDefaultApp.Message;

namespace IoTCoreDefaultApp.Player
{
    class SignagePlayer
    {
        Dictionary<string, BitmapImage> bitmapMap = new Dictionary<string, BitmapImage>();
        Image imagePlayer;
        MediaElement videoPlayer;
        WebView webBrowser;
        Image defaultImage;
        CoreDispatcher dispatcher;
        public SignagePlayer(Image defaultImage,Image imagePlayer,MediaElement videoPlayer,
            WebView webBrowser,CoreDispatcher dispatcher)
        {
            this.defaultImage = defaultImage;
            this.imagePlayer = imagePlayer;
            this.videoPlayer = videoPlayer;
            this.webBrowser = webBrowser;
            this.dispatcher = dispatcher;
        }
        public Task StartPlayer()
        {
            Log.Write("Started Player");
            return Task.Factory.StartNew(() => {
                Play();
            });

        }
        private void  Play()
        {
            //while (true)
            //{
            //    Log.Write("While loop");
            //    dispatcher.RunAsync(
            //        CoreDispatcherPriority.High,
            //          () =>
            //          {
            //              imagePlayer.Source = new BitmapImage(new Uri("http://digitalsignagestore.blob.core.windows.net/signageimagecontainer/Burger.png"));
            //          }).AsTask().Wait();
            //    Task.Delay(3000).Wait();
            //    dispatcher.RunAsync(
            //        CoreDispatcherPriority.High,
            //          () =>
            //          {
            //              imagePlayer.Source = new BitmapImage(new Uri("ms-appx:///Assets/welcome.jpg"));
            //          }).AsTask().Wait();
            //    Task.Delay(3000).Wait();

            //}
            Log.Write("Inside Play()");
            while (true)
            {
                //Log.Write("loading file");
                StorageFolder folder = ApplicationData.Current.LocalFolder.GetFolderAsync("Signage").AsTask().Result;
               StorageFile config = folder.GetFileAsync("config.json").AsTask().Result;
                string text = FileIO.ReadTextAsync(config).AsTask().Result;
                //Log.Write("file contents");
                //Log.Write(text);

                JsonPlaylist currentPlaylist = JsonConvert.DeserializeObject<JsonPlaylist>(text);
                //Log.Write("Found in config");

                //currentPlaylist.playlist.ForEach((mes) => Log.Write(mes.ContentUrl));

                bool contentAvailable = false;
                if (currentPlaylist.playlist.Count == 0)
                {
                    dispatcher.RunAsync(
                               CoreDispatcherPriority.Low,
                               () =>
                               {
                                   imagePlayer.Visibility = Visibility.Collapsed;
                                   defaultImage.Visibility = Visibility.Visible;
                                   webBrowser.Visibility = Visibility.Collapsed;
                                   videoPlayer.Visibility = Visibility.Collapsed;
                               }).AsTask().Wait();
                    Task.Delay(3000).Wait();
                    continue;
                }
                foreach(CloudMessage message in currentPlaylist.playlist)
                {
                    message.ContentUrl = message.ContentUrl.Trim();
                    if (message.show == "True" && Validations.ValidateDate(message.Start, message.End, message.Frequency, message.DaysOfWeek))
                    {
                        contentAvailable = true;
                        if (message.ContentType.Contains("IMAGE"))
                        {
                            dispatcher.RunAsync(
                             CoreDispatcherPriority.Normal,
                             () =>
                             {
                                 defaultImage.Visibility = Visibility.Collapsed;
                                 imagePlayer.Source = GetBitmapImage(message.ContentUrl);


                                 imagePlayer.Visibility = Visibility.Visible;
                                 webBrowser.Visibility = Visibility.Collapsed;

                                 videoPlayer.Visibility = Visibility.Collapsed;
                             }).AsTask().Wait();
                        }
                        else if (message.ContentType.Contains("VIDEO"))
                        {
                            dispatcher.RunAsync(
                             CoreDispatcherPriority.Normal,
                             () =>
                             {
                                 imagePlayer.Visibility = Visibility.Collapsed;
                                 defaultImage.Visibility = Visibility.Collapsed;
                                 webBrowser.Visibility = Visibility.Collapsed;
                                 videoPlayer.Visibility = Visibility.Visible;
                                 videoPlayer.Source = new Uri(Path.Combine(Config.Environment.ImagesFolder.Path, message.ContentUrl));
                             }).AsTask().Wait();
                        }
                        else if(message.ContentType.Contains("WEB"))
                        {
                            dispatcher.RunAsync(
                             CoreDispatcherPriority.Normal,
                             () =>
                             {
                                 defaultImage.Visibility = Visibility.Collapsed;
                                 imagePlayer.Visibility = Visibility.Collapsed;
                                 webBrowser.Visibility = Visibility.Visible;
                                 webBrowser.Source = new Uri(message.ContentUrl);
                                 videoPlayer.Visibility = Visibility.Collapsed;
                             }).AsTask().Wait();
                        }
                        Task.Delay(int.Parse(message.Duration)).Wait(); ;
                    }
                }
                if (!contentAvailable)
                {
                    dispatcher.RunAsync(
                                CoreDispatcherPriority.Normal,
                                () =>
                                {
                                    imagePlayer.Visibility = Visibility.Collapsed;
                                    defaultImage.Visibility = Visibility.Visible;
                                    webBrowser.Visibility = Visibility.Collapsed;
                                    videoPlayer.Visibility = Visibility.Collapsed;
                                }).AsTask().Wait();
                    Task.Delay(3000).Wait(); ;
                }

            }
        }
   
        private BitmapImage GetBitmapImage(string url)
        {
            int index = url.LastIndexOf('/');
            string fileName = url.Substring(index + 1).Trim();
            if (bitmapMap.ContainsKey(fileName)) return bitmapMap[fileName];

            var bitmap= new BitmapImage(new Uri(Path.Combine(Config.Environment.ImagesFolder.Path, fileName)));
            bitmapMap.Add(fileName, bitmap);
            return bitmap;
        }
        
    }
}
