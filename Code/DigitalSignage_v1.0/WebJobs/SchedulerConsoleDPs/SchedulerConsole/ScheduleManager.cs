using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Globalization;

namespace SchedulerConsole
{
    /// <summary>
    /// Building Intelligent Signage : From Embedded to Cloud -IOT
    /// This Class is responsible for Schedule Management: Logical Part  for Updating status ,sending messages  
    /// </summary>

    class ScheduleManager
    {
        public static List<Thread> ThreadPool = new List<Thread>();
        int count = 0;
        static int timer = 5000;
        SqlConnection connection;
        public static Queue<int> feedq = new Queue<int>();
        public static Queue<int> cancelfeedq = new Queue<int>();
        public void DatabaseConnection()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SchedulerConsole.Properties.Settings.SchedulerConnectionString"].ConnectionString;
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Database Connection Unsuccessfull");
                Console.WriteLine(ex);
                LogHelper.WriteDebugLog(ex.ToString());
            }
        }

        public void execute()
        {
            try
            {
                DatabaseConnection();
                while (true)
                {

                    using (SqlDataAdapter adapter = new SqlDataAdapter(Sqlqueries.selectAll, connection))
                    {
                        DataTable ScheduleTable = new DataTable();
                        adapter.Fill(ScheduleTable);
                        DateTime localDate = DateTime.Now;
                        string date = localDate.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        Console.WriteLine(date);

                        foreach (DataRow dataRow in ScheduleTable.Rows)
                        {
                            int Tid = (int)dataRow["TrackerId"];
                            string DId = dataRow["PlayerSerialNo"].ToString();//should be nvarchar
                            string player = dataRow["PlayerSerialNo"].ToString();//should be nvarchar
                            string url = dataRow["ContentUrl"].ToString();
                            string status = dataRow["Status"].ToString();//change nvarchar to int
                            bool published = (bool)dataRow["Published"];//change nvarchar to int

                            DateTime s = Convert.ToDateTime(dataRow["StartDate"]);
                            var startdate = s.Date.ToString("dd-MM-yyyy ", CultureInfo.InvariantCulture);
                            string starttime = dataRow["StartTime"].ToString();
                            string start = startdate + starttime;               //assumed to be start

                            DateTime e = Convert.ToDateTime(dataRow["EndDate"]);
                            var enddate = e.Date.ToString("dd-MM-yyyy ", CultureInfo.InvariantCulture);
                            string endtime = dataRow["EndTime"].ToString();
                            string end = enddate + endtime;

                            string duration = dataRow["interval"].ToString();
                            string contentType = dataRow["ContentType"].ToString();
                            string templateType = dataRow["TemplateType"].ToString();
                            string iconPosition = dataRow["IconPosition"].ToString();
                            string frequency = dataRow["Frequency"].ToString();
                            string daysofWeek = dataRow["DaysOfWeek"].ToString();

                            string zone = dataRow["Zone"].ToString();

                            string action = "";

                            //Console.WriteLine("got data {0}",ID);
                            if (status == "NotStarted" && (published))
                            {
                                action = "Started Publishing: " + DateTime.Now;

                                Schedule job = new Schedule(DId, player, url, start, end, duration, contentType, templateType, iconPosition, Tid, frequency, daysofWeek);
                                //Console.WriteLine("data Node created {0}", ID);
                                int c1 = date.CompareTo(start);
                                int c2 = date.CompareTo(end);
                                bool valid=ValidateDate(start, end);

                                //Console.WriteLine("got time {0} {1}", start ,end , );
                                if (valid)
                                {
                                    //Console.WriteLine("thread created {0}", ID);
                                    Sqlqueries.UpdateStatus(Tid, "Started", action, connection);

                                    Thread thread = new Thread(() => job.DoJob(connection));
                                    ThreadPool.Add(thread);
                                    ThreadPool[count].Start();
                                    count++;
                                }

                            }
                            else if (status == "NotStarted" && (!published))
                            //if (status.CompareTo("-2") == 0)
                            {
                                action = "Started Canceling: " + DateTime.Now;

                                Sqlqueries.UpdateStatus(Tid, "Started", action, connection);
                                Console.WriteLine("The message is being sent to Device {0} to cancel content {1}", DId, url);
                                Schedule jobc = new Schedule(DId, player, url, start, end, duration, contentType, templateType, iconPosition, Tid, frequency, daysofWeek);
                                Thread thread = new Thread(() => jobc.CancelJob(connection));
                                ThreadPool.Add(thread);
                                ThreadPool[count].Start();
                                count++;
                            }


                        }
                    }

                    int feeds_count = feedq.Count;
                    //Console.WriteLine("Feed Count {0}", feeds_count);
                    while (feeds_count > 0)
                    {
                        string action = "";
                        action = "Published: " + DateTime.Now;
                        int deq = (int)feedq.Dequeue();
                        Console.WriteLine("Updating the status of schedule {0}", deq);
                        Sqlqueries.UpdateStatus(deq, "Published", action, connection);
                        feeds_count--;
                    }
                    int cancel_feeds_count = cancelfeedq.Count;
                    while (cancel_feeds_count > 0)
                    {
                        string action = "";
                        action = "Cancelled: " + DateTime.Now;
                        int deq = (int)cancelfeedq.Dequeue();
                        Console.WriteLine("Updating the status of schedule {0} as canceled and feedback received", deq);
                        Sqlqueries.UpdateStatus(deq, "Cancelled", action, connection);
                        cancel_feeds_count--;
                    }
                    Thread.Sleep(timer);
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteDebugLog(ex.ToString());
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
