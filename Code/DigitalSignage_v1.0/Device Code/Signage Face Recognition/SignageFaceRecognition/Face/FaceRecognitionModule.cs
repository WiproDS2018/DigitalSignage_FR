using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignageFaceRecognition.Face
{
    /// <summary>
    /// contains all methods related to facial recognition
    /// </summary>
    class FaceRecognitionModule
    {
        private static string faceApiKey = ConfigurationManager.AppSettings["FaceApiKey"];
        private static string faceApiUrl = ConfigurationManager.AppSettings["FaceApiUrl"];
        private static readonly IFaceServiceClient faceServiceClient =
           new FaceServiceClient(faceApiKey, faceApiUrl);

        private static Stack<string> pool = new Stack<string>();

        /// <summary>
        /// Adds the image to image pool.
        /// </summary>
        /// <param name="image">The image.</param>
        public static void AddFaceRequest(string image)
        {
            pool.Push(image);
        }

        /// <summary>
        /// Runs the face module.
        /// Main method of Face Recognition API fetches the latest image from the image pool sends it to face api, gets images from database updates ui and then again fetches latest image from image pool.
        /// </summary>
        public static void RunFaceModule()
        {

            while (true)
            {
                try
                {
                    if (pool.Count == 0)
                        continue;
                    string imagePath = pool.Pop();

                    //pool.Clear();
                    /*  while (pool.Count != 0)
                      {
                          try
                          {
                              File.Delete(pool.Pop());
                          }
                          catch (Exception e) { break; }
                      }*/
                    //  Console.WriteLine("Added to face recognition " + imagePath);
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    Microsoft.ProjectOxford.Face.Contract.Face[] faces = RunFaceDetection(imagePath).Result;
                    File.Delete(imagePath);
                    DateTime end = DateTime.Now;
                    stopwatch.Stop();
                    //Console.WriteLine("Time Taken for Face API for image " + imagePath + " :" + stopwatch.ElapsedMilliseconds);
                    Logger.LogToFaceRecog("Time Taken for Face API for image " + imagePath + " :" + stopwatch.ElapsedMilliseconds);
                    //LogWriter.AddToLog("Time Taken for Face API for image " + imagePath + " :" + (end - b).Milliseconds + "Completed at " + end);
                    if (faces.Length == 0)
                    {
                        SignageController.ProcessRequest("undefined", "undefined");
                        var newLine = string.Format("{0},{1},{2}\n", imagePath, "undefined", "undefined");
                        Logger.LogToFaceRecog(newLine);
                        File.AppendAllText("C:/Signage/Logs/results.csv", newLine);
                    }
                    else
                    {
                        SignageController.ProcessRequest(faces[0].FaceAttributes.Age.ToString(), faces[0].FaceAttributes.Gender);
                        var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}\n", imagePath,
                            faces[0].FaceAttributes.Emotion.Anger, faces[0].FaceAttributes.Emotion.Sadness, faces[0].FaceAttributes.Emotion.Disgust,
                       faces[0].FaceAttributes.Emotion.Fear, faces[0].FaceAttributes.Emotion.Happiness, faces[0].FaceAttributes.Emotion.Neutral, faces[0].FaceAttributes.Emotion.Contempt, faces[0].FaceAttributes.Emotion.Surprise);
                        //    Logger.LogToFaceRecog(newLine);
                        StorageAccountHelper.AddToTable(faces[0]);
                        File.AppendAllText("C:/Signage/Logs/results.csv", newLine);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }
            }

        }
        /// <summary>
        /// Runs the face detection for given image Path
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <returns></returns>
        public static async Task<Microsoft.ProjectOxford.Face.Contract.Face[]> RunFaceDetection(string imagePath)
        {
            DateTime b = DateTime.Now;

            Microsoft.ProjectOxford.Face.Contract.Face[] faces;
            IEnumerable<FaceAttributeType> faceAttributes =
           new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Emotion, FaceAttributeType.Glasses, FaceAttributeType.Hair };

            // Call the Face API.
            try
            {
                using (FileStream imageStream = new FileStream(imagePath, FileMode.Open))
                {
                    Console.WriteLine("Waiting for Face Api..........");
                    //LogWriter.AddToLog("Waiting for Face Api..........");
                    //LogHelper.WriteDebugLog("Waiting For Face Api");
                    faces = await faceServiceClient.DetectAsync(imageStream, returnFaceId: true, returnFaceLandmarks: false, returnFaceAttributes: faceAttributes);
                    imageStream.Dispose();
                    imageStream.Close();

                }


            }
            // Catch and display Face API errors.
            catch (FaceAPIException f)
            {
                Console.WriteLine(f.ErrorMessage + " " + f.ErrorCode);
                faces = new Microsoft.ProjectOxford.Face.Contract.Face[0];
            }
            // Catch and display all other errors.
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                faces = new Microsoft.ProjectOxford.Face.Contract.Face[0];
            }

            return faces;
        }

        private static async Task<Microsoft.ProjectOxford.Face.Contract.Face[]> UploadAndDetectFaces(Stream imageStream)
        {
            // The list of Face attributes to return.
            IEnumerable<FaceAttributeType> faceAttributes =
                new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Emotion, FaceAttributeType.Glasses, FaceAttributeType.Hair };

            // Call the Face API.
            try
            {
                using (imageStream)
                {
                    Microsoft.ProjectOxford.Face.Contract.Face[] faces = await faceServiceClient.DetectAsync(imageStream, returnFaceId: true, returnFaceLandmarks: false, returnFaceAttributes: faceAttributes);
                    imageStream.Dispose();
                    imageStream.Close();
                    return faces;
                }


            }
            // Catch and display Face API errors.
            catch (FaceAPIException f)
            {
                Console.WriteLine(f.ErrorMessage + " " + f.ErrorCode);
                return new Microsoft.ProjectOxford.Face.Contract.Face[0];
            }
            // Catch and display all other errors.
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Microsoft.ProjectOxford.Face.Contract.Face[0];
            }
        }

        private static string FaceDescription(Microsoft.ProjectOxford.Face.Contract.Face face)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Face: ");

            // Add the gender, age, and smile.
            sb.Append(face.FaceAttributes.Gender);
            sb.Append(", ");
            sb.Append(face.FaceAttributes.Age);
            sb.Append(", ");
            sb.Append(String.Format("smile {0:F1}%, ", face.FaceAttributes.Smile * 100));

            // Add the emotions. Display all emotions over 10%.
            sb.Append("Emotion: ");
            EmotionScores emotionScores = face.FaceAttributes.Emotion;
            sb.Append(String.Format("anger {0:F1}%, ", emotionScores.Anger * 100));
            sb.Append(String.Format("contempt {0:F1}%, ", emotionScores.Contempt * 100));
            sb.Append(String.Format("disgust {0:F1}%, ", emotionScores.Disgust * 100));
            sb.Append(String.Format("fear {0:F1}%, ", emotionScores.Fear * 100));
            sb.Append(String.Format("happiness {0:F1}%, ", emotionScores.Happiness * 100));
            sb.Append(String.Format("neutral {0:F1}%, ", emotionScores.Neutral * 100));
            sb.Append(String.Format("sadness {0:F1}%, ", emotionScores.Sadness * 100));
            sb.Append(String.Format("surprise {0:F1}%, ", emotionScores.Surprise * 100));

            // Add glasses.
            sb.Append(face.FaceAttributes.Glasses);
            sb.Append(", ");

            // Add hair.
            sb.Append("Hair: ");

            // Display baldness confidence if over 1%.
            if (face.FaceAttributes.Hair.Bald >= 0.01f)
                sb.Append(String.Format("bald {0:F1}% ", face.FaceAttributes.Hair.Bald * 100));

            // Display all hair color attributes over 10%.
            HairColor[] hairColors = face.FaceAttributes.Hair.HairColor;
            foreach (HairColor hairColor in hairColors)
            {
                if (hairColor.Confidence >= 0.1f)
                {
                    sb.Append(hairColor.Color.ToString());
                    sb.Append(String.Format(" {0:F1}% ", hairColor.Confidence * 100));
                }
            }

            // Return the built string.
            //Console.WriteLine(sb);
            return sb.ToString();
        }
    }
}
