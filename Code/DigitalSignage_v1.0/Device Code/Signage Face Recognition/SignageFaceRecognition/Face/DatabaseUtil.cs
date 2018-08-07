using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SignageFaceRecognition.Face
{
    /// <summary>
    /// Utility class for all database related operations
    /// </summary>
    public class DatabaseUtil
    {
        private static
            string connString = ConfigurationManager.AppSettings["DatabaseConnectionString"];
        private static OdbcConnection con = new OdbcConnection(connString);

        /// <summary>
        /// Opens the data base connection.
        /// </summary>
        public static void OpenDataBaseConnection()
        {
            try
            {
                con.Open();
                Console.WriteLine("Database Connection established");
               // LogHelper.WriteDebugLog("Database Connection established");
            }
            catch (Exception )
            {
                Console.WriteLine("Failed To Establish DataBase Connection");
             //   LogHelper.WriteDebugLog("Failed To Establish Database Connection");
                //Console.WriteLine(e.StackTrace);
                //LogWriter.AddToLog("Failed To Establish DataBase Connection");
            }
        }
        /// <summary>
        /// Closes the database connection.
        /// </summary>
        public static void CloseDatabaseConnection()
        {
            try
            {
                con.Close();
              //  LogHelper.WriteDebugLog("Database Connection Closed");
            }
            catch (Exception )
            {
                Console.WriteLine("Failed To Close DataBase Connection");
            }
        }
        /// <summary>
        /// Downloads all images for FaceRecogntion
        /// </summary>
        public static void DownloadAllImages()
        {
            WebClient webClient = new WebClient();
            int count = 0;
            string query = "select * from dbo.FaceRecogSignage ;";
            OdbcCommand command = new OdbcCommand(query, con);

            try
            {
                OdbcDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string httpPath = reader[4].ToString().Replace("https", "http");
                    int pos = httpPath.LastIndexOf('/');
                    Directory.CreateDirectory(@"C:/Signage/Images");
                    string requestedFile = @"C:/Signage/Images/" + httpPath.Substring(pos + 1);
                    if (!File.Exists(requestedFile)) webClient.DownloadFile(httpPath, requestedFile);
                    count++;
                }
                reader.Close();
                DateTime end = DateTime.Now;
                //Console.WriteLine("Time Taken for database " + (end - b).Milliseconds);
                //LogWriter.AddToLog("Time Taken for database " + (end - b).Milliseconds);
                Console.WriteLine(count + " files downloaded");
             //   LogHelper.WriteDebugLog(count + " files downloaded");
            }
            catch (Exception )
            {
                Console.WriteLine("Failed To Fetch Data From DataBase");
              //  LogHelper.WriteDebugLog("Failed To Fetch Data From DataBase");
            }

        }
        /// <summary>
        /// Gets the signage urls for particular age and gender
        /// </summary>
        /// <param name="age">The age of Person.</param>
        /// <param name="gender">The gender of Person.</param>
        /// <returns>List of images to be showed for person</returns>
        public static List<string> GetSignageUrls(string age, string gender)
        {
            DateTime b = DateTime.Now;
            List<string> urls = new List<string>();
            Console.WriteLine("Waiting for Database....");
            string query = "select * from dbo.FaceRecogSignage where AgeLowerLimit<=" + age + " and AgeUpperLimit>=" + age + " and Gender='" + gender + "' and IsActive=1;";
            OdbcCommand command = new OdbcCommand(query, con);

            try
            {
                OdbcDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string httpsPath = reader[4].ToString();
                    string path = httpsPath.Replace("https", "http");
                    urls.Add(path);
                }
                reader.Close();
                DateTime end = DateTime.Now;
                //Console.WriteLine("Time Taken for database " + (end - b).Milliseconds);
                //LogWriter.AddToLog("Time Taken for database " + (end - b).Milliseconds);
                return urls;
            }
            catch (Exception )
            {
                Console.WriteLine("Failed To Fetch Data From DataBase");
               // LogHelper.WriteDebugLog("Failed To Fetch Data From DataBase");
                return urls;
            }

        }

    }
}
