using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SignageFaceRecognition.Face
{
    /// <summary>
    /// Contains All the methods related to capture images
    /// </summary>
    class FaceRecognitionHandler
    {
        static FilterInfoCollection WebcamColl;
        static VideoCaptureDevice Device;
        private static Stack<string> pool = new Stack<string>();
        private static volatile bool takePhotos = true;

        private static Thread faceThread = new Thread(FaceRecognitionModule.RunFaceModule);
        private static Thread timer = new Thread(TimerThread);


        /// <summary>
        /// Delay Thread instructs the camara to save current frame every 2 seconds.
        /// </summary>
        /// <param name="obj">The object.</param>
        private static void TimerThread(object obj)
        {
            int delay = (int)obj;
            while (true)
            {
                takePhotos = true;
                Thread.Sleep(delay);
            }
        }
        /// <summary>
        /// Starts Camera
        /// </summary>
        public static void FaceThread()
        {
            Directory.CreateDirectory(@"C:/Signage/faces");
            string[] filePaths = Directory.GetFiles(@"C:/Signage/faces");
            StorageAccountHelper.CreateTable();

            foreach (string file in filePaths)
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (Exception ) { continue; }
            }
            WebcamColl = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //if you have connected with one more camera choose index as you want s
            if (WebcamColl.Count > 0)
            {
                Device = new VideoCaptureDevice(WebcamColl[0].MonikerString);
                //  Device.ProvideSnapshots = true;
                //deletion.Start();
                timer.Start(2000);
                Device.NewFrame += new NewFrameEventHandler(DeviceNewFrame);
                Device.Start();
                faceThread.Start();

                Logger.LogToFaceRecog("Started camera");
            }
            else
            {

            }
        }
        /// <summary>
        /// Save the current frame in camera if the timer thread instructs it to save the current image and add the image in FaceRecognitonModule stack.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="NewFrameEventArgs"/> instance containing the event data.</param>
        static void DeviceNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (!takePhotos) return;
            Image img = (Bitmap)eventArgs.Frame.Clone();
            string fileName = @"C:\Signage\faces\" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss__fff") + ".jpg";
            img.Save(fileName);
            takePhotos = false;
            FaceRecognitionModule.AddFaceRequest(fileName);
        }
    }
}
