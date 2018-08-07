using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace SchedulerConsole
{
    /// <summary>
    /// Building Intelligent Signage : From Embedded to Cloud -IOT
    /// This Class is responsible for Sending Cloud to device Messages and Receive Feedback from device.  
    /// </summary>

    class SendCloudToDevice
    {
        static ServiceClient serviceClient;
        //static string connectionString = "HostName=DigitalSignage.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=L82B+OVXh66Zp+Adg6uzq7OrXROc8GdXbHTBKAGRUPA=";
        static string connectionString = ConfigurationManager.ConnectionStrings["hubconnection"].ConnectionString;
        public static void Init()
        {           
            Console.WriteLine("Send Cloud-to-Device messages");
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
        }

        public async static Task SendCloudToDeviceMessageAsync(Schedule Job, bool isShow)
        {
            try {

                Console.WriteLine("Sending message to {0} with SId {1}", Job.Deviceid, Job.TrackerId);

                var telemetryDataPoint = new
                {
                    sid = Job.TrackerId,
                    ContentUrl = Job.content,
                    Start = Job.StartTime,
                    End = Job.EndTime,
                    Duration = Job.Duration,
                    ContentType=Job.ContentType,
                    show = isShow.ToString(),
                    TemplateType = Job.TemplateType,
                    IconPosition = Job.IconPosition,
                    Frequency = Job.Frequency,
                    DaysOfWeek= Job.DaysofWeek
                    

                };

                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var commandMessage = new Message(Encoding.ASCII.GetBytes(messageString));
                commandMessage.Ack = DeliveryAcknowledgement.Full;
                commandMessage.MessageId = Job.TrackerId.ToString();
                await serviceClient.SendAsync(Job.Deviceid, commandMessage);
                ReceiveFeedbackAsync(isShow);
            }
            catch (Exception ex)
            {
                LogHelper.WriteDebugLog(ex.ToString());
            }
         }
        public async static void ReceiveFeedbackAsync(bool isShow)
        {
            try
            {
                var feedbackReceiver = serviceClient.GetFeedbackReceiver();

                Console.WriteLine("Receiving feedback from service");

                while (true)
                {
                    var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                    if (feedbackBatch == null) continue;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    string feedback = string.Join(" ", feedbackBatch.Records.Select(f => f.OriginalMessageId));
                    if (isShow)
                        Console.WriteLine("Received feedback: {0}", feedback);
                    else
                        Console.WriteLine("Received Canceled feedback: {0}", feedback);
                    Console.ResetColor();
                    int size = feedback.Length, i;
                    //Console.WriteLine(size);
                    string feedid = "";
                    int fid;
                    for (i = 0; i < size; i++)
                    {
                        //Console.WriteLine(feedid);
                        if (feedback[i] == ' ')
                        {
                            fid = Int32.Parse(feedid);
                            //Console.WriteLine("Enqueuing {0}", fid);
                            if (isShow)
                                ScheduleManager.feedq.Enqueue(fid);
                            else
                                ScheduleManager.cancelfeedq.Enqueue(fid);
                            //sqlqueries.UpdateStatus(fid, 2, connection);
                            feedid = "";
                        }
                        else
                        {
                            feedid += feedback[i];
                        }
                    }
                    fid = Int32.Parse(feedid);
                    //Console.WriteLine("Enqueuing {0}", fid);
                    if (isShow)
                        ScheduleManager.feedq.Enqueue(fid);
                    else
                        ScheduleManager.cancelfeedq.Enqueue(fid);
                    await feedbackReceiver.CompleteAsync(feedbackBatch);
                }
            }
            catch(Exception ex)
            {
                LogHelper.WriteDebugLog(ex.ToString());
            }
        }
    }
}
