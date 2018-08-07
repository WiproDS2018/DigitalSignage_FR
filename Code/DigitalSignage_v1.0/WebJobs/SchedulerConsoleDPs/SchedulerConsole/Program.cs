namespace SchedulerConsole
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            SendCloudToDevice.Init();
            ScheduleManager scheduler = new ScheduleManager();
            scheduler.execute();                     

        }
    }
}
