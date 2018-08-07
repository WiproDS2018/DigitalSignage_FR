using Microsoft.ProjectOxford.Common.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SignageFaceRecognition.Face
{
    /// <summary>
    /// 
    /// </summary>
    class SignageController
    {
        private static WebClient webClienet = new WebClient();
        private static HashSet<string> imagePool = new HashSet<string>();
        private static SignageRequest previousRequest;
        private static DateTime startTime;
        public static MediaElement FacePlayer;
        public static MediaElement SignagePlayer;
        public static MainWindow mainWindow;
        public static void ProcessRequest(string age, string gender)
        {
            Logger.LogToFaceRecog("Request came for " + age + " : " + gender);
           // LogHelper.WriteDebugLog("Request came for " + age + " : " + gender);
            SignageRequest currentRequest = new SignageRequest(age, gender);
            if (currentRequest.Equals(previousRequest))
            {
                previousRequest = currentRequest;

                return;
            }
            else
            {

                EndPreviousRequest();
                previousRequest = currentRequest;

                if (age == "undefined" && gender == "undefined")
                {
                    mainWindow.Dispatcher.Invoke(() =>
                    {
                        FacePlayer.Visibility = System.Windows.Visibility.Hidden;
                        SignagePlayer.Visibility = System.Windows.Visibility.Visible;
                    });
                    //Console.WriteLine("Processing  ......     " + age + " : " + gender);
                    //LogHelper.WriteDebugLog("Processing........." + age + " : " + gender);
                    //   LogWriter.AddToLog("Processing  ......     " + age + " : " + gender);
                }
                else
                {
                    startTime = DateTime.Now;


                    List<string> urls = currentRequest.ProcessRequest();

                    Task.Factory.StartNew(() => {
                        Logger.LogToFaceRecog("Entered New Task");
                    for (int i = 0; i < urls.Count; i++)
                    {
                        string url = urls[i];
                        int pos = url.LastIndexOf('/');
                        string requestedFile = url.Substring(pos + 1);
                        if (!File.Exists(@"C:\Signage\Images\" + requestedFile))
                        {
                            imagePool.Add(requestedFile);
                            requestedFile = @"C:\Signage\Images\" + requestedFile;
                                
                            webClienet.DownloadFile(url, requestedFile);
                        }
                        else
                        {
                            requestedFile = @"C:\Signage\Images\" + requestedFile;
                        }
                            mainWindow.Dispatcher.Invoke(() =>
                        {
                            Logger.LogToFaceRecog("Invoked Dispatcher");
                            FacePlayer.Source = new Uri(requestedFile);
                            SignagePlayer.Visibility = System.Windows.Visibility.Hidden;
                            FacePlayer.Visibility = System.Windows.Visibility.Visible;
                        });
                            Logger.LogToFaceRecog("Playing : " + requestedFile);
                            Task.Delay(3000).Wait();
                        }
                    });

                }
                
            }
        }
        public static void EndPreviousRequest()
        {
            FacePlayer.Dispatcher.Invoke(() =>
            {
                FacePlayer.Visibility = System.Windows.Visibility.Hidden;
                SignagePlayer.Visibility = System.Windows.Visibility.Visible;
            });
        }

    }

}
