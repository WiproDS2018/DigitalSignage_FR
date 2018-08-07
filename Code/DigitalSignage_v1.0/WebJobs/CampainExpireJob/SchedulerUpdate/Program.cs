using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace ExpireScheduler
{
    class Program
    {


        static void Main(string[] args)
        {
            string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
            DateTime localDate = DateTime.Now;
            //string dt = localDate.ToShortDateString();
            string currentDate = localDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string Sqlqueries = "Select * from Campaign where published=1 and [Status]!='Expired' and  EndDate < '" + currentDate + "'";
            SqlConnection sqlConnection = new SqlConnection(connectionstring);

            try
            {
               
                sqlConnection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(Sqlqueries, sqlConnection))
                {
                    DataTable ScheduleTable = new DataTable();
                    adapter.Fill(ScheduleTable);
                    foreach (DataRow dataRow in ScheduleTable.Rows)
                    {
                        int campaignId = (int)dataRow["CampaignId"];
                        SqlCommand cmd = new SqlCommand("Update Campaign set Status = @Status where [CampaignId]=" + campaignId);
                        cmd.Connection = sqlConnection;
                        cmd.Parameters.AddWithValue("@Status", "Expired");
                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("Status Updated");
                }

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
            }

        }
        public static bool ValidateDate(string startString, string endString)
        {
            DateTime startTime = DateTime.ParseExact(startString, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime endTime = DateTime.ParseExact(endString, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime now = DateTime.Now;
            TimeSpan past = now - startTime;
            TimeSpan future = endTime - now;
            if (past.Seconds >= 0 && future.Seconds >= 0) return true;
            return false;
        }


    }
}