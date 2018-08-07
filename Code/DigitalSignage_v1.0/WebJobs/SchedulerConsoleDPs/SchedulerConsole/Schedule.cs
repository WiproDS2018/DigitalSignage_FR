using System;
using System.Data.SqlClient;
namespace SchedulerConsole
{
    /// <summary>
    /// Building Intelligent Signage : From Embedded to Cloud -IOT
    /// This Class is responsible for Schedule and sending published parameter from Cloud to Device Via. IOT HUB
    /// </summary>
    class Schedule
    {
        public int TrackerId { get; set; }
        public String Deviceid { get; set; }
        public String PlayerSerialNo { get; set; }
        public String content { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public String Duration { get; set; }
        public String ContentType { get; set; }
        public String TemplateType { get; set; }
        public String IconPosition { get; set; }
        public String Frequency { get; set; }
        public String DaysofWeek { get; set; }
        public void DoJob(SqlConnection conn)
        {
            try
            {
                Console.WriteLine(string.Format("The Job \"{0}\" was started. ID : \"{1}\" Player : \"{2}\" Start time: {3} Current time: {4}", content, Deviceid, PlayerSerialNo, StartTime, DateTime.Now));
                SendCloudToDevice.ReceiveFeedbackAsync(true);
                SendCloudToDevice.SendCloudToDeviceMessageAsync(GetCurrentJob(), true).Wait();        //Message Sent
                Console.WriteLine("Message sent to IOT hub for Device {0} ", Deviceid);
            }
            catch(Exception ex)
            {
                LogHelper.WriteDebugLog(ex.ToString());
            }
        }
        /// <summary>
        /// Send Cancel Message to Hub for Publish cancelled.
        /// </summary>
        /// <param name="conn"></param>
        public void CancelJob(SqlConnection conn)
        {
            try
            {  
                System.Console.WriteLine(string.Format("The Job \"{0}\" was canceled. ID : \"{1}\" Player : \"{2}\" Start time: {3} Current time: {4}", content, Deviceid, PlayerSerialNo, StartTime, DateTime.Now));
                SendCloudToDevice.ReceiveFeedbackAsync(false);
                SendCloudToDevice.SendCloudToDeviceMessageAsync(GetCurrentJob(), false).Wait();
                Console.WriteLine("Cancel Message sent to IOT hub for Device {0} ", Deviceid);
            }
            catch(Exception ex)
            {
                LogHelper.WriteDebugLog(ex.ToString());
            }
        }
        public Schedule()
        {
        }
        /// <summary>
        /// Create  object for current Job-Schedule 
        /// </summary>
        /// <returns></returns>
        private Schedule GetCurrentJob()
        {
            Schedule job = new Schedule();
            job.TrackerId = TrackerId;
            job.Deviceid = Deviceid;
            job.PlayerSerialNo = PlayerSerialNo;
            job.content = content;
            job.StartTime = StartTime;
            job.EndTime = EndTime;
            job.Duration = Duration;
            job.ContentType = ContentType;
            job.TemplateType = TemplateType;
            job.IconPosition = IconPosition;
            job.Frequency = Frequency;
            job.DaysofWeek = DaysofWeek;

            return job;
        }

        public Schedule(string DId, string player, string url, string start, string end, string duration, string contentType, string templateType, string iconPosition, int ID, string frequency, string daysofWeek)
        {
            TrackerId = ID;
            Deviceid = DId;
            PlayerSerialNo = player;
            content = url;
            StartTime = start;
            EndTime = end;
            Duration = duration;
            ContentType = contentType;
            TemplateType = templateType;
            IconPosition = iconPosition;
            Frequency = frequency;
            DaysofWeek = daysofWeek;
            
        }
    }
}
