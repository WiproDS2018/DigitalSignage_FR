using System;
using System.Data.SqlClient;
using System.Configuration;

namespace SchedulerConsole
{
    /// <summary>
    /// Building Intelligent Signage : From Embedded to Cloud -IOT
    /// This Class is responsible for Updating Scheduled  job Status  
    /// </summary>
    
    class Sqlqueries
    {
        public static string selectAll = ConfigurationManager.AppSettings["selectall"];
        /// <summary>
        /// Update status for Published /Cancelled  JOB in Signage DB  
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="status"></param>
        /// <param name="connection"></param>
        public static void UpdateStatus(int ID, string status, string action,SqlConnection connection)
        {
            try
            {
                using (SqlCommand sqlcmd = new SqlCommand(ConfigurationManager.AppSettings["UpdateStatus"], connection))
                {
                    sqlcmd.Parameters.AddWithValue("@NewS", status);
                    sqlcmd.Parameters.AddWithValue("@Actiontime", DateTime.Now);
                    sqlcmd.Parameters.AddWithValue("@Action", action);
                    sqlcmd.Parameters.AddWithValue("@Sid", ID);
                    int rows = sqlcmd.ExecuteNonQuery();
                    //rows number of record got updated
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                LogHelper.WriteDebugLog(ex.ToString());
            }

        }
    }
}
